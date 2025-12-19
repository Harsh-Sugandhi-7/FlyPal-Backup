<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAircraftSpecificationReport_Ajax.aspx.vb" Inherits="Flypal.wfrptAircraftSpecificationReport_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Specification Report</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <span ID="lbltitle" Class="clstitle1">Aircraft Specification Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Pleaes Select the Aircraft"
                                        ControlToValidate="cmbMachine" Display="None" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                    
                                    <%-- Client side validation for comboboxes--%>
                                    <script type="text/javascript">
                                        //Aircraft List
                                        function ValidateAircraft(source, args) {
                                            args.IsValid = false;
                                            var dd = $get("cmbMachine");
                                            if (dd.selectedIndex != 0) {
                                                args.IsValid = true;
                                                return;

                                            }

                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span ID="lblStep2" Class="clsLabelHeader">Step I. Selection of Month and Year</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left">
                                    <span ID="lblYear" Class="clsLabelAuto">Month and Year</span>
                                </td>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbMonth" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                 <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbYear" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span ID="lblStep3" Class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span ID="lblAircraftStar1" Class="clsLabelStar">*</span>
                                </td>
                                <td>
                                    <span ID="lblAircraft" Class="clsLabelAuto">Aircraft</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbMachine" runat="server" DataTextField="RegNo"
                                        DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span ID="lblStep4" Class="clsLabelHeader">Step III. Display Report</span>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto" Visible="False">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblyear1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            
                            <tr>
                                <td align="left">
                                </td>
                                <td colspan="2" align="left">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                            ToolTip="Click to Display Current Searching criterias" Text="Current Criteria"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server"  ToolTip="Click to Display Report"
                                                            Text="Display"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" ToolTip="Click to close the Aircraft Specification Report screen"
                                                            Text="Close" CausesValidation="False"></asp:Button>
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </form>
</body>
</html>
