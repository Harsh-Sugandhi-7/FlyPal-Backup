Partial Class wfnWOCallOutJobList
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents txtFromDate As SIControls.SICalendar
	Protected WithEvents txtToDate As SIControls.SICalendar
	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Variable and Declarations "

	'Parent object and its Child collection(s)
	Public mnWO As nWO 'WO
	Public mnWOJobs As nWOJobs

	Dim SearchIndex, DateIndex, FromDate, ToDate, StatusId, WOText, Name, No As String

	Public mCallOut As Callout
	Public mCalloutList As CalloutList
	Public mQCTextList As DistinctTextListForCallout
	Dim mIsSelected As Boolean = False
#End Region

#Region " Methods "
	Private Sub GetSession()
		mnWO = CType(Session("mnWO"), nWO) 'WO)
		mCallOut = CType(Session("wfWODetail.SelectList.CallOut"), Callout)
		mCalloutList = Session("mCalloutList")
	End Sub

	Private Sub SetSession()
		Session("mnWO") = mnWO
		Session("wfWODetail.SelectList.CallOut") = mCallOut
		Session("mCalloutList") = mCalloutList
	End Sub
	Private Sub DataFieldBind()
		mCalloutList = Nothing
		dgCallOut.DataSource = Nothing
		'Get List From the Database as per Criteria             
		mCalloutList = CalloutList.GetCalloutList(, , , , , , mnWO.RegNo)
		lblResult.Text = "List of CallOut as per criteria:" & mCalloutList.Count & "Record Found."
		'Set DataSource of the Grid
		dgCallOut.DataSource = mCalloutList
		Session("mCalloutList") = mCalloutList
		cmbCallOutText.DataSource = DistinctTextListForCallout.GetDistinctTextList(1)

		If Not CType(Session("CallOutID"), Guid).Equals(Guid.Empty) Then
			ShowJobs(CType(Session("CallOutID"), Guid))
			chkAll.Visible = True
			chkAll.Checked = False
			btnDone.Visible = True
		End If

		DataBind()
	End Sub
	Private Sub ShowJobs(ByVal Id As Guid)
		mCallOut = Callout.GetCallOutForWO(Id)

		If mnWO IsNot Nothing Then
			Dim Child As CalloutJob
			For Each Child In mCallOut.CalloutJobs
				Child.IsSelect = mnWO.WOJobs.ContainsCallOutJob(Child.ID)
			Next
		End If
		dgCallOutJobs.DataSource = mCallOut.CalloutJobs
		dgCallOutJobs.DataBind()
		Session("wfWODetail.SelectList.CallOut") = mCallOut
	End Sub

	Private Sub FindNow(Optional ByVal mWOText As String = "", Optional ByVal mWONo As Integer = 0, Optional ByVal mFromDate As String = "", Optional ByVal mTodate As String = "", Optional ByVal mCustomerName As String = "", Optional ByVal mRegNo As String = "", Optional ByVal mStatusID As Integer = 0)
		'get the new list
		mCalloutList = CalloutList.GetCalloutList(mWOText, mWONo, mFromDate, mTodate, mCustomerName, mStatusID, mRegNo)
		'bind the list to the grid
		dgCallOut.DataSource = mCalloutList
		Session("mCalloutList") = mCalloutList
	End Sub
	Private Sub CallFindNow(ByVal Index As Int32)

		WOText = IIf(cmbCallOutText.SelectedIndex <= 0, "", cmbCallOutText.SelectedItem.Text)

		Select Case Index
			Case -1
				Call FindNow(, , FromDate, ToDate, , mnWO.RegNo, )
			Case 0  'All
				Call FindNow(, , FromDate, ToDate, , mnWO.RegNo, )
			Case 1  'Date.
				Call FindNow(, , txtFromDate.Value.ToString, txtToDate.Value.ToString, , mnWO.RegNo, )
			Case 2  'QCCallOut
				Call FindNow(WOText, Val(No), FromDate, ToDate, , mnWO.RegNo, )
				'Case 4 ' Aircraft
				''    Call FindNow(, , "1/1/1900", "1/1/2200", "", mnWO.RegNo, )
			Case 3  ' Customer Name / Vendor Name
				Call FindNow("", 0, "1/1/1900", "1/1/2200", Name, mnWO.RegNo, 0)
			Case 4  'Status
				Call FindNow("", 0, "1/1/1900", "1/1/2200", "", mnWO.RegNo, CInt(StatusId))
		End Select
	End Sub
	Private Sub setPeriod(ByVal Index As Int32)
		Select Case Index
			Case 0 ' All   
				txtFromDate.Value = "1-Jan-1900"
				txtToDate.Value = "1-Jan-2200"
			Case 1 'Last Week
				txtFromDate.Value = Today.AddDays(-6)
				txtToDate.Value = Today.Date
			Case 2 'Last Month
				txtFromDate.Value = Today.AddDays(1).AddMonths(-1)
				txtToDate.Value = Today.Date
			Case 3 'Last Quater
				Select Case Today.Month
					Case 1, 2, 3
						txtFromDate.Value = CDate("01-Oct-" + CStr(Today.Year - 1))
						txtToDate.Value = CDate("31-Dec-" + CStr(Today.Year - 1))
					Case 4, 5, 6
						txtFromDate.Value = CDate("01-Jan-" + CStr(Today.Year))
						txtToDate.Value = CDate("31-Mar-" + CStr(Today.Year))
					Case 7, 8, 9
						txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))
						txtToDate.Value = CDate("30-Jun-" + CStr(Today.Year))
					Case 10, 11, 12
						txtFromDate.Value = CDate("01-Jul-" + CStr(Today.Year))
						txtToDate.Value = CDate("30-Sep-" + CStr(Today.Year))
				End Select
			Case 4 'Last Year
				txtFromDate.Value = Today.AddDays(1).AddYears(-1)
				txtToDate.Value = Today.Date
			Case 5 'Current Financial Year
				'Dim Month As Integer
				'Month = Today.Month
				If Today.Month <= 3 Then  'Jan|Feb|Mar
					txtFromDate.Value = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year))
				Else
					txtFromDate.Value = CDate("01-Apr-" + CStr(Today.Year))
				End If
				txtToDate.Value = Today.Date
			Case 6 'Between Dates
				txtFromDate.Value = Today.Date
				txtToDate.Value = Today.Date
		End Select
	End Sub
	Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
		cmbDate.Visible = IIf(SearchIndex = 1, True, False)
		lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		''txtFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		''txtToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		''calFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)
		''calToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0 And DateIndex = 6, True, False)

		'Added by Saylee on 20-June 2007**************
		If SearchIndex = 1 And DateIndex = 6 Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = True
			txtToDate.Enabled = True
		ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = False
			txtToDate.Enabled = False
		Else
			txtFromDate.Visible = False
			txtToDate.Visible = False
		End If
		'**********************************************
		cmbCallOutText.Visible = IIf(SearchIndex = 2, True, False)
		lblNo.Visible = IIf((SearchIndex = 2) And (cmbCallOutText.SelectedIndex <> 0), True, False)
		txtNo.Visible = IIf((SearchIndex = 2) And (cmbCallOutText.SelectedIndex <> 0), True, False)
		txtName.Visible = IIf(SearchIndex = 3 Or SearchIndex = 4, True, False)
		cmbStatus.Visible = IIf(SearchIndex = 5, True, False)
	End Sub
	Private Sub SetPage()
		lblResult.Text = "List of CallOut as per criteria:" & mCalloutList.Count & "Record Found."
	End Sub
	Private Sub ClearControls()
		txtNo.Text = ""
		txtName.Text = ""
	End Sub
	Private Sub SetControl()
		setPeriod(DateIndex)
		CallFindNow(SearchIndex)
		dgCallOut.DataBind()
		cmbSearch.SelectedIndex = SearchIndex
		cmbDate.SelectedIndex = DateIndex
		cmbStatus.SelectedValue = StatusId
		cmbCallOutText.SelectedValue = IIf(WOText = "", "All", WOText)
		txtName.Text = Name
		txtNo.Text = No
		ControlVisibility(SearchIndex, DateIndex)
		lblResult.Text = "List of CallOut as per criteria :" & mCalloutList.Count & " Record(s) found."
	End Sub
	Private Sub setVariables()
		SearchIndex = IIf(cmbSearch.SelectedIndex < 0, 0, cmbSearch.SelectedIndex)
		DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Value.ToString <> "", txtFromDate.Value.ToString, "01/01/1900")
		ToDate = IIf(txtToDate.Value.ToString <> "", txtToDate.Value.ToString, "01/01/2050")
		StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
		WOText = IIf(cmbCallOutText.SelectedIndex <= 0, "", cmbCallOutText.SelectedValue)
		Name = txtName.Text.Trim
		No = txtNo.Text.Trim
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("StatusId") = StatusId
		Session("WOText") = WOText
		Session("No") = No
		Session("Name") = Name
	End Sub
	Private Sub AddCalloutJobs()
		If Session("wfWODetail.SelectList.CallOut") IsNot Nothing Then
			Dim mCallout As Callout = Session("wfWODetail.SelectList.CallOut")

			If mnWO.RegNo = "" Then
				mnWO.RegNo = mCallout.RegNo
				mnWO.MachineID = mCallout.MachineID
				mnWO.ModelName = mCallout.MachineModelNo
				mnWO.SerialNo = mCallout.MachineSerialNo
			End If

			Dim mCalloutJob As CalloutJob

			For Each mCalloutJob In mCallout.CalloutJobs
				If mCalloutJob.IsSelect Then
					mnWO.CallOutID = mCallout.ID
					mIsSelected = True
					If Not mnWO.WOJobs.ContainsCallOutJob(mCalloutJob.ID) Then
						'   mnWO.WOJobs.Add(nWOJob.NewWOJobChil(mnWO.ID, mCalloutJob.ID, mnWO.WOJobs.Count + 1, mCalloutJob.JobDescription, mCalloutJob.JobAction, New SmartDate(mCalloutJob.StartDate.ToString).FormattedText, New SmartDate(mCalloutJob.CompletionDate.ToString).FormattedText, mCalloutJob.UsedHours))
						mnWO.WOJobs.Add(mnWO.ID, Val(Session("WOJobTypeID")))

						mnWO.WOJobs.CurrentItem.CallOutJobID = mCalloutJob.ID
						mnWO.WOJobs.CurrentItem.WOJobDescription = mCalloutJob.JobDescription
						mnWO.WOJobs.CurrentItem.WOJobAction = mCalloutJob.JobAction
						' mnWO.WOJobs.CurrentItem.WOJobStartDate = New SmartDate(mCalloutJob.StartDate.ToString).FormattedText
						' mnWO.WOJobs.CurrentItem.WOJobCloseDate = New SmartDate(mCalloutJob.CompletionDate.ToString).FormattedText
						mnWO.WOJobs.CurrentItem.WOJobActualTime = mCalloutJob.UsedHours()

						'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
						'TASK(s):
						Dim mMaintenanceTask As MaintenanceTask
						Dim mMaintenanceTaskDetail As MaintenanceTaskDetail

						If mCalloutJob.OnTypeID = 1 Then        'Assembly
							mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(mCalloutJob.MonitorTypeID, mCalloutJob.BeforeDataID, True)
						ElseIf mCalloutJob.OnTypeID = 2 Then    'Componant
							mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(mCalloutJob.MonitorTypeID, mCalloutJob.BeforeDataID, False)
						End If

						If mCalloutJob.OnTypeID = 1 Or mCalloutJob.OnTypeID = 2 Then
							For Each mMaintenanceTaskDetail In mMaintenanceTask.MaintenanceTaskDetails
								mnWO.WOJobs.CurrentItem.WOJobTasks.Add(mnWO.WOJobs.CurrentItem.ID)

								With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem
									.TaskAction = mMaintenanceTaskDetail.Task
									.ActualStartDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
									.ActualEndDate = mnWO.WOJobs.CurrentItem.WOJobCloseDate
									.IsDone = False
									.TaskCardID = mMaintenanceTaskDetail.TaskCardID
								End With
							Next
						End If

						'KIT(s):
						Dim mMaintenanceKit As MaintenanceKit
						Dim mMaintenanceKitDetail As MaintenanceKitDetail

						If mCalloutJob.OnTypeID = 1 Then        'Assembly
							mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(mCalloutJob.MonitorTypeID, mCalloutJob.BeforeDataID, True)
						ElseIf mCalloutJob.OnTypeID = 2 Then    'Componant
							mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(mCalloutJob.MonitorTypeID, mCalloutJob.BeforeDataID, False)
						End If

						If mCalloutJob.OnTypeID = 1 Or mCalloutJob.OnTypeID = 2 Then
							For Each mMaintenanceKitDetail In mMaintenanceKit.MaintenanceKitDetails
								mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

								With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
									.ItemID = mMaintenanceKitDetail.ItemID
									.RequiredQty = mMaintenanceKitDetail.Qty
								End With
							Next
						End If
						'-----------------------------------------------------------------------
					End If
				End If
			Next
		End If
		Session("mnWO") = mnWO
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack And Session("sender") = "" Then
			DataFieldBind()
			SetSession()
		End If
	End Sub

	Private Sub dgCallOut_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCallOut.ItemCommand
		Dim mId As New Guid(e.Item.Cells(0).Text)
		Session("CallOutID") = mId
		Select Case e.CommandName
			Case "Select"
				ShowJobs(mId)
				chkAll.Visible = True
				chkAll.Checked = False
				btnDone.Visible = True
		End Select
	End Sub

	Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
		GetSession()
		Dim item As DataGridItem
		Dim chkBox As CheckBox
		Dim Recordno, PageItems As Integer
		Dim i As Integer
		PageItems = dgCallOutJobs.Items.Count - 1
		'Set Selected Notes value  
		For i = 0 To PageItems
			Recordno = i + dgCallOutJobs.PageSize * dgCallOutJobs.CurrentPageIndex
			item = dgCallOutJobs.Items(i)
			chkBox = CType(item.FindControl("chkSelect"), CheckBox)
			mCallOut.CalloutJobs(Recordno).IsSelect = chkBox.Checked

			If mCallOut.CalloutJobs(Recordno).IsSelect = False Then
				If mnWO.WOJobs.ContainsCallOutJob(mCallOut.CalloutJobs.Item(i).ID) Then
					mnWO.WOJobs.Remove(mCallOut.CalloutJobs.Item(i).ID, "", "")
				End If
			End If
		Next
		Session("wfWODetail.SelectList.CallOut") = mCallOut
		AddCalloutJobs()
		If mIsSelected = False And dgCallOutJobs.Items.Count <> 0 Then
			Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SelectAtleastOne, SIMsgBox.Message_text.SelectAtleastOne, "Please select atleast one callout job", MsgBoxStyle.OKOnly)
			msg.ReplacePage = "wfnWOCallOutJobList.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1")
			msg.Show()
			Exit Sub
		Else
			Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub

	Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
		cmbDate.SelectedIndex = 0
		cmbCallOutText.SelectedIndex = 0
		ClearControls()
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
		ControlVisibility(CInt(cmbSearch.SelectedValue), DateIndex)
		setPeriod(DateIndex)
	End Sub
	Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		setVariables()
		CallFindNow(SearchIndex)
		dgCallOut.DataBind()

		chkAll.Visible = False
		chkAll.Checked = False
		btnDone.Visible = False
		dgCallOutJobs.DataSource = Nothing
		dgCallOutJobs.DataBind()

		lblResult.Text = "List of CallOut as per criteria :" & mCalloutList.Count & " Record(s) found."
	End Sub
	Private Sub cmbCallOutText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCallOutText.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub
	Private Sub chkAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
		If mCallOut.CalloutJobs.Count > 0 Then
			chkAll.Visible = True
			btnDone.Visible = True
		End If

		If chkAll.Checked = True Then
			' Set Selected Notes value  
			If mCalloutList IsNot Nothing Then
				For Each Child As CalloutJob In mCallOut.CalloutJobs
					Child.IsSelect = True
				Next
			End If
			dgCallOutJobs.DataSource = mCallOut.CalloutJobs
			dgCallOutJobs.DataBind()
		Else
			If mCalloutList IsNot Nothing Then
				For Each Child As CalloutJob In mCallOut.CalloutJobs
					Child.IsSelect = False
				Next
			End If
			dgCallOutJobs.DataSource = mCallOut.CalloutJobs
			dgCallOutJobs.DataBind()
		End If
	End Sub
#End Region


End Class
