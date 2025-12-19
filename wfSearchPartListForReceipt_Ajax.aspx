<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchPartListForReceipt_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchPartListForReceipt_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <title>Part List For Goods Receipt</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
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
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblPartList" class="clsFormHeader">Part List For Goods Receipt</span>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                    ToolTip="Click to go back to the previous page" Text="Back" CausesValidation="False">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabel">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblDescription" class="clsLabel">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"></asp:TextBox>
                                                    </td>
                                                    <td align="right">
                                                        <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                            ToolTip="Click to find the list of Part as per searching criteria"></asp:Button>--%>
                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="images/Search2.png" CssClass="clsSearch2btn" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="width: 100%">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True"></asp:Label>
                                            </div>
                                            <div style="width: 100%">
                                                <asp:GridView ID="gdvItem" EnableViewState="false" runat="server" CssClass="clsGridNewStyle"
                                                    Width="100%" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowPaging="True"
                                                    PageSize="25" AllowSorting="True" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part No.">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AlternatePartPresent" SortExpression="AlternatePartPresent"
                                                            HeaderText="Alternate Part Present">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                            <HeaderStyle HorizontalAlign="Left" ForeColor="Blue" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField Visible="False" DataField="QtyRemovedFromAircraft" HeaderText="QtyRemovedFromAircraft">
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <asp:Panel ID="PnlPaging" runat="server">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <table border="0" cellpadding="2" cellspacing="1" align="right">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label Text="" EnableViewState="false" runat="server" ClientIDMode="Static" ID="valuetodisplay"
                                                                                class="letterbox" />
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnfirstpage" class="first" onclick="setValue(0);" title="Move First">
                                                                            </span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnprevpage" onclick="setValue(1);" class="prev" title="Move Previous">
                                                                            </span>
                                                                        </td>
                                                                        <td align="center">
                                                                            <div align="center">
                                                                                <asp:TextBox runat="server" Text="" ID="Slidercontrol">
                                                                                </asp:TextBox>
                                                                                <cc2:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="Slidercontrol"
                                                                                    Minimum="-100" Maximum="100" BoundControlID="txtPageDisplay" EnableHandleAnimation="true"
                                                                                    Length="300" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnnextvpage" onclick="setValue(2);" class="next" title="Move Next"></span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnlastpage" onclick="setValue(3);" class="last" title="Move Last"></span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtPageDisplay" ToolTip="Enter page no." CssClass="clsTextBoxMegaSmall_Ajax" />
                                                                        </td>
                                                                        <td>
                                                                            <span>of </span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label Text="" ID="lblpagecount" CssClass="clsLabelHeader" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <div>
                                                                                <asp:Button ID="btnGridPaging" CssClass="clsButtonPlus_Ajax" runat="server" Text="Go" />
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <!--End-->
                            </tr>
                            <tr>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
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
    </div>
    </form>
    <!-- Slider control events  -->
    <script type="text/javascript">
        //initialize slider control and attach events
        function pageLoad(sender, e) {
            var slider = $find('<%=SliderExtender1.ClientID %>');
            if (slider) {
                slider.add_slideStart(sliderStart);
                slider.add_slideEnd(sliderEnd);
                slider.add_valueChanged(valChanged);
            }
        }

            
    </script>
    <script type="text/javascript">
        function valChanged() {
            var showval = $('#valuetodisplay');
            var curval = $('#<%=Slidercontrol.ClientID %>');
            showval.html(curval.val());
        }
       
        
    </script>
    <script type="text/javascript">

        function sliderStart() {
            $('#valuetodisplay').css('display', 'inline-block');
        }
    </script>
    <script type="text/javascript">
        function sliderEnd() {
            $('#valuetodisplay').css('display', 'none');

        }
    </script>
    <script type="text/javascript">
        function setValue(val) {
            if (val === 0) {//first
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var minval = slider.get_Minimum();
                $('#<%=txtPageDisplay.ClientID %>').val(minval);
                $('#<%=Slidercontrol.ClientID %>').val(minval);
                slider.set_Value(minval);


            }
            else if (val === 1) {//prev
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval - 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);


            }
            else if (val === 2) {//next
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval + 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);
                //                            sliderStart();
                //                            valChanged();
                //                            sliderEnd();

            }
            else if (val === 3) {//last
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var maxval = slider.get_Maximum();
                $('#<%=txtPageDisplay.ClientID %>').val(maxval);
                $('#<%=Slidercontrol.ClientID %>').val(maxval);
                slider.set_Value(maxval);
            }
        }
    </script>
    <!-- End  -->
</body>
</html>
