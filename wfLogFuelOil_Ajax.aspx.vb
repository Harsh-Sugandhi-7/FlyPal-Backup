' Rajnish   07-09-2006
Partial Class wfLogFuelOil_Ajax
	Inherits System.Web.UI.Page

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

#Region " Variable Declaration "
	Public mLog As Log
	Public mMachine As Machine
	Public mTankList As TankList
	Public mFuelUpliftUnit As UnitListMain
	Private Flag As Int16

	Private mOpenFromWO As Boolean = False 'Added by Saylee on 14-dec-2010
	Private mWOStatusID As Integer = 0
	Private mStatusIDForWO As Integer = 0

	Private mOpenFromLogFuelNew As Boolean = False 'Added by Saylee on 28-Apr-2011
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mLogDetail As String

	Dim mUpdateFuelsOfAllAboveLogs As UpdateFuelsOfAllAboveLogs 'Saylee on 16-Nov-2011 for ALL16112012
	Public mFuelType As FuelType 'Added By Shweta On 14-June-2013 For  ALL05062013
	Public mFuelTypeList As FuelTypeList 'Added By Shweta On 14-June-2013 For  ALL05062013
	Public mnWO As nWO
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mLog = CType(Session("mLog"), Log)
		mMachine = CType(Session("mMachine"), Machine)
		mTankList = CType(Session("mTankList"), TankList)
		mFuelUpliftUnit = CType(Session("mFuelUpliftUnit"), UnitListMain)
		mOpenFromWO = CType(Session("OpenFromWO"), Boolean)
		mWOStatusID = CType(Session("WOStatusID"), Integer)
		mStatusIDForWO = CType(Session("StatusIDForWO"), Integer)
		mOpenFromLogFuelNew = CType(Session("mOpenFromLogFuelNew"), Boolean)

		mUpdateFuelsOfAllAboveLogs = Session("mUpdateFuelsOfAllAboveLogs") 'Saylee on 16-Nov-2011 for ALL16112012
		mFuelType = CType(Session("mFuelType"), FuelType)  'Added By Shweta On 14-June-2013 For  ALL05062013
		mFuelTypeList = CType(Session("mFuelTypeList"), FuelTypeList) 'Added By Shweta On 14-June-2013 For  ALL05062013
		mnWO = Session("mnWO")
	End Sub
	Private Sub SetSession()
		Session("mLog") = mLog
		Session("mMachine") = mMachine
		Session("mTankList") = mTankList
		Session("mFuelUpliftUnit") = mFuelUpliftUnit
		Session("OpenFromWO") = mOpenFromWO
		Session("mWOStatusID") = mWOStatusID
		Session("mStatusIDForWO") = mStatusIDForWO
		Session("mOpenFromLogFuelNew") = mOpenFromLogFuelNew

		Session("mUpdateFuelsOfAllAboveLogs") = mUpdateFuelsOfAllAboveLogs 'Saylee on 16-Nov-2011 for ALL16112012
		Session("mFuelType") = mFuelType
		Session("FuelTypeList") = mFuelTypeList
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mFuelUpliftUnit")
		Session.Remove("mTankList")
		Session.Remove("mMachine")
		Session.Remove("mWOStatusID")
		Session.Remove("mStatusIDForWO")
		Session.Remove("mOpenFromLogFuelNew")
		Session.Remove("mFuelType")
		Session.Remove("mFuelTypeList")
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
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Try
			Dim str As String
			'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
			'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
			str = "document.getElementById('" + cntrl.ClientID + "').focus();"
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
		Catch ex As Exception
			'
		End Try
	End Sub
	Private Sub NewRecord()
		mLog = Log.NewLog(mMachine, Today.Date)
		Session("mLog") = mLog
		lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
	End Sub
	Private Sub SetObject()
		mLog.FuelUpLifts.CurrentItem.UpLift = CDec(Val(txtTotalFuelUplift.Text)) 'txtFuelUplift
		mLog.FuelUpLifts.CurrentItem.UnitID = CInt(cmbFuelUpliftUnit.SelectedValue)

		'Added By Utkarsh ON 31-Aug-2012 FOR ALL-30082012
		mLog.FuelUpLifts.CurrentItem.TOWeight = txtTOWeight.Text.Trim
		mLog.FuelUpLifts.CurrentItem.Altitude = txtAltitude.Text.Trim
		mLog.FuelUpLifts.CurrentItem.Remark = txtRemark.Text.Trim
		'End
		mLog.FuelUpLifts.CurrentItem.FuelTypeID = New Guid(cmbFuelType.SelectedValue.ToString)  'Added By Shweta On 14-June-2013 For  ALL05062013
		Session("mLog") = mLog
	End Sub
	Public Function IsEngineHoursSame() As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
		Dim IsSame As Boolean = True
		For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
			'If mLog.LogAFAssemblies(0).Hours = mLog.LogEngAssemblies(i).Hours Then
			If mLog.TotalTime = mLog.LogEngAssemblies(i).Hours Then
				IsSame = True
			Else
				IsSame = False
				Exit For
			End If
		Next
		If IsSame = True Then
			Return True
		Else
			Return False
		End If
	End Function
	Public Function IsCGBHoursSame() As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
		Dim IsSame As Boolean = True
		''If mLog.LogCGBAssemblies Is Nothing Then
		''    Return True
		''End If
		For i As Integer = 0 To mLog.LogCGBAssemblies.Count - 1
			'If mLog.LogAFAssemblies(0).Hours = mLog.LogCGBAssemblies(i).Hours Then
			If mLog.TotalTime = mLog.LogCGBAssemblies(i).Hours Then
				IsSame = True
			Else
				IsSame = False
				Exit For
			End If
		Next
		If IsSame = True Then
			Return True
		Else
			Return False
		End If
	End Function
	Public Sub SetGridObject()        ' For First Grid i.e AirFrame
		Dim txtFuelUpLifted, txtFuelAtArrival As TextBox
		Dim txtWOFuelUpLifted, txtWOFuelDrainedOut As TextBox

		Dim txtBurnOnGround As TextBox  'Added By Utkarsh ON 31-Aug-2012 FOR ALL-30082012

		' '' ''For i As Integer = 0 To Me.dgLogFuel.Items.Count - 1
		' '' ''    txtFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelUpLifted"), TextBox)
		' '' ''    txtFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelAtArrival"), TextBox)

		' '' ''    txtWOFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("txtWOFuelUpLifted"), TextBox)
		' '' ''    txtWOFuelDrainedOut = CType(Me.dgLogFuel.Items(i).FindControl("txtWOFuelDrainedOut"), TextBox)

		' '' ''    mLog.LogFuels.Item(i).FuelUplifted = Val(txtFuelUpLifted.Text.Trim)
		' '' ''    mLog.LogFuels.Item(i).FuelOnArrival = Val(txtFuelAtArrival.Text.Trim)

		' '' ''    mLog.LogFuels.Item(i).WOFuelUplifted = Val(txtWOFuelUpLifted.Text.Trim)
		' '' ''    mLog.LogFuels.Item(i).WOFuelDrainedOut = Val(txtWOFuelDrainedOut.Text.Trim)

		' '' ''    'Added By Utkarsh ON 31-Aug-2012 FOR ALL-30082012
		' '' ''    txtBurnOnGround = CType(Me.dgLogFuel.Items(i).FindControl("txtBurnOnGround"), TextBox)
		' '' ''    mLog.LogFuels.Item(i).BurnOnGround = Val(txtBurnOnGround.Text.Trim)
		' '' ''    'End

		' '' ''Next i

		For i As Integer = 0 To Me.dgLogFuel.Rows.Count - 1
			txtFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelUpLifted"), TextBox)
			txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)

			txtWOFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelUpLifted"), TextBox)
			txtWOFuelDrainedOut = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelDrainedOut"), TextBox)

			mLog.LogFuels.Item(i).FuelUplifted = Val(txtFuelUpLifted.Text.Trim)
			mLog.LogFuels.Item(i).FuelOnArrival = Val(txtFuelAtArrival.Text.Trim)

			mLog.LogFuels.Item(i).WOFuelUplifted = Val(txtWOFuelUpLifted.Text.Trim)
			mLog.LogFuels.Item(i).WOFuelDrainedOut = Val(txtWOFuelDrainedOut.Text.Trim)

			'Added By Utkarsh ON 31-Aug-2012 FOR ALL-30082012
			txtBurnOnGround = CType(Me.dgLogFuel.Rows(i).FindControl("txtBurnOnGround"), TextBox)
			mLog.LogFuels.Item(i).BurnOnGround = Val(txtBurnOnGround.Text.Trim)
			'End

		Next i

		Dim txtValue As TextBox
		Dim txtUpdatedDate, txtUpdatedTime As TextBox  'Added By Vikrant On 21-Dec-2018 For ALL21122018
		' '' ''For i As Integer = 0 To Me.dgLogOil.Items.Count - 1
		' '' ''    txtValue = CType(Me.dgLogOil.Items(i).FindControl("txtValue"), TextBox)
		' '' ''    mLog.LogOils.Item(i).Value = Val(txtValue.Text.Trim)
		' '' ''Next i   
		For i As Integer = 0 To Me.dgLogOil.Rows.Count - 1
			txtValue = CType(Me.dgLogOil.Rows(i).FindControl("txtValue"), TextBox)

			mLog.LogOils.Item(i).Value = Val(txtValue.Text.Trim)
			'Added By Vikrant On 21-Dec-2018 For ALL21122018
			txtUpdatedDate = CType(Me.dgLogOil.Rows(i).FindControl("txtUpdatedDate"), TextBox)
			txtUpdatedTime = CType(Me.dgLogOil.Rows(i).FindControl("txtTime"), TextBox)
			If txtUpdatedTime.Text <> "" Then
				mLog.LogOils.Item(i).OilUpdatedDateTime = CType(txtUpdatedDate.Text.ToString.Trim + " " + txtUpdatedTime.Text.ToString.Trim, DateTime)
			Else
				mLog.LogOils.Item(i).OilUpdatedDateTime = System.DBNull.Value
			End If
			'End
		Next i

		Session("mLog") = mLog
	End Sub
	Public Function SaveLogAfterHrsSame() As Boolean
		Dim LogClone As Log
		Dim mtmpLog As Log

		LogClone = CType(mLog.Clone, Log)
		SetObject()
		SetGridObject()

		'Added by Saylee on 2-May2011
		If Not mLog.IsNew Then
			Dim mUpperLogNo As MaxLogNo
			mUpperLogNo = MaxLogNo.GetUpperLog(mLog.ID, mLog.MachineID)   'Gets the just immediate upper log
			If mUpperLogNo IsNot Nothing Then
				If mUpperLogNo.Count > 0 Then
					mtmpLog = Log.GetLog(mUpperLogNo(0).LogId)
					Dim txtFuelAtArrival As TextBox
					Dim txtWOFuelUpLifted As TextBox
					Dim txtWOFuelDrainedOut As TextBox
					For i As Integer = 0 To mLog.LogFuels.Count - 1
						'''''txtFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelAtArrival"), TextBox)
						txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)
						txtWOFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelUpLifted"), TextBox)
						txtWOFuelDrainedOut = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelDrainedOut"), TextBox)

						mtmpLog.LogFuels.Item(i).FuelOnDeparture = (Val(txtFuelAtArrival.Text.Trim) + Val(txtWOFuelUpLifted.Text.Trim)) - Val(txtWOFuelDrainedOut.Text.Trim)

					Next i
				End If
			End If
		End If

		If mLog.IsValid = True Then
			Try

				If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
					If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
				   Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
						'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly)
						'''''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
						'''''msg1.Show()
						MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction, MSGBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly, "")
						Return False
					End If
				End If
				'End

				mLog.ApplyEdit()
				mLog = CType(mLog.Save(), Log)
				Dim mLogDetail As String
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				'MarkLog(Util.Action.[New], "Flight Log", mLogDetail + " Tank : " + mTankList.Item(mTankList.CurrentIndex).Name, Util.ErrorType.NoError, New Guid(cmbTankList.SelectedValue.ToString))
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added By Prashant 23-Aug-2018
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Log Fuel Oil", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)

				Session("mLog") = mLog

				'Commented by Saylee on 16-Nov-2012 for ALL16112012
				''''''To save the Immediate Upper Log
				''''Try

				''''    If Not mtmpLog Is Nothing Then
				''''        If mtmpLog.IsValid = True Then
				''''            mtmpLog.ApplyEdit()
				''''            mtmpLog = CType(mtmpLog.Save(), Log)
				''''        End If
				''''    End If

				''''Catch ex As Exception

				''''End Try
				'''''***********************************

				'Added by Saylee on 16-Nov-2012 for ALL16112012
				Try
					Dim mUpdateFuelsOfAllAboveLogsInfo As UpdateFuelsOfAllAboveLogs.UpdateFuelsOfAllAboveLogsInfo
					Dim mtmpLogFuelList As LogFuelList
					If mUpdateFuelsOfAllAboveLogs.Count > 0 Then
						For Each mUpdateFuelsOfAllAboveLogsInfo In mUpdateFuelsOfAllAboveLogs
							mtmpLogFuelList = LogFuelList.GetLogFuelList(mUpdateFuelsOfAllAboveLogsInfo.ID)
							Dim txtFuelAtArrival As TextBox
							'For i As Integer = 0 To mLog.LogFuels.Count - 1
							For i As Integer = 0 To mtmpLogFuelList.Count - 1
								''''' txtFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelAtArrival"), TextBox)
								txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)
								mUpdateFuelsOfAllAboveLogs.UpdateLogFuels(mtmpLogFuelList(i).LogFuelId, Val(txtFuelAtArrival.Text.Trim), mUpdateFuelsOfAllAboveLogsInfo)
							Next i
						Next
					Else
						'Save just immediate upper log
						If mtmpLog IsNot Nothing Then
							If mtmpLog.IsValid = True Then
								mtmpLog.ApplyEdit()
								mtmpLog = CType(mtmpLog.Save(), Log)
							End If
						End If
					End If


				Catch ex As Exception

				End Try
				''End of ALL16112012
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, "Rate or Qty or Conversion Factor.", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				End If
				Return False
			Finally
				LogClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function
	Private Function Save() As Boolean
		Dim LogClone As Log
		Dim mtmpLog As Log

		LogClone = CType(mLog.Clone, Log)
		SetObject()
		SetGridObject()

		'Added by Saylee on 2-May2011
		If Not mLog.IsNew Then
			Dim mUpperLogNo As MaxLogNo
			mUpperLogNo = MaxLogNo.GetUpperLog(mLog.ID, mLog.MachineID)   'Gets the just immediate upper log
			If mUpperLogNo IsNot Nothing Then
				If mUpperLogNo.Count > 0 Then
					mtmpLog = Log.GetLog(mUpperLogNo(0).LogId)
					Dim txtFuelAtArrival As TextBox
					Dim txtWOFuelUpLifted As TextBox
					Dim txtWOFuelDrainedOut As TextBox
					For i As Integer = 0 To mLog.LogFuels.Count - 1
						'''''txtFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelAtArrival"), TextBox)
						txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)
						txtWOFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelUpLifted"), TextBox)
						txtWOFuelDrainedOut = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelDrainedOut"), TextBox)

						mtmpLog.LogFuels.Item(i).FuelOnDeparture = (Val(txtFuelAtArrival.Text.Trim) + Val(txtWOFuelUpLifted.Text.Trim)) - Val(txtWOFuelDrainedOut.Text.Trim)
					Next i
				End If
			End If
		End If
		If mLog.IsValid = True Then
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
			Try
				If IsEngineHoursSame() = False Or IsCGBHoursSame() = False Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''Session("sender") = "SaveLogAfterHrsSame"
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
					Exit Function
				End If

				mLog.ApplyEdit()
				mLog = CType(mLog.Save(), Log)
				'MarkLog(Util.Action.[New], "Log", "Aircraft Name ->" + mLog.Machine.RegNo + " Tank-> " + mTankList.Item(mTankList.CurrentIndex).Name, Util.ErrorType.NoError, New Guid(cmbTankList.SelectedValue.ToString))
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added By Prashant 23-Aug-2018
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Log Fuel Oil", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)

				Session("mLog") = mLog

				'Commented by Saylee on 16-Nov-2012 for ALL16112012
				''''To save the Immediate Upper Log
				''Try

				''    If Not mtmpLog Is Nothing Then
				''        If mtmpLog.IsValid = True Then
				''            mtmpLog.ApplyEdit()
				''            mtmpLog = CType(mtmpLog.Save(), Log)
				''        End If
				''    End If
				''Catch ex As Exception

				''End Try
				'***********************************

				'Added by Saylee on 16-Nov-2012 for ALL16112012
				''To save the all Upper Logs
				Try
					Dim mUpdateFuelsOfAllAboveLogsInfo As UpdateFuelsOfAllAboveLogs.UpdateFuelsOfAllAboveLogsInfo
					Dim mtmpLogFuelList As LogFuelList
					If mUpdateFuelsOfAllAboveLogs.Count > 0 Then
						For Each mUpdateFuelsOfAllAboveLogsInfo In mUpdateFuelsOfAllAboveLogs
							mtmpLogFuelList = LogFuelList.GetLogFuelList(mUpdateFuelsOfAllAboveLogsInfo.ID)
							Dim txtFuelAtArrival As TextBox
							'For i As Integer = 0 To mLog.LogFuels.Count - 1
							For i As Integer = 0 To mtmpLogFuelList.Count - 1
								'''''txtFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelAtArrival"), TextBox)
								txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)
								mUpdateFuelsOfAllAboveLogs.UpdateLogFuels(mtmpLogFuelList(i).LogFuelId, Val(txtFuelAtArrival.Text.Trim), mUpdateFuelsOfAllAboveLogsInfo)
							Next i
						Next
					Else
						Try

							If mtmpLog IsNot Nothing Then
								If mtmpLog.IsValid = True Then
									mtmpLog.ApplyEdit()
									mtmpLog = CType(mtmpLog.Save(), Log)
								End If
							End If
						Catch ex As Exception

						End Try
					End If


				Catch ex As Exception

				End Try
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, " ")
				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, " ")
				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, " ")
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, " ")
				End If
				Return False
			Finally
				LogClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		' '' ''If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
		' '' ''    Result1 = -1
		' '' ''Else
		' '' ''    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		' '' ''End If

		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					' '' ''If CType(Session("sender"), String) = "MEL" Then
					If MSGBoxCtrl.Sender = "MEL" Then
						' '' ''Session("sender") = ""
						mLog = Session("mLog")
						DataFieldBind()
						'DataBind()
						If mLog.IsValid Then
							If Save() = True Then
								mLog = Log.GetLog(mLog.ID)
								mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
								mLog.IsTLP = mMachine.IsTLP 'Added By Prashant 23-Aug-2018
								Session("mLog") = mLog
								' '' ''Response.Redirect("wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
								DataFieldBind()
							End If
						End If
						' '' ''ElseIf CType(Session("sender"), String) = "SaveLogAfterHrsSame" Then    'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
					ElseIf MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then
						' '' ''Session("sender") = ""
						mLog = Session("mLog")
						DataFieldBind()
						'DataBind()
						If mLog.IsValid Then
							If SaveLogAfterHrsSame() = True Then
								mLog = Log.GetLog(mLog.ID)
								mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
								mLog.IsTLP = mMachine.IsTLP 'Added By Prashant 23-Aug-2018
								Session("mLog") = mLog
								' '' ''Response.Redirect("wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
								DataFieldBind()
							End If
						End If
					Else
						Session("sender") = ""
						' '' ''Response.Redirect("wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
						DataFieldBind()
					End If
				Case MsgBoxResult.No
					Session("sender") = ""
					' '' ''Response.Redirect("wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
					DataFieldBind()
				Case MsgBoxResult.Cancel
					Session("sender") = ""
					' '' ''Response.Redirect("wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
					DataFieldBind()
				Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
					Session("sender") = ""
					DataFieldBind()
					' '' ''Response.Redirect("wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
					DataFieldBind()
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					Session("sender") = ""
					DataFieldBind()
					' '' ''Response.Redirect("wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
					DataFieldBind()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			' '' ''Response.Redirect("wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
			DataFieldBind()
		ElseIf Result1 = 0 Then   'Code Added
			Session("sender") = ""
			' DataFieldBind()
		End If
	End Sub
	Private Sub SetTitle()
		If mLog.IsNew Then 'New SmartDate(mLog.Date.ToString).FormattedText
			lblTitle.Text = "Log Details of " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
		Else
			lblTitle.Text = "Log Details of " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
		End If

		'lblFuelOilUnit2.Text = Val(txtTotalFuelUplift.Text) 'mLog.FuelUpLifts.CurrentItem.UpLift
		lblFuelOilUnit1.Text = UnitListMain.GetUnitList()(mMachine.UnitID).Name
		lblFuelOilUnit2.Text = (mLog.FuelUpLifts.CurrentItem.CUpLift).ToString + " " + lblFuelOilUnit1.Text
	End Sub
	Private Sub addAttributes()
		txtTotalFuelUplift.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtTotalFuelUplift').value,event)")
		lblFuelOilUnit2.Text = Val(txtTotalFuelUplift.Text.Trim)
	End Sub
	Private Sub ControlVisibility()

		'Added By Utkarsh On 06-Mar-2012
		btnParameterList.Visible = IIf(mMachine.IsTLP = True, False, True) And mLog.LogTypeID = 1
		'btnLogPax.Visible = IIf(mLog.IsTLP = True, False, True) And mLog.LogTypeID = 1
		btnHobbsOffset.Visible = IIf(mMachine.IsTLP = True Or AppSettings("ShowExtraLogTabs") = "False", False, True) And mLog.LogTypeID = 1
		btnFlightCrew.Visible = IIf(mMachine.IsTLP = True Or AppSettings("LogDetailPage") = "NewPage", True, False) 'Added By Utkarsh ON 16-Jul-2012 FOR ALL16072012-3
		'End
		btnMaintenanceAcitvity.Visible = IIf(mMachine.IsTLP = True Or AppSettings("LogDetailPage") = "NewPage", True, False)  'Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013 

		btnDefectActionList.Visible = (mLog.LogTypeID = 1)
		btnMaintenanceAcitvity.Visible = (mLog.LogTypeID = 1)


		btnLogPax.Enabled = Not mLog.IsNew
		btnDefectActionList.Enabled = Not mLog.IsNew
		btnHobbsOffset.Enabled = (mMachine.HourType = 2)
		'Commented By Prashant 08-Dec-2009 because Taal even want it  
		'If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" OR AppSettings("ClientCode") = "GlobalJet") Then
		'    lblTotalFuelUpliftedInTank.Visible = False
		'    txtTotalFuelUplift.Visible = False
		'    cmbFuelUpliftUnit.Visible = False
		'    lblFuelOilUnit2.Visible = False
		'Else
		'    lblTotalFuelUpliftedInTank.Visible = True
		'    txtTotalFuelUplift.Visible = True
		'    cmbFuelUpliftUnit.Visible = True
		'    lblFuelOilUnit2.Visible = True
		'End If

		'Added by Saylee on 14-Dec-2010
		Dim txtWOFuelUpLifted, txtWOFuelDrainedOut As TextBox
		' '' ''Dim btnWOFuelUpLifted, btnWOFuelDrainedOut As Button
		Dim txtFuelUpLifted, txtFuelAtArrival As TextBox
		' '' ''Dim btnFuelUpLifted, btnFuelAtArrival As Button

		'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
		Dim txtBurnOnGround As TextBox
		' '' ''Dim btnBurnOnGround As Button
		'End

		For i As Integer = 0 To Me.dgLogFuel.Rows.Count - 1
			' '' ''For i As Integer = 0 To Me.dgLogFuel.Items.Count - 1
			' '' ''txtFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelUpLifted"), TextBox)
			' '' ''txtFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelAtArrival"), TextBox)

			' '' ''txtWOFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("txtWOFuelUpLifted"), TextBox)
			' '' ''txtWOFuelDrainedOut = CType(Me.dgLogFuel.Items(i).FindControl("txtWOFuelDrainedOut"), TextBox)

			' '' ''btnFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("btnFuelUpLifted"), Button)
			' '' ''btnFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("btnFuelOnArrival"), Button)

			' '' ''btnWOFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("btnWOFuelUpLifted"), Button)
			' '' ''btnWOFuelDrainedOut = CType(Me.dgLogFuel.Items(i).FindControl("btnWOFuelDrainedOut"), Button)

			'' '' ''Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
			' '' ''txtBurnOnGround = CType(Me.dgLogFuel.Items(i).FindControl("txtBurnOnGround"), TextBox)
			' '' ''btnBurnOnGround = CType(Me.dgLogFuel.Items(i).FindControl("btnBurnOnGround"), Button)
			'' '' ''End grdLogFuel 

			txtFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelUpLifted"), TextBox)
			txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)

			txtWOFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelUpLifted"), TextBox)
			txtWOFuelDrainedOut = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelDrainedOut"), TextBox)

			' '' ''btnFuelUpLifted = CType(Me.grdLogFuel.Rows(i).FindControl("btnFuelUpLifted"), Button)
			' '' ''btnFuelAtArrival = CType(Me.grdLogFuel.Rows(i).FindControl("btnFuelOnArrival"), Button)

			' '' ''btnWOFuelUpLifted = CType(Me.grdLogFuel.Rows(i).FindControl("btnWOFuelUpLifted"), Button)
			' '' ''btnWOFuelDrainedOut = CType(Me.grdLogFuel.Rows(i).FindControl("btnWOFuelDrainedOut"), Button)

			'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
			txtBurnOnGround = CType(Me.dgLogFuel.Rows(i).FindControl("txtBurnOnGround"), TextBox)
			' '' ''btnBurnOnGround = CType(Me.grdLogFuel.Rows(i).FindControl("btnBurnOnGround"), Button)
			'End  

			If mOpenFromWO = False Then
				txtWOFuelUpLifted.Enabled = False
				txtWOFuelDrainedOut.Enabled = False
				' '' ''btnWOFuelUpLifted.Enabled = False
				' '' ''btnWOFuelDrainedOut.Enabled = False

				txtFuelUpLifted.Enabled = True
				txtFuelAtArrival.Enabled = True
				' '' ''btnFuelUpLifted.Enabled = True
				' '' ''btnFuelAtArrival.Enabled = True

				'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
				txtBurnOnGround.Enabled = True
				' '' ''btnBurnOnGround.Enabled = True
				lblTOWeight.Visible = True
				txtTOWeight.Visible = True
				lblAltitude.Visible = True
				txtAltitude.Visible = True
				lblRemark.Visible = True
				txtRemark.Visible = True
				'End

			Else
				txtWOFuelUpLifted.Enabled = True
				txtWOFuelDrainedOut.Enabled = True
				' '' ''btnWOFuelUpLifted.Enabled = True
				' '' ''btnWOFuelDrainedOut.Enabled = True

				txtFuelUpLifted.Enabled = False
				txtFuelAtArrival.Enabled = False
				' '' ''btnFuelUpLifted.Enabled = False
				' '' ''btnFuelAtArrival.Enabled = False


				''txtWOFuelUpLifted.ReadOnly = IIf(mWOStatusID <> 3, True, False) And IIf(mStatusIDForWO <> 4, True, False)
				''txtWOFuelDrainedOut.ReadOnly = IIf(mWOStatusID <> 3, True, False) And IIf(mStatusIDForWO <> 4, True, False)
				''btnWOFuelUpLifted.Enabled = IIf(mWOStatusID = 3, True, False) And IIf(mStatusIDForWO = 4, True, False)
				''btnWOFuelDrainedOut.Enabled = IIf(mWOStatusID = 3, True, False) And IIf(mStatusIDForWO = 4, True, False)

				'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
				txtBurnOnGround.Enabled = False
				' '' ''btnBurnOnGround.Enabled = False
				lblTOWeight.Visible = False
				txtTOWeight.Visible = False
				lblAltitude.Visible = False
				txtAltitude.Visible = False
				lblRemark.Visible = False
				txtRemark.Visible = False
				'End


			End If
		Next i

		If mOpenFromWO = True Or mOpenFromLogFuelNew = True Then
			'commented and added by Saylee on 29-Sep-2014
			'btnLogDetails.Enabled = False
			'btnDefectActionList.Enabled = False
			'btnParameterList.Enabled = False
			'btnLogPax.Enabled = False
			btnHobbsOffset.Enabled = False
			btnLogDetails.Visible = False
			btnDefectActionList.Visible = False
			btnParameterList.Visible = False
			'btnLogPax.Visible = False
			btnHobbsOffset.Visible = False
			lblFuelOil.Visible = False
			'**********************
			dgLogOil.Visible = IIf(mOpenFromWO = True, False, True)

			txtTotalFuelUplift.Enabled = False
			cmbFuelUpliftUnit.Enabled = False
			lblLogOil.Visible = IIf(mOpenFromWO = True, False, True)
			dgLogFuel.Enabled = IIf(mWOStatusID = 3, False, True) And IIf(mStatusIDForWO = 4, False, True)


			btnSave.Enabled = IIf(mWOStatusID = 3, False, True) And IIf(mStatusIDForWO = 4, False, True)

			'commented and added by Saylee on 29-Sep-2014
			'btnFlightCrew.Enabled = False 'U
			'btnMaintenanceAcitvity.Enabled = False 'Added by Saylee on 28-08-2012

			btnFlightCrew.Visible = False 'U
			btnMaintenanceAcitvity.Visible = False
			'******************************
			btnFuelType.Enabled = False
			btnBack.Text = "Close"
			btnBack.ToolTip = "Click to close Log Fuel Oil page"
			btnBack.Visible = True
		Else
			btnLogDetails.Enabled = True

			btnParameterList.Enabled = True
			If Not mLog.IsNew Then
				btnLogPax.Enabled = True
				btnDefectActionList.Enabled = True
			Else
				btnLogPax.Enabled = False
				btnDefectActionList.Enabled = False
			End If

			If mMachine.HourType = 2 Then
				btnHobbsOffset.Enabled = True
			End If

			dgLogOil.Visible = True

			txtTotalFuelUplift.Enabled = True
			cmbFuelUpliftUnit.Enabled = True
			lblLogOil.Visible = (mLog.LogTypeID = 1)

			btnFlightCrew.Enabled = True 'Added By Utkarsh ON 17-Jul-2012 FOR ALL16072012-3
			btnMaintenanceAcitvity.Enabled = True 'Added by Saylee on 28-08-2012

			'''''btnSave.Visible = (dgLogOil.Items.Count > 0) Or (dgLogFuel.Items.Count > 0)
			btnSave.Visible = (dgLogFuel.Rows.Count > 0) Or (dgLogFuel.Rows.Count > 0)
			btnFuelType.Enabled = True
			'   btnBack.Visible = False


		End If
		btnLogDetails.Visible = (mLog.LogTypeID = 1) And (mOpenFromWO = False And mOpenFromLogFuelNew = False)
		btnFlightCrew.Visible = (mLog.LogTypeID = 1) And (mOpenFromWO = False And mOpenFromLogFuelNew = False)
	End Sub
	Private Sub ControlVisibilityOnWO()

		'Added by Saylee on 14-Dec-2010
		Dim txtWOFuelUpLifted, txtWOFuelDrainedOut As TextBox
		' '' ''Dim btnWOFuelUpLifted, btnWOFuelDrainedOut As Button
		Dim txtFuelUpLifted, txtFuelAtArrival As TextBox
		' '' ''Dim btnFuelUpLifted, btnFuelAtArrival As Button

		'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
		Dim txtBurnOnGround As TextBox
		' '' ''Dim btnBurnOnGround As Button
		'End   

		For i As Integer = 0 To Me.dgLogFuel.Rows.Count - 1
			' '' ''txtFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelUpLifted"), TextBox)
			' '' ''txtFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("txtFuelAtArrival"), TextBox)

			' '' ''txtWOFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("txtWOFuelUpLifted"), TextBox)
			' '' ''txtWOFuelDrainedOut = CType(Me.dgLogFuel.Items(i).FindControl("txtWOFuelDrainedOut"), TextBox)

			' '' ''btnFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("btnFuelUpLifted"), Button)
			' '' ''btnFuelAtArrival = CType(Me.dgLogFuel.Items(i).FindControl("btnFuelOnArrival"), Button)

			' '' ''btnWOFuelUpLifted = CType(Me.dgLogFuel.Items(i).FindControl("btnWOFuelUpLifted"), Button)
			' '' ''btnWOFuelDrainedOut = CType(Me.dgLogFuel.Items(i).FindControl("btnWOFuelDrainedOut"), Button)

			'' '' ''Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
			' '' ''txtBurnOnGround = CType(Me.dgLogFuel.Items(i).FindControl("txtBurnOnGround"), TextBox)
			' '' ''btnBurnOnGround = CType(Me.dgLogFuel.Items(i).FindControl("btnBurnOnGround"), Button)
			'' '' ''End

			txtFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelUpLifted"), TextBox)
			txtFuelAtArrival = CType(Me.dgLogFuel.Rows(i).FindControl("txtFuelAtArrival"), TextBox)

			txtWOFuelUpLifted = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelUpLifted"), TextBox)
			txtWOFuelDrainedOut = CType(Me.dgLogFuel.Rows(i).FindControl("txtWOFuelDrainedOut"), TextBox)

			' '' ''btnFuelUpLifted = CType(Me.grdLogFuel.Rows(i).FindControl("btnFuelUpLifted"), Button)
			' '' ''btnFuelAtArrival = CType(Me.grdLogFuel.Rows(i).FindControl("btnFuelOnArrival"), Button)

			' '' ''btnWOFuelUpLifted = CType(Me.grdLogFuel.Rows(i).FindControl("btnWOFuelUpLifted"), Button)
			' '' ''btnWOFuelDrainedOut = CType(Me.grdLogFuel.Rows(i).FindControl("btnWOFuelDrainedOut"), Button)

			'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
			txtBurnOnGround = CType(Me.dgLogFuel.Rows(i).FindControl("txtBurnOnGround"), TextBox)
			' '' ''btnBurnOnGround = CType(Me.grdLogFuel.Rows(i).FindControl("btnBurnOnGround"), Button)
			'End

			If mOpenFromWO = False Then
				txtWOFuelUpLifted.Enabled = False
				txtWOFuelDrainedOut.Enabled = False
				' '' ''btnWOFuelUpLifted.Enabled = False
				' '' ''btnWOFuelDrainedOut.Enabled = False

				txtFuelUpLifted.Enabled = True
				txtFuelAtArrival.Enabled = True
				' '' ''btnFuelUpLifted.Enabled = True
				' '' ''btnFuelAtArrival.Enabled = True

				'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
				txtBurnOnGround.Enabled = True
				' '' ''btnBurnOnGround.Enabled = True
				lblTOWeight.Visible = True
				txtTOWeight.Visible = True
				lblAltitude.Visible = True
				txtAltitude.Visible = True
				lblRemark.Visible = True
				txtRemark.Visible = True
				'End


			Else
				txtWOFuelUpLifted.Enabled = True
				txtWOFuelDrainedOut.Enabled = True
				' '' ''btnWOFuelUpLifted.Enabled = True
				' '' ''btnWOFuelDrainedOut.Enabled = True

				txtFuelUpLifted.Enabled = False
				txtFuelAtArrival.Enabled = False
				' '' ''btnFuelUpLifted.Enabled = False
				' '' ''btnFuelAtArrival.Enabled = False


				'Added By Utkarsh ON 03-Sep-2012 FOR ALL-30082012
				txtBurnOnGround.Enabled = False
				' '' ''btnBurnOnGround.Enabled = False
				lblTOWeight.Visible = False
				txtTOWeight.Visible = False
				lblAltitude.Visible = False
				txtAltitude.Visible = False
				lblRemark.Visible = False
				txtRemark.Visible = False
				'End
				''txtWOFuelUpLifted.ReadOnly = IIf(mWOStatusID <> 3, True, False) And IIf(mStatusIDForWO <> 4, True, False)
				''txtWOFuelDrainedOut.ReadOnly = IIf(mWOStatusID <> 3, True, False) And IIf(mStatusIDForWO <> 4, True, False)
				''btnWOFuelUpLifted.Enabled = IIf(mWOStatusID = 3, True, False) And IIf(mStatusIDForWO = 4, True, False)
				''btnWOFuelDrainedOut.Enabled = IIf(mWOStatusID = 3, True, False) And IIf(mStatusIDForWO = 4, True, False)
			End If
		Next i

		If mOpenFromWO = True Or mOpenFromLogFuelNew = True Then
			btnLogDetails.Enabled = False
			btnDefectActionList.Enabled = False
			btnParameterList.Enabled = False
			btnLogPax.Enabled = False
			btnHobbsOffset.Enabled = False
			dgLogOil.Visible = IIf(mOpenFromWO = True, False, True)

			txtTotalFuelUplift.Enabled = False
			cmbFuelUpliftUnit.Enabled = False
			lblLogOil.Visible = IIf(mOpenFromWO = True, False, True)

			dgLogFuel.Enabled = IIf(mWOStatusID = 3, False, True) And IIf(mStatusIDForWO = 4, False, True)



			btnSave.Enabled = IIf(mWOStatusID = 3, False, True) And IIf(mStatusIDForWO = 4, False, True)
			btnMaintenanceAcitvity.Enabled = False

		Else
			btnLogDetails.Enabled = True
			btnDefectActionList.Enabled = True
			btnParameterList.Enabled = True
			btnLogPax.Enabled = True
			btnHobbsOffset.Enabled = True
			dgLogOil.Visible = True

			txtTotalFuelUplift.Enabled = True
			cmbFuelUpliftUnit.Enabled = True
			lblLogOil.Visible = True
			btnMaintenanceAcitvity.Enabled = True
		End If
	End Sub
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
	Private Function callZeroDifferenceValue(ByVal obj) As Boolean
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
	'Added By Vikrant On 21-Dec-2018 For ALL21122018
	Private Function IsValidTime(ByVal TimeValue As String) As Boolean
		Dim TimeRegulerExpression As String = ""
		If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
			'TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm)$"    '12 Hour Format
			TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
		Else
			TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
		End If

		If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
			Return True
		Else
			Return False
		End If
	End Function
	'End
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mTankList = TankList.GetTankList(mLog.MachineID, "<SELECT>")
		mFuelUpliftUnit = UnitListMain.GetUnitList(, "<SELECT>")
		cmbTankList.DataSource = mTankList
		cmbFuelUpliftUnit.DataSource = mFuelUpliftUnit
		Session("mTankList") = mTankList
		Session("mFuelUpliftUnit") = mFuelUpliftUnit
		dgLogFuel.DataSource = mLog.LogFuels

		dgLogOil.DataSource = mLog.LogOils


		'Added by Saylee on 16-Nov-2012 for ALL16112012
		mUpdateFuelsOfAllAboveLogs = UpdateFuelsOfAllAboveLogs.GetLogFuelAndOilList(mLog.ID, mLog.MachineID)
		Session("mUpdateFuelsOfAllAboveLogs") = mUpdateFuelsOfAllAboveLogs

		'Added By Shweta On 14-June-2013 For  ALL05062013
		mFuelTypeList = FuelTypeList.GetFuelTypeList("", "<SELECT>")
		cmbFuelType.DataSource = mFuelTypeList
		Session("mFuelTypeList") = mFuelTypeList
		'
		DataBind()
	End Sub
	Private Sub DataBindGrid()
		dgLogFuel.DataSource = mLog.LogFuels
		dgLogFuel.DataBind()

		dgLogOil.DataSource = mLog.LogOils
		dgLogOil.DataBind()

		'txtTotalFuelUplift.DataBind()
		txtTotalFuelOnDeparture.DataBind()
		txtTotalFuelOnArrival.DataBind()
		txtTotalFuelConsumption.DataBind()
		Session("mLog") = mLog
	End Sub
	Private Function TankListVaidate() As Boolean
		Return cmbTankList.SelectedIndex > 0
	End Function
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "cmbTankList" And cmbTankList.SelectedIndex = 0 Then
			custValidator.ErrorMessage = "Select tank form List."
			e.IsValid = False
		ElseIf custValidator.ControlToValidate = "cmbFuelUpliftUnit" And cmbFuelUpliftUnit.SelectedIndex = 0 Then
			custValidator.ErrorMessage = "Select fuel up lift unit List."
			e.IsValid = False
		Else
			e.IsValid = True
		End If
	End Sub
	Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs) ' Validation From AIRFRAMEGRID (Grid-1)
		If Flag = 1 Then Exit Sub
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		SetObject()
		SetGridObject()
		Dim str As String = ""
		'Log
		If Not mLog.IsValid Then
			For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
				str = str + mLog.GetBrokenRulesCollection(i).Description + "<BR>"
			Next
		End If
		'Log Oils
		For i As Integer = 0 To mLog.LogOils.Count - 1
			If Not mLog.LogOils(i).IsValid Then
				For j As Integer = 0 To mLog.LogOils(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.LogOils.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
				Next
			End If
		Next
		For i As Integer = 0 To mLog.FuelUpLifts.Count - 1
			If Not mLog.FuelUpLifts(i).IsValid Then
				For j As Integer = 0 To mLog.FuelUpLifts(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.FuelUpLifts.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
				Next
			End If
		Next
		For i As Integer = 0 To mLog.LogFuels.Count - 1
			If Not mLog.LogFuels(i).IsValid Then
				For j As Integer = 0 To mLog.LogFuels(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.LogFuels.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
				Next
			End If
		Next
		If str <> "" Then
			''custValidator.ErrorMessage = str
			''e.IsValid = False
			cvFuelUpLiftList.ErrorMessage = str
			cvFuelUpLiftList.IsValid = False
		End If
		Flag = 1
	End Sub
	'Public Function CustomValidate2() As Boolean    'For DgLog Fuel Oils
	'    Dim str As String = ""
	'    'AirFrame
	'    For i As Integer = 0 To mLog.LogParameters.Count - 1
	'        If Not mLog.LogParameters(i).IsValid Then
	'            For j As Integer = 0 To mLog.LogParameters(i).GetBrokenRulesCollection.Count - 1
	'                str = str + mLog.LogParameters.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
	'            Next
	'        End If
	'    Next
	'    If str <> "" Then
	'        cvParameterList.ErrorMessage = str
	'        cvParameterList.IsValid = False
	'        Return False
	'    End If
	'    Return True
	'End Function
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		addAttributes()

		If Not IsPostBack And CType(Session("sender"), String) = "" Then

			If txtTotalFuelUplift.Enabled = True Then
				setFocus(txtTotalFuelUplift)
			End If
			DataFieldBind()
		End If
		' '' ''MessageBoxResult()
		SetTitle()
		ControlVisibility()

	End Sub

	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		Dim IsInRoleString As String
		If mOpenFromWO = True Then
			If AppSettings("ShowNewWOFlow") = "True" Then
				If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then
					If mnWO.TransTypeID = Trans.WO145 Then
						IsInRoleString = "WOCreate"
					Else
						IsInRoleString = "CAMOWOCreate"
					End If
				ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then
					IsInRoleString = "WOPlanning"
				ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
					IsInRoleString = "WOExecution"
				ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then
					IsInRoleString = "WOCompletion"
				ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then
					IsInRoleString = "WOQCApproval"
				ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then
					IsInRoleString = "WOCAMOUpdate"
				ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then
					IsInRoleString = "WOBilling"
				End If
			Else
				'IsInRoleString = "WorkOrder"
				If mnWO.TransTypeID = Trans.WO145 Then
					IsInRoleString = "WorkOrder"
				ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then
					IsInRoleString = "SpareAssemblyWO"
				ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
					IsInRoleString = "SpareComponentWO"
				ElseIf mnWO.TransTypeID = Trans.EngineeringWO Then
					IsInRoleString = "EngineeringOrder"
				Else
					IsInRoleString = "CAMOWO"
				End If
			End If
		End If

		'Added by Saylee on 8-Apr-2014 for ALL08042014
		If ((mOpenFromLogFuelNew = False) And ((Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew))) _
			Or ((mOpenFromLogFuelNew = True) And ((Not User.IsInRole("LogFuelOilNew") And mLog.IsNew) Or (Not User.IsInRole("LogFuelOilEdit") And Not mLog.IsNew))) _
			Or ((mOpenFromWO = True) And ((Not User.IsInRole(IsInRoleString + "New") And mLog.IsNew) Or (Not User.IsInRole(IsInRoleString + "Edit") And Not mLog.IsNew))) Then
			SetObject()
			SetSession()
			'MarkLog(Util.Action.Save, "Log Fuel Oil", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Save, "Log Fuel Oil", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
			' '' ''msg.ReplacePage = "wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
			' '' ''Session("sender") = "Authorization"
			' '' ''msg.Show()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "User is not Authorised", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If Not IsValid Then
			upnlError.Update()
			Exit Sub
		End If

		If IsValid Then
			'If Not CustomValidate2() Then Exit Sub

			'Code Added By Deven on 15-01-2009 for Checking MEL Qty
			'Commented By Prashant 29-Apr-2010
			'Dim IsMELCount As Boolean = True
			'Dim MELList As Aircraft_MEL.MELList
			'MELList = Aircraft_MEL.MELList.GetMELList(mMachine.ID, Guid.Empty, mLog.Date)
			'For i As Integer = 0 To MELList.Count - 1
			'    If MELList(i).FlyMELQty <> CDec(MELList(i).CurrentMELQty) Then
			'        IsMELCount = False
			'        Exit For
			'    End If
			'Next
			'MELList = Nothing
			'----------------------------------
			'Added By Prashant 12-Apr-2010
			Dim IsMELCount As Boolean = False
			Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
			mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
			For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
				If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010
					If (CDate(mLog.Date) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
						IsMELCount = True
						Exit For
					Else
						IsMELCount = False
					End If
				End If
			Next
			mTempMELSnagCorrectiveActionList = Nothing
			'---------------------------------------------------------------
			'If IsMELCount = False Then
			If IsMELCount = True Then
				' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
				' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
				' '' ''Session("sender") = "MEL"
				' '' ''msg1.Show()
				MSGBoxCtrl.show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MEL")
				If IsValid Then
					SetObject()
					SetGridObject()
				End If
				Exit Sub
			End If
			'-------------------------------

			'-------------------------------
			'added by shital on 29-Mar-2022 for APFT
			Dim AvgFuelConsumption, AvgOilConsumption, Total, TotalfuelConsume, TotalOilConsume As Decimal
			Dim EngineName As String
			If AppSettings("Clientcode") = "APFT" Or
			   AppSettings("ClientCode") = "AAP" Then
				Dim MaxfuelLimit As String = "28 lt/hr"

				If AppSettings("ClientCode") = "AAP" Then
					MaxfuelLimit = "22.1 lt/hr"
				End If

				Dim txtFuelUpLifted, txtFuelAtArrival, txtBurnonGround, txtValue1, txtValue2 As TextBox
				TotalfuelConsume = mLog.LogFuels(0).Consumtion
				Dim BT As Decimal
				' BT = ((mLog.BlockTimeDec) / 60)
				BT = (mLog.BlockTimeDec)
				If BT <> 0 Then
					AvgFuelConsumption = (TotalfuelConsume / BT) * 60
					EngineName = mLog.LogFuels(0).TankName.ToString
					lblAvgFuelConsumption1.Text = "Avg. Fuel Consumption For Engine: " + EngineName + " is: " + AvgFuelConsumption.ToString(".##")
					lblAvgFuelConsumption1.ForeColor = Color.Black

					If (AvgFuelConsumption > 28 And AppSettings("Clientcode") = "APFT") Or (AvgFuelConsumption > 22.1 And AppSettings("Clientcode") = "AAP") Then
						' MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, " Maximum fuel consumption is 28lt/hr,you consume average fuel in current log is greater than 28lt/hr. ", MsgBoxStyle.OkOnly, " ")
						Dim str As String = "ALERT: Maximum fuel consumption is " + MaxfuelLimit
						MSGBoxCtrl.Show("Alert!", str, "This Flight Log is exceeding the above limit.", MsgBoxStyle.OkOnly, "ok")
						lblAvgFuelConsumption1.ForeColor = Color.Red
						GoTo EndIFstatement
					End If
				End If


				If dgLogFuel.Rows.Count = 2 Then
					'txtFuelUpLifted = CType(Me.dgLogFuel.Rows(1).FindControl("txtFuelUpLifted"), TextBox)
					'txtBurnonGround = CType(Me.dgLogFuel.Rows(1).FindControl("txtBurnOnGround"), TextBox)
					'txtFuelAtArrival = CType(Me.dgLogFuel.Rows(1).FindControl("txtFuelAtArrival"), TextBox)
					'Total = (Val(txtTotalFuelOnDeparture.Text) + Val(txtFuelUpLifted.Text)) - Val(txtBurnonGround.Text)
					'TotalfuelConsume = Total - Val(txtFuelAtArrival.Text)
					TotalfuelConsume = mLog.LogFuels(1).Consumtion
					If BT <> 0 Then
						AvgFuelConsumption = (TotalfuelConsume / BT) * 60
						EngineName = mLog.LogFuels(1).TankName.ToString
						lblAvgFuelConsumption2.Text = "Avg. Fuel Consumption For Engine: " + EngineName + " is: " + AvgFuelConsumption.ToString(".##")
						lblAvgFuelConsumption2.ForeColor = Color.Black

						If (AvgFuelConsumption > 28 And AppSettings("Clientcode") = "APFT") Or (AvgFuelConsumption > 22.1 And AppSettings("Clientcode") = "AAP") Then
							'  MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, " Maximum fuel consumption is 28lt/hr,you consume average fuel in current log is greater than 28lt/hr. ", MsgBoxStyle.OkOnly, " ")
							'Dim str As String = "ALERT: Maximum fuel consumption is 28 lt/hr"
							Dim str As String = "ALERT: Maximum fuel consumption is " + MaxfuelLimit
							MSGBoxCtrl.Show("Alert!", str, "This Flight Log is exceeding the above limit.", MsgBoxStyle.OkOnly, "ok")
							lblAvgFuelConsumption2.ForeColor = Color.Red
							GoTo EndIFstatement
						End If
					End If
				End If


				txtValue1 = CType(Me.dgLogOil.Rows(0).FindControl("txtValue"), TextBox)
				If BT <> 0 Then

					AvgOilConsumption = (Val(txtValue1.Text) / BT) * 60
					EngineName = mLog.LogOils(0).AssemblyName.ToString
					' lblAvgOilConsumption1.Text = "Avg.Oil Consumption For Engine :" + EngineName + "is : " + AvgOilConsumption.ToString(".##")
					Dim AvgOilConsumption0 As String
					If AvgOilConsumption > 0 Then
						AvgOilConsumption0 = AvgOilConsumption.ToString(".##")
					Else
						AvgOilConsumption0 = AvgOilConsumption.ToString
					End If
					lblAvgOilConsumption1.Text = "Avg. Oil Consumption For Engine: " + EngineName + " is: " + AvgOilConsumption0
					lblAvgOilConsumption1.ForeColor = Color.Black

					If AvgOilConsumption > 0.1 Then
						' MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, " Maximum oil consumption is 0.1,you consume average Oil in current log is greater than 0.1 lt/hr. ", MsgBoxStyle.OkOnly, " ")
						Dim str As String = "ALERT: Maximum Oil consumption is 0.1 lt/hr"
						MSGBoxCtrl.Show("Alert!", str, "This Flight Log is exceeding the above limit.", MsgBoxStyle.OkOnly, "ok")
						lblAvgOilConsumption1.ForeColor = Color.Red
						GoTo EndIFstatement
					End If

				End If
				If dgLogOil.Rows.Count = 2 Then
					txtValue2 = CType(Me.dgLogOil.Rows(1).FindControl("txtValue"), TextBox)
					If BT <> 0 Then
						AvgOilConsumption = (Val(txtValue2.Text) / BT) * 60
						EngineName = mLog.LogOils(1).AssemblyName.ToString
						' lblAvgOilConsumption2.Text = "Avg.Oil Consumption For Engine :" + EngineName + "is : " + AvgOilConsumption.ToString(".##")
						Dim AvgOilConsumption1 As String
						If AvgOilConsumption > 0 Then
							AvgOilConsumption1 = AvgOilConsumption.ToString(".##")
						Else
							AvgOilConsumption1 = AvgOilConsumption.ToString
						End If
						EngineName = mLog.LogOils(1).AssemblyName.ToString
						lblAvgOilConsumption2.Text = "Avg. Oil Consumption For Engine: " + EngineName + " is: " + AvgOilConsumption1
						lblAvgOilConsumption2.ForeColor = Color.Black

						If AvgOilConsumption > 0.1 Then
							' MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, " Maximum oil consumption is 0.1,you consume average Oil in current log is greater than 0.1 lt/hr. ", MsgBoxStyle.OkOnly, " ")
							Dim str As String = "ALERT: Maximum Oil consumption is 0.1 lt/hr"
							MSGBoxCtrl.Show("Alert!", str, "This Flight Log is exceeding the above limit.", MsgBoxStyle.OkOnly, "ok")
							lblAvgOilConsumption2.ForeColor = Color.Red
						End If
					End If
				End If

EndIFstatement:
				upnlMaxAvgFuel1.Update()
				upnlMaxAvgFuel2.Update()
				upnlMaxAvgOil1.Update()
				upnlMaxAvgOil2.Update()
				upnlDetOil.Update()
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
			End If
			'--------------------------
			'--------------------------
			If Save() = True Then
				upnlError.Update()
				' '' ''Response.Redirect("wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))                
				DataFieldBind()
			End If

		End If
	End Sub

	' '' ''Private Sub dgLogFuel_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLogFuel.ItemCommand
	' '' ''    Dim Index As Integer = e.Item.ItemIndex + dgLogFuel.CurrentPageIndex * dgLogFuel.PageSize
	' '' ''    ControlVisibility()
	' '' ''    Select Case e.CommandName
	' '' ''        Case "FuelUpLifted"
	' '' ''            Dim txtFuelUpLifted As TextBox
	' '' ''            txtFuelUpLifted = CType(Me.dgLogFuel.Items(Index).FindControl("txtFuelUpLifted"), TextBox)
	' '' ''            mLog.LogFuels(Index).FuelUplifted = Val(txtFuelUpLifted.Text)
	' '' ''            DataBindGrid()
	' '' ''            ControlVisibilityOnWO()
	' '' ''            upnlLogFuel.Update()
	' '' ''        Case "FuelOnArrival"
	' '' ''            Dim txtFuelAtArrival As TextBox
	' '' ''            txtFuelAtArrival = CType(Me.dgLogFuel.Items(Index).FindControl("txtFuelAtArrival"), TextBox)
	' '' ''            mLog.LogFuels(Index).FuelOnArrival = Val(txtFuelAtArrival.Text)
	' '' ''            DataBindGrid()
	' '' ''            ControlVisibilityOnWO()
	' '' ''        Case "WOFuelUpLifted"
	' '' ''            Dim txtWOFuelUpLifted As TextBox
	' '' ''            txtWOFuelUpLifted = CType(Me.dgLogFuel.Items(Index).FindControl("txtWOFuelUpLifted"), TextBox)
	' '' ''            mLog.LogFuels(Index).WOFuelUplifted = Val(txtWOFuelUpLifted.Text)
	' '' ''            DataBindGrid()
	' '' ''            ControlVisibilityOnWO()
	' '' ''        Case "WOFuelDrainedOut"
	' '' ''            Dim txtWOFuelDrainedOut As TextBox
	' '' ''            txtWOFuelDrainedOut = CType(Me.dgLogFuel.Items(Index).FindControl("txtWOFuelDrainedOut"), TextBox)
	' '' ''            mLog.LogFuels(Index).WOFuelDrainedOut = Val(txtWOFuelDrainedOut.Text)
	' '' ''            DataBindGrid()
	' '' ''            ControlVisibilityOnWO()
	' '' ''            'Added By Utkarsh ON 31-Aug-2012 FOR ALL-30082012
	' '' ''        Case "BurnOnGround"
	' '' ''            Dim txtBurnOnGround As TextBox
	' '' ''            txtBurnOnGround = CType(Me.dgLogFuel.Items(Index).FindControl("txtBurnOnGround"), TextBox)
	' '' ''            mLog.LogFuels.Item(Index).BurnOnGround = Val(txtBurnOnGround.Text.Trim)
	' '' ''            DataBindGrid()
	' '' ''            ControlVisibilityOnWO()
	' '' ''            'End
	' '' ''    End Select

	' '' ''End Sub

	'----------

	' '' ''Private Sub dgLogOil_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLogOil.ItemCommand
	' '' ''    Dim Index As Integer = e.Item.ItemIndex + dgLogOil.PageSize * dgLogOil.CurrentPageIndex
	' '' ''    Select Case e.CommandName
	' '' ''        Case "Value"
	' '' ''            Dim txtValue As TextBox
	' '' ''            txtValue = CType(Me.dgLogOil.Items(Index).FindControl("txtValue"), TextBox)
	' '' ''            mLog.LogOils(Index).Value = Val(txtValue.Text)
	' '' ''            DataBindGrid()
	' '' ''            ControlVisibilityOnWO()
	' '' ''    End Select
	' '' ''End Sub
	Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
			'MarkLog(Util.Action.[New], "Log Fuel Oil", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Save, "Log Fuel Oil", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
			' '' ''msg.ReplacePage = "wfLogFuelOil.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
			' '' ''Session("sender") = "Authorization"
			' '' ''msg.Show()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "User is not Authorised", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If Not TankListVaidate() Then
			cvTankList.IsValid = False
			Exit Sub
		End If
		If mLog.LogFuels.Contains(mMachine.MachineTanks(New Guid(cmbTankList.SelectedValue.ToString), "").ID) Then
			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "log fuel.", MsgBoxStyle.OKOnly)
			' '' ''msg.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
			' '' ''Session("sender") = "Delete"
			' '' ''msg.Show()
			MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "log fuel.", MsgBoxStyle.OkOnly, "Delete")
			Exit Sub
		Else
			mLog.LogFuels.Add(mLog.ID, mMachine.MachineTanks(New Guid(cmbTankList.SelectedValue), "").ID, cmbTankList.SelectedItem.Text, mMachine.UnitID)
			REM: Making explicitely dirty
			mLog.LogFuels.CurrentItem.FuelUplifted = 1
			mLog.LogFuels.CurrentItem.FuelOnArrival = 0
			dgLogFuel.DataSource = mLog.LogFuels

			' '' ''dgLogOil.DataSource = mLog.LogOils


			dgLogFuel.DataBind()

			dgLogOil.DataBind()


			txtTotalFuelOnDeparture.DataBind()
			txtTotalFuelOnArrival.DataBind()
			Session("mLog") = mLog
			'MarkLog(Util.Action.[New], "Log", "Aircraft Name ->" + mLog.Machine.RegNo + " Tank-> " + cmbTankList.SelectedItem.Text, Util.ErrorType.NoError, New Guid(cmbTankList.SelectedValue.ToString))
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Save, "Log Fuel Oil", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)

		End If
	End Sub
	Private Sub btnLogDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogDetails.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub
	Private Sub btnDefectActionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefectActionList.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnParameterList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParameterList.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		Response.Redirect("wfLogParameterList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnLogPax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogPax.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		NewLogPax()
		'Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogFuelOil_Ajax.aspx")
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnHobbsOffset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHobbsOffset.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		NewHobbsOffSet()
		Response.Redirect("wfHobbsOffset_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogFuelOil_Ajax.aspx")
	End Sub

	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		MarkLog(Util.Action.Close, "Log Fuel Oil", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		Session("mOpenFromLogFuelNew") = False
		Session.Remove("mOpenFromLogFuelNew")


		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End

		Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub


	Private Sub txtTotalFuelUplift_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTotalFuelUplift.TextChanged
		SetObject()
		lblFuelOilUnit2.Text = (mLog.FuelUpLifts.CurrentItem.CUpLift).ToString + " " + lblFuelOilUnit1.Text
	End Sub
	Private Sub cmbFuelUpliftUnit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFuelUpliftUnit.SelectedIndexChanged
		SetObject()
		setFocus(cmbFuelUpliftUnit)
		lblFuelOilUnit2.Text = (mLog.FuelUpLifts.CurrentItem.CUpLift).ToString + " " + lblFuelOilUnit1.Text
		upnlTotalFuelUpLift.Update()
	End Sub
	'Added By Prashant 22-June-2009 for grid sorting
	' '' ''Private Sub dgLogFuel_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgLogFuel.SortCommand
	' '' ''    mLog.LogFuels.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
	' '' ''    ' '' ''dgLogFuel.DataSource = mLog.LogFuels
	' '' ''    ' '' ''dgLogFuel.DataBind()
	' '' ''    grdLogFuel.DataSource = mLog.LogFuels
	' '' ''    grdLogFuel.DataBind()

	' '' ''End Sub
	Private Sub dgLogFuel_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgLogFuel.Sorted
		mLog.LogFuels.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		' '' ''dgLogFuel.DataSource = mLog.LogFuels
		' '' ''dgLogFuel.DataBind()
		dgLogFuel.DataSource = mLog.LogFuels
		dgLogFuel.DataBind()

	End Sub
	' '' ''Private Sub dgLogOil_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgLogOil.SortCommand
	' '' ''    mLog.LogOils.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
	' '' ''    dgLogOil.DataSource = mLog.LogOils
	' '' ''    dgLogOil.DataBind()
	' '' ''End Sub
	Private Sub dgLogOil_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgLogFuel.Sorted
		mLog.LogOils.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgLogFuel.DataSource = mLog.LogOils
		dgLogFuel.DataBind()
	End Sub
	'----------------------------------------------
	'Added by Utkarsh On 06-Apr-2012
	Private Sub btnFlightCrew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFlightCrew.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False

		''Added By Utkarsh ON 16-Jul-2012 FOR ALL16072012-3
		'If AppSettings("LogDetailPage") = "NewPage" Then
		'    Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")
		'ElseIf mLog.IsTLP = True Then
		'    Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfTLP_Ajax.aspx")
		'End If
		''End
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnMaintenanceAcitvity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMaintenanceAcitvity.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		''Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
		'If AppSettings("LogDetailPage") = "NewPage" Then
		'    Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx")
		'Else
		'    Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLP_Ajax.aspx")
		'End If
		''End
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	'End
	Protected Sub txtFuelUpLifted_TextChanged(ByVal sender As Object, ByVal e As EventArgs)


		Dim txtFuelUpLifted As TextBox = DirectCast(sender, TextBox)
		Dim gv1 As GridViewRow = DirectCast(txtFuelUpLifted.NamingContainer, GridViewRow)


		txtFuelUpLifted = CType(Me.dgLogFuel.Rows(gv1.RowIndex).FindControl("txtFuelUpLifted"), TextBox)
		mLog.LogFuels(gv1.RowIndex).FuelUplifted = Val(txtFuelUpLifted.Text)
		DataBindGrid()
		ControlVisibilityOnWO()
		customvalidate1(Nothing, Nothing)
		upnlError.Update()
		upnldgLogFuel.Update()

	End Sub

	Protected Sub txtFuelAtArrival_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim txtFuelAtArrival As TextBox = DirectCast(sender, TextBox)
		Dim gv1 As GridViewRow = DirectCast(txtFuelAtArrival.NamingContainer, GridViewRow)

		txtFuelAtArrival = CType(Me.dgLogFuel.Rows(gv1.RowIndex).FindControl("txtFuelAtArrival"), TextBox)
		mLog.LogFuels(gv1.RowIndex).FuelOnArrival = Val(txtFuelAtArrival.Text)
		DataBindGrid()
		ControlVisibilityOnWO()
		customvalidate1(Nothing, Nothing)
		upnlError.Update()
		upnldgLogFuel.Update()

	End Sub

	Protected Sub txtBurnOnGround_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

		Dim txtBurnOnGround As TextBox = DirectCast(sender, TextBox)
		Dim gv1 As GridViewRow = DirectCast(txtBurnOnGround.NamingContainer, GridViewRow)
		txtBurnOnGround = CType(Me.dgLogFuel.Rows(gv1.RowIndex).FindControl("txtBurnOnGround"), TextBox)
		mLog.LogFuels.Item(gv1.RowIndex).BurnOnGround = Val(txtBurnOnGround.Text.Trim)
		DataBindGrid()
		ControlVisibilityOnWO()
		customvalidate1(Nothing, Nothing)
		upnlError.Update()
		upnldgLogFuel.Update()


	End Sub

	Protected Sub txtValue_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

		'Dim txtValue As TextBox = DirectCast(sender, TextBox)
		'Dim gv1 As GridViewRow = DirectCast(txtValue.NamingContainer, GridViewRow)

		'txtValue = CType(Me.dgLogOil.Rows(gv1.RowIndex).FindControl("txtValue"), TextBox)
		'mLog.LogOils(gv1.RowIndex).Value = Val(txtValue.Text)
		'DataBindGrid()
		'ControlVisibilityOnWO()
		'UpnldgLogOil.Update()


		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent

		Dim txtValue As TextBox = TryCast(currentRow.FindControl("txtValue"), TextBox)

		mLog.LogOils.Item(currentRow.RowIndex).Value = Val(txtValue.Text)    ' Trim(txtValue.Text)   'Changed by Yogita it characters entered in value textbox
		DataBindGrid()
		ControlVisibilityOnWO()
		UpnldgLogOil.Update()
	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	''Added By Shweta On 14-June-2013 For  ALL05062013
	Private Sub btnFuelType_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnFuelType.Click
		If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
			SetObject()
			MarkLog(Util.Action.Save, "Log Fuel Oil", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		SetObject()
		Session("mFuelType") = mFuelType

		''If mLog.LogTypeID = 1 Then
		''    Response.Redirect("wfFuelType_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogFuelOil_Ajax.aspx")
		''Else
		''    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFuelOilWindow", "OpenFuelOilWindow()", True)
		''End If
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFuelOilWindow", "OpenFuelOilWindow()", True)

	End Sub
	Protected Sub txtWOFuelUpLifted_TextChanged(ByVal sender As Object, ByVal e As EventArgs)


		Dim txtWOFuelUpLifted As TextBox = DirectCast(sender, TextBox)
		Dim gv1 As GridViewRow = DirectCast(txtWOFuelUpLifted.NamingContainer, GridViewRow)


		txtWOFuelUpLifted = CType(Me.dgLogFuel.Rows(gv1.RowIndex).FindControl("txtWOFuelUpLifted"), TextBox)
		mLog.LogFuels(gv1.RowIndex).WOFuelUplifted = Val(txtWOFuelUpLifted.Text)
		DataBindGrid()
		ControlVisibilityOnWO()
		customvalidate1(Nothing, Nothing)
		upnlError.Update()
		upnldgLogFuel.Update()

	End Sub
	Protected Sub txtWOFuelDrainedOut_TextChanged(ByVal sender As Object, ByVal e As EventArgs)


		Dim txtWOFuelDrainedOut As TextBox = DirectCast(sender, TextBox)
		Dim gv1 As GridViewRow = DirectCast(txtWOFuelDrainedOut.NamingContainer, GridViewRow)


		txtWOFuelDrainedOut = CType(Me.dgLogFuel.Rows(gv1.RowIndex).FindControl("txtWOFuelDrainedOut"), TextBox)
		mLog.LogFuels(gv1.RowIndex).WOFuelDrainedOut = Val(txtWOFuelDrainedOut.Text)
		DataBindGrid()
		ControlVisibilityOnWO()
		customvalidate1(Nothing, Nothing)
		upnlError.Update()
		upnldgLogFuel.Update()

	End Sub

	Private Sub hdnBtnFuelOil_Click(sender As Object, e As System.EventArgs) Handles hdnBtnFuelOil.Click
		mFuelTypeList = FuelTypeList.GetFuelTypeList("", "<SELECT>")
		cmbFuelType.DataSource = mFuelTypeList
		Session("mFuelTypeList") = mFuelTypeList
		cmbFuelType.DataBind()
		upnlFuelType.Update()
	End Sub
	'Added By Vikrant On 21-Dec-2018 For ALL21122018
	Protected Sub txtTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim thisTextBox As TextBox = CType(sender, TextBox)
		Dim currentRow As GridViewRow = CType(thisTextBox.Parent.Parent, GridViewRow)
		Dim rowindex As Integer = 0
		rowindex = currentRow.RowIndex

		If IsValidTime(thisTextBox.Text.ToString.Trim) = False Then
			thisTextBox.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = CType(Me.dgLogOil.Rows(rowindex).FindControl("txtUpdatedDate"), TextBox).Text + " " + thisTextBox.Text.Trim
		End If
	End Sub
	'End
#End Region


End Class
