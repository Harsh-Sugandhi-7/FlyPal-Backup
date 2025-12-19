<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPlace_Ajax.aspx.vb"
    Inherits="Flypal.wfPlace_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Place</title>
    <script language="javascript" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <style type="text/css">
        .hideGridColumn {
            display: none;
        }
    </style>
    <%-- <script type="text/jscript">
        function SetCityValue() {
            var dd = $get("cmbCity");
            $get('CityValue').value = dd.options[dd.selectedIndex].value;

        }
    </script>
    <script type="text/jscript">
        function SetSearchCityValue() {
            var dd = $get("cmbSearchCity");
            $get('SearchCityValue').value = dd.options[dd.selectedIndex].text;
        }
    </script>--%>
    <script type="text/javascript">
        function SetCityValue(elem, combo) {
            switch (combo) {
                //City Value  
                case 'City':
                    var id = $(":selected", elem).val();
                    $("#CityValue").val(id);
                    break;
                case 'SearchCity':
                    var text = $(":selected", elem).text();
                    var id = $(":selected", elem).val();
                    if (text == "(SELECT)") {
                        $("#SearchCityValue").val("");
                    }
                    else {
                        $("#SearchCityValue").val(text);

                    }

                    $("#SearchCityID").val(id);
                    break;
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
        <script src="js/query-1.7.1.js" type="text/javascript"></script>
        <script type="text/javascript" language="javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

                var gridHeader = $('#<%=dgGridView.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
                $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
                $('#<%=dgGridView.ClientID%> tr th').each(function (i) {
                    // Here Set Width of each th from gridview to new table(clone table) th 
                    $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width() + 1).toString() + "px");
                });
                $("#GHead").append(gridHeader);
                $('#GHead').css('position', 'absolute');
            //    $('#GHead').css('top', $('#<%=dgGridView.ClientID%>').offset().top);

            });
        </script>
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <asp:UpdatePanel ID="upnlNew" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="clstablelistout" id="tblmain">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                                    <table id="tblInner" class="clstablelistin">
                                        <tr>
                                            <td>
                                                <div>
                                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="clsFormHeader1" style="width: 400px">
                                                                        <table width="100%" >
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Place [New]</asp:Label>
                                                                                </td>
                                                                                <asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <td align="right">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                                            Text="New" ToolTip="Click to add new Place" />
                                                                                                    </td>

                                                                                                    <td>
                                                                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                                            Text="Close" ToolTip="Click to close Place screen" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>

                                                                                        </td>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                                            ValidationGroup="a"></asp:ValidationSummary>
                                                                        <asp:RequiredFieldValidator ID="rfvCode" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtCode"
                                                                            Display="None" ErrorMessage="Code Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
                                                                            Display="None" ErrorMessage="Place Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                        <asp:CustomValidator ID="cvCity" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbCity"
                                                                            Display="None" ErrorMessage="Select city from the list." ClientValidationFunction="ValidateCity"
                                                                            ValidationGroup="a"></asp:CustomValidator>
                                                                        <script type="text/javascript">
                                                                            function ValidateCity(source, args) {
                                                                                args.IsValid = false;
                                                                                var dd = $get("cmbCity");
                                                                                if (dd.selectedIndex != 0) {
                                                                                    args.IsValid = true;
                                                                                    return;
                                                                                }
                                                                            }
                                                                        </script>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div>
                                                    <asp:UpdatePanel runat="server" ID="upnlPlaceDetails" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <%--  <td>
                                                                <span id="spAdd" class="clsLabelAuto">Click To Add New Record </span>
                                                            </td>--%>
                                                                </tr>
                                                                <%--  <tr>
                                                            <td colspan="2">
                                                                <span id="spPlaceDetails" class="clsLabelHeader">Place Details</span>
                                                            </td>
                                                        </tr>--%>
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 100%">
                                                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-top: -5px">

                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td width="8">
                                                                                            <span id="spCode1" class="clsLabelStar">*</span>
                                                                                        </td>
                                                                                        <td width="68">
                                                                                            <span id="spCode" class="clsLabelAuto">Short Name</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="5"
                                                                                                Text="<%# mPlace.Code %>" ToolTip="Enter Code"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="8">
                                                                                            <span id="Span1" class="clsLabelStar"></span>
                                                                                        </td>
                                                                                        <td width="68">
                                                                                            <span id="Span2" class="clsLabelAuto">ICAO</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtICAO" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="5"
                                                                                                Text="<%# mPlace.ICAO %>" ToolTip="Enter ICAO"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="8">
                                                                                            <span id="spName1" class="clsLabelStar">*</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="spName" class="clsLabelAuto">Name</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mPlace.Name %>"
                                                                                                ToolTip="Enter Place Name"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="8">
                                                                                            <span id="lblCity2" class="clsLabelStar">*</span>
                                                                                        </td>
                                                                                        <td width="68">
                                                                                            <span id="lblCity" class="clsLabelAuto">City</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:DropDownList ID="cmbCity" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="Name"
                                                                                                DataValueField="ID" SelectedValue="<%# mPlace.CityID %>" EnableViewState="false"
                                                                                                onChange="SetCityValue(this,'City')" ClientIDMode="Static">
                                                                                            </asp:DropDownList>
                                                                                            <%--   <asp:Button ID="imgbtnCity" runat="server" CausesValidation="False" CssClass="clsButtonGrid_Ajax"
                                                                                        Text="..." ToolTip="Click to Add New City" />--%>
                                                                                            <asp:ImageButton ID="imgbtnCityNew" runat="server" CausesValidation="False" CssClass="clsButtonImg" Width="24px" Height="20px"
                                                                                                ImageUrl="~/images/plus1.png" ToolTip="Click to Add New City" />
                                                                                            <asp:Button ID="hdnimgBtnCityMain" ClientIDMode="Static" runat="server" Text="..."
                                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:HiddenField runat="server" ID="CityValue" ClientIDMode="Static" />
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to save the Place Information"
                                                                                                ValidationGroup="a" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <%-- <td colspan="3">
                                                                                    <span id="spSave" class="clsLabelAuto">Click To Save Current Record</span>
                                                                                </td>--%>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <%-- <tr>
                                                            <td colspan="2">
                                                                <span id="spSearch" class="clsLabelHeader">Search . . .</span>
                                                            </td>
                                                        </tr>--%>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-top: 5px">
                                                    <div style="width: 100%">
                                                        <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td width="12">&nbsp;
                                                                        </td>
                                                                        <td width="68">
                                                                            <span id="spPlace" class="clsLabelAuto">Place</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                                ToolTip="Enter Place Name"></asp:TextBox>
                                                                        </td>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                        <td width="12">&nbsp;
                                                                        </td>
                                                                        <td width="68">
                                                                            <span id="spCity1" class="clsLabelAuto">City</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbSearchCity" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="Name"
                                                                                DataValueField="ID" EnableViewState="false" onChange="SetCityValue(this,'SearchCity')">
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField runat="server" ID="SearchCityValue" ClientIDMode="Static" />
                                                                            <asp:HiddenField runat="server" ID="SearchCityID" ClientIDMode="Static" />
                                                                        </td>
                                                                        <td align="right">
                                                                            <%-- <asp:Button ID="btnFindNow" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                                        Text="Find Now" ToolTip="Click to find the list of Place as per searching criteria" />--%>
                                                                            <asp:ImageButton ID="btnImgFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of Place as per searching criteria." />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div>
                                                        <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div id="GHead" style="overflow: auto; z-index: 3; position: relative;">
                                                                            </div>
                                                                            <div style="height: 275px; overflow: auto; width: 100%">
                                                                                <asp:GridView ID="dgGridView" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                                                    CellPadding="5" ForeColor="Black" GridLines="Horizontal" CssClass="clsGridNewStyle" PageSize="25" ShowHeaderWhenEmpty="True">
                                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                    <PagerStyle HorizontalAlign="Right" />
                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                                            ItemStyle-CssClass="hideGridColumn">
                                                                                            <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                                            <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Code" HeaderText="Short Name">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <%--  Ajay 28-Des-2022--%>
                                                                                        <asp:BoundField DataField="ICAO" HeaderText="ICAO">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <%-- ---------%>
                                                                                        <asp:BoundField DataField="Name" HeaderText="Name">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CityName" HeaderText="City">
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Is Day Light">
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsDayLight") %>'
                                                                                                    Enabled="False" />
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <%--<asp:ButtonField CommandName="EditView" HeaderText="Edit/View" Text="Edit/View" />
                                                                                <asp:ButtonField CommandName="Remove" HeaderText="Delete" Text="Delete" />--%>
                                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                            <%--6--%>
                                                                                            <ItemTemplate>
                                                                                                <div class="dropdown">
                                                                                                    <div class="dropdownbtn-content">
                                                                                                        <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                        CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                                        Style="cursor: pointer" />
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                        </asp:TemplateField>
                                                                                        <%--7--%>
                                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                            DataField="IsSyncFromCRS" HeaderText="IsSyncFromCRS"></asp:BoundField>
                                                                                    </Columns>
                                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
        <!-- City Main Popup -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyCityMain" Text="Dummy City Main" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlCityMain" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupCityMain" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupCityMain" runat="server" TargetControlID="btnDummyCityMain"
            PopupControlID="pnlCityMain" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameCityMainComplete() {
                $("#btnDummyCityMain").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            $(document).ready(function () {
               
                $("#imgbtnCityNew").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupCityMain").attr("src", "wfCityMain_Ajax.aspx?Type=1&Typepup=pup");
                        if (!$.browser.msie) {
                            $("#btnDummyCityMain").click();
                            $get("AjaxLoader").style.visibility = "hidden";
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }
                });
            });
        </script>
        <script type="text/javascript">
            function ParentCallBackFunction() {
                var atawindow = $find("<%=mdlPopupCityMain.ClientID %>");
                //close ata popup window
                atawindow.hide();
                $("#iPopupCityMain").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnimgBtnCityMain").click();
            }
        </script>
        <!-------------------->
         <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackPlaceFunction();
            return false;
        }
    </script>
    <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Typepup") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFramePlaceComplete();
                }
            });

    <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("Typepup") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
              //  onResize();//for Top bottom link
           <% End if %>
            }
            function ReSetPageLayout() {
                $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
    </form>
</body>
</html>
