'Created By Prashant On 24-May-2019
Public Class wfnWOExecutionList
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mWOJobStatusList As nWOJobStatusList
	Public mWOJobList As nWOJobList
	Public mWOJobTypeList As nWOJobTypeList
	Public mnWOJob As nWOJob
	Public mnWO As nWO
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, WOJobTypeID, No, WOJobStatusID As String
	Dim EventLogID As Guid
	Dim mWODetail As String
	Dim totcnt As Integer
	Dim ShowCompletedJobs As Boolean = False
	Public mWOForAMECompletion As WOForAMECompletion
	Dim ForJobs As Boolean = True
	Dim AMECompletion As Boolean = False
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mWOJobStatusList = Session("mWOJobStatusList")
		mDistinctWOText = Session("mDistinctWOText")
		WOText = Session("WOText")
		No = IIf(IsNothing(Session("No")), 0, Session("No"))
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		SearchIndex = Session("SearchIndex")
		DateIndex = Session("DateIndex")
		WOJobTypeID = Session("WOJobTypeIDFromWOExecutionList")
		WOJobStatusID = Session("WOJobStatusID")
		mWOJobTypeList = Session("mWOJobTypeList")
		mWOJobList = Session("mWOJobList")
		mnWOJob = Session("mnWOJob")
		mnWO = Session("mnWO")
		totcnt = Session("totcnt")
		ShowCompletedJobs = Session("ShowCompletedJobs")
		mWOForAMECompletion = Session("mWOForAMECompletion")
		ForJobs = Session("ForJobs")
		AMECompletion = Session("AMECompletion")
	End Sub
	Private Sub SetSession()
		Session("mWOJobStatusList") = mWOJobStatusList
		Session("mWOJobList") = mWOJobList
		Session("mDistinctWOText") = mDistinctWOText
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("WOJobTypeIDFromWOExecutionList") = WOJobTypeID
		Session("WOJobStatusID") = WOJobStatusID
		Session("No") = No
		Session("WOText") = WOText
		Session("mWOJobTypeList") = mWOJobTypeList
		Session("mWOJobList") = mWOJobList
		Session("mnWOJob") = mnWOJob
		Session("mnWO") = mnWO
		Session("ShowCompletedJobs") = ShowCompletedJobs
		Session("mWOForAMECompletion") = mWOForAMECompletion
		Session("ForJobs") = ForJobs
		Session("AMECompletion") = AMECompletion
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mWOJobStatusList")
		Session.Remove("mWOJobList")
		Session.Remove("mDistinctWOText")
		Session.Remove("FromDate")
		Session.Remove("ToDate")
		Session.Remove("SearchIndex")
		Session.Remove("DateIndex")
		Session.Remove("WOJobTypeID")
		Session.Remove("WOJobStatusID")
		Session.Remove("No")
		Session.Remove("WOText")
		Session.Remove("mWOJobTypeList")
		Session.Remove("mWOJobList")
		Session.Remove("mnWOJob")
		Session.Remove("mnWO")
		Session.Remove("totcnt")
		Session.Remove("ShowCompletedJobs")
		Session.Remove("mWOForAMECompletion")
		Session.Remove("ForJobs")
		Session.Remove("AMECompletion")
	End Sub
	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "wfnWOExecutionList.aspx") <= 0 Then
			RemoveSession()
			Session.Remove("mWOJobList")
		End If
	End Sub
	Private Sub EditRecord(ByVal mId As Guid, ByVal mWOID As Guid)
		mnWO = nWO.GetWO(mWOID, False)
		If mnWO.WOJobs.Count <> 0 Then
			mnWOJob = mnWO.WOJobs.Item(mId)
			mnWO.WOJobs.CurrentIndex = mnWO.WOJobs.IndexOfItem(mId)
			Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
		ElseIf mnWO.WONRCJobs.Count <> 0 Then
			mnWOJob = mnWO.WONRCJobs.Item(mId)
			mnWO.WONRCJobs.CurrentIndex = mnWO.WONRCJobs.IndexOfItem(mId)
			Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
			Session("mnWO") = mnWO
		End If
		Session("mnWOJob") = mnWOJob
		Session("mnWO") = mnWO
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub setPeriod(ByVal Index As Int32)
		Select Case Index
			Case 0 'All'
				txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
				txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
			Case 1 'Last 1 Week
				txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 2 'Last 1 Month
				txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 3 'Last 1 Quater
				Select Case Today.Month
					Case 1, 2, 3
						txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
						txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
					Case 4, 5, 6
						txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
						txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
					Case 7, 8, 9
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
						txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
					Case 10, 11, 12
						txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
						txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
				End Select
			Case 4 'Last 1 Year
				txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 5 'Current Financial Year
				If Today.Month <= 3 Then  'Jan|Feb|Mar
					txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
				Else
					txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
				End If
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 6 'Between Dates
				FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
				ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
				txtFromDate.Text = FromDate
				txtToDate.Text = ToDate
		End Select
	End Sub
	Private Sub setVariables()
		'SearchIndex = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
		DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
		WOJobTypeID = IIf(cmbWOJobType.SelectedIndex <= 0, 0, cmbWOJobType.SelectedValue)
		ShowCompletedJobs = IIf(chkShowCompletedJobs.Checked, True, False)
		ForJobs = IIf(rbForJobs.Checked, True, False)
		AMECompletion = IIf(rbAMECompletion.Checked, True, False)
		No = txtNo.Text.Trim

		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("WOJobTypeIDFromWOExecutionList") = WOJobTypeID
		Session("WOJobStatusID") = WOJobStatusID
		Session("No") = No
		Session("WOText") = WOText
		Session("ShowCompletedJobs") = ShowCompletedJobs
		Session("ForJobs") = ForJobs
		Session("AMECompletion") = AMECompletion
	End Sub
	Private Sub SetControl()
		setPeriod(DateIndex)
		CallFindNow(SearchIndex)
		dgWOJobList.DataBind()
		'cmbSearch.SelectedIndex = SearchIndex
		cmbDate.SelectedIndex = DateIndex
		''cmbWO.SelectedValue = WOText
		cmbWO.SelectedValue = IIf(WOText = "", "(All)", WOText)
		cmbWOJobType.SelectedIndex = WOJobTypeID
		chkShowCompletedJobs.Checked = ShowCompletedJobs

		If Session("ForJobs") Is Nothing Then
			rbForJobs.Checked = True
		Else
			rbForJobs.Checked = ForJobs
		End If

		rbAMECompletion.Checked = AMECompletion

		txtNo.Text = No

		ControlVisibility(DateIndex)

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOJobList.Columns(3).HeaderText = "E.O. No."
			dgWOJobList.DataBind()
			lblResult.Text = "List for Engineering Order Jobs Execution as per criteria :" & mWOJobList.Count & " Record(s) found."
		Else
			dgWOJobList.Columns(3).HeaderText = "W.O. No."

			dgWOJobList.DataBind()
			lblResult.Text = "List for Work Order Jobs Execution as per criteria :" & mWOJobList.Count & " Record(s) found."
		End If
		lblResultAMECompletion.Text = "List of Work Order as per criteria for AME Completion : " & mWOForAMECompletion.Count & " Record(s) found."

	End Sub
	Private Sub SetTitle()
		'Dim mWOJobList As nWOJobList
		'mWOJobList = nWOJobList.GetWOJobList()
		'totcnt = Session("totcnt")
		If (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblTitle.Text = "List for Engineering Order Jobs Execution" ' [Total No of Record(s):-" + totcnt.ToString() + "]"  'shweta
		Else
			lblTitle.Text = "List for Work Order Jobs Execution" ' [Total No of Record(s):-" + totcnt.ToString() + "]"  'shweta
		End If

	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub
	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "1/1/1900", _
						Optional ByVal ToDate As String = "1/1/2200", Optional ByVal WOJobStatusID As Integer = 0, _
						Optional ByVal WOJobTypeID As Integer = 0, Optional ByVal ShowCompletedJobs As Boolean = False)
		mWOJobList = Nothing
		dgWOJobList.DataSource = Nothing

		mWOJobList = nWOJobList.GetWOJobList(Text, No, FromDate, ToDate, WOJobStatusID, WOJobTypeID, ShowCompletedJobs, ShowPlannedWOOnly:=True, _
											 IsForWOExecutionList:=True)
		dgWOJobList.DataSource = mWOJobList

		Session("mWOJobList") = mWOJobList

		mWOForAMECompletion = WOForAMECompletion.GetWOListForAMECompletion(Text, No, FromDate, ToDate, WOJobStatusID, WOJobTypeID, ShowCompletedJobs, ShowPlannedWOOnly:=True, _
											 IsForWOExecutionList:=True)
		dgWOForAMECompletion.DataSource = mWOForAMECompletion
		Session("mWOForAMECompletion") = mWOForAMECompletion
	End Sub
	Private Sub CallFindNow(ByVal Index As Integer)
		FindNow(WOText, CInt(Val(No)), txtFromDate.Text.ToString, txtToDate.Text.ToString, , WOJobTypeID, ShowCompletedJobs)
		dgWOJobList.PageIndex = 0
	End Sub
	Private Sub ClearControls()
		txtNo.Text = ""
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim TempWOID As Guid

		Dim msgCount As Integer = 0
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							Dim mnWO As nWO
							Session("sender") = ""
							mnWO = CType(Session("mnWO"), nWO)
							TempWOID = mnWO.ID
							If ((AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer") Then
								If (mnWO.IsSync = 1 Or mnWO.IsSync = 2) Then
									'Dim msg1 As New SIMsgBox(Page, "Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OKOnly)
									'msg1.ReplacePage = "wfIssueList.aspx?BackPage=" & Request.QueryString("BackPage")
									'msg1.Show()
									MSGBoxCtrl.show("Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								Else
									mnWO.Delete()
									mnWO.Save()
									DataFieldBind()
									SetControl()
									SetTitle()
									upnlGridView.Update()
									upnlActionBtnTop.Update()
									upnlActionBtnBottom.Update()
									upnlResult.Update()
								End If
							Else
								mnWO.Delete()
								mnWO.Save()
								DataFieldBind()
								SetControl()
								SetTitle()
								upnlGridView.Update()
								upnlActionBtnTop.Update()
								upnlActionBtnBottom.Update()
								upnlResult.Update()
							End If
						Catch ex As SqlException
							If ex.Number = 8145 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 547 Then
								MarkLog(Util.Action.Delete, "Work Order", "Can't delete : " & mWODetail & " is Currently in use", Util.ErrorType.NoError, TempWOID, EventLogID)
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
							End If
							DataFieldBind()
							SetControl()
							upnlResult.Update()
							msgCount = ex.Errors.Count
						Finally
							If msgCount = 0 Then
								' MarkLog(Util.Action.Delete, , mIssue.IssueNo, Util.ErrorType.NoError, mIssue.ID)
							End If
						End Try
					End If
				Case MsgBoxResult.No
					Session("sender") = ""
				Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
					Session("sender") = ""
					'  DataFieldBind()
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					'  DataFieldBind()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""
			' DataFieldBind()
		End If
	End Sub
	Public Sub SetToolTip()
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblTitle.Text = "List of Engineering Order Jobs"
			btnClose.ToolTip = "Click to close List of Engineering Order Job screen"
			btnCloseTop.ToolTip = "Click to close List of Engineering Job Order screen"
			btnFindNow.ToolTip = "Click to find list of Engineering Order Jobs as per searching criteria"
		Else
			lblTitle.Text = "List of Work Order Jobs"
			btnClose.ToolTip = "Click to close List of Work Order screen"
			btnCloseTop.ToolTip = "Click to close List of Work Order screen"
			btnFindNow.ToolTip = "Click to find list of Work Order Jobs as per searching criteria"
		End If
	End Sub
	Private Sub ControlVisibility(Optional ByVal PeriodIndex As Int32 = 0)
		If PeriodIndex = 6 Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = True
			txtToDate.Enabled = True
			lblFromDate.Visible = True
			lblToDate.Visible = True
		ElseIf PeriodIndex = 1 Or PeriodIndex = 2 Or PeriodIndex = 3 Or PeriodIndex = 4 Or PeriodIndex = 5 Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = False
			txtToDate.Enabled = False
			lblFromDate.Visible = True
			lblToDate.Visible = True
		Else
			txtFromDate.Visible = False
			txtToDate.Visible = False
			lblFromDate.Visible = False
			lblToDate.Visible = False
		End If
		txtNo.Visible = IIf(cmbWO.SelectedIndex = 0, False, True)
	End Sub
	Private Sub Visibility()
		If rbForJobs.Checked = True Then
			dgWOJobList.Visible = True
			lblResult.Visible = True
			dgWOForAMECompletion.Visible = False
			lblResultAMECompletion.Visible = False
		ElseIf rbAMECompletion.Checked = True Then
			dgWOJobList.Visible = False
			lblResult.Visible = False
			dgWOForAMECompletion.Visible = True
			lblResultAMECompletion.Visible = True
			upnlGridView.Update()
			upnlWOForAMECompletion.Update()
		End If
		upnlGridView.Update()
		upnlWOForAMECompletion.Update()
	End Sub
#End Region

#Region " DataFieldBind "
	Private Sub DataFieldBind()
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		DateIndex = IIf(IsNothing(DateIndex), 0, DateIndex)
		WOJobTypeID = Session("WOJobTypeIDFromWOExecutionList")
		WOJobStatusID = Session("WOJobStatusID")
		WOText = Session("WOText")

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
		cmbWO.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText

		mWOJobList = nWOJobList.GetWOJobList(, , , , , , ShowCompletedJobs:=ShowCompletedJobs, ShowPlannedWOOnly:=True, IsForWOExecutionList:=True)

		totcnt = mWOJobList.Count 'Added by shweta on 11-1-12
		Session("totcnt") = totcnt 'Added by shweta on 11-1-12

		dgWOJobList.DataSource = mWOJobList
		Session("mWOJobList") = mWOJobList

		mWOJobTypeList = nWOJobTypeList.GetWOJobTypeList("(All)")
		cmbWOJobType.DataSource = mWOJobTypeList
		Session("mWOJobTypeList") = mWOJobTypeList

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblResult.Text = "List of Engineering Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		Else
			lblResult.Text = "List of Work Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		End If

		'Added by Prashant 16-Aug-2019
		mWOForAMECompletion = WOForAMECompletion.GetWOListForAMECompletion(, , , , , , ShowCompletedJobs:=ShowCompletedJobs, ShowPlannedWOOnly:=True, IsForWOExecutionList:=True)
		dgWOForAMECompletion.DataSource = mWOForAMECompletion
		Session("mWOForAMECompletion") = mWOForAMECompletion
		lblResultAMECompletion.Text = "List of Work Order as per criteria for AME Completion : " & mWOForAMECompletion.Count & " Record(s) found."
		DataBind()

	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		ClearAll()
		addAttributes()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack Then
			Session("MiddleFrame") = "wfnWOExecutionList.aspx"
			DataFieldBind()
			'ControlVisibility(PeriodIndex:=1)
			SetControl()
		End If
		SetToolTip()
		SetTitle()
		Visibility()
	End Sub
	Private Sub dgWOJobList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOJobList.PageIndexChanging
		dgWOJobList.PageIndex = e.NewPageIndex
		dgWOJobList.DataSource = mWOJobList
		Session("mWOJobList") = mWOJobList
		dgWOJobList.DataBind()
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		setVariables()
		CallFindNow(SearchIndex)
		dgWOJobList.DataBind()

		dgWOForAMECompletion.DataBind() 'Added by Prashant 16-Aug-2019

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblResult.Text = "List of Engineering Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		Else
			lblResult.Text = "List of Work Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		End If
		lblResultAMECompletion.Text = "List of Work Order as per criteria for AME Completion : " & mWOForAMECompletion.Count & " Record(s) found."
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
		upnlWOForAMECompletion.Update()
	End Sub
	'Private Sub cmbWO_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWO.SelectedIndexChanged
	'    ClearControls()
	'    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
	'    setPeriod(DateIndex)
	'    If cmbWO.Enabled = True Then
	'        setFocus(cmbWO)
	'    End If

	'    ''FindNow
	'    setVariables()
	'    CallFindNow(SearchIndex)
	'    dgWOJobList.DataBind()

	'    If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
	'        lblResult.Text = "List of Engineering Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
	'    Else
	'        lblResult.Text = "List of Work Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
	'    End If
	'    upnlGridView.Update()
	'    upnlActionBtnTop.Update()
	'    upnlActionBtnBottom.Update()
	'    upnlResult.Update()
	'    '------------------------------------
	'End Sub
	Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
		ClearControls()
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(PeriodIndex:=DateIndex)
		setPeriod(DateIndex)

		'If cmbDate.Enabled = True Then
		'    setFocus(cmbDate)
		'End If
		'''FindNow
		'setVariables()
		'CallFindNow(SearchIndex)
		'dgWOJobList.DataBind()

		'If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
		'    lblResult.Text = "List of Engineering Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		'Else
		'    lblResult.Text = "List of Work Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		'End If
		'upnlGridView.Update()
		'upnlActionBtnTop.Update()
		'upnlActionBtnBottom.Update()
		'upnlResult.Update()
		'------------------------------------
	End Sub
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgWOJobList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOJobList.RowCommand
		Select Case e.CommandName
			Case "ExecuteWOJob"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobList.PageSize * dgWOJobList.PageIndex
				Dim mJobSatatus As String = mWOJobList(Index).WOJobStatusName
				'If mJobSatatus = "Complete" Then
				'    MSGBoxCtrl.show("Alert!", "Can not executed as it is alredy completed ", "", MsgBoxStyle.OkOnly, "")
				'    Exit Sub
				'End If
				Dim mId As Guid = mWOJobList(Index).ID
				Dim mWOID As Guid = mWOJobList(Index).WOID
				Dim mDate As String = mWOJobList(Index).WODateFormatted
				Dim mWorkOrderNo As String = mWOJobList(Index).WONumber
				Dim mDescription As String = mWOJobList(Index).WOJobDescription
				Dim mAction As String = mWOJobList(Index).WOJobAction
				Dim mJobType As String = mWOJobList(Index).WOJobType

				mWODetail = mWorkOrderNo + " Dated : " + mDate + " Description : " + mDescription + " Action : " + mAction + " Job Type : " + mJobType + " Job status : " + mJobSatatus
				MarkLog(Util.Action.Edit, "Work Order Job", mWODetail, Util.ErrorType.NoError, mId, EventLogID)
				'EditRecord(mId, mWOID)
				mnWO = nWO.GetWO(mWOID, False)
				'IF Condition And case added by Shital on 13-Mar-2020 for OJS WO.
				'If mWOJobList(Index).WOJobTypeID = "5" Then
				If mWOJobList(Index).WOJobTypeID = "5" And (Not mnWO.TransTypeID = 90 And Not mnWO.TransTypeID = 91) Then
					'mnWO = nWO.GetWO(mWOID, False)
					mnWOJob = mnWO.WONRCJobs.Item(mId)
					mnWO.WONRCJobs.CurrentIndex = mnWO.WONRCJobs.IndexOfItem(mId, mWOID)
					Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID
					Session("mnWOJob") = mnWOJob
					Session("mnWO") = mnWO
					Dim str1 As String = "openledgersame('wfnWONRC.aspx?BackPage1=index.aspx&BackPage=" & Request.QueryString("BackPage") & "');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str1, True)
				Else
					' mnWO = nWO.GetWO(mWOID, False)
					mnWOJob = mnWO.WOJobs.Item(mId)
					mnWO.WOJobs.CurrentIndex = mnWO.WOJobs.IndexOfItem(mId, mWOID)
					Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
					Session("mnWOJob") = mnWOJob
					Session("mnWO") = mnWO
					Dim str As String
					str = "openledgersame('wfnWOJobDetail.aspx?BackPage1=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				End If
			Case "ViewRec"
				Dim mFileAttachments As New FileAttachments

				Dim mWOID As Guid = New Guid(e.CommandArgument.ToString)

				mnWO = nWO.GetWO(mWOID)

				mFileAttachments = FileAttachments.GetChildFileAttachments(mnWO.ID)

				Dim AttachmentCount As Integer = mFileAttachments.Count
				DataFieldBind()
				SetControl()
				SetTitle()
				upnlGridView.Update()
				upnlActionBtnTop.Update()
				upnlActionBtnBottom.Update()
				upnlResult.Update()
				Session("mnWO") = mnWO
				If AttachmentCount > 1 Then
					Session("mFileAttachments") = mFileAttachments
					Session("TransactionNameMarkLog") = "Work Order" 'used for marklog
					Session("TransactionName") = "Work Order No. & Date"
					Session("TransactionDetails") = mnWO.WONumber + " & " + mnWO.WODateFormatted.ToString
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)
				Else
					Dim mFileAttach As FileAttach
					Dim No As New Random
					Dim StrName As String = "abc" & No.Next.ToString
					mFileAttach = FileAttach.GetAttachment(mWOID, , mnWO.FileAttachments(0).FileName)
					If mFileAttach.Size > 0 Then
						Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
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
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
							Dim Detail As String = "Work Order Attachment( " + mFileAttach.FileName + ") viewed by  " + User.Identity.Name
							MarkLog(Util.Action.View, "Work Order", Detail, Util.ErrorType.HandledError, mWOID, EventLogID)
						End If
					End If
				End If
		End Select
	End Sub
	Private Sub dgWOJobList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOJobList.Sorting
		mWOJobList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOJobList.DataSource = mWOJobList
		Session("mWOJobList") = mWOJobList
		dgWOJobList.DataBind()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	'Private Sub cmbWOJobType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbWOJobType.SelectedIndexChanged, txtFromDate.TextChanged, txtToDate.TextChanged, txtNo.TextChanged
	'    ''FindNow
	'    setVariables()
	'    CallFindNow(SearchIndex)
	'    dgWOJobList.DataBind()

	'    If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
	'        lblResult.Text = "List of Engineering Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
	'    Else
	'        lblResult.Text = "List of Work Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
	'    End If
	'    upnlGridView.Update()
	'    upnlActionBtnTop.Update()
	'    upnlActionBtnBottom.Update()
	'    upnlResult.Update()
	'    '------------------------------------
	'End Sub
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomPrint.Click, btnPrintTop.Click
		Dim mCompanyDetail As New CompanyDetail
		Dim SearchStr1 As String
		Dim SearchStr2 As String

		Dim Rpt As New crnWOJobList
		Dim da As New CSLA.Data.ObjectAdapter
		Dim ds As New dsWOJobList
		Dim ReportDetails As New rptStatusList

		setVariables()
		CallFindNow(SearchIndex)
		SearchStr1 = "The report shows all records till date."
		SearchStr2 = ""

		'Date
		SearchStr1 = "The report shows records filtered by the following criteria"
		'If cmbDate.SelectedIndex = 0 Then
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
		'ElseIf cmbDate.SelectedIndex = 6 Then
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + "    " + lblFromDate.Text + ":" + "  " + New SmartDate(txtFromDate.Text).FormattedText + "    " + lblToDate.Text + ":" + " " + New SmartDate(txtToDate.Text).FormattedText
		'Else
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + "    " + lblFromDate.Text + ":" + " " + New SmartDate(txtFromDate.Text).FormattedText + "    " + lblToDate.Text + ":" + " " + New SmartDate(txtToDate.Text).FormattedText
		'End If
		''WO No.
		'SearchStr1 = "The report shows records filtered by the following criteria"
		'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbWO.SelectedItem.Text + "    " + lblNo.Text + " " + txtNo.Text
		''Status
		'SearchStr1 = "The report shows records filtered by the following criteria"
		'SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbWOJobType.SelectedItem.Text

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
		mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
		mCompanyDetail.WebSite, "", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))


		If mWOJobList.Count = 0 Then
			MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, mrptImage)
		da.Fill(ds, mWOJobList)
		da.Fill(ds, Report)
		Rpt.SetDataSource(ds)
		Session("CrystalReport") = Rpt
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
	End Sub
	Private Sub cmbWO_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbWO.SelectedIndexChanged
		If cmbWO.SelectedIndex = 0 Then
			txtNo.Visible = False
		Else
			txtNo.Visible = True
		End If
		upnlSearchCriteria.Update()
	End Sub
	'Added By Prashant 16-Aug-2019
	Private Sub rbForJobs_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbForJobs.CheckedChanged
		dgWOJobList.Visible = True
		lblResult.Visible = True
		dgWOForAMECompletion.Visible = False
		lblResultAMECompletion.Visible = False
		btnPrintTop.Visible = True
		ForJobs = IIf(rbForJobs.Checked, True, False)
		AMECompletion = IIf(rbAMECompletion.Checked, True, False)
		Session("ForJobs") = ForJobs
		Session("AMECompletion") = AMECompletion

		upnlGridView.Update()
		upnlWOForAMECompletion.Update()
		upnlResult.Update()
		upnlActionBtnTop.Update()
	End Sub
	Private Sub rbAMECompletion_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbAMECompletion.CheckedChanged
		dgWOJobList.Visible = False
		lblResult.Visible = False
		dgWOForAMECompletion.Visible = True
		lblResultAMECompletion.Visible = True
		btnPrintTop.Visible = False

		ForJobs = IIf(rbForJobs.Checked, True, False)
		AMECompletion = IIf(rbAMECompletion.Checked, True, False)
		Session("ForJobs") = ForJobs
		Session("AMECompletion") = AMECompletion

		upnlGridView.Update()
		upnlWOForAMECompletion.Update()
		upnlResult.Update()
		upnlActionBtnTop.Update()
	End Sub
	Private Sub dgWOForAMECompletion_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOForAMECompletion.PageIndexChanging
		dgWOForAMECompletion.PageIndex = e.NewPageIndex
		dgWOForAMECompletion.DataSource = mWOForAMECompletion
		Session("mWOForAMECompletion") = mWOForAMECompletion
		dgWOForAMECompletion.DataBind()
		upnlWOForAMECompletion.Update()
	End Sub
	Private Sub dgWOForAMECompletion_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOForAMECompletion.RowCommand
		Select Case e.CommandName
			Case "CallWOForAMECompletion"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOForAMECompletion.PageSize * dgWOForAMECompletion.PageIndex
				Dim mWOID As Guid = mWOForAMECompletion(Index).WOID
				mnWO = nWO.GetWO(mWOID, False)
				mnWO.MarkClean()
				Session("mnWO") = mnWO

				Dim str As String
				str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage1=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "OpenToAddWODetail", "OpenToAddWODetail();", True)
				'Exit Sub
		End Select
	End Sub
	Private Sub chkShowCompletedJobs_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkShowCompletedJobs.CheckedChanged
		If chkShowCompletedJobs.Checked = True Then
			rbAMECompletion.Checked = False
			rbAMECompletion.Enabled = False
			dgWOJobList.Visible = True
			lblResult.Visible = True
			dgWOForAMECompletion.Visible = False
			lblResultAMECompletion.Visible = False
			btnPrintTop.Visible = True
		Else
			rbAMECompletion.Enabled = True
		End If
		Call btnFindNow_Click(sender, e)
		upnlGridView.Update()
		upnlWOForAMECompletion.Update()
		upnlResult.Update()
		upnlActionBtnTop.Update()
	End Sub
#End Region

End Class