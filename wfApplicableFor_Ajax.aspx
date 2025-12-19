<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfApplicableFor_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfApplicableFor_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Applicable For</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table class="clstablelistin" id="tblLedgerList">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <span id="lblModelList" class="clsFormHeader">Applicability Model List For Part No.[<%=mItem.Name %>]</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" ValidationGroup="1" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <%-- <asp:CustomValidator ID="cvTypeList" runat="server" Display="None" ControlToValidate="cmbTypeList"
                                                ErrorMessage="Select Type from the List" ClientValidationFunction="ValidateType"></asp:CustomValidator>--%>
                                                <asp:CustomValidator ID="cvModelList" runat="server" Display="None" ControlToValidate="cmbModelList" ValidationGroup="1"
                                                    ErrorMessage="Select Model from the List" ClientValidationFunction="ValidateModel"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <%-- <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTabs" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPartInformation" runat="server" ToolTip="Click to open the Part Information"
                                                            CssClass="clsButtonLong_Ajax" CausesValidation="false" Text="Part Information"
                                                            EnableViewState="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAlternatePart" CausesValidation="false" runat="server" ToolTip="Click to open the Alternate Part List"
                                                            CssClass="clsButtonLong_Ajax" Text="Alternate Part" EnableViewState="False">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <span id="lblApplicability" title="Current page of Aircraft Status Detail" class="clsLabelButton1">
                                                            Applicability</span>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnOpeningStock" CausesValidation="false" runat="server" ToolTip="Click to open the Opening Stock List"
                                                            CssClass="clsButtonLong_Ajax" Text="Opening Stock" EnableViewState="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset runat="server" class="clsFieldSetNewStyle" >
                                                <table border="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="lblModels" class="clsLabel">Models</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbTypeList" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                AutoPostBack="True" DataValueField="ID" DataTextField="Name">
                                                                <asp:ListItem Value="0">None</asp:ListItem>
                                                                <asp:ListItem Value="2">Aircraft</asp:ListItem>
                                                                <asp:ListItem Value="10">Engine</asp:ListItem>
                                                                <asp:ListItem Value="9">Ground Equipment</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <%-- <asp:Button ID="imgbtnModels" ValidationGroup="1" runat="server" ToolTip="Click to Add New Model" ClientIDMode="Static" CssClass="clsButtonGrid_Ajax"
                                                            Text="..." CausesValidation="False"></asp:Button>--%>
                                                            <asp:ImageButton ID="imgbtnModelsNew" runat="server" ImageUrl="~/images/plus1.png"
                                                                Height="22px" Width="24px" ToolTip="Click to Add Model" CausesValidation="True"></asp:ImageButton>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbModelList" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                DataValueField="Id" EnableViewState="false" DataTextField="ModelName" onchange="setModelIdName(this)">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <table id="Table1">
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnAddNew" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add Model and its Type in the List"
                                                                            Text="Add"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chkGroundSupportEquipment" runat="server" CssClass="clsCheckBox"></asp:CheckBox>
                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Ground Support Equipment</asp:Label>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Button ID="hdnbtnModel" ValidationGroup="1" ClientIDMode="Static" runat="server" Text="..."
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                                    </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                            <ContentTemplate>
                                               <%-- <div style="width: 100%; margin-bottom: 3px;">
                                                   
                                                </div>--%>
                                                <fieldset runat="server" class="clsFieldSetNewStyle" >
                                                    <legend > <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List Of Applicable Models : Record(s).</asp:Label></legend>
                                                <div style="width: 100%;">
                                                    <asp:GridView ID="gdvItemApplicables" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                        DataKeyNames="ID" ForeColor="Black" GridLines="Horizontal" PageSize="25" ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle BackColor="white" forecolor="black" CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ModelType" HeaderText="Model Type">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ModelName" HeaderText="Model Name">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ground Support Equipment">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBox2" runat="server" CssClass="clsCheckBox" Checked='<%# DataBinder.Eval(Container.DataItem, "GroundSupportEquipment") %>'
                                                                        Enabled="False"></asp:CheckBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="Remove">
                                                                <HeaderStyle HorizontalAlign="Left"  />
                                                                <ItemStyle ForeColor="blue" />
                                                            </asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                    </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <table class="clstableButton" align="right">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnBack" ValidationGroup="1" CausesValidation="false" runat="server" CssClass="clsbtnH clsinfoH1"
                                                        ToolTip="Click to go back to the previous page" Text="Back"></asp:Button>
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
        <asp:HiddenField ID="ModelIDValue" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="ModelNameValue" runat="server" ClientIDMode="Static" />
        <%-- Client side validation for comboboxes--%>
        <script type="text/javascript">
            //Model List
            function ValidateModel(source, args) {
                args.IsValid = false;
                var dd = $get("cmbModelList");
                if (dd.selectedIndex != 0) {
                    args.IsValid = true;
                    return;

                }

            }

            //Type List
            //        function ValidateType(source, args) {
            //            args.IsValid = false;
            //            var dd = $get("cmbTypeList");
            //            if (dd.selectedIndex != 0) {
            //                args.IsValid = true;
            //                return;

            //            }

            //        }

        </script>
        <!-- javascript function to set combobox selected value to appropriate hidden field-->
        <script type="text/javascript">
            function setModelIdName(elem) {
                var id = $(":selected", elem).val();
                var text = $(":selected", elem).text();
                //set id and text to hidden fields
                $("#ModelIDValue").val(id);
                $("#ModelNameValue").val(text);
            }
        </script>

        <!-- Select Model popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyModel" Text="TaskCard Tool" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlModel" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeModel" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupModel" runat="server" TargetControlID="btnDummyModel"
            PopupControlID="pnlModel" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameModelStateComplete() {
                $("#btnDummyModel").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenModelWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeModel").attr("src", "wfModel_Ajax.aspx?OpenAs=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyModel").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForModel() {
                var Modelwindow = $find("<%=mdlPopupModel.ClientID %>");
                //close Task Card Tool popup window
                Modelwindow.hide();
                //           release resources
                $("#IframeModel").attr("src", "JavaScript:''");
                //call image button

                $("#hdnbtnModel").click();
            }
        </script>
        <!-- End-->
        <%-- hide validation summary when server event occurs--%>
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {

                if ((typeof (Page_ClientValidate) == 'function')) {
                    if (Page_ValidationActive) {
                        if (!ValidatorCommonOnSubmit()) {
                            return false;
                        }
                        else {
                            $(".clsValidationSummary").css('display', 'none');


                        }
                    }
                }
            });
        </script>
        <%-- End--%>
    </form>

</body>
</html>
