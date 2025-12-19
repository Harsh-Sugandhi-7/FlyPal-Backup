<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogFlightCrew_Ajax.aspx.vb"
    Inherits="Flypal.wfLogFlightCrew_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Flight Log Crew</title>
    <meta http-equiv="x-ua-compatible" content="IE=9"/>
    <meta name="vs_showGrid" content="True"/>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
    <link id="MainStyle" type="text/css" rel="stylesheet"/>
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
			function openledgersame(FileName)
        {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
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
            <table id="tblMain" class="clstablelistout" border="0">
                <tr>
                    <td class="clstablecell">
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                            <table id="tblinner" class="clsTablelistin" border="0">
                                <tr>
                                     
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Flight Crew</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right">

                                                    <asp:Button ID="btnAdd" TabIndex="0" runat="server" Text="Add" CssClass="clsbtnH clsinfoH"
                                                        ToolTip="Click to Add the Flight Crew"></asp:Button>

                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" Visible="true" 
                                                        CausesValidation="False" ToolTip="Click to go Previous page"></asp:Button>
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
                                                    <asp:Button ID="Button1" runat="server" CausesValidation="False" Text="Log details"
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
                                                    <asp:Button ID="btnParameterList" runat="server" Text="Parameter List" CausesValidation="False"
                                                        CssClass="clsButtonLong_Ajax"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnLogPax" runat="server" Text="Passenger Log" CausesValidation="False"
                                                        Visible='<%# iif(AppSettings("ShowExtraLogTabs") = "True",True,False) %>' CssClass="clsButtonLong_Ajax"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnHobbsOffset" runat="server" Text="Hobbs Offset" CausesValidation="False"
                                                        Visible='<%# iif(AppSettings("ShowExtraLogTabs") = "True",True,False) %>' CssClass="clsButtonLong_Ajax"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFlightCrew" runat="server" ToolTip="Flight Crew details" Text="Flight Crew"
                                                        CssClass="clsLabelButton" Width="70px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnMaintenanceAcitvity" runat="server" CausesValidation="False" Text="Maintenance Activity"
                                                        CssClass="clsButtonLong_Ajax"></asp:Button>
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
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator Style="z-index: 0" ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"
                                                    OnServerValidate="customvalidate" Display="None" ControlToValidate="txtTLPNo"></asp:CustomValidator><asp:CustomValidator
                                                        Style="z-index: 0" ID="cvCrew" runat="server" CssClass="clsValidationSummary"
                                                        OnServerValidate="customvalidate" Display="None" ControlToValidate="cmbCrew"></asp:CustomValidator><asp:CustomValidator
                                                            Style="z-index: 0" ID="cvDutyAs" runat="server" CssClass="clsValidationSummary"
                                                            OnServerValidate="customvalidate" Display="None" ControlToValidate="cmbDutyAs"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <table id="Table4" border="0">
                                                <tr>
                                                    <td style="width: 16px"></td>
                                                    <td>
                                                        <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(mLog.IsUTC = True, "Date (UTC)", "Date") %>'>Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table id="Table3" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="CalDate" runat="server" CssClass="clsTextBoxTagSearchDate" BackColor="Gainsboro"
                                                                        ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTLPNo" runat="server" CssClass="clsLabelAuto" Width="100px">TLP No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTLPNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLog.LogPageNoFormatted %>"
                                                                        BackColor="Gainsboro" ReadOnly="True" MaxLength="25" Height="15px" ></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 16px">
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesignation" runat="server" CssClass="clsLabelAuto">Designation</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbDesignationList" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                            DataValueField="ID" DataTextField="Name" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 16px">
                                                        <asp:Label ID="lblStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCrew" runat="server" CssClass="clsLabelAuto">Crew</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbCrew" runat="server" CssClass="clsTextBoxTagSearchCombo" DataValueField="ID"
                                                            DataTextField="EmpNoName" Enabled="False">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 16px">
                                                        <asp:Label Style="z-index: 0" ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label Style="z-index: 0" ID="lblDutyAs" runat="server" CssClass="clsLabelAuto">Duty As</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList Style="z-index: 0" ID="cmbDutyAs" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                            DataValueField="ID" DataTextField="DutyType">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td align="right">
                                            <table id="Table11" border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" Text="Add" CssClass="clsButton_Ajax"
                                                            ToolTip="Click to Add the Flight Crew"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFlightCrewTitle" runat="server" CssClass="clsLabelHeader">Flight Crew Details</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="dgCrewList1" runat="server" CssClass="clsGridNewStyle" ToolTip="Flight Crew List" CellPadding="5"
                                                PageSize="3" AutoGenerateColumns="False" GridLines="Horizontal">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No."></asp:BoundField>
                                                    <asp:BoundField DataField="CrewName" SortExpression="CertificateName" HeaderText="Name">
                                                        <HeaderStyle></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DutyType" HeaderText="Duty As">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--<asp:BoundField Text="Edit/View" HeaderText="Edit" CommandName="Edit"></asp:BoundField>
                                                    <asp:BoundField Text="Remove" HeaderText="Remove" CommandName="Remove"></asp:BoundField>--%>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="ImgEditView" runat="server" CommandName="EditRec" Style="height: 15px; width: 15px"
                                                                                    ImageUrl="~/images/edit.png" CausesValidation="false" CommandArgument='<%# Eval("ID") %>' />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="DeleteRec" Style="height: 20px; width: 20px"
                                                                                    ImageUrl="~/images/delete.png" CausesValidation="false" CommandArgument='<%# Eval("ID") %>' />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <%--<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>--%>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td align="right">
                                            <table style="z-index: 0" id="Table1" border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Back"
                                                            CausesValidation="False" ToolTip="Click to go Previous page"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
                  parent.ParentCallBackFunctionForLogCrew();
                  return false;
              }
          </script>
    </form>
    
</body>
</html>
