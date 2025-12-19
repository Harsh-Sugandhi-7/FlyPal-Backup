Imports System.Linq
Public Class wfnWOJobListToComplete_AJAX
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mWOJobStatusList As nWOJobStatusList
	Public mWOJobListToComplete As nWOJobListToComplete
	Public mWOJobTypeList As nWOJobTypeList
	Public mnWOJob As nWOJob
	Public mnWO As nWO
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, WOJobTypeID, No, WOJobStatusID As String
	Dim EventLogID As Guid
	Dim mWODetail As String
	Dim mMachineNameValueList As MachineNameValueList
	Dim IsReadOnly As Boolean
	Dim mnWOJobNRC As nWOJob
	Dim mIsClosedJobs As Boolean = False
	Dim WOJobTypeIDList As Object = Nothing
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
		WOJobTypeID = Session("SearchWOJobTypeID")
		WOJobStatusID = Session("WOJobStatusID")
		mWOJobListToComplete = Session("mWOJobListToComplete")
		mnWOJob = Session("mnWOJob")
		mnWO = Session("mnWO")
		mMachineNameValueList = Session("mMachineNameValueList")
		mIsClosedJobs = Session("IsClosedJobs")
	End Sub
	Private Sub SetSession()
		Session("mWOJobStatusList") = mWOJobStatusList
		Session("mWOJobListToComplete") = mWOJobListToComplete
		Session("mDistinctWOText") = mDistinctWOText
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("SearchWOJobTypeID") = WOJobTypeID
		Session("WOJobStatusID") = WOJobStatusID
		Session("No") = No
		Session("WOText") = WOText
		Session("mWOJobListToComplete") = mWOJobListToComplete
		Session("mnWOJob") = mnWOJob
		Session("mnWO") = mnWO
		Session("mMachineNameValueList") = mMachineNameValueList
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mWOJobStatusList")
		Session.Remove("mWOJobListToComplete")
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
		Session.Remove("mWOJobListToComplete")
		Session.Remove("mnWOJob")
		Session.Remove("mnWO")
		Session.Remove("mMachineNameValueList")
		Session.Remove("OpenFromWOJobListToCompleteForm")
		Session.Remove("IsClosedJobs")
	End Sub
	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "wfnWOJobListToComplete_AJAX.aspx") <= 0 Then
			RemoveSession()
			Session.Remove("mWOJobListToComplete")
		End If
	End Sub
	Private Sub EditWOJobRecord(ByVal mId As Guid, ByVal mWOID As Guid)
		mnWO = nWO.GetWO(mWOID, False)
		mnWOJob = mnWO.WOJobs.Item(mId)
		mnWO.WOJobs.CurrentIndex = mnWO.WOJobs.IndexOfItem(mId)
		Session("WOJobTypeID") = mnWO.WOJobs.CurrentItem.WOJobTypeID
		Session("mnWOJob") = mnWOJob
		Session("mnWO") = mnWO
	End Sub
	Private Sub EditWONRCRecord(ByVal mId As Guid, ByVal mWOID As Guid)
		mnWO = nWO.GetWO(mWOID, False)
		mnWOJob = mnWO.WONRCJobs.Item(mId)
		mnWO.WONRCJobs.CurrentIndex = mnWO.WONRCJobs.IndexOfItem(mId)
		Session("WOJobTypeID") = mnWO.WONRCJobs.CurrentItem.WOJobTypeID
		Session("mnWOJob") = mnWOJob
		Session("mnWO") = mnWO
	End Sub
	Private Sub EditWOJobNRCRecord(ByVal mId As Guid, ByVal mWOID As Guid)
		mnWO = nWO.GetWO(mWOID, False)
		mnWOJobNRC = nWOJob.GetWOJobNRC(mId)
		Session("nWOJobNRC") = mnWOJobNRC
		Session("mnWO") = mnWO
		Session("mnWOJobParent") = mnWO.WOJobs.CurrentItem
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub setPeriod(ByVal Index As Int32)
		'Last 1 Week
		If FromDate = "1/1/1900" Then
			txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
		Else
			txtFromDate.Text = FromDate
		End If
		If ToDate = "1/1/2200" Then
			txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
		Else
			txtToDate.Text = ToDate
		End If
	End Sub
	Private Sub setVariables()
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
		WOJobTypeID = IIf(cmbWOJobType.SelectedIndex <= 0, 0, cmbWOJobType.SelectedValue)
		mIsClosedJobs = chkIsClosedJobs.Checked

		No = txtNo.Text.Trim
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("SearchWOJobTypeID") = WOJobTypeID
		Session("WOJobStatusID") = WOJobStatusID
		Session("No") = No
		Session("WOText") = WOText
		Session("IsClosedJobs") = mIsClosedJobs
	End Sub
	Private Sub SetControl()
		setPeriod(DateIndex)
		CallFindNow(SearchIndex)
		dgWOJobList.DataBind()
		cmbWO.SelectedValue = IIf(WOText = "", "(All)", WOText)
		cmbWOJobType.SelectedIndex = WOJobTypeID
		txtNo.Text = No
		chkIsClosedJobs.Checked = mIsClosedJobs
		ControlVisibility(SearchIndex, DateIndex)
		lblResult.Text = "As per criteria :" & mWOJobListToComplete.Count & " Record(s) found."
	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub
	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "1/1/1900",
						Optional ByVal ToDate As String = "1/1/2200", Optional ByVal WOJobStatusID As Integer = 0,
						Optional ByVal WOJobTypeID As Integer = 0, Optional ByVal IsClosedJobs As Boolean = 0)
		mWOJobListToComplete = Nothing
		dgWOJobList.DataSource = Nothing

		mWOJobListToComplete = nWOJobListToComplete.GetWOJobListToComplete(Text, No, FromDate, ToDate, WOJobStatusID, WOJobTypeID,
																		   IsClosedJobs:=IsClosedJobs) '88 145 WO i.e Third Party Work order
		dgWOJobList.DataSource = mWOJobListToComplete
		Session("mWOJobListToComplete") = mWOJobListToComplete
	End Sub
	Private Sub CallFindNow(ByVal Index As Integer)
		FindNow(WOText, CInt(Val(No)), txtFromDate.Text.ToString, txtToDate.Text.ToString, , WOJobTypeID, mIsClosedJobs)
		dgWOJobList.PageIndex = 0
	End Sub
	Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
		If chkIsClosedJobs.Checked = True Then
			dgWOJobList.Columns(15).Visible = False 'Complete
			dgWOJobList.Columns(16).Visible = True  'Edit
		Else
			dgWOJobList.Columns(15).Visible = True  'Complete
			dgWOJobList.Columns(16).Visible = False  'Edit
		End If
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
									MSGBoxCtrl.Show("Alert!", "This Transaction cannot be deleted. Already sent for billing.", "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								Else
									mnWO.Delete()
									mnWO.Save()
									DataFieldBind()
									SetControl()
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
								MarkLog(Util.Action.Delete, "WOJobListToComplete", "Can't delete : " & mWODetail & " is Currently in use", Util.ErrorType.NoError, TempWOID, EventLogID)
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
							End If
							DataFieldBind()
							SetControl()
							upnlResult.Update()
							msgCount = ex.Errors.Count
						Finally
							If msgCount = 0 Then
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
		End If
	End Sub

#End Region

#Region " DataFieldBind "
	Private Sub DataFieldBind()
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
		WOJobTypeID = Session("SearchWOJobTypeID")
		WOJobStatusID = Session("WOJobStatusID")
		WOText = Session("WOText")

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
		cmbWO.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText


		'mWOJobListToComplete = nWOJobListToComplete.GetWOJobListToComplete(, , , , , )

		'dgWOJobList.DataSource = mWOJobListToComplete
		'Session("mWOJobListToComplete") = mWOJobListToComplete

		mWOJobTypeList = nWOJobTypeList.GetWOJobTypeList("(All)")
		WOJobTypeIDList = (From c As nWOJobTypeList.nWOJobTypeListInfo In mWOJobTypeList
						   Where {0, 1, 2, 5}.Contains(c.ID)
						   Select c).ToList
		cmbWOJobType.DataSource = WOJobTypeIDList

		'lblResult.Text = "As per criteria :" & mWOJobListToComplete.Count & " Record(s) found."
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
			Session("MiddleFrame") = "wfnWOJobListToComplete_AJAX.aspx"
			DataFieldBind()
			SetControl()
		End If
	End Sub
	Private Sub dgWOJobList_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOJobList.PageIndexChanging
		dgWOJobList.PageIndex = e.NewPageIndex
		dgWOJobList.DataSource = mWOJobListToComplete
		Session("mWOJobListToComplete") = mWOJobListToComplete
		dgWOJobList.DataBind()
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		setVariables()
		CallFindNow(SearchIndex)

		dgWOJobList.DataBind()
		If chkIsClosedJobs.Checked = True Then
			dgWOJobList.Columns(15).Visible = False 'Complete
			dgWOJobList.Columns(16).Visible = True  'Edit
		Else
			dgWOJobList.Columns(15).Visible = True  'Complete
			dgWOJobList.Columns(16).Visible = False  'Edit
		End If
		lblResult.Text = "As per criteria :" & mWOJobListToComplete.Count & " Record(s) found."
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
	End Sub
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
		', btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgWOJobList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOJobList.RowCommand
		Select Case e.CommandName
			Case "CompleteRec"
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOJobList.PageSize * dgWOJobList.PageIndex
				Dim mId As Guid = mWOJobListToComplete(Index).ID
				Dim mWOID As Guid = mWOJobListToComplete(Index).WOID
				Dim mDate As String = mWOJobListToComplete(Index).WODateFormatted
				Dim mWorkOrderNo As String = mWOJobListToComplete(Index).WONumber
				Dim mDescription As String = mWOJobListToComplete(Index).WOJobDescription
				Dim mAction As String = mWOJobListToComplete(Index).WOJobAction
				Dim mJobType As String = mWOJobListToComplete(Index).WOJobType
				Dim mJobSatatus As String = mWOJobListToComplete(Index).WOJobStatusName
				mWODetail = "Completed From Allocation Wo Jobs Link " + mWorkOrderNo + " Dated : " + mDate + " Description : " + mDescription + " Action : " + mAction + " Job Type : " + mJobType + " Job status : " + mJobSatatus
				MarkLog(Util.Action.Edit, "WOJobListToComplete", mWODetail, Util.ErrorType.NoError, mId, EventLogID)
				Session("OpenFromWOJobListToCompleteForm") = "True"


				'If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA")) Then
				'    Dim str As String
				'    str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage1=index.aspx');"
				'    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				'Else
				If (mWOJobListToComplete(Index).WOJobTypeID = "5" And mWOJobListToComplete(Index).WOJobID.Equals(Guid.Empty)) Then 'This is Work Order NRC
					EditWONRCRecord(mId, mWOID)
					Dim str1 As String = "openledgersame('wfnWONRC.aspx?BackPage1=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str1, True)
				ElseIf (mWOJobListToComplete(Index).WOJobTypeID = "5" And Not mWOJobListToComplete(Index).WOJobID.Equals(Guid.Empty)) Then 'This is Work Order Job's NRC
					EditWOJobNRCRecord(mId, mWOID)
					Dim str2 As String
					str2 = "openledgersame('wfnWOJobNRC.aspx?BackPage1=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str2, True)
				Else
					EditWOJobRecord(mId, mWOID)
					Dim str3 As String
					str3 = "openledgersame('wfnWOJobDetail.aspx?BackPage1=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str3, True)
				End If
				'End If
			Case "EditView"
				Dim Index As Integer = CInt(e.CommandArgument) 'CInt(e.CommandArgument) + dgWOJobList.PageSize * dgWOJobList.PageIndex
				Dim mId As Guid = mWOJobListToComplete(Index).ID
				Dim mWOID As Guid = mWOJobListToComplete(Index).WOID
				Dim mDate As String = mWOJobListToComplete(Index).WODateFormatted
				Dim mWorkOrderNo As String = mWOJobListToComplete(Index).WONumber
				Dim mDescription As String = mWOJobListToComplete(Index).WOJobDescription
				Dim mAction As String = mWOJobListToComplete(Index).WOJobAction
				Dim mJobType As String = mWOJobListToComplete(Index).WOJobType
				Dim mJobSatatus As String = mWOJobListToComplete(Index).WOJobStatusName
				mWODetail = "Completed From Allocation Wo Jobs Link " + mWorkOrderNo + " Dated : " + mDate + " Description : " + mDescription + " Action : " + mAction + " Job Type : " + mJobType + " Job status : " + mJobSatatus
				MarkLog(Util.Action.Edit, "WOJobListToComplete", mWODetail, Util.ErrorType.NoError, mId, EventLogID)
				Session("OpenFromWOJobListToCompleteForm") = "True"
				'If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA")) Then
				'    Dim str As String
				'    str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage1=index.aspx');"
				'    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				'Else
				If (mWOJobListToComplete(Index).WOJobTypeID = "5" And mWOJobListToComplete(Index).WOJobID.Equals(Guid.Empty)) Then 'This is Work Order NRC
					EditWONRCRecord(mId, mWOID)
					Dim str1 As String = "openledgersame('wfnWONRC.aspx?BackPage1=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str1, True)
				ElseIf (mWOJobListToComplete(Index).WOJobTypeID = "5" And Not mWOJobListToComplete(Index).WOJobID.Equals(Guid.Empty)) Then 'This is Work Order Job's NRC
					EditWOJobNRCRecord(mId, mWOID)
					Dim str2 As String
					str2 = "openledgersame('wfnWOJobNRC.aspx?BackPage1=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str2, True)
				Else
					EditWOJobRecord(mId, mWOID)
					Dim str3 As String
					str3 = "openledgersame('wfnWOJobDetail.aspx?BackPage1=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str3, True)
				End If
		End Select
	End Sub
	Private Sub dgWOJobList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOJobList.Sorting
		mWOJobListToComplete.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOJobList.DataSource = mWOJobListToComplete
		Session("mWOJobListToComplete") = mWOJobListToComplete
		dgWOJobList.DataBind()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottomPrint.Click
		', btnPrintTop.Click
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
		SearchStr2 = "By" + " " + "Date Range " + FromDate + " " + ToDate + " " + IIf(WOText = "" And txtNo.Text.Trim = "0", "", "WO. No. " + WOText + "-" + CStr(txtNo.Text.Trim)) +
					 IIf(cmbWOJobType.SelectedIndex <= 0, "", " Job Type " + cmbWOJobType.SelectedItem.Text)
		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
		mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
		mCompanyDetail.WebSite, "", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

		If mWOJobListToComplete.Count = 0 Then
			MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, mrptImage)
		da.Fill(ds, mWOJobListToComplete)
		da.Fill(ds, Report)
		Rpt.SetDataSource(ds)
		Session("CrystalReport") = Rpt
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
	End Sub
#End Region

End Class