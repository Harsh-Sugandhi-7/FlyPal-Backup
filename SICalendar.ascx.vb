Imports System.ComponentModel
Namespace SIControls
	<DefaultProperty("Text"), ToolboxData("<{0}:SICalendar runat=server></{0}:SICalendar>"), ValidationProperty("Text")> _
	Partial Class SICalendar
		Inherits System.Web.UI.UserControl
		Public Event Click As EventHandler
		Public Event TextChanged As EventHandler
		Public Event CalendarVisibleChanged As EventHandler
		'New variable added by Kalpesh
		'Private tmpLastDate As SmartDate = New SmartDate(True)
		Public ShowTime As Boolean = False
		Private Time As DateTime = Date.MinValue

#Region " Web Form Designer Generated Code "

		'This call is required by the Web Form Designer.
		<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

		End Sub

		'NOTE: The following placeholder declaration is required by the Web Form Designer.
		'Do not delete or move it.
		Private designerPlaceholderDeclaration As System.Object

		Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			'CODEGEN: This method call is required by the Web Form Designer
			'Do not modify it using the code editor.
			InitializeComponent()
		End Sub

#End Region

		<Bindable(True), Category("Appearance"), DefaultValue("")> _
		Public ReadOnly Property Text() As String
			Get
				If TextBox1.Text = "" Then
					Return ""
				Else
					If ShowTime Then
						Return CDate(Cal.SelectedDate.ToShortDateString + " " + GetTime()).ToString
					Else
						Return CDate(Cal.SelectedDate.ToShortDateString).ToString
					End If
				End If
			End Get
		End Property
		'<Bindable(True), Category("Appearance"), DefaultValue("")> _
		'Public ReadOnly Property Text() As String
		'    Get
		'        If TextBox1.Text = "" Then
		'            Return ""
		'        Else
		'            If ShowTime Then
		'                Return CDate(DateText).ToString
		'            Else
		'                Return CDate(DateText + " " + TimeText).ToString
		'            End If
		'        End If
		'    End Get
		'End Property

		'<Bindable(True), Category("Appearance"), DefaultValue("")> _
		'Public ReadOnly Property DateText() As String
		'    Get
		'        If TextBox1.Text = "" Then
		'            Return ""
		'        Else
		'            Return CDate(Cal.SelectedDate.ToShortDateString).ToString
		'        End If
		'    End Get
		'End Property
		'<Bindable(True), Category("Appearance"), DefaultValue("")> _
		'Public ReadOnly Property TimeText() As String
		'    Get
		'        If Textbox2.Value = "" Then
		'            Return ""
		'        Else
		'            Return (CDate(Textbox2.Value).ToString(Flypal.Util.WebTimeFormat)())
		'        End If
		'    End Get
		'End Property
		<Bindable(True), Category("Appearance"), DefaultValue("")> _
		Public ReadOnly Property IsDateValue() As Boolean
			Get
				If TextBox1.Text = "" Then
					Return False
				Else
					Return IsDate(Cal.SelectedDate)
				End If
			End Get
		End Property
		'New
		<Bindable(True), Category("Appearance")> _
		Public Property Value() As Object
			Get
				If TextBox1.Text = "" Then
					Return DBNull.Value
				Else
					If ShowTime Then
						Return CDate(Cal.SelectedDate.ToShortDateString + " " + GetTime())
					Else
						Return CDate(Cal.SelectedDate.ToShortDateString)
					End If
				End If
			End Get
			Set(ByVal Value As Object)
				If Not (New SmartDate(Value.ToString).IsEmpty) Then
					Cal.SelectedDate = Value

					If ShowTime Then
						If ShowTime Then Time = CDate(Value).ToString(Flypal.Util.WebTimeFormat) : Session("Time") = Time

						'Time
						Textbox2.Value = CDate(Value).ToString(Flypal.Util.WebTimeFormat)
						'Date and Time
						TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
					Else
						TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat)
					End If
				Else
					TextBox1.Text = New SmartDate(Value.ToString).Text
				End If

				SelectCorrectValues()
				'RaiseEvent TextChanged(TextBox1.Text, New System.EventArgs)
			End Set
		End Property
		<Bindable(True), Category("Appearance")> _
		Public Property Enabled() As Boolean
			Get
				Return TextBox1.Enabled
			End Get
			Set(ByVal Value As Boolean)
				TextBox1.Enabled = Value
				ImageButton1.Enabled = Value
			End Set
		End Property
		<Bindable(True), Category("Appearance")> _
		Public Property Height() As System.Web.UI.WebControls.Unit
			Get
				Return TextBox1.Height
			End Get
			Set(ByVal Value As System.Web.UI.WebControls.Unit)
				TextBox1.Height = Value
			End Set
		End Property
		<Bindable(True), Category("Appearance")> _
		Public Property [ReadOnly]() As Boolean
			Get
				Return Panel1.Enabled
			End Get
			Set(ByVal Value As Boolean)
				Panel1.Enabled = Not Value
			End Set
		End Property
		<Bindable(True), Category("Appearance")> _
		Public Property BackColor() As System.Drawing.Color
			Get
				Return TextBox1.BackColor
			End Get
			Set(ByVal Value As System.Drawing.Color)
				TextBox1.BackColor = Value
			End Set
		End Property
		<Bindable(True), Category("Apperance")> _
		Public WriteOnly Property HideCalendar() As Boolean
			Set(ByVal Value As Boolean)
				Panel2.Visible = False
				Dim e1 As EventArgs = New EventArgs
				RaiseEvent CalendarVisibleChanged(Panel2.Visible, e1)
			End Set
		End Property
		<Bindable(True), Category("Appearance")> _
		Public Property ShowClearButton() As Boolean
			Get
				Return CancelButton.Visible
			End Get
			Set(ByVal Value As Boolean)
				CancelButton.Visible = Value
			End Set
		End Property
		Private Function GetTime() As String 'DateTime
			'Change1
			'If Not Session("Time") Is Nothing Then
			'    Return CDate(Session("Time")).ToString(Flypal.Util.WebTimeFormat)
			'Else
			'    Return Date.MinValue.ToString(Flypal.Util.WebTimeFormat)
			'End If

			If ShowTime And IsDate(Textbox2.Value) Then
				Return CDate(Textbox2.Value).ToString(Flypal.Util.WebTimeFormat)
			Else
				Return Date.MinValue.ToString(Flypal.Util.WebTimeFormat)
			End If
		End Function
		Private Sub FillCalendarChoices()
			Dim thisdate As New DateTime(DateTime.Today.Year, 1, 1)

			Dim x As Integer
			For x = 0 To 11
				' Loops through 12 months of the year and fills in each month value
				Dim li As New ListItem(thisdate.ToString("MMMM"), thisdate.Month.ToString())
				MonthSelect.Items.Add(li)
				thisdate = thisdate.AddMonths(1)
			Next x
		End Sub
		Private Sub SelectCorrectValues()
			'lblDate.Text = Cal.SelectedDate.ToShortDateString()
			If MonthSelect.SelectedValue <> Cal.SelectedDate.Month.ToString() Or Trim(YearSelect.Text) <> Cal.SelectedDate.Year.ToString() Then
				datechosen.Value = Cal.SelectedDate.ToShortDateString()
				MonthSelect.SelectedIndex = MonthSelect.Items.IndexOf(MonthSelect.Items.FindByValue(Cal.SelectedDate.Month.ToString()))
				YearSelect.Text = Cal.SelectedDate.Year.ToString()
			End If
		End Sub
		Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
			'Textbox2.Visible = ShowTime

			If Not Page.IsPostBack Then
				Cal.FirstDayOfWeek = CType(Convert.ToInt32(ConfigurationManager.AppSettings([Global].CfgKeyFirstDayOfWeek)), System.Web.UI.WebControls.FirstDayOfWeek)

				Try
					'If Not IsDate(TextBox1.Text) Then
					If Not IsDateValue Then
						Cal.SelectedDate = DateTime.Today
						Cal.VisibleDate = DateTime.Today
					Else
						'Cal.SelectedDate = CDate(TextBox1.Text)
						'Cal.VisibleDate = CDate(TextBox1.Text)
					End If
				Catch
					Cal.SelectedDate = DateTime.Today
					Cal.VisibleDate = DateTime.Today
				End Try

				FillCalendarChoices()
				datechosen.Value = Cal.SelectedDate.ToShortDateString()
				MonthSelect.SelectedIndex = MonthSelect.Items.IndexOf(MonthSelect.Items.FindByValue(Cal.SelectedDate.Month.ToString()))
				YearSelect.Text = Cal.SelectedDate.Year.ToString()
			Else
				'
			End If
		End Sub
		Private Sub Cal_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cal.SelectionChanged
			If Not (Not IsDate(Cal.SelectedDate) Or (IsDate(Cal.SelectedDate) AndAlso CDate(Cal.SelectedDate) < #1/1/1753#)) Then
				Cal.VisibleDate = Cal.SelectedDate

				If ShowTime Then
					TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
				Else
					TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat)
				End If

				SelectCorrectValues()
				Panel2.Visible = False

				RaiseEvent TextChanged(TextBox1.Text, e)
				Dim e1 As EventArgs = New EventArgs
				RaiseEvent CalendarVisibleChanged(Panel2.Visible, e1)
			End If
		End Sub
		Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
			'Added By Utkarsh on 13-Aug-2013 for ALL13082013-2
			If Not ShowTime Then
				TextBox1.Text = TextBox1.Text.Trim.Replace(" ", "")
			End If
			'End
			If (Not IsDate(TextBox1.Text)) Then

				If Len(Trim(TextBox1.Text)) = 0 Then
					Cal.VisibleDate = Cal.SelectedDate

					If ShowTime Then
						TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
					Else
						TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat)
					End If
					SelectCorrectValues()
					RaiseEvent TextChanged(TextBox1.Text, e)
				Else
					Try
						Dim mSeparator As String = ""
						If Trim(Flypal.Util.WebDateFormat).IndexOf("/") <> -1 Then
							mSeparator = "/"
						ElseIf Trim(Flypal.Util.WebDateFormat).IndexOf("-") <> -1 Then
							mSeparator = "-"
						ElseIf Trim(Flypal.Util.WebDateFormat).IndexOf("\") <> -1 Then
							mSeparator = "\"
						ElseIf Trim(Flypal.Util.WebDateFormat).IndexOf(".") <> -1 Then
							mSeparator = "."
						End If

						Dim mWebDateFormat As String() = Flypal.Util.WebDateFormat.Split(mSeparator)
						Dim mEnteredDateString As String() = TextBox1.Text.Split(mSeparator)
						Dim mDateSequence As String() = {"", "", "", ""}

						For i As Integer = 0 To UBound(mWebDateFormat)
							If Trim(mWebDateFormat(i)).IndexOf("y") <> -1 Then
								Dim YRLength As Integer = Len(mEnteredDateString(i).Substring(0, Len(Trim(mWebDateFormat(i)))))

								mDateSequence(0) = Trim(mEnteredDateString(i).Substring(0, YRLength))

								mDateSequence(3) = Trim(mEnteredDateString(i).Substring(YRLength, Len(mEnteredDateString(i)) - YRLength))
							ElseIf Trim(mWebDateFormat(i)).IndexOf("M") <> -1 Then
								If Len(mWebDateFormat(i)) >= 3 Then 'MMM / MMMM
									Dim thisdate As New DateTime(DateTime.Today.Year, 1, 1)

									Dim x As Integer
									For x = 1 To 12
										If thisdate.ToString("MMM").ToUpper = mEnteredDateString(i).ToUpper Or thisdate.ToString("MMMM").ToUpper = mEnteredDateString(i).ToUpper Then
											mDateSequence(1) = x
										End If

										thisdate = thisdate.AddMonths(1)
									Next x
								Else                               'M or MM 
									mDateSequence(1) = Trim(mEnteredDateString(i))
								End If
							ElseIf Trim(mWebDateFormat(i)).IndexOf("d") <> -1 Then
								mDateSequence(2) = Trim(mEnteredDateString(i))
							End If
						Next

						If UBound(mEnteredDateString) = 2 Then
							Dim mActualDateTime As DateTime = New Date(CInt(mDateSequence(0)), CInt(mDateSequence(1)), CInt(mDateSequence(2)))
							If ShowTime And Len(mDateSequence(3)) > 0 Then
								'mActualDateTime = mActualDateTime.ToString("MM-dd-yyyy") + " " + mDateSequence(3)
								mActualDateTime = mActualDateTime.ToString(Flypal.Util.WebDateFormat) + " " + mDateSequence(3)
							Else
								'mActualDateTime = mActualDateTime.ToString("MM-dd-yyyy") 
							End If

							Cal.SelectedDate = mActualDateTime
							Cal.VisibleDate = Cal.SelectedDate
							SelectCorrectValues()

							Textbox2.Value = CDate(mActualDateTime).ToString(Flypal.Util.WebTimeFormat)
							If ShowTime Then Time = CDate(mActualDateTime).ToString(Flypal.Util.WebTimeFormat) : Session("Time") = Time
							'Added By Utkarsh on 13-Aug-2013 for ALL13082013-2
							If ShowTime Then
								TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
							End If
							'End
							RaiseEvent TextChanged(TextBox1.Text, e)
						End If
					Catch ex As Exception
						Cal.VisibleDate = Cal.SelectedDate

						If ShowTime Then
							TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
						Else
							TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat)
						End If
						SelectCorrectValues()
						RaiseEvent TextChanged(TextBox1.Text, e)
					End Try
				End If
			ElseIf ((IsDate(TextBox1.Text) AndAlso CDate(TextBox1.Text) < #1/1/1753#)) Then
				Cal.VisibleDate = Cal.SelectedDate

				If ShowTime Then
					TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
				Else
					TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat)
				End If
				SelectCorrectValues()
				RaiseEvent TextChanged(TextBox1.Text, e)
			ElseIf IsDate(TextBox1.Text) Then
				Try
					Dim mSeparator As String = ""
					If Trim(Flypal.Util.WebDateFormat).IndexOf("/") <> -1 Then
						mSeparator = "/"
					ElseIf Trim(Flypal.Util.WebDateFormat).IndexOf("-") <> -1 Then
						mSeparator = "-"
					ElseIf Trim(Flypal.Util.WebDateFormat).IndexOf("\") <> -1 Then
						mSeparator = "\"
					ElseIf Trim(Flypal.Util.WebDateFormat).IndexOf(".") <> -1 Then
						mSeparator = "."
					End If

					Dim mWebDateFormat As String() = Flypal.Util.WebDateFormat.Split(mSeparator)
					Dim mEnteredDateString As String() = TextBox1.Text.Split(mSeparator)
					Dim mDateSequence As String() = {"", "", "", ""}

					For i As Integer = 0 To UBound(mWebDateFormat)
						If Trim(mWebDateFormat(i)).IndexOf("y") <> -1 Then
							Dim YRLength As Integer = Len(mEnteredDateString(i).Substring(0, Len(Trim(mWebDateFormat(i)))))

							mDateSequence(0) = Trim(mEnteredDateString(i).Substring(0, YRLength))

							mDateSequence(3) = Trim(mEnteredDateString(i).Substring(YRLength, Len(mEnteredDateString(i)) - YRLength))
						ElseIf Trim(mWebDateFormat(i)).IndexOf("M") <> -1 Then
							If Len(mWebDateFormat(i)) >= 3 Then 'MMM / MMMM
								Dim thisdate As New DateTime(DateTime.Today.Year, 1, 1)

								Dim x As Integer
								For x = 1 To 12
									If thisdate.ToString("MMM").ToUpper = mEnteredDateString(i).ToUpper Or thisdate.ToString("MMMM").ToUpper = mEnteredDateString(i).ToUpper Then
										mDateSequence(1) = x
									End If

									thisdate = thisdate.AddMonths(1)
								Next x
							Else                               'M or MM 
								mDateSequence(1) = Trim(mEnteredDateString(i))
							End If
						ElseIf Trim(mWebDateFormat(i)).IndexOf("d") <> -1 Then
							mDateSequence(2) = Trim(mEnteredDateString(i))
						End If
					Next

					If UBound(mEnteredDateString) = 2 Then
						Dim mActualDateTime As DateTime = New Date(CInt(mDateSequence(0)), CInt(mDateSequence(1)), CInt(mDateSequence(2)))
						If ShowTime And Len(mDateSequence(3)) > 0 Then
							'mActualDateTime = mActualDateTime.ToString("MM-dd-yyyy") + " " + mDateSequence(3)
							mActualDateTime = mActualDateTime.ToString(Flypal.Util.WebDateFormat) + " " + mDateSequence(3)
						Else
							'mActualDateTime = mActualDateTime.ToString("MM-dd-yyyy") 
						End If

						Cal.SelectedDate = mActualDateTime
						Cal.VisibleDate = Cal.SelectedDate
						SelectCorrectValues()

						Textbox2.Value = CDate(mActualDateTime).ToString(Flypal.Util.WebTimeFormat)

						If ShowTime Then Time = CDate(mActualDateTime).ToString(Flypal.Util.WebTimeFormat) : Session("Time") = Time

						If ShowTime Then
							TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
						Else
							TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat)
						End If

						RaiseEvent TextChanged(TextBox1.Text, e)
					End If
				Catch ex As Exception
					Cal.VisibleDate = Cal.SelectedDate

					If ShowTime Then
						TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
					Else
						TextBox1.Text = Cal.SelectedDate.ToString(Flypal.Util.WebDateFormat)
					End If
					SelectCorrectValues()
					RaiseEvent TextChanged(TextBox1.Text, e)
				End Try
				'Cal.SelectedDate = CDate(TextBox1.Text)
				'Cal.VisibleDate = Cal.SelectedDate
				'SelectCorrectValues()
				'If ShowTime Then Time = CDate(TextBox1.Text).ToString(Flypal.Util.WebTimeFormat) : Session("Time") = Time
				'RaiseEvent TextChanged(TextBox1.Text, e)
			End If
		End Sub
		Private Sub MonthSelect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MonthSelect.SelectedIndexChanged
			Try
				Cal.VisibleDate = New DateTime(Convert.ToInt32(YearSelect.Text), Convert.ToInt32(MonthSelect.SelectedItem.Value), 1)
			Catch ex As Exception
			End Try
		End Sub 'MonthSelect_SelectedIndexChanged
		Private Sub YearSelect_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles YearSelect.TextChanged
			Try
				If Not CInt(YearSelect.Text) < 1753 Then
					Cal.VisibleDate = New DateTime(Convert.ToInt32(YearSelect.Text), Convert.ToInt32(MonthSelect.SelectedItem.Value), Cal.SelectedDate.Day)
				Else
					YearSelect.Text = Cal.SelectedDate.Year.ToString()
				End If
			Catch ex As Exception
				YearSelect.Text = Cal.SelectedDate.Year.ToString()
			End Try
		End Sub
		Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click

			If ShowTime Then
				TextBox1.Text = Today.ToString(Flypal.Util.WebDateFormat) + " " + GetTime()
			Else
				TextBox1.Text = Today.ToString(Flypal.Util.WebDateFormat)
			End If

			Panel2.Visible = False

			Cal.SelectedDate = Today.Date
			Cal.VisibleDate = Cal.SelectedDate
			SelectCorrectValues()

			RaiseEvent TextChanged(TextBox1.Text, e)
			Dim e1 As EventArgs = New EventArgs
			RaiseEvent CalendarVisibleChanged(Panel2.Visible, e1)
		End Sub
		Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
			TextBox1.Text = ""
			Panel2.Visible = False

			RaiseEvent TextChanged(TextBox1.Text, e)
			Dim e1 As EventArgs = New EventArgs
			RaiseEvent CalendarVisibleChanged(Panel2.Visible, e1)
		End Sub
		Protected Overrides Function OnBubbleEvent(ByVal source As Object, ByVal e As EventArgs) As Boolean
			Dim handled As Boolean = False
			If TypeOf e Is CommandEventArgs Then
				Dim ce As CommandEventArgs = CType(e, CommandEventArgs)
				Response.Write(ce.CommandArgument)
				If ce.CommandName = "Click" Then
					OnClick(ce)
					handled = True
				End If
			End If
			Return handled
		End Function
		Protected Overridable Sub OnClick(ByVal e As EventArgs)
			RaiseEvent Click(Me, e)
		End Sub
		Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
			If Panel2.Visible = False Then
				Try
					Cal.SelectedDate = Convert.ToDateTime(Value) 'TextBox1.Text)
					Cal.VisibleDate = Convert.ToDateTime(Value) 'TextBox1.Text)
				Catch
					Cal.SelectedDate = DateTime.Today
					Cal.VisibleDate = DateTime.Today
				End Try
			End If

			Panel2.Visible = Not Panel2.Visible
			'OKButton.Visible = Not OKButton.Visible
			Dim e1 As EventArgs = New EventArgs
			RaiseEvent CalendarVisibleChanged(Panel2.Visible, e1)
		End Sub

	End Class
End Namespace