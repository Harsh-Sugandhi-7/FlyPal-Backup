'Added By Vikrant On 17-Dec-2019 For Gantt Chart
Public Class wfUpdateWOJobTaskDateTimeList_Ajax
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
	Public mWOList As nWOList
	Dim mMachineNameValueList As MachineNameValueList
	Dim mWOModelNameValueList As nWOModelNameValueList
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, StatusID, No, WOStatusID, RegNo, ModelName As String
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mWODetail As String
	Dim totcnt As Integer

	Dim IsReadOnly As Boolean 'Added by Saylee

	Public mTransTypeID As Trans 'Added by Saylee on 5-Sep-2018
#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mWOList = Session("mWOList")
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
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mWOList")
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
	End Sub
	Private Sub ClearAll()
		mTransTypeID = Session("mTransTypeId") 'Added by Saylee on 5-Sep-2018
		'If InStr(Session("MiddleFrame"), "wfnWOList_AJAX.aspx?TransTypeId=" & mTransTypeID) <= 0 Then
		If Session("MiddleFrame") <> "wfUpdateWOJobTaskDateTimeList_Ajax.aspx?" Then
			RemoveSession()
			Session.Remove("mWOList")
		End If
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
		'Dim mWOList As nWOList
		'mWOList = nWOList.GetWOList()
		'totcnt = Session("totcnt")
		'Commented by Saylee on 3-Mar-2017 for performance
		'  lblTitle.Text = "List of Work Order    [Total No of Record(s):-" + totcnt.ToString() + "]"  'shweta
		mWOList = Session("mWOList")
		totcnt = mWOList.TotalWOCount
		Session("totcnt") = totcnt
		lblTitle.Text = "List of Submitted Work Order(s)"
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

		SearchIndex = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
		DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)         '--Changed By Utkarsh On 17-Jan-2011
		RegNo = IIf(cmbAircraft.SelectedIndex <= 0, "", cmbAircraft.SelectedValue)
		ModelName = IIf(cmbModel.SelectedIndex <= 0, "", cmbModel.SelectedValue) '--Changed By Utkarsh On 17-Jan-2011
		No = txtNo.Text.Trim
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
	End Sub


	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		'Deciding IsInRole String to check Rights
		Select Case mTransTypeID
			Case Util.Trans.WO145
				IsInRoleString = "WorkOrder"
			Case Util.Trans.WOCAMO
				IsInRoleString = "CAMOWO"
			Case Util.Trans.SpareAssemblyWO
				IsInRoleString = "SpareAssemblyWO"
			Case Util.Trans.SpareComponentWO
				IsInRoleString = "SpareComponentWO"
		End Select
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
	Private Sub SetControl()
		setPeriod(DateIndex)
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		cmbSearch.SelectedIndex = SearchIndex
		cmbDate.SelectedIndex = DateIndex
		cmbAircraft.SelectedValue = IIf(RegNo = "", "(ALL)", RegNo)

		cmbModel.SelectedValue = IIf(ModelName = "", "(ALL)", ModelName) '--Changed By Utkarsh On 17-Jan-2011
		cmbWO.SelectedValue = IIf(WOText = "", "(ALL)", WOText) '--Changed By Utkarsh On 17-Jan-2011
		txtNo.Text = No
		ControlVisibility(SearchIndex, DateIndex)
		dgWOList.DataBind()
		lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
	End Sub

	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal RegNo As String = "", Optional ByVal ModelName As String = "", Optional ByVal WOStatusID As Integer = 0, Optional ByVal StatusID As Integer = 0, Optional ByVal AddTopItem As String = "")
		mWOList = Nothing
		dgWOList.DataSource = Nothing

		mWOList = nWOList.GetWOList(Text, No, FromDate, ToDate, RegNo, ModelName, 2, 1, AddTopItem)
		dgWOList.DataSource = mWOList
		Session("mWOList") = mWOList
		totcnt = mWOList.TotalWOCount
		Session("totcnt") = totcnt
	End Sub

	Private Sub CallFindNow(ByVal Index As Integer)
		Select Case Index
			Case -1 'all
				FindNow()
			Case 0 'all
				FindNow(, , , , , , , , "(ALL)")
			Case 1 'WO Date date
				FindNow(, , txtFromDate.Text.ToString, txtToDate.Text.ToString, , , , , )
			Case 2  'Work Order
				FindNow(WOText, CInt(Val(No)), , , , , , , )
			Case 3  'Aircraft
				FindNow(, , , , RegNo, , , , )
			Case 4 'Model
				FindNow(, , , , , ModelName)
			Case 5  'Status
				FindNow(, , , , , , WOStatusID, , )
			Case 6  'DocStatus
				FindNow(, , , , , , , StatusID, )

		End Select
	End Sub

	Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)

		cmbDate.Visible = IIf(SearchIndex = 1, True, False)
		'lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		'lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		'txtFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		'txtToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		If SearchIndex = 1 And DateIndex = 6 Then
			txtFromDate.Enabled = True
			txtToDate.Enabled = True
		ElseIf SearchIndex = 1 And (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
			txtFromDate.Enabled = False
			txtToDate.Enabled = False
		End If

		cmbWO.Visible = IIf(SearchIndex = 2, True, False)
		cmbAircraft.Visible = IIf(SearchIndex = 3, True, False)
		cmbModel.Visible = IIf(SearchIndex = 4, True, False)
		txtNo.Visible = IIf(SearchIndex = 2 And cmbWO.SelectedIndex <> 0, True, False) '--Changed By Utkarsh On 17-Jan-2011
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

				Case MsgBoxResult.No
					Session("sender") = ""
				Case MsgBoxResult.Ok
					Session("sender") = ""

				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added

			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""

		End If
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
		DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
		'end
		StatusID = Session("StatusID")
		WOStatusID = Session("WOStatusID")
		WOText = Session("WOText")
		RegNo = Session("RegNo")
		ModelName = Session("ModelName")

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(ALL)", TransTypeID:=mTransTypeID)
		cmbWO.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText

		mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(ALL)", SkipIsForInventoryAircarft:=True)
		cmbAircraft.DataSource = mMachineNameValueList
		Session("mMachineNameValueList") = mMachineNameValueList

		mWOModelNameValueList = nWOModelNameValueList.GetModelList("(ALL)")
		cmbModel.DataSource = mWOModelNameValueList
		Session("mWOModelNameValueList") = mWOModelNameValueList

		DataBind()
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		ClearAll()
		addAttributes()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack Then
			Session("MiddleFrame") = "wfUpdateWOJobTaskDateTimeList_Ajax.aspx?"
			If cmbSearch.Enabled = True Then
				setFocus(cmbSearch)
			End If
			DataFieldBind()
			SetControl()
		Else
			dgWOList.DataSource = mWOList
			dgWOList.DataBind()
		End If
		SetTitle()
	End Sub
	Private Sub dgWOList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
		Select Case e.CommandName

			Case "UpdateRec"
				Dim mFileAttachments As New FileAttachments

				Dim Idx As Int32
				Dim mID As Guid
				Idx = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
				mID = mWOList(Idx).ID
				mnWO = nWO.GetWO(mID, False)

				Session("mnWO") = mnWO
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenUpdateWindow", "OpenUpdateWindow();", True)
		End Select
	End Sub
	Private Sub dgWOList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
		dgWOList.PageIndex = e.NewPageIndex
		dgWOList.DataSource = mWOList
		Session("mWOList") = mWOList
		dgWOList.DataBind()
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		If IsValid Then
			setVariables()
			CallFindNow(SearchIndex)
			dgWOList.DataBind()
			dgWOList.DataBind()
			upnlGridView.Update()
			upnlActionBtnTop.Update()
			upnlActionBtnBottom.Update()
			upnlResult.Update()
		End If
		'lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
	End Sub
	Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
		ClearControls()
		cmbDate.SelectedIndex = 0
		cmbWO.SelectedIndex = 0
		cmbAircraft.SelectedIndex = 0
		cmbModel.SelectedIndex = 0
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)

		'FindNow
		setVariables()
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
		'--------------------------------------
		If cmbSearch.Enabled = True Then
			setFocus(cmbSearch)
		End If
	End Sub
	Private Sub cmbWO_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWO.SelectedIndexChanged
		ClearControls()

		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbWO.Enabled = True Then
			setFocus(cmbWO)
		End If

		'FindNow
		setVariables()
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
		'--------------------------------------
	End Sub
	Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		setPeriod(DateIndex)
		If cmbDate.Enabled = True Then
			setFocus(cmbDate)
		End If

		'FindNow
		setVariables()
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
		'--------------------------------------
	End Sub

	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Session.Remove("IsReadOnly")
		'ModuleName = Nothing
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgWOList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOList.Sorting
		mWOList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOList.DataSource = mWOList
		Session("mWOList") = mWOList
		dgWOList.DataBind()

	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
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
	Protected Sub cmbModel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbModel.SelectedIndexChanged, cmbAircraft.SelectedIndexChanged, txtNo.TextChanged, txtFromDate.TextChanged, txtToDate.TextChanged
		'FindNow

		IsReadOnly = mMachineNameValueList(cmbAircraft.SelectedValue).IsReadOnly 'Added by Saylee - Restrict User from using ReadOnly Aircraft
		Session("IsReadOnly") = IsReadOnly

		setVariables()
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()
		'--------------------------------------
		Session.Remove("IsReadOnly")
	End Sub

End Class