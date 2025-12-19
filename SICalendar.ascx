<%@ Control Language="vb" AutoEventWireup="false" Codebehind="SICalendar.ascx.vb" Inherits="Flypal.SIControls.SICalendar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" debug="False" %>
<asp:panel id="Panel1" tabIndex="-1" Width="171px" runat="server">
	<asp:TextBox id="TextBox1" accessKey="" runat="server" Width="136px" CssClass="clsTextBoxDate"
		AutoPostBack="True"></asp:TextBox>
	<asp:ImageButton id="ImageButton1" runat="server" ImageUrl="images/icon-calendar.gif"
		CausesValidation="False"></asp:ImageButton>
</asp:panel><asp:panel id="Panel2" style="Z-INDEX: 500; POSITION: absolute" tabIndex="-1" runat="server"
	CssClass="clsCalPanel" Visible="False" BorderWidth="1px" BorderStyle="Solid" BackColor="WhiteSmoke">
	<TABLE cellSpacing="0" cellPadding="0" border="0">
		<TR tabIndex="-1">
			<TD tabIndex="-1" align="center" colSpan="2">
				<asp:dropdownlist id="MonthSelect" tabIndex="-1" runat="server" Width="90px" CssClass="clsComboMonth"
					AutoPostBack="True" Height="22px"></asp:dropdownlist>&nbsp;
				<asp:TextBox id="YearSelect" tabIndex="-1" runat="server" Width="54px" CssClass="clsTextYear"
					AutoPostBack="True" MaxLength="4"></asp:TextBox><BR>
				<asp:Image id="Image3" runat="server" Width="8px" Height="8px"></asp:Image><BR>
				<asp:calendar id="Cal" runat="server" Width="164px" CssClass="clsCal" BackColor="WhiteSmoke" BorderWidth="0px"
					FirstDayOfWeek="Sunday" DayNameFormat="FirstLetter" Font-Names="Verdana" Font-Size="XX-Small"
					ShowNextPrevMonth="False" ShowTitle="False" BorderColor="White">
					<TodayDayStyle ForeColor="White" CssClass="clsCalTodayDay" BackColor="Brown"></TodayDayStyle>
					<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="#333333" VerticalAlign="Bottom"></NextPrevStyle>
					<DayHeaderStyle CssClass="clsCalDayHeader"></DayHeaderStyle>
					<SelectedDayStyle ForeColor="Black" CssClass="clsCalSelectedDay" BackColor="LightSalmon"></SelectedDayStyle>
					<TitleStyle Font-Size="12pt" Font-Bold="True" BorderWidth="4px" ForeColor="#333399" BorderColor="Black"
						BackColor="White"></TitleStyle>
					<WeekendDayStyle CssClass="clsCalWeekendDay" BackColor="Silver"></WeekendDayStyle>
					<OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
				</asp:calendar></TD>
		</TR>
		<TR>
			<TD style="HEIGHT: 10px" align="center" colSpan="2">
				<asp:Image id="Image1" runat="server" Width="8px" Height="8px"></asp:Image></TD>
		</TR>
		<TR>
			<TD align="center">
				<asp:button id="OKButton" tabIndex="-1" runat="server" Width="60px" CssClass="clsButton" CausesValidation="False"
					Text="Today"></asp:button></TD>
			<TD tabIndex="-1" align="center"><A href="javascript:CloseWindow()">
					<asp:button id="CancelButton" tabIndex="-1" runat="server" Width="60px" CssClass="clsButton"
						CausesValidation="False" Text="Clear"></asp:button></A></TD>
		</TR>
		<TR>
			<TD align="center" colSpan="2">
				<asp:Image id="Image2" runat="server" Width="8px" Height="8px"></asp:Image></TD>
		</TR>
	</TABLE>
	<INPUT id="datechosen" style="WIDTH: 64px; HEIGHT: 22px" type="hidden" size="5" name="datechosen"
		runat="server"> <INPUT id="Textbox2" style="WIDTH: 64px; HEIGHT: 22px" type="hidden" size="5" name="datechosen"
		runat="server">
</asp:panel><INPUT id="Hidden1" style="WIDTH: 64px; HEIGHT: 22px" type="hidden" size="5" name="datechosen"
	runat="server">
