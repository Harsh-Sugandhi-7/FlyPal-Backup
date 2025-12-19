Imports System.Linq

Imports java.lang
Partial Class wfLogDefectActionList_Ajax
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
	Protected mMELSnagCorrectiveAction As MELSnagCorrectiveAction
	Public mATAList As ATAList
	Public mLog As Log
	Public mMachine As Machine
	Private Flag As Int16
	Private rptLogDefectAction As LogDefectActionList
	Public mDocumentTypeForID As Integer
	Public mAttachToID As Guid
	Public mName, mATA As String
	Public mMELSnagPartList As MELSnagPartList
	Public mReportLogRegister As New ReportLogRegister
	Public mRectifiedReportLogRegister As New ReportLogRegister
	Dim mTempAssemblyList As AssemblyList
	Public mMELSnagCorrectiveActionLog As MELSnagCorrectiveActionLog
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mLogDetail As String
	Dim mSubATAList As SubATAList 'Added By Vikrant On 02-Apr-2013 For ALL01042013
	Dim mAssemblylist As AssemblyList 'Added By Vikrant On 02-Sept-2014 For All04092014\

	Dim mFileAttach As FileAttach
	Dim IsAttachmentDeleted As Boolean = False
	'MLNo
	Dim LicenseNo As String = String.Empty
	Dim EmpName As String = String.Empty
	Dim DoneByID As Guid = Guid.Empty
	Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
	Shared UserNameForLicenceList As String
	'End
	Dim mModuleList As ModuleList
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mATAList = CType(Session("mATAList"), ATAList)
		mLog = CType(Session("mLog"), Log)
		mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
		mMachine = CType(Session("mMachine"), Machine)
		mRectifiedReportLogRegister = CType(Session("mRectifiedReportLogRegister"), ReportLogRegister)
		mTempAssemblyList = CType(Session("mTempAssemblyList"), AssemblyList)
		mMELSnagCorrectiveActionLog = CType(Session("mMELSnagCorrectiveActionLog"), MELSnagCorrectiveActionLog)
		mMELSnagPartList = Session("mMELSnagPartList")
		mSubATAList = Session("mSubATAList") 'Added By Vikrant On 02-Apr-2013 For ALL01042013
		mAssemblylist = Session("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014

		mFileAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		'MLNo
		mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
		UserNameForLicenceList = Session("UserNameForLicenceList")
		'End
		mModuleList = Session("mModuleList")
	End Sub
	Private Sub SetSession()
		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		Session("mATAList") = mATAList
		Session("mLog") = mLog
		Session("mMachine") = mMachine
		Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister
		Session("mTempAssemblyList") = mTempAssemblyList
		Session("mMELSnagCorrectiveActionLog") = mMELSnagCorrectiveActionLog
		Session("mMELSnagPartList") = mMELSnagPartList
		'Added By Vikrant On 02-Apr-2013 For ALL01042013
		Session("mSubATAList") = mSubATAList
		'End
		Session("mFileAttach") = mFileAttach
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mMachine")
		Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
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
	'MLNo
	Public Sub SetLicenceCount()
		If mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 1 Then
			lblLicenceCount.Text = "and " + (mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
		End If
		lblLicenceCount.DataBind()
		'lblAllLicenceNos.DataBind()
	End Sub
	Private Sub BindLicenceNo()
		If mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then
			txtLicenceNo.Text = mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeName + "]"
		Else
			txtLicenceNo.Text = String.Empty
		End If
	End Sub
	'End
	Private Sub NewRecord()
		mLog = Log.NewLog(mMachine, Today.Date)
		Session("mLog") = mLog

		SetTitle()

	End Sub
	Private Function Save() As Boolean
		Dim LogClone As Log
		LogClone = CType(mLog.Clone, Log)
		If mLog.IsValid = True Then
			Try
				mLog = CType(mLog.Save(), Log)
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDefectActionList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, "", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDefectActionList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDefectActionList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDefectActionList.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
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
		Dim msgCount As Integer = 0

		' '' ''If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
		' '' ''    Result1 = -1
		' '' ''Else
		' '' ''    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		' '' ''End If
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Dim LogDefectActionID As Guid = Guid.Empty
						Try
							LogDefectActionID = mLog.MELSnagCorrectiveActions(mLog.MELSnagCorrectiveActions.CurrentIndex).ID
							mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " Defect No. : " + mLog.MELSnagCorrectiveActions(mLog.MELSnagCorrectiveActions.CurrentIndex).DefectNo + " Date of occurrence : " + mLog.MELSnagCorrectiveActions(mLog.MELSnagCorrectiveActions.CurrentIndex).DateOfOccurrenceFormatted + " Defect : " + mLog.MELSnagCorrectiveActions(mLog.MELSnagCorrectiveActions.CurrentIndex).Defect
							mLog.MELSnagCorrectiveActions.RemoveAt(mLog.MELSnagCorrectiveActions.CurrentIndex)

							For i As Integer = 0 To mLog.MELSnagCorrectiveActions.Count - 1
								mLog.MELSnagCorrectiveActions(i).SerialNo = i + 1
							Next

							mLog.Save()

							Session("mLog") = mLog
							Session("Edit") = False
							mMELSnagCorrectiveAction = Nothing

							'NewRecord()
							chkShowMEL.Enabled = True
							DataFieldBind()

							SetGrid()
							If chkClose.Checked Then
								txtRectifiedDate.ReadOnly = False
							Else
								txtRectifiedDate.Text = ""
								txtRectifiedDate.ReadOnly = True
								If mRectifiedReportLogRegister IsNot Nothing Then cmbRectifiedLogNo.SelectedIndex = 0
								cmbRectifiedLogNo.Enabled = False
								txtRectificationSector.Text = ""
								mMELSnagCorrectiveAction.RectifiedLogID = Guid.Empty
							End If
							ImageButton2.Visible = False
							btnDelAttach.Enabled = False
							upnlDetails.Update()
							upnlRectifiedDate.Update()
							upnlRectifiedCombo.Update()
							' '' ''Response.Redirect("wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
						Catch ex As SqlException
							If ex.Number = 8145 Then
								' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, "", MsgBoxStyle.OKOnly)
								' '' ''msg1.ReplacePage = "wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
								' '' ''msg1.Show()

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 2627 Then
								' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
								' '' ''msg1.ReplacePage = "wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
								' '' ''msg1.Show()

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 547 Then
								' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OKOnly)
								' '' ''msg1.ReplacePage = "wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
								' '' ''msg1.Show()

								MarkLog(Util.Action.Delete, "Log Defect Action", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mLog.ID, EventLogID)
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
							End If
							DataFieldBind()
							msgCount = ex.Errors.Count
						Finally
							If msgCount = 0 Then
								MarkLog(Util.Action.Delete, "Log Defect Action", mLogDetail, Util.ErrorType.NoError, LogDefectActionID, EventLogID)
								LogDefectActionID = Nothing
							End If
						End Try
					ElseIf MSGBoxCtrl.Sender = "MELSnagCountATAWise" Then 'Added By Prashant 2-Jan-2014  --ALL02012014-1
						mLog.MELSnagCorrectiveActions.CurrentItem.IsRepetitive = True
						mLog.Save()
						NewMELSnagCorrectiveAction()
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "MELSnagCountATAWise" Then 'Added By Prashant 2-Jan-2014  --ALL02012014-1
						mLog.Save()
						NewMELSnagCorrectiveAction()
					Else
						DataFieldBind()
						SetGrid()
						RectifiedLog() 'Added By Prashant 22-Feb-2013 'All22022013-1
						' '' ''Response.Redirect("wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
					End If
				Case MsgBoxResult.Cancel
					DataFieldBind()
					' '' ''Response.Redirect("wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
				Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
					GetSession()
					'DataFieldBind()
					' '' ''Response.Redirect("wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
				Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
					DataFieldBind()
					' '' ''Response.Redirect("wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
			End Select
		ElseIf Result1 = -1 Then
			' '' ''Response.Redirect("wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
		ElseIf Result1 = 0 Then    'Code Added
			'
		End If
	End Sub
	Private Sub SetTitle()
		If mLog.IsNew Then
			lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
		Else
			lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
		End If

		upnlTitle.Update()

	End Sub
	Private Sub ControlVisibility()

		btnLogDetails.Visible = (mLog.LogTypeID = 1)
		btnFuelOil.Visible = (mLog.LogTypeID = 1)


		'Added By Utkarsh On 06-Mar-2012
		btnParameterList.Visible = IIf(mMachine.IsTLP = True, False, True) And mLog.LogTypeID = 1  'IIf(mLog.IsTLP = True, False, True) And mLog.LogTypeID = 1
		btnLogPax.Visible = IIf(mLog.IsTLP = True Or AppSettings("ShowExtraLogTabs") = "False", False, True) And mLog.LogTypeID = 1
		btnHobbsOffset.Visible = IIf(mMachine.IsTLP = True Or AppSettings("ShowExtraLogTabs") = "False", False, True) And mLog.LogTypeID = 1
		btnFlightCrew.Visible = IIf(mMachine.IsTLP = True Or AppSettings("LogDetailPage") = "NewPage", True, False) And mLog.LogTypeID = 1 'Added By Utkarsh ON 16-Jul-2012 FOR ALL16072012-3
		'End
		btnMaintenanceAcitvity.Visible = IIf(mMachine.IsTLP = True Or AppSettings("LogDetailPage") = "NewPage", True, False) And mLog.LogTypeID = 1  'Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
		lblSnagReport.Visible = (mLog.LogTypeID = 1)


		btnLogPax.Enabled = Not mLog.IsNew
		btnHobbsOffset.Enabled = (mMachine.HourType = 2)

		'If mMELSnagCorrectiveAction.ImageSize > 0 Then
		'    ImageButton2.Visible = True
		'    btnDelAttach.Enabled = True
		'Else
		'    ImageButton2.Visible = False
		'    btnDelAttach.Enabled = False
		'End If

		If chkClose.Checked Then
			txtRectifiedDate.Enabled = True
		Else
			txtRectifiedDate.Enabled = False
		End If

		txtDueDate.Enabled = False

		'''''Commented by Saylee on 15-Dec-2014 as BSA needed this date editable
		'''''txtDateofoccurrence.Enabled = False
		txtDateofoccurrence.Enabled = False ' comment opened on 16-Jun-2023, as  now it should be disabled

		If mLog IsNot Nothing Then
			If mLog.MELSnagCorrectiveActions.Count > 10 Then
				'' btnSave.Visible = True
				'' btnBack.Visible = True

			End If
		End If
		'Added By Vikrant On 03-Apr-2013 For ALL01042013
		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
		'cmbSubATAList.DataBind()
		'End

		If cmbPartNo.SelectedIndex <= 0 Then
			If txtPartNo.Text <> "" Then
				'
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtPartNo.Text = ""
			End If
			If txtDescription.Text <> "" Then
				'
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtDescription.Text = ""
			End If
			If txtSerialNo.Text <> "" Then
				'
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtSerialNo.Text = ""
			End If
			'txtATAChapter.Text = ""


			txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)
			'txtPartNo.BackColor = txtATAChapter.BackColor.FromKnownColor(KnownColor.White)
			txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtDescription.ReadOnly = False
			txtPartNo.ReadOnly = False
			txtSerialNo.ReadOnly = False
		Else
			'txtFrequencyInHours.Enabled = True
			'If chkShowMEL.Checked = True And chkIsInHours.Checked = False Then
			'    txtFrequencyInDay.Enabled = True
			'End If
			txtDescription.ReadOnly = True
			txtPartNo.ReadOnly = True
			txtSerialNo.ReadOnly = True

			txtDescription.BackColor = Color.FromName("#E0E0E0")
			txtPartNo.BackColor = Color.FromName("#E0E0E0")
			txtSerialNo.BackColor = Color.FromName("#E0E0E0")
		End If

		If chkShowMEL.Checked = True Then
			If chkClose.Checked = True Then
				chkExtensionApplied.Enabled = False
			Else
				chkExtensionApplied.Enabled = True
				'txtExtensionInDays.Enabled = True
				'txtExtensionApprovalNo.Enabled = True
			End If
		Else
			chkExtensionApplied.Enabled = False
			txtExtensionInDays.Enabled = False
			txtExtensionApprovalNo.Enabled = False
		End If

		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)

		ControlVisibilityForAttachment()
		upnlFileupload.Update()

		upnlTabs.Update()
		upnlDetails.Update()

	End Sub
	Private Sub ControlVisibilityAfterEdit()
		'''If mMELSnagCorrectiveAction.MELCategoryID = 1 And mMELSnagCorrectiveAction.IsHours = True Then
		'''    chkIsInHours.Enabled = True
		'''    txtFrequencyInHours.Enabled = True
		'''ElseIf mMELSnagCorrectiveAction.MELCategoryID = 1 And mMELSnagCorrectiveAction.IsHours = False Then
		'''    chkIsInHours.Enabled = True
		'''    txtFrequencyInDay.Enabled = True
		'''ElseIf (mMELSnagCorrectiveAction.MELCategoryID = 2 Or mMELSnagCorrectiveAction.MELCategoryID = 3 Or mMELSnagCorrectiveAction.MELCategoryID = 4) And mMELSnagCorrectiveAction.IsHours = False Then
		'''    txtFrequencyInDay.Enabled = True
		'''ElseIf (mMELSnagCorrectiveAction.MELCategoryID = 2 Or mMELSnagCorrectiveAction.MELCategoryID = 3 Or mMELSnagCorrectiveAction.MELCategoryID = 4) And mMELSnagCorrectiveAction.IsHours = True Then
		'''    chkIsInHours.Enabled = True
		'''    txtFrequencyInHours.Enabled = True
		'''End If

		If mMELSnagCorrectiveAction.MELCategoryID = 1 And mMELSnagCorrectiveAction.IsHours = True Then
			chkIsInHours.Enabled = True
			txtFrequencyInHours.Enabled = True
			txtFrequencyInDay.Enabled = False
		ElseIf mMELSnagCorrectiveAction.MELCategoryID = 1 And mMELSnagCorrectiveAction.IsHours = False Then
			chkIsInHours.Enabled = True
			txtFrequencyInHours.Enabled = False
			txtFrequencyInDay.Enabled = True
		ElseIf (mMELSnagCorrectiveAction.MELCategoryID = 2 Or mMELSnagCorrectiveAction.MELCategoryID = 3 Or mMELSnagCorrectiveAction.MELCategoryID = 4) Then
			chkIsInHours.Enabled = False
			txtFrequencyInHours.Enabled = False
		End If

		cmbMELCategory.Enabled = False


		If mMELSnagCorrectiveAction.RectifiedDate.ToString <> "" Then
			cmbRectifiedLogNo.Enabled = True
		End If
		'If Not mMELSnagCorrectiveAction.LogID.Equals(Guid.Empty) Then
		'    lnkCheckStatus.Enabled = True
		'End If

		If cmbPartNo.SelectedIndex <= 0 Then
			If txtPartNo.Text <> "" Then
				'
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtPartNo.Text = ""
			End If
			If txtDescription.Text <> "" Then
				'
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtDescription.Text = ""
			End If
			If txtSerialNo.Text <> "" Then
				'
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtSerialNo.Text = ""
			End If
			'txtATAChapter.Text = ""


			txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)
			'txtPartNo.BackColor = txtATAChapter.BackColor.FromKnownColor(KnownColor.White)
			txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtDescription.ReadOnly = False
			txtPartNo.ReadOnly = False
			txtSerialNo.ReadOnly = False


			'cmbMELCategory.BackColor = Color.FromKnownColor(KnownColor.White)
		Else
			'txtFrequencyInHours.Enabled = True
			'''If chkShowMEL.Checked = True And chkIsInHours.Checked = False Then
			'''    txtFrequencyInDay.Enabled = True
			'''End If
			txtDescription.ReadOnly = True
			txtPartNo.ReadOnly = True
			txtSerialNo.ReadOnly = True

			txtDescription.BackColor = Color.FromName("#E0E0E0")
			txtPartNo.BackColor = Color.FromName("#E0E0E0")
			txtSerialNo.BackColor = Color.FromName("#E0E0E0")
			'cmbMELCategory.BackColor = Color.FromName("#E0E0E0")
		End If

		If mMELSnagCorrectiveAction.IsHours = True Then
			txtFrequencyInDay.Enabled = False
			txtFrequencyInDay.Text = "0"
		Else
			txtFrequencyInHours.Text = ""
			txtFrequencyInHours.Enabled = False
		End If

		If mMELSnagCorrectiveAction.IsMEL = True Or chkShowMEL.Checked = True Then
			If mMELSnagCorrectiveAction.InvestigationStatus = True Or chkClose.Checked = True Then
				chkExtensionApplied.Enabled = False
			Else
				chkExtensionApplied.Enabled = True
			End If
			upnlExtension.Update()
		Else
			chkExtensionApplied.Enabled = False
			txtExtensionInDays.Enabled = False
			txtExtensionApprovalNo.Enabled = False
			txtExtensionInDays.Text = 0
			txtExtensionApprovalNo.Text = ""
			mMELSnagCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			upnlExtension.Update()
		End If
		'Commented by Saylee on 15-Dec-2014 as BSA needed this date editable
		'txtDateofoccurrence.Enabled = False
	End Sub
	'Private Sub ControlVisibilityAfterEdit()
	'    If mMELSnagCorrectiveAction.MELCategoryID = 1 And mMELSnagCorrectiveAction.IsHours = True Then
	'        chkIsInHours.Enabled = True
	'        txtFrequencyInHours.Enabled = True
	'    ElseIf mMELSnagCorrectiveAction.MELCategoryID = 1 And mMELSnagCorrectiveAction.IsHours = False Then
	'        chkIsInHours.Enabled = True
	'        txtFrequencyInDay.Enabled = True
	'    ElseIf (mMELSnagCorrectiveAction.MELCategoryID = 2 Or mMELSnagCorrectiveAction.MELCategoryID = 3 Or mMELSnagCorrectiveAction.MELCategoryID = 4) And mMELSnagCorrectiveAction.IsHours = False Then
	'        txtFrequencyInDay.Enabled = True
	'    ElseIf (mMELSnagCorrectiveAction.MELCategoryID = 2 Or mMELSnagCorrectiveAction.MELCategoryID = 3 Or mMELSnagCorrectiveAction.MELCategoryID = 4) And mMELSnagCorrectiveAction.IsHours = True Then
	'        chkIsInHours.Enabled = True
	'        txtFrequencyInHours.Enabled = True
	'    End If
	'    If mMELSnagCorrectiveAction.RectifiedDate.ToString <> "" Then
	'        cmbRectifiedLogNo.Enabled = True
	'    End If

	'    If chkClose.Checked Then                'Code Added by Yogita for control visibility
	'        txtRectifiedDate.Enabled = True
	'    Else
	'        txtRectifiedDate.Enabled = False
	'    End If

	'    If chkIsInHours.Checked Then
	'        txtFrequencyInDay.Enabled = False
	'        txtFrequencyInDay.Text = "0"
	'    Else
	'        txtFrequencyInHours.Text = ""
	'        txtFrequencyInHours.Enabled = False
	'    End If

	'    If cmbPartNo.SelectedIndex <= 0 Then
	'        If txtPartNo.Text <> "" Then
	'            '
	'        ElseIf cmbPartNo.SelectedIndex <= 0 Then
	'            txtPartNo.Text = ""
	'        End If
	'        If txtDescription.Text <> "" Then
	'            '
	'        ElseIf cmbPartNo.SelectedIndex <= 0 Then
	'            txtDescription.Text = ""
	'        End If
	'        If txtSerialNo.Text <> "" Then
	'            '
	'        ElseIf cmbPartNo.SelectedIndex <= 0 Then
	'            txtSerialNo.Text = ""
	'        End If
	'        'txtATAChapter.Text = ""


	'        txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)
	'        txtPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
	'        txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

	'        txtDescription.ReadOnly = False
	'        txtPartNo.ReadOnly = False
	'        txtSerialNo.ReadOnly = False

	'        cmbATAChapter.Enabled = True
	'        'cmbATAChapter.BackColor = Color.FromKnownColor(KnownColor.White)
	'        If mMELSnagCorrectiveAction.IsMEL = True Then
	'            cmbMELCategory.Enabled = True
	'            'cmbMELCategory.BackColor = Color.FromKnownColor(KnownColor.White)

	'            'cmbATAChapter.Enabled = False
	'            'cmbATAChapter.BackColor = Color.FromName("#E0E0E0")
	'        Else
	'            cmbMELCategory.Enabled = False
	'            'cmbMELCategory.BackColor = Color.FromName("#E0E0E0")

	'        End If
	'    Else
	'        'txtFrequencyInHours.Enabled = True
	'        If chkShowMEL.Checked = True And chkIsInHours.Checked = False Then
	'            txtFrequencyInDay.Enabled = True
	'        End If
	'        txtDescription.ReadOnly = True
	'        txtPartNo.ReadOnly = True
	'        txtSerialNo.ReadOnly = True

	'        txtDescription.BackColor = Color.FromName("#E0E0E0")
	'        txtPartNo.BackColor = Color.FromName("#E0E0E0")
	'        txtSerialNo.BackColor = Color.FromName("#E0E0E0")

	'        cmbMELCategory.Enabled = False
	'        'cmbMELCategory.BackColor = Color.FromName("#E0E0E0")
	'        If mMELSnagCorrectiveAction.IsMEL = True Then
	'            cmbATAChapter.Enabled = False
	'            'cmbATAChapter.BackColor = Color.FromName("#E0E0E0")
	'        Else
	'            chkIsInHours.Enabled = False
	'            cmbATAChapter.Enabled = True
	'            'cmbATAChapter.BackColor = Color.FromKnownColor(KnownColor.White)
	'        End If

	'    End If
	'    ControlVisibilityForAttachment()
	'    upnlFileupload.Update()

	'    upnlDetails.Update()
	'End Sub
	Private Function GetDefectNo() As String
		Dim No As New Random
		Dim ReportNo As String
		ReportNo = "DEFECT" + "/" + mLog.RegNo
		Return ReportNo
	End Function
	Private Sub SetObject()
		With mLog.MELSnagCorrectiveActions.CurrentItem
			'.LogID = New Guid(cmbLogNo.SelectedValue)
			.LogID = mLog.ID
			If txtDateofoccurrence.Text.ToString <> "" Then
				.DateOfOccurrence = txtDateofoccurrence.Text.ToString
			Else
				.DateOfOccurrence = System.DBNull.Value
			End If
			.DefectReportNo = Trim(txtDefectReportNo.Text)
			.No = Val(txtNo.Text)
			.Sector = Trim(txtSector.Text)
			'.DateOfOccurence = mLog.Date
			.LastMajorCheckHour = Trim(txtLastMajorCheck.Text)
			.SnagReportedBy = Trim(txtSnagReportedBy.Text)
			.ReportedBy = Trim(txtReportedBy.Text)
			.PartID = New Guid(cmbPartNo.SelectedValue)

			If cmbPartNo.SelectedIndex > 0 Then
				.PartSerialNo = mMELSnagPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").SerialNo
			Else
				.PartSerialNo = Trim(txtSerialNo.Text)
			End If
			.Description = Trim(txtDescription.Text)
			.ComponentHour = Trim(txtHrsofComp.Text)
			.Defect = Trim(txtDefect.Text)
			.CauseOfDefect = Trim(txtCauseofDefect.Text)
			.Action = Trim(txtAction.Text)
			.ActionAgainstStaff = Trim(txtActionTakenAganistEngStaff.Text)
			.PreventionTaken = Trim(txtPreventiveMeasuresTaken.Text)

			Dim mMELPartList As MELPartList = MELPartList.GetMELPartList(mLog.Date.ToString, mLog.MachineID.ToString)
			If mMELPartList.Contains(mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Name, mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").CompID) Then
				'.ATAChapterID = mMELPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").CompStatusATAID
				.MELCategoryID = mMELPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").MELCategoryID
				.IsMEL = True
			Else
				.IsMEL = chkShowMEL.Checked
				.MELCategoryID = cmbMELCategory.SelectedValue
			End If
			'Commented & Added By Vikrant On 12-Apr-2013
			'If cmbATAChapter.SelectedIndex > 0 Then
			'    .ATAChapterID = New Guid(cmbATAChapter.SelectedValue)
			'End If
			.ATAChapterID = New Guid(cmbATAChapter.SelectedValue)
			'End
			.IsMajor = rbMajor.Checked
			.IsMinor = rbMinor.Checked
			.InvestigationStatus = chkClose.Checked
			.MachineID = New Guid(mLog.MachineID.ToString)
			'.LogNo = mReportLogRegister(New Guid(cmbLogNo.SelectedValue)).LogNo
			'.LogNo = cmbLogNo.SelectedItem.Text
			'.LogNo = cmbLogNo.SelectedItem.Text
			.IsHours = chkIsInHours.Checked
			.FrequencyInDays = Val(txtFrequencyInDay.Text)
			.FrequencyInHours = txtFrequencyInHours.Text.Trim
			'.RectifiedMechanic = txtRectificationMechanic.Text.Trim
			.RectifiedStation = txtRectificationSector.Text.Trim
			If txtDueDate.Text.ToString <> "" Then
				.DueDate = txtDueDate.Text.ToString
			Else
				.DueDate = System.DBNull.Value
			End If

			If txtRectifiedDate.Text.ToString <> "" Then
				.RectifiedDate = txtRectifiedDate.Text.ToString
			Else
				.RectifiedDate = System.DBNull.Value
			End If

			If cmbRectifiedLogNo.SelectedIndex > 0 Then
				.RectifiedLogID = New Guid(cmbRectifiedLogNo.SelectedValue)
			End If

			.PartNo = Trim(txtPartNo.Text)
			.IsRepetitive = chkIsRepetitive.Checked
			.Remark = Trim(txtRemark.Text)
			.SubATAID = New Guid(cmbSubATAList.SelectedValue) 'Added By Vikrant On 02-Apr-2013 For ALL01042013
			.IsPireps = rbPireps.Checked  'Added By Saylee On 29-Apr-2013 For ALL29042013-3
			.IsMaintenanceDefect = rbMaintenanceDefect.Checked 'Added By Saylee On 29-Apr-2013 For ALL29042013-3
			.IsInReliability = chkIsInReliability.Checked 'Added By Saylee On 29-Apr-2013 For ALL29042013-3
			.AssemblyStatusID = New Guid(cmbAssembly.SelectedValue) 'Added By Vikrant On 02-Sept-2014 For All04092014
			.ExtensionApplied = chkExtensionApplied.Checked
			.ExtensionInDays = Val(txtExtensionInDays.Text)
			.ExtensionApprovalNo = Trim(txtExtensionApprovalNo.Text)
			.IncidentTypeID = cmbIncidentType.SelectedValue
			.IncidentTypeName = cmbIncidentType.SelectedItem.Text
		End With

		' '' ''If MyFile1.FileBytes.Length <> "" Then
		' '' ''    Dim BackupPath As String = ""
		' '' ''    BackupPath = AppSettings("DOCPath") & "New.PDF"

		' '' ''    Try
		' '' ''        MyFile1.PostedFile.SaveAs(BackupPath)
		' '' ''        Dim fs As New FileStream(BackupPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
		' '' ''        Dim fileSize As Integer = CType(fs.Length, Integer)

		' '' ''        Dim fileBytes(fileSize) As Byte
		' '' ''        fs.Read(fileBytes, 0, fileSize)

		' '' ''        'mLog.MELSnagCorrectiveActions.CurrentItem.ImageFile = fileBytes
		' '' ''        'mLog.MELSnagCorrectiveActions.CurrentItem.ImageSize = fileSize
		' '' ''        'mLog.MELSnagCorrectiveActions.CurrentItem.FileExtension = MyFile1.Extension

		' '' ''        mLog.MELSnagCorrectiveActions.CurrentItem.ImageFile = MyFile1.FileBytes
		' '' ''        mLog.MELSnagCorrectiveActions.CurrentItem.ImageSize = MyFile1.FileContent.Length
		' '' ''        mLog.MELSnagCorrectiveActions.CurrentItem.FileExtension = System.IO.Path.GetExtension(MyFile1.PostedFile.FileName)

		' '' ''        btnDelAttach.Enabled = True
		' '' ''        fs.Close()

		' '' ''        System.IO.File.Delete(BackupPath)

		' '' ''    Catch ex As Exception
		' '' ''    End Try
		' '' ''End If

		If mFileAttach.Size > 0 Then
			mMELSnagCorrectiveAction.IsAttachmentAdded = True
		Else
			mMELSnagCorrectiveAction.IsAttachmentAdded = False
		End If

		If mMELSnagCorrectiveAction IsNot Nothing Then
			mLog.MELSnagCorrectiveActions.CurrentItem.ImageFile = mMELSnagCorrectiveAction.ImageFile
			mLog.MELSnagCorrectiveActions.CurrentItem.ImageSize = mMELSnagCorrectiveAction.ImageSize
			mLog.MELSnagCorrectiveActions.CurrentItem.FileExtension = mMELSnagCorrectiveAction.FileExtension
		End If



		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		' Session("mLog") = mLog 
	End Sub
	Private Sub SetGrid()

		'Dim P As Boolean
		'For j As Integer = 0 To dgLogDefectActions.Items.Count - 1
		'    P = mLog.MELSnagCorrectiveActions(j).IsAttachmentAdded 'CType(Me.dgLogDefectActions.Items.Item(j).Cells(12).Text, Integer)
		'    If P = False Then

		'        dgLogDefectActions.Items.Item(j).Cells(11).Enabled = False
		'    End If
		'Next
		dgLogDefectActions.Columns(2).HeaderText = IIf(mLog.IsUTC = True, "Date Of Occurrence (UTC)", "Date Of Occurrence")
		dgLogDefectActions.Columns(7).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Is ADD", "Is MEL") ' Added By Vikrant On 07-Sep-2020 For ALL07092020
	End Sub
	'MLNo
	Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
		If IsValid Then
			'SetObject()
			Session("mMaintenanceID") = mMELSnagCorrectiveAction.ID
			mMaintenanceDoneByEmployees = mMELSnagCorrectiveAction.MaintenanceDoneByEmployees
			Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
			Session("MaintenanceDoneOnDate") = mMELSnagCorrectiveAction.DateOfOccurrence.ToString
			ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
		Else
			upnlErrorList.Update()
			upnlErrors1.Update()
		End If

	End Sub
	Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
		For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
			Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
			If Not mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Contains(ID) Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
			ElseIf mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Contains(ID) Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
				'mAssemblyStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
			End If
		Next

		For j As Integer = 0 To mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count - 1
			If Not mMaintenanceDoneByEmployees.Contains(mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(j).ID) Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Remove(mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(j).ID, "")
			End If
		Next
		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		BindLicenceNo()
		SetLicenceCount() 'MLNo
		upnlLicenceNo.Update()
	End Sub
	Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
		'SetObject()
		If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
			LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
			EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
		Else
			LicenseNo = Trim(txtLicenceNo.Text)
		End If
		DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
		Session("LicenseNo") = LicenseNo
		Session("EmployeeID") = DoneByID
		If Not DoneByID.Equals(Guid.Empty) Then
			If mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
			Else
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Add(mMELSnagCorrectiveAction.ID, 11, DoneByID, LicenseNo, "", EmpName)
			End If

		Else
			If mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.RemoveAt(0)
			End If
		End If
		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		BindLicenceNo()
		SetLicenceCount()
	End Sub
	'End
	Private Sub hdnimgBtnMELMasterChapter_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnMELMasterChapter.Click
		mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")

		If mMELSnagCorrectiveAction.MELCategoryID <= 0 Then
			chkShowMEL.Checked = False
		Else
			chkShowMEL.Checked = True
		End If


		cmbMELCategory.SelectedValue = mMELSnagCorrectiveAction.MELCategoryID

		txtDefect.Text = mMELSnagCorrectiveAction.Defect
		cmbATAChapter.SelectedValue = mMELSnagCorrectiveAction.ATAChapterID.ToString

		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)

		mSubATAList = SubATAList.GetSubATAList(mMELSnagCorrectiveAction.ATAChapterID, "", "(SELECT)")
		cmbSubATAList.DataSource = mSubATAList
		cmbSubATAList.DataBind()
		Session("mSubATAList") = mSubATAList
		upnlSubATA.Update()

		cmbSubATAList.SelectedValue = mMELSnagCorrectiveAction.SubATAID.ToString
		txtFrequencyInDay.Text = mMELSnagCorrectiveAction.FrequencyInDays
		txtFrequencyInHours.Text = mMELSnagCorrectiveAction.FrequencyInHours
		chkIsInHours.Checked = mMELSnagCorrectiveAction.IsHours
		txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		cmbIncidentType.SelectedValue = mMELSnagCorrectiveAction.IncidentTypeID

		ControlVisibilityAfterEdit()
		upnlMMELDetails.Update()
		upnlDetails.Update()
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallParentFunction", "CallParentFunction();", True)
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
	Private Sub NewMELSnagCorrectiveAction()
		'Added by By Utkarsh on 13-Aug-2013 for ALL13082013-2
		chkShowMEL.Checked = False
		'End
		chkShowMEL.Enabled = True
		DataFieldBind()

		Session("mLog") = mLog
		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " Defect No. : " + mLog.MELSnagCorrectiveActions.CurrentItem.DefectNo + " Date of occurrence : " + mLog.MELSnagCorrectiveActions.CurrentItem.DateOfOccurrenceFormatted + " Defect : " + mLog.MELSnagCorrectiveActions.CurrentItem.Defect
		MarkLog(Util.Action.Save, "Log Defect Action", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)

		DataBindGrid()

		If chkIsInHours.Checked = True Then
			chkIsInHours.Checked = False
		End If
		txtFrequencyInDay.Text = "0"
		txtFrequencyInHours.Text = ""
		chkIsInHours.Checked = False
		chkIsInHours.Enabled = False

		txtFrequencyInHours.Enabled = False
		txtFrequencyInDay.Enabled = False
		cmbMELCategory.Enabled = False
		txtDueDate.Text = ""
		txtRectifiedDate.Text = "" '---

		txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)
		txtPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
		txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

		txtDescription.ReadOnly = False
		txtPartNo.ReadOnly = False
		txtSerialNo.ReadOnly = False

		If mRectifiedReportLogRegister IsNot Nothing Then
			cmbRectifiedLogNo.SelectedValue = Guid.Empty.ToString '---
		End If
		If txtDefect.Enabled = True Then
			setFocus(txtDefect)
		End If

		chkExtensionApplied.Checked = False
		txtExtensionInDays.Text = "0"
		txtExtensionApprovalNo.Text = ""

		mMELSnagCorrectiveAction = MELSnagCorrectiveAction.NewChildMELSnagCorrectiveAction(mLog.ID)
		mMELSnagCorrectiveAction.LogNo = mLog.LogNoLogPageNo
		mMELSnagCorrectiveAction.DateOfOccurrence = mLog.Date
		mMELSnagCorrectiveAction.Sector = mLog.SourceName

		With mMELSnagCorrectiveActionLog
			'txtLastMajorCheck.Text = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H" + ", " + mMELSnagCorrectiveActionLog.Item(0).FinalLandings + " L"
			If mMELSnagCorrectiveActionLog.Item(0).FinalLandings = "" Then
				mMELSnagCorrectiveAction.LastMajorCheckHour = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H"
			Else
				mMELSnagCorrectiveAction.LastMajorCheckHour = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H" + ", " + mMELSnagCorrectiveActionLog.Item(0).FinalLandings + " L"
			End If
		End With

		If mMELSnagCorrectiveAction.IsNew Then
			mMELSnagCorrectiveAction.DefectReportNo = GetDefectNo()
		Else
			mMELSnagCorrectiveAction.DefectReportNo = mMELSnagCorrectiveAction.DefectReportNo
			mMELSnagCorrectiveAction.No = mMELSnagCorrectiveAction.No
		End If

		mFileAttach = FileAttach.NewAttachment(Guid.Empty, mMELSnagCorrectiveAction.ID) 'Sort = 1 : Installation
		Session("mFileAttach") = mFileAttach

		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		Session("Edit") = False
		BindLicenceNo() 'LicenceNo
		SetLicenceCount()
		dgLogDefectActions.Columns(2).HeaderText = IIf(mLog.IsUTC = True, "Date Of Occurrence (UTC)", "Date Of Occurrence")
		dgLogDefectActions.Columns(7).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Is ADD", "Is MEL") ' Added By Vikrant On 07-Sep-2020 For ALL07092020
		DataBind()
		SetGrid()

		If chkClose.Checked Then
			txtRectifiedDate.Enabled = True
		Else
			txtRectifiedDate.Enabled = False
		End If
		If mMELSnagCorrectiveAction.DateOfOccurrence.ToString = "" Then
			txtDateofoccurrence.Text = mMELSnagCorrectiveAction.DateOfOccurrence.ToString
		Else
			txtDateofoccurrence.Text = mMELSnagCorrectiveAction.DateOfOccurrenceFormatted
		End If

		ImageButton2.Visible = False
		btnDelAttach.Enabled = False

		upnlSnagType.Update()
	End Sub

	Private Function Get_Whichever_LesserDate(LogDate As String, OccurranceDate As String) As String
		Dim mLogDate As SmartDate = New SmartDate(LogDate)
		Dim mOccurranceDate As SmartDate = New SmartDate(OccurranceDate)

		If CDate(mLogDate.ToString) < CDate(mOccurranceDate.ToString) Then
			Return mLogDate.ToString
		Else
			Return mOccurranceDate.ToString
		End If
	End Function
	Private Sub ControlVisibilityForAttachment()
		If mFileAttach IsNot Nothing Then
			If mFileAttach.Size > 0 Then 'change from  to current condition
				ImageButton2.Visible = True
				btnDelAttach.Enabled = True
			Else
				ImageButton2.Visible = False
			End If
		End If

	End Sub
	Private Sub GetAttachment()
		If mMELSnagCorrectiveAction.IsAttachmentAdded And mFileAttach Is Nothing Then
			mFileAttach = FileAttach.GetAttachment(mMELSnagCorrectiveAction.ID)
			Session("mFileAttach") = mFileAttach
		End If

		'If mFileAttach Is Nothing Then
		'    NewRecordAttachment()
		'End If
	End Sub
	Private Sub SaveAttachment() '
		mFileAttach.ReferenceID = mMELSnagCorrectiveAction.ID
		If mFileAttach.Size > 0 Then
			Try
				mFileAttach.Save()
				Session("mFileAttach") = mFileAttach
				'mEmployee.IsAttachmentAdded = True
			Catch ex As Exception
				ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
			End Try
		Else
			If (Not mMELSnagCorrectiveAction.IsNew) And IsAttachmentDeleted Then
				FileAttach.DeleteAttachment(mFileAttach.ID, mMELSnagCorrectiveAction.ID)
			End If
			IsAttachmentDeleted = False
			Session("IsAttachmentDeleted") = IsAttachmentDeleted
		End If
	End Sub
	Private Sub ViewImage()
		Dim No As New Random
		Dim StrName As String = "abc" & No.Next.ToString

		If mFileAttach.Size > 0 Then
			Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
			Dim fs As FileStream
			If File.Exists(AppSettings("DOCPath")) = False Then
				'Delete File if exist
				System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
				' Create the file.
				fs = File.Create(path)
				'' Add some information to the file.
				fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
				fs.Close()
				Session("DOCPath") = path
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
			End If
		End If
	End Sub
	'End
#End Region

#Region " DataBinding "
	Private Sub DataFieldBind()
		mATAList = ATAList.GetATAList("", "(SELECT)")
		Session("mATAList") = mATAList
		cmbATAChapter.DataSource = mATAList

		mTempAssemblyList = AssemblyList.GetAssemblyList(1, mLog.MachineID.ToString)
		Session("mTempAssemblyList") = mTempAssemblyList


		'If Not mMELSnagCorrectiveAction Is Nothing And Not mMELSnagCorrectiveAction.IsNew Then
		'mReportLogRegister = ReportLogRegister.GetRectifiedLog(mMELSnagCorrectiveAction.DateOfOccurrence.ToString, mMELSnagCorrectiveAction.DateOfOccurrence.ToString, mTempAssemblyList(0).ID.ToString, mMELSnagCorrectiveAction.MachineID.ToString, False, , 1, , , , "<SELECT>", True)
		'Else
		mReportLogRegister = ReportLogRegister.GetRectifiedLog(mLog.Date.ToString, mLog.Date.ToString, mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "(SELECT)", True, mLog.ID.ToString, True)
		'End If


		'cmbLogNo.DataSource = mReportLogRegister
		Session("mReportLogRegister") = mReportLogRegister

		'chkShowMEL.DataBind()
		cmbPartNo.Items.Clear()
		mMELSnagPartList = MELSnagPartList.GePartList(mLog.Date.ToString, mLog.MachineID.ToString, "(SELECT)")
		cmbPartNo.DataSource = mMELSnagPartList
		Session("mMELSnagPartList") = mMELSnagPartList

		If chkShowMEL.Checked = True Then
			If mMELSnagCorrectiveAction IsNot Nothing Then
				'Added By Utkarsh on 15-Jul-2013 FOR ALL15072013-3
				If Not mMELSnagPartList.Contains(mMELSnagCorrectiveAction.PartNo) Then mMELSnagCorrectiveAction.PartID = Guid.Empty
				'End
			End If

			cmbATAChapter.Enabled = False
			cmbSubATAList.Enabled = False
		Else
			cmbATAChapter.Enabled = True
			cmbSubATAList.Enabled = True
		End If

		If mMELSnagCorrectiveAction Is Nothing Then
			mMELSnagCorrectiveAction = MELSnagCorrectiveAction.NewChildMELSnagCorrectiveAction(mLog.ID)
			Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		Else
			If mMELSnagCorrectiveAction.ImageSize > 0 Then
				ImageButton2.Visible = True
				btnDelAttach.Enabled = True
			Else
				ImageButton2.Visible = False
				btnDelAttach.Enabled = False
			End If
		End If

		DataBindGrid()

		'Commented and Added by Utkarsh ON 27-Feb-2013 FOR All27022013
		'mMELSnagCorrectiveAction.LogNo = mLog.LogTextNo
		mMELSnagCorrectiveAction.LogNo = mLog.LogNoLogPageNo
		'End
		If mMELSnagCorrectiveAction.IsNew Then mMELSnagCorrectiveAction.DateOfOccurrence = mLog.Date
		mMELSnagCorrectiveAction.Sector = mLog.SourceName

		If mMELSnagCorrectiveAction.IsNew Then
			mMELSnagCorrectiveAction.DefectReportNo = GetDefectNo()
		Else
			mMELSnagCorrectiveAction.DefectReportNo = mMELSnagCorrectiveAction.DefectReportNo
			mMELSnagCorrectiveAction.No = mMELSnagCorrectiveAction.No
		End If

		'If Not mMELSnagCorrectiveAction Is Nothing Then
		'    If IsDBNull(mMELSnagCorrectiveAction.RectifiedDate) = True Then
		'        mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(Today.Date.ToString, Today.Date.ToString, mTempAssemblyList(0).ID.ToString, mMELSnagCorrectiveAction.MachineID.ToString, False, , 1, , , , "<SELECT>", True)
		'    Else
		'        mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(mMELSnagCorrectiveAction.RectifiedDate.ToString, mMELSnagCorrectiveAction.RectifiedDate.ToString, mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 1, , , , "<SELECT>", True)
		'    End If
		'End If
		'cmbRectifiedLogNo.DataSource = mRectifiedReportLogRegister
		'Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister

		'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		mMELSnagCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(mLog.ID.ToString)
		Session("mMELSnagCorrectiveActionLog") = mMELSnagCorrectiveActionLog

		If mMELSnagCorrectiveActionLog.Count > 0 Then
			With mMELSnagCorrectiveActionLog
				'txtLastMajorCheck.Text = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H" + ", " + mMELSnagCorrectiveActionLog.Item(0).FinalLandings + " L"
				If mMELSnagCorrectiveActionLog.Item(0).FinalLandings = "" Then
					mMELSnagCorrectiveAction.LastMajorCheckHour = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H"
				Else
					mMELSnagCorrectiveAction.LastMajorCheckHour = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H" + ", " + mMELSnagCorrectiveActionLog.Item(0).FinalLandings + " L"
				End If
			End With
		End If

		'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
		'Added By Vikrant On 02-Apr-2013 For ALL01042013
		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
		mSubATAList = SubATAList.GetSubATAList(mMELSnagCorrectiveAction.ATAChapterID, "", "(SELECT)")
		cmbSubATAList.DataSource = mSubATAList
		Session("mSubATAList") = mSubATAList
		'End

		If mMELSnagCorrectiveAction IsNot Nothing Then

			If mMELSnagCorrectiveAction.DueDate.ToString = "" Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DueDate.ToString
			Else
				txtDueDate.Text = mMELSnagCorrectiveAction.DueDateFormatted
			End If
			If mMELSnagCorrectiveAction.RectifiedDate.ToString = "" Then
				txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDate.ToString
			Else
				txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDateFormatted
			End If
			If mMELSnagCorrectiveAction.DateOfOccurrence.ToString = "" Then
				txtDateofoccurrence.Text = mMELSnagCorrectiveAction.DateOfOccurrence.ToString
			Else
				txtDateofoccurrence.Text = mMELSnagCorrectiveAction.DateOfOccurrenceFormatted
			End If

		End If

		'Added By Vikrant On 02-Sept-2014 For All04092014
		mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, mLog.MachineID.ToString, mLog.Date.ToString, "", True)
		cmbAssembly.DataSource = mAssemblylist
		Session("mAssemblylist") = mAssemblylist
		'End

		cmbMELCategory.DataSource = MELCategoryList.GetMELCategoryList("(SELECT)")
		cmbMELCategory.DataBind()

		cmbIncidentType.DataSource = IncidentTypeList.GetIncidentTypeList() 'Added By Prashant On 23-Nov-2021 ALL23112021
		cmbIncidentType.DataBind()

		If mMELSnagCorrectiveAction.IsNew Then

			mFileAttach = FileAttach.NewAttachment(Guid.Empty, mMELSnagCorrectiveAction.ID)
			Session("mFileAttach") = mFileAttach
		Else
			If mMELSnagCorrectiveAction.IsAttachmentAdded Then
				mFileAttach = FileAttach.GetAttachment(mMELSnagCorrectiveAction.ID) 'Sort = 1 - Installation
				Session("mFileAttach") = mFileAttach
			End If
		End If
		BindLicenceNo() 'MLNo
		dgLogDefectActions.Columns(2).HeaderText = IIf(mLog.IsUTC = True, "Date Of Occurrence (UTC)", "Date Of Occurrence")
		dgLogDefectActions.Columns(7).HeaderText = IIf(AppSettings("MELSnagNomenclature") = "True", "Is ADD", "Is MEL") ' Added By Vikrant On 07-Sep-2020 For ALL07092020
		DataBind()

		upnlDetails.Update()

	End Sub
	Private Sub DataBindGrid()
		dgLogDefectActions.DataSource = mLog.MELSnagCorrectiveActions
		dgLogDefectActions.DataBind()


		upnlDetails.Update()

	End Sub
	'Modified By Harsh on 1st April 2024 -- Removed the validation checks for Rectified Date & Log from this function
	'[ as a separate Function is created for the same ], So that validation messages are displayed in respective tabs.
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)

		'If txtDefect.Text.Length > 1000 And custValidator.ControlToValidate = "txtDefect" Then
		'    txtDefect.Text = txtDefect.Text.Substring(0, 996) + "..."
		'    custValidator.ErrorMessage = "Defect Length must not be greater than 1000 character."
		'    e.IsValid = False
		'ElseIf txtAction.Text.Length > 1000 And custValidator.ControlToValidate = "txtAction" Then
		'    txtAction.Text = txtAction.Text.Substring(0, 996) + "..."
		'    custValidator.ErrorMessage = "Action Length must not be greater than 1000 character."
		'    e.IsValid = False
		'elseif
		If custValidator.ControlToValidate = "cmbATAChapter" Then
			If (chkIsInReliability.Checked = True And cmbATAChapter.SelectedIndex = 0) Then
				custValidator.ErrorMessage = "Select the ATA Chapter as it is to be considered in Reliability."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "cmbMELCategory" Then
			If (chkShowMEL.Checked = True) And (cmbMELCategory.SelectedIndex = 0) Then
				custValidator.ErrorMessage = "Select the " & IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL") & " Category." 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtExtensionInDays" Then
			If (chkExtensionApplied.Checked = True And (txtExtensionInDays.Text = "0" Or txtExtensionInDays.Text = "")) Then
				custValidator.ErrorMessage = "Extension days should be greater than zero."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
		upnlErrorList.Update()
		upnlErrors1.Update()
	End Sub

	'Modified By Harsh on 1st April 2024 -- Updated the validation messages.
	Public Function customvalidate1()
		Dim strMsg As String = ""

		If Trim(txtDefectReportNo.Text) = "" Then strMsg = "Defect Text is Required."
		If Trim(txtNo.Text) = "" Then strMsg = strMsg + "Defect No is Required."
		If Trim(txtDefect.Text) = "" Then strMsg = strMsg + "Defect is Required."

		If strMsg <> "" Then
			cvDefectList.ErrorMessage = strMsg
			cvDefectList.IsValid = False
			Return False
		End If

		Return True
	End Function

	'Modified By Harsh on 1st April 2024 -- Updated the validation message for Rectified Log-No from [Select Rectified Log No] to [Please select Rectified Log No.].
	Public Function customvalidate2()

		Dim strMsg1 As String = ""
		If (chkClose.Checked = True And txtRectifiedDate.Text.ToString = "") Then
			strMsg1 = strMsg1 + "Please select the Rectification Date."

		ElseIf (chkClose.Checked = True And txtRectifiedDate.Text.ToString <> "" And cmbRectifiedLogNo.SelectedIndex = 0) Then
			strMsg1 = strMsg1 + "Please select Rectified Log No."

		ElseIf (New SmartDate(txtRectifiedDate.Text.ToString).Date < New SmartDate(txtDateofoccurrence.Text.ToString).Date) And (chkClose.Checked = True) Then
			strMsg1 = strMsg1 + "Rectified Date should be equal or later to Occurrence Date."
		End If

		If strMsg1 <> "" Then
			CustomValidator2.ErrorMessage = strMsg1
			CustomValidator2.IsValid = False
			Return False
		End If

		Return True
	End Function
	Private Sub RectifiedLog() 'Added By Prashant 22-Feb-2013 'All22022013-1
		If mMELSnagCorrectiveAction IsNot Nothing Then
			If IsDBNull(mMELSnagCorrectiveAction.RectifiedDate) = False Then
				' 'Commented by Saylee on 15-Dec-2014 as to fill combo from Log date to Recification date 
				''mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(mMELSnagCorrectiveAction.DateOfOccurrence.ToString, mMELSnagCorrectiveAction.RectifiedDate.ToString, mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "<SELECT>", True, mLog.ID.ToString)
				mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(Get_Whichever_LesserDate(mLog.Date.ToString, mMELSnagCorrectiveAction.DateOfOccurrence.ToString), "1/1/2100", mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "(SELECT)", True, mLog.ID.ToString, True)
			End If
		End If
		cmbRectifiedLogNo.DataSource = mRectifiedReportLogRegister
		cmbRectifiedLogNo.DataBind()
		cmbRectifiedLogNo.SelectedValue = mMELSnagCorrectiveAction.RectifiedLogID.ToString

		upnlDetails.Update()

	End Sub
	Private Sub addAttributes()
		txtFrequencyInDay.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInDay').value,event)")
		txtFrequencyInHours.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtFrequencyInHours').value,event)")
		txtExtensionInDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtExtensionInDays').value,event)")
		'txtFrequencyInHours.Attributes.Add("onkeypress", "var key; if(window.event){ key = event.keyCode;}else if(event.which){ key = event.which;} return (key == 45 || key == 13 || key == 8 || key == 9 || key == 189 || (key >= 48 && key <= 58) )")
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		addAttributes()

		If Not IsPostBack And CType(Session("sender"), String) = "" Then
			If txtDefectReportNo.Enabled = True Then
				setFocus(txtDefectReportNo)
			End If
			If Session("LogFromMEL") IsNot Nothing Then 'Used when WO Created from Grid and returned on this page
				mLog = CType(Session("LogFromMEL"), Log)
				Session("mLog") = mLog
				Session.Remove("LogFromMEL")
			End If
			DataFieldBind()
			RectifiedLog() 'Added By Prashant 22-Feb-2013 'All22022013-1
			If chkShowMEL.Checked = False Then cmbMELCategory.Enabled = False
			'MLNo
			SetLicenceCount()
			UserNameForLicenceList = User.Identity.Name
			Session("UserNameForLicenceList") = UserNameForLicenceList
			'End
		End If

		SetTitle()
		ControlVisibility()
		SetGrid()

	End Sub
	Private Sub dgLogDefectActions_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLogDefectActions.ItemCommand
		Dim Index As Int32 = dgLogDefectActions.CurrentPageIndex * dgLogDefectActions.PageSize + e.Item.ItemIndex
		Select Case e.CommandName
			Case "EditRec"
				If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then
					MarkLog(Util.Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				Dim mID As New Guid(e.Item.Cells(0).Text)
				mMELSnagCorrectiveAction = mLog.MELSnagCorrectiveActions(Index)
				mLog.MELSnagCorrectiveActions.CurrentIndex = Index
				Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
				Session("mLog") = mLog
				Session("Edit") = True
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " Defect No. : " + mLog.MELSnagCorrectiveActions(Index).DefectNo + " Date of occurrence : " + mLog.MELSnagCorrectiveActions(Index).DateOfOccurrenceFormatted + " Defect : " + mLog.MELSnagCorrectiveActions(Index).Defect
				MarkLog(Util.Action.Edit, "Log Defect Action", mLogDetail, Util.ErrorType.HandledError, mID, EventLogID)

				dgLogDefectActions.DataSource = mLog.MELSnagCorrectiveActions
				mMELSnagCorrectiveAction.LogNo = mLog.LogNoLogPageNo  'mLog.LogTextNo
				''Added By Prashant 22-Feb-2013 'All22022013-1
				If mMELSnagCorrectiveAction IsNot Nothing Then
					If IsDBNull(mMELSnagCorrectiveAction.RectifiedDate) = False Then
						'Commented and added by Saylee on 15-Dec-2014 as to fill combo from Log date to Recification date 
						'mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(mMELSnagCorrectiveAction.DateOfOccurrence.ToString, mMELSnagCorrectiveAction.RectifiedDate.ToString, mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "<SELECT>", True, mLog.ID.ToString)
						mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(Get_Whichever_LesserDate(mLog.Date.ToString, mMELSnagCorrectiveAction.DateOfOccurrence.ToString), "1/1/2100", mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "(SELECT)", True, mLog.ID.ToString, True)
						Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister
					End If
					mSubATAList = SubATAList.GetSubATAList(mMELSnagCorrectiveAction.ATAChapterID, , "(SELECT)") 'Added By Vikrant On 12-Apr-2013 For ALL01042013
				End If
				cmbRectifiedLogNo.DataSource = mRectifiedReportLogRegister
				cmbSubATAList.DataSource = mSubATAList  'Added By Vikrant On 12-Apr-2013 For ALL01042013

				mMELSnagPartList = MELSnagPartList.GePartList(mLog.Date.ToString, mLog.MachineID.ToString, "(SELECT)")
				cmbPartNo.Items.Clear()
				cmbPartNo.DataSource = mMELSnagPartList
				Session("mMELSnagPartList") = mMELSnagPartList
				If mMELSnagCorrectiveAction.IsMEL = True Then  'Added By Saylee On 13-May-2013 For ALL29042013
					'Added By Utkarsh on 15-Jul-2013 FOR ALL15072013-3
					If Not mMELSnagPartList.Contains(mMELSnagCorrectiveAction.PartNo) Then mMELSnagCorrectiveAction.PartID = Guid.Empty
					'End
					cmbMELCategory.Enabled = True
				Else
					cmbMELCategory.Enabled = False
				End If
				'MLNo
				BindLicenceNo()
				SetLicenceCount()
				'End
				DataBind()
				SetGrid()

				If mMELSnagCorrectiveAction.IsAttachmentAdded Then
					mFileAttach = FileAttach.GetAttachment(mMELSnagCorrectiveAction.ID) 'Sort = 1 - Installation
					Session("mFileAttach") = mFileAttach
				Else
					mFileAttach = FileAttach.NewAttachment(Guid.Empty, mMELSnagCorrectiveAction.ID)
					Session("mFileAttach") = mFileAttach
				End If

				ControlVisibilityAfterEdit()
				ControlVisibilityForAttachment()

				If mMELSnagCorrectiveAction.DueDate.ToString = "" Then
					txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
				Else
					txtDueDate.Text = mMELSnagCorrectiveAction.DateValue
				End If
				If mMELSnagCorrectiveAction.RectifiedDate.ToString = "" Then
					txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDate.ToString
				Else
					txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDateFormatted
				End If
				If mMELSnagCorrectiveAction.DateOfOccurrence.ToString = "" Then
					txtDateofoccurrence.Text = mMELSnagCorrectiveAction.DateOfOccurrence.ToString
				Else
					txtDateofoccurrence.Text = mMELSnagCorrectiveAction.DateOfOccurrenceFormatted
				End If

				chkShowMEL.Enabled = False

				cmbATAChapter.SelectedValue = mMELSnagCorrectiveAction.ATAChapterID.ToString
				cmbRectifiedLogNo.SelectedValue = mMELSnagCorrectiveAction.RectifiedLogID.ToString '---
				cmbSubATAList.SelectedValue = mMELSnagCorrectiveAction.SubATAID.ToString 'Added By Vikrant On 12-Apr-2013 For ALL01042013
				cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
				cmbAssembly.SelectedValue = mMELSnagCorrectiveAction.AssemblyStatusID.ToString 'Added By Vikrant On 02-Sept-2014 For All04092014

				upnlDetails.Update()
				upnlFileupload.Update()
				upnlMailTool.Update()
			Case "DeleteRec"
				If (Not User.IsInRole("LogDelete")) Then
					MarkLog(Util.Action.Delete, "Flight Log", User.Identity.Name & " is not Authorized User to delete " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					SetGrid()
					Exit Sub
				End If

				MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
				mLog.MELSnagCorrectiveActions.CurrentIndex = Index
				Session("mLog") = mLog
				Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
				'=====================Added By Saylee on 4thJuly 2007=================
			Case "ViewRec"
				If (Not User.IsInRole("LogView")) Then
					MarkLog(Util.Action.View, "Flight Log", User.Identity.Name & " is not Authorized User to view " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					SetGrid()
					Exit Sub
				End If

				'----------------------------------------------------------------------
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				'----------------------------------------------------------------------
				Dim mLogDefect As MELSnagCorrectiveAction
				Dim mID As New Guid(e.Item.Cells(0).Text)
				'mLogDefect = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mID, "")
				mLogDefect = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mID)

				Dim mFileAttach As FileAttach
				mFileAttach = FileAttach.GetAttachment(mLogDefect.ID)
				'Session("mFileAttach") = mFileAttach

				If mFileAttach.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						Dim Str As String
						Str = "openFile();"
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
					End If
				Else

				End If

				'=====================================================================
			Case "PrintRec"

				If (Not User.IsInRole("LogPrint")) Then
					MarkLog(Util.Action.Print, "Flight Log", User.Identity.Name & " is not Authorized User to print " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				Dim mID As New Guid(e.Item.Cells(0).Text)
				Dim Rpt As New crLogDefectActionList
				Dim ds As New dsMELSnagCorrectiveAction 'dsLogDefectActionList
				Dim da As New CSLA.Data.ObjectAdapter
				Dim mCompanyDetail As New CompanyDetail
				'rptLogDefectAction = LogDefectActionList.GetLogDefectAction(mID)
				Dim mrptMELSnagCorrectiveAction As rptMELSnagCorrectiveAction
				mrptMELSnagCorrectiveAction = rptMELSnagCorrectiveAction.GetrptMELSnagCorrectiveAction(mID.ToString)

				Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
						mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
						mCompanyDetail.WebSite, "PRELIMINARY DEFECT REPORT", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"))

				'da.Fill(ds, rptLogDefectAction)
				da.Fill(ds, mrptMELSnagCorrectiveAction)
				da.Fill(ds, Report)
				Rpt.SetDataSource(ds)
				Session("CrystalReport") = Rpt

				Dim Str As String
				Str = "openTranDetail();"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
			Case "CreateWORec"
				Dim mnWO As nWO
				Dim tmpAssemblyStatusList As AssemblyStatusList
				Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList


				mMELSnagCorrectiveAction = mLog.MELSnagCorrectiveActions(Index)


				'Added by Saylee on 2-Feb-2023
				Dim mnWOListForDueJobs As nWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(mMELSnagCorrectiveAction.ID)
				If mnWOListForDueJobs.Count > 0 Then
					mMELSnagCorrectiveAction.IsWOCreated = True
					mMELSnagCorrectiveAction.WONumber = mnWOListForDueJobs(0).WONumber & vbCrLf & mnWOListForDueJobs(0).WODateFormatted
					mMELSnagCorrectiveAction.WOID = mnWOListForDueJobs(0).ID
				End If

				If mMELSnagCorrectiveAction.IsWOCreated Then

					mnWO = nWO.GetWO(mMELSnagCorrectiveAction.WOID, False)
					Session("mnWO") = mnWO
					Session("IsShowAllWOs") = True

				Else

					mnWO = nWO.NewWO(TransTypeID:=Trans.WOCAMO)
					mnWO.WODate = mMELSnagCorrectiveAction.DateOfOccurrenceFormatted
					mnWO.MachineID = mLog.MachineID

					If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "ADeccan") Then
						Dim TempRegNo As String = ""
						TempRegNo = mLog.RegNo
						mnWO.WOText = Replace(TempRegNo, "VT-", "")
						If AppSettings("ClientCode") = "ADeccan" Then 'ADeccan Code Added by Saylee on 11-May-2018 for ADeccan11052018
							mnWO.WOText = mnWO.WOText + "/" + Today.Date.ToString("yy")
						End If
					ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
						mnWO.WOText = "MJO# " & CStr(CDate(txtDateofoccurrence.Text).Date.Year) & " - " & mnWO.ModelName
					ElseIf AppSettings("ClientCode") = "TP" Then
						mnWO.WOText = Replace(mLog.RegNo, "VT-", "") & "/" & CStr(CDate(txtDateofoccurrence.Text).Date.Year)
					End If


					tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtDateofoccurrence.Text.ToString, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
					AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList

					mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)

					mnWO.WOJobs.Add(mnWO.ID, 3)
					mnWO.WOJobs.CurrentItem.PreviousTransID = mMELSnagCorrectiveAction.ID

					mnWO.WOJobs.CurrentItem.DateOfOccurrence = mMELSnagCorrectiveAction.DateOfOccurrence
					mnWO.WOJobs.CurrentItem.MELCategoryID = mMELSnagCorrectiveAction.MELCategoryID

					mnWO.WOJobs.CurrentItem.ATAChapterID = mMELSnagCorrectiveAction.ATAChapterID
					mnWO.WOJobs.CurrentItem.IsUnderMEL = mMELSnagCorrectiveAction.IsMEL
					mnWO.WOJobs.CurrentItem.CompID = mMELSnagCorrectiveAction.PartID

					mnWO.WOJobs.CurrentItem.IsMajor = mMELSnagCorrectiveAction.IsMajor

					mnWO.WOJobs.CurrentItem.IsHours = mMELSnagCorrectiveAction.IsHours
					mnWO.WOJobs.CurrentItem.FrequencyInDays = mMELSnagCorrectiveAction.FrequencyInDays
					mnWO.WOJobs.CurrentItem.FrequencyInHours = mMELSnagCorrectiveAction.FrequencyInHours

					mnWO.WOJobs.CurrentItem.IsRepetitive = mMELSnagCorrectiveAction.IsRepetitive
					Dim Description As String = ""
					Description = mMELSnagCorrectiveAction.Description & "<BR>" & mMELSnagCorrectiveAction.LogNo & "<BR>" & mMELSnagCorrectiveAction.Defect & "<BR>" & "Date Of Occurence : " & mMELSnagCorrectiveAction.DateOfOccurrence

					'Component
					If mMELSnagCorrectiveAction.PartName <> "" Then Description = Description & "<BR>" & "On Part : " & mMELSnagCorrectiveAction.PartName

					'MEL Category
					If mMELSnagCorrectiveAction.MELCategoryName <> "" Then
						Description = Description & "<BR>" & IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Category : ", "MEL Category : ") & mMELSnagCorrectiveAction.MELCategoryName & "with "
						If mMELSnagCorrectiveAction.FrequencyInDays <> 0 Then
							Description = Description & mMELSnagCorrectiveAction.FrequencyInDays & " Days"
						Else
							Description = Description & mMELSnagCorrectiveAction.FrequencyInHours & " Hours"
						End If
					End If
					mnWO.WOJobs.CurrentItem.WOJobDescription = Description.Replace("<BR>", vbCrLf)
					mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = Trim(Description.Replace("<BR>", vbCrLf))
					mnWO.WOJobs.CurrentItem.DueAsOf = mMELSnagCorrectiveAction.DueDateFormatted.ToString

					If AppSettings("ShowCAMOOnlyForNewClients") = "True" Then
						mnWO.WOJobs.CurrentItem.TaskCardNo = mMELSnagCorrectiveAction.DefectNo
					End If
					Session("mnWO") = mnWO
					Session("LogFromMEL") = mLog
					Dim URLFromDueReportPreview As Stack = New Stack
					URLFromDueReportPreview.Push(Request.Url)
					Session("wfLogDefectActionList_Ajax") = "wfLogDefectActionList_Ajax"
					Session("URLFromDueReportPreview") = URLFromDueReportPreview
					''Response.Redirect("wfnWODetail_Ajax.aspx?BackPage=index.aspx")

				End If

				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenToAddWODetail", "OpenToAddWODetail();", True)
		End Select
	End Sub
	Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
		If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Save, "Log Defect Action", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Page.Validate()
		If Not IsValid Then
			upnlErrorList.Update()
			upnlErrors1.Update()
			upnlErrorList2.Update()
			Exit Sub
		End If

		If Not customvalidate1() Then upnlErrorList.Update() : upnlErrors1.Update() : Exit Sub
		If Not customvalidate2() Then upnlErrorList2.Update() : Exit Sub

		If Session("Edit") = False Then
			'Extra if Case ----Yogita Checked MarkDeleted bcaz "Rectification can be done on same or later the occurance Log(TLP)." MEL mark as deleted and number is not detting generated automatically 
			If (Session("mMELSnagCorrectiveAction") IsNot Nothing) AndAlso CType(Session("mMELSnagCorrectiveAction"), MELSnagCorrectiveAction).IsNew = True AndAlso CType(Session("mMELSnagCorrectiveAction"), MELSnagCorrectiveAction).IsDeleted = False Then
				mLog.MELSnagCorrectiveActions.add(Session("mMELSnagCorrectiveAction"))
			Else
				mLog.MELSnagCorrectiveActions.add(mLog.ID)
			End If

			For i As Integer = 0 To mLog.MELSnagCorrectiveActions.Count - 1
				mLog.MELSnagCorrectiveActions(i).SerialNo = i + 1
			Next
		End If

		SetObject()
		SetSession()

		If mLog.IsValid Then
			'Added by Utkarsh ON 27-Feb-2013 FOR All27022013
			If mLog.MELSnagCorrectiveActions.CurrentItem.InvestigationStatus Then
				If ((CDate(mLog.MELSnagCorrectiveActions.CurrentItem.DateOfOccurrence) <= CDate(mLog.MELSnagCorrectiveActions.CurrentItem.RectifiedDate)) AndAlso (mRectifiedReportLogRegister.Item(New Guid(cmbRectifiedLogNo.SelectedValue)).IntLogNo < mLog.LogNo)) Then
					'Commented by Yogita instead of Alert Box ..msgBox show
					'''''lblAlertTitle.Text = "Save Alert !"
					'''''lblAlertMessage.Text = "Rectification can be done on same or later the occurance Log(TLP)."
					' '' ''ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", "<script type='text/javascript'>OpenAlert();</script>")
					'''''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", "OpenAlert();", True)

					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, "Rectification can be done on same or later the occurance Log(TLP).", MsgBoxStyle.OkOnly, "")

					If Session("Edit") = False Then
						mLog.MELSnagCorrectiveActions.Remove(mLog.MELSnagCorrectiveActions.CurrentItem)
					End If
					Exit Sub

				Else

				End If
			End If
			'End

			If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
				If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
			   Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
					'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly)
					'''''msg1.ReplacePage = "wfLogDefectActionList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					'''''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction, MSGBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			End If
			'End
			''Added By Prashant 2-Jan-2014  --ALL02012014-1
			If (cmbATAChapter.SelectedIndex > 0 And chkIsRepetitive.Checked = False) Then
				Dim mMELSnagCountATAWise As MELSnagCountATAWise
				mMELSnagCountATAWise = MELSnagCountATAWise.GetMELSnagCountATAWise(mLog.MELSnagCorrectiveActions.CurrentItem.ATAChapterID.ToString, mLog.MELSnagCorrectiveActions.CurrentItem.MachineID.ToString, mLog.MELSnagCorrectiveActions.CurrentItem.ID.ToString, Val(AppSettings("MEL_Occurrance_In_Days")), mLog.MELSnagCorrectiveActions.CurrentItem.DateOfOccurrence, Val(AppSettings("MEL_Check_ON"))) 'Added config parameters by Saylee on  24-Feb-2020 for ALL24022020

				If mMELSnagCountATAWise.Item(0).MELSnagCount > 0 Then
					Dim MsgStr As String = String.Empty
					MsgStr = "There are " + mMELSnagCountATAWise.Item(0).MELSnagCount.ToString + IIf(AppSettings("MELSnagNomenclature") = "True", " ADD/Defect", " MEL/Snag") + " reported for this ATA. " + " Last Log Date is " + New SmartDate(mMELSnagCountATAWise.Item(0).LogInfo.ToString.Substring(0, mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(0).Trim.Length)).FormattedText + "<BR>" + " Log No. : " + mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(1).Trim + "<BR>" + " Log Page No. : " + mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(2).Trim + "<BR>" + " Do you want to make this " + IIf(AppSettings("MELSnagNomenclature") = "True", "Defect as Repetitive Defect", "Snag as Repetitive Snag") + "?" 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
					MSGBoxCtrl.show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, MsgStr, MsgBoxStyle.YesNo, "MELSnagCountATAWise")
					Exit Sub
				End If
			End If
			''Added By Prashant 2-Jan-2014  --ALL02012014-1
			mLog.Save()
			SaveAttachment()
			If Session("Edit") = True Then mLog = Log.GetLog(mLog.ID)
		Else
			Exit Sub
		End If

		NewMELSnagCorrectiveAction()
	End Sub
	'Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
	'    '----------------------------------------------------------------------
	'    Dim No As New Random
	'    Dim StrName As String = "abc" & No.Next.ToString
	'    '----------------------------------------------------------------------
	'    mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
	'    ' '' ''Dim mLogDefect As MELSnagCorrectiveAction
	'    ' '' ''mLogDefect = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mMELSnagCorrectiveAction.ID)
	'    If mMELSnagCorrectiveAction.ImageSize > 0 Then
	'        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mMELSnagCorrectiveAction.FileExtension
	'        Dim fs As FileStream
	'        If File.Exists(AppSettings("DOCPath")) = False Then
	'            'Delete File if exist
	'            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mMELSnagCorrectiveAction.FileExtension)
	'            ' Create the file.
	'            fs = File.Create(path)
	'            '' Add some information to the file.
	'            fs.Write(mMELSnagCorrectiveAction.ImageFile, 0, mMELSnagCorrectiveAction.ImageFile.Length)
	'            fs.Close()
	'            Session("DOCPath") = path
	'            Dim Str As String
	'            ' '' ''Str = "<script language=Javascript>openFile();</script>"
	'            ' '' ''ClientScript.RegisterStartupScript(Me.GetType(), "openFilel", Str)

	'            Str = "openFile();"
	'            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
	'        End If
	'    End If
	'End Sub
	'Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
	'    mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
	'    Dim fileSize1 As Integer = 0
	'    Dim file1(fileSize1) As Byte
	'    mMELSnagCorrectiveAction.ImageFile = file1
	'    mMELSnagCorrectiveAction.ImageSize = 0
	'    ImageButton2.Visible = False
	'    btnDelAttach.Enabled = False

	'End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click  'btnBack.Click
		MarkLog(Util.Action.Close, "Log Defect Action", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " Defect No. : " + txtDefectReportNo.Text.Trim + "-" + txtNo.Text + " Date of occurrence : " + New SmartDate(txtDateofoccurrence.Text.ToString).FormattedText
		MarkLog(Util.Action.Close, "Log Defect Action", mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
		Session.Remove("mMELSnagCorrectiveAction")
		Session.Remove("mFileAttach")
		Session.Remove("IsAttachmentDeleted")
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
		Session.Remove("wfLogDefectActionList_Ajax") 'Added by saylee 20-11-2023 
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End


		'Added By Utkarsh On 06-Apr-2012
		'If mLog.IsTLP = "True" Then
		If mMachine.IsTLP = "True" Then
			Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		Else
			'End
			'--------CHANGED BY VIKRANT---------------
			If AppSettings("LogDetailPage") = "NewPage" Then
				Response.Redirect("wfLogSOP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			Else
				Response.Redirect("wfLogDetail_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			End If
			'----------------------------------------
		End If
	End Sub
	Private Sub cmbPartNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPartNo.SelectedIndexChanged
		'If cmbPartNo.SelectedIndex <= 0 Then
		'    txtPartNo.Text = ""
		'    txtDescription.Text = ""
		'    'txtATAChapter.Text = ""
		'    cmbATAChapter.ClearSelection() 'cmbATAChapter.SelectedIndex = 0
		'    cmbATAChapter.Enabled = True
		'    txtSerialNo.Text = ""
		'    cmbMELCategory.ClearSelection()  'cmbMELCategory.SelectedIndex = 0
		'    txtFrequencyInDay.Text = "0"
		'    txtFrequencyInHours.Text = ""
		'    chkIsInHours.Checked = False

		'    txtFrequencyInDay.Enabled = False
		'    txtFrequencyInHours.Enabled = False
		'    chkIsInHours.Enabled = False

		'    txtFrequencyInHours.Text = ""
		'    txtFrequencyInDay.Text = "0"

		'    txtDueDate.Text = ""
		'    cmbSubATAList.ClearSelection() 'cmbSubATAList.SelectedIndex = 0

		'    txtDescription.ReadOnly = False
		'    txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)

		'    txtPartNo.ReadOnly = False
		'    txtPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

		'    txtSerialNo.ReadOnly = False
		'    txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

		'    cmbMELCategory.Enabled = True
		'    'cmbMELCategory.BackColor = Color.FromKnownColor(KnownColor.White)
		'Else
		'    Dim mMELPartList As MELPartList = MELPartList.GetMELPartList(mLog.Date.ToString, mLog.MachineID.ToString)
		'    If mMELPartList.Contains(mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Name, mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").CompID) Then
		'        txtFrequencyInDay.Enabled = True
		'        With mMELPartList(New Guid(cmbPartNo.SelectedValue.ToString), "")
		'            txtPartNo.Text = .Name
		'            'txtATAChapter.Text = .ATAChapter
		'            cmbATAChapter.SelectedValue = .CompStatusATAID.ToString
		'            txtDescription.Text = .Description
		'            cmbMELCategory.SelectedValue = .MELCategoryID
		'            txtFrequencyInDay.Text = .MELFrequencyInDays
		'            txtFrequencyInHours.Text = .MELFrequencyInHours
		'            chkIsInHours.Checked = .MELIsHours
		'            txtSerialNo.Text = .SerialNo
		'            cmbATAChapter.Enabled = False
		'        End With
		'        If mMELPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").MELCategoryID = 1 And mMELPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").MELIsHours = True Then
		'            chkIsInHours.Enabled = True
		'            txtFrequencyInHours.Enabled = True
		'            txtFrequencyInDay.Enabled = False
		'        Else
		'            txtFrequencyInDay.Enabled = True
		'            chkIsInHours.Enabled = False
		'            txtFrequencyInHours.Enabled = False
		'        End If
		'        If chkIsInHours.Checked = True Then
		'            mMELSnagCorrectiveAction.IsHours = True
		'            mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
		'            ' txtDueDate.Text = DateAdd(DateInterval.Minute, mMELSnagCorrectiveAction.FrequencyInHoursDec, CDate(txtDateofOccurrence.Text))
		'            txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.tostring
		'        Else
		'            mMELSnagCorrectiveAction.IsHours = False
		'            mMELSnagCorrectiveAction.FrequencyInDays = txtFrequencyInDay.Text
		'            mMELSnagCorrectiveAction.DateOfOccurrence = txtDateofoccurrence.Text
		'            txtDueDate.Text = mMELSnagCorrectiveAction.DateValue   'DateAdd(DateInterval.Day, Val(txtFrequencyInDay.Text), CDate(txtDateofoccurrence.Text))
		'        End If

		'    Else
		'        txtPartNo.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Name
		'        txtDescription.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Description
		'        txtSerialNo.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").SerialNo
		'        cmbMELCategory.ClearSelection() 'cmbMELCategory.SelectedIndex = 0
		'        txtFrequencyInDay.Text = "0"
		'        txtFrequencyInHours.Text = ""
		'        chkIsInHours.Checked = False
		'        txtFrequencyInDay.Enabled = False
		'        cmbATAChapter.Enabled = True
		'        cmbATAChapter.ClearSelection() 'cmbATAChapter.SelectedIndex = 0
		'        txtDueDate.Text = ""
		'    End If
		'    txtDescription.ReadOnly = True
		'    txtPartNo.ReadOnly = True
		'    txtSerialNo.ReadOnly = True
		'    cmbMELCategory.Enabled = False

		'    txtDescription.BackColor = Color.FromName("#E0E0E0")
		'    txtPartNo.BackColor = Color.FromName("#E0E0E0")
		'    txtSerialNo.BackColor = Color.FromName("#E0E0E0")
		'    'cmbMELCategory.BackColor = Color.FromName("#E0E0E0")
		'End If
		'Added By Vikrant On 03-Apr-2013 For ALL01042013
		'cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)

		'If cmbATAChapter.SelectedIndex > 0 Then
		'    mSubATAList = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "(SELECT)")
		'    cmbSubATAList.DataSource = mSubATAList
		'    cmbSubATAList.DataBind()
		'End If
		'Session("mSubATAList") = mSubATAList

		'End

		If cmbPartNo.SelectedIndex <= 0 Then
			txtPartNo.Text = ""
			txtDescription.Text = ""
			txtSerialNo.Text = ""
			txtDescription.ReadOnly = False
			txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)

			txtPartNo.ReadOnly = False
			txtPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtSerialNo.ReadOnly = False
			txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)
		Else
			txtPartNo.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Name
			txtDescription.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Description
			txtSerialNo.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").SerialNo

			'Added By Saylee on 8-Aug-2019
			If mAssemblylist.Count > 0 Then
				'cmbAssembly.SelectedValue = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").AssemblyStatusID.ToString
				'upnlMMELDetails.Update()
				If mAssemblylist.Contains(mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").AssemblyStatusID, "") Then
					cmbAssembly.SelectedValue = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").AssemblyStatusID.ToString
					upnlMMELDetails.Update()
				End If
			End If
			'***************************************

			txtDescription.ReadOnly = True
			txtPartNo.ReadOnly = True
			txtSerialNo.ReadOnly = True

			txtDescription.BackColor = Color.FromName("#E0E0E0")
			txtPartNo.BackColor = Color.FromName("#E0E0E0")
			txtSerialNo.BackColor = Color.FromName("#E0E0E0")
		End If

		If txtDescription.Text.Length > 30 Then
			txtDescription.TextMode = TextBoxMode.MultiLine
		Else
			txtDescription.TextMode = TextBoxMode.SingleLine
		End If
		If cmbPartNo.Enabled = True Then
			setFocus(cmbPartNo)
		End If
		upnlPartNo.Update()
		upnlDesc.Update()
		upnlSerialNo.Update()
		upnlATA.Update()
		upnlSubATA.Update()
		upnlMELCategory.Update()
		upnlFreq.Update()
		upnlDueDate.Update()
	End Sub
	'Private Sub txtDateofoccurrence_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDateofoccurrence.TextChanged
	'    mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtDateofoccurrence.Text, txtDateofoccurrence.Text, mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 1, , , , "<SELECT>", True)
	'    cmbLogNo.DataSource = mReportLogRegister
	'    Session("mReportLogRegister") = mReportLogRegister
	'    cmbLogNo.DataBind()
	'    If chkIsInHours.Checked = True Then
	'        mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
	'        txtDueDate.Value = DateAdd(DateInterval.Minute, mMELSnagCorrectiveAction.FrequencyInHoursDec, CDate(txtDateofoccurrence.Text.ToString))
	'    Else
	'        mMELSnagCorrectiveAction.FrequencyInDays = txtFrequencyInDay.Text
	'        mMELSnagCorrectiveAction.DateOfOccurrence = txtDateofoccurrence.Text.ToString
	'        txtDueDate.Value = mMELSnagCorrectiveAction.DateValue
	'    End If
	'End Sub

	Private Sub txtRectifiedDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRectifiedDate.TextChanged
		If IsDate(txtRectifiedDate.Text) Or (txtRectifiedDate.Text = "") Then

			cmbRectifiedLogNo.Enabled = True

			'Commented by Saylee on 15-Dec-2014 as to fill combo from Log date to Recification date 
			'mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(txtDateofoccurrence.Text, txtRectifiedDate.Text, mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "<SELECT>", True, mLog.ID.ToString)
			'mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(mLog.Date.ToString, txtRectifiedDate.Text, mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "<SELECT>", True, mLog.ID.ToString)
			mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(Get_Whichever_LesserDate(mLog.Date.ToString, txtDateofoccurrence.Text), "1/1/2100", mTempAssemblyList(0).ID.ToString, mLog.MachineID.ToString, False, , 0, , , , "(SELECT)", True, mLog.ID.ToString, True)

			cmbRectifiedLogNo.DataSource = mRectifiedReportLogRegister
			cmbRectifiedLogNo.DataBind()

			If txtRectifiedDate.Text = "" Then
				mMELSnagCorrectiveAction.RectifiedDate = System.DBNull.Value
			Else
				mMELSnagCorrectiveAction.RectifiedDate = txtRectifiedDate.Text
			End If

			If mMELSnagCorrectiveAction.RectifiedDate.ToString = "" Then
				txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDate.ToString
			Else
				txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDateFormatted
			End If

			Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister

		Else
			If mMELSnagCorrectiveAction.RectifiedDate.ToString = "" Then
				txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDate.ToString
			Else
				txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDateFormatted
			End If

		End If
	End Sub
	'Private Sub cmbLogNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'    If cmbLogNo.SelectedIndex > 0 Then
	'        'lnkCheckStatus.Enabled = True
	'        mMELSnagCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(cmbLogNo.SelectedValue.ToString)
	'        Session("mMELSnagCorrectiveActionLog") = mMELSnagCorrectiveActionLog
	'        With mMELSnagCorrectiveActionLog
	'            txtSector.Text = mMELSnagCorrectiveActionLog.Item(0).SourceName
	'            txtLastMajorCheck.Text = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H" + ", " + mMELSnagCorrectiveActionLog.Item(0).FinalLandings + " L"
	'        End With
	'    End If
	'    If cmbLogNo.Enabled = True Then
	'        setFocus(cmbLogNo)
	'    End If
	'End Sub
	Private Sub cmbRectifiedLogNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRectifiedLogNo.SelectedIndexChanged
		If cmbRectifiedLogNo.SelectedIndex > 0 Then
			mMELSnagCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(cmbRectifiedLogNo.SelectedValue.ToString)
			Session("mMELSnagCorrectiveActionLog") = mMELSnagCorrectiveActionLog
			With mMELSnagCorrectiveActionLog
				txtRectificationSector.Text = mMELSnagCorrectiveActionLog.Item(0).DestinationName
			End With
		Else
			txtRectificationSector.Text = ""
		End If

		If cmbRectifiedLogNo.Enabled = True Then
			setFocus(cmbRectifiedLogNo)
		End If

	End Sub
	Private Sub chkClose_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkClose.CheckedChanged
		If chkClose.Checked = True Then
			txtRectifiedDate.ReadOnly = False
			'  plhRectification.Visible = True
		Else
			txtRectifiedDate.Text = ""
			txtRectifiedDate.ReadOnly = True
			cmbRectifiedLogNo.ClearSelection() 'cmbRectifiedLogNo.SelectedIndex = 0
			cmbRectifiedLogNo.Enabled = False
			txtRectificationSector.Text = ""
			mMELSnagCorrectiveAction.RectifiedLogID = Guid.Empty
			' plhRectification.Visible = False
		End If
		upnlRectifiedDate.Update()
		upnlRectifiedCombo.Update()
	End Sub
	' '' ''Private Sub txtDateofoccurrence_CalendarVisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDateofoccurrence.CalendarVisibleChanged
	' '' ''    'cmbLogNo.Visible = Not CType(sender, Boolean)
	' '' ''    cmbPartNo.Visible = Not CType(sender, Boolean)

	' '' ''    upnlDetails.Update()

	' '' ''End Sub
	Private Sub txtFrequencyInDay_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFrequencyInDay.TextChanged
		If chkIsInHours.Checked = True Then
			txtFrequencyInDay.Text = ""
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = True
		Else
			txtFrequencyInHours.Text = ""
			txtFrequencyInDay.Enabled = True
			txtFrequencyInHours.Enabled = False
		End If
		'If chkIsInHours.Checked = True Then
		'    txtDueDate.Text = Format(CDate(DateAdd(DateInterval.Minute, mMELSnagCorrectiveAction.FrequencyInHoursDec, CDate(txtDateofoccurrence.Text.ToString)).ToString), AppSettings("DateFormat"))

		'Else
		'    'mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
		'    mMELSnagCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text.Trim)  'Changed By Utkarsh on 13-Aug-2013 for ALL13082013-2
		'    txtDueDate.Text = Format(CDate(mMELSnagCorrectiveAction.DateValue.ToString), AppSettings("DateFormat"))
		'End If
		mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked

		If chkIsInHours.Checked = True Then
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text.Trim
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString 'DateAdd(DateInterval.Minute, mMELSnagCorrectiveAction.FrequencyInHoursDec, CDate(txtDateofOccurrence.Text))
		Else
			'mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			mMELSnagCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text.Trim)  'Changed By Utkarsh on 13-Aug-2013 for ALL13082013-2
			''txtDueDate.Text = mMELSnagCorrectiveAction.DateValue 'DateAdd(DateInterval.Day, Val(txtFrequencyInDay.Text), CDate(txtDateofoccurrence.Text))
			If chkShowMEL.Checked = True And (txtFrequencyInDay.Text <> "0") Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			Else
				txtDueDate.Text = ""
			End If
		End If
		upnlDueDate.Update()
	End Sub
	Private Sub txtFrequencyInHours_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFrequencyInHours.TextChanged
		If chkIsInHours.Checked = True Then
			txtFrequencyInDay.Text = "0"
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = True
		Else
			txtFrequencyInHours.Text = ""
			txtFrequencyInDay.Enabled = True
			txtFrequencyInHours.Enabled = False
		End If
		mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked
		If chkIsInHours.Checked = True Then
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			'txtDueDate.Text = DateAdd(DateInterval.Minute, mMELSnagCorrectiveAction.FrequencyInHoursDec, CDate(txtDateofoccurrence.Text))
			'txtDueDate.Text = mMELSnagCorrectiveAction.DateValue
			If chkShowMEL.Checked = True And (txtFrequencyInHours.Text <> "") Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			Else
				txtDueDate.Text = ""
			End If

		Else
			txtDueDate.Text = DateAdd(DateInterval.Day, Val(txtFrequencyInDay.Text), CDate(txtDateofoccurrence.Text))
		End If
		upnlDueDate.Update()
	End Sub
	Private Sub chkIsInHours_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsInHours.CheckedChanged
		If chkIsInHours.Checked = True Then
			txtFrequencyInDay.Text = ""
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = True
		Else
			txtFrequencyInHours.Text = ""
			txtFrequencyInDay.Text = mMELSnagCorrectiveAction.FrequencyInDays
			txtFrequencyInDay.Enabled = True
			txtFrequencyInHours.Enabled = False
		End If
		mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked


		If chkIsInHours.Checked = True Then
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		Else
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString 'DateAdd(DateInterval.Day, Val(txtFrequencyInDay.Text), CDate(txtDateofoccurrence.Text))
		End If
		upnlDueDate.Update()
	End Sub
	Private Sub cmbMELCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMELCategory.SelectedIndexChanged

		If cmbMELCategory.SelectedIndex > 0 Then mMELSnagCorrectiveAction.MELCategoryID = cmbMELCategory.SelectedValue
		txtFrequencyInDay.Text = mMELSnagCorrectiveAction.FrequencyInDays
		mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked

		If cmbMELCategory.SelectedIndex = 1 Then
			chkIsInHours.Enabled = True
			txtFrequencyInDay.Enabled = True
			If chkIsInHours.Checked = True Then
				mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
				If txtFrequencyInHours.Text <> "" Then
					txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
				Else
					txtDueDate.Text = ""
				End If
			Else
				mMELSnagCorrectiveAction.FrequencyInDays = txtFrequencyInDay.Text

				If txtFrequencyInDay.Text <> "" Then
					txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
				Else
					txtDueDate.Text = ""
				End If
			End If


		ElseIf cmbMELCategory.SelectedIndex = 0 Then
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = False
			chkIsInHours.Enabled = False

			txtFrequencyInHours.Text = ""
			txtFrequencyInDay.Text = "0"
			txtDueDate.Text = ""
			If chkIsInHours.Checked = True Then
				chkIsInHours.Checked = False
			End If
			mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			mMELSnagCorrectiveAction.FrequencyInDays = txtFrequencyInDay.Text
		Else
			If chkIsInHours.Checked = True Then
				chkIsInHours.Checked = False
			End If
			chkIsInHours.Enabled = False
			txtFrequencyInDay.Enabled = True

			If txtFrequencyInHours.Text <> "" Then
				txtFrequencyInHours.Text = ""
			End If
			txtFrequencyInHours.Enabled = False

			mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			mMELSnagCorrectiveAction.FrequencyInDays = txtFrequencyInDay.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString

		End If
		upnlFreq.Update()
		upnlDueDate.Update()
	End Sub

	'Private Sub lnkCheckStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'    SetObject()
	'    Session("mDateofoccurrence") = mMELSnagCorrectiveAction.DateOfOccurence
	'    Session("mlnkCheckStatus") = True
	'    If cmbLogNo.SelectedIndex > 0 Then
	'        Session("mTempLogID") = cmbLogNo.SelectedValue.ToString
	'        'Response.Redirect("wfAuditor.aspx?BackPage1=wfAuditExecution.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
	'        Response.Redirect("wfMELSnagCorrectiveActionLogInfo.aspx?BackPage1=wfMELSnagCorrectiveActionNew.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
	'    End If
	'End Sub
	'Private Sub chkShowMEL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowMEL.CheckedChanged
	'    cmbPartNo.Items.Clear()
	'    If chkShowMEL.Checked = True Then
	'        mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateofoccurrence.Text, mLog.MachineID.ToString, "(SELECT)")
	'        cmbPartNo.DataSource = mMELSnagPartList
	'        Session("mMELSnagPartList") = mMELSnagPartList
	'        If Not mMELSnagPartList.Contains(mMELSnagCorrectiveAction.PartNo) Then mMELSnagCorrectiveAction.PartID = Guid.Empty
	'        'Commented By Utkarsh on 15-Jul-2013 FOR ALL15072013-3
	'        'mbPartNo.SelectedIndex = 0
	'        cmbPartNo.DataBind()
	'        cmbATAChapter.SelectedIndex = 0
	'        ''cmbATAChapter.Enabled = False
	'        txtDueDate.Text = ""

	'        cmbMELCategory.SelectedIndex = 0
	'        cmbMELCategory.Enabled = True
	'        'cmbMELCategory.BackColor = Color.FromKnownColor(KnownColor.White)
	'    Else
	'        mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateofoccurrence.Text, , "(SELECT)")
	'        cmbPartNo.DataSource = mMELSnagPartList
	'        Session("mMELSnagPartList") = mMELSnagPartList
	'        cmbPartNo.DataBind()
	'        If Not mMELSnagPartList Is Nothing Then cmbPartNo.SelectedIndex = 0
	'        cmbATAChapter.SelectedIndex = 0
	'        cmbATAChapter.Enabled = True
	'        txtDueDate.Text = ""

	'        cmbMELCategory.SelectedIndex = 0
	'        cmbMELCategory.Enabled = False

	'        txtPartNo.Text = ""
	'        txtDescription.Text = ""
	'        'txtATAChapter.Text = ""
	'        cmbATAChapter.SelectedIndex = 0
	'        txtSerialNo.Text = ""
	'        cmbMELCategory.SelectedIndex = 0
	'        txtFrequencyInDay.Text = "0"
	'        txtFrequencyInHours.Text = ""
	'        chkIsInHours.Checked = False

	'    End If
	'    'Added By Vikrant On 03-Apr-2013 For ALL01042013
	'    cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)

	'    cmbSubATAList.DataSource = mSubATAList
	'    cmbSubATAList.DataBind()
	'    If cmbATAChapter.SelectedIndex > 0 Then cmbSubATAList.SelectedIndex = 0
	'    'End
	'    Call cmbMELCategory_SelectedIndexChanged(sender, e)

	'    txtPartNo.Text = ""
	'    txtDescription.Text = ""
	'    cmbMELCategory.SelectedIndex = 0
	'    txtFrequencyInDay.Text = "0"
	'    txtFrequencyInHours.Text = ""
	'    txtSerialNo.Text = ""
	'    Session("ShowMEL") = chkShowMEL.Checked

	'    txtDescription.ReadOnly = False
	'    txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)

	'    txtPartNo.ReadOnly = False
	'    txtPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

	'    txtSerialNo.ReadOnly = False
	'    txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)
	'    upnlPartNo.Update()
	'    upnlDesc.Update()
	'    upnlSerialNo.Update()
	'    upnlATA.Update()
	'    upnlSubATA.Update()
	'    upnlMELCategory.Update()
	'    upnlFreq.Update()
	'    upnlDueDate.Update()
	'End Sub
	Private Sub chkShowMEL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowMEL.CheckedChanged
		If chkShowMEL.Checked Then
			Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMELMasterWindow", "OpenMELMasterWindow();", True)
		Else

			'mMELSnagCorrectiveAction.MELID = Guid.Empty
			'mMELSnagCorrectiveAction.MELCategoryID = 0
			'mMELSnagCorrectiveAction.MELCategoryName = ""
			'mMELSnagCorrectiveAction.Defect = ""
			'mMELSnagCorrectiveAction.ATAChapterID = Guid.Empty
			'mMELSnagCorrectiveAction.SubATAID = Guid.Empty
			'mMELSnagCorrectiveAction.FrequencyInDays = 0
			'mMELSnagCorrectiveAction.FrequencyInHours = ""
			'mMELSnagCorrectiveAction.IsHours = False

			cmbMELCategory.SelectedIndex = 0

			txtDefect.Text = ""
			cmbATAChapter.SelectedIndex = 0

			cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)

			mSubATAList = SubATAList.GetSubATAList(mMELSnagCorrectiveAction.ATAChapterID, "", "(SELECT)")
			cmbSubATAList.DataSource = mSubATAList
			cmbSubATAList.DataBind()
			Session("mSubATAList") = mSubATAList
			upnlSubATA.Update()


			txtFrequencyInDay.Text = 0
			txtFrequencyInHours.Text = ""
			chkIsInHours.Checked = False
			'txtDueDate.Text = mMELSnagCorrectiveAction.DateValue
			txtDueDate.Text = ""

			chkExtensionApplied.Checked = False
			txtExtensionInDays.Text = 0
			txtExtensionApprovalNo.Text = ""

			ControlVisibilityAfterEdit()
			upnlMMELDetails.Update()
		End If

	End Sub
	'Private Sub txtDateofoccurrence_CalendarVisibleChanged1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDateofoccurrence.TextChanged
	'    Me.cmbATAChapter.Visible = Not CType(sender, Boolean)
	'    Me.cmbPartNo.Visible = Not CType(sender, Boolean)

	'End Sub
	'Private Sub txtDueDate_CalendarVisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDueDate.TextChanged

	'    'Changed by Yogita for Wrong Date Format
	'    If IsDate(txtDueDate.Text) Or (txtDueDate.Text = "") Then
	'        If txtDueDate.Text = "" Then
	'            mMELSnagCorrectiveAction.DueDate = System.DBNull.Value
	'            txtRectifiedDate.Text = mMELSnagCorrectiveAction.DueDate.ToString
	'        Else
	'            mMELSnagCorrectiveAction.DueDate = txtDueDate.Text
	'            txtRectifiedDate.Text = mMELSnagCorrectiveAction.DueDateFormatted
	'        End If
	'    Else
	'        txtDueDate.Text = ""
	'    End If
	'End Sub
	Private Sub txtDateofoccurrence_TextChanged(sender As Object, e As System.EventArgs) Handles txtDateofoccurrence.TextChanged
		Session("mDateofoccurrence") = txtDateofoccurrence.Text

		'Here if New then consider Occurrance Date for binding else
		'if Old: then get Lesser date from LogDate or Occurrance date


		If chkIsInHours.Checked = True Then
			mMELSnagCorrectiveAction.DateOfOccurrence = txtDateofoccurrence.Text
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString 'DateAdd(DateInterval.Minute, mMELSnagCorrectiveAction.FrequencyInHoursDec, CDate(txtDateofOccurrence.Text))
		Else
			mMELSnagCorrectiveAction.DateOfOccurrence = txtDateofoccurrence.Text
			mMELSnagCorrectiveAction.FrequencyInDays = txtFrequencyInDay.Text
			If chkShowMEL.Checked = True And (txtFrequencyInDay.Text <> "0") Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			ElseIf chkShowMEL.Checked = True And cmbMELCategory.SelectedIndex = 1 Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			Else
				txtDueDate.Text = ""
			End If
		End If
		'Added By Vikrant On 02-Sept-2014 For All04092014
		mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, mLog.MachineID.ToString, txtDateofoccurrence.Text, "", True)
		cmbAssembly.DataSource = mAssemblylist
		Session("mAssemblylist") = mAssemblylist
		cmbAssembly.DataBind()
		'End
		cmbRectifiedLogNo.DataSource = mRectifiedReportLogRegister
		Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister
		cmbRectifiedLogNo.DataBind()

		'Added by Saylee on 25-Nov-2014 to reset rectification details on date change
		chkClose.Checked = False
		txtRectifiedDate.Text = ""
		txtRectifiedDate.ReadOnly = True
		If mRectifiedReportLogRegister IsNot Nothing Then cmbRectifiedLogNo.ClearSelection() 'cmbRectifiedLogNo.SelectedIndex = 0
		cmbRectifiedLogNo.Enabled = False
		txtRectificationSector.Text = ""
		mMELSnagCorrectiveAction.RectifiedLogID = Guid.Empty

		upnlClose.Update()
		upnlRectifiedDate.Update()
		upnlRectifiedCombo.Update()
		upnlDueDate.Update()
	End Sub
	'Added By Vikrant On 02-Apr-2013 For ALL01042013
	Private Sub cmbATAChapter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbATAChapter.SelectedIndexChanged
		mMELSnagCorrectiveAction.SubATAID = Guid.Empty
		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction

		mSubATAList = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "(SELECT)")

		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
		cmbSubATAList.DataSource = mSubATAList
		cmbSubATAList.DataBind()

		Session("mSubATAList") = mSubATAList
		upnlSubATA.Update()
	End Sub
	'End
	'Added by Utkarsh On 06-Apr-2012
	Private Sub btnLogDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogDetails.Click
		Session.Remove("mMELSnagCorrectiveAction")
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
		'Added By Utkarsh On 06-Apr-2012
		If mMachine.IsTLP = "True" Then
			Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		Else
			'End
			'--------CHANGED BY VIKRANT---------------
			If AppSettings("LogDetailPage") = "NewPage" Then
				Response.Redirect("wfLogSOP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			Else
				Response.Redirect("wfLogDetail_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			End If
			'----------------------------------------
		End If
	End Sub
	Private Sub btnFuelOil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFuelOil.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
		Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnParameterList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParameterList.Click
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
		Response.Redirect("wfLogParameterList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnLogPax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogPax.Click
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
		NewLogPax()
		'Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogParameterList_Ajax.aspx")
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnHobbsOffset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHobbsOffset.Click
		NewHobbsOffSet()
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
		Response.Redirect("wfHobbsOffset_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogParameterList_Ajax.aspx")
	End Sub

	Private Sub btnFlightCrew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFlightCrew.Click
		Session.Remove("mMELSnagCorrectiveAction")
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
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
		Session.Remove("mMELSnagCorrectiveAction")
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
		''Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
		'If AppSettings("LogDetailPage") = "NewPage" Then
		'    Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx")
		'ElseIf mLog.IsTLP = True Then
		'    Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLP_Ajax.aspx")
		'End If
		''End
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	'End
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	Private Sub ImageButton2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
		ViewImage()
	End Sub
	Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
		ControlVisibilityForAttachment()
		upnlFileupload.Update()
	End Sub
	Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte

		mFileAttach.ImageFile = file1
		mFileAttach.Size = 0

		ImageButton2.Visible = False
		btnDelAttach.Enabled = False
		IsAttachmentDeleted = True
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
	End Sub
	Private Sub lnlMELDetail_Click(sender As Object, e As System.EventArgs) Handles lnlMELDetail.Click
		Dim mMEL As MEL
		mMEL = MEL.GetMEL(mMELSnagCorrectiveAction.MELID)
		mMEL.MarkClean()
		Session("mMEL") = mMEL
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMELDetail", "OpenMELDetail();", True)
	End Sub
	Private Sub chkExtensionApplied_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkExtensionApplied.CheckedChanged
		If chkExtensionApplied.Checked = True Then
			txtExtensionInDays.Enabled = True
			txtExtensionApprovalNo.Enabled = True
		Else
			txtExtensionInDays.Enabled = False
			txtExtensionApprovalNo.Enabled = False
			txtExtensionInDays.Text = 0
			txtExtensionApprovalNo.Text = ""
			mMELSnagCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		End If
		upnlExtension.Update()
	End Sub
	Private Sub txtExtensionInDays_TextChanged(sender As Object, e As System.EventArgs) Handles txtExtensionInDays.TextChanged
		If chkExtensionApplied.Checked = True Then
			mMELSnagCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
			If chkShowMEL.Checked = True Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			Else
				txtDueDate.Text = ""
			End If
		Else
			txtExtensionInDays.Enabled = False
			txtExtensionApprovalNo.Enabled = False
			txtExtensionInDays.Text = 0
			txtExtensionApprovalNo.Text = ""
			mMELSnagCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		End If
		upnlExtension.Update()
	End Sub
	Public Sub SetUserMailIDs()
		Session("UserEmailID") = mModuleList.Item("MELSnagCorrectiveAction").SendToMailID
		Session("UserCcEmailID") = mModuleList.Item("MELSnagCorrectiveAction").SendCCMailID
		Session("MailsRequire") = mModuleList.Item("MELSnagCorrectiveAction").MailsRequire
		Session("SmtpHost") = mModuleList.Item("MELSnagCorrectiveAction").SmtpHost
		Session("SmtpPort") = mModuleList.Item("MELSnagCorrectiveAction").SmtpPort
		Session("SmtpUser") = mModuleList.Item("MELSnagCorrectiveAction").SmtpUser
		Session("SmtpPassword") = mModuleList.Item("MELSnagCorrectiveAction").SmtpPassword
	End Sub
	Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
		Dim Str As String

		SetUserMailIDs()

		Session("btnSendMail") = "btnSendMail"
		Str = "OpenByMaiWindow();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
	End Sub
	Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
		Try
			Dim str As String
			Dim mSendMailFile As New SendMailFile


			str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following New " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect", "MEL/Snag") + " has been added in FlyPal System and need your attention." + "</font></P></br> ") 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
			str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")

			str = str + ("<p><font face=""Calibri"">")
			str = str + ("<b> Aircraft :</b>" + mMELSnagCorrectiveAction.RegNo + "<b>" + " Log No :" + "</b>" + mMELSnagCorrectiveAction.LogNo)
			str = str + ("</font></p>")


			str = str + ("<p><font face=""Calibri"">")
			str = str + ("<b>Defect No. :" + "</b>" + mMELSnagCorrectiveAction.DefectNo + "<b> Date of Occurrence :</b>" + mMELSnagCorrectiveAction.DateOfOccurrenceFormatted + "<b>" + " Defect :" + "</b>" + mMELSnagCorrectiveAction.Defect)
			str = str + ("</font></p>")



			str = str + ("<p><font face=""Calibri"">")
			str = str + "<b>" + " Name of Pilot/AME & License No./Observed By :" + "</b>" + mMELSnagCorrectiveAction.ReportedBy
			str = str + ("</font></p>")


			str = str + ("</body></html>")

			SendMailFile.SendMailFile(, System.Threading.Thread.CurrentPrincipal.Identity.Name, IIf(AppSettings("MELSnagNomenclature") = "True", "New ADD/Defect Notification", "New MEL/Snag Notification"), , str,
									"", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
									 SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword")) 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020

			Dim mDirectiveDetail As String = "New " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect", "MEL/Snag") + " Notification sent successfully to " + Session("ToSendMailIDs") + " by " + User.Identity.Name
			MarkLog(Util.Action.SendMail, "Log Defect Action", mDirectiveDetail, Util.ErrorType.HandledError, mMELSnagCorrectiveAction.ID, EventLogID)
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True) 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020


		Catch ex As Exception
			Dim Day, Month, Year As String
			Day = Format(Today.Date.Day, "0#")
			Month = Format(Today.Date.Month, "0#")
			Year = Format(Today.Date.Year, "0#")
			Dim todaydate As String = Day & Month & Year
			Dim Path As String = AppSettings("DOCPath") & todaydate
			FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
			FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
			FileClose(1)
		Finally
			Session.Remove("mModelMonitorModtmp")
		End Try

	End Sub
	Private Sub hdnBtnAddWODetail_Click(sender As Object, e As System.EventArgs) Handles hdnBtnAddWODetail.Click
		If Session("LogFromMEL") IsNot Nothing Then 'Used when WO Created from Grid and returned on this page
			mLog = CType(Session("LogFromMEL"), Log)
			Session("mLog") = mLog
			Session.Remove("LogFromMEL")
		End If
		DataFieldBind()
	End Sub
#End Region

#Region "Service Methods"
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetLicenceList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		'Dim itemlist As ItemListAutoComplete
		'itemlist = ItemListAutoComplete.GetItemList(prefixText, False)

		Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, "", , , True)
		If count = 0 Then
			Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
		Else
			Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
		End If
	End Function
#End Region

End Class
