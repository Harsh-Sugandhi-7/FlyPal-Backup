<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTrainingGroupSelectionList_Ajax.aspx.vb"
    Inherits="Flypal.wfTrainingGroupSelectionList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Training Group Selection</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        } 
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script type="text/javascript" src="tooltip.js"></script>
    <link rel="stylesheet" type="text/css" href="tooltip.css" />
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
</head>
<body bottommargin="5" ms_positioning="GridLayout" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="wfgroup" method="post" runat="server">
    <%--AJAX- ScriptManager Added--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--AJAX- Add MSGBox Control--%>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span class="clsFormHeader">Training Group Selection</span>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlAddBottom" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnDone" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                            Text="DONE" ToolTip="Click to Add the Trainings" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                            Text="Close" ToolTip="Click to close Training Group Selection screen" />
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
                                        <asp:UpdatePanel ID="upnlTrainingGroupSelection" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="lblGroup" class="clsLabel">Group</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="cmbGroup" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataTextField="GroupName"
                                                                        DataValueField="GroupName" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td align="right">
                                                            <table id="Table1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnFindNow" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to find the Training Group Selection as per searching criteria"
                                                                            Text="Find Now" Visible="false"></asp:Button>
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
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlGridViewTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Training(s) as per criteria : Record(s) found.</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlAddTop" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnDoneTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax" Visible="false"
                                                                Text="DONE" ToolTip="" />
                                                        </td>
                                                        <td>
                                                            <%-- <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Close" ToolTip="Click to close Training Group Selection screen" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%-- <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgTrainingList" runat="server" AllowSorting="True" AutoGenerateColumns="False" RepeatDirection="Horizontal"
                                                    CssClass="clsGrid" AllowPaging="True" PageSize="15" ShowHeaderWhenEmpty="true"
                                                    PagerSettings-Mode="NumericFirstLast" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <HeaderTemplate>
                                                                <input type="checkbox" id="chkSelectAll" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                    <%# NumeroChequeInclus(Eval("ID").ToString()) %> />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderText="Id" Visible="False"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Training" SortExpression="Name">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="#FFFFFF" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Recurring Status" ItemStyle-Width="80px">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkRecurringStatus" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "RecurringStatus") %>'
                                                                    Enabled="False"></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                                        </asp:TemplateField>
                                                         <asp:BoundField DataField="FreqInMonths" HeaderText="Freq In Months">
                                                            <HeaderStyle HorizontalAlign="Center" ForeColor="#FFFFFF" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                        <asp:UpdatePanel ID="upnlTrainingList" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fdsMonitoringDetails" class="clsFieldSetNewStyle" style="border-width: 1px;
                                                    position: relative">
                                                    <legend id="ledTrainingList">
                                                        <asp:CheckBox ID="chkAll" runat="server" CssClass="clsCheckBox" Text="ALL" AutoPostBack="true"></asp:CheckBox></legend>
                                                    <asp:CheckBoxList ID="ChklistTrainingList" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                        ClientIDMode="Static" DataValueField="ID" DataTextField="Name" RepeatColumns="8"
                                                        RepeatDirection="Horizontal">
                                                    </asp:CheckBoxList>
                                                    </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td align="right">
                                        <asp:UpdatePanel ID="upnlAddBottom" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnDone" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                Text="DONE" ToolTip="" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                Text="Close" ToolTip="Click to close Training Group Selection screen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpTraining();
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
                        parent.IFrameEmpTrainingStateComplete();
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
		 Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){
            $("label[for*='ChklistTrainingList']").mouseover(function () {
				 var TrainingListObject=new Object(); //RecurringStatus value in object["Key"]= "value" format...
				 var tempDescription='';
				<% For i As Integer = 0 To mTrainingList.Count - 1%>
						tempDescription='<%=mTrainingList(i).RecurringStatusWithFreq.Replace(Environment.NewLine,"¿") %>';	//REplace Line break with custom char....
						tempDescription=tempDescription.replace(new RegExp('¿','g'), '<br />');									//Replace all custom char(if exists) with new line char of javascript to show exactly same as entered
						TrainingListObject['<%=mTrainingList(i).Name %>']= tempDescription;
				<%  Next %>
				               		
           $(this).attr('title',TrainingListObject[$(this).text()]); //Returns short code(text) of the current mouse hover item(View HTML for CheckBoxList)
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("label[for*='ChklistTrainingList']").tooltip({
                //borderColor:"#009DD9",
                borderColor: 'Grey',
                borderSize: 1,
                cancelClick: 1,
                tooltipPadding: 5,
                tooltipBGColor: '#96c8a2',
                tooltipTextColor: 'black'
            });
        });
    </script>
    </form>
</body>
</html>
