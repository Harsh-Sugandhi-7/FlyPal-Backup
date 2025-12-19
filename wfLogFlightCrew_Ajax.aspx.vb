
Partial Class wfLogFlightCrew_Ajax
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

	' Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
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
	Public mLog As Log
	Public mRegNo As String
	Public mAttachToID As Guid
	Public mCrewDesignationList As DesignationList
	Public mEmployeeList As EmployeeList
	Public mDutyTypeList As DutyTypeList
	Dim mLogFlightCrewDetail As String
	Dim EventLogID As Guid
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mLog = CType(Session("mLog"), Log)
		mRegNo = CType(Session("mRegNo"), String)
		mEmployeeList = Session("mEmployeeList")
	End Sub
	Private Sub SetSession()
		Session("mLog") = mLog
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mRegNo")
		Session.Remove("mMachinePreviousRegDetailEdit")
	End Sub
	Private Sub NewRecord()
	End Sub
	Private Sub SetObject()
		mLog.LogCrews.Item(mLog.LogCrews.CurrentIndex).CrewID = New Guid(cmbCrew.SelectedValue)
		mLog.LogCrews.Item(mLog.LogCrews.CurrentIndex).DutyAsID = cmbDutyAs.SelectedValue
		'Commented & Added By Vikrant On 11-Sept-2013 For ALL02092013
		'mLog.LogCrews.Item(mLog.LogCrews.CurrentIndex).CrewName = IIf(cmbCrew.SelectedIndex > 0, mEmployeeList(cmbCrew.SelectedIndex).EmpName, "") 'New added
		mLog.LogCrews.Item(mLog.LogCrews.CurrentIndex).CrewName = IIf(cmbCrew.SelectedIndex > 0, cmbCrew.SelectedItem.Text, "") 'New added
		'End
		mLog.LogCrews.Item(mLog.LogCrews.CurrentIndex).DutyType = IIf(cmbDutyAs.SelectedIndex > 0, cmbDutyAs.SelectedItem.Text, "") 'New added

		mLogFlightCrewDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " " + lblTLPNo.Text + " : " + mLog.LogPageNoFormatted + " Name : " + IIf(cmbCrew.SelectedIndex > 0, mEmployeeList(cmbCrew.SelectedIndex).EmpName, "")
		MarkLog(Util.Action.Save, "Log Flight Crew", mLogFlightCrewDetail, Util.ErrorType.HandledError, mLog.LogCrews.Item(mLog.LogCrews.CurrentIndex).ID, EventLogID)
	End Sub
	Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
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

	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		'' ''If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
		'' ''    Result1 = -1
		'' ''Else
		'' ''    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		'' ''End If
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then '''''CType(Session("sender"), String)
						Dim LogFlightCrewID As Guid = Guid.Empty
						Try
							Session("sender") = ""
							LogFlightCrewID = mLog.LogCrews(mLog.LogCrews.CurrentIndex).ID
							mLogFlightCrewDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " " + lblTLPNo.Text + " : " + mLog.LogPageNoFormatted + " Name : " + mLog.LogCrews(mLog.LogCrews.CurrentIndex).CrewName

							mLog.LogCrews.Remove(mLog.LogCrews(mLog.LogCrews.CurrentIndex))
							For i As Integer = 0 To mLog.LogCrews.Count - 1
								mLog.LogCrews(i).SrNo = i + 1
							Next
							Session("mLog") = mLog
							Session("mMachinePreviousRegDetailEdit") = False
							DataFieldBind()
							upnlDetails.Update()

							' '' ''Response.Redirect("wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
						Catch ex As SqlException
							If ex.Number = 8145 Then
								' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
								' '' ''msg1.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
								' '' ''msg1.Show()
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 2627 Then
								' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
								' '' ''msg1.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
								' '' ''msg1.Show()
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 547 Then
								' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
								' '' ''msg1.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
								' '' ''msg1.Show()
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
							End If
							DataFieldBind()
							msgCount = ex.Errors.Count
						Finally
							If msgCount = 0 Then
								MarkLog(Util.Action.Delete, "Log Flight Crew", mLogFlightCrewDetail, Util.ErrorType.NoError, LogFlightCrewID, EventLogID)
							End If
						End Try
					End If
				Case MsgBoxResult.No
					' '' ''Session("sender") = ""
					DataFieldBind()
					' '' ''Response.Redirect("wfLogFlightCrew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
				Case MsgBoxResult.Ok
					' '' ''Session("sender") = ""
					' '' ''DataFieldBind()
					' '' ''Response.Redirect("wfLogFlightCrew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"
					Session("sender") = ""
					DataFieldBind()
					' '' ''Response.Redirect("wfLogFlightCrew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			' '' ''Response.Redirect("wfLogFlightCrew.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))\
			DataFieldBind()
		ElseIf Result1 = 0 Then   'Code Added
			Session("sender") = ""
			'DataFieldBind()
		End If
	End Sub
	Private Sub EditRecord(ByVal ID As Guid)

		cmbDutyAs.SelectedValue = mLog.LogCrews.Item(ID).DutyAsID
		cmbDesignationList.SelectedValue = Employee.GetEmployee(mLog.LogCrews.Item(ID).CrewID).DesignationID.ToString
		If cmbDesignationList.SelectedIndex > 0 Then
			'Commented & Added By Vikrant On 11-Sept-2013 For ALL02092013
			'cmbCrew.DataSource = mEmployeeList
			cmbCrew.DataSource = EmployeeList.GetEmployeeList("", cmbDesignationList.SelectedItem.Text, "(SELECT)")
			'End
			Session("mEmployeeList") = mEmployeeList
			cmbCrew.DataBind()
			cmbCrew.SelectedValue = mLog.LogCrews.Item(ID).CrewID.ToString
		End If
	End Sub
	Private Sub SetPage()
		If mLog.IsNew Then
			lblTitle.Text = "Flight Crew [New]"
		Else
			lblTitle.Text = "Flight Crew [" & mLog.RegNo & "]"
		End If
		'lblResult.Text = "List of Crews: " & mLog.LogCrews.Count & " Record(s) found"
	End Sub
	Private Sub ControlVisibility()
		'Added By Utkarsh ON 17-Jul-2012 FOR ALL16072012-3

		btnDefectActionList.Enabled = Not mLog.IsNew

		If AppSettings("LogDetailPage") = "NewPage" Then
			btnLogPax.Enabled = Not mLog.IsNew
			btnHobbsOffset.Enabled = (mLog.HourType = 2)
		Else
			'btnLogPax.Visible = False
			btnHobbsOffset.Visible = False
		End If

		btnParameterList.Visible = IIf(AppSettings("LogDetailPage") = "NewPage", True, False)
		'Commented By Utkarsh ON 15-Jan-2013 FOR ALL15012013
		'btnMaintenanceAcitvity.Visible = IIf(AppSettings("LogDetailPage") = "NewPage", False, True) 
		'End
		lblTLPNo.Text = IIf(AppSettings("LogDetailPage") = "NewPage", "Log Page No.", "TLP No.")
		'End
		If cmbDesignationList.SelectedIndex > 0 Then
			cmbCrew.Enabled = True
		Else
			cmbCrew.Enabled = False
		End If
		' '' ''upnlErrorList.Update()
	End Sub
	Private Function CustomValidate1() As Boolean
		Dim strMSG As String = ""
		If Not mLog.LogDetails.IsValid Then
			For i As Integer = 0 To mLog.LogCrews.CurrentItem.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mLog.LogCrews.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If
		If strMSG.Trim <> "" Then
			cvControlValidator.ErrorMessage = strMSG
			cvControlValidator.IsValid = False
			Return False
		End If
		Return True
	End Function
	'Added By Utkarsh ON 17-Jul-2012 FOR ALL16072012-3
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

#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()

		dgCrewList1.DataSource = mLog.LogCrews

		mCrewDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
		cmbDesignationList.DataSource = mCrewDesignationList
		Session("mCrewDesignationList") = mCrewDesignationList


		cmbDesignationList_SelectedIndexChanged(Nothing, Nothing)

		mDutyTypeList = DutyTypeList.GetDutyTypeList(True, "(SELECT)")
		cmbDutyAs.DataSource = mDutyTypeList
		Session("mDutyTypeList") = mDutyTypeList

		DataBind()

		If Session("mMachinePreviousRegDetailEdit") = True Then
			mAttachToID = Session("mAttachToID")
			EditRecord(mAttachToID)
		End If
	End Sub
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)

		If custValidator.ControlToValidate = "txtTLPNo" Then
			If cmbDesignationList.SelectedIndex = 0 Then
				custValidator.ErrorMessage = "Select designation"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
		If custValidator.ControlToValidate = "cmbCrew" Then
			If cmbCrew.SelectedIndex = 0 Then
				custValidator.ErrorMessage = "Crew required"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
		If custValidator.ControlToValidate = "cmbDutyAs" Then
			If cmbDutyAs.SelectedIndex = 0 Then
				custValidator.ErrorMessage = "Duty type required"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack And Session("sender") = "" Then
			DataFieldBind()
		End If
		If mLog IsNot Nothing Then
			CalDate.Text = Format(CDate(mLog.Date), AppSettings("DateFormat"))
			calDate.ReadOnly = True
		End If

		SetPage()
		ControlVisibility()
		' '' ''MessageBoxResult()
	End Sub
	Private Sub cmbDesignationList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesignationList.SelectedIndexChanged
		If cmbDesignationList.SelectedIndex > 0 Then
			mEmployeeList = EmployeeList.GetEmployeeList("", cmbDesignationList.SelectedItem.Text, "(SELECT)")
			cmbCrew.DataSource = mEmployeeList
			Session("mEmployeeList") = mEmployeeList
			cmbCrew.DataBind()
			cmbCrew.Enabled = True
		Else
			cmbCrew.Enabled = False
		End If

	End Sub
	Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		'If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
		'    SetSession()
		'    'MarkLog(Util.Action.[New], "Log", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
		'    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
		'    msg.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
		'    Session("sender") = "Authorization"
		'    msg.Show()
		'    Exit Sub
		'End If
		If Not IsValid Then upnlErrorList.Update() : Exit Sub

		'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
		If cmbCrew.SelectedIndex > 0 Then
			Dim Title As String = "Save Alert !"
			Dim Message As String = ""
			Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(cmbCrew.SelectedValue.ToString, mLog.Date.ToString)
			If mEmployeeStatus(0).Information <> "" Then
				Message = mEmployeeStatus(0).Information
				MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		End If
		'End

		If Session("mMachinePreviousRegDetailEdit") = False Then
			'MarkLog(Util.Action.[New], "Log", " Aircraft Name ->" & mLog.RegNo & " Certificate No. -> " & Trim(txtNo.Text) & "  Certificate Name -> " & txtName.Text, Util.ErrorType.NoError, Guid.Empty)

			If mLog.LogCrews.Contains(New Guid(cmbCrew.SelectedValue.ToString)) Then
				' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
				' '' ''msg1.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
				' '' ''msg1.Show()
				MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Flight Crew", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			mLog.LogCrews.Add(mLog.ID)
			If Not CustomValidate1() Then
				mLog.LogCrews.Remove(mLog.LogCrews.CurrentItem)
				Exit Sub
			End If

			For i As Integer = 0 To mLog.LogCrews.Count - 1
				mLog.LogCrews(i).SrNo = i + 1
			Next
			mLog.LogCrews.CurrentIndex = mLog.LogCrews.Count - 1 'New Added
			SetObject() 'New Added
			Session("mLog") = mLog
			' '' ''Response.Redirect("wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
			DataFieldBind()

		Else
			'Added By Vikrant On 11-Sept-2013 For ALL02092013
			If Not mLog.LogCrews.CurrentItem.CrewID.Equals(New Guid(cmbCrew.SelectedValue.ToString)) Then
				If mLog.LogCrews.Contains(New Guid(cmbCrew.SelectedValue.ToString)) Then
					'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly)
					'msg1.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					'msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Flight Crew", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			End If
			'End
			SetObject()
			If Not CustomValidate1() Then
				Exit Sub
			End If
			Session("mLog") = mLog
			Session("mMachinePreviousRegDetailEdit") = False
			' '' ''Response.Redirect("wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))]
			DataFieldBind()
		End If
		If cmbDesignationList.SelectedIndex > 0 Then
			cmbCrew.Enabled = True
		Else
			cmbCrew.Enabled = False
		End If
		upnlErrorList.Update()
	End Sub
	Private Sub dgCrewList_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCrewList1.RowCommand
		'Dim Index As Int32 = dgCrewList1.CurrentPageIndex * dgCrewList1.PageSize + e.Item.ItemIndex
		'mRegNo = e.Item.Cells(2).Text
		'Session("mRegNo") = mRegNo
		Dim index As Integer
		Dim ID As Guid
		Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay 21-04-2023
		index = gvr.RowIndex
		Select Case e.CommandName
			Case "DeleteRec"

				If (Not User.IsInRole("LogDelete")) Then
					'MarkLog(Util.Action.Delete, "Flight Log", User.Identity.Name & " is not Authorized User to delete " & mLog.LogNo.ToString & " Dated : " & mLog.DateFormatted, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				If mLog.PilotID1.Equals(mLog.LogCrews.Item(Index).CrewID) Or mLog.PilotID2.Equals(mLog.LogCrews.Item(Index).CrewID) Then
					' '' ''Dim msg1 As New SIMsgBox(Page, "Alert ! ", "Pilot or Co-Pilot can not be deleted from Flight Crew.", "", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.Show("Alert ! ", "Pilot or Co-Pilot can not be deleted from Flight Crew.", "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
				' '' ''msg.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
				' '' ''Session("sender") = "Delete"
				' '' ''msg.Show()
				MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
				mLog.LogCrews.CurrentIndex = Index
				Session("mLog") = mLog
			Case "EditRec"
				If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then
					'MarkLog(Util.Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLog.LogNo.ToString & " Dated : " & mLog.DateFormatted, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
					'MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				mLog.LogCrews.CurrentIndex = index
				'Dim mID As New Guid(e.Item.Cells(0).Text)
				'Dim mName As String = e.Item.Cells(2).Text
				ID = New Guid(e.CommandArgument.ToString)
				mAttachToID = ID
				If mLog.PilotID1.Equals(mLog.LogCrews.Item(Index).CrewID) Or mLog.PilotID2.Equals(mLog.LogCrews.Item(Index).CrewID) Then
					' '' ''Dim msg1 As New SIMsgBox(Page, "Alert ! ", "Pilot or Co-Pilot can not be changed from Flight Crew.", "", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogFlightCrew.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
					' '' ''msg1.Show()
					MSGBoxCtrl.Show("Alert ! ", "Pilot or Co-Pilot can not be changed from Flight Crew.", "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				'mLogFlightCrewDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted + " " + lblTLPNo.Text + " : " + mLog.LogPageNoFormatted + " Name : " + mName
				'MarkLog(Util.Action.Edit, "Log Flight Crew", mLogFlightCrewDetail, Util.ErrorType.HandledError, mID, EventLogID)

				EditRecord(ID)
				'''mEmployeeList = EmployeeList.GetEmployeeList("", "", "(SELECT)")
				'''cmbCrew.DataSource = mEmployeeList
				'''dgCrewList.DataSource = mLog.LogCrews
				'''DataBind()
				Session("mMachinePreviousRegDetailEdit") = True
				Session("mAttachToID") = mAttachToID
				Session("mLog") = mLog
				ControlVisibility()
				upnlErrorList.Update()
		End Select
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		SetSession()
		RemoveSession()
		''Added By Utkash On 17-Jul-2012 FOR ALL16072012-3
		'If AppSettings("LogDetailPage") = "NewPage" Then
		'    Response.Redirect("wfLogSOP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		'Else
		'    Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		'End If
		''End
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage")) 'Added by Prashant 23-Aug-2018
	End Sub
	Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
		SetSession()
		RemoveSession()
		''Added By Utkash On 17-Jul-2012 FOR ALL16072012-3
		'If AppSettings("LogDetailPage") = "NewPage" Then
		'    Response.Redirect("wfLogSOP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		'Else
		'    Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		'End If
		''End
		If AppSettings("LogDetailPage") = "NewPage" Then
			If mLog.IsTLP = "True" Then 'Added by Prashant 23-Aug-2018
				Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")) 'Added by Prashant 23-Aug-2018
			Else
				Response.Redirect("wfLogSOP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
			End If
		Else
			Response.Redirect("wfTLP_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub

	Private Sub btnFuelOil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFuelOil.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnDefectActionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefectActionList.Click
		SetSession()
		RemoveSession()
		Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnMaintenanceAcitvity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMaintenanceAcitvity.Click
		SetSession()
		RemoveSession()
		''Added By Utkarsh ON 15-Jan-2013 FOR ALL15012013
		'If AppSettings("LogDetailPage") = "NewPage" Then
		'    Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx")
		'ElseIf mLog.IsTLP = True Then
		'    Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLP_Ajax.aspx")
		'End If
		''End
		'Added by Prashant 23-Aug-2018
		Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub

	'Added By Utkarsh ON 17-Jul-2012 FOR ALL16072012-3
	Private Sub btnLogPax_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogPax.Click
		SetSession()
		RemoveSession()
		NewLogPax()
		'Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogFlightCrew_Ajax.aspx")
		'Added by Prashant 23-Aug-2018
		Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	Private Sub btnHobbsOffset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHobbsOffset.Click
		SetSession()
		RemoveSession()
		NewHobbsOffSet()
		Response.Redirect("wfHobbsOffset_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage1=wfLogFlightCrew_Ajax.aspx")
	End Sub
	Private Sub btnParameterList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnParameterList.Click
		SetSession()
		RemoveSession()
		Response.Redirect("wfLogParameterList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
	End Sub
	'End
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
#End Region



End Class
