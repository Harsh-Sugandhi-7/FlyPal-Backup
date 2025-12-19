
'AJAX Conversion By Saylee On 25-Jul-2014

Public Class wfnWOJobList_AJAX
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mWOJobStatusList As nWOJobStatusList
	Public mWOJobList As nWOJobList
	Public mWOJobTypeList As nWOJobTypeList
	Public mnWOJob As nWOJob
	Public mnWO As nWO
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, WOJobTypeID, No, WOJobStatusID, RegNo As String
	Dim EventLogID As Guid
	Dim mWODetail As String
	Dim totcnt As Integer
	Dim mMachineNameValueList As MachineNameValueList
	Dim IsReadOnly As Boolean 'Added by Saylee
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
		WOJobTypeID = Session("WOJobTypeID")
		WOJobStatusID = Session("WOJobStatusID")
		mWOJobTypeList = Session("mWOJobTypeList")
		mWOJobList = Session("mWOJobList")
		mnWOJob = Session("mnWOJob")
		mnWO = Session("mnWO")
		totcnt = Session("totcnt")
		mMachineNameValueList = Session("mMachineNameValueList")
		RegNo = Session("RegNo")
	End Sub
	Private Sub SetSession()
		Session("mWOJobStatusList") = mWOJobStatusList
		Session("mWOJobList") = mWOJobList
		Session("mDistinctWOText") = mDistinctWOText
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("WOJobTypeID") = WOJobTypeID
		Session("WOJobStatusID") = WOJobStatusID
		Session("No") = No
		Session("WOText") = WOText
		Session("mWOJobTypeList") = mWOJobTypeList
		Session("mWOJobList") = mWOJobList
		Session("mnWOJob") = mnWOJob
		Session("mnWO") = mnWO
		Session("mMachineNameValueList") = mMachineNameValueList
		Session("RegNo") = RegNo
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
		Session.Remove("mMachineNameValueList")
		Session.Remove("RegNo")
	End Sub
	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "wfnWOJobList_AJAX.aspx") <= 0 Then
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
			Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID
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
		'Select Case Index
		'    Case 0 'All'
		'        txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
		'        txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
		'    Case 1 'Last 1 Week
		'Last 1 Week
		If FromDate = "1/1/1900" Then
			txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
		Else
			txtFromDate.Text = FromDate
		End If
		If ToDate = "1/1/2200" Then
			txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
		Else
			txtToDate.Text = ToDate
		End If
		'    Case 2 'Last 1 Month
		'txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
		'txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
		'    Case 3 'Last 1 Quater
		'Select Case Today.Month
		'    Case 1, 2, 3
		'        txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
		'        txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
		'    Case 4, 5, 6
		'        txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
		'        txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
		'    Case 7, 8, 9
		'        txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
		'        txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
		'    Case 10, 11, 12
		'        txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
		'        txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
		'End Select
		'    Case 4 'Last 1 Year
		'txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
		'txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
		'    Case 5 'Current Financial Year
		'If Today.Month <= 3 Then  'Jan|Feb|Mar
		'    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
		'Else
		'    txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
		'End If
		'txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
		'    Case 6 'Between Dates
		'FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
		'ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
		'txtFromDate.Text = FromDate
		'txtToDate.Text = ToDate
		'End Select
	End Sub
	Private Sub setVariables()
		'SearchIndex = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex) 'Commented By Prashant on 2-Mar-2021 ALL01032021
		'DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)'Commented By Prashant on 2-Mar-2021 ALL01032021
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
		WOJobTypeID = IIf(cmbWOJobType.SelectedIndex <= 0, 0, cmbWOJobType.SelectedValue)
		RegNo = IIf(cmbAircraft.SelectedIndex <= 0, "", cmbAircraft.SelectedValue)

		No = txtNo.Text.Trim
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("WOJobTypeID") = WOJobTypeID
		Session("WOJobStatusID") = WOJobStatusID
		Session("No") = No
		Session("WOText") = WOText
		Session("RegNo") = RegNo
	End Sub
	Private Sub SetControl()
		setPeriod(DateIndex)
		CallFindNow(SearchIndex)
		dgWOJobList.DataBind()
		'cmbSearch.SelectedIndex = SearchIndex 'Commented By Prashant on 2-Mar-2021 ALL01032021
		'cmbDate.SelectedIndex = DateIndex 'Commented By Prashant on 2-Mar-2021 ALL01032021
		''cmbWO.SelectedValue = WOText
		cmbWO.SelectedValue = IIf(WOText = "", "(All)", WOText)
		cmbWOJobType.SelectedIndex = WOJobTypeID

		txtNo.Text = No
		cmbAircraft.SelectedValue = IIf(RegNo = "", "(ALL)", RegNo)

		ControlVisibility(SearchIndex, DateIndex)

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOJobList.Columns(3).HeaderText = "E.O. No."
			dgWOJobList.DataBind()
			lblResult.Text = "List of Engineering Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		Else
			dgWOJobList.Columns(3).HeaderText = "W.O. No."

			dgWOJobList.DataBind()
			lblResult.Text = "List of Work Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		End If
	End Sub
	Private Sub SetTitle()
		'Dim mWOJobList As nWOJobList
		'mWOJobList = nWOJobList.GetWOJobList()
		'totcnt = Session("totcnt")
		If (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblTitle.Text = "List of Engineering Order Jobs "  'shweta
		Else
			lblTitle.Text = "List of Work Order Jobs "  'shweta
		End If
	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub
	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "1/1/1900", Optional ByVal ToDate As String = "1/1/2200", Optional ByVal WOJobStatusID As Integer = 0, Optional ByVal WOJobTypeID As Integer = 0, Optional ByVal RegNo As String = "")
		mWOJobList = Nothing
		dgWOJobList.DataSource = Nothing

		mWOJobList = nWOJobList.GetWOJobList(Text, No, FromDate, ToDate, WOJobStatusID, WOJobTypeID, RegNo:=RegNo)
		dgWOJobList.DataSource = mWOJobList

		'dgWOJobList.DataBind()
		Session("mWOJobList") = mWOJobList
	End Sub
	Private Sub CallFindNow(ByVal Index As Integer)
		FindNow(WOText, CInt(Val(No)), txtFromDate.Text.ToString, txtToDate.Text.ToString, , WOJobTypeID, RegNo:=RegNo)
		dgWOJobList.PageIndex = 0
	End Sub
	Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
		'cmbDate.Visible = IIf(SearchIndex = 1, True, False) 'Commented By Prashant on 2-Mar-2021 ALL01032021
		'lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		'lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		'txtFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		'txtToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)

		'Commented By Prashant on 2-Mar-2021 ALL01032021
		'If SearchIndex = 1 And DateIndex = 6 Then
		'    txtFromDate.Enabled = True
		'    txtToDate.Enabled = True
		'ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
		'    txtFromDate.Enabled = False
		'    txtToDate.Enabled = False
		'End If

		'cmbWO.Visible = IIf(SearchIndex = 2, True, False)
		'cmbWOJobType.Visible = IIf(SearchIndex = 3, True, False)
		'txtNo.Visible = IIf(SearchIndex = 2 And cmbWO.SelectedIndex <> 0, True, False)
		'cmbAircraft.Visible = IIf(SearchIndex = 4, True, False)
		'End of Commented By Prashant on 2-Mar-2021 ALL01032021
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
									SetGrid()
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
								SetGrid()
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
							SetGrid()
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
			'lblInfo.Text = "Select Engineering Order Job from the list. Click On Edit Link To Modify The Selected Engineering Order Job."'Commented By Prashant on 2-Mar-2021 ALL01032021
			dgWOJobList.ToolTip = "List of Engineering Order Jobs"
			btnClose.ToolTip = "Click to close List of Engineering Order Job screen"
			btnCloseTop.ToolTip = "Click to close List of Engineering Job Order screen"
			btnFindNow.ToolTip = "Click to find list of Engineering Order Jobs as per searching criteria"
		Else
			lblTitle.Text = "List of Work Order Jobs"
			'lblInfo.Text = "Select Work Order Job from the list. Click On Edit Link To Modify The Selected Work Order Job."'Commented By Prashant on 2-Mar-2021 ALL01032021
			dgWOJobList.ToolTip = "List of Work Order Job"
			btnClose.ToolTip = "Click to close List of Work Order Job screen"
			btnCloseTop.ToolTip = "Click to close List of Work Order Job screen"
			btnFindNow.ToolTip = "Click to find list of Work Order Jobs as per searching criteria"
		End If
	End Sub
#End Region

#Region " DataFieldBind "
	Private Sub DataFieldBind()
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
		WOJobTypeID = Session("WOJobTypeID")
		WOJobStatusID = Session("WOJobStatusID")
		WOText = Session("WOText")
		RegNo = Session("RegNo")

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
		cmbWO.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText

		mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(ALL)", SkipIsForInventoryAircarft:=True)
		cmbAircraft.DataSource = mMachineNameValueList
		Session("mMachineNameValueList") = mMachineNameValueList

		mWOJobList = nWOJobList.GetWOJobList(, , , , , )

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
		DataBind()
	End Sub
	Private Sub SetGrid()
		For j As Integer = 0 To dgWOJobList.Rows.Count - 1
			If Me.dgWOJobList.Rows.Item(j).Cells(17).Text = Util.Trans.WO145.ToString Then
				IsReadOnly = False
			Else
				If Me.dgWOJobList.Rows.Item(j).Cells(5).Text = "&nbsp;" Then
					IsReadOnly = False
				ElseIf mMachineNameValueList(Me.dgWOJobList.Rows.Item(j).Cells(5).Text) Is Nothing Then '    If mMachineNameValueList(Me.dgWOJobList.Rows.Item(j).Cells(3).Text) Is Nothing Then
					IsReadOnly = True
				Else
					IsReadOnly = mMachineNameValueList(Me.dgWOJobList.Rows.Item(j).Cells(5).Text).IsReadOnly 'Added by Saylee - Restrict User from using ReadOnly Aircraft
				End If
			End If
			dgWOJobList.Rows(j).Cells(11).Enabled = Not (IsReadOnly = True) 'Edit
		Next

		IsReadOnly = Session("IsReadOnly") 'Added by Saylee
		If IsReadOnly = True Then
			lblReadOnly.Visible = True
		Else
			lblReadOnly.Visible = False
		End If
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
			Session("MiddleFrame") = "wfnWOJobList_AJAX.aspx"
			'If cmbSearch.Enabled = True Then 'Commented By Prashant on 2-Mar-2021 ALL01032021
			'    setFocus(cmbSearch)
			'End If
			DataFieldBind()
			SetControl()
		End If
		SetToolTip()
		SetTitle()
		SetGrid()
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

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblResult.Text = "List of Engineering Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		Else
			lblResult.Text = "List of Work Order Jobs as per criteria :" & mWOJobList.Count & " Record(s) found."
		End If
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
	End Sub
	'Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged 'Commented By Prashant on 2-Mar-2021 ALL01032021
	'    ClearControls()
	'    cmbDate.SelectedIndex = 0
	'    cmbWO.SelectedIndex = 0
	'    cmbWOJobType.SelectedIndex = 0
	'    cmbAircraft.SelectedIndex = 0
	'    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
	'    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
	'    setPeriod(DateIndex)
	'    If cmbSearch.Enabled = True Then
	'        setFocus(cmbSearch)
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
	'Private Sub cmbWO_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWO.SelectedIndexChanged  'Commented By Prashant on 2-Mar-2021 ALL01032021
	'    ClearControls()
	'    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
	'    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
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
	'Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged  'Commented By Prashant on 2-Mar-2021 ALL01032021
	'    ClearControls()
	'    Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
	'    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
	'    ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
	'    setPeriod(DateIndex)
	'    If cmbDate.Enabled = True Then
	'        setFocus(cmbDate)
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
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgWOJobList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOJobList.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobList.PageSize * dgWOJobList.PageIndex
				Dim mId As Guid = mWOJobList(Index).ID
				Dim mWOID As Guid = mWOJobList(Index).WOID
				Dim mDate As String = mWOJobList(Index).WODateFormatted
				Dim mWorkOrderNo As String = mWOJobList(Index).WONumber
				Dim mDescription As String = mWOJobList(Index).WOJobDescription
				Dim mAction As String = mWOJobList(Index).WOJobAction
				Dim mJobType As String = mWOJobList(Index).WOJobType
				Dim mJobSatatus As String = mWOJobList(Index).WOJobStatusName
				Dim mRegNo As String = IIf(mWOJobList(Index).RegNo = "", "", mWOJobList(Index).RegNo)
				mWODetail = mWorkOrderNo + " Dated : " + mDate + " Description : " + mDescription + " Action : " + mAction + " Job Type : " + mJobType + " Job status : " + mJobSatatus + IIf(mRegNo <> "", " Aircraft : " + mRegNo, "")
				MarkLog(Util.Action.Edit, "Work Order Job", mWODetail, Util.ErrorType.NoError, mId, EventLogID)

				EditRecord(mId, mWOID)

				If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "FIT")) Then
					Dim str As String
					str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage1=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				Else
					'Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
					'Session("mnWO") = mnWO
					If mWOJobList(Index).WOJobTypeID = "5" Then
						Dim str1 As String = "openledgersame('wfnWONRC_Ajax.aspx?BackPage1=index.aspx&BackPage=" & Request.QueryString("BackPage") & "');"
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str1, True)
					Else
						Dim str As String
						str = "openledgersame('wfnWOJobDetail.aspx?BackPage1=index.aspx');"
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
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
	'Private Sub cmbWOJobType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbWOJobType.SelectedIndexChanged, txtFromDate.TextChanged, txtToDate.TextChanged, txtNo.TextChanged 'Commented By Prashant on 2-Mar-2021 ALL01032021
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
		'If cmbSearch.SelectedIndex = 0 Then
		'    SearchStr1 = "The report shows all records till date."
		'    SearchStr2 = ""
		'ElseIf cmbSearch.SelectedIndex = 1 Then
		'    'Date
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    If cmbDate.SelectedIndex = 0 Then
		'        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
		'    ElseIf cmbDate.SelectedIndex = 6 Then
		'        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + "    " + lblFromDate.Text + ":" + "  " + New SmartDate(txtFromDate.Text).FormattedText + "    " + lblToDate.Text + ":" + " " + New SmartDate(txtToDate.Text).FormattedText
		'    Else
		'        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + "    " + lblFromDate.Text + ":" + " " + New SmartDate(txtFromDate.Text).FormattedText + "    " + lblToDate.Text + ":" + " " + New SmartDate(txtToDate.Text).FormattedText
		'    End If
		'ElseIf cmbSearch.SelectedIndex = 2 Then
		'    'WO No.
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbWO.SelectedItem.Text + "    " + lblNo.Text + " " + txtNo.Text
		'ElseIf cmbSearch.SelectedIndex = 3 Then
		'    'Status
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbWOJobType.SelectedItem.Text
		'ElseIf cmbSearch.SelectedIndex = 4 Then
		'    'Status
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbAircraft.SelectedItem.Text
		'End If
		SearchStr1 = "The report shows records filtered by the following criteria"
		SearchStr2 = "By" + " " + "Date Range " + FromDate + " " + ToDate + " " + IIf(WOText = "" And txtNo.Text.Trim = "0", "", "WO. No. " + WOText + "-" + CStr(txtNo.Text.Trim)) + _
					 IIf(cmbWOJobType.SelectedIndex <= 0, "", " Job Type " + cmbWOJobType.SelectedItem.Text) + IIf(RegNo = "", "", " Reg No. " + RegNo)
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
	Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
		IsReadOnly = mMachineNameValueList(cmbAircraft.SelectedValue).IsReadOnly 'Added by Saylee - Restrict User from using ReadOnly Aircraft
		Session("IsReadOnly") = IsReadOnly
		SetGrid()
		upnlSearchCriteria.Update()
		Session.Remove("IsReadOnly")
	End Sub
	Private Sub txtBarcode_TextChanged(sender As Object, e As System.EventArgs) Handles txtBarcode.TextChanged
		Dim BarcodeNoExists As nWOBarcodeNoExists = nWOBarcodeNoExists.GetBarcodeNoCount(txtBarcode.Text.Trim)
		If Not BarcodeNoExists.ID.Equals(Guid.Empty) Then
			Dim mnWOJob As nWOJob
			Dim mnWOJobTask As nWOJobTask
			Select Case BarcodeNoExists.Type
				'Case "WO"
				'    EditRecord(BarcodeNoExists.ID)
				'    Session("Edit") = True
				'    DataFieldBind()
				'    SetControl()
				'    SetTitle()
				'    SetGrid()
				'    GridColumnsVisibility()
				'    upnlGridView.Update()
				'    upnlActionBtnTop.Update()
				'    upnlActionBtnBottom.Update()
				'    upnlResult.Update()

				'    Dim str As String
				'    str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage=index.aspx');"
				'    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				Case "WOJob"

					If BarcodeNoExists.JobType = 5 Then
						mnWO = nWO.GetWO(BarcodeNoExists.WOID, False)
						mnWOJob = mnWO.WONRCJobs.Item(BarcodeNoExists.ID)
						mnWO.WONRCJobs.CurrentIndex = mnWO.WONRCJobs.IndexOfItem(BarcodeNoExists.ID, BarcodeNoExists.WOID)
						Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID
						Session("mnWOJob") = mnWOJob
						Session("mnWO") = mnWO
						Dim str1 As String = "openledgersame('wfnWONRC.aspx?BackPage1=index.aspx&BackPage=" & Request.QueryString("BackPage") & "');"
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str1, True)
					Else
						mnWO = nWO.GetWO(BarcodeNoExists.WOID, False)
						mnWOJob = mnWO.WOJobs.Item(BarcodeNoExists.ID)
						mnWO.WOJobs.CurrentIndex = mnWO.WOJobs.IndexOfItem(BarcodeNoExists.ID, BarcodeNoExists.WOID)
						Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
						Session("mnWOJob") = mnWOJob
						Session("mnWO") = mnWO
						Dim str As String
						str = "openledgersame('wfnWOJobDetail.aspx?BackPage1=index.aspx');"
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
					End If
				Case "WOJobTask"
					mnWO = nWO.GetWO(BarcodeNoExists.WOID, False)
					mnWOJob = mnWO.WOJobs.Item(BarcodeNoExists.WOJobID)
					mnWO.WOJobs.CurrentIndex = mnWO.WOJobs.IndexOfItem(BarcodeNoExists.WOJobID, BarcodeNoExists.WOID)
					mnWOJobTask = mnWOJob.WOJobTasks(BarcodeNoExists.ID)
					mnWOJob.WOJobTasks.CurrentIndex = mnWOJob.WOJobTasks.IndexOfItem(BarcodeNoExists.ID)
					Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
					Session("mnWOJob") = mnWOJob
					Session("mnWOJobTask") = mnWOJobTask
					Session("mnWO") = mnWO
					Dim Index As Integer = mnWOJob.WOJobTasks.CurrentIndex
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddJobTaskDetail", "OpenToAddJobTaskDetail('" + Index.ToString + "');", True)
			End Select
		Else
			MSGBoxCtrl.show("Alert..!!", "Invalid Barcode Number", "", MsgBoxStyle.OkOnly, "")
			txtBarcode.Text = ""
			Exit Sub
		End If
	End Sub
#End Region



End Class