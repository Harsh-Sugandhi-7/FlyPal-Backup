'Ajax Conversion By Vikrant On 28-Jan-2014

Public Class wfrptIssueTonWO_Ajax
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mnWOListForIssueToWO As nWOListForIssueToWO
	Public mWOStatusList As nWOStatusList
	Dim mMachineNameValueList As MachineNameValueList
	Dim mWOModelNameValueList As nWOModelNameValueList
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, StatusID, No, WOStatusID, RegNo, ModelName, IssueToWoTypeID As String
	Dim EventLogDetail As String = String.Empty
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mWOStatusList = Session("mWOStatusList")
		mnWOListForIssueToWO = Session("mnWOListForIssueToWO")
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
		IssueToWoTypeID = Session("IssueToWoTypeID")
	End Sub
	Private Sub SetSession()
		Session("mWOStatusList") = mWOStatusList
		Session("mnWOListForIssueToWO") = mnWOListForIssueToWO
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
		Session("IssueToWoTypeID") = IssueToWoTypeID
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mWOStatusList")
		Session.Remove("mnWOListForIssueToWO")
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
		Session.Remove("IssueToWoTypeID")
	End Sub
	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "wfrptIssueTonWO_Ajax.aspx") <= 0 Then
			RemoveSession()
			Session.Remove("mnWOListForIssueToWO")
		End If
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub
	Private Sub setPeriod(ByVal Index As Int32)
		Select Case Index
			Case 0 'All   
				txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
				txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
			Case 1 'Last 1 Week
				txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			Case 2 'Last 1 Month
				txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			Case 3 'Last 1 Quater
				Select Case Today.Month
					Case 1, 2, 3
						txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
						txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
					Case 4, 5, 6
						txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
						txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
					Case 7, 8, 9
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
						txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
					Case 10, 11, 12
						txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
						txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
				End Select
			Case 4 'Last 1 Year
				txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat"))
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			Case 5 'Current Financial Year
				If Today.Month <= 3 Then  'Jan|Feb|Mar
					txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
				Else
					txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))   '31-Mar-2006
				End If
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			Case 6 'Between Dates
				txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
		End Select
	End Sub
	Private Sub setVariables()
		SearchIndex = IIf(cmbSearch.SelectedIndex <= 0, 0, cmbSearch.SelectedIndex)
		DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Text <> "", txtFromDate.Text, "1/1/1900")
		ToDate = IIf(txtToDate.Text <> "", txtToDate.Text, "1/1/2200")

		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)         '--Changed By Utkarsh On 17-Jan-2011
		WOStatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
		StatusID = IIf(cmbDocStatus.SelectedIndex <= 0, 0, cmbDocStatus.SelectedValue)
		RegNo = IIf(cmbAircraft.SelectedIndex <= 0, "", cmbAircraft.SelectedItem.ToString)
		ModelName = IIf(cmbModel.SelectedIndex <= 0, "", cmbModel.SelectedItem.ToString) '--Changed By Utkarsh On 17-Jan-2011
		No = txtNo.Text.Trim
		IssueToWoTypeID = IIf(cmbIssueType.SelectedIndex <= 0, 0, cmbIssueType.SelectedValue)

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
		Session("IssueToWoTypeID") = IssueToWoTypeID
	End Sub
	Private Sub SetToolTip()
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblTitle.Text = "List of Engineering Order"
			btnClose.ToolTip = "Click to close List of Engineering Order screen"
			btnFindNow.ToolTip = "Click to find list of Engineering Order as per searching criteria"
		Else
			lblTitle.Text = "List of Work Order"
			btnClose.ToolTip = "Click to close List of Work Order screen"
			btnFindNow.ToolTip = "Click to find list of Work Order as per searching criteria"
		End If
		upnlTitle.Update()
	End Sub
	Private Sub SetEventLogDetail()
		If cmbSearch.SelectedIndex = 0 Then
			EventLogDetail = "All"
		ElseIf cmbSearch.SelectedIndex = 1 Then
			EventLogDetail = IIf(cmbDate.SelectedIndex <= 0, "Date : All", "Date : From Date : " + txtFromDate.Text + " To Date : " + txtToDate.Text)
		ElseIf cmbSearch.SelectedIndex = 2 Then
			EventLogDetail = IIf(cmbWO.SelectedIndex <= 0, "W.O. : All", "W.O. : " + cmbWO.SelectedItem.ToString + "," + txtNo.Text)
		ElseIf cmbSearch.SelectedIndex = 3 Then
			EventLogDetail = IIf(cmbAircraft.SelectedIndex <= 0, "Aircraft : All", "Aircraft : " + cmbAircraft.SelectedItem.ToString)
		ElseIf cmbSearch.SelectedIndex = 4 Then
			EventLogDetail = IIf(cmbModel.SelectedIndex <= 0, "Model : All", "Model : " + cmbModel.SelectedItem.ToString)
		ElseIf cmbSearch.SelectedIndex = 5 Then
			EventLogDetail = IIf(cmbStatus.SelectedIndex <= 0, "Status : All", "Status : " + cmbStatus.SelectedItem.ToString)
		ElseIf cmbSearch.SelectedIndex = 6 Then
			EventLogDetail = IIf(cmbDocStatus.SelectedIndex <= 0, "Doc Status : All", "Doc Status : " + cmbDocStatus.SelectedItem.ToString)
		ElseIf cmbSearch.SelectedIndex = 7 Then
			EventLogDetail = IIf(cmbIssueType.SelectedIndex <= 0, "Issue to WO Type : All", "Issue to WO Type : " + cmbIssueType.SelectedItem.ToString)
		End If
	End Sub
	Private Sub SetControl()
		ControlVisibility2(DateIndex)
		setPeriod(DateIndex)
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		cmbSearch.SelectedIndex = SearchIndex
		cmbDate.SelectedIndex = DateIndex
		cmbAircraft.SelectedValue = IIf(RegNo = "", "(SELECT)", RegNo)

		cmbModel.SelectedValue = IIf(ModelName = "", "(All)", ModelName) '--Changed By Utkarsh On 17-Jan-2011
		cmbWO.SelectedValue = IIf(WOText = "", "(All)", WOText) '--Changed By Utkarsh On 17-Jan-2011
		txtNo.Text = No
		cmbStatus.SelectedValue = WOStatusID
		cmbDocStatus.SelectedValue = StatusID
		cmbIssueType.SelectedValue = IssueToWoTypeID
		ControlVisibility(SearchIndex, DateIndex)

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOList.Columns(2).HeaderText = "E.O. No."
			dgWOList.Columns(11).HeaderText = "E.O. Status"

			dgWOList.DataBind()
			lblResult.Text = "List of Engineering Order as per criteria :" & mnWOListForIssueToWO.Count & " Record(s) found."
		Else
			dgWOList.Columns(2).HeaderText = "W.O. No."
			dgWOList.Columns(11).HeaderText = "W.O. Status"

			dgWOList.DataBind()
			lblResult.Text = "List of Work Order as per criteria :" & mnWOListForIssueToWO.Count & " Record(s) found."
		End If

	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub
	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal RegNo As String = "", Optional ByVal ModelName As String = "", Optional ByVal WOStatusID As Integer = 0, Optional ByVal StatusID As Integer = 0, Optional ByVal AddTopItem As String = "", Optional ByVal IssueToWoTypeID As Integer = 0)
		mnWOListForIssueToWO = Nothing
		dgWOList.DataSource = Nothing

		mnWOListForIssueToWO = nWOListForIssueToWO.GetWOListForIssueToWO(Text, No, FromDate, ToDate, RegNo, ModelName, StatusID, WOStatusID, AddTopItem, IssueToWoTypeID)
		dgWOList.DataSource = mnWOListForIssueToWO
		Session("mnWOListForIssueToWO") = mnWOListForIssueToWO
	End Sub
	Private Sub CallFindNow(ByVal Index As Integer)
		Select Case Index
			Case -1 'all
				FindNow()
			Case 0 'all
				FindNow(, , , , , , , , "(ALL)")
			Case 1 'WO Date date
				FindNow(, , txtFromDate.Text, txtToDate.Text, , , , , )
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
			Case 7  'DocStatus
				FindNow(, , , , , , , , , IssueToWoTypeID)

		End Select
	End Sub
	Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
		cmbDate.Visible = IIf(SearchIndex = 1, True, False)
		lblFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		lblToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		txtFromDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		txtToDate.Visible = IIf(SearchIndex = 1 And DateIndex <> 0, True, False)
		cmbWO.Visible = IIf(SearchIndex = 2, True, False)
		cmbAircraft.Visible = IIf(SearchIndex = 3, True, False)
		cmbModel.Visible = IIf(SearchIndex = 4, True, False)
		cmbStatus.Visible = IIf(SearchIndex = 5, True, False)
		cmbDocStatus.Visible = IIf(SearchIndex = 6, True, False)
		cmbIssueType.Visible = IIf(SearchIndex = 7, True, False)
		txtNo.Visible = IIf(SearchIndex = 2 And cmbWO.SelectedIndex <> 0, True, False) '--Changed By Utkarsh On 17-Jan-2011

	End Sub
	Private Sub ControlVisibility2(ByVal Index As Int16)
		lblFromDate.Visible = IIf(Index <> 0, True, False)
		lblToDate.Visible = IIf(Index <> 0, True, False)

		If Index = 6 Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = True
			txtToDate.Enabled = True
		ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = False
			txtToDate.Enabled = False
		Else
			txtFromDate.Visible = False
			txtToDate.Visible = False
		End If
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
					'Response.Redirect("wfrptIssueTonWO_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
				Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added
					Session("sender") = ""
					'DataFieldBind()
					'Response.Redirect("wfrptIssueTonWO_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					'DataFieldBind()
					'Response.Redirect("wfrptIssueTonWO_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			'Response.Redirect("wfrptIssueTonWO_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""
			'DataFieldBind()
		End If
	End Sub
#End Region

#Region "DataFieldBind"
	Private Sub DataFieldBind()
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		DateIndex = IIf(IsNothing(DateIndex), 6, DateIndex)
		StatusID = Session("StatusID")
		WOStatusID = Session("WOStatusID")
		WOText = Session("WOText")
		RegNo = Session("RegNo")
		ModelName = Session("ModelName")

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
		cmbWO.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText

		mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, , , , , , , True, "(SELECT)", ForInventory:=True)
		cmbAircraft.DataSource = mMachineNameValueList
		Session("mMachineNameValueList") = mMachineNameValueList
		cmbAircraft.DataBind()

		mWOModelNameValueList = nWOModelNameValueList.GetModelList("(All)")
		cmbModel.DataSource = mWOModelNameValueList
		Session("mWOModelNameValueList") = mWOModelNameValueList

		mWOStatusList = nWOStatusList.GetWOStatusListList(, "(All)")
		cmbStatus.DataSource = mWOStatusList
		Session("mWOStatusList") = mWOStatusList

		' mnWOListForIssueToWO = nWOList.GetWOList(, , , , , , , , "(All)")
		'mnWOListForIssueToWO = nWOListForIssueToWO.GetWOListForIssueToWO(, , , , , , , , "(All)")
		'dgWOList.DataSource = mnWOListForIssueToWO
		'Session("mnWOListForIssueToWO") = mnWOListForIssueToWO

		'lblResult.Text = "List of Work Order as per criteria :" & mnWOListForIssueToWO.Count & " Record(s) found."
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
			Session("MiddleFrame") = "wfrptIssueTonWO_Ajax.aspx"
			If cmbSearch.Enabled = True Then
				setFocus(cmbSearch)
			End If
			DataFieldBind()
			SetControl()
			SetToolTip()
		End If
	End Sub
	Private Sub dgWOList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
		Select e.CommandName
			Case "EditRec"
				Dim index As Integer = dgWOList.PageIndex * dgWOList.PageSize + CInt(e.CommandArgument)
				Dim ID As Guid = mnWOListForIssueToWO(index).ID 'New Guid(dgWOList.DataKeys(index).Value.ToString)

				Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
				Dim objSearch As rptSearchingCriteriaForReceipt
				'        Dim objReg As rptIssueToWOReg
				Dim objReg As rptIssueTOnWOReg
				Dim da As New CSLA.Data.ObjectAdapter
				'Dim dsReceipt As New dsReceipt
				Dim dsReceipt As New dsIssue
				Dim ReportName As String
				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then 'Added by Archana on Dec8,2009 for TAAL
					ReportName = "Issue To Engineering Order Report"
				Else
					ReportName = "Issue To Work Order Report"
				End If

				myReport = New crptnIssueRegisterLandscapeForRequisition
				'objReg = rptIssueToWOReg.GetIssueTOWOList(ID)
				objReg = rptIssueTOnWOReg.GetIssueTOnWOList(ID)
				objSearch = rptSearchingCriteriaForReceipt.GetSearchingCriteriaForReceipt(New Guid("{249760E7-93F9-40BD-B4D8-0DD7D4E7C450}"), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ReportName, "", "", "", "", "", "", "", 0, "", "", AppSettings("Logo"))
				If objReg.Count <= 0 Then
					MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
					Exit Sub
				Else
					RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1109)
				End If

				dsReceipt.Clear()
				Dim mrptImage As rptImage = rptImage.GetImage(dsReceipt) 'Added by Shweta on 22-Feb-2012
				da.Fill(dsReceipt, objReg)
				da.Fill(dsReceipt, mrptImage) 'Added by Shweta on 22-Feb-2012
				da.Fill(dsReceipt, objSearch)
				myReport.SetDataSource(dsReceipt)
				Session("CrystalReport") = myReport
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
				SetEventLogDetail()
				MarkLog(Util.Action.Print, "IssueToWorkOrderReport", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
		End Select
	End Sub
	Private Sub dgWOList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
		dgWOList.PageIndex = e.NewPageIndex
		dgWOList.DataSource = mnWOListForIssueToWO
		Session("mnWOListForIssueToWO") = mnWOListForIssueToWO

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOList.Columns(2).HeaderText = "E.O. No."
			dgWOList.Columns(11).HeaderText = "E.O. Status"
		Else
			dgWOList.Columns(2).HeaderText = "W.O. No."
			dgWOList.Columns(11).HeaderText = "W.O. Status"
		End If

		dgWOList.DataBind()
		upnlGrid.Update()
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		setVariables()
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOList.Columns(2).HeaderText = "E.O. No."
			dgWOList.Columns(11).HeaderText = "E.O. Status"
			lblResult.Text = "List of Engineering Order as per criteria :" & mnWOListForIssueToWO.Count & " Record(s) found."
		Else
			dgWOList.Columns(2).HeaderText = "W.O. No."
			dgWOList.Columns(11).HeaderText = "W.O. Status"
			lblResult.Text = "List of Work Order as per criteria :" & mnWOListForIssueToWO.Count & " Record(s) found"
		End If
		upnlGrid.Update()
		'lblResult.Text = "List of WOrk Order as per criteria :" & mnWOListForIssueToWO.Count & " Record(s) found"
	End Sub
	Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
		ClearControls()
		cmbDate.SelectedIndex = 0
		cmbWO.SelectedIndex = 0
		cmbAircraft.SelectedIndex = 0
		cmbModel.SelectedIndex = 0
		cmbStatus.SelectedIndex = 0
		cmbDocStatus.SelectedIndex = 0
		cmbIssueType.SelectedIndex = 0
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		ControlVisibility2(DateIndex)
		setPeriod(DateIndex)
		If cmbSearch.Enabled = True Then
			setFocus(cmbSearch)
		End If
	End Sub

	Private Sub cmbWO_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWO.SelectedIndexChanged
		ClearControls()
		Dim SearchIndex As Int32 = cmbWO.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		ControlVisibility2(DateIndex)
		setPeriod(DateIndex)
		If cmbWO.Enabled = True Then
			setFocus(cmbWO)
		End If
	End Sub
	Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
		Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
		ControlVisibility2(DateIndex)
		setPeriod(DateIndex)
		If cmbDate.Enabled = True Then
			setFocus(cmbDate)
		End If
	End Sub
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgWOList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOList.Sorting
		mnWOListForIssueToWO.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOList.DataSource = mnWOListForIssueToWO
		Session("mnWOListForIssueToWO") = mnWOListForIssueToWO
		dgWOList.DataBind()
		upnlGrid.Update()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
#End Region

End Class