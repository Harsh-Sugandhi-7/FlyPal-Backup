Public Class wfnWOCAMOUpdatList
	Inherits System.Web.UI.Page
#Region " Variable Declaration "
	Public mnWO As nWO
	Public mWOCAMOUpdateList As nWOList
	Public mWOStatusList As nWOStatusList
	Dim mMachineNameValueList As MachineNameValueList
	Dim mWOModelNameValueList As nWOModelNameValueList
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, StatusID, No, WOStatusID, RegNo, ModelName As String
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mWOQCApprovalDetail As String
	Dim totcnt As Integer
	Dim mWOCAMODetail As String
	Dim IsReadOnly As Boolean 'Added by Saylee

	Public mIsForCAMOUpdate As Integer 'Added by Saylee on 5-Sep-2018
	Public BillingRequired As Integer = 0
	Public CAMOUpdateRequired As Integer = 0
	Dim IsAllWOsTicked As Boolean  'Added by Saylee on 14-Jan-2020
#End Region

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


#Region " Business Methods "
	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""

		'IsInRoleString = "WOCAMOUpdate"
		If Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then
			IsInRoleString = "WOCAMOUpdate"
		ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then
			IsInRoleString = "WOBilling"
		End If

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
		End Select
	End Function
	Private Sub GetSession()
		mWOStatusList = Session("mWOStatusList")
		mWOCAMOUpdateList = Session("mWOCAMOUpdateList")
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
		mIsForCAMOUpdate = Session("IsForCAMOUpdate")  'Added by Saylee on 5-Sep-2018
		BillingRequired = Session("BillingRequired")
		CAMOUpdateRequired = Session("CAMOUpdateRequired")
		IsAllWOsTicked = Session("IsAllWOsTicked")  'Added by Saylee on 14-Jan-2020
	End Sub

	Private Sub SetSession()
		Session("mWOStatusList") = mWOStatusList
		Session("mWOCAMOUpdateList") = mWOCAMOUpdateList
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

		Session("IsForCAMOUpdate") = mIsForCAMOUpdate  'Added by Saylee on 5-Sep-2018
		Session("BillingRequired") = BillingRequired
		Session("CAMOUpdateRequired") = CAMOUpdateRequired
		Session("IsAllWOsTicked") = IsAllWOsTicked  'Added by Saylee on 14-Jan-2020
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mWOStatusList")
		Session.Remove("mWOCAMOUpdateList")
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
		Session.Remove("IsForCAMOUpdate") 'Added by Saylee on 5-Sep-2018
		Session.Remove("BillingRequired")
		Session.Remove("CAMOUpdateRequired")
		Session.Remove("IsAllWOsTicked")  'Added by Saylee on 14-Jan-2020
	End Sub

	Private Sub ClearAll()
		' IsForCAMOUpdate = Session("IsForCAMOUpdate") 'Added by Saylee on 5-Sep-2018
		'If InStr(Session("MiddleFrame"), "wfnWOList_AJAX.aspx?TransTypeId=" & IsForCAMOUpdate) <= 0 Then
		If InStr(Session("MiddleFrame"), "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=" & Request.QueryString("IsForCAMOUpdate")) <= 0 Then
			RemoveSession()
			Session.Remove("mWOCAMOUpdateList")
			Session.Remove("IsForCAMOUpdate")
			Session.Remove("BillingRequired")
			Session.Remove("CAMOUpdateRequired")
		End If
	End Sub
	Private Sub SetTitle()
		mWOCAMOUpdateList = Session("mWOCAMOUpdateList")
		totcnt = mWOCAMOUpdateList.TotalWOCount
		Session("totcnt") = totcnt


		If mIsForCAMOUpdate = 1 Then
			lblTitle.Text = "List for Updating CAMO" '   [Total No of Record(s):-" + totcnt.ToString() + "]"
		ElseIf mIsForCAMOUpdate = 0 Then
			lblTitle.Text = "List for Work Order Billing" '   [Total No of Record(s):-" + totcnt.ToString() + "]"
		End If
		' lblTitle.Text = "List of Work Order    [Total No of Record(s):-" + totcnt.ToString() + "]"

		upnlTitle.Update()
	End Sub
	Private Sub SetGrid()
		For j As Integer = 0 To dgCAMOUpdateWOList.Rows.Count - 1

			If dgCAMOUpdateWOList.Rows.Item(j).Cells(9).Text = "Not Required" Then
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(9).ForeColor = Color.Red
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(9).Font.Bold = True
			ElseIf dgCAMOUpdateWOList.Rows.Item(j).Cells(9).Text = "Billing Done" Then
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(9).ForeColor = Color.Green
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(9).Font.Bold = True
			ElseIf dgCAMOUpdateWOList.Rows.Item(j).Cells(9).Text = "None" Then
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(9).ForeColor = Color.Violet
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(9).Font.Bold = True
			End If

			If dgCAMOUpdateWOList.Rows.Item(j).Cells(10).Text = "Not Required" Then
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(10).ForeColor = Color.Red
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(10).Font.Bold = True
			ElseIf dgCAMOUpdateWOList.Rows.Item(j).Cells(10).Text = "CAMO Updated" Then
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(10).ForeColor = Color.Green
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(10).Font.Bold = True
			ElseIf dgCAMOUpdateWOList.Rows.Item(j).Cells(10).Text = "None" Then
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(10).ForeColor = Color.Violet
				Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(10).Font.Bold = True
			End If

			If Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(13).Text = Trans.WOCAMO.ToString Then
				If Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(3).Text = "" Then
					IsReadOnly = True
				ElseIf Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(3).Text = "&nbsp;" Or mMachineNameValueList(Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(3).Text) Is Nothing Then
					IsReadOnly = False
				Else
					IsReadOnly = mMachineNameValueList(Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(3).Text).ROCntxt 'Added by Saylee - Restrict User from using ReadOnly Aircraft
				End If
			ElseIf Me.dgCAMOUpdateWOList.Rows.Item(j).Cells(13).Text = Trans.WO145.ToString Then
				IsReadOnly = False
			End If


			If IsReadOnly = True Then
				dgCAMOUpdateWOList.Rows(j).Cells(12).Enabled = False
			Else
				dgCAMOUpdateWOList.Rows(j).Cells(12).Enabled = True
			End If
		Next

		If mIsForCAMOUpdate = 1 Then 'CAMO
			dgCAMOUpdateWOList.Columns(6).Visible = True   'WOStatusOnCAMOUpdateList
			dgCAMOUpdateWOList.Columns(10).Visible = True
			dgCAMOUpdateWOList.Columns(7).Visible = False  'WOStatusOnBillingList
			dgCAMOUpdateWOList.Columns(9).Visible = False  'Billing Status
		Else 'Billing
			dgCAMOUpdateWOList.Columns(6).Visible = False
			dgCAMOUpdateWOList.Columns(10).Visible = False
			dgCAMOUpdateWOList.Columns(7).Visible = True
			dgCAMOUpdateWOList.Columns(9).Visible = True
		End If
		upnlGridView.Update()

	End Sub

	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "", Optional ByVal ToDate As String = "", Optional ByVal RegNo As String = "", Optional ByVal ModelName As String = "", Optional ByVal WOStatusID As Integer = 0, Optional ByVal StatusID As Integer = 0, Optional ByVal AddTopItem As String = "", Optional BillingRequired As Integer = 0, Optional CAMOUpdateRequired As Integer = 0)
		mWOCAMOUpdateList = Nothing
		dgCAMOUpdateWOList.DataSource = Nothing

		If IsAllWOsTicked = True Then                 'Added by Saylee on 14-Jan-2020
			WOStatusID = cmbStatus.SelectedValue
			If WOStatusID = 1 Then StatusID = 1
			CAMOUpdateRequired = -1
			BillingRequired = -1
		End If

		mWOCAMOUpdateList = nWOList.GetWOList(Text, No, FromDate, ToDate, RegNo, ModelName, StatusID, WOStatusID, AddTopItem, , IIf(mIsForCAMOUpdate = 1, Trans.WOCAMO, 0), False, IsForCAMOUpdate:=IIf(mIsForCAMOUpdate = 1 And IsAllWOsTicked = False, True, False), IsForBilling:=IIf(mIsForCAMOUpdate = 0 And IsAllWOsTicked = False, True, False), BillingRequired:=BillingRequired, IsCAMOUpdatedRequired:=CAMOUpdateRequired)
		dgCAMOUpdateWOList.DataSource = mWOCAMOUpdateList
		dgCAMOUpdateWOList.DataBind()
		upnlGridView.Update()
		Session("mWOCAMOUpdateList") = mWOCAMOUpdateList
		totcnt = mWOCAMOUpdateList.TotalWOCount
		Session("totcnt") = totcnt

	End Sub

	Private Sub CallFindNow(ByVal Index As Integer)
		FindNow(WOText, CInt(Val(No)), txtFromDate.Text.ToString, txtToDate.Text.ToString, RegNo:=RegNo, ModelName:=ModelName, BillingRequired:=BillingRequired, CAMOUpdateRequired:=CAMOUpdateRequired)
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
	Private Sub SetControl()
		setPeriod(DateIndex)

		chkShowAllWOs.Checked = IsAllWOsTicked  'Added by Saylee on 14-Jan-2020
		cmbStatus.SelectedValue = WOStatusID  'Added by Saylee on 14-Jan-2020

		CallFindNow(SearchIndex)
		dgCAMOUpdateWOList.DataBind()

		cmbDate.SelectedIndex = DateIndex

		cmbWO.SelectedValue = IIf(WOText = "", "(ALL)", WOText) '--Changed By Utkarsh On 17-Jan-2011
		txtNo.Text = No

		cmbBillingStatus.SelectedValue = BillingRequired

		cmbCAMOUpdateStatus.SelectedValue = CAMOUpdateRequired

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgCAMOUpdateWOList.Columns(2).HeaderText = "E.O. No."
			lblResult.Text = "List of Engineering Order as per criteria :" & mWOCAMOUpdateList.Count & " Record(s) found."
			dgCAMOUpdateWOList.DataBind()

		Else
			dgCAMOUpdateWOList.Columns(2).HeaderText = "W.O. No."
			'lblResult.Text = "List of Work Order as per criteria :" & mWOCAMOUpdateList.Count & " Record(s) found"

			If mIsForCAMOUpdate = 1 Then
				lblResult.Text = "List of Work Order for Updating CAMO as per criteria :" & mWOCAMOUpdateList.Count & " Record(s) found"
			ElseIf mIsForCAMOUpdate = 0 Then
				lblResult.Text = "List for Work Order Billing as per criteria :" & mWOCAMOUpdateList.Count & " Record(s) found"
			End If

			dgCAMOUpdateWOList.DataBind()


		End If
		ControlVisibility(DateIndex)
		'GridColumnsVisibility()
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

		If mIsForCAMOUpdate = 0 Then 'Billing 
			plhBillingStatus.Visible = True
			dgCAMOUpdateWOList.Columns(9).Visible = True
			plhCAMOUpdateStatus.Visible = False
		Else
			plhBillingStatus.Visible = False
			dgCAMOUpdateWOList.Columns(9).Visible = False
			plhCAMOUpdateStatus.Visible = True
		End If


		If chkShowAllWOs.Checked = True Then  'Added by Saylee on 14-Jan-2020
			phStatus.Visible = True
		End If

		upnllblNo.Update()
		upnlNo.Update()
	End Sub
	Private Sub DataFieldBind()
		Session("totcnt") = totcnt
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)

		DateIndex = IIf(IsNothing(DateIndex), 0, DateIndex)

		mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(ALL)", SkipIsForInventoryAircarft:=True)
		Session("mMachineNameValueList") = mMachineNameValueList

		StatusID = Session("StatusID")
		WOStatusID = Session("WOStatusID")
		WOText = Session("WOText")
		RegNo = Session("RegNo")
		ModelName = Session("ModelName")
		BillingRequired = Session("BillingRequired")
		CAMOUpdateRequired = Session("CAMOUpdateRequired")
		IsAllWOsTicked = Session("IsAllWOsTicked")  'Added by Saylee on 14-Jan-2020

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(ALL)")
		cmbWO.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText

		mWOStatusList = nWOStatusList.GetWOStatusListList(, "(ALL)")  'Added by Saylee on 14-Jan-2020
		cmbStatus.DataSource = mWOStatusList
		Session("mWOStatusList") = mWOStatusList

		DataBind()
	End Sub

	Private Sub setVariables()


		DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)         '--Changed By Utkarsh On 17-Jan-2011
		No = txtNo.Text.Trim

		BillingRequired = IIf(cmbBillingStatus.SelectedValue < 0, -1, CInt(Val(cmbBillingStatus.SelectedValue)))
		CAMOUpdateRequired = IIf(cmbCAMOUpdateStatus.SelectedValue < 0, -1, CInt(Val(cmbCAMOUpdateStatus.SelectedValue)))

		IsAllWOsTicked = chkShowAllWOs.Checked  'Added by Saylee on 14-Jan-2020

		WOStatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue) 'Added by Saylee on 14-Jan-2020
		Session("WOStatusID") = WOStatusID
		Session("IsAllWOsTicked") = IsAllWOsTicked  'Added by Saylee on 14-Jan-2020

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
		Session("BillingRequired") = BillingRequired
		Session("CAMOUpdateRequired") = CAMOUpdateRequired
	End Sub
	Private Sub EditRecord(ByVal mId As Guid)
		mnWO = nWO.GetWO(mId, False)
		mnWO.MarkClean()
		Session("mnWO") = mnWO
		' Session("mTransTypeId") = mTransTypeID 'Added by Saylee on 5-Sep-2018
	End Sub
	Private Sub ClearControls()
		txtNo.Text = ""
	End Sub
#End Region

#Region "Events"

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		'Put user code to initialize the page here
		ClearAll()
		' addAttributes()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		If Not IsPostBack Then
			mIsForCAMOUpdate = Request.QueryString("IsForCAMOUpdate")
			Session("IsForCAMOUpdate") = mIsForCAMOUpdate
			Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=" & mIsForCAMOUpdate
			DataFieldBind()
			SetControl()
		Else
			dgCAMOUpdateWOList.DataSource = mWOCAMOUpdateList
			dgCAMOUpdateWOList.DataBind()
			' SetGrid()
		End If
		SetGrid()
		SetTitle()
	End Sub

	Private Sub btnFindNow_Click(sender As Object, e As System.EventArgs) Handles btnFindNow.Click

		setVariables()
		CallFindNow(SearchIndex)
		dgCAMOUpdateWOList.DataBind()
		SetGrid()
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgCAMOUpdateWOList.Columns(2).HeaderText = "E.O. No."

			lblResult.Text = "List of Engineering Order as per criteria :" & mWOCAMOUpdateList.Count & " Record(s) found."
		Else
			dgCAMOUpdateWOList.Columns(2).HeaderText = "W.O. No."

			' lblResult.Text = "List of CAMO Update Work Order as per criteria :" & mWOCAMOUpdateList.Count & " Record(s) found"
			If mIsForCAMOUpdate = 1 Then
				lblResult.Text = "List of Work Order for Updating CAMO as per criteria :" & mWOCAMOUpdateList.Count & " Record(s) found"
			ElseIf mIsForCAMOUpdate = 0 Then
				lblResult.Text = "List for Work Order Billing as per criteria :" & mWOCAMOUpdateList.Count & " Record(s) found"
			End If

		End If
		ControlVisibility(DateIndex)
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()

	End Sub

	Private Sub cmbDate_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDate.SelectedIndexChanged

		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(DateIndex)
		setPeriod(DateIndex)
		If cmbDate.Enabled = True Then
			SetFocus(cmbDate)
		End If

	End Sub

	Private Sub dgCAMOUpdateWOList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCAMOUpdateWOList.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
					'Exit Sub
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				Dim mDate As String = mWOCAMOUpdateList(mID).WODateFormatted
				Dim mWorkOrderNo As String = mWOCAMOUpdateList(mID).WONumber
				Dim mCreatedBy As String = mWOCAMOUpdateList(mID).WOBy
				Dim mRegNo As String = IIf(mWOCAMOUpdateList(mID).RegNo = "", "", mWOCAMOUpdateList(mID).RegNo)
				Dim mModel As String = IIf(mWOCAMOUpdateList(mID).ModelName = "", "", mWOCAMOUpdateList(mID).ModelName)
				Dim mSerialNo As String = mWOCAMOUpdateList(mID).SerialNo
				mWOCAMODetail = mWorkOrderNo + " Dated : " + mDate + " Created By : " + mCreatedBy + IIf(mRegNo <> "", " Aircraft : " + mRegNo, "") + IIf(mModel <> "", " Model : " + mModel, "") + IIf(mSerialNo <> "", " Serial No. : " + mSerialNo, "")
				MarkLog(Util.Action.Edit, "Work Order", mWOCAMODetail, Util.ErrorType.NoError, mID, EventLogID)
				EditRecord(mID)
				Session("Edit") = True
				DataFieldBind()
				SetControl()
				SetGrid()
				SetTitle()

				upnlGridView.Update()
				upnlActionBtnTop.Update()
				upnlActionBtnBottom.Update()
				upnlResult.Update()

				If chkShowAllWOs.Checked And Not (cmbStatus.SelectedIndex = 8) Then  'Added by Saylee on 14-Jan-2020
					Session("IsShowAllWOs") = True
				Else
					Session("IsShowAllWOs") = False
				End If


				Dim str As String
				str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
		End Select
	End Sub
	Private Sub dgCAMOUpdateWOList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCAMOUpdateWOList.Sorting
		mWOCAMOUpdateList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgCAMOUpdateWOList.DataSource = mWOCAMOUpdateList
		Session("mWOCAMOUpdateList") = mWOCAMOUpdateList
		dgCAMOUpdateWOList.DataBind()
		SetGrid()
	End Sub
	Private Sub dgCAMOUpdateWOList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCAMOUpdateWOList.PageIndexChanging
		dgCAMOUpdateWOList.PageIndex = e.NewPageIndex
		dgCAMOUpdateWOList.DataSource = mWOCAMOUpdateList
		Session("mWOCAMOUpdateList") = mWOCAMOUpdateList

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgCAMOUpdateWOList.Columns(2).HeaderText = "E.O. No."
			dgCAMOUpdateWOList.Columns(6).HeaderText = "E.O. Status"
			dgCAMOUpdateWOList.Columns(7).HeaderText = "E.O. Status"
		Else
			dgCAMOUpdateWOList.Columns(2).HeaderText = "W.O. No."
			dgCAMOUpdateWOList.Columns(6).HeaderText = "W.O. Status"
			dgCAMOUpdateWOList.Columns(7).HeaderText = "W.O. Status"
		End If

		dgCAMOUpdateWOList.DataBind()
		SetGrid()
	End Sub
	Private Sub cmbWO_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbWO.SelectedIndexChanged
		ClearControls()

		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(DateIndex)


	End Sub
	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click, btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		'ModuleName = Nothing
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub chkShowAllWOs_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkShowAllWOs.CheckedChanged  'Added by Saylee on 14-Jan-2020
		If chkShowAllWOs.Checked = False Then
			chkShowAllWOs.Checked = False
			phStatus.Visible = False
		ElseIf chkShowAllWOs.Checked = True Then
			phStatus.Visible = True

		End If
		cmbStatus.SelectedIndex = 0
	End Sub
#End Region



End Class