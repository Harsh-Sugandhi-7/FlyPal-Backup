<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEvidenceDetail.aspx.vb"
    Inherits="Flypal.wfEvidenceDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1.0" />
    <title>Evidence Details</title>
    <meta content="Alix Mobile App" name="description" />
    <meta content="themepassion" name="author" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Panel ID="tblmain" runat="server" Style="left: 16px;">
        <table id="Table4" border="1" style="width: 90%; z-index: 1000; opacity: 1; background-color: whitesmoke;">
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upnlActivities" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="mainContainerIndent">
                                <div>
                                    <table class="jumbotron" style="width: 100%; height: 95px">
                                        <tr>
                                            <td align="center">
                                                &nbsp;&nbsp; <span style="font-size: 27px; font-weight: 100" class="text-center text-info"
                                                    runat="server" id="lblLogDet"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp; <span style="font-size: 22px; font-weight: 100" class="text-danger">Affected
                                                    Complied Activities &nbsp;&nbsp; <span runat="server" id="lblResultActivities" class="text-warning">
                                                    </span></span>
                                            </td>
                                            <div class="container">
                                                <div class="section">
                                                    <asp:LinkButton runat="server" class="btn btn-link pull-right" ID="btnClose"><span class="fa fa-times"></span> Close</asp:LinkButton>
                                                </div>
                                            </div>
                                        </tr>
                                    </table>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="dgEvidenceMaintActivitiesDetailsLogList" runat="server" Width="100%"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" CssClass="table table-striped table-bordered table-hover"
                                                    AllowPaging="true" PageSize="10" AutoGenerateColumns="False" EmptyDataText="There are no data records to display.">
                                                    <Columns>
                                                        <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ATACode" HeaderText="ATA" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ActivityTypeName" HeaderText="Activity" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ModelTypeCode" HeaderText="Data Type">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Reference" HeaderText="Reference">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" HtmlEncode="false">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On" HtmlEncode="false">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Right" Width="70px" ForeColor="Black">
                                                            </HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false"
                                                            Visible="false">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Right" Width="70px" ForeColor="Black">
                                                            </HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DoneOnValue" HeaderText="Done On Value" HtmlEncode="false"
                                                            Visible="false">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Right" Width="70px" ForeColor="Black">
                                                            </HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="StatusMasterID" HeaderText="Status Master ID" Visible="false">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Position="Bottom" />
                                                    <PagerStyle CssClass="paging" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upnlLog" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="Div1">
                                <div>
                                    <table class="jumbotron" style="width: 100%; height: 95px">
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp; <span style="font-size: 22px; font-weight: 100" class="text-danger">Affected
                                                    Log(s) &nbsp;&nbsp; <span runat="server" id="lblResultLogs" class="text-warning">
                                                    </span></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="grdLogs" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="5"
                                                    AutoGenerateColumns="False" EmptyDataText="There are no data records to display.">
                                                    <Columns>
                                                        <asp:BoundField DataField="LogTextNo" HeaderText="Log No." HtmlEncode="false" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                            <ItemStyle  Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LogDateFormatted" HeaderText="Log Date" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                            <ItemStyle  Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LogPageNo" HeaderText="Log Page No." HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black" Wrap="true"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TimeInAir" HeaderText="Time In Air" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FieldName" HeaderText="Field Name" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OldValue" HeaderText="Old Value" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NewValue" HeaderText="New Value" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                       <%-- <asp:BoundField DataField="PeriodName" HeaderText="Period" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>--%>
                                                        <asp:BoundField DataField="DateTimeStampFormatted" HeaderText="Date">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" CssClass="title"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <FooterStyle Wrap="False"></FooterStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Position="Bottom" />
                                                    <PagerStyle CssClass="paging" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upnlAssemblyRemoval" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="Div2">
                                <div>
                                    <table class="jumbotron" style="width: 100%; height: 95px">
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp; <span style="font-size: 22px; font-weight: 100" class="text-danger">Affected
                                                    Assembly Removal(s) &nbsp;&nbsp; <span runat="server" id="lblAssemblyRemovals" class="text-warning">
                                                    </span></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="grdAssemblyRemoval" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10"
                                                    AutoGenerateColumns="False" EmptyDataText="There are no data records to display.">
                                                    <Columns>
                                                        <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No." HtmlEncode="false" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssemblyTypeName" HeaderText="Assembly Type" HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Position="Bottom" />
                                                    <PagerStyle CssClass="paging" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upnlAssemblyInstallation" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="Div3">
                                <div>
                                    <table class="jumbotron" style="width: 100%; height: 95px">
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp; <span style="font-size: 22px; font-weight: 100" class="text-danger">Affected
                                                    Assembly Installation(s) &nbsp;&nbsp; <span runat="server" id="lblAssemblyInstallations"
                                                        class="text-warning"></span></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="grdAssemblyInstallation" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10"
                                                    AutoGenerateColumns="False" EmptyDataText="There are no data records to display.">
                                                    <Columns>
                                                        <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." HtmlEncode="false"
                                                            ShowHeader="true" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No." HtmlEncode="false" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssemblyTypeName" HeaderText="Assembly Type" HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Position="Bottom" />
                                                    <PagerStyle CssClass="paging" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upnlCompRemoval" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="Div4">
                                <div>
                                    <table class="jumbotron" style="width: 100%; height: 95px">
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp; <span style="font-size: 22px; font-weight: 100" class="text-danger">Affected
                                                    Component Removal(s) &nbsp;&nbsp; <span runat="server" id="lblCompRemovals" class="text-warning">
                                                    </span></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="grdCompRemoval" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10"
                                                    AutoGenerateColumns="False" EmptyDataText="There are no data records to display.">
                                                    <Columns>
                                                        <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No." HtmlEncode="false" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssemblyTypeName" HeaderText="Assembly Type" HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CompInfo" HeaderText="Component Info." HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Position="Bottom" />
                                                    <PagerStyle CssClass="paging" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upnlCompInstallation" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="Div5">
                                <div>
                                    <table class="jumbotron" style="width: 100%; height: 95px">
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp; <span style="font-size: 22px; font-weight: 100" class="text-danger">Affected
                                                    Component Installation(s) &nbsp;&nbsp; <span runat="server" id="lblCompInstallations"
                                                        class="text-warning"></span></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="grdCompInstallation" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10"
                                                    AutoGenerateColumns="False" EmptyDataText="There are no data records to display.">
                                                    <Columns>
                                                        <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RegNo" HeaderText="Reg No." HtmlEncode="false" HeaderStyle-CssClass="hidden-xs"
                                                            ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssemblyTypeName" HeaderText="Assembly Type" HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CompInfo" HeaderText="Component Info." HtmlEncode="false"
                                                            HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Position="Bottom" />
                                                    <PagerStyle CssClass="paging" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="container">
                                        <div class="section">
                                            <asp:LinkButton runat="server" class="btn btn-link pull-right" ID="btnCloseBottom"><span class="fa fa-times"></span> Close</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <%--   <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </asp:UpdateProgress>--%>
    <!-- OTHER SCRIPTS INCLUDED ON THIS PAGE - END -->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEvidenceDetails();
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
      // if ($.browser.msie) {
             parent.IFrameEvidenceDetailsStateComplete();
       //  }
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
          //onResize();//for Top bottom link
           <% End if %>
       }
       function ReSetPageLayout()
       {
       $("body,html").css({ 'background-color': 'transparent' });
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px','margin-left':'20px'});
          }
       
       }
    </script>
    <%--End--%>
    </form>
    <script type="text/javascript">
		if("<%= not HttpContext.Current.Session("StyleSheet") is nothing %>"=="True")
			{
			$("#MainStyle").attr('href',"<%= HttpContext.Current.Session("StyleSheet") %>");
			}
    </script>
</body>
</html>
