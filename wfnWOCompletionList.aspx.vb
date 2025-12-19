'Created By Shital On 24-May-2019

Public Class wfnWOCompletionList
	Inherits System.Web.UI.Page


#Region " Enumaration "
	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
	End Enum
#End Region

#Region " Variable Declaration "
	Public mnWO As nWO
	Public mWOCompletionList As nWOList
	Public mWOStatusList As nWOStatusList
	Dim mMachineNameValueList As MachineNameValueList
	Dim mWOModelNameValueList As nWOModelNameValueList
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, StatusID, No, WOStatusID, RegNo, ModelName As String
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mWODetail As String
	Dim totcnt As Integer

	Dim IsReadOnly As Boolean 'Added by Saylee

	Public mTransTypeID As Trans 'Added by Saylee on 5-Sep-2018
	Dim mMachineNameAutoCompleteList As DistinctTextListAutoComplete
	Dim mModelNameAutoCompleteList As DistinctTextListAutoComplete
	Dim ShowCompletedJobs As Boolean = False
	Dim IsAllWOsTicked As Boolean  'Added by Saylee on 14-Jan-2020

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mWOStatusList = Session("mWOStatusList")
		mWOCompletionList = Session("mWOCompletionList")
		mMachineNameValueList = Session("mMachineNameValueList")
		mWOModelNameValueList = Session("mWOModelNameValueList")
		mDistinctWOText = Session("mDistinctWOText")

		WOText = Session("WOText")
		No = IIf(IsNothing(Session("No")), 0, Session("No"))
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		SearchIndex = Session("SearchIndex")
		DateIndex = Session("DateIndex")
		StatusID = Session("StatusID")
		WOStatusID = Session("WOStatusID")
		RegNo = Session("RegNo")
		ModelName = Session("ModelName")
		mTransTypeID = Session("mTransTypeId")  'Added by Saylee on 5-Sep-2018
		mMachineNameAutoCompleteList = Session("mMachineNameAutoCompleteList")
		mModelNameAutoCompleteList = Session("mModelNameAutoCompleteList")
		ShowCompletedJobs = Session("ShowCompletedJobs")
		IsAllWOsTicked = Session("IsAllWOsTicked")  'Added by Saylee on 14-Jan-2020
	End Sub

	Private Sub SetSession()
		Session("mWOStatusList") = mWOStatusList
		Session("mWOCompletionList") = mWOCompletionList
		Session("mMachineNameValueList") = mMachineNameValueList
		Session("mWOModelNameValueList") = mWOModelNameValueList
		Session("mDistinctWOText") = mDistinctWOText
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("StatusID") = StatusID
		Session("WOStatusID") = WOStatusID

		Session("No") = No
		Session("RegNo") = RegNo
		Session("ModelName") = ModelName
		Session("WOText") = WOText

		Session("mTransTypeId") = mTransTypeID  'Added by Saylee on 5-Sep-2018
		Session("mMachineNameAutoCompleteList") = mMachineNameAutoCompleteList
		Session("mModelNameAutoCompleteList") = mModelNameAutoCompleteList

		Session("IsAllWOsTicked") = IsAllWOsTicked  'Added by Saylee on 14-Jan-2020
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mWOStatusList")
		Session.Remove("mWOCompletionList")
		Session.Remove("mMachineNameValueList")
		Session.Remove("mWOModelNameValueList")
		Session.Remove("mDistinctWOText")
		Session.Remove("FromDate")
		Session.Remove("ToDate")
		Session.Remove("SearchIndex")
		Session.Remove("DateIndex")
		Session.Remove("StatusID")
		Session.Remove("WOStatusID")
		Session.Remove("No")
		Session.Remove("RegNo")
		Session.Remove("ModelName")
		Session.Remove("WOText")
		Session.Remove("mMachineList")
		Session.Remove("totcnt")
		Session.Remove("mTransTypeId") 'Added by Saylee on 5-Sep-2018
		Session.Remove("mMachineNameAutoCompleteList")
		Session.Remove("mModelNameAutoCompleteList")
		Session.Remove("ShowCompletedJobs")
		Session.Remove("IsAllWOsTicked")  'Added by Saylee on 14-Jan-2020
	End Sub

	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "wfnWOCompletionList.aspx?") <= 0 Then
			RemoveSession()
		End If
	End Sub

	Private Sub NewRecord()
		mnWO = nWO.NewWO(, mTransTypeID)
		Session("mnWO") = mnWO
		Session("mTransTypeID") = mTransTypeID 'Added by Saylee on 5-Sep-2018
	End Sub

	Private Sub EditRecord(ByVal mId As Guid)
		mnWO = nWO.GetWO(mId, False)
		mnWO.MarkClean()
		Session("mnWO") = mnWO
		Session("mTransTypeId") = mTransTypeID 'Added by Saylee on 5-Sep-2018
	End Sub

	Private Sub DeleteRecord(ByVal mId As Guid)
		mnWO = nWO.GetWO(mId)
		Session("mnWO") = mnWO
		DataFieldBind()
		SetControl()
		SetTitle()
		SetGrid()
		GridColumnsVisibility()
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()

		If mnWO.WOJobs.IsScheduledJobExists Then
			Dim WOstr As String = ""
			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
				WOstr = "Engineering Order"
			Else
				WOstr = "Work Order"
			End If
			MSGBoxCtrl.show("Alert!", "<BR>There are Scheduled jobs in this " & WOstr & " which may have been already complied,to change their status please use the Maintenance menu option" & ".<BR><BR>Do you want to continue?", "", MsgBoxStyle.YesNo, "IsScheduledJobExists")
		Else
			MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, " ", MsgBoxStyle.YesNo, "Delete")
		End If

	End Sub

	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub

	Private Sub SetTitle()
		'Dim mWOCompletionList As nWOList
		'mWOCompletionList = nWOList.GetWOList()
		'totcnt = Session("totcnt")
		'Commented by Saylee on 3-Mar-2017 for performance
		'  lblTitle.Text = "List of Work Order    [Total No of Record(s):-" + totcnt.ToString() + "]"  'shweta
		mWOCompletionList = Session("mWOCompletionList")
		totcnt = mWOCompletionList.TotalWOCount
		Session("totcnt") = totcnt
		lblTitle.Text = "List for Work Order Closing/Completion" '[Total No of Record(s):-" + totcnt.ToString() + "]"
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


		DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)         '--Changed By Utkarsh On 17-Jan-2011

		RegNo = IIf(cmbAircraft.SelectedIndex <= 0, "", cmbAircraft.SelectedValue)
		ModelName = IIf(cmbModel.SelectedIndex <= 0, "", cmbModel.SelectedValue) '--Changed By Utkarsh On 17-Jan-2011
		No = txtNo.Text.Trim

		ShowCompletedJobs = IIf(chkShowCompletedJobs.Checked, True, False)
		IsAllWOsTicked = chkShowAllWOs.Checked  'Added by Saylee on 14-Jan-2020

		WOStatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue) 'Added by Saylee on 14-Jan-2020
		Session("WOStatusID") = WOStatusID

		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("StatusID") = StatusID
		Session("WOStatusID") = WOStatusID
		Session("No") = No
		Session("RegNo") = RegNo
		Session("ModelName") = ModelName
		Session("WOText") = WOText

		Session("ShowCompletedJobs") = ShowCompletedJobs
		Session("IsAllWOsTicked") = IsAllWOsTicked  'Added by Saylee on 14-Jan-2020
	End Sub

	Private Sub SetToolTip()
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblTitle.Text = "List of Engineering Order"
			lblInfo.Text = "Select Engineering Order from the list. Click On Edit Link To Modify The Selected Engineering Order. Click On Delete Link To Delete The Selected Engineering Order. Click On Add New button To Add A New Engineering Order."


			btnPrintTop.ToolTip = "Click to Print the list of Engineering Order"
			btnPrint.ToolTip = "Click to Print the list of Engineering Order"
			btnClose.ToolTip = "Click to close List of Engineering Order screen"
			btnCloseTop.ToolTip = "Click to close List of Engineering Order screen"
			btnFindNow.ToolTip = "Click to find list of Engineering Order as per searching criteria"

		Else
			lblTitle.Text = "List of Work Order"
			lblInfo.Text = "Select Work Order from the list. Click On Edit Link To Modify The Selected Work Order. Click On Delete Link To Delete The Selected Work Order. Click On Add New button To Add A New Work Order."


			btnPrintTop.ToolTip = "Click to Print the list of Work Order"
			btnPrint.ToolTip = "Click to Print the list of Work Order"
			btnClose.ToolTip = "Click to close List of Work Order screen"
			btnCloseTop.ToolTip = "Click to close List of Work Order screen"
			btnFindNow.ToolTip = "Click to find list of Work Order as per searching criteria"
		End If
	End Sub
	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = "WOCompletion"

		'Depending upon decided IsInRole String; checkign Rights of the User
		Select Case CheckFor
			Case Rights.[New]
				Return User.IsInRole(IsInRoleString + "New")
			Case Rights.Edit
				Return User.IsInRole(IsInRoleString + "Edit")
			Case Rights.Save
				Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
			Case Rights.Delete
				Return User.IsInRole(IsInRoleString + "Delete")
			Case Rights.View
				Return User.IsInRole(IsInRoleString + "View")
			Case Rights.Print
				Return User.IsInRole(IsInRoleString + "Print")
				'Case Rights.FindNow
				'    Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
		End Select
	End Function
	Private Sub CallFindNow(ByVal Index As Integer)
		'Select Case Index
		'    Case -1 'all
		'        FindNow()
		'    Case 0 'all
		'        FindNow(, , , , , , , , "(ALL)")
		'    Case 1 'WO Date date
		'        FindNow(, , txtFromDate.Text.ToString, txtToDate.Text.ToString, , , , , )
		'    Case 2  'Work Order
		'        FindNow(WOText, CInt(Val(No)), , , , , , , )
		'    Case 3  'Aircraft
		'        FindNow(, , , , RegNo, , , , )
		'    Case 4 'Model
		'        FindNow(, , , , , ModelName)
		'    Case 5  'Status
		'        FindNow(, , , , , , WOStatusID, , )
		'    Case 6  'DocStatus
		'        FindNow(, , , , , , , StatusID, )

		'End Select
		FindNow(WOText, CInt(Val(No)), txtFromDate.Text.ToString, txtToDate.Text.ToString, RegNo:=RegNo, ModelName:=ModelName, WOStatusID:=WOStatusID, StatusID:=0)
	End Sub
	Private Sub SetControl()
		setPeriod(DateIndex)
		chkShowCompletedJobs.Checked = ShowCompletedJobs
		chkShowAllWOs.Checked = IsAllWOsTicked  'Added by Saylee on 14-Jan-2020
		cmbStatus.SelectedValue = WOStatusID  'Added by Saylee on 14-Jan-2020
		CallFindNow(SearchIndex)
		dgWOList.DataBind()

		cmbDate.SelectedIndex = DateIndex
		cmbAircraft.SelectedValue = IIf(RegNo = "", "(ALL)", RegNo)

		cmbModel.SelectedValue = IIf(ModelName = "", "(ALL)", ModelName) '--Changed By Utkarsh On 17-Jan-2011
		cmbWO.SelectedValue = IIf(WOText = "", "(ALL)", WOText) '--Changed By Utkarsh On 17-Jan-2011
		txtNo.Text = No
		chkShowCompletedJobs.Checked = ShowCompletedJobs


		ControlVisibility(DateIndex)

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOList.Columns(2).HeaderText = "E.O. No."
			'dgWOList.Columns(12).HeaderText = "E.O. Status"

			dgWOList.DataBind()
			lblResult.Text = "List for Engineering Order Closing/Completion as per criteria :" & mWOCompletionList.Count & " Record(s) found."
		Else
			dgWOList.Columns(2).HeaderText = "W.O. No."
			'dgWOList.Columns(12).HeaderText = "W.O. Status"

			dgWOList.DataBind()
			lblResult.Text = "List for Work Order Closing/Completion as per criteria :" & mWOCompletionList.Count & " Record(s) found."
		End If
		GridColumnsVisibility()
	End Sub

	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal RegNo As String = "", Optional ByVal ModelName As String = "", Optional ByVal WOStatusID As Integer = 0, Optional ByVal StatusID As Integer = 0, Optional ByVal AddTopItem As String = "")

		If IsAllWOsTicked = True Then                 'Added by Saylee on 14-Jan-2020
			WOStatusID = cmbStatus.SelectedValue
			dgWOList.Columns(15).HeaderText = "Edit"
			If WOStatusID = 1 Then StatusID = 1
		Else
			dgWOList.Columns(15).HeaderText = "Complete WO."
		End If

		mWOCompletionList = Nothing
		dgWOList.DataSource = Nothing
		mWOCompletionList = nWOList.GetWOList(Text, No, FromDate, ToDate, RegNo, ModelName, StatusID, WOStatusID, "", "", mTransTypeID, IsAllJobsCompletedButWONotCompletedListRequired:=IIf(chkShowCompletedJobs.Checked = True Or IsAllWOsTicked = True, False, True), ShowOnlyCompletedWOs:=IIf(chkShowCompletedJobs.Checked = True, True, False))
		dgWOList.DataSource = mWOCompletionList
		Session("mWOCompletionList") = mWOCompletionList
		totcnt = mWOCompletionList.TotalWOCount
		Session("totcnt") = totcnt

	End Sub
	Private Sub ControlVisibility(Optional ByVal index As Int32 = 0)
		If index = 6 Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = True
			txtToDate.Enabled = True
			lblFromDate.Visible = True
			lblToDate.Visible = True
		ElseIf index = 1 Or index = 2 Or index = 3 Or index = 4 Or index = 5 Then
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

		txtNo.Visible = IIf(cmbWO.SelectedIndex <> 0, True, False)
		lblNo.Visible = IIf(cmbWO.SelectedIndex <> 0, True, False)

		If chkShowCompletedJobs.Checked Then  'Added by Saylee on 14-Jan-2020
			chkShowAllWOs.Checked = False
			phStatus.Visible = False
		ElseIf chkShowAllWOs.Checked = True Then
			chkShowCompletedJobs.Checked = False
			phStatus.Visible = True
		End If

		upnllblNo.Update()
		upnlNo.Update()
	End Sub

	Private Sub ClearControls()
		txtNo.Text = ""
	End Sub

	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Dim msgCount As Integer = 0
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Dim TempWOID As Guid
						Try
							Dim mnWO As nWO
							Session("sender") = ""
							mnWO = CType(Session("mnWO"), nWO)
							TempWOID = mnWO.ID
							mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy + IIf(Not mnWO.MachineID.Equals(Guid.Empty), " Aircraft : " + mnWO.RegNo, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
							MarkLog(Util.Action.Delete, "Work Order", mWODetail, Util.ErrorType.NoError, TempWOID, EventLogID)


							If Not mnWO.LogID.Equals(Guid.Empty) Then
								Dim mLog As Log
								mLog = Log.GetLog(mnWO.LogID)
								If SaveLog(mLog) Then
									mLog = Nothing
								End If
							End If

							mnWO.Delete()
							mnWO.Save()
							DataFieldBind()
							SetControl()
							SetTitle()
							upnlGridView.Update()
							upnlActionBtnTop.Update()
							upnlActionBtnBottom.Update()
							upnlResult.Update()
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
							msgCount = ex.Errors.Count
						Finally

						End Try
					ElseIf MSGBoxCtrl.Sender = "IsScheduledJobExists" Then
						Session("sender") = ""
						MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, " ", MsgBoxStyle.YesNo, "Delete")
					End If
				Case MsgBoxResult.No
					Session("sender") = ""
				Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
					Session("sender") = ""
					DataFieldBind()
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					DataFieldBind()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""
			DataFieldBind()
		End If
	End Sub

	Private Sub GridColumnsVisibility()
		'If (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then
		'    dgWOList.Columns(5).Visible = False
		'    dgWOList.Columns(9).Visible = False
		'    dgWOList.Columns(6).Visible = True
		'    dgWOList.Columns(14).Visible = True
		'Else
		'    dgWOList.Columns(5).Visible = True
		'    dgWOList.Columns(9).Visible = True
		'    dgWOList.Columns(6).Visible = False
		'    dgWOList.Columns(14).Visible = False
		'End If
	End Sub
#End Region

#Region "DataFieldBind"
	Private Sub DataFieldBind()
		Session("totcnt") = totcnt 'Added by shweta on 11-1-12
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		'Commented and added by Shweta on 19-August-2013 For  ALL16082013-1
		'DateIndex = IIf(IsNothing(DateIndex), 2, DateIndex)
		DateIndex = IIf(IsNothing(DateIndex), 0, DateIndex)
		'end
		StatusID = Session("StatusID")
		WOStatusID = Session("WOStatusID")
		WOText = Session("WOText")
		RegNo = Session("RegNo")
		ModelName = Session("ModelName")
		IsAllWOsTicked = Session("IsAllWOsTicked")  'Added by Saylee on 14-Jan-2020

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(ALL)")
		cmbWO.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText

		''mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(ALL)", SkipIsForInventoryAircarft:=True)
		mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True, IsTagRequired:=True, TagText:="(ALL)", SkipReadOnlyAircrafts:=True)
		mMachineNameAutoCompleteList = DistinctTextListAutoComplete.GetDistinctTextList(, 28, TagText:="(ALL)")
		cmbAircraft.DataSource = mMachineNameAutoCompleteList
		Session("mMachineNameAutoCompleteList") = mMachineNameAutoCompleteList
		Session("mMachineNameValueList") = mMachineNameValueList

		mWOModelNameValueList = nWOModelNameValueList.GetModelList("(ALL)")
		mModelNameAutoCompleteList = DistinctTextListAutoComplete.GetDistinctTextList(, 29, TagText:="(ALL)")
		cmbModel.DataSource = mModelNameAutoCompleteList
		Session("mWOModelNameValueList") = mWOModelNameValueList
		Session("mModelNameAutoCompleteList") = mModelNameAutoCompleteList


		mWOStatusList = nWOStatusList.GetWOStatusListList(, "(ALL)")  'Added by Saylee on 14-Jan-2020
		cmbStatus.DataSource = mWOStatusList
		Session("mWOStatusList") = mWOStatusList

		DataBind()
	End Sub
	Private Sub SetGrid()
		Dim P As Boolean



		For j As Integer = 0 To dgWOList.Rows.Count - 1
			P = CType(Me.dgWOList.Rows(j).Cells(17).Text, Boolean)
			If Me.dgWOList.Rows.Item(j).Cells(18).Text = Trans.WOCAMO Then
				If Me.dgWOList.Rows.Item(j).Cells(3).Text = "" Or mMachineNameValueList(Me.dgWOList.Rows.Item(j).Cells(3).Text) Is Nothing Then '    If mMachineNameValueList(Me.dgWOList.Rows.Item(j).Cells(3).Text) Is Nothing Then
					IsReadOnly = True
				ElseIf Me.dgWOList.Rows.Item(j).Cells(3).Text = "&nbsp;" Then
					IsReadOnly = False
				Else
					IsReadOnly = mMachineNameValueList(Me.dgWOList.Rows.Item(j).Cells(3).Text).ROCntxt 'Added by Saylee - Restrict User from using ReadOnly Aircraft
				End If

			ElseIf Me.dgWOList.Rows.Item(j).Cells(18).Text = Trans.WO145 Then
				IsReadOnly = False

			End If



			If IsReadOnly = True Then
				dgWOList.Rows(j).Cells(16).Enabled = False
			Else
				dgWOList.Rows(j).Cells(16).Enabled = True
			End If

			'If P = False Then
			'    dgWOList.Rows(j).Cells(18).Visible = False
			'End If

			dgWOList.Rows(j).Cells(15).Enabled = Not (IsReadOnly = True)
			'dgWOList.Rows(j).Cells(16).Enabled = Not (IsReadOnly = True)

			If Me.dgWOList.Rows.Item(j).Cells(10).Text = "QC Rejected" Then
				Me.dgWOList.Rows.Item(j).Cells(10).ForeColor = Color.Red
				Me.dgWOList.Rows.Item(j).Cells(10).Font.Bold = True
			ElseIf Me.dgWOList.Rows.Item(j).Cells(10).Text = "Planned" Or Me.dgWOList.Rows.Item(j).Cells(10).Text = "PPC Completed" Then
				Me.dgWOList.Rows.Item(j).Cells(10).ForeColor = Color.Green
				Me.dgWOList.Rows.Item(j).Cells(10).Font.Bold = True
			ElseIf Me.dgWOList.Rows.Item(j).Cells(10).Text = "AME Completed" Then
				Me.dgWOList.Rows.Item(j).Cells(10).ForeColor = Color.HotPink
				Me.dgWOList.Rows.Item(j).Cells(10).Font.Bold = True
			End If
		Next

		IsReadOnly = Session("IsReadOnly") 'Added by Saylee
		If IsReadOnly = True Then
			lblReadOnly.Visible = True
		Else
			lblReadOnly.Visible = False
		End If
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		ClearAll()
		addAttributes()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		If Not IsPostBack Then
			Session("MiddleFrame") = "wfnWOCompletionList.aspx?"
			DataFieldBind()
			SetControl()
		Else
			dgWOList.DataSource = mWOCompletionList
			dgWOList.DataBind()
			SetGrid()
		End If
		SetToolTip()
		SetGrid()
		SetTitle()
	End Sub
	Private Sub dgWOList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
					'Exit Sub
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				'  Dim Index As Integer = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
				'   Dim mId As Guid = mWOCompletionList(Index).ID
				Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				Dim mDate As String = mWOCompletionList(mID).WODateFormatted
				Dim mWorkOrderNo As String = mWOCompletionList(mID).WONumber
				Dim mCreatedBy As String = mWOCompletionList(mID).WOBy
				Dim mRegNo As String = IIf(mWOCompletionList(mID).RegNo = "", "", mWOCompletionList(mID).RegNo)
				Dim mModel As String = IIf(mWOCompletionList(mID).ModelName = "", "", mWOCompletionList(mID).ModelName)
				Dim mSerialNo As String = mWOCompletionList(mID).SerialNo
				mWODetail = mWorkOrderNo + " Dated : " + mDate + " Created By : " + mCreatedBy + IIf(mRegNo <> "", " Aircraft : " + mRegNo, "") + IIf(mModel <> "", " Model : " + mModel, "") + IIf(mSerialNo <> "", " Serial No. : " + mSerialNo, "")
				MarkLog(Util.Action.Edit, "Work Order", mWODetail, Util.ErrorType.NoError, mID, EventLogID)
				EditRecord(mID)
				Session("Edit") = True
				DataFieldBind()
				SetControl()
				SetTitle()
				SetGrid()
				GridColumnsVisibility()
				upnlGridView.Update()
				upnlActionBtnTop.Update()
				upnlActionBtnBottom.Update()
				upnlResult.Update()

				If chkShowAllWOs.Checked And Not (cmbStatus.SelectedIndex = 3 Or cmbStatus.SelectedIndex = 7 Or cmbStatus.SelectedIndex = 6) Then  'Added by Saylee on 14-Jan-2020
					Session("IsShowAllWOs") = True
				Else
					Session("IsShowAllWOs") = False
				End If

				Dim str As String
				str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
			Case "DeleteRec"
				If (Not IsInRole(Rights.Delete)) Then
					'  ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				' Dim Index As Integer = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
				' Dim mId As Guid = mWOCompletionList(Index).ID
				Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				DeleteRecord(mID)

				'************ Commented by Saylee on 7 Feb 2019 ,no merging of multiple attachments ***********************
				'Case "ViewRec"
				'    '----------------------------------------------------------------------
				'    Dim No As New Random
				'    Dim StrName As String = "abc" & No.Next.ToString
				'    '----------------------------------------------------------------------
				'    Dim Idx As Int32
				'    Dim mID As Guid

				'    'Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				'    'Dim rowIndex As Integer = gvr.RowIndex
				'    'Idx = rowIndex '+ dgEmployeeList.PageIndex * dgEmployeeList.PageSize
				'    'mID = New Guid(dgWOList.DataKeys(Idx).Value.ToString)
				'    Idx = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
				'    mID = mWOCompletionList(Idx).ID
				'    mnWO = nWO.GetWO(mID)
				'    GridColumnsVisibility()
				'    If Not mnWO.FileAttachments.Contains(".pdf") Then
				'        MSGBoxCtrl.show("Attachment Alert!!", "No PDF File Attached!!", "Files attached does not have any file with .pdf extension. Here only pdf files will be viewed", MsgBoxStyle.OkOnly, "")
				'        Exit Sub
				'    End If

				'    Dim path As String = AppSettings("DOCPath") & StrName & mnWO.FileExtension

				'    DataFieldBind()
				'    SetControl()
				'    SetTitle()
				'    SetGrid()
				'    upnlGridView.Update()
				'    upnlActionBtnTop.Update()
				'    upnlActionBtnBottom.Update()
				'    upnlResult.Update()

				'    Dim PDFNoChild As Integer = 1
				'    ' PDFNoChild = PDFNoChild + 1
				'    Dim pdfList As New System.Collections.ArrayList

				'    For j As Integer = 0 To mnWO.FileAttachments.Count - 1
				'        If mnWO.FileAttachments(j).Size > 0 And mnWO.FileAttachments(j).Extension = ".pdf" Then
				'            Dim ChildAttachment_path As String = "C:\Temp\" & mnWO.WONumber & PDFNoChild.ToString & mnWO.FileAttachments(j).Extension

				'            Dim fs As FileStream
				'            If File.Exists(AppSettings("DOCPath")) = False Then
				'                'Delete File if exist
				'                System.IO.File.Delete(ChildAttachment_path)
				'                ' Create the file.
				'                fs = File.Create(ChildAttachment_path)
				'                '' Add some information to the file.
				'                fs.Write(mnWO.FileAttachments(j).ImageFile, 0, mnWO.FileAttachments(j).ImageFile.Length)
				'                fs.Close()
				'                'Session("DOCPath") = path
				'                '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
				'                pdfList.Add(ChildAttachment_path)                               '2. TaskCardAttachment attachment
				'                ' PDFNo = PDFNo + 1
				'                PDFNoChild = PDFNoChild + 1
				'            End If
				'        End If

				'    Next

				'    Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
				'    Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

				'    Dim filesByte As New List(Of Byte())()
				'    For Each file__1 As String In pdfList 'files
				'        filesByte.Add(File.ReadAllBytes(file__1))
				'    Next

				'    File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))

				'    AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WONumber, , , iTextSharp.text.BaseColor.GRAY, , 0.0, 0)
				'    ''//********************************************Set Sessions*********************************************************//
				'    Session("CrystalReport") = MergedPath_WM
				'    Session("PrintReportWithAttachment") = "True"

				'    '//*******************************************Delete created file*********************************************************//

				'    Dim DeleteThis As String = mnWO.WONumber
				'    Dim Files As String() = Directory.GetFiles("C:\Temp\")

				'    For Each file__1 As String In Files
				'        If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
				'            File.Delete(file__1)
				'        End If
				'    Next
				'    'End
				'    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)

				'************ Added by Saylee on 7 Feb 2019 ,to give choice of print single from multiple attachments ***********************
			Case "ViewRec"
				Dim mFileAttachments As New FileAttachments

				Dim Idx As Int32
				Dim mID As Guid
				'  Idx = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
				'  mID = mWOCompletionList(Idx).ID
				mID = New Guid(e.CommandArgument.ToString)

				mnWO = nWO.GetWO(mID)

				mFileAttachments = FileAttachments.GetChildFileAttachments(mnWO.ID)

				'Dim AttachmentCount As Integer = mnWO.FileAttachments.Count
				Dim AttachmentCount As Integer = mFileAttachments.Count

				GridColumnsVisibility()
				DataFieldBind()
				SetControl()
				SetTitle()
				SetGrid()
				upnlGridView.Update()
				upnlActionBtnTop.Update()
				upnlActionBtnBottom.Update()
				upnlResult.Update()
				Session("mnWO") = mnWO

				If AttachmentCount > 1 Then
					'Session("mFileAttachments") = mnWO.FileAttachments
					Session("mFileAttachments") = mFileAttachments
					Session("TransactionNameMarkLog") = "Work Order" 'used for marklog
					Session("TransactionName") = "Work Order No. & Date"
					Session("TransactionDetails") = mnWO.WONumber + " & " + mnWO.WODateFormatted.ToString
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)

				Else
					Dim mFileAttach As FileAttach
					Dim No As New Random
					Dim StrName As String = "abc" & No.Next.ToString

					mFileAttach = FileAttach.GetAttachment(mID, , mnWO.FileAttachments(0).FileName)
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
							MarkLog(Util.Action.View, "Work Order", Detail, Util.ErrorType.HandledError, mWOCompletionList(Idx).ID, EventLogID)
						End If
					End If
				End If
		End Select
	End Sub
	Private Sub dgWOList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
		dgWOList.PageIndex = e.NewPageIndex
		dgWOList.DataSource = mWOCompletionList
		Session("mWOCompletionList") = mWOCompletionList

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOList.Columns(2).HeaderText = "E.O. No."
			'dgWOList.Columns(12).HeaderText = "E.O. Status"
		Else
			dgWOList.Columns(2).HeaderText = "W.O. No."
			'dgWOList.Columns(12).HeaderText = "W.O. Status"
		End If
		GridColumnsVisibility()
		dgWOList.DataBind()
		SetGrid()
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		setVariables()
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		SetGrid()

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOList.Columns(2).HeaderText = "E.O. No."
			'dgWOList.Columns(12).HeaderText = "E.O. Status"
			lblResult.Text = "List for Engineering Order Closing/Completion as per criteria :" & mWOCompletionList.Count & " Record(s) found."
		Else
			dgWOList.Columns(2).HeaderText = "W.O. No."
			'dgWOList.Columns(12).HeaderText = "W.O. Status"
			lblResult.Text = "List for Work Order Closing/Completion as per criteria :" & mWOCompletionList.Count & " Record(s) found."
		End If
		GridColumnsVisibility()
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()

		'lblResult.Text = "List of Work Order as per criteria :" & mWOCompletionList.Count & " Record(s) found"
	End Sub

	Private Sub cmbWO_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWO.SelectedIndexChanged
		ClearControls()

		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(DateIndex)


	End Sub
	Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
		ClearControls()

		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(DateIndex)
		setPeriod(DateIndex)
		If cmbDate.Enabled = True Then
			setFocus(cmbDate)
		End If

	End Sub
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		'ModuleName = Nothing
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgWOList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOList.Sorting
		mWOCompletionList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOList.DataSource = mWOCompletionList
		Session("mWOCompletionList") = mWOCompletionList
		dgWOList.DataBind()
		SetGrid()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Protected Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged
		'FindNow

		If mMachineNameValueList.Contains(cmbAircraft.SelectedValue.ToString) Then
			IsReadOnly = mMachineNameValueList(cmbAircraft.SelectedValue).ROCntxt 'Added by Saylee - Restrict User from using ReadOnly Aircraft
			Session("IsReadOnly") = IsReadOnly
		End If

		SetGrid()
		upnlGridView.Update()
		GridColumnsVisibility()
	End Sub
	Private Sub chkShowAllWOs_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkShowAllWOs.CheckedChanged  'Added by Saylee on 14-Jan-2020
		If chkShowAllWOs.Checked = False Then
			chkShowAllWOs.Checked = False
			phStatus.Visible = False
		ElseIf chkShowAllWOs.Checked = True Then
			chkShowCompletedJobs.Checked = False
			phStatus.Visible = True

		End If
		cmbStatus.SelectedIndex = 0
	End Sub
	Private Sub chkShowCompletedJobs_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkShowCompletedJobs.CheckedChanged  'Added by Saylee on 14-Jan-2020
		chkShowAllWOs.Checked = False
		phStatus.Visible = False
		cmbStatus.SelectedIndex = 0
	End Sub
#End Region

#Region "Log"
	Private Function CheckZeroDifferenceValue(mLog As Log) As Boolean
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
		If Not callZeroDifferenceValue(checkcol, mLog) Then
			Return False
		End If
		checkcol = mLog.LogAPUAssemblies
		If Not callZeroDifferenceValue(checkcol, mLog) Then
			Return False
		End If
		checkcol = mLog.LogEngAssemblies
		If Not callZeroDifferenceValue(checkcol, mLog) Then
			Return False
		End If
		checkcol = mLog.LogCGBAssemblies
		If Not callZeroDifferenceValue(checkcol, mLog) Then
			Return False
		End If
		Return True
	End Function
	Private Function callZeroDifferenceValue(ByVal obj As Object, ByVal mLog As Log) As Boolean
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
	Public Function IsEngineHoursSame(mLog As Log) As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
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
	Public Function IsCGBHoursSame(mLog As Log) As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
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
	Private Function SaveLog(mLog As Log) As Boolean
		Dim LogClone As Log
		Dim mtmpLog As Log

		LogClone = CType(mLog.Clone, Log)

		For i As Integer = 0 To mLog.LogFuels.Count - 1
			mLog.LogFuels.Item(i).WOFuelUplifted = 0
			mLog.LogFuels.Item(i).WOFuelDrainedOut = 0
		Next i

		If Not mLog.IsNew Then
			Dim mUpperLogNo As MaxLogNo
			mUpperLogNo = MaxLogNo.GetUpperLog(mLog.ID, mLog.MachineID)   'Gets the just immediate upper log
			If mUpperLogNo IsNot Nothing Then
				If mUpperLogNo.Count > 0 Then
					mtmpLog = Log.GetLog(mUpperLogNo(0).LogId)
					For i As Integer = 0 To mLog.LogFuels.Count - 1
						mtmpLog.LogFuels.Item(i).FuelOnDeparture = mLog.LogFuels.Item(i).FuelOnArrival
					Next i
				End If
			End If
		End If
		If mLog.IsValid = True Then
			If Not CheckZeroDifferenceValue(mLog) Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
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
				'If IsEngineHoursSame(mLog) = False Or IsCGBHoursSame(mLog) = False Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
				'    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo)
				'    ' '' ''msg1.ReplacePage = "wfLogFuelOil.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
				'    ' '' ''Session("sender") = "SaveLogAfterHrsSame"
				'    ' '' ''msg1.Show()
				'    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
				'    Exit Function
				'End If

				mLog.ApplyEdit()
				mLog = CType(mLog.Save(), Log)
				'MarkLog(Util.Action.[New], "Log", "Aircraft Name ->" + mLog.Machine.RegNo + " Tank-> " + mTankList.Item(mTankList.CurrentIndex).Name, Util.ErrorType.NoError, New Guid(cmbTankList.SelectedValue.ToString))
				Dim mUpdateFuelsOfAllAboveLogs As UpdateFuelsOfAllAboveLogs
				mUpdateFuelsOfAllAboveLogs = UpdateFuelsOfAllAboveLogs.GetLogFuelAndOilList(mLog.ID, mLog.MachineID)
				Try
					Dim mUpdateFuelsOfAllAboveLogsInfo As UpdateFuelsOfAllAboveLogs.UpdateFuelsOfAllAboveLogsInfo
					Dim mtmpLogFuelList As LogFuelList
					If mUpdateFuelsOfAllAboveLogs.Count > 0 Then
						For Each mUpdateFuelsOfAllAboveLogsInfo In mUpdateFuelsOfAllAboveLogs
							mtmpLogFuelList = LogFuelList.GetLogFuelList(mUpdateFuelsOfAllAboveLogsInfo.ID)
							For i As Integer = 0 To mtmpLogFuelList.Count - 1
								mUpdateFuelsOfAllAboveLogs.UpdateLogFuels(mtmpLogFuelList(i).LogFuelId, mtmpLogFuelList(i).FuelOnArrival, mUpdateFuelsOfAllAboveLogsInfo)
							Next i
						Next
					Else
						If mtmpLog IsNot Nothing Then
							If mtmpLog.IsValid = True Then
								mtmpLog.ApplyEdit()
								mtmpLog = CType(mtmpLog.Save(), Log)
							End If
						End If
					End If
				Catch ex As Exception

				End Try
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				'If ex.Number = 8114 Or ex.Number = 8115 Then
				'    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " ", MsgBoxStyle.OkOnly, " ")
				'ElseIf ex.Number = 8145 Then
				'    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, " ")
				'ElseIf ex.Number = 2627 Then
				'    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, " ")
				'ElseIf ex.Number = 547 Then
				'    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, " ")
				'End If
				Return False
			Finally
				LogClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function
#End Region



End Class