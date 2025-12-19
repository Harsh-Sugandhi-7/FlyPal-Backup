'************************************
' Rajnish   07-09-2006
'Modified by Harsh Sugandhi on 6th May 2025 for FLYPAL-2360 API for LogParameterList Grid View.
'************************************


Partial Class wfLogParameterList_Ajax
	Inherits Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As Object

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region


#Region " Variable Declaration "

	Public mLog As Log
	Public mMachine As Machine
	Public mAssemblyParameterListForAssemblyStatus As AssemblyParameterListForAssemblyStatus
	Private Flag As Int16
	Dim mLogDetail As String 'Added by Utkarsh On 08-Sep-2011
	Dim EventLogID As Guid 'Added by Utkarsh On 08-Sep-2011
	Private mOpenFromParameterListNew As Boolean = False 'Added by Utkarsh On 08-Sep-2011
	Public mLogParameterList As LogParameters = CType(LogParameters.NewLogParameters(), LogParameters)

#End Region

#Region " Enum "

	Enum ControlType
		TextBox = 1
		Button = 2
		TextBoxAndButton = 3
	End Enum

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mLog = CType(Session("mLog"), Log)
		mMachine = CType(Session("mMachine"), Machine)
		' mLogList = CType(Session("mLogList"), LogList)
		'mParameterList = CType(Session("mParameterList"), ParameterList)
		mAssemblyParameterListForAssemblyStatus = CType(Session("mAssemblyParameterListForAssemblyStatus"), AssemblyParameterListForAssemblyStatus)
		mOpenFromParameterListNew = CType(Session("mOpenFromParameterListNew"), Boolean)  'Added by Utkarsh On 08-Sep-2011
	End Sub

	Private Sub SetSession()
		Session("mLog") = mLog
		Session("mMachine") = mMachine
		'' Session("mLogList") = mLogList
		'Session("mParameterList") = mParameterList
		Session("mAssemblyParameterListForAssemblyStatus") = mAssemblyParameterListForAssemblyStatus
		Session("mOpenFromParameterListNew") = mOpenFromParameterListNew 'Added by Utkarsh On 08-Sep-2011
	End Sub

	Private Sub RemoveSession()
		'Session.Remove("mParameterList")
		Session.Remove("mMachine")
		Session.Remove("mAssemblyParameterListForAssemblyStatus")
		Session.Remove("mOpenFromParameterListNew") 'Added by Utkarsh On 08-Sep-2011
		Session.Remove("mLogParameterList")
	End Sub

	Private Sub NewLogPax()
		Dim mLogPax As LogPax
		mLogPax = LogPax.NewLogPax(mLog.ID)
		Session("mLogPax") = mLogPax
	End Sub

	Private Sub NewHobbsOffSet()
		Dim mHobbsOffset As HobbsOffset
		mHobbsOffset = HobbsOffset.NewHobbsOffset(Guid.NewGuid, mLog.MachineID)
		Session("mHobbsOffset") = mHobbsOffset
	End Sub

	Private Overloads Sub setFocus(cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Try
			Dim str As String
			'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
			'ClientScript.RegisterStartupScript([gettype], "focusscript", str)
			str = "document.getElementById('" + cntrl.ClientID + "').focus();"
			ScriptManager.RegisterStartupScript(Me, [GetType], "focusscript", str, True)
		Catch ex As Exception
			'
		End Try
	End Sub

	Private Sub NewRecord()
		mLog = Log.NewLog(mMachine, Today.Date)
		Session("mLog") = mLog
		SetTitle()

	End Sub

	Public Sub SetGridObject()        ' For First Grid i.e AirFrame
		'Commeted and Added By Utkarsh On 20-Aug-2012 FOR ALL-20082012

		'Dim txtParameterValue As TextBox
		'For i As Integer = 0 To Me.dgLogParameters.Items.Count - 1
		'    txtParameterValue = CType(Me.dgLogParameters.Items(i).FindControl("txtParameterValue"), TextBox)
		'    mLog.LogParameters(i).ParameterValue = Val(txtParameterValue.Text.Trim)
		'Next i

		For i As Integer = 0 To dgLogParameters.Items.Count - 1
			Dim parameterID As Guid = New Guid(dgLogParameters.Items(i).Cells(0).Text)
			'Static columns = 8 
			'Loop through dynamic columns
			Dim l As Integer = 0
			'Dim k As Integer = dgLogParameters.Columns.Count - 8 ''Commented by Saylee on 6-Sep-2012
			Dim k As Integer = dgLogParameters.Columns.Count - 7 ''Commented by Saylee on 6-Sep-2012
			'Dim k As Integer = CType(Session("CountAssembly"), Integer) 'Added by Saylee on 6-Sep-2012
			For j As Integer = 0 To k - 1
				If j <> 1 Then
					Dim lableAssemblyID As Label
					lableAssemblyID = CType(dgLogParameters.Items(i).FindControl("AssemblyID" & l), Label)
					Dim AssemblyID As Guid = New Guid(lableAssemblyID.Text)
					'Dim AssemblyID As Guid = mAssemblyParameterListForAssemblyStatus(j).AssemblyID

					If mLog.LogParameters.Contains(parameterID, AssemblyID) Then
						Dim txtParameterValue As TextBox
						txtParameterValue = CType(dgLogParameters.Items(i).FindControl("parameter" & l), TextBox)
						mLog.LogParameters(parameterID, AssemblyID).ParameterValue = Val(txtParameterValue.Text.Trim)
					End If
					l = l + 1
				End If
			Next
		Next
		'End
		Session("mLog") = mLog
	End Sub

	Private Function Save() As Boolean
		Dim LogClone As Log
		LogClone = CType(mLog.Clone, Log)
		SetGridObject()
		If mLog.IsValid = True Then
			Try
				If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
					If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
				   Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
						''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly)
						''''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
						''''msg1.Show()

						MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction, MSGBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly, "")

						Return False
					End If
				End If
				'End
				mLog.ApplyEdit()
				mLog = CType(mLog.Save(), Log)
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")

				End If
				Return False
			Finally
				LogClone = Nothing

			End Try
		Else
			upnlErrorList.Update()

			Return False
		End If
	End Function

	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult

		' '' ''Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					DataBindGrid()
					'Response.Redirect("wfLogParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
				Case MsgBoxResult.Ok
					'Response.Redirect("wfLogParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
				Case MsgBoxResult.No
					'Response.Redirect("wfLogParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
				Case MsgBoxResult.Cancel
					'Response.Redirect("wfLogParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
				Case Else
					'Response.Redirect("wfLogParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
			End Select
		ElseIf Result1 = -1 Then
			'Response.Redirect("wfLogParameterList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
		End If
	End Sub

	Private Sub SetTitle()

		If mLog.IsNew Then
			lblTitle.Text = "Log Details of " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
		Else
			lblTitle.Text = "Log Details of " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
		End If

		upnlTitle.Update()

	End Sub

	Private Sub ControlVisibility()

		If mOpenFromParameterListNew Then

			btnSave.Visible = True
			btnLogDetails.Visible = False
			btnDefectActionList.Visible = False
			btnFuelOil.Visible = False
			btnHobbsOffset.Visible = False
			lblParameterList.Visible = False
			btnBack.Text = "Close"
			btnBack.ToolTip = "Click to close Log Parameter page"
			'**************************************
			btnBack.Visible = True
			btnLogCrew.Visible = False 'Added By Utkarsh ON 16-Jul-2012 FOR ALL16072012-3
			btnMaintenanceAcitvity.Visible = False  'Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013

		Else

			btnLogPax.Enabled = Not mLog.IsNew
			btnDefectActionList.Enabled = Not mLog.IsNew
			btnHobbsOffset.Enabled = (mMachine.HourType = 2)
			btnSave.Visible = True   'Added By Saylee on 6-Sep-2012
			btnLogCrew.Visible = IIf(AppSettings("LogDetailPage") = "NewPage", True, False) 'Added By Utkarsh ON 16-Jul-2012 FOR ALL16072012-3
			btnMaintenanceAcitvity.Visible = IIf(AppSettings("LogDetailPage") = "NewPage", True, False) 'Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
			ScriptManager.RegisterStartupScript(Me, [GetType], "CallParentFunction", "CallParentFunction();", True)

		End If

		btnSave.Visible = (dgLogParameters.Items.Count > 0)

		upnlTabs.Update()

	End Sub

	'Added By Utkarsh ON 20-Aug-2012 FOR ALL-20082012
	Private Sub SetParameterValues()

		Try

			For i As Integer = 0 To dgLogParameters.Items.Count - 1

				Dim dataSource = CType(dgLogParameters.DataSource, LogParameters)
				Dim dataItem = dataSource(i)
				Dim parameterID As Guid = dataItem.ParameterID

				'Static columns = 8 
				'Loop through dynamic columns
				Dim l As Integer = 0
				Dim k As Integer = dgLogParameters.Columns.Count - 7    'Commented by Saylee on 6-Sep-2012

				For j As Integer = 0 To k - 1

					If j <> 1 Then

						Dim LabelAssemblyID As Label
						LabelAssemblyID = CType(dgLogParameters.Items(i).FindControl("AssemblyID" & l), Label)
						Dim AssemblyID As New Guid(LabelAssemblyID.Text)

						Dim txtParameterValue As TextBox

						txtParameterValue = CType(dgLogParameters.Items(i).FindControl("parameter" & l), TextBox)

						txtParameterValue.Height = "17"

						If mAssemblyParameterListForAssemblyStatus.Contains(parameterID, AssemblyID) Then

							If mLog.LogParameters.Contains(parameterID, AssemblyID) Then
								txtParameterValue.Text = mLog.LogParameters(parameterID, AssemblyID).ParameterValue
							End If

						Else

							txtParameterValue.Text = "0"
							txtParameterValue.Enabled = False

						End If

						l = l + 1

					End If

				Next

			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CopyParameterValues(Index As Integer)
		'For i As Integer = 0 To dgLogParameters.Items.Count - 1
		' Dim parameterID As Guid = New Guid(dgLogParameters.Items(i).Cells(0).Text)
		'Static columns = 8 
		'Loop through dynamic columns
		Dim l As Integer = 0


		Dim AssemblyTypeName As String = dgLogParameters.Items(0).Cells(1).Text
		'Dim k As Integer = dgLogParameters.Columns.Count - 8       'Commented by Saylee on 6-Sep-2012 
		Dim k As Integer = dgLogParameters.Columns.Count - 7       'Commented by Saylee on 6-Sep-2012 
		'Dim k As Integer = CType(Session("CountAssembly"), Integer) 'Added by Saylee on 6-Sep-2012
		Dim copyParamValue As String = String.Empty
		Dim CopyStatus As Boolean = False
		For j As Integer = 0 To k - 1
			If j <> 1 Then
				If j = 0 Then
					If AssemblyTypeName = "Airframe" Then
						j = j + 1
					Else
						Dim txtParameterValue As TextBox
						txtParameterValue = CType(dgLogParameters.Items(Index).FindControl("parameter" & l), TextBox)
						CopyStatus = txtParameterValue.Enabled
						If CopyStatus Then
							copyParamValue = txtParameterValue.Text.Trim
						End If
					End If
				ElseIf j = 2 Then
					If AssemblyTypeName = "Airframe" Then
						Dim txtParameterValue As TextBox
						txtParameterValue = CType(dgLogParameters.Items(Index).FindControl("parameter" & l), TextBox)
						CopyStatus = txtParameterValue.Enabled
						If CopyStatus Then
							copyParamValue = txtParameterValue.Text.Trim
						End If
					Else
						Dim txtParameterValue As TextBox
						txtParameterValue = CType(dgLogParameters.Items(Index).FindControl("parameter" & l), TextBox)
						If CopyStatus Then
							If txtParameterValue.Enabled Then
								txtParameterValue.Text = copyParamValue
							End If

						End If
					End If

				Else
					Dim txtParameterValue As TextBox
					txtParameterValue = CType(dgLogParameters.Items(Index).FindControl("parameter" & l), TextBox)

					If CopyStatus Then
						If txtParameterValue.Enabled Then
							txtParameterValue.Text = copyParamValue
						End If
					End If
				End If
				l = l + 1
			End If

		Next
		' Next
	End Sub
	'End

	'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013
	Private Function CheckZeroDifferenceValue() As Boolean
		If mLog.IsHobbs Then
			If Val(mLog.TotalTime) <> 0 Then
				Return False
			End If
			If Val(mLog.TimeInAir) <> 0 Then
				Return False
			End If
		Else
			If mLog.TimeInAir = "0:00" OrElse mLog.TimeInAir = "" Then
			Else
				Return False
			End If
			If mLog.TotalTime = "0:00" OrElse mLog.TotalTime = "" Then
			Else
				Return False
			End If
			If mLog.BlockTime = "0:00" OrElse mLog.BlockTime = "" Then
			Else
				Return False
			End If
			If mLog.TimeOnGround = "0:00" OrElse mLog.TimeOnGround = "" Then
			Else
				Return False
			End If
		End If
		If Val(mLog.TotalLandings) <> 0 Then
			Return False
		End If

		Dim checkcol = mLog.LogAFAssemblies
		If Not callZeroDifferenceValue(checkcol) Then
			Return False
		End If
		checkcol = mLog.LogAPUAssemblies
		If Not callZeroDifferenceValue(checkcol) Then
			Return False
		End If
		checkcol = mLog.LogEngAssemblies
		If Not callZeroDifferenceValue(checkcol) Then
			Return False
		End If
		checkcol = mLog.LogCGBAssemblies
		If Not callZeroDifferenceValue(checkcol) Then
			Return False
		End If
		Return True
	End Function

	Private Function callZeroDifferenceValue(obj) As Boolean
		For i As Integer = 0 To obj.Count - 1
			If mLog.IsHobbs Then
				If Val(obj(i).Hours) <> 0 Then
					Return False
				End If
			Else
				If obj(i).Hours <> "0:00" Then
					Return False
				End If
			End If
			If obj(i).ShowLandings Then
				If Val(obj(i).Landings) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowCycles Then
				If Val(obj(i).Cycles) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowStarts Then
				If Val(obj(i).Starts) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowNGCycles Then
				If Val(obj(i).NGCycles) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowNFCycles Then
				If Val(obj(i).NFCycles) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowRINS Then
				If Val(obj(i).RINS) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowBleeds Then
				If Val(obj(i).Bleeds) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowImpellerCycles Then
				If Val(obj(i).ImpellerCycles) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowCTCycles Then
				If Val(obj(i).CTCycles) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowPTCycles Then
				If Val(obj(i).PTCycles) <> 0 Then
					Return False
				End If
			End If
			If obj(i).ShowGeneratorMods Then
				If Val(obj(i).GeneratorMods) <> 0 Then
					Return False
				End If
			End If
		Next
		Return True
	End Function
	'End

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs) ' Validation From AIRFRAMEGRID (Grid-1)

		If Flag = 1 Then Exit Sub

		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		SetGridObject()
		Dim str As String = ""

		'Log
		If Not mLog.IsValid Then

			For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
				str = str + mLog.GetBrokenRulesCollection(i).Description + "<BR>"
			Next

		End If

		'Log parameters
		For i As Integer = 0 To mLog.LogParameters.Count - 1

			If Not mLog.LogParameters(i).IsValid Then
				For j As Integer = 0 To mLog.LogParameters(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.LogParameters.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
				Next
			End If

		Next

		If str <> "" Then

			custValidator.ErrorMessage = str
			e.IsValid = False

			upnlErrorList.Update()

		End If

		Flag = 1

	End Sub

	Public Function CustomValidate2() As Boolean    'For DgLog Fuel Oils

		Dim str As String = ""

		'AirFrame
		For i As Integer = 0 To mLog.LogParameters.Count - 1

			If Not mLog.LogParameters(i).IsValid Then

				For j As Integer = 0 To mLog.LogParameters(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.LogParameters.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
				Next

			End If

		Next

		If str <> "" Then

			cvParameterList.ErrorMessage = str
			cvParameterList.IsValid = False

			upnlErrorList.Update()

			Return False

		End If

		Return True

	End Function

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Dim LogParameterListHelper As New LogParameterListHelper
		Try

			mAssemblyParameterListForAssemblyStatus =
				AssemblyParameterListForAssemblyStatus.
					GetAssemblyParameterListForAssemblyStatus(mLog.Date.ToString,
															  mLog.MachineID)

			cmbParameterList.DataSource = mAssemblyParameterListForAssemblyStatus

			Dim result = LogParameterListHelper.
							GetLogParameterList(_AssemblyParameterListForAssemblyStatus:=mAssemblyParameterListForAssemblyStatus,
												_Log:=mLog,
												_LogParameterList:=mLogParameterList)

			dgLogParameters.DataSource = result.Item1

			For Each column As TemplateColumn In result.Item2
				dgLogParameters.Columns.Add(column)
			Next

			Session("mLog") = result.Item3

			DataBind()
			SetParameterValues()

			upnlDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DataBindGrid()
		dgLogParameters.DataSource = mLogParameterList
		dgLogParameters.DataBind()
	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()

			If Not IsPostBack And CType(Session("sender"), String) = "" Then

			End If

			DataFieldBind()
			SetTitle()
			ControlVisibility()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub dgLogParameters_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles dgLogParameters.ItemCommand
		Dim Index As Int32 = e.Item.ItemIndex + dgLogParameters.CurrentPageIndex * dgLogParameters.PageSize
		Select Case e.CommandName
			Case "ParameterValue"
				'Dim txtParameterValue As TextBox
				'For i As Integer = 0 To mLog.LogParameters.Count - 1
				'    txtParameterValue = CType(Me.dgLogParameters.Items(i).FindControl("txtParameterValue"), TextBox)
				'    mLog.LogParameters(i).ParameterValue = Val(txtParameterValue.Text)
				'Next
				'DataBindGrid()
				'Added By Utkarsh ON 20-Aug-2012 FOR ALL-20082012
			Case "ParameterRefresh"
				CopyParameterValues(Index)
				SetGridObject()
				'End
		End Select
	End Sub

	Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
		If Not IsValid Then Exit Sub
		If mLog.LogParameters.Contains(New Guid(cmbParameterList.SelectedValue)) Then
			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "Log parameters.", MsgBoxStyle.OKOnly)
			' '' ''msg.ReplacePage = "wfLogParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
			' '' ''Session("sender") = "Delete"
			' '' ''msg.Show()

			MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "Delete")

			Exit Sub
		Else
			mLog.LogParameters.Add(mLog.ID, New Guid(cmbParameterList.SelectedValue))
			REM: Making explicitely dirty
			mLog.LogParameters.CurrentItem.ParameterValue = 1
			mLog.LogParameters.CurrentItem.ParameterValue = 0
			Session("mLog") = mLog
			'MarkLog(Util.Action.[New], "Log", "Aircraft -> " + mLog.Machine.RegNo + "Log FlightNo ->" + mLog.FlightNo + " Log Parameter " + cmbParameterList.SelectedItem.Text, Util.ErrorType.NoError, New Guid(cmbParameterList.SelectedValue.ToString))
			dgLogParameters.DataSource = mLog.LogParameters
			dgLogParameters.DataBind()
			If cmbParameterList.Enabled = True Then
				setFocus(cmbParameterList) '9/10/6
			End If
		End If
	End Sub

	Private Sub btnLogDetails_Click(sender As Object, e As EventArgs) Handles btnLogDetails.Click
		Session("mOpenFromParameterListNew") = False 'Added By Utkarsh On -08-Sep-2011
		Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub

	Private Sub btnFuelOil_Click(sender As Object, e As EventArgs) Handles btnFuelOil.Click
		Session("mOpenFromParameterListNew") = False 'Added By Utkarsh On -08-Sep-2011
		Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub

	Private Sub btnDefectActionList_Click(sender As Object, e As EventArgs) Handles btnDefectActionList.Click
		Session("mOpenFromParameterListNew") = False 'Added By Utkarsh On -08-Sep-2011
		Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub

	Private Sub btnLogPax_Click(sender As Object, e As EventArgs) Handles btnLogPax.Click
		NewLogPax()
		Session("mOpenFromParameterListNew") = False 'Added By Utkarsh On -08-Sep-2011
		'Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogParameterList_Ajax.aspx")
		Session("mOpenFromParameterListNew") = False
		If AppSettings("LogDetailPage") = "NewPage" Then
			If mLog.IsTLP = True Then
				Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLP_Ajax.aspx")
			Else
				Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")
			End If
		ElseIf mLog.IsTLP = True Then
			Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLP_Ajax.aspx")
		End If
	End Sub

	Private Sub btnHobbsOffset_Click(sender As Object, e As EventArgs) Handles btnHobbsOffset.Click
		NewHobbsOffSet()
		Session("mOpenFromParameterListNew") = False 'Added By Utkarsh On -08-Sep-2011
		Response.Redirect("wfHobbsOffset_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogParameterList_Ajax.aspx")
	End Sub

	Private Sub GoBack(sender As Object, e As EventArgs) Handles btnBack.Click

		SetGridObject()
		Session("mOpenFromParameterListNew") = False 'Added By Utkarsh On -08-Sep-2011
		Session.Remove("mOpenFromParameterListNew")

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, [GetType], "on close", "CallParentCallback();", True)
			Exit Sub
		End If
		'End

		Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))

	End Sub

	'Added By Prashant 22-June-2009 for grid sorting
	Private Sub dgLogParameters_SortCommand(source As Object, e As DataGridSortCommandEventArgs) Handles dgLogParameters.SortCommand
		mLog.LogParameters.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgLogParameters.DataSource = mLog.LogParameters
		dgLogParameters.DataBind()
	End Sub
	'----------------------------------------------

	'Added By Utkarsh On 08-Sep-2011
	Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
		'Added by Saylee on 8-Apr-2014 for ALL08042014
		If ((mOpenFromParameterListNew = False) And ((Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew))) _
			Or ((mOpenFromParameterListNew = True) And ((Not User.IsInRole("LogParameterListNew") And mLog.IsNew) Or (Not User.IsInRole("LogParameterListEdit") And Not mLog.IsNew))) Then

			SetGridObject()
			SetSession()
			'MarkLog(Util.Action.Save, "Log Fuel Oil", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Save, "LogParameterList", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
			' '' ''msg.ReplacePage = "wfLogParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
			' '' ''Session("sender") = "Authorization"
			' '' ''msg.Show()

			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

			Exit Sub
		End If

		If Save() = True Then
			'Added By Utkarsh On 08-Sep-2011
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Save, "Log Parameter List", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
			'End
			' '' ''Response.Redirect("wfLogParameterList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
		End If

	End Sub
	'End 

	'Added By Utkarsh ON 16-Jul-2012 FOR ALL16072012-3
	Private Sub btnLogCrew_Click(sender As Object, e As EventArgs) Handles btnLogCrew.Click
		Session("mOpenFromParameterListNew") = False
		If AppSettings("LogDetailPage") = "NewPage" Then
			If mLog.IsTLP = True Then
				Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLP_Ajax.aspx")
			Else
				Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")
			End If
		ElseIf mLog.IsTLP = True Then
			Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLP_Ajax.aspx")
		End If
	End Sub
	'End    

	'Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013 
	Private Sub btnMaintenanceAcitvity_Click(sender As Object, e As EventArgs) Handles btnMaintenanceAcitvity.Click
		Session("mOpenFromParameterListNew") = False
		If AppSettings("LogDetailPage") = "NewPage" Then
			If mLog.IsTLP = True Then
				Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLP_Ajax.aspx")
			Else
				Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx")
			End If
		ElseIf mLog.IsTLP = True Then
			Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLP_Ajax.aspx")
		End If
	End Sub
	'End    

	Public Shared Sub Param_Command(sender As Object, e As CommandEventArgs)

	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

#End Region

End Class
