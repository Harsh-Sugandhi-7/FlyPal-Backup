<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTraining_Ajax.aspx.vb" Inherits="Flypal.wfTraining_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat ="server" >     
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Training</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <LINK    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" --> 
    </asp:placeholder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">

    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox runat="server" ID="MSGBoxCntrl"  />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Training</asp:Label>   
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnNew" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add new Training"
                                                            Text="New" CausesValidation="False"></asp:Button>
                                                    </td>

                                                    <td align="right">
                                                        <%-- AJAX Update Panel --%>
                                                        <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to save Training"
                                                                    Text="Save" CausesValidation="true" ValidationGroup="1"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>

                                                    <td valign="bottom" align="right">
                                                          <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                        <asp:Button ID="btnBack" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close Training Information screen"
                                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                                </ContentTemplate>
                                                         </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                 <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary" ValidationGroup="1">
                                        </asp:ValidationSummary>
                                       
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtName" ErrorMessage="Training Name Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvTrainingType" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="cmbTrainingType" ErrorMessage="Please select the Training Type." ValidationGroup="1" ClientValidationFunction="validateTraningType"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtName" ErrorMessage="Training Name too long." ValidationGroup="1" ClientValidationFunction="validateName"></asp:CustomValidator>
                                        
                                        
                                        <script type="text/javascript" >
                                            function validateTraningType(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbTrainingType");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }

                                            function validateName(source, args) {
                                                args.IsValid = false;
                                                var length = $("#txtName").val().length;
                                                if (length <= 100) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td>
                                <span ID="lblAdd" Class="clsLabelAuto">Click To Add New Record</span>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnNew" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add new Training"
                                Text="New" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="2">
                                <span ID="lblTrainingDetails" Class="clsLabelHeader">Training Details</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                               <asp:UpdatePanel ID="upnlTrainingDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0">
                                            <tr>
                                                <td>
                                                    <span ID="Label2" Class="clsLabelStar" style="color:Red;">*</span>
                                                </td>
                                                <td>
                                                    <span ID="lblTrainingName" Class="clsLabelAuto">Training Name </span>
                                                </td>
                                                <td>
                                                    <table id="Table9" border="0">
                                                        <tr>
                                                            <td>
                                                                 <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Training Name"
                                                                    Text="<%# mTraining.Name %>" MaxLength="100"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span ID="Label3" Class="clsLabelStar" style="color:Red;">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTrainingType" runat="server" CssClass="clsLabelAuto">Training Type  </asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table3" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbTrainingType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                    DataTextField="Name" SelectedValue="<%# mTraining.TrainingTypeID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <%--<asp:Button ID="btnTrainingTypeList" runat="server" CssClass="clsButtonGrid_Ajax" Text="..." ClientIDMode="Static" 
                                                                    CausesValidation="False"></asp:Button>--%>

                                                                <asp:ImageButton ID="btnTrainingTypeList" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px" ToolTip="Click to Add New Training Type" 
                                                                    CausesValidation="False" ></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span ID="lblRecurringStatus" Class="clsLabel">Recurring Status </span>
                                                </td>
                                                <td>
                                                    <table id="Table18" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkRecurringStatus" runat="server" AutoPostBack="true" CssClass="clsCheckBox" ToolTip="Check this in case the Training is Recurring"
                                                                    Text="(Check this in case the Training is Recurring)" Checked="<%# mTraining.RecurringStatus %>">
                                                                </asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span ID="lblFreqInMonths" Class="clsLabelAuto">Freq In Months </span>
                                                </td>
                                                <td>
                                                    <table id="Table19" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtFreqInMonths" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    ToolTip="Enter Freq In Month" Text="<%# mTraining.FreqInMonths %>" MaxLength="5" Enabled='<%# iif(mTraining.RecurringStatus = True,"True","False") %>'>
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span ID="lblWarningDays" Class="clsLabelAuto">Warning Days</span>
                                                </td>
                                                <td>
                                                    <table id="Table20" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    ToolTip="Enter Warning Days" Text="<%# mTraining.WarningDays %>" MaxLength="5">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlModalPopUpLinks" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                            <table id="Table10" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField runat="server" ID="btnDummyControl"/>
                                                            <asp:LinkButton ID="lnkTrainingDesignation" runat="server" CssClass="clsLinkButton"
                                                                ToolTip="Click to add Training Designation" CausesValidation="false"  >Training Designation</asp:LinkButton>
                                                            
                                                           
                                                            <cc2:ModalPopupExtender ID="lnkTrainingDesignation_ModalPopupExtender" 
                                                                runat="server" DynamicServicePath="" Enabled="True"  Y=50
                                                                TargetControlID="btnDummyControl" PopupControlID="Panl1" BackgroundCssClass="clsModalPopupBG"></cc2:ModalPopupExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="Panl1" runat="server" align="center" style = "display:none;background-color: #FFFFFF;border-width: 1px;border-style: solid;width:auto;max-height:450px;overflow-x:hidden;overflow-y:auto;">
                                                                                                                                                 
                                                                            <%--<iframe style=" width: 350px; height: 300px;" id="irm1" src="wfTrainingDesignation.aspx?Childpage3=index.aspx&BackPage=&ChildPage=&ChildPage1=&ChildPage2=&IsFromRenewal=" runat="server"></iframe>
                                                                            <br/>--%>
                                                                            <table class="clstablelistout" id="Table5" border="0" >
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1">
                                                                                            <table class="clstablelistin" id="Table7" border="0">
                                                                                                <tr>
                                                                                                    <td colspan="4" class="clsFormHeader1Newstyle">
                                                                                                        <asp:Label ID="lblTitleTrainingDesg" CssClass="clsFormHeader" runat="server">Training Designation Information</asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="4">
                                                                                                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"
                                                                                                            Height="40px"></asp:ValidationSummary>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="4">
                                                                                                        <asp:Label ID="lblTrainingTypeDetails" runat="server" CssClass="clsLabelHeader">Training Designation Details</asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="1">
                                                                                                        <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Training</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtTrainingName" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                                                                            Enabled="False" ToolTip="Training" MaxLength="25"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td colspan="2">
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    
                                                                                                    <%--<td align="right">
                                                                                                        <asp:Button ID="btnSaveTrainingDesg" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to Save Training Designation Information"
                                                                                                            Text="Save"></asp:Button>
                                                                                                    </td>--%>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="3">
                                                                                                        <asp:Label ID="Label8" runat="server" CssClass="clsLabelHeader">Training Designation List</asp:Label>
                                                                                                    </td>
                                                                                                    <td align="right">
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="4">
                                                                                                        <%--<div style="width:320px;">
                                                                                                            <table class="clsGrid"style="width:320px;" cellpadding="0" cellspacing="0" style="border-collapse:collapse;">
                                                                                                                <tr>
                                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                                        <span>Select</span>

                                                                                                                    </td>
                                                                                                                    <td width="270px" class="clsdgHeader">
                                                                                                                        <span class="clsdgHeader">Model Name</span>
                                                                                                                    </td>
                                                                                                   
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </div>--%>
                                                                                                        <div style="max-height:150px;overflow-y:auto ;overflow-x:hidden;width:338px">
                                                                                                        <asp:GridView ID="dgTrainingDesignationList" runat="server" 
                                                                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" 
                                                                                                            style="width:320px;" AutoGenerateColumns="False" ShowHeader="true" >
                                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <Columns>
                                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                                <asp:TemplateField HeaderText="Select">
                                                                                                                    <HeaderStyle HorizontalAlign="left" Width="50px"></HeaderStyle>
                                                                                                                    <ItemStyle HorizontalAlign="left" Width="50px"></ItemStyle>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="chkDesignation" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelect") %>'>
                                                                                                                        </asp:CheckBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="left"></FooterStyle>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField DataField="Name" HeaderText="Designation Name">
                                                                                                                    <HeaderStyle HorizontalAlign="left" Width="270px"></HeaderStyle>
                                                                                                                    <ItemStyle HorizontalAlign="left" Width="270px" Wrap="true" ></ItemStyle>
                                                                                                                </asp:BoundField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                       </div>
                                                                                                    </td>
                                                                                                    
                                                                                                </tr>
                                                                                                <%--<tr>
                                                                                                    
                                                                                                    <td colspan="4" align="right">
                                                                                                        <table id="Table11" height="100%" width="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                                                                            <tr>
                                                                                                                <td align="right" colspan="3">
                                                                                                                    <%--<asp:Button ID="btnSaveTrainingDesg" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save Training Designation Information"
                                                                                                                        Text="Save"></asp:Button>
                                                                                                                </td>
                           
                                                                                                                <td align="right">
                                                                                                                    <%--<asp:Button ID="btnCloseTrainingDesg" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close Training Designation Information screen"
                                                                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>--%>
                                                                                                <tr>
                                                                                                    <td align="right" colspan="5">
                                                                                                        <table id="tb" cellspacing="1" cellpadding="1" border="0">
                                                                                                            <tr>
                                                                                                                <td align="right" colspan="3">
                                                                                                                    <asp:Button ID="btnSaveTrainingDesg" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save Training Designation Information"
                                                                                                                        Text="Save"></asp:Button>
                                                                                                                    <td align="right">
                                                                                                                        <asp:Button ID="btnCloseTrainingDesg" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close Training Designation Information screen"
                                                                                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>

                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>    
                                                                                    
                                                            </asp:Panel>
                                                        </td>

                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField runat="server" ID="btnDummyControl2"/>
                                                            <asp:LinkButton ID="lnkTrainingOrgDetail" runat="server" CssClass="clsLinkButton"
                                                                ToolTip="Click to add Training Organization Detail" CausesValidation="False">Training Organization Detail</asp:LinkButton>
                                                            <cc2:ModalPopupExtender ID="lnkTrainingOrgDetail_ModalPopupExtender" 
                                                                runat="server" DynamicServicePath="" Enabled="True" Y=50 
                                                                TargetControlID="btnDummyControl2" PopupControlID="Panl2" BackgroundCssClass="clsModalPopupBG"></cc2:ModalPopupExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="Panl2" runat="server" align="center" style = "display:none;background-color: #FFFFFF;border-width: 1px;border-style: solid;width:auto;max-height:450px;overflow-x:hidden;overflow-y:auto;">
                                                                <table class="clstablelistout" id="Table12" border="0">
                                                                    <tr>
                                                                        <td colspan="1">
                                                                            <asp:Panel ID="Panel2" runat="server" CssClass="clspanel1">
                                                                                <table class="clstablelistin" id="Table13" border="0">
                                                                                    <tr>
                                                                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                                                                            <asp:Label ID="lblTitleTrainingOrg" runat="server" CssClass="clsFormHeader">Training Organization Detail</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" CssClass="clsValidationSummary">
                                                                                            </asp:ValidationSummary>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                            <asp:Label ID="Label9" runat="server" CssClass="clsLabelHeader">Training Organization Detail </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label10" runat="server" CssClass="clsLabelAuto">Training</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtOrganisationName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Training"
                                                                                                MaxLength="5" Enabled="False"></asp:TextBox>
                                                                                        </td>
                                                                                        <td colspan="2">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <%--<tr>
                                                                                        <td colspan="3">
                                                                                            <asp:Label ID="Label11" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="btnSaveTrainingOrg" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save Training Organization Information"
                                                                                                Text="Save"></asp:Button>
                                                                                        </td>
                                                                                    </tr>--%>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            <asp:Label ID="Label12" runat="server" CssClass="clsLabelHeader">Training Organization List</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                            <%--<div style="width:320px;">
                                                                                                <table class="clsGrid" cellpadding="0" cellspacing="0" style="border-collapse:collapse;width:320px;">
                                                                                                    <tr>
                                                                                                        <td width="50px" class="clsdgHeader">
                                                                                                            <span>Select</span>

                                                                                                        </td>
                                                                                                        <td width="270px" class="clsdgHeader">
                                                                                                        <span class="clsdgHeader">Training Organization</span>
                                                                                                        </td>
                                                                                                   
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>--%>
                                                                                            <div style="max-height:150px;overflow-y:auto;overflow-x:hidden;width:338px">
                                                                                            <asp:GridView ID="dgTrainingOrgDetailList" runat="server" ShowHeader="true" style="width:320px;" 
                                                                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                    <asp:TemplateField HeaderText="Select">
                                                                                                        <HeaderStyle Width="50px" HorizontalAlign="left" />
                                                                                                        <ItemStyle Width="50px" HorizontalAlign="left" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkTrainingOrg" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelect") %>'>
                                                                                                            </asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="Name" HeaderText="Training Organization">
                                                                                                         <HeaderStyle Width="270px" HorizontalAlign="left"  />
                                                                                                        <ItemStyle Width="270px" HorizontalAlign="left"  Wrap="true" />
                                                                                                    </asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                            </div>
                                                                                        </td>
                                                                                       
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4" align="right" valign="bottom">
                                                                                            <table id="Table14" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Button ID="btnSaveTrainingOrg" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Save Training Organization Information"
                                                                                                            Text="Save"></asp:Button>
                                                                                                    </td>
                                                                                                    <td>&nbsp</td>
                                                                                                    <td valign="bottom" align="right">
                                                                                                        <asp:Button ID="btnCloseTraningOrg" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close Training Organization screen"
                                                                                                            Text="Close" CausesValidation="False"></asp:Button>
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
                                                            </asp:Panel>
                                                        </td>

                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:HiddenField runat="server" ID="btnDummyControl3"/>
                                                            <asp:LinkButton ID="lnkTrainingModel" runat="server" CssClass="clsLinkButton" ToolTip="Click to add Training Model"
                                                                CausesValidation="False">Training Model</asp:LinkButton>
                                                            <cc2:ModalPopupExtender ID="lnkTrainingModel_ModalPopupExtender" 
                                                                runat="server" DynamicServicePath="" Enabled="True" Y="50" 
                                                                TargetControlID="btnDummyControl3" PopupControlID="Panl3" BackgroundCssClass="clsModalPopupBG"></cc2:ModalPopupExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="Panl3" runat="server" align="center" style = "display:none;background-color: #FFFFFF;border-width: 1px;border-style: solid;width:auto;max-height:450px;overflow-x:hidden;overflow-y:auto;">
                                                                <table class="clstablelistout" id="Table15" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="Panel3" runat="server" CssClass="clspanel1">
                                                                                <table class="clstablelistin" id="Table16" border="0">
                                                                                    <tr>
                                                                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                                                                            <asp:Label ID="lblTitleTrainingModel" CssClass="clsFormHeader" runat="server">Training Model Information</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                            <asp:ValidationSummary ID="ValidationSummary4" runat="server" CssClass="clsValidationSummary"
                                                                                                Height="40px"></asp:ValidationSummary>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                            <asp:Label ID="Label14" runat="server" CssClass="clsLabelHeader">Training Model Details</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="1">
                                                                                            <asp:Label ID="Label15" runat="server" CssClass="clsLabel">Training</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtModelName" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                                                                Enabled="False" ToolTip="Training" MaxLength="25"></asp:TextBox>
                                                                                        </td>
                                                                                        <td colspan="2">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            <asp:Label ID="Label16" runat="server" CssClass="clsLabelAuto" Visible="false">Click To Save Current Record</asp:Label>
                                                                                        </td>
                                                                                        <%--<td align="right">
                                                                                            <asp:Button ID="btnSaveTrainingModel" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to Save Training Model Information"
                                                                                                Text="Save"></asp:Button>
                                                                                        </td>--%>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            <asp:Label ID="Label17" runat="server" CssClass="clsLabelHeader">Training Model List</asp:Label>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                        <%--<div style="width:320px;">
                                                                                            <table class="clsGrid"style="width:320px;" cellpadding="0" cellspacing="0" style="border-collapse:collapse;">
                                                                                                <tr>
                                                                                                    <td width="50px" class="clsdgHeader">
                                                                                                        <span>Select</span>

                                                                                                    </td>
                                                                                                    <td width="270px" class="clsdgHeader">
                                                                                                        <span class="clsdgHeader">Model Name</span>
                                                                                                    </td>
                                                                                                   
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>--%>
                                                                                            <div  style="height:150px;overflow-y:scroll;overflow-x:hidden;width:338px">  
                                                                                            <asp:GridView ID="dgTrainingModelList" runat="server" ShowHeader="true"
                                                                                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"  style="width:320px;" 
                                                                                                ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"  >
                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                <Columns>
                                                                                                    <asp:BoundField Visible="false"  DataField="ID" HeaderText="ID">
                                                                                                    </asp:BoundField>
                                                                                                    <asp:TemplateField HeaderText="Select" Visible="true" >
                                                                                                        <HeaderStyle HorizontalAlign="left" Width="50px"></HeaderStyle>
                                                                                                        <ItemStyle HorizontalAlign="left" wrap="true" Width="50px"></ItemStyle>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="chkModel" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelected") %>'>
                                                                                                            </asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterStyle HorizontalAlign="left"></FooterStyle>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="Name" HeaderText="Model Name">
                                                                                                    <HeaderStyle HorizontalAlign="left" Width="270px"></HeaderStyle>
                                                                                                        <ItemStyle HorizontalAlign="left" wrap="true" Width="270px"></ItemStyle></asp:BoundField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                            </div>
                                                                                         
                                                                                        </td>
                                                                                       
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4" align="right" valign="bottom">
                                                                                            <table id="Table17" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                                                                <tr>
                                                                                                    <td align="right">
                                                                                                        <asp:Button ID="btnSaveTrainingModel" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save Training Model Information"
                                                                                                            Text="Save"></asp:Button>
                                                                                                    </td>
                                                                                                    <td>&nbsp
                                                                                                    </td>
                                                                                                    <td valign="bottom" align="right">
                                                                                                        <asp:Button ID="btnCloseTrainingModel" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close Training Model Information screen"
                                                                                                            Text="Close" CausesValidation="False"></asp:Button>
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
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>    
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <%--<tr>
                            <td style="width: 548px">
                                <asp:Label ID="lblSave" runat="server" CssClass="clsLabelAuto">Click To Save Current Record</asp:Label>
                            </td>
                            <td align="right">
                                <%-- AJAX Update Panel 
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                             <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to save Training"
                                            Text="Save" CausesValidation="true" ValidationGroup="1" ></asp:Button>    
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Search</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="Table4" border="0">
                                    <tr>
                                        <td width="9px">
                                                            
                                        </td>
                                        <td width="90px">
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search By</asp:Label>
                                        </td>
                                        <td>
                                            <%-- AJAX Update Panel --%>
                                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbSearchType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Name</asp:ListItem>
                                                                    <asp:ListItem Value="2">Training Type</asp:ListItem>
                                                                </asp:DropDownList>    
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFor" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50" Visible="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>    
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            
                                                            
                                        </td>
                                                        
                                    </tr>
                                </table>
                                
                                
                            </td>
                            <td align="right">
                                <%-- AJAX Update Panel --%>
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                          <%--  <asp:Button ID="btnFindNow" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to find the list of Training as per searching criteria"
                                    Text="Find Now" CausesValidation="False"></asp:Button> --%> 
                                        
                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                            ToolTip="Click to find the list of Training as per searching criteria" CausesValidation="False"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>      
                                                </td>    
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgTrainingList" runat="server" 
                                                    AllowSorting="True" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" 
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" EnableViewState="false"   ShowHeaderWhenEmpty="true" >
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"/>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Training Name">
                                                            <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TrainingTypeName" SortExpression="TrainingTypeName" HeaderText="Training Type">
                                                            <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Recurring Status">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "RecurringStatus") %>'
                                                                    Enabled="False"></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FreqInMonths" SortExpression="FreqInMonths" HeaderText="Freq In months">
                                                            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WarningDays" SortExpression="WarningDays" HeaderText="Warning Days">
                                                            <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <HeaderStyle HorizontalAlign="Left" />
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
                                                </td>
                                            </tr>
                                        </table>
                                                
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </td>
                           
                        </tr>
                        <%--<tr>
                             <td align="right" valign="bottom" colspan="2">
                                <%-- AJAX Update Panel 
                                            <table id="Table2" align="right" style="position: relative; width: 100%;">
                                                <tr>
                                                    <td valign="bottom" align="right">
                                                          <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                        <asp:Button ID="btnBack" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to close Training Information screen"
                                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                                </ContentTemplate>
                                                         </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>    
                            </td>    
                        </tr>--%>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
        <ProgressTemplate>
            <div class="clsAjaxLoader" style="height: 100%;width: 100%; left: 0; position: fixed; background-color: #000000;
                top: 0;  z-index: 99999;">
            </div>
            <div style="position: fixed; top: 50%; left: 50%; margin-left:-27px;margin-top:-27px;z-index: 100000; ">
                <div class="ext-el-mask-msg x-mask-loading">
                    <div class="clsLoad_ajax">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                            Height="48px" Width="48px"  /> 
                    </div>
                </div>
            </div> 
        </ProgressTemplate>
    </asp:UpdateProgress>
     <!-- Training Type --ModalPopUp -->
     <div style="display: none">
        <asp:Button runat="server" ID="btnDummyTrainingType" Text="Dummy Training Type" />
    </div>
    <asp:Panel runat="server" ID="Panl4" Style="display: none ">
        <div>
            <table class="clstablelistout" id="Table8">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upnlTrainingType" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="Table6" class="clstablelistin">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <asp:Label ID="lblTitleTrainingType" runat="server" CssClass="clsFormHeader">Training Type Information [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:ValidationSummary ID="ValidationSummary5" runat="server" CssClass="clsValidationSummary" ValidationGroup="4"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvTrainingTypeName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Training Type Required ."
                                                Display="None" ControlToValidate="txtTrainingTypeName" ValidationGroup="4">Training Type Required.</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3">
                                            <span ID="Label13" Class="clsLabelAuto">Click To Add New Record</span>
                                        </td>
                                        <%--<td align="right">
                                            <asp:Button ID="btnNewTrainingType" runat="server" CssClass="clsButton_Ajax" Text="New" CausesValidation="False"
                                                ToolTip="Click to Add the new TrainingType "></asp:Button>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="4">
                                            <span id="Label18" class="clsLabelHeader">Training Type Details</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <span id="Label19" class="clsLabelStar" style="color:Red;">*</span>
                                        </td>
                                        <td>
                                            <span id="Label20" class="clsLabel">Training Type</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTrainingTypeName" runat="server" CssClass="clsTextBoxTagSearch" 
                                                ToolTip="Enter TrainingType" MaxLength="49">
                                            </asp:TextBox>
                                        </td>
                                        <td align="right">
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="3">
                                            <span ID="Label21" Class="clsLabelAuto">Click To Save Current Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSaveTrainingType" runat="server" ValidationGroup="4" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to Save TrainingType Information">
                                            </asp:Button>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblSearchTrainingType" runat="server" CssClass="clsLabelHeader">Training Type List</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <%--<div style="width:420px;">
                                                <table class="clsGrid" cellpadding="0" cellspacing="0" style="border-collapse:collapse;width:420px;">
                                                    <tr>
                                                        <td style="width:300px" class="clsdgHeader">
                                                            <span>Training Type Name</span>
                                                        </td>
                                                        <td style="width:70px" class="clsdgHeader">
                                                            <span>Edit/View</span>
                                                        </td>
                                                         <td style="width:50px" class="clsdgHeader">
                                                            <span>Delete</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <div>
                                                <asp:GridView ID="dgTrainingType" runat="server" 
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                    AllowSorting="True" ShowHeader="true" style="width:420px;" AllowPaging="true" PageSize="5"
                                                    AutoGenerateColumns="False">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                    <PagerSettings  Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="TrainingTypeID"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Training Type Name">
                                                            <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                                            <ItemStyle Width="300px" />
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="70px" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                             <ItemStyle Width="50px" />
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
                                        <td colspan="4" align="right">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <%--<td align="right">
                                                            <asp:Button ID="btnNewTrainingType" runat="server" CssClass="clsbtnH clsinfoH1" Text="New" CausesValidation="False"
                                                                ToolTip="Click to Add the new TrainingType "></asp:Button>
                                                        </td>--%>
                                                    </td>
                                                    <%--<td align="right">
                                                        <asp:Button ID="btnSaveTrainingType" runat="server" ValidationGroup="4" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to Save TrainingType Information"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTrainingType" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                            CausesValidation="False" ToolTip="Click to close Training Type Information screen"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" align="right">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="tb2" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                            <asp:Button ID="btnNewTrainingType" runat="server" CssClass="clsbtnH clsinfoH1" Text="New" CausesValidation="False"
                                                                ToolTip="Click to Add the new TrainingType "></asp:Button>
                                                        </td>

                                                            <td align="right">
                                                        <asp:Button ID="btnSaveTrainingType" runat="server" ValidationGroup="4" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to Save TrainingType Information"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseTrainingType" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                            CausesValidation="False" ToolTip="Click to close Training Type Information screen"></asp:Button>
                                                    </td>
                                                        </tr>
                                                    </table>
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
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="TrainingType_ModalPopupExtender" runat="server" TargetControlID="btnDummyTrainingType"
        PopupControlID="Panl4" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForTrainingMaster();
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
                parent.IFrameTrainingMasterStateComplete();
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
    <%-- hide validation summary when server event occurs--%>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
            //Page_ClientValidate();
            // ValidationSummaryOnSubmit();
            //Page_IsValid=true;
            //            Page_ClientValidate();
            //            if (Page_IsValid) {
            //                $("#ValidationSummary1").css('display', 'none');
            //            }

            if ((typeof (Page_ClientValidate) == 'function')) {
                if (Page_ValidationActive) {
                    if (!ValidatorCommonOnSubmit()) {
                        return false;
                    }
                    else {
                        $(".clsValidationSummary").css('display', 'none');
                        //ValidationSummaryOnSubmit();

                    }
                }
            }
        });
    </script>
    <%-- End--%>
    </form>

</body>
</html>
