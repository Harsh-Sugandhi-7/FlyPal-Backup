<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfEmployeeDisciplinary_Ajax.aspx.vb" Inherits="Flypal.wfEmployeeDisciplinary_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD runat ="server">
        <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
		<title>Employee Disciplinary</title>
		<SCRIPT language="javascript">
			function openledgersame(FileName)
               {
                  window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

               }
		</SCRIPT>
		
		<LINK    id="MainStyle" type="text/css" rel="stylesheet">
        <asp:placeholder runat="server">
            <!-- #include file= "LocalFunctionAjax.htm" -->
		</asp:placeholder>
		
		<script language="javascript" id="clientEventHandlersJS">
			function openTranDetail()
			{
				str = "wfReports.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openTranDetail1()
			{
				str = "webform1.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openFile()
			{
				str = "wfFileView.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openDetail()
			{
				str = "wfDetail.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
		</script>
		<script language="javascript" src="VALIDATEFUNCTIONS.js">
		</script>
	</HEAD>
	<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <table class="clstablelistin" id="tblInner">
                                <tr>
                                    <td colspan="5" class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblTitle" TabIndex="1" CssClass="clsFormHeader" runat="server">Employee Disciplinary [New]</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right" colspan="5">
                                                    <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                                <tr>
                                                                    <td align="right" colspan="3">
                                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save Disciplinary Information"
                                                                            Text="Save" ValidationGroup="valGroup1"></asp:Button></td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                            Text="Back" CausesValidation="False"></asp:Button></td>
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
                                    <td colspan="5">
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" Width="440px" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Date should not be blank." Display="None" ControlToValidate="calIncidentDate" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvDescription" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                                    Display="None" ControlToValidate="txtDescription" ValidationGroup="valGroup1"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                                    ErrorMessage="Description Required" Display="None" ControlToValidate="txtDescription" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvComments" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                                    Display="None" ControlToValidate="txtComments" ValidationGroup="valGroup1"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvFeedBack" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                                    Display="None" ControlToValidate="txtFeedBack" ValidationGroup="valGroup1"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:UpdatePanel ID="upnlDisciplinaryDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="5">
                                                            <span id="lblSkillDetails" class="clsLabelHeader">Employee Disciplinary Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblEmployeeName" class="clsLabelAuto">Employee Name</span></td>
                                                        <td align="left" colspan="3">
                                                            <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxSearch_Ajax" MaxLength="25" ToolTip="Employee Name" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mEmployee.Name %>">
                                                            </asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Label2" class="clsLabelStar" style="color: Red;">*</span></td>
                                                        <td>
                                                            <span id="lblIncidentDate" class="clsLabelAuto">Incident Date</span></td>
                                                        <td colspan="3">
                                                            <table id="Table3" cellspacing="0" cellpadding="0" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="calIncidentDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                                            runat="server" CausesValidation="true" AutoPostBack="true"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="calIncidentDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="calIncidentDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="calIncidentDate" ID="Calender_watermarkextender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Label1" class="clsLabelStar" style="color: Red;">*</span></td>
                                                        <td>
                                                            <span id="lblDescription" class="clsLabelAuto">Description</span></td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" MaxLength="500" ToolTip="Enter Description" Text="<%# mEmployeeDisciplinary.Description %>" TextMode="MultiLine">
                                                            </asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblReportedBy" class="clsLabelAuto">Reported By</span></td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtReportedBy" runat="server" CssClass="clsTextBoxSearch_Ajax" MaxLength="100" ToolTip="Enter Reported By" Text="<%# mEmployeeDisciplinary.ReportedBy %>">
                                                            </asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblDisciplinary" class="clsLabelAuto">Disciplinary Action</span></td>
                                                        <td colspan="3">

                                                            <table id="Table6" cellspacing="0" cellpadding="0" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbDisciplinaryList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID" DataTextField="Name" SelectedValue="<%# mEmployeeDisciplinary.DisciplinaryID %>">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:Button ID="imgDisciplinary" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Disciplinary"
                                                                            Text="..." CausesValidation="False"></asp:Button></td>--%>

                                                                    <asp:ImageButton ID="imgDisciplinary" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                      Width="24px" ToolTip="Click to Add New Disciplinary" CausesValidation="False"></asp:ImageButton>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblComments" class="clsLabelAuto">Comments</span></td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtComments" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" MaxLength="500" ToolTip="Enter Comments" Text="<%# mEmployeeDisciplinary.Comments %>" TextMode="MultiLine">
                                                            </asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblFeedBack" class="clsLabelAuto">FeedBack</span></td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtFeedBack" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" MaxLength="500" ToolTip="Enter FeedBack" Text="<%# mEmployeeDisciplinary.FeedBack %>" TextMode="MultiLine">
                                                            </asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td class="clsInnerTable">
                                                            <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                        </td>
                                                        <td colspan="3">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                            class="clsbtnH clsinfoH1">
                                                                    </td>
                                                                    <td style="padding-left: 3px;">
                                                                        <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                            Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                    </td>
                                                                    <td style="padding-left: 2px;">
                                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                            Height="20px" Width="20px"></asp:ImageButton>
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
                                    <%--<TD align="right" colSpan="5">
                                        <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <TABLE id="Table1" cellSpacing="1" cellPadding="1" border="0">
											        <TR>
												        <TD align="right" colSpan="3">
													        <asp:Button id="btnSave" CssClass="clsbtnH clsinfoH" Runat="server" ToolTip="Click to Save Disciplinary Information"
														        Text="Save" ValidationGroup="valGroup1"></asp:Button></TD>
												        <TD align="right">
													        <asp:button id="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
														        Text="Back" CausesValidation="False"></asp:button></TD>
											        </TR>
										        </TABLE>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
										
									</TD>--%>
                                </tr>
                                <!--Dummy panel to open modelpopup-->
                                <tr style="height: 0px;">
                                    <td style="height: 0px;">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--End -->
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForEmpDisciplinary();
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
                    parent.IFrameEmpDisciplinaryStateComplete();
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
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
            width: 100%;">
            <iframe id="IFileUpload" allowTransparency="true" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                scrolling="auto"></iframe>
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
                        if (!$.browser.msie) {
                            $("#btnDummyFileUpload").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

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
        <!-- End -->
         <!-- Disciplinary Master --ModalPopUp -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDisciplinaryMaster" Text="Dummy Disciplinary Master" />
        </div>
        <asp:Panel runat="server" ID="pnlDisciplinaryMaster" Style="display: none ">
            <div>
                <table class="clstablelistout" id="TABLE2">
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upnlDisciplinaryMaster" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="clstablelistin" id="TABLE4">
                                        <tr>
                                            <td colspan="4" class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                             <asp:Label ID="lblTitleDisciplinaryMaster" TabIndex="1" CssClass="clsFormHeader" runat="server">Disciplinary Information [New]</asp:Label></td>
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnNewDisciplinaryMaster" CssClass="clsbtnH clsinfoH" runat="server" Text="New" ToolTip="Click to Add the Disciplinary"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSaveDisciplinaryMaster" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Disciplinary Information"
                                                                ValidationGroup="valGroup2"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCloseDisciplinaryMaster" TabIndex="0" runat="server"  CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Disciplinary Information screen"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            </tr>
                                                </table>
                                               
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary" ValidationGroup="valGroup2"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Disciplinary Required."
                                                    Display="None" ControlToValidate="txtName" ValidationGroup="valGroup2"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvDocument" runat="server" CssClass="clsLabelAuto" ErrorMessage="Disciplinary Name too Long."
                                                    Display="None" ControlToValidate="txtName" OnServerValidate="Customvalidate1" ValidationGroup="valGroup2"></asp:CustomValidator></td>
                                        </tr>
                                        <tr>
                       
                                            

                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <span id="lblDocumentDetails" class="clsLabelHeader">Disciplinary Details</span></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%">
                                                    <tr>
                                                        <td valign="middle" align="center">
                                                            <span id="Label4" class="clsLabelStar" style="color: Red;">*</span></td>
                                                        <td valign="middle">
                                                            <span id="lblName" class="clsLabelAuto">Name</span></td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtName" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" Text="<%# mDisciplinary.Name %>" ToolTip="Enter Disciplinary Name" MaxLength="5000" TextMode="MultiLine">
                                                            </asp:TextBox></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                               
                                            <td align="right">
                                                <%--<asp:Button ID="btnSaveDisciplinaryMaster" CssClass="clsButton_Ajax" runat="server" Text="Save" ToolTip="Click to Save Disciplinary Information" ValidationGroup="valGroup2"></asp:Button></td>--%>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <span id="lblSearch" class="clsLabelHeader">Disciplinary List</span></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <%--<div style="width: 610px;">
                                                    <table cellpadding="0" cellspacing="0" class="clsGrid"
                                                        style="width: 610px; border-collapse: collapse;">
                                                        <tr>
                                                            <td class="clsdgHeader" width="490px">
                                                                <span>Name</span>
                                                            </td>
                                                            <td class="clsdgHeader" width="70px">
                                                                <span>Edit/View</span>
                                                            </td>
                                                            <td class="clsdgHeader" width="50px">
                                                                <span>Delete</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>--%>
                                                <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 631px;">
                                                    <asp:GridView ID="dgDisciplinary" runat="server"
                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False" ShowHeader="true" ShowHeaderWhenEmpty="true" Style="width: 610px;" DataKeyNames="ID">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="Name" HeaderText="Name">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="490px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                            </asp:ButtonField>--%>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div id="dropDownImg" class="dropdown">
                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="editICN" Style="height: 15px; width: 15px" runat="server" 
                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                            ToolTip="Click to Edit record"
                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                            ToolTip="Click to Delete record"
                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                    </td>

                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="4">
                                                <table id="Table5" cellspacing="0" cellpadding="0" align="right" border="0">
                                                    <tr>
                                                        <%--<td valign="bottom" align="right">
                                                            <asp:Button ID="btnCloseDisciplinaryMaster" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to close Disciplinary Information screen"
                                                                CausesValidation="False"></asp:Button></td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopUpDisciplinaryMaster" runat="server" TargetControlID="btnDummyDisciplinaryMaster"
            PopupControlID="pnlDisciplinaryMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
		</form>
	</body>
</HTML>
