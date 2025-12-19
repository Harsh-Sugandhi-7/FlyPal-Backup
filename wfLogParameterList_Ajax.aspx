<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogParameterList_Ajax.aspx.vb"
    Inherits="Flypal.wfLogParameterList_Ajax" %>

<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Log Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script src="VALIDATEFUNCTIONS.js" type="text/javascript"></script>
    <script  src="DATEFUNCTIONS.js" type="text/javascript"></script>

</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                            <table id="tblinner" class="clsTablelistin" border="0" cellpadding="0">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Log Details</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table7" border="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save current record"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" runat="server" Text="Back" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                            ToolTip="Click to go back to previous page"></asp:Button>
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
                                    <td>
                                        <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table style="display: none;">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnLogDetails" runat="server" Text="Log Details" CausesValidation="False"
                                                                CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnFuelOil" runat="server" Text="Fuel Oil" CausesValidation="False"
                                                                CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDefectActionList" runat="server" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Defect Reporting","Snag Reporting") %>'
                                                                CausesValidation="False" CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblParameterList" runat="server" CssClass="clsLabelButton" ToolTip="Parameter List">Parameter List</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnLogPax" runat="server" Text="Passenger Log" CausesValidation="False"
                                                                Visible='<%#IIf(AppSettings("ShowExtraLogTabs") = "True", True, False) %>' CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnHobbsOffset" runat="server" Text="Hobbs Offset" CausesValidation="False"
                                                                Visible='<%#IIf(AppSettings("ShowExtraLogTabs") = "True", True, False) %>' CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnLogCrew" runat="server" Text="Flight Crew" CausesValidation="False"
                                                                CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button Style="z-index: 0" ID="btnMaintenanceAcitvity" runat="server" Text="Maintenance Activity"
                                                                CausesValidation="False" CssClass="clsButtonLong_Ajax"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server"
                                                    CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields" />
                                                <asp:CustomValidator ID="cvParameterList" runat="server"
                                                    ControlToValidate="cmbParameterList"
                                                    ErrorMessage="Select Parameters form List." Display="None" />
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
                                                            <asp:Label ID="lblParameterStar1" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblParameter" runat="server" CssClass="clsLabel" Visible="False">Parameter</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbParameterList" runat="server" CssClass="clsComboBox_Ajax"
                                                                Visible="False" DataValueField="ParameterID" DataTextField="ParameterName">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" Text="Add" CssClass="clsbtnH clsinfoH"
                                                                ToolTip="Click to Add the Assembly" Visible="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:DataGrid ID="dgLogParameters" runat="server" CssClass="clsGridNewStyle" 
                                                                AllowSorting="True" PageSize="3" AutoGenerateColumns="False" 
                                                                CellPadding="5" ForeColor="Black" GridLines="Horizontal" BorderWidth="0">
                                                                <ItemStyle CssClass="clsdgItem" HorizontalAlign="Left"></ItemStyle>
                                                                <AlternatingItemStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingItemStyle>
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn Visible="False" DataField="ParameterID" HeaderText="ParameterID "></asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="AssemblyTypeName" HeaderText="Assembly Type">
                                                                        <HeaderStyle></HeaderStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="AssemblyName" HeaderText="Assembly Info">
                                                                        <HeaderStyle></HeaderStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ParameterName" HeaderText="Parameter Name ">
                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ParameterDescription" HeaderText="Parameter Description ">
                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="MinValue" HeaderText="Min">
                                                                        <HeaderStyle Wrap="False"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="MaxValue" HeaderText="Max">
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn Visible="False" HeaderText="Parameter Value ">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtParameterValue" runat="server"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"ParameterValue") %>'
                                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                ToolTip="Parameter Value " MaxLength="10">
                                                                            </asp:TextBox>
                                                                            <asp:CustomValidator ID="cvParameterValue" runat="server"
                                                                                ErrorMessage="Log parameter value."
                                                                                OnServerValidate="CustomValidate" Display="None" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="4"></td>
                                                    </tr>
                                                    <asp:PlaceHolder ID="pl" runat="server" Visible="false">
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label Style="z-index: 0" ID="Label1" runat="server" 
                                                                    CssClass="clsLabelAuto">
                                                                    *Note : "Copy" button indicates to copy  the value of that 
                                                                            parameter into respective assemblies.
                                                                </asp:Label>
                                                            </td>
                                                        </tr>
                                                    </asp:PlaceHolder>
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
                parent.ParentCallBackFunctionForLogParameter();
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
                        parent.IFrameLogParameterStateComplete();
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
                function CallParentFunction() {
                    window.parent.autoResizeParameterList();
                }
            </script>

        </div>
    </form>
</body>
</html>
