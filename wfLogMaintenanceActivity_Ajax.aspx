<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogMaintenanceActivity_Ajax.aspx.vb"
    Inherits="Flypal.wfLogMaintenanceActivity_Ajax" %>

<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> --%>
<html>
<head runat="server">
    <title>Maintenance Activity Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js" />
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="vs_showGrid" content="True">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" rel="stylesheet" type="text/css">
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
    <style type="text/css">
        .clsCursorStyle {
            cursor: pointer;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <asp:UpdatePanel ID="upnlMaint" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table id="tblmain" class="clstablelistout" border="0">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlMain" runat="server">
                                    <table id="tblinner" class="clstablelistin" border="0" cellpadding="0">
                                        <tr>
                                            <td class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblTitle"  runat="server" CssClass="clstitle1">Flight Maintenance Activity</asp:Label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td align="right">
                                                            <table>
                                                                <tr>
                                                                    <td align="right">
                                                                        <%--   <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>--%>

                                                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add Maintenance Activity"
                                                                            ValidationGroup="a"></asp:Button>

                                                                        <%--  </ContentTemplate>
                                                                        </asp:UpdatePanel>--%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button Style="z-index: 0" ID="btnSave" TabIndex="0" runat="server" Text="Save"
                                                                            ValidationGroup="a" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Maintenance"
                                                                            Visible="False"></asp:Button>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" CausesValidation="False" Text="Back"
                                                                            CssClass="clsbtnH clsinfoH" ToolTip="Click to go Previous page"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table id="Table2" border="0" style="display: none;">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnLogDetails" runat="server" CausesValidation="False" Text="Log details"
                                                                CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnFuelOil" runat="server" CausesValidation="False" Text="Fuel Oil"
                                                                CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDefectActionList" runat="server" CausesValidation="False" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Defect Reporting","Snag Reporting") %>'
                                                                CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button Style="z-index: 0" ID="btnParameterList" runat="server" CausesValidation="False"
                                                                Text="Parameter List" CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnLogPax" runat="server" CausesValidation="False" Text="Passenger Log"
                                                                Visible='<%# iif(AppSettings("ShowExtraLogTabs") = "True",True,False) %>' CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button Style="z-index: 0" ID="btnHobbsOffset" runat="server" CausesValidation="False"
                                                                Visible='<%#IIf(AppSettings("ShowExtraLogTabs") = "True", True, False) %>' Text="Hobbs Offset"
                                                                CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnFlightCrew" runat="server" CausesValidation="False" Text="Flight Crew"
                                                                CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMaintenanceActivityDetails" runat="server" CssClass="clsLabelButton"
                                                                Width="150px" ToolTip="Maintenance Activity Details">Maintenance Activity</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                            ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                        <asp:CustomValidator Style="z-index: 0" ID="cvMainActivityList" runat="server" CssClass="clsValidationSummary"
                                                            ValidationGroup="a" ControlToValidate="txtMainActivity" Display="None" OnServerValidate="customvalidate">
                                                        </asp:CustomValidator>
                                                        <%--<asp:RequiredFieldValidator ID="rfvDescription" runat="server" CssClass="clsValidationSummary"
                                                ErrorMessage="Description required" ControlToValidate="txtMainActivity" Display="None">
                                            </asp:RequiredFieldValidator>--%>
                                                        <asp:CustomValidator Style="z-index: 0" ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"
                                                            ValidationGroup="a" OnServerValidate="CustomValidate2" Display="None" ControlToValidate="txtMainActivity">
                                                        </asp:CustomValidator>
                                                        <asp:CustomValidator Style="z-index: 0" ID="cvDoneBy" runat="server" CssClass="clsValidationSummary"
                                                            ValidationGroup="a" ControlToValidate="txtLicenceNo" Display="None" OnServerValidate="customvalidate">
                                                        </asp:CustomValidator>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <table id="Table3" border="0">
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:Label Style="z-index: 0" ID="lblDate" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(mLog.IsUTC = True, "Date (UTC)", "Date") %>' Width="70px">Date</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox Style="z-index: 0" ID="txtDate" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                                BackColor="Gainsboro" ReadOnly="True"></asp:TextBox>
                                                                                        </td>
                                                                                        <td></td>
                                                                                        <td>
                                                                                            <asp:Label Style="z-index: 0" ID="lblTLPNo" runat="server" CssClass="clsLabelAuto" Width="100px">TLP No.</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox Style="z-index: 0" ID="txtTLPNo" runat="server" Text="<%# mLog.LogPageNoFormatted %>"
                                                                                                CssClass="clsTextBoxTagSearch" BackColor="Gainsboro" ReadOnly="True"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="lblMaintOn" class="clsLabelAuto">Maintenance On</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchCombo" DataTextField="ModelSerialNoPostion"
                                                                                                DataValueField="AssemblyStatusID" Width="200px">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblStar" runat="server" CssClass="clsLabelStar" Style="z-index: 0">*</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto" Style="z-index: 0">Description</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox Style="z-index: 0" ID="txtMainActivity" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                                    Width="600px" BackColor="White" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto" Style="z-index: 0">NRC/WO NO</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtNCRNo" runat="server" BackColor="White" CssClass="clsTextBoxTagSearch"
                                                                                                MaxLength="50" Style="z-index: 0"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>&nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblDoneBy" runat="server" CssClass="clsLabelAuto" Style="z-index: 0">Done By</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter License No."
                                                                                                                    AutoComplete="off" ClientIDMode="Static" onchange="SetEmployeeIdonChange(this,'txtLicenceNo_AutoComplete')"
                                                                                                                    AutoPostBack="true" MaxLength="200"></asp:TextBox>
                                                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNo_AutoComplete" runat="server"
                                                                                                                    DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1"
                                                                                                                    CompletionInterval="1" ServicePath="" ServiceMethod="GetEmployeeList" TargetControlID="txtLicenceNo"
                                                                                                                    UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                                                                    CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                                                                    OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientItemSelected="SetID"
                                                                                                                    OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                                                </cc2:AutoCompleteExtender>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
                                                                                                                    Height="22px" Width="24px" ToolTip="Click to select multiple Licence No." CausesValidation="true" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblLicenceCount" runat="server" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
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
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPlace" runat="server" BackColor="White" CssClass="clsTextBoxTagSearch"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>&nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblClosedDate" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(mLog.IsUTC = True, "Closed Date (UTC)", "Closed Date") %>'>Closed Date</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="calClosedDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
                                                                                                Width="100px"></asp:TextBox>
                                                                                            <cc2:CalendarExtender ID="calClosedDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="calClosedDate"></cc2:CalendarExtender>
                                                                                            <cc2:TextBoxWatermarkExtender ID="calClosedDateWatermarkExtender" runat="server" TargetControlID="calClosedDate"
                                                                                                WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblAttachFile1" runat="server" CssClass="clsLabelAuto">Attach File</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlAttach" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="button" runat="server" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                                        class="clsbtnH clsinfoH1" />
                                                                                                </td>
                                                                                                <td style="padding-left: 3px;">
                                                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" Enabled="False"
                                                                                                        Text="Remove Attachment" ToolTip="Click to Remove Attachment" Width="140px" />
                                                                                                </td>
                                                                                                <td style="padding-left: 3px;">
                                                                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" Height="20px"
                                                                                                        ImageUrl="icons/CLIP01.ICO" Width="20px" />
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
                                                                <%--<td align="right">
                                                            <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table style="z-index: 0" id="Table12" border="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="clsButton_Ajax" ToolTip="Click to Add Maintenance Activity"
                                                                                    ValidationGroup="a"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>--%>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label Style="z-index: 0" ID="lblLogMaintenanceTitle" runat="server" CssClass="clsLabelHeader">Log Maintenance Details</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="dgMaintenanceActivity1" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
                                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                        <Columns>
                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID "></asp:BoundField>
                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No."></asp:BoundField>
                                                                            <asp:BoundField DataField="Maintenance" SortExpression="Maintenance" HeaderText="Activity"></asp:BoundField>
                                                                            <asp:BoundField DataField="NRCWONO" SortExpression="NRCWONO" HeaderText="NRC/WO No"></asp:BoundField>
                                                                            <asp:BoundField DataField="AllLicenceNosWithEmpName" HeaderText="Done By"></asp:BoundField>
                                                                            <asp:BoundField DataField="Place" SortExpression="Place" HeaderText="Place"></asp:BoundField>
                                                                            <asp:BoundField DataField="ClosedDateFormatted" HeaderText="Closed Date"></asp:BoundField>
                                                                            <%-- <asp:BoundField Text="Edit" HeaderText="Edit" CommandName="Edit"></asp:BoundField>
                                                                    <asp:BoundField Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:BoundField>--%>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <ItemTemplate>
                                                                                    <%-- <span id="button">Login</span>--%>
                                                                                    <div class="dropdown">
                                                                                        <div class="dropdownbtn-content">
                                                                                            <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="ImgEditView" runat="server" CommandName="EditRec" Style="height: 15px; width: 15px"
                                                                                                            ImageUrl="~/images/edit.png" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>' />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="DeleteRec" Style="height: 20px; width: 20px"
                                                                                                            ImageUrl="~/images/delete.png" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>' />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="View" runat="server" CausesValidation="false" CommandArgument='<%# Eval("ID") %>' ToolTip="Click to View Attachment"
                                                                                                            CommandName="View" Style="height: 20px; width: 17px" ImageUrl="icons/CLIP01.ICO"
                                                                                                            Visible='<%#  DataBinder.Eval(Container.DataItem, "ImageSize") > 0 %>' />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                        <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn" Style="height: 20px; width: 20px; cursor: pointer" />
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>


                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn"
                                                                                ItemStyle-CssClass="hideGridColumn" DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                        </Columns>
                                                                        <%--  <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>--%>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table style="z-index: 0" id="Table14" border="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <%--<asp:Button ID="btnBack" TabIndex="0" runat="server" CausesValidation="False" Text="Back"
                                                                                    CssClass="clsButton_Ajax" ToolTip="Click to go Previous page"></asp:Button>--%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <!--Dummy panel to open modelpopup for FileUpload-->
                                                            <tr style="height: 0px;">
                                                                <td style="height: 0px;">
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                        <ContentTemplate>
                                                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                            <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="AjaxLoader" DynamicLayout="false" DisplayAfter="200" runat="server">
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
        </div>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForLogMaintenanceActivity();
                return false;
            }

            function CallautoResize() {
                parent.autoResizeMaintActivity();
                return false;
            }
        </script>
        <div>
            <%--UPDATEPANEL --%>
            <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFrameLogMaintenanceActivityStateComplete();
                    }


                });
        <% End if %>
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
                function endRequestHandler() {
                    SetPageLayout();

                }

                function SetPageLayout() {
            <% Dim mopenas As String = Request.QueryString("Type") %>
                <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                    ReSetPageLayout();
                    onResize();//for Top bottom link
                <% End if %>
                }
                function ReSetPageLayout() {
                    $("body,html").css({ 'background-color': 'transparent' });
                    var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                    var windowheight = $(window).height();
                    if (tempMargtop >= windowheight) {
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                    }
                    else {
                        var margintop = (windowheight / 2) - (tempMargtop / 2);
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                    }

                }
            </script>
        </div>
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IFileUpload" frameborder="0" height="100%" width="100%" allowtransparency="true"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
            PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameFileUploadStateComplete() {
                $("#btnDummyFileUpload").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }
            $(document).ready(function () {
                $("#btnSelectFile").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IFileUpload").attr("src", "wfFileUpload.aspx");
                        $("#IFileUpload").ready(function () {
                            $("#btnDummyFileUpload").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        });

                        return false;
                    } catch (e) {
                        alert(e);
                    }


                });
            });
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForFileUpload(fileattached) {
                var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
                //close File Upload popup window
                FileUpwindow.hide();
                //Free resources
                $("#IFileUpload").attr("src", "JavaScript:''");
                if (fileattached) {
                    //call hidden button to set file upload content to object
                    $("#hdnBtnFileUpload").click();
                }
            }
        </script>
        <!-- End File Upload Modal Dialog-->
        <asp:HiddenField runat="server" ClientIDMode="Static" ID="EmployeeID" />
        <%-- Autocomplete functions to set id--%>
        <script type="text/javascript">
            function SetID(source, e) {
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
                if (source._id == "txtLicenceNo_AutoComplete") {
                    textbox = document.getElementById('EmployeeID');
                }
                textbox.value = value;
            }
            //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
            function SetEmployeeIdonChange(source, extenderid) {
                var popup = $find(extenderid);
                var complist = popup.get_completionList();
                var text = $(source).val().toLowerCase();
                for (var i = 0; i < complist.childNodes.length; i++) {
                    var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                    if (text == texttocompare) {
                        var val = complist.childNodes[i]._value;

                        if (extenderid == "txtLicenceNo_AutoComplete") {
                            textbox = document.getElementById('EmployeeID');
                        }
                        textbox.value = val;
                        return;
                    }

                }

                if (extenderid == "txtLicenceNo_AutoComplete") {
                    document.getElementById('EmployeeID').value = '';
                }
            }

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


            function AddEmployeeLicNo() {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=12");

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
    </form>
</body>
</html>
