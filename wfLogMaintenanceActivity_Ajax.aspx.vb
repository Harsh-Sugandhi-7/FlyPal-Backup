Imports System.Linq


Partial Class wfLogMaintenanceActivity_Ajax
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	'Added on 29-05-2007 by Saylee

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
	Private mLicenseNoListWithEmployee As LicenseNoListWithEmployee 'Added by Saylee on 07-Nov-2014 for ALL07112014
	Private mLogMaintenance As LogMaintenance
	Public mLog As Log
	Dim EventLogID As Guid

	Private Flag As Int16 'Added by Saylee on 10-May-2012

	Dim mLogDetail As String 'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
	Dim mEmployeeStatus As EmployeeStatus
	Dim mAssemblylist As AssemblyList 'Added By Vikrant On 02-Sept-2014 For All04092014
	'MLNo
	Dim LicenseNo As String = String.Empty
	Dim EmpName As String = String.Empty
	Dim DoneByID As Guid = Guid.Empty
	Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
	Shared UserNameForLicenceList As String
	'End
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mLogMaintenance = Session("mLogMaintenance")
		mLog = CType(Session("mLog"), Log)
		mAssemblylist = Session("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
		mLicenseNoListWithEmployee = Session("mLicenseNoListWithEmployee") 'Added by Saylee on 07-Nov-2014 for ALL07112014
		'MLNo
		mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
		UserNameForLicenceList = Session("UserNameForLicenceList")
		'End
	End Sub
	Private Sub SetSession()
		Session("mLogMaintenance") = mLogMaintenance
		Session("mLog") = mLog
		Session("mLicenseNoListWithEmployee") = mLicenseNoListWithEmployee 'Added by Saylee on 07-Nov-2014 for ALL07112014
	End Sub
	Private Sub RemoveSession()
		Session.Remove("Edit")
		Session.Remove("mLogMaintenance")
		Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
		Session.Remove("LogMaintenanceEdit")
		Session.Remove("OpenFromLMA")
		'MLNo
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		'End
	End Sub
	'MLNo
	Public Sub SetLicenceCount(ByVal LogMaint As LogMaintenance)
		If LogMaint.MaintenanceDoneByEmployees.Count > 1 Then
			lblLicenceCount.Text = "and " + (LogMaint.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
		End If
		lblLicenceCount.DataBind()
		'lblAllLicenceNos.DataBind()
	End Sub
	Private Sub BindLicenceNo(ByVal LogMaint As LogMaintenance)
		If LogMaint.MaintenanceDoneByEmployees.Count > 0 Then
			txtLicenceNo.Text = LogMaint.MaintenanceDoneByEmployees(0).LicenceNo + " [" + LogMaint.MaintenanceDoneByEmployees(0).EmployeeName + "]"
		Else
			txtLicenceNo.Text = String.Empty
		End If
		lblLicenceCount.ToolTip = LogMaint.AllLicenceNosWithEmpName
	End Sub
	'End
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
	Private Sub DeleteRecord(ByVal Index As Integer)
		MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
		mLog.LogMaintenances.CurrentIndex = Index
		Session("mLog") = mLog
	End Sub
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)  'Added by Saylee on 10-May-2012
		If Flag = 1 Then Exit Sub
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)

		Dim str As String = ""
		'Log
		If Not mLog.IsValid Then
			For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
				str = str + mLog.GetBrokenRulesCollection(i).Description + "<BR>"
			Next
		End If
		'Added by Saylee on 07-Nov-2014 for ALL07112014
		'Dim mEmployeeID As String
		'mEmployeeID = IIf(EmployeeID.Value.Length > 0, EmployeeID.Value, Guid.Empty.ToString)
		'******

		'Log Maintenance Activity
		If txtMainActivity.Text.Length > 2000 And custValidator.ControlToValidate = "txtMainActivity" Then
			'txtMainActivity.Text = txtMainActivity.Text.Substring(0, 996) + "..."
			custValidator.ErrorMessage = "Activity  must not be greater than 2000 characters."
			e.IsValid = False
		ElseIf txtNCRNo.Text.Length > 50 And custValidator.ControlToValidate = "txtNCRNo" Then
			custValidator.ErrorMessage = "NRC/WO No. must not be greater than 50 characters."
			e.IsValid = False
		ElseIf txtPlace.Text.Length > 50 And custValidator.ControlToValidate = "txtPlace" Then
			custValidator.ErrorMessage = "Place must not be greater than 50 characters."
			e.IsValid = False
			'ElseIf txtLicenceNo.Text <> "" And New Guid(mEmployeeID).Equals(Guid.Empty) And cvDoneBy.ControlToValidate = "txtLicenceNo" Then 'Added by Saylee on 07-Nov-2014 for ALL07112014

			'    custValidator.ErrorMessage = "Please select proper Done By."
			'    e.IsValid = False
		End If

		If str <> "" Then
			custValidator.ErrorMessage = str
			e.IsValid = False
		End If
		Flag = 1
	End Sub
	'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
	Private Function customvalidate1() As Boolean
		Dim str As String = ""
		'Log
		If Not mLog.IsValid Then
			For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
				str = str + mLog.GetBrokenRulesCollection(i).Description + "<BR>"
			Next
		End If
		'Log Maintenances
		For i As Integer = 0 To mLog.LogMaintenances.Count - 1
			If Not mLog.LogMaintenances(i).IsValid Then
				For j As Integer = 0 To mLog.LogMaintenances(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.LogMaintenances.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
				Next
			End If
		Next
		If str <> "" Then
			cvMainActivityList.ErrorMessage = str
			cvMainActivityList.IsValid = False
			Return False
		End If
		Return True
	End Function
	'end

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
					' '' ''If CType(Session("sender"), String) = "Delete" Then
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							' '' ''Session("sender") = ""
							mLog.LogMaintenances.Remove(mLog.LogMaintenances(mLog.LogMaintenances.CurrentIndex))
							For i As Integer = 0 To mLog.LogMaintenances.Count - 1
								mLog.LogMaintenances(i).SrNo = i + 1
							Next
							Session("mLog") = mLog
							Session("LogMaintenanceEdit") = False 'Changed by By Utkarsh on 13-Aug-2013 for ALL13082013-2
							mLogMaintenance = Nothing
							DataFieldBind()
							ImageButton2.Visible = False
							btnDelAttach.Enabled = False
							ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallautoResize", "CallautoResize();", True)
							'''''Response.Redirect("wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
						Catch ex As SqlException
							If ex.Number = 8145 Then
								'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly)
								'''''msg1.ReplacePage = "wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
								'''''msg1.Show()

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")

							ElseIf ex.Number = 2627 Then
								'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly)
								'''''msg1.ReplacePage = "wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
								'''''msg1.Show()

								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")

							ElseIf ex.Number = 547 Then
								'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly)
								'''''msg1.ReplacePage = "wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
								'''''msg1.Show()

								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")

								MarkLog(Util.Action.Delete, "Log Maintenance Activity List", "Can't delete : This is Currently in use", Util.ErrorType.NoError, mLog.ID, EventLogID)
							End If
							DataFieldBind()
							msgCount = ex.Errors.Count
						Finally
							If msgCount = 0 Then
								'Added By Utkarsh On 08-Sep-2011
								mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " Description :" + mLogMaintenance.Maintenance
								MarkLog(Util.Action.Delete, "LogMaintenanceActivityList", mLogDetail, Util.ErrorType.NoError, mLog.ID, EventLogID)
								'End
							End If
						End Try
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Delete" Then
						' '' ''Session("sender") = ""
						DataFieldBind()
					End If
					'''''Response.Redirect("wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
				Case MsgBoxResult.Cancel
					Session("sender") = ""
					DataFieldBind()
					'Response.Redirect("wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
				Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
					'''''Session("sender") = ""
					'''''GetSession()
					'''''DataFieldBind()
					'''''Response.Redirect("wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					'''''Session("sender") = ""
					'''''DataFieldBind()
					'''''Response.Redirect("wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			' '' ''Response.Redirect("wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
			DataFieldBind()
		ElseIf Result1 = 0 Then    'Code Added
			Session("sender") = ""
		End If
	End Sub
	Private Sub SetTitle()
		If mLog.IsNew Then
			lblTitle.Text = "Maintenance Activity " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
		Else
			lblTitle.Text = "Maintenance Activity [ " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " ]"
		End If

		upnlTitle.Update()

	End Sub
	Private Sub SetObject()
		mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).Maintenance = txtMainActivity.Text.Trim
		mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).NRCWONO = txtNCRNo.Text.Trim
		mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).Place = txtPlace.Text.Trim
		'If txtLicenceNo.Text <> "" Then
		'    mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).DoneByID = New Guid(EmployeeID.Value) 'New Guid(cmbDoneBy.SelectedValue)
		'    mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).DoneByName = mLicenseNoListWithEmployee(mLog.LogMaintenances.CurrentItem.DoneByID).EmpName   'IIf(cmbDoneBy.SelectedIndex <= 0, "", cmbDoneBy.SelectedItem.Text)

		'End If
		'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
		'''''If Not (calClosedDate.IsDateValue) Then
		If calClosedDate.Text = "" Then
			mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).ClosedDate = System.DBNull.Value
		Else
			mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).ClosedDate = calClosedDate.Text.ToString
		End If

		'mLogMaintenance = mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex)
		'AttachMyFile()
		' '' ''If MyFile1.FileBytes.Length > 0 Then
		' '' ''    Dim BackupPath As String = ""
		' '' ''    BackupPath = AppSettings("DOCPath") & "New.PDF"
		' '' ''    Try
		' '' ''        MyFile1.PostedFile.SaveAs(BackupPath)
		' '' ''        Dim fs As New FileStream(BackupPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
		' '' ''        Dim fileSize As Integer = CType(fs.Length, Integer)

		' '' ''        Dim fileBytes(fileSize) As Byte
		' '' ''        fs.Read(fileBytes, 0, fileSize)
		' '' ''        mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).ImageFile = fileBytes
		' '' ''        mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).ImageSize = fileSize
		' '' ''        mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).FileExtension = MyFile1.PostedFile.FileName
		' '' ''        btnDelAttach.Enabled = True
		' '' ''        fs.Close()
		' '' ''        System.IO.File.Delete(BackupPath)
		' '' ''    Catch ex As Exception
		' '' ''    End Try
		' '' ''End If

		If mLogMaintenance IsNot Nothing Then
			mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).ImageFile = mLogMaintenance.ImageFile
			mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).ImageSize = mLogMaintenance.ImageSize
			mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).FileExtension = mLogMaintenance.FileExtension
			'MLNo
			For i As Integer = 0 To mLogMaintenance.MaintenanceDoneByEmployees.Count - 1
				Dim ID As Guid = mLogMaintenance.MaintenanceDoneByEmployees(i).ID
				If Not mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Contains(ID) Then
					mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Add(mLogMaintenance.MaintenanceDoneByEmployees(i))
				ElseIf mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Contains(ID) Then
					mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(ID).LicenceNo = mLogMaintenance.MaintenanceDoneByEmployees(i).LicenceNo
					'mAssemblyStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
					mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(ID).EmployeeID = mLogMaintenance.MaintenanceDoneByEmployees(i).EmployeeID
					mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(ID).EmployeeName = mLogMaintenance.MaintenanceDoneByEmployees(i).EmployeeName
				End If
			Next

			For j As Integer = 0 To mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Count - 1
				If Not mLogMaintenance.MaintenanceDoneByEmployees.Contains(mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(j).ID) Then
					mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Remove(mLogMaintenance.MaintenanceDoneByEmployees(j).ID, "")
				End If
			Next
			'End
		End If

		If mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).ImageSize > 0 Then
			ImageButton2.Visible = True
			btnDelAttach.Enabled = True
		Else
			ImageButton2.Visible = False
			btnDelAttach.Enabled = False
		End If
		'End

		mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).AssemblyStatusID = New Guid(cmbAssembly.SelectedValue) 'Added By Vikrant On 02-Sept-2014 For All04092014


	End Sub

	'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
	'''Private Sub SetGrid()
	'''    Dim P As Integer
	'''    Dim lb As LinkButton 'ButtonColumn 

	'''    For j As Integer = 0 To dgMaintenanceActivity1.Rows.Count - 1
	'''        If Me.dgMaintenanceActivity1.Rows.Item(j).Cells(8).Text = "" Then
	'''            P = 0
	'''        Else
	'''            P = CType(Me.dgMaintenanceActivity1.Rows.Item(j).Cells(9).Text, Integer) '10 => 9
	'''        End If

	'''        If P <= 0 Then
	'''            lb = CType(dgMaintenanceActivity1.Rows.Item(j).Cells(8).FindControl("LinkButton1"), LinkButton) '9 => 8
	'''            lb.Enabled = False
	'''        End If
	'''    Next
	'''End Sub
	Private Function Save() As Boolean
		'Dim mEmployeeID As String
		'mEmployeeID = IIf(EmployeeID.Value.Length > 0, EmployeeID.Value, Guid.Empty.ToString)
		Dim LogClone As Log
		LogClone = CType(mLog.Clone, Log)
		SetObject()
		If customvalidate1() Then
			Try
				'Added By Vikrant On 08-Aug-2013 For ALL01082013
				If txtLicenceNo.Text <> "" Then
					If mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Count > 0 Then
						If Not mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(0).EmployeeID.Equals(Guid.Empty) Then
							Dim title As String = "Save Alert !"
							Dim message As String = ""
							mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(0).EmployeeID.ToString, mLog.Date)
							If (mEmployeeStatus(0).Information <> "") Then
								message = mEmployeeStatus(0).Information
								MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "")
								Return False
							End If
						End If
					End If
				End If
				'End
				If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
					If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
				   Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
						'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly)
						'''''msg1.ReplacePage = "wfLogMaintenanceActivity.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
						'''''msg1.Show()

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
					'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
					'''''msg1.ReplacePage = "wfLogMaintenanceActivity.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
					'''''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 8145 Then
					'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
					'''''msg1.ReplacePage = "wfLogMaintenanceActivity.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
					'''''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 2627 Then
					'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
					'''''msg1.ReplacePage = "wfLogMaintenanceActivity.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
					'''''msg1.Show()

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 547 Then
					'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
					'''''msg1.ReplacePage = "wfLogMaintenanceActivity.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
					'''''msg1.Show()

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
	'End
	Private Sub ControlVisibility()
		'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
		If Session("OpenFromLMA") = True Then
			btnDefectActionList.Enabled = Not mLog.IsNew And mLog.LogTypeID = 1

			'''If AppSettings("LogDetailPage") = "NewPage" Then
			'''    btnLogPax.Enabled = False
			'''    btnHobbsOffset.Enabled = False
			'''Else
			'''    btnLogPax.Visible = False
			'''    btnHobbsOffset.Visible = False
			'''End If
			btnParameterList.Visible = IIf(AppSettings("LogDetailPage") = "NewPage", True, False) And mLog.LogTypeID = 1
			lblTLPNo.Text = IIf(AppSettings("LogDetailPage") = "NewPage", "Log Page No.", "TLP No.")
			'btnDefectActionList.Enabled = False
			'btnParameterList.Enabled = False
			'btnFlightCrew.Enabled = False
			'btnFuelOil.Enabled = False
			'btnLogDetails.Enabled = False

			btnDefectActionList.Visible = False
			btnParameterList.Visible = False
			btnFlightCrew.Visible = False
			btnFuelOil.Visible = False
			btnLogDetails.Visible = False
			lblMaintenanceActivityDetails.Visible = False
			'btnLogPax.Visible = False
			btnHobbsOffset.Visible = False

			btnBack.Text = "Close"
			btnBack.ToolTip = "Click to close Log Maintenance Activity page"

			btnAdd.Visible = False
			lblLogMaintenanceTitle.Visible = False
			dgMaintenanceActivity1.Visible = False
			btnSave.Visible = True
			'End
			btnBack.Visible = True
		Else
			'Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
			btnDefectActionList.Enabled = Not mLog.IsNew

			If AppSettings("LogDetailPage") = "NewPage" And mLog.LogTypeID = 1 Then
				btnLogPax.Enabled = Not mLog.IsNew
				btnHobbsOffset.Enabled = (mLog.HourType = 2)
			Else
				'btnLogPax.Visible = False
				btnHobbsOffset.Visible = False
			End If



			btnParameterList.Visible = IIf(AppSettings("LogDetailPage") = "NewPage", True, False) And mLog.LogTypeID = 1
			'Commented By Utkarsh ON 15-Jan-2013 FOR ALL15012013
			'btnMaintenanceAcitvity.Visible = IIf(AppSettings("LogDetailPage") = "NewPage", False, True)
			'End
			lblTLPNo.Text = IIf(AppSettings("LogDetailPage") = "NewPage", "Log Page No.", "TLP No.")
			'End
			btnDefectActionList.Enabled = Not mLog.IsNew
			btnFuelOil.Visible = mLog.LogTypeID = 1
			'btnBack.Visible = False
		End If
		'Ajay
		'If mLogMaintenance.ImageSize > 0 Then
		'    ImageButton2.Visible = True
		'    btnDelAttach.Enabled = True
		'Else
		'    ImageButton2.Visible = False
		'    btnDelAttach.Enabled = False
		'End If
		If Session("OpenFromLMA") = False Then
			btnLogDetails.Visible = mLog.LogTypeID = 1
			btnDefectActionList.Visible = mLog.LogTypeID = 1
			btnFlightCrew.Visible = mLog.LogTypeID = 1
			lblMaintenanceActivityDetails.Visible = mLog.LogTypeID = 1
		End If
		'lblLicenceCount.Visible = mLogMaintenance.MaintenanceDoneByEmployees.Count > 1 'Ajay
	End Sub
	'Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013 
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

	'End
	Private Sub AttachMyFile()

		mLogMaintenance = Session("mLogMaintenance")

		Try
			mLogMaintenance.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
			mLogMaintenance.ImageSize = Session("FileUpload.FileSize")
			mLogMaintenance.FileExtension = Session("FileUpload.FileExtension")
			Session.Remove("FileUpload.FileSize")
			Session.Remove("FileUpload.FileContent")
			Session.Remove("FileUpload.FileExtension")
			If mLogMaintenance.ImageSize > 0 Then
				ImageButton2.Visible = True
				btnDelAttach.Enabled = True
			Else
				ImageButton2.Visible = False
				btnDelAttach.Enabled = False
			End If
			upnlAttach.Update()

		Catch ex As Exception
			MSGBoxCtrl.Show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
		End Try


		Session("mLogMaintenance") = mLogMaintenance
		Session("mLog") = mLog
	End Sub
	'Added function CustomValidate1  (solution for replacing REQUIRED FIELD VALIDATOR with CUSTOM VALIDATOR) 
	'Please refer HTML Changes -
	' -- Required Field validator removed
	' -- Custome Validator added (if not already there..) for any of Control. And Used in below function.
	Public Function CustomValidate2() As Boolean
		Dim strMSG As String = ""
		Dim str As String = ""
		If Len(Trim(txtMainActivity.Text)) = 0 Then strMSG = "Description Required" + "<Br>"


		''Added by Saylee on 07-Nov-2014 for ALL07112014
		'Dim mEmployeeID As String
		'mEmployeeID = IIf(EmployeeID.Value.Length > 0, EmployeeID.Value, Guid.Empty.ToString)
		''******
		'If txtEmployee.Text <> "" And New Guid(mEmployeeID).Equals(Guid.Empty) And cvDoneBy.ControlToValidate = "txtEmployee" Then 'Added by Saylee on 07-Nov-2014 for ALL07112014
		'    strMSG = strMSG + "<br>" + "Please select proper Done By."
		'    cvDoneBy.IsValid = False
		'End If


		If strMSG.Trim <> "" Then
			Me.cvControlValidator.ErrorMessage = strMSG
			Me.cvControlValidator.IsValid = False
			Return False

		End If
		Return True
	End Function
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
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		'COMMENTED by Saylee on 07-Nov-2014 for ALL07112014
		'Commented and Added by Saylee on 10-Oct-2014, here send ExcludeUseInFlightLogRequired=False as need to show all employees
		' mEmployeeList = EmployeeListForCombo.GetEmployeeListForCombo("<SELECT>", False, True, True)
		'mEmployeeList = EmployeeListForCombo.GetEmployeeListForCombo("<SELECT>", False, True, False)

		'Added by Saylee on 07-Nov-2014 for ALL07112014
		mLicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList()
		'cmbDoneBy.DataSource = mEmployeeList
		Session("mLicenseNoListWithEmployee") = mLicenseNoListWithEmployee
		'*********

		DataBindGrid()

		'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
		If (mLog.LogMaintenances.Count = 0) Then
			mLogMaintenance = LogMaintenance.NewChildLogMaintenance(mLog.ID)
		Else
			If Session("LogMaintenanceEdit") = True Then 'Changed by By Utkarsh on 13-Aug-2013 for ALL13082013-2
				mLogMaintenance = mLog.LogMaintenances.CurrentItem
			Else
				mLogMaintenance = LogMaintenance.NewChildLogMaintenance(mLog.ID)
			End If
		End If
		Session("mLogMaintenance") = mLogMaintenance

		If mLogMaintenance.ClosedDate.ToString = "" Then
			calClosedDate.Text = ""
		Else
			calClosedDate.Text = mLogMaintenance.ClosedDateFormatted
		End If

		'End

		If Session("LogMaintenanceEdit") = True Then 'Changed by By Utkarsh on 13-Aug-2013 for ALL13082013-2
			txtMainActivity.Text = mLog.LogMaintenances.CurrentItem.Maintenance
			txtNCRNo.Text = mLog.LogMaintenances.CurrentItem.NRCWONO
			txtPlace.Text = mLog.LogMaintenances.CurrentItem.Place
			'cmbDoneBy.SelectedValue = mLog.LogMaintenances.CurrentItem.DoneByID.ToString
			'EmployeeID.Value = mLog.LogMaintenances.CurrentItem.DoneByID.ToString
			'If Not mLog.LogMaintenances.CurrentItem.DoneByID.Equals(Guid.Empty) Then
			'    txtLicenceNo.Text = mLicenseNoListWithEmployee(mLog.LogMaintenances.CurrentItem.DoneByID).LicenseNoEmpName 'mLog.LogMaintenances.CurrentItem.DoneByName
			'    EmployeeID.Value = mLog.LogMaintenances.CurrentItem.DoneByID.ToString
			'End If

			'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
			If mLog.LogMaintenances.CurrentItem.ClosedDate.ToString = "" Then
				calClosedDate.Text = ""
			Else
				calClosedDate.Text = mLog.LogMaintenances.CurrentItem.ClosedDateFormatted
			End If
			cmbAssembly.SelectedValue = mLog.LogMaintenances.CurrentItem.AssemblyStatusID.ToString  'Added By Vikrant On 02-Sept-2014 For All04092014
		Else    'code Added by Yogita to refresh values of cotrol after New Record
			txtMainActivity.Text = mLogMaintenance.Maintenance
			txtNCRNo.Text = mLogMaintenance.NRCWONO
			txtPlace.Text = mLogMaintenance.Place
			txtLicenceNo.Text = mLogMaintenance.DoneByName
		End If

		'Added By Vikrant On 02-Sept-2014 For All04092014
		mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, mLog.MachineID.ToString, mLog.Date.ToString, "", True)
		cmbAssembly.DataSource = mAssemblylist
		Session("mAssemblylist") = mAssemblylist
		'End
		BindLicenceNo(mLogMaintenance) 'MLNo
		DataBind()
		upnlDetails.Update()
	End Sub
	Private Sub DataBindGrid()
		dgMaintenanceActivity1.DataSource = mLog.LogMaintenances
		dgMaintenanceActivity1.DataBind()

		upnlDetails.Update()
	End Sub
#End Region

#Region " Events "

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack And CType(Session("sender"), String) = "" Then
			'If txtMainActivity.Enabled = True Then
			'    setFocus(txtMainActivity)
			'End If
			DataFieldBind()
			'If mLog.ImageSize > 0 Then
			If mLogMaintenance.ImageSize > 0 Then
				ImageButton2.Visible = True
				btnDelAttach.Enabled = True
			Else
				ImageButton2.Visible = False
				btnDelAttach.Enabled = False
			End If
			'MLNo
			SetLicenceCount(mLogMaintenance)

			UserNameForLicenceList = User.Identity.Name
			Session("UserNameForLicenceList") = UserNameForLicenceList
			'End
		End If
		SetTitle()

		If mLog.Date.ToString = "" Then
			txtDate.Text = ""
		Else
			txtDate.Text = mLog.DateFormatted
		End If

		' '' ''MessageBoxResult()
		ControlVisibility()

		''' SetGrid()  'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
	End Sub
	'MLNo
	Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
		If IsValid Then
			'SetObject()
			Session("mMaintenanceID") = mLogMaintenance.ID
			mMaintenanceDoneByEmployees = mLogMaintenance.MaintenanceDoneByEmployees
			Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
			Session("MaintenanceDoneOnDate") = mLog.DateFormatted.ToString
			ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
		Else
			upnlErrorList.Update()
		End If

	End Sub
	Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
		For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
			Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
			If Not mLogMaintenance.MaintenanceDoneByEmployees.Contains(ID) Then
				mLogMaintenance.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
			ElseIf mLogMaintenance.MaintenanceDoneByEmployees.Contains(ID) Then
				mLogMaintenance.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
				'mAssemblyStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
				mLogMaintenance.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
				mLogMaintenance.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
			End If
		Next

		For j As Integer = 0 To mLogMaintenance.MaintenanceDoneByEmployees.Count - 1
			If Not mMaintenanceDoneByEmployees.Contains(mLogMaintenance.MaintenanceDoneByEmployees(j).ID) Then
				mLogMaintenance.MaintenanceDoneByEmployees.Remove(mLogMaintenance.MaintenanceDoneByEmployees(j).ID, "")
			End If
		Next
		Session("mLogMaintenance") = mLogMaintenance
		BindLicenceNo(mLogMaintenance)
		SetLicenceCount(mLogMaintenance)
		upnlLicenceNo.Update()
	End Sub
	'Private Sub txtEmployee_TextChanged(sender As Object, e As System.EventArgs) Handles txtEmployee.TextChanged
	'    'Added by Saylee on 07-Nov-2014 for ALL07112014
	'    Dim mEmployeeID As String
	'    mEmployeeID = IIf(EmployeeID.Value.Length > 0, EmployeeID.Value, Guid.Empty.ToString)
	'    '******

	'    Dim str As String = ""
	'    If txtLicenceNo.Text <> "" And New Guid(mEmployeeID).Equals(Guid.Empty) And cvDoneBy.ControlToValidate = "txtEmployee" Then 'Added by Saylee on 07-Nov-2014 for ALL07112014
	'        cvDoneBy.ErrorMessage = "Please select proper Done By."
	'        cvDoneBy.IsValid = False
	'    End If
	'    upnlErrorList.Update()
	'    upnlDetails.Update()
	'End Sub
	Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs) Handles txtLicenceNo.TextChanged
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
			If mLogMaintenance.MaintenanceDoneByEmployees.Count > 0 Then
				mLogMaintenance.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
				mLogMaintenance.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
				mLogMaintenance.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
			Else
				mLogMaintenance.MaintenanceDoneByEmployees.Add(mLogMaintenance.ID, 12, DoneByID, LicenseNo, "", EmpName)
			End If

		Else
			If mLogMaintenance.MaintenanceDoneByEmployees.Count > 0 Then
				mLogMaintenance.MaintenanceDoneByEmployees.RemoveAt(0)
			End If
		End If
		Session("mLogMaintenance") = mLogMaintenance
		BindLicenceNo(mLogMaintenance)
		SetLicenceCount(mLogMaintenance)
	End Sub
	'End
	Private Sub dgMaintenanceActivity_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMaintenanceActivity1.RowCommand
		'Dim Index As Int32 = dgMaintenanceActivity1.CurrentPageIndex * dgMaintenanceActivity1.PageSize + e.Item.ItemIndex
		Dim index As Integer
		'   Dim ID As Guid
		Select Case e.CommandName
			Case "EditRec"

				If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then
					MarkLog(Util.Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'  ID = New Guid(e.CommandArgument.ToString)
				index = (CInt(e.CommandArgument) - 1) + dgMaintenanceActivity1.PageSize * dgMaintenanceActivity1.PageIndex
				mLog.LogMaintenances.CurrentIndex = index
				txtMainActivity.Text = mLog.LogMaintenances.Item(index).Maintenance
				txtNCRNo.Text = mLog.LogMaintenances.Item(index).NRCWONO
				txtPlace.Text = mLog.LogMaintenances.Item(index).Place
				'cmbDoneBy.SelectedValue = mLog.LogMaintenances.Item(mID).DoneByID.ToString
				'EmployeeID.Value = mLog.LogMaintenances.Item(mID).DoneByID.ToString
				'
				BindLicenceNo(mLog.LogMaintenances.CurrentItem)
				SetLicenceCount(mLog.LogMaintenances.CurrentItem)
				cmbAssembly.SelectedValue = mLog.LogMaintenances.Item(index).AssemblyStatusID.ToString 'Added By Vikrant On 02-Sept-2014 For All04092014
				DataBindGrid()

				'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
				mLogMaintenance = mLog.LogMaintenances.CurrentItem

				If mLogMaintenance.ClosedDate.ToString = "" Then
					calClosedDate.Text = ""
				Else
					calClosedDate.Text = mLogMaintenance.ClosedDateFormatted
				End If

				Session("mLogMaintenance") = mLogMaintenance
				ControlVisibility()
				'End
				DataBind()
				'Added By Utkarsh On 08-Sep-2011
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " Description :" + mLogMaintenance.Maintenance
				MarkLog(Util.Action.Edit, "LogMaintenanceActivityList", mLogDetail, Util.ErrorType.NoError, mLog.ID, EventLogID)
				'End
				Session("LogMaintenanceEdit") = True 'Changed by By Utkarsh on 13-Aug-2013 for ALL13082013-2
				Session("mLog") = mLog

				If mLog.LogMaintenances.Item(mLog.LogMaintenances.CurrentIndex).ImageSize > 0 Then
					ImageButton2.Visible = True
					btnDelAttach.Enabled = True
				Else
					ImageButton2.Visible = False
					btnDelAttach.Enabled = False
				End If

				SetTitle()
				upnlErrorList.Update()
			Case "DeleteRec"
				If (Not User.IsInRole("LogDelete")) Then
					MarkLog(Util.Action.Delete, "Flight Log", User.Identity.Name & " is not Authorized User to delete " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				index = (CInt(e.CommandArgument) - 1) + dgMaintenanceActivity1.PageSize * dgMaintenanceActivity1.PageIndex
				DeleteRecord(index)

				'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
			Case "View"

				If (Not User.IsInRole("LogView")) Then
					MarkLog(Util.Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				Dim No As New Random
				'   Dim StrName As String = "abc" & No.Next.ToString
				Dim strName As String = "Maintenance Activity " & mLog.LogNoLogPageNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
				'----------------------------------------------------------------------
				Dim mLogMaintenance As LogMaintenance
				mLogMaintenance = mLog.LogMaintenances(New Guid(e.CommandArgument.ToString))
				If mLogMaintenance.ImageSize > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & mLogMaintenance.FileExtension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mLogMaintenance.FileExtension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mLogMaintenance.ImageFile, 0, mLogMaintenance.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						Dim Str As String
						Str = "openFile();"
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
					End If
				End If
				'End
		End Select
	End Sub
	Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		'Added by Saylee on 8-Apr-2014 for ALL08042014
		If ((Session("OpenFromLMA") = False) And ((Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew))) _
		  Or ((Session("OpenFromLMA") = True) And ((Not User.IsInRole("LogMaintenanceActivityNew") And mLog.IsNew) Or (Not User.IsInRole("LogMaintenanceActivityEdit") And Not mLog.IsNew))) Then
			'MarkLog(Util.Action.Save, "Log Maintenance Activity", User.Identity.Name & " is not Authorized User to Add " & "Flight Maintenance Activity", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			'''''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
			'''''msg.ReplacePage = "wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
			'''''msg.Show()
			'''''Session("sender") = "Authorization"

			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If Not IsValid Then upnlErrorList.Update() : Exit Sub

		If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub

		'Added By Vikrant On 05-Aug-2013 For ALL01082013
		'Dim mEmployeeID As String
		'mEmployeeID = IIf(EmployeeID.Value.Length > 0, EmployeeID.Value, Guid.Empty.ToString)

		'If cmbDoneBy.SelectedIndex > 0 Then
		If txtLicenceNo.Text <> "" Then
			Dim mLogMaint As LogMaintenance
			If Session("LogMaintenanceEdit") = True Then
				mLogMaint = mLog.LogMaintenances.CurrentItem
			Else
				mLogMaint = mLogMaintenance
			End If
			If mLogMaint.MaintenanceDoneByEmployees.Count > 0 Then
				Dim title As String = "Save Alert !"
				Dim message As String = ""
				mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLogMaint.MaintenanceDoneByEmployees(0).EmployeeID.ToString, mLog.Date)
				If (mEmployeeStatus(0).Information <> "") Then
					message = mEmployeeStatus(0).Information
					'''''ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message))
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			End If

		End If
		'End

		If Session("LogMaintenanceEdit") = False Then 'Changed by By Utkarsh on 13-Aug-2013 for ALL13082013-2
			mLog.LogMaintenances.Add(mLog.ID)
			''For i As Integer = 0 To mLog.LogMaintenances.Count - 1
			''    mLog.LogMaintenances(i).SrNo = i + 1
			''Next

			mLog.LogMaintenances.CurrentIndex = mLog.LogMaintenances.Count - 1

			SetObject()

			Session("mLog") = mLog
			'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
			mLogMaintenance = LogMaintenance.NewChildLogMaintenance(mLog.ID)
			Session("mLogMaintenance") = mLogMaintenance
			'End

			' '' ''Response.Redirect("wfLogMaintenanceActivity.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))

			DataFieldBind()

		Else
			SetObject()
			Session("mLog") = mLog
			Session("LogMaintenanceEdit") = False 'Changed by By Utkarsh on 13-Aug-2013 for ALL13082013-2
			'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
			mLogMaintenance = LogMaintenance.NewChildLogMaintenance(mLog.ID)
			Session("mLogMaintenance") = mLogMaintenance
			'End
			'Response.Redirect("wfLogMaintenanceActivity.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))

			DataFieldBind()

		End If

		If mLogMaintenance.ImageSize > 0 Then
			ImageButton2.Visible = True
			btnDelAttach.Enabled = True
		Else
			ImageButton2.Visible = False
			btnDelAttach.Enabled = False
		End If

		SetTitle()
		lblLicenceCount.Visible = False
		upnlDetails.Update()
		upnlErrorList.Update()
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallautoResize", "CallautoResize();", True)
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		MarkLog(Util.Action.Close, "Log Maintenance Activity", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
		SetSession()
		RemoveSession()


		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End


		'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
		If Session("OpenFromLMA") = True Then
			Response.Redirect("Index.aspx")
			'End
		Else
			''Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
			'If AppSettings("LogDetailPage") = "NewPage" Then
			'    Response.Redirect("wfLogSOP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			'Else
			'    Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			'End If
			''End
			Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage")) 'Added by Prashant 23-Aug-2018
		End If
	End Sub
	Private Sub btnLogDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogDetails.Click
		SetSession()
		RemoveSession()
		''Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
		'If AppSettings("LogDetailPage") = "NewPage" Then
		'    Response.Redirect("wfLogSOP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		'Else
		'    Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		'End If
		''End
		If AppSettings("LogDetailPage") = "NewPage" Then
			If mLog.IsTLP = "True" Then  'Added by Prashant 23-Aug-2018
				Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")) 'Added by Prashant 23-Aug-2018
			Else
				Response.Redirect("wfLogSOP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			End If
		Else
			Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub
	Private Sub btnFuelOil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFuelOil.Click
		SetSession()
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		'Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("BackPage1"))
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnDefectActionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefectActionList.Click
		SetSession()
		RemoveSession()
		'Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("BackPage1"))
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnFlightCrew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFlightCrew.Click
		SetSession()
		RemoveSession()
		'Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("BackPage1"))
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	'Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
	Private Sub btnLogPax_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogPax.Click
		SetSession()
		RemoveSession()
		NewLogPax()
		'Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("BackPage1") & "&BackPage1=wfLogMaintenanceActivity_Ajax.aspx")
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnHobbsOffset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHobbsOffset.Click
		SetSession()
		RemoveSession()
		NewHobbsOffSet()
		Response.Redirect("wfHobbsOffset_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("BackPage1") & "&BackPage1=wfLogMaintenanceActivity_Ajax.aspx")
	End Sub
	Private Sub btnParameterList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnParameterList.Click
		SetSession()
		RemoveSession()
		'Response.Redirect("wfLogParameterList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("BackPage1"))
		'Added By Prashant 23-Aug-2018
		Response.Redirect("wfLogParameterList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	'End

	'Added By Utkarsh ON 28-Feb-2013 FOR All27022013-1
	Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
		Dim No As New Random
		Dim StrName As String = "abc" & No.Next.ToString
		If mLogMaintenance.ImageSize > 0 Then
			Dim path As String = AppSettings("DOCPath") & "\" & StrName & mLogMaintenance.FileExtension
			Dim fs As FileStream
			If File.Exists(AppSettings("DOCPath")) = False Then
				'Delete File if exist
				System.IO.File.Delete(AppSettings("DOCPath") & StrName & mLogMaintenance.FileExtension)
				' Create the file.
				fs = File.Create(path)
				'' Add some information to the file.
				fs.Write(mLogMaintenance.ImageFile, 0, mLogMaintenance.ImageFile.Length)
				fs.Close()
				Session("DOCPath") = path
				Dim Str As String
				Str = "openFile();"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
			End If
		End If
	End Sub
	Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte
		mLogMaintenance.ImageFile = file1
		mLogMaintenance.ImageSize = 0
		Session("mLogMaintenance") = mLogMaintenance
		'End

		ImageButton2.Visible = False
		btnDelAttach.Enabled = False
	End Sub
	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
		'Added by Saylee on 8-Apr-2014 for ALL08042014
		If ((Session("OpenFromLMA") = False) And ((Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew))) _
		 Or ((Session("OpenFromLMA") = True) And ((Not User.IsInRole("LogMaintenanceActivityNew") And mLog.IsNew) Or (Not User.IsInRole("LogMaintenanceActivityEdit") And Not mLog.IsNew))) Then

			SetObject()
			SetSession()
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Save, "LogMaintenanceActivityList", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			'''''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
			'''''msg.ReplacePage = "wfLogMaintenanceActivity.aspx?MsgResult=0&BackPage=Index.aspx"
			'''''Session("sender") = "Authorization"
			'''''msg.Show()

			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

			Exit Sub
		End If
		If Not IsValid Then upnlErrorList.Update() : Exit Sub
		If Save() = True Then
			'Added By Utkarsh On 08-Sep-2011
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " Description :" + mLogMaintenance.Maintenance
			MarkLog(Util.Action.Save, "LogMaintenanceActivityList", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
			'End
			'''''Response.Redirect("wfLogMaintenanceActivity.aspx?Index.aspx")
			DataFieldBind()
		End If

	End Sub
	'End
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	Protected Sub calClosedDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calClosedDate.TextChanged
		'Changed by Yogita for Wrong Date Format
		If IsDate(calClosedDate.Text) Or (calClosedDate.Text = "") Then
			'

			If calClosedDate.Text = "" Then
				mLogMaintenance.ClosedDate = System.DBNull.Value
				calClosedDate.Text = mLogMaintenance.ClosedDate.ToString
			Else
				mLogMaintenance.ClosedDate = calClosedDate.Text
				calClosedDate.Text = mLogMaintenance.ClosedDateFormatted
			End If

		Else
			calClosedDate.Text = ""
		End If
	End Sub

	Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
		AttachMyFile()
	End Sub

#End Region


	'Added by Saylee on 07-Nov-2014 for ALL07112014
#Region "Service Methods"
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
	Public Shared Function GetEmployeeList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()


		Dim Employeelist As LicenseNoListWithEmployee
		Employeelist = LicenseNoListWithEmployee.GetLicenseNoList(prefixText) 'EmployeeListForCombo.GetEmployeeListForCombo(" <SELECT>", False, True, False)

		If count = 0 Then
			Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In Employeelist
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
		Else
			Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In Employeelist
					Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
		End If
	End Function
#End Region


End Class
