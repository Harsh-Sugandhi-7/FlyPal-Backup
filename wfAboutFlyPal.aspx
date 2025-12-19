<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAboutFlyPal.aspx.vb"
    Inherits="Flypal.wfAboutFlyPal" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>About FlyPal</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var popID = $('a.poplight[href^=#]').attr('rel');
            var popURL = $('a.poplight[href^=#]').attr('href');
            var query = popURL.split('?');
            var dim = query[1].split('&amp;');
            var popWidth = dim[0].split('=')[1];
            var tempwidth = $('#' + popID).outerWidth(true);

            $('#' + popID).fadeIn().css({ 'width': Number(tempwidth) }).prepend('<a href="#" class="close"><img src="images/close2.png" style="position:relative;margin: -47 -46 0 782;" class="btn_close" title="Close" alt="Close" /></a>');

            var popMargTop = ($('#' + popID).height() + 80) / 2;
            var popMargLeft = ($('#' + popID).width() + 80) / 2;

            $('#' + popID).css({
                'margin-top': 25,
                'margin-left': -popMargLeft
            });

            $pos = $('#' + popID).position();
            var top = $pos.top;
            var body = $('#' + popID).outerHeight(true);

            var temp = top + body + 50;
            var hight = ($(window).height() > temp ? $(window).height() : temp);

            $('body').append('<div id="fade"></div>');
            $('#fade').css({ 'filter': 'alpha(opacity=70)', 'width': '100%', 'height': hight, 'background': '#000' }).fadeIn();
            return false;
        });
        $('a.close').live('click', function () {
            $('#fade , .popup_block').fadeOut(function () {
                $('#fade, a.close').remove()
                //Commented and added By Vikrant on 01-Dec-2021 for PBH
                //                var url = "index.aspx";
                //                $(location).attr('href', url);
                PageMethods.SignOut(Success, Failure);

                function Success(result) {
                    if (result != '') {
                        window.location.href = result;
                    }
                    else {
                        window.location.href = 'index.aspx';
                    }
                }
                function Failure(result) {
                    window.location.href = 'index.aspx';
                }
                //End
            });
            return false;
        });
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="scriptmanager" runat="server" EnablePageMethods="true" />
    <!--added By Vikrant on 01-Dec-2021 for PBH-->
    <!-- ================================================================ -->
    <a class="poplight" href="#?w=500" rel="popup_name"></a>
    <div id="popup_name" class="popup_block" align="center">
        <table id="tblmain" class="clstable1" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">FlyPal®</asp:Label>
                                </td>
                                <td align="left">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Logo/ClientLogo.jpg" Height="70px"
                                        Width="130px"></asp:Image>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table id="Table4" border="0" cellspacing="1" cellpadding="1">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLicense" runat="server" CssClass="clsLabelHeader">License:</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table id="Table6" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFlyPalVersion1" runat="server" CssClass="clsLabelHeader">Flypal Ver. :</asp:Label>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lblFlyPalVersion" runat="server" CssClass="clsLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblReleaseNo1" runat="server" CssClass="clsLabelHeader">Release No. :</asp:Label>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lblReleaseNo" runat="server" CssClass="clsLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLastUpdatedDate1" runat="server" CssClass="clsLabelHeader">Last Updated Date :</asp:Label>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lblLastUpdatedDate" runat="server" CssClass="clsLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblClientCode" class="clsLabelHeader">Client Code :</span>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lblCode" runat="server" CssClass="clsLabel"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRegisteredTo" runat="server" CssClass="clsLabelHeader" Width="104px">Registered To :</asp:Label>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lbk" runat="server" CssClass="clsLabel" Width="230px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblSubscriptionvalidtill" runat="server" CssClass="clsLabelHeader"
                                                    Width="152px"></asp:Label>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lblSubscription" runat="server" CssClass="clsLabelauto" Width="230px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 20px">
                                                <asp:Label ID="lblDaysRemaining1" runat="server" CssClass="clsLabelHeader">Days Remaining : </asp:Label>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lblDaysRemaining" runat="server" CssClass="clsLabelauto"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAircraftLicense1" runat="server" CssClass="clsLabelHeader">Aircraft(s) License :</asp:Label>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lblAircraftLicense" runat="server" CssClass="clsLabelauto"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom">
                                                <asp:Label ID="lblUserLicense1" runat="server" CssClass="clsLabelHeader">User(s) License : </asp:Label>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Label ID="lblUserLicense" runat="server" CssClass="clsLabelauto"></asp:Label>
                                            </td>
                                            <td valign="bottom">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="dgPBHList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                        PageSize="25">
                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                        <RowStyle CssClass="clsdgItem" />
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RegNo" HeaderText="Aircraft">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="HoursFrequencyText" HeaderText="Subscribed Hours">
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                             <asp:BoundField DataField="CarryForwardHoursText" HeaderText="Carry Forward Hours">
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                             </asp:BoundField>
                                            <asp:BoundField DataField="RemainingHoursText" HeaderText="Remaining Hours">
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DaysFrequency" HeaderText="Subscription Days">
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days">
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ValidUptoFormatted" HeaderText="Valid Upto">
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RemainingDays">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RemainingHoursDec">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="HoursFrequencyDec">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblCompanyDetails" runat="server" CssClass="clsLabelHeader">Disclaimer:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:TextBox ID="txtCompName" runat="server" CssClass="clsTextBoxMultilineDefectActionAuto"
                                        Width="760px" ReadOnly="True" MaxLength="50" TextMode="MultiLine">BytzSoft Technologies Pvt. Ltd. makes no representations or warranties about the suitability of the software, either express or implied, including but not limited to the implied warranties of merchantability, fitness for a particular purpose, or non-infringement. BytzSoft Technologies Pvt. Ltd. shall not be liable for any damages suffered by licensee as a result of using, modifying or distributing this software or its derivatives.</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblSave" runat="server" CssClass="clsLabelHeader">Copyrights ©:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="clsTextBoxMultilineDefectActionAuto"
                                        Width="760px" ReadOnly="True" MaxLength="50" TextMode="MultiLine">This software and documentation is the confidential and proprietary information of BytzSoft Technologies Pvt. Ltd. You shall not disclose such Confidential Information and shall use it only in accordance with the terms of the license agreement you entered into with BytzSoft Technologies Pvt. Ltd.</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Contact Us:</asp:Label>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <table id="Table5" border="0" cellspacing="1" cellpadding="1">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">BytzSoft Technologies Pvt. Ltd.</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" CssClass="clsLabelauto">Email: support@bytzsoft.com</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblWebsite" runat="server" CssClass="clsLabelauto">Website:</asp:Label><a
                                                    title="BytzSoft Technologies" href="http://www.bytzsoft.aero" target="_blank"><font
                                                        size="2" face="Verdana">www.bytzsoft.aero</font></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" CssClass="clsLabelauto"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" CssClass="clsLabelauto">All rights reserved.</asp:Label>
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
    </div>
    </form>
    <script>
        var d = new Date();
        document.getElementById("Label7").innerHTML = 'Copyrights @ ' + d.getFullYear() + ' Bytzsoft Technologies Pvt. Ltd.';
    </script>
</body>
</html>
