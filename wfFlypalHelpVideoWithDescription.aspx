<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFlypalHelpVideoWithDescription.aspx.vb"
    Inherits="Flypal.wfFlypalHelpVideoWithDescription" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <link href="musicshow/css/styles.css" rel="stylesheet" type="text/css" />
    <link href="moviehunter/css/style.css" rel="stylesheet" type="text/css" />
    <link href="moviehunter/css/ie6.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <%--<div id="shell">--%>
    <div id="sub-navigation">
        <ul>
            <li><a href="#">
                <asp:Label ID="lblFlypalVideoHelp" runat="server" Text="Flypal Help Video" Style="font-size: large;"
                    ForeColor="white"></asp:Label></a></li>
        </ul>
        <div id="search">
            <label for="search-field">
                SEARCH</label>
            <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtSearch" runat="server" class="blink search-field" AutoPostBack="true"> </asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="rightCol">
        <div id="videoBlock">
            <div id="videoBlockTop">
                <h3>
                    VIDEOS</h3>
                <p>
                    List of videos</p>
            </div>
            <div id="videoBlockBody">
                <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="dgGridView" runat="server" AutoGenerateColumns="False" PageSize="9"
                            AllowPaging="true" ShowHeaderWhenEmpty="True" ShowHeader="false" DataKeyNames="ID,VideoPath,SrNo"
                            BorderStyle="None" BorderColor="Black">
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last"
                                Visible="false" />
                            <PagerStyle HorizontalAlign="Center" />
                            <RowStyle BorderStyle="None"></RowStyle>
                            <HeaderStyle />
                            <AlternatingRowStyle BorderStyle="None"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                    ItemStyle-CssClass="hideGridColumn">
                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="vidBox">
                                            <div class="leftBox">
                                                <p class="light">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("VideoName") %>'></asp:Label>
                                                </p>
                                            </div>
                                            <p class="rightBox">
                                                <asp:ImageButton ID="EditView" runat="server" CommandName="VideoView" Width="81"
                                                    Height="38" border="0" alt="" ImageUrl='<%# Eval("ThumbnailPath") %>' CommandArgument='<%# Container.DataItemIndex %>' /></p>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="VideoPath" HeaderStyle-CssClass="hideGridColumn" HeaderText="VideoPath"
                                    ItemStyle-CssClass="hideGridColumn">
                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SrNo" HeaderStyle-CssClass="hideGridColumn" HeaderText="SrNo"
                                    ItemStyle-CssClass="hideGridColumn">
                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="videoBlockBot">
        </div>
    </div>
    <div id="centrCol" class="banr">
        <div class="movie-image">
            <div style="margin-left: 12px;">
                <asp:UpdatePanel runat="server" ID="upnlVideo" UpdateMode="Conditional">
                    <ContentTemplate>
                        <video width="1000" controls controlslist="nodownload" id="Vediosource" runat="server">
                                    <source type="video/mp4" >
                                   </video>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="upnlVideoGridView" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="dgGridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                            Height="80px" DataKeyNames="id" PageSize="1" AllowPaging="True" ShowHeader="false"
                            BorderStyle="None" GridLines="None">
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <video width="1000" controls controlslist="nodownload" src="<%# Eval("VideoPath") %>">
                          
                                                                        </video>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Previous" />
                            <PagerStyle CssClass="text-right" HorizontalAlign="Right" ForeColor="White" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="footer">
        <asp:UpdatePanel runat="server" ID="upnlBelowList" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="coming">
                    <div class="head">
                    </div>
                    <div class="content">
                        <h4>
                            <asp:Label ID="lblVideoName1" runat="server"></asp:Label>
                        </h4>
                        <a href="#" runat="server" id="a1">
                            <img src="" height="50" width="100" alt="Image" class="mr-3" runat="server" id="img1" /></a>
                        <p>
                            <asp:Label ID="lblDescription1" runat="server"></asp:Label></p>
                    </div>
                    <div class="cl">
                        &nbsp;</div>
                    <div class="content">
                        <h4>
                            <asp:Label ID="lblVideoName2" runat="server"></asp:Label>
                        </h4>
                        <a href="#" runat="server" id="a2">
                            <img src="" height="50" width="100" alt="Image" class="mr-3" runat="server" id="img2" /></a>
                        <p>
                            <asp:Label ID="lblDescription2" runat="server"></asp:Label>
                        </p>
                    </div>
                    <div class="content">
                        <h4>
                            <asp:Label ID="lblVideoName3" runat="server"></asp:Label>
                        </h4>
                        <a href="#" runat="server" id="a3">
                            <img src="" height="50" width="100" alt="Image" class="mr-3" runat="server" id="img3" /></a>
                        <p>
                            <asp:Label ID="lblDescription3" runat="server"></asp:Label></p>
                    </div>
                    <div class="cl">
                        &nbsp;</div>
                    <div class="content">
                        <h4>
                            <asp:Label ID="lblVideoName4" runat="server"></asp:Label>
                        </h4>
                        <a href="#" runat="server" id="a4">
                            <img src="" height="50" width="100" alt="Image" class="mr-3" runat="server" id="img4" /></a>
                        <p>
                            <asp:Label ID="lblDescription4" runat="server"></asp:Label>
                        </p>
                    </div>
                    <div class="cl">
                        &nbsp;</div>
                    <div class="content">
                        <h4>
                            <asp:Label ID="lblVideoName5" runat="server"></asp:Label>
                        </h4>
                        <a href="#" runat="server" id="a5">
                            <img src="" height="50" width="100" alt="Image" class="mr-3" runat="server" id="img5" /></a>
                        <p>
                            <asp:Label ID="lblDescription5" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--</div>--%>
    </form>
</body>
</html>
