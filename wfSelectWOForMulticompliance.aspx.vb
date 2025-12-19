
'Created By :   Saylee
'Dated      :   17-Aug-2010

Partial Class wfSelectWOForMulticompliance
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents txtAsOnDate As SIControls.SICalendar
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
	'Public mWOListForCombo As FlyPal22.Maintain.WOListForCombo
	Public mWOListForCombo As nWOListForCombo
	'Public mSelectDueJobForWO As SelectDueJobForWO
	'Public mSelectDueJobsForWO As SelectDueJobsForWO

	Public mSelectDueJobForWO As SelectDueJobFornWO
	Public mSelectDueJobsForWO As SelectDueJobsFornWO

	'Public mWO As FlyPal22.Maintain.WO
	Public mWO As nWO
	Public mDueLimits As DueLimits

	Dim mLog As Log
	Dim AsonDate As String
	Dim AOnDate As String
	Dim MachineName As String
	Dim AssemblyName As String
	Public mAssemblyInfo As String
	Public mCompInfo As String
	Dim LogId As String
	Dim WOName As String

	Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
	Public mBoardInfo As AircraftInformationBoard.BoardInfo

	Public mMachineMaintenanceForAssemblyService As MachineMaintenance
	Public mMachineMaintenanceListForAssemblyService As MachineMaintenanceList

	Public mMachineMaintenanceForAssemblyInsp As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
	Public mMachineMaintenanceListForAssemblyInsp As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

	Public mMachineMaintenanceForAssemblyMod As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
	Public mMachineMaintenanceListForAssemblyMod As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

	Public mMachineMaintenanceForCompService As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
	Public mMachineMaintenanceListForCompService As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

	Public mMachineMaintenanceForCompInsp As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
	Public mMachineMaintenanceListForCompInsp As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

	Public mMachineMaintenanceForCompMod As MachineMaintenance 'Added by Saylee on 6th-Oct-2009
	Public mMachineMaintenanceListForCompMod As MachineMaintenanceList 'Added by Saylee on 6th-Oct-2009

	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mAssemblyInfoDetail As String

#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mWOListForCombo = Session("mWOListForCombo")
		AsonDate = Session("AsonDate")
		MachineName = Session("AircraftId")
		WOName = Session("WOId")

		LogId = CType(Session("LogId"), String)
		mSelectDueJobForWO = Session("mSelectDueJobForWO")
		mSelectDueJobsForWO = Session("mSelectDueJobsForWO")
		AssemblyStatusPeriodList = Session("AssemblyStatusPeriodList")

		mMachineMaintenanceForAssemblyService = CType(Session("mMachineMaintenanceForAssemblyService"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForAssemblyService = CType(Session("mMachineMaintenanceListForAssemblyService"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForAssemblyInsp = CType(Session("mMachineMaintenanceForAssemblyInsp"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForAssemblyInsp = CType(Session("mMachineMaintenanceListForAssemblyInsp"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForAssemblyMod = CType(Session("mMachineMaintenanceForAssemblyMod"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForAssemblyMod = CType(Session("mMachineMaintenanceListForAssemblyMod"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForCompService = CType(Session("mMachineMaintenanceForCompService"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForCompService = CType(Session("mMachineMaintenanceListForCompService"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForCompInsp = CType(Session("mMachineMaintenanceForCompInsp"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForCompInsp = CType(Session("mMachineMaintenanceListForCompInsp"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForCompMod = CType(Session("mMachineMaintenanceForCompMod"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForCompMod = CType(Session("mMachineMaintenanceListForCompMod"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009


	End Sub
	Private Sub SetSession()
		Session("mWOListForCombo") = mWOListForCombo
		Session("LogId") = LogId
		Session("AsonDate") = AsonDate

		Session("LogId") = LogId
		Session("AsonDate") = AsonDate
		Session("AircraftId") = MachineName

		Session("mSelectDueJobForWO") = mSelectDueJobForWO
		Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
		Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

		Session("mMachineMaintenanceForAssemblyService") = mMachineMaintenanceForAssemblyService 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForAssemblyService") = mMachineMaintenanceListForAssemblyService 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForAssemblyInsp") = mMachineMaintenanceForAssemblyInsp 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForAssemblyInsp") = mMachineMaintenanceListForAssemblyInsp 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForAssemblyMod") = mMachineMaintenanceForAssemblyMod 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForAssemblyMod") = mMachineMaintenanceListForAssemblyMod 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForCompService") = mMachineMaintenanceForCompService 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForCompService") = mMachineMaintenanceListForCompService 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForCompInsp") = mMachineMaintenanceForCompInsp 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForCompInsp") = mMachineMaintenanceListForCompInsp 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForCompMod") = mMachineMaintenanceForCompMod 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForCompMod") = mMachineMaintenanceListForCompMod 'Added by Saylee on 28th-Oct-2009
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mWOListForCombo")
		Session.Remove("AsonDate")
		Session.Remove("AonDate")
		Session.Remove("AircraftId")

		Session.Remove("mSelectDueJobForWO")
		Session.Remove("mSelectDueJobsForWO")
		Session.Remove("mWO")
		Session.Remove("mDueLimits")

		Session.Remove("mLog")

		Session.Remove("MachineName")
		Session.Remove("mAssemblyInfo")
		Session.Remove("mCompInfo")
		Session.Remove("LogId")

		Session.Remove("mMachineMaintenanceForAssemblyService")
		Session.Remove("mMachineMaintenanceListForAssemblyService")

		Session.Remove("mMachineMaintenanceForAssemblyInsp")
		Session.Remove("mMachineMaintenanceListForAssemblyInsp")

		Session.Remove("mMachineMaintenanceForAssemblyMod")
		Session.Remove("mMachineMaintenanceListForAssemblyMod")

		Session.Remove("mMachineMaintenanceForCompService")
		Session.Remove("mMachineMaintenanceListForCompService")

		Session.Remove("mMachineMaintenanceForCompInsp")
		Session.Remove("mMachineMaintenanceListForCompInsp")

		Session.Remove("mMachineMaintenanceForCompMod")
		Session.Remove("mMachineMaintenanceListForCompMod")

		Session.Remove("WOId")

		Session.Remove("OpenFindNowSelectLogForm")

	End Sub
	Private Sub AddJobs()
		Dim item As DataGridItem
		Dim chkBox As CheckBox
		Dim txtComplyRemark As TextBox
		Dim Recordno, PageItems As Integer
		Dim i As Integer
		PageItems = dgDueJob.Items.Count - 1
		' Set Selected Notes value  
		For i = 0 To PageItems
			Recordno = i + dgDueJob.PageSize * dgDueJob.CurrentPageIndex
			item = dgDueJob.Items(i)
			chkBox = CType(item.FindControl("chkSelect"), CheckBox)
			txtComplyRemark = CType(item.FindControl("txtAssemblyRemark"), TextBox)
			mSelectDueJobsForWO(Recordno).IsSelected = chkBox.Checked
			mSelectDueJobsForWO(Recordno).DoneRemark = txtComplyRemark.Text
		Next
		Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
	End Sub
	Private Sub SetValues()
		If (cmbWOList.SelectedItem.Text = "(SELECT)") Then
			MachineName = "{00000000-0000-0000-0000-000000000000}"

		Else
			mWO = Session("mWO")
			MachineName = mWO.MachineID.ToString
			WOName = mWO.ID.ToString

			If CType(Session("LogId"), String) <> "" Or Session("LogId") IsNot Nothing Then
				'' SetLog()
				'do nothing
			Else
				Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, MachineName.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
				Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
				tmpAssemblyStatusList = Nothing
			End If

			dgDoneOnValue.DataSource = AssemblyStatusPeriodList
			dgDoneOnValue.DataBind()

		End If

		If Not (txtAsOnDate.IsDateValue) Then
			AsonDate = ""
			AOnDate = ""
		Else
			AsonDate = txtAsOnDate.Value.ToString
			AOnDate = txtAsOnDate.Value.ToString
		End If

		Session("AsonDate") = AsonDate
		Session("AonDate") = AOnDate
		Session("AircraftId") = MachineName
		Session("WOId") = WOName
	End Sub
	Private Sub ControlVisibility()
		If mSelectDueJobsForWO IsNot Nothing Then
			If mSelectDueJobsForWO.Count > 0 Then
				btnSave.Enabled = True
				If mSelectDueJobsForWO.Count > 10 Then
					btnSaveTop.Visible = True
					btnCloseTop.Visible = True
				Else
					btnSaveTop.Visible = False
					btnCloseTop.Visible = False
				End If
				lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
			Else
				btnSaveTop.Visible = False
				btnCloseTop.Visible = False
			End If
		Else
			btnSave.Enabled = False
			btnSaveTop.Visible = False
			btnCloseTop.Visible = False
		End If
	End Sub
	Private overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub ResetValues()
		MachineName = "{00000000-0000-0000-0000-000000000000}"
		If AsonDate <> "" Then
			txtAsOnDate.Value = AsonDate
		End If
		AsonDate = ""
		AssemblyName = Guid.Empty.ToString
		mSelectDueJobsForWO = Nothing
	End Sub
#End Region

#Region " Data Binding "
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "cmbWOList" Then
			If cmbWOList.SelectedIndex <= 0 Then
				custValidator.ErrorMessage = "Work Order Required"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub
	Private Sub DataFieldBind()
		'mWOListForCombo = FlyPal22.Maintain.WOListForCombo.GetWOListForCombo("(SELECT)")
		mWOListForCombo = nWOListForCombo.GetnWOListForCombo("(SELECT)")
		cmbWOList.DataSource = mWOListForCombo
		cmbWOList.DataBind()

		Dim mMachineMaintenanceList As MachineMaintenanceList
		mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
		Session("mMachineMaintenanceListForAssemblyService") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForAssemblyInsp") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForAssemblyMod") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForCompService") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForCompInsp") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForCompMod") = mMachineMaintenanceList


		IIf(AsonDate <> "", txtAsOnDate.Value = CDate(AsonDate), txtAsOnDate.Value = Today.Date)
		If WOName <> "" Then
			cmbWOList.SelectedValue = WOName
		Else
			cmbWOList.SelectedIndex = 0
		End If

		If mSelectDueJobsForWO IsNot Nothing Then
			dgDueJob.DataSource = mSelectDueJobsForWO
			dgDueJob.DataBind()
		End If

		If AssemblyStatusPeriodList IsNot Nothing Then
			dgDoneOnValue.DataSource = AssemblyStatusPeriodList
			dgDoneOnValue.DataBind()
		End If

		If CType(Session("OpenFindNowSelectLogForm"), Boolean) = True Then
			dgDoneOnValue.DataSource = AssemblyStatusPeriodList
			dgDoneOnValue.DataBind()
			txtAsOnDate.Value = AsonDate
		End If
	End Sub
#End Region

#Region " Machine Maintenance "
	Private Sub SaveMachineMaintenance(ByVal mMachineMaintenance As MachineMaintenance)
		'Added by Saylee on 9th-Oct-2009
		If mMachineMaintenance.IsValid = True Then
			Try
				mMachineMaintenance.ApplyEdit()
				mMachineMaintenance.Save()
				Session("mMachineMaintenance") = mMachineMaintenance
			Catch ex As Exception

			End Try
		End If
		''  End If
	End Sub
#End Region

#Region " Save Status "
#Region "Assembly Service Status"
	Private Sub SaveAssemblyMonitorServiceStatusBoardInfo(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus)
		Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod
		Dim DueOnValue As String

		If (mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mAssemblyMonitorServiceStatus.DoneOn IsNot DBNull.Value) Or (mAssemblyMonitorServiceStatus.IsApplicable = False) Then
			DueOnValue = ""
		Else
			For Each mAssemblyMonitorServiceStatusPeriod In mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
				If mAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
				Else
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorServiceStatusPeriod.DueOnValueTextFormatted
				End If
			Next
		End If

		mBoardInfo = Session("mBoardInfo")
		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorServiceStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
			Session("mBoardInfo") = mBoardInfo
		End If
		Session("mAircraftInformationBoardList") = Nothing
	End Sub
	Public Function SaveAssemblyMonitorServiceStatus(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
		clnAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)

		SetAssemblyMonitorServiceStatusObject(mAssemblyMonitorServiceStatus, mSelectDueJobForWO)

		If mAssemblyMonitorServiceStatus.IsValid Then
			If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then
				Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Service Status.Assembly Service Status can not be saved without period units.", MsgBoxStyle.OkOnly)
				msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
				msg1.Show()
			End If
			Try
				mAssemblyMonitorServiceStatus.ApplyEdit()
				mAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)
				SaveAssemblyMonitorServiceStatusBoardInfo(mAssemblyMonitorServiceStatus)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyService)
				Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
				mAssemblyInfo = Session("mAssemblyInfo")
				'MarkLog(Util.Action.Save, "AssemblyServiceMonitor", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
				mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Assembly Service Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mAssemblyMonitorServiceStatus") = clnAssemblyMonitorServiceStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 8145 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 2627 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 547 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				End If
			Finally
				clnAssemblyMonitorServiceStatus = Nothing
			End Try
		End If
	End Function
	Private Sub SetAssemblyMonitorServiceStatusObject(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
		mAssemblyMonitorServiceStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mAssemblyMonitorServiceStatus.DoneWONo = mWO.WONumber

		'Added by Saylee on 28th-Oct-2009
		If Not (mMachineMaintenanceListForAssemblyService.Contains(mAssemblyMonitorServiceStatus.ID, 5, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForAssemblyService = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 5, txtAsOnDate.Value.ToString, mAssemblyMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorServiceStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyService = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatus.ID, 5)
		End If

		With mMachineMaintenanceForAssemblyService
			''.MachineID = mAssemblyStatus.MachineID
			''.MaintenanceActivityTypeID =5
			.MaintenanceID = mAssemblyMonitorServiceStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Value
			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Value.ToString, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForAssemblyService") = mMachineMaintenanceForAssemblyService
	End Sub
#End Region

#Region "Assembly Inspection Status"
	Private Sub SaveAssemblyMonitorInspStatusBoardInfo(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus)
		Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod
		Dim DueOnValue As String

		If (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mAssemblyMonitorInspStatus.DoneOn IsNot DBNull.Value) Or (mAssemblyMonitorInspStatus.IsApplicable = False) Then
			DueOnValue = ""
		Else
			For Each mAssemblyMonitorInspStatusPeriod In mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
				If mAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
				Else
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueTextFormatted
				End If
			Next
		End If

		mBoardInfo = Session("mBoardInfo")
		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorInspStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
			Session("mBoardInfo") = mBoardInfo
		End If
		Session("mAircraftInformationBoardList") = Nothing
	End Sub
	Public Function SaveAssemblyMonitorInspStatus(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
		clnAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)

		SetAssemblyMonitorInspStatusObject(mAssemblyMonitorInspStatus, mSelectDueJobForWO)

		If mAssemblyMonitorInspStatus.IsValid Then
			If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
				Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Insp Status.Assembly Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly)
				msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
				msg1.Show()
			End If
			Try
				mAssemblyMonitorInspStatus.ApplyEdit()
				mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
				SaveAssemblyMonitorInspStatusBoardInfo(mAssemblyMonitorInspStatus)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyInsp)
				Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
				mAssemblyInfo = Session("mAssemblyInfo")
				'MarkLog(Util.Action.Save, "AssemblyInspMonitor", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID)
				mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Assembly Inspection Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mAssemblyMonitorInspStatus") = clnAssemblyMonitorInspStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 8145 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 2627 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 547 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				End If
			Finally
				clnAssemblyMonitorInspStatus = Nothing
			End Try
		End If
	End Function
	Private Sub SetAssemblyMonitorInspStatusObject(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
		mAssemblyMonitorInspStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mAssemblyMonitorInspStatus.DoneWONo = mWO.WONumber

		If Not (mMachineMaintenanceListForAssemblyInsp.Contains(mAssemblyMonitorInspStatus.ID, 6, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForAssemblyInsp = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 6, txtAsOnDate.Value.ToString, mAssemblyMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorInspStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyInsp = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatus.ID, 6)
		End If

		With mMachineMaintenanceForAssemblyInsp
			''.MachineID = mAssemblyStatus.MachineID
			''.MaintenanceActivityTypeID =5
			.MaintenanceID = mAssemblyMonitorInspStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Value
			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Value.ToString, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForAssemblyInsp") = mMachineMaintenanceForAssemblyInsp
	End Sub
#End Region

#Region "Assembly Modification Status"
	Private Sub SaveAssemblyMonitorModStatusBoardInfo(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus)
		Dim mAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriod
		Dim DueOnValue As String

		If (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mAssemblyMonitorModStatus.DoneOn IsNot DBNull.Value) Or (mAssemblyMonitorModStatus.IsApplicable = False) Then
			DueOnValue = ""
		Else
			For Each mAssemblyMonitorModStatusPeriod In mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
				If mAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueFormatted
				Else
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueTextFormatted
				End If
			Next
		End If

		mBoardInfo = Session("mBoardInfo")
		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorModStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
			Session("mBoardInfo") = mBoardInfo
		End If
		Session("mAircraftInformationBoardList") = Nothing
	End Sub
	Public Function SaveAssemblyMonitorModStatus(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus
		clnAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)

		SetAssemblyMonitorModStatusObject(mAssemblyMonitorModStatus, mSelectDueJobForWO)

		If mAssemblyMonitorModStatus.IsValid Then
			If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
				Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Assembly Mod Status.Assembly Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly)
				msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
				msg1.Show()
			End If
			Try
				mAssemblyMonitorModStatus.ApplyEdit()
				mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
				SaveAssemblyMonitorModStatusBoardInfo(mAssemblyMonitorModStatus)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyMod)
				Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
				mAssemblyInfo = Session("mAssemblyInfo")
				'MarkLog(Util.Action.Save, "AssemblyModMonitor", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID)
				mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Assembly Modification Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mAssemblyMonitorModStatus") = clnAssemblyMonitorModStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 8145 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 2627 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 547 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				End If
			Finally
				clnAssemblyMonitorModStatus = Nothing
			End Try
		End If
	End Function
	Private Sub SetAssemblyMonitorModStatusObject(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
		mAssemblyMonitorModStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mAssemblyMonitorModStatus.DoneWONo = mWO.WONumber

		If Not (mMachineMaintenanceListForAssemblyMod.Contains(mAssemblyMonitorModStatus.ID, 7, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForAssemblyMod = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 7, txtAsOnDate.Value.ToString, mAssemblyMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorModStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyMod = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatus.ID, 7)
		End If

		With mMachineMaintenanceForAssemblyMod
			''.MachineID = mAssemblyStatus.MachineID
			''.MaintenanceActivityTypeID =5
			.MaintenanceID = mAssemblyMonitorModStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Value
			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Value.ToString, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForAssemblyMod") = mMachineMaintenanceForAssemblyMod
	End Sub
#End Region

#Region "Component Service Status"
	Private Sub SetCompMonitorServiceStatusObject(ByVal mCompMonitorServiceStatus As CompMonitorServiceStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
		mCompMonitorServiceStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mCompMonitorServiceStatus.DoneWONo = mWO.WONumber

		'Added by Saylee on 28th-Oct-2009
		If Not (mMachineMaintenanceListForCompService.Contains(mCompMonitorServiceStatus.ID, 8, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForCompService = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 8, txtAsOnDate.Value.ToString, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, mCompMonitorServiceStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForCompService = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatus.ID, 8)
		End If

		With mMachineMaintenanceForCompService
			''.MachineID = mCompStatus.MachineID
			''.MaintenanceActivityTypeID =8
			.MaintenanceID = mCompMonitorServiceStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Value

			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Value.ToString, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForCompService") = mMachineMaintenanceForCompService
	End Sub
	Public Function SaveCompMonitorServiceStatus(ByVal mCompMonitorServiceStatus As CompMonitorServiceStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus
		clnCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)

		SetCompMonitorServiceStatusObject(mCompMonitorServiceStatus, mSelectDueJobForWO)
		If mCompMonitorServiceStatus.IsValid Then
			If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
				Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Component Service Status.Component Service Status can not be saved without period units.", MsgBoxStyle.OkOnly)
				msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
				msg1.Show()
			End If
			Try
				mCompMonitorServiceStatus.ApplyEdit()
				mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
				Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
				SaveMachineMaintenance(mMachineMaintenanceForCompService)
				mCompInfo = Session("mCompInfo")
				'MarkLog(Util.Action.Save, "CompServiceMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
				mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Component Service Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)

				Return True
			Catch ex As SqlException
				Session("mCompMonitorServiceStatus") = clnCompMonitorServiceStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 8145 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 2627 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 547 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				End If
			Finally
				clnCompMonitorServiceStatus = Nothing
			End Try
		End If
	End Function
#End Region

#Region "Component Insp Status"
	Private Sub SetCompMonitorInspStatusObject(ByVal mCompMonitorInspStatus As CompMonitorInspStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
		mCompMonitorInspStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mCompMonitorInspStatus.DoneWONo = mWO.WONumber

		'Added by Saylee on 28th-Oct-2009
		If Not (mMachineMaintenanceListForCompInsp.Contains(mCompMonitorInspStatus.ID, 9, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForCompInsp = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 9, txtAsOnDate.Value.ToString, mCompMonitorInspStatus.ID, Guid.Empty, 0, 0, mCompMonitorInspStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForCompInsp = MachineMaintenance.GetMachineMaintenance(mCompMonitorInspStatus.ID, 9)
		End If

		With mMachineMaintenanceForCompInsp
			''.MachineID = mCompStatus.MachineID
			''.MaintenanceActivityTypeID =8
			.MaintenanceID = mCompMonitorInspStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Value

			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Value.ToString, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForCompInsp") = mMachineMaintenanceForCompInsp
	End Sub
	Public Function SaveCompMonitorInspStatus(ByVal mCompMonitorInspStatus As CompMonitorInspStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnCompMonitorInspStatus As CompMonitorInspStatus
		clnCompMonitorInspStatus = CType(mCompMonitorInspStatus.Clone, CompMonitorInspStatus)

		SetCompMonitorInspStatusObject(mCompMonitorInspStatus, mSelectDueJobForWO)
		If mCompMonitorInspStatus.IsValid Then
			If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count = 0 Then
				Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Component Insp Status.Component Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly)
				msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
				msg1.Show()
			End If
			Try
				mCompMonitorInspStatus.ApplyEdit()
				mCompMonitorInspStatus = CType(mCompMonitorInspStatus.Save(), CompMonitorInspStatus)
				Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
				SaveMachineMaintenance(mMachineMaintenanceForCompInsp)
				mCompInfo = Session("mCompInfo")
				'MarkLog(Util.Action.Save, "CompInspMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorInspStatus.ID)
				mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Component Inspection Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mCompMonitorInspStatus") = clnCompMonitorInspStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 8145 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 2627 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 547 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				End If
			Finally
				clnCompMonitorInspStatus = Nothing
			End Try
		End If
	End Function
#End Region

#Region "Component Mod Status"
	Private Sub SetCompMonitorModStatusObject(ByVal mCompMonitorModStatus As CompMonitorModStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO)
		mCompMonitorModStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mCompMonitorModStatus.DoneWONo = mWO.WONumber

		'Added by Saylee on 28th-Oct-2009
		If Not (mMachineMaintenanceListForCompMod.Contains(mCompMonitorModStatus.ID, 10, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForCompMod = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 10, txtAsOnDate.Value.ToString, mCompMonitorModStatus.ID, Guid.Empty, 0, 0, mCompMonitorModStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForCompMod = MachineMaintenance.GetMachineMaintenance(mCompMonitorModStatus.ID, 10)
		End If

		With mMachineMaintenanceForCompMod
			''.MachineID = mCompStatus.MachineID
			''.MaintenanceActivityTypeID =8
			.MaintenanceID = mCompMonitorModStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Value

			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Value.ToString, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForCompMod") = mMachineMaintenanceForCompMod
	End Sub
	Public Function SaveCompMonitorModStatus(ByVal mCompMonitorModStatus As CompMonitorModStatus, ByVal mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnCompMonitorModStatus As CompMonitorModStatus
		clnCompMonitorModStatus = CType(mCompMonitorModStatus.Clone, CompMonitorModStatus)

		SetCompMonitorModStatusObject(mCompMonitorModStatus, mSelectDueJobForWO)
		If mCompMonitorModStatus.IsValid Then
			If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count = 0 Then
				Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.PeriodRequired, SIMsgBox.Message_text.PeriodRequired, "You are trying to save Component Mod Status.Component Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly)
				msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
				msg1.Show()
			End If
			Try
				mCompMonitorModStatus.ApplyEdit()
				mCompMonitorModStatus = CType(mCompMonitorModStatus.Save(), CompMonitorModStatus)
				Session("mCompMonitorModStatus") = mCompMonitorModStatus
				SaveMachineMaintenance(mMachineMaintenanceForCompMod)
				mCompInfo = Session("mCompInfo")
				'MarkLog(Util.Action.Save, "CompModMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorModStatus.ID)
				mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Component Modification Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mCompMonitorModStatus") = clnCompMonitorModStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 8145 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 2627 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				ElseIf ex.Number = 547 Then
					Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
				End If
			Finally
				clnCompMonitorModStatus = Nothing
			End Try
		End If
	End Function
#End Region
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		txtAsOnDate.ShowClearButton = False
		If Not IsPostBack Then

			If (CType(Session("OpenFindNowSelectLogForm"), Boolean) = False) Then
				ResetValues()
				txtAsOnDate.Value = Today.Date
				AOnDate = Today.Date
			End If

			txtAsOnDate.Value = Today.Date
			AsonDate = Today.Date.ToString
			DataFieldBind()
		End If
		SetFocus(cmbWOList)
		ControlVisibility()
	End Sub

	Private Sub cmbWOList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWOList.SelectedIndexChanged
		mDueLimits = DueLimits.GetDueLimits(New Guid("{00000000-0000-0000-0000-000000000000}"))
		If cmbWOList.SelectedIndex > 0 Then
			'mWO = FlyPal22.Maintain.WO.GetWO(New Guid(cmbWOList.SelectedValue))
			mWO = nWO.GetWO(New Guid(cmbWOList.SelectedValue))
			'mSelectDueJobsForWO = SelectDueJobsForWO.GetSelectDueJobsForWO(txtAsOnDate.Value.ToString, mDueLimits, mWO.MachineID.ToString, 0, mWO, chkShowAll.Checked)
			mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Value.ToString, mDueLimits, mWO.MachineID.ToString, 0, mWO)

			If mSelectDueJobsForWO.Count = 0 Then
				Dim msg1 As New SIMsgBox(Page, "Monitoring Services / Inspections / Directives not available", "<BR><BR> All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OKOnly)
				msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
				msg1.Show()
				dgDueJob.DataSource = mSelectDueJobsForWO
				Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
				Session("mWO") = mWO
				dgDueJob.DataBind()
				Exit Sub
			End If
			dgDueJob.DataSource = mSelectDueJobsForWO
			Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
			Session("mWO") = mWO
			dgDueJob.DataBind()

			Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
			AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

			dgDoneOnValue.DataSource = AssemblyStatusPeriodList
			dgDoneOnValue.DataBind()
			If mSelectDueJobsForWO.Count > 0 Then
				btnSave.Enabled = True
				If mSelectDueJobsForWO.Count > 10 Then btnSaveTop.Visible = True
				If mSelectDueJobsForWO.Count > 10 Then btnCloseTop.Visible = True

			Else
				btnSave.Enabled = False
			End If
			lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
		Else
			mSelectDueJobsForWO = Nothing
			dgDueJob.DataBind()
			btnSave.Enabled = False
		End If
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click

		If IsValid = True Then
			AddJobs()
			SetSession()
			Session("OpenFindNowSelectLogForm") = True
			mWO = Session("mWO")
			SetValues()
			'' Dim mtmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , , ).Item(cmbAssembly.SelectedIndex), MachineInfo).AssemblyStatusList
			'' If cmbAssembly.SelectedIndex = 0 Then
			Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
			''Dim str As String
			''str = "<script language='javascript'>openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=wfSelectWOForMulticompliance.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & tmpAssemblyStatusList(0).ID.ToString & "&AssemblyID=" & tmpAssemblyStatusList(0).AssemblyID.ToString & "'); </script>"
			'' ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", str)

			Response.Redirect("wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=wfSelectWOForMulticompliance.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & tmpAssemblyStatusList(0).ID.ToString & "&AssemblyID=" & tmpAssemblyStatusList(0).AssemblyID.ToString)
		End If
	End Sub
	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveTop.Click
		SetValues()
		AddJobs()
		mSelectDueJobsForWO = Session("mSelectDueJobsForWO")

		Dim index As Integer
		For index = 0 To mSelectDueJobsForWO.Count - 1
			If mSelectDueJobsForWO.Item(index).IsSelected = True Then
				Dim mMachine As Machine = Machine.GetMachine(mSelectDueJobsForWO.Item(index).MachineID)
				If mSelectDueJobsForWO(index).OnAssemblyOrComponent = "Assembly" Then
					Select Case mSelectDueJobsForWO(index).DataType
						Case "Servicing" 'Service

							Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
							Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mMachine.HourType)
							If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							Else
								If CType(Session("FromLog"), Boolean) = True Then
									mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, New Guid(LogId), mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
								Else
									mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
								End If

								Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
								Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
								Session("From") = 0 'New record
								''
								mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
								Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus

								Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(index).AssemblyStatusID)
								Session("mMachine") = mMachine
								Session("mAssemblyStatus") = mAssemblyStatus


								mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
								Session("mBoardInfo") = mBoardInfo

								Session("mAssemblyInfo") = ""
								''Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).MachineInfo+ "->" + mSelectDueJobsForWO.Item(index).ModelSerialNo + "->" + mSelectDueJobsForWO.Item(index).Reference + "->" + mSelectDueJobsForWO.Item(index).MonitorInfo + "->" + mSelectDueJobsForWO.Item(index).MonitorType + "->" + mSelectDueJobsForWO.Item(index).ATA + "->" + mSelectDueJobsForWO.Item(index).Description
								Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).LogBook

								If SaveAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatus, mSelectDueJobsForWO.Item(index)) = True Then
									If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
										Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
										mWOJob.IsComplied = True
										mWOJob.Save()
									End If
								End If
								Dim mTmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
								mTmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(AsonDate, MachineName, "", "")
								Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
								Session("MaintenanceActivityTypeID") = 5
							End If
						Case "Inspection" 'Inspection

							Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
							Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mMachine.HourType)
							If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							Else
								If CType(Session("FromLog"), Boolean) = True Then
									mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, New Guid(LogId), mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
								Else
									mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
								End If

								Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
								Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
								Session("From") = 0 'New record
								''
								mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
								Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

								Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(index).AssemblyStatusID)
								Session("mMachine") = mMachine
								Session("mAssemblyStatus") = mAssemblyStatus


								mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
								Session("mBoardInfo") = mBoardInfo

								Session("mAssemblyInfo") = ""
								''Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).MachineInfo+ "->" + mSelectDueJobsForWO.Item(index).ModelSerialNo + "->" + mSelectDueJobsForWO.Item(index).Reference + "->" + mSelectDueJobsForWO.Item(index).MonitorInfo + "->" + mSelectDueJobsForWO.Item(index).MonitorType + "->" + mSelectDueJobsForWO.Item(index).ATA + "->" + mSelectDueJobsForWO.Item(index).Description
								Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).LogBook

								If SaveAssemblyMonitorInspStatus(mAssemblyMonitorInspStatus, mSelectDueJobsForWO.Item(index)) = True Then
									If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
										Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
										mWOJob.IsComplied = True
										mWOJob.Save()
									End If
								End If
								Dim mTmpComplyAssemblyMonitorInspStatusList As tmpComplyAssemblyMonitorInspStatusList
								mTmpComplyAssemblyMonitorInspStatusList = tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(AsonDate, MachineName, "", "")
								Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
								Session("MaintenanceActivityTypeID") = 6
							End If
						Case "Modification" 'Modification

							Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
							Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mMachine.HourType)
							If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							Else
								If CType(Session("FromLog"), Boolean) = True Then
									mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, New Guid(LogId), mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
								Else
									mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
								End If

								Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
								Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
								Session("From") = 0 'New record
								''
								mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
								Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

								Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(index).AssemblyStatusID)
								Session("mMachine") = mMachine
								Session("mAssemblyStatus") = mAssemblyStatus


								mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
								Session("mBoardInfo") = mBoardInfo

								Session("mAssemblyInfo") = ""
								''Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).MachineInfo+ "->" + mSelectDueJobsForWO.Item(index).ModelSerialNo + "->" + mSelectDueJobsForWO.Item(index).Reference + "->" + mSelectDueJobsForWO.Item(index).MonitorInfo + "->" + mSelectDueJobsForWO.Item(index).MonitorType + "->" + mSelectDueJobsForWO.Item(index).ATA + "->" + mSelectDueJobsForWO.Item(index).Description
								Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).LogBook


								If SaveAssemblyMonitorModStatus(mAssemblyMonitorModStatus, mSelectDueJobsForWO.Item(index)) = True Then
									If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
										Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
										mWOJob.IsComplied = True
										mWOJob.Save()
									End If
								End If
								Dim mTmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList
								mTmpComplyAssemblyMonitorModStatusList = tmpComplyAssemblyMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, "", "")
								Session("mTmpComplyAssemblyMonitorModStatusList") = mTmpComplyAssemblyMonitorModStatusList
								Session("MaintenanceActivityTypeID") = 7
							End If
					End Select
				ElseIf mSelectDueJobsForWO(index).OnAssemblyOrComponent = "Component" Then
					Select Case mSelectDueJobsForWO(index).DataType
						Case "Servicing"

							Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
							Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mSelectDueJobsForWO.Item(index).CompStatusID, mMachine.HourType)
							If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							Else
								If CType(Session("FromLog"), Boolean) = True Then
									mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, New Guid(LogId), mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
								Else
									mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
								End If

								Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
								Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
								Session("From") = 0 'NewRecord

								Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(index).AssemblyStatusID)
								Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, AsonDate)
								Session("mMachine") = mMachine
								Session("mCompStatus") = mCompStatus
								Session("mAssemblyStatus") = mAssemblyStatus
								mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
								Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

								Session("mCompInfo") = ""
								'Session("mCompInfo") = mSelectDueJobsForWO.Item(index).MachineInfo + "->" + mSelectDueJobsForWO.Item(index).CompSerialNo + "->" + mSelectDueJobsForWO.Item(index).Reference + "->" + mSelectDueJobsForWO.Item(index).MonitorInfo + "->" + mSelectDueJobsForWO.Item(index).CompInfo + "->" + mSelectDueJobsForWO.Item(index).MonitorType + "->" + mSelectDueJobsForWO.Item(index).ATA + "->" + mSelectDueJobsForWO.Item(index).Description
								'Session("mCompInfo") = mSelectDueJobsForWO.Item(index).ComponentInfo
								Session("mCompInfo") = mSelectDueJobsForWO.Item(index).LogBook

								If SaveCompMonitorServiceStatus(mCompMonitorServiceStatus, mSelectDueJobsForWO.Item(index)) = True Then
									If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
										Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
										mWOJob.IsComplied = True
										mWOJob.Save()
									End If
								End If
								Dim mTmpComplyCompMonitorServiceStatusList As tmpComplyCompMonitorServiceStatusList
								mTmpComplyCompMonitorServiceStatusList = tmpComplyCompMonitorServiceStatusList.GetDueMonitorServiceList(AsonDate, MachineName, "", "", mSelectDueJobsForWO.Item(index).AssemblyID)
								Session("mTmpComplyCompMonitorServiceStatusList") = mTmpComplyCompMonitorServiceStatusList
								Session("MaintenanceActivityTypeID") = 8
							End If
						Case "Inspection" 'Inspection

							Dim mCompMonitorInspStatus As CompMonitorInspStatus
							Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mSelectDueJobsForWO.Item(index).CompStatusID, mMachine.HourType, IsForSpareComp:=mSelectDueJobsForWO.Item(index).IsSpareComponent)
							If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							ElseIf mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4 And mPrevCompMonitorInspStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							Else
								If CType(Session("FromLog"), Boolean) = True Then
									mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, New Guid(LogId), mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
								Else
									mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
								End If

								Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
								Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
								Session("From") = 0 'NewRecord

								If mSelectDueJobsForWO.Item(index).IsSpareComponent = False Then


									Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(index).AssemblyStatusID)
									Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, AsonDate)
									Session("mMachine") = mMachine
									Session("mCompStatus") = mCompStatus
									Session("mAssemblyStatus") = mAssemblyStatus
								End If
								mCompMonitorInspStatus.RequiredManHours = mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours
								Session("mCompMonitorInspStatus") = mCompMonitorInspStatus

								Session("mCompInfo") = ""
								'Session("mCompInfo") = mSelectDueJobsForWO.Item(index).MachineInfo + "->" + mSelectDueJobsForWO.Item(index).CompSerialNo + "->" + mSelectDueJobsForWO.Item(index).Reference + "->" + mSelectDueJobsForWO.Item(index).MonitorInfo + "->" + mSelectDueJobsForWO.Item(index).CompInfo + "->" + mSelectDueJobsForWO.Item(index).MonitorType + "->" + mSelectDueJobsForWO.Item(index).ATA + "->" + mSelectDueJobsForWO.Item(index).Description
								'Session("mCompInfo") = mSelectDueJobsForWO.Item(index).ComponentInfo
								Session("mCompInfo") = mSelectDueJobsForWO.Item(index).LogBook

								If SaveCompMonitorInspStatus(mCompMonitorInspStatus, mSelectDueJobsForWO.Item(index)) = True Then
									If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
										Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
										mWOJob.IsComplied = True
										mWOJob.Save()
									End If
								End If
								Dim mTmpComplyCompMonitorInspStatusList As tmpComplyCompMonitorInspStatusList
								mTmpComplyCompMonitorInspStatusList = tmpComplyCompMonitorInspStatusList.GetDueMonitorInspList(AsonDate, MachineName, "", "", mSelectDueJobsForWO.Item(index).AssemblyID)
								Session("mTmpComplyCompMonitorInspStatusList") = mTmpComplyCompMonitorInspStatusList
								Session("MaintenanceActivityTypeID") = 8
							End If
						Case "Modification" 'Modification

							Dim mCompMonitorModStatus As CompMonitorModStatus
							Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mSelectDueJobsForWO.Item(index).CompStatusID, mMachine.HourType, IsForSpareComp:=mSelectDueJobsForWO.Item(index).IsSpareComponent)
							If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.OneTimeMonitoring, SIMsgBox.Message_text.OneTimeMonitoring, "You are trying to comply the record.One time monitoring already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							ElseIf mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 4 And mPrevCompMonitorModStatus.IsCompleted Then
								Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Expiry, SIMsgBox.Message_text.Expiry, "You are trying to comply the record.Expiery compliance already done. Can not be complied again.", MsgBoxStyle.OKOnly)
								msg.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
								msg.Show()
								Exit Sub
							Else
								If CType(Session("FromLog"), Boolean) = True Then
									mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, New Guid(LogId), mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
								Else
									mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
								End If

								Session("mCompMonitorModStatus") = mCompMonitorModStatus
								Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
								Session("From") = 0 'NewRecord

								If mSelectDueJobsForWO.Item(index).IsSpareComponent = False Then


									Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(index).AssemblyStatusID)
									Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, AsonDate)
									Session("mMachine") = mMachine
									Session("mCompStatus") = mCompStatus
									Session("mAssemblyStatus") = mAssemblyStatus
								End If
								mCompMonitorModStatus.RequiredManHours = mCompMonitorModStatus.PartMonitorMod.RequiredManHours
								Session("mCompMonitorModStatus") = mCompMonitorModStatus

								Session("mCompInfo") = ""
								'Session("mCompInfo") = mSelectDueJobsForWO.Item(index).MachineInfo + "->" + mSelectDueJobsForWO.Item(index).CompSerialNo + "->" + mSelectDueJobsForWO.Item(index).Reference + "->" + mSelectDueJobsForWO.Item(index).MonitorInfo + "->" + mSelectDueJobsForWO.Item(index).CompInfo + "->" + mSelectDueJobsForWO.Item(index).MonitorType + "->" + mSelectDueJobsForWO.Item(index).ATA + "->" + mSelectDueJobsForWO.Item(index).Description
								'Session("mCompInfo") = mSelectDueJobsForWO.Item(index).ComponentInfo
								Session("mCompInfo") = mSelectDueJobsForWO.Item(index).LogBook

								If SaveCompMonitorModStatus(mCompMonitorModStatus, mSelectDueJobsForWO.Item(index)) = True Then
									If mWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
										Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
										mWOJob.IsComplied = True
										mWOJob.Save()
									End If
								End If
								Dim mTmpComplyCompMonitorModStatusList As tmpComplyCompMonitorModStatusList
								mTmpComplyCompMonitorModStatusList = tmpComplyCompMonitorModStatusList.GetDueMonitorModList(AsonDate, MachineName, "", "", mSelectDueJobsForWO.Item(index).AssemblyID)
								Session("mTmpComplyCompMonitorModStatusList") = mTmpComplyCompMonitorModStatusList
								Session("MaintenanceActivityTypeID") = 8
							End If
					End Select
				End If
			End If
		Next

		If cmbWOList.SelectedIndex > 0 Then
			'mWO = FlyPal22.Maintain.WO.GetWO(New Guid(cmbWOList.SelectedValue))
			mWO = nWO.GetWO(New Guid(cmbWOList.SelectedValue))
			'mSelectDueJobsForWO = SelectDueJobsForWO.GetSelectDueJobsForWO(txtAsOnDate.Value.ToString, mDueLimits, mWO.MachineID.ToString, 0, mWO, chkShowAll.Checked)
			mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Value.ToString, mDueLimits, mWO.MachineID.ToString, 0, mWO)

			dgDueJob.DataSource = mSelectDueJobsForWO
			Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
			Session("mWO") = mWO
			dgDueJob.DataBind()

			Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
			AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

			dgDoneOnValue.DataSource = AssemblyStatusPeriodList
			dgDoneOnValue.DataBind()
		End If
	End Sub
	Private Sub btnMaintenanceActivity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMaintenanceActivity.Click
		RemoveSession()
		Response.Redirect("index.aspx")
	End Sub
	Private Sub txtAsOnDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAsOnDate.CalendarVisibleChanged
		Me.cmbWOList.Visible = Not CType(sender, Boolean)
	End Sub
	''Private Sub txtAsOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAsOnDate.TextChanged
	''    If cmbWOList.SelectedIndex > 0 Then
	''        mWO = FlyPal22.Maintain.WO.GetWO(New Guid(cmbWOList.SelectedValue))
	''        mSelectDueJobsForWO = SelectDueJobsForWO.GetSelectDueJobsForWO(txtAsOnDate.Value.ToString, mDueLimits, mWO.MachineID.ToString, 0, mWO, chkShowAll.Checked)

	''        dgDueJob.DataSource = mSelectDueJobsForWO
	''        Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
	''        Session("mWO") = mWO
	''        dgDueJob.DataBind()

	''        Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
	''        AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
	''        Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

	''        dgDoneOnValue.DataSource = AssemblyStatusPeriodList
	''        dgDoneOnValue.DataBind()
	''    End If
	''End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		If IsValid Then
			mDueLimits = DueLimits.GetDueLimits(New Guid("{00000000-0000-0000-0000-000000000000}"))
			If cmbWOList.SelectedIndex > 0 Then
				'mWO = FlyPal22.Maintain.WO.GetWO(New Guid(cmbWOList.SelectedValue))
				mWO = nWO.GetWO(New Guid(cmbWOList.SelectedValue))
				'mSelectDueJobsForWO = SelectDueJobsForWO.GetSelectDueJobsForWO(txtAsOnDate.Value.ToString, mDueLimits, mWO.MachineID.ToString, 0, mWO, chkShowAll.Checked)
				mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Value.ToString, mDueLimits, mWO.MachineID.ToString, 0, mWO)
				If mSelectDueJobsForWO.Count = 0 Then
					Dim msg1 As New SIMsgBox(Page, "Monitoring Services / Inspections / Directives not available", "<BR><BR> All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OKOnly)
					msg1.ReplacePage = "wfSelectWOForMulticompliance.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
					msg1.Show()
					dgDueJob.DataSource = mSelectDueJobsForWO
					Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
					Session("mWO") = mWO
					dgDueJob.DataBind()
					Exit Sub
				End If
				dgDueJob.DataSource = mSelectDueJobsForWO
				Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
				Session("mWO") = mWO
				dgDueJob.DataBind()

				Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, mWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
				Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

				dgDoneOnValue.DataSource = AssemblyStatusPeriodList
				dgDoneOnValue.DataBind()
				If mSelectDueJobsForWO.Count > 0 Then
					btnSave.Enabled = True
					If mSelectDueJobsForWO.Count > 10 Then btnSaveTop.Visible = True
					If mSelectDueJobsForWO.Count > 10 Then btnCloseTop.Visible = True

				Else
					btnSave.Enabled = False
				End If
				lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
			Else
				mSelectDueJobsForWO = Nothing
				dgDueJob.DataBind()
				btnSave.Enabled = False
			End If
		End If

	End Sub
#End Region

End Class
