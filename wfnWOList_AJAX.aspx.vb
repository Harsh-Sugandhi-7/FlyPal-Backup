'******************************************
'AJAX Conversion By Saylee On 24-Jul-2014
'******************************************

Public Class wfnWOList_AJAX
	Inherits Page

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
	Public mWOStatusList As nWOStatusList
	Dim mMachineNameValueList As MachineNameValueList
	Dim mWOModelNameValueList As nWOModelNameValueList
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex,
		DateIndex,
		FromDate,
		ToDate,
		WOText,
		StatusID,
		No,
		WOStatusID,
		RegNo,
		ModelName,
		ShowNoE,
		pageIndex As String

	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mWODetail As String
	Dim totcnt As Integer
	Dim IsReadOnly As Boolean 'Added by Saylee

	Public mTransTypeID As Trans 'Added by Saylee on 5-Sep-2018
	'Added By Vikrant On 27-Jul-2020 For ALL27072020
	Public mRemovedAssemblyListForCombo As RemovedAssemblyListForCombo
	Public mRemovedCompListForCombo As RemovedCompListForCombo
	'End
	Dim CustomerIDForSearchOnWOList As String = Guid.Empty.ToString 'Added By Prashant on 3-Jul-2023

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mWOStatusList = Session("mWOStatusList")
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
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		mRemovedAssemblyListForCombo = Session("mRemovedAssemblyListForCombo")
		mRemovedCompListForCombo = Session("mRemovedCompListForCombo")
		'End
		CustomerIDForSearchOnWOList = Session("CustomerIDForSearchOnWOList")
		ShowNoE = Session("ShowNoE") 'added ajay 17-08-2023
		pageIndex = Session("PageIndex")
	End Sub

	Private Sub SetSession()
		Session("mWOStatusList") = mWOStatusList
		Session("mWOList") = mWOList
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
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		Session("mRemovedAssemblyListForCombo") = mRemovedAssemblyListForCombo
		Session("mRemovedCompListForCombo") = mRemovedCompListForCombo
		'End
		Session("CustomerIDForSearchOnWOList") = CustomerIDForSearchOnWOList
		Session("ShowNoE") = ShowNoE   'added ajay 17-08-2023
		Session("PageIndex") = pageIndex
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mWOStatusList")
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
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		Session.Remove("mRemovedAssemblyListForCombo")
		Session.Remove("mRemovedCompListForCombo")
		'End
		Session.Remove("CustomerIDForSearchOnWOList")
		Session.Remove("ShowNoE") 'added ajay 17-08-2023
		Session.Remove("PageIndex")
	End Sub

	Private Sub ClearAll()
		mTransTypeID = Session("mTransTypeId") 'Added by Saylee on 5-Sep-2018
		'If InStr(Session("MiddleFrame"), "wfnWOList_AJAX.aspx?TransTypeId=" & mTransTypeID) <= 0 Then
		If InStr(Session("MiddleFrame"), "wfnWOList_AJAX.aspx?TransTypeID=" & Request.QueryString("TransTypeId")) <= 0 Then
			RemoveSession()
			Session.Remove("mWOList")
			Session.Remove("OpenFromWOJobListToCompleteForm")
			Session.Remove("wfProject_Ajax") 'Added By Prashant on 13-May-2024
		End If
	End Sub

	Private Sub NewRecord()
		mnWO = nWO.NewWO(, mTransTypeID)

		'Added by Shital on 05-Sept-2019
		If AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "LNT" Or AppSettings("ClientCode") = "SUH" Then
			mnWO.IssueTo = "INDAMER AVIATION PVT. LTD"
		ElseIf AppSettings("ClientCode") = "KZN" Then
			mnWO.IssueTo = "TAJ AIR LTD"
		ElseIf AppSettings("ClientCode") = "SHR" Then
			mnWO.IssueTo = "Indocopters Private Limited (ICPL)"
		End If

		Session("mnWO") = mnWO
		Session("mTransTypeID") = mTransTypeID 'Added by Saylee on 5-Sep-2018
	End Sub

	Private Sub EditRecord(mId As Guid)
		If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "SHN" Or AppSettings("ClientCode") = "MYT" Or AppSettings("ClientCode") = "" Then
			mnWO = nWO.GetWO(mId, False, getAircraftValuesAsOnCompletionDate:=True)
		Else
			mnWO = nWO.GetWO(mId, False)
		End If

		mnWO.MarkClean()
		Session("mnWO") = mnWO
		Session("mTransTypeId") = mTransTypeID 'Added by Saylee on 5-Sep-2018
	End Sub

	'Added By Vikrant On 27-Jul-2020 For ALL27072020
	Public Sub HighlightSpareAssembly()
		Dim da As New CSLA.Data.ObjectAdapter
		Dim ds As New DataSet()

		If mTransTypeID = Trans.SpareAssemblyWO Then
			da.Fill(ds, mRemovedAssemblyListForCombo)
			Dim dv As DataView = ds.Tables(0).DefaultView
			dv.RowFilter = "IsSpareAssembly='True'"
			For Each dr As DataRowView In dv
				For Each item1 As Web.UI.WebControls.ListItem In cmbAssembly.Items
					If dr("AssemblyStatusID").ToString() = item1.Value.ToString() Then
						item1.Attributes.Add("style", "background-color:#ffbf00;color:white;font-weight:bold;")
						item1.Attributes.Add("title", "Spare Assembly")
					End If
				Next
			Next
		ElseIf mTransTypeID = Trans.SpareComponentWO Then
			da.Fill(ds, mRemovedCompListForCombo)
			Dim dv As DataView = ds.Tables(0).DefaultView
			dv.RowFilter = "IsSpareComp='True'"
			For Each dr As DataRowView In dv
				For Each item1 As Web.UI.WebControls.ListItem In cmbComponent.Items
					If dr("CompStatusID").ToString() = item1.Value.ToString() Then
						item1.Attributes.Add("style", "background-color:#ffbf00;color:white;font-weight:bold;")
						item1.Attributes.Add("title", "Spare Component")
					End If
				Next
			Next
		End If

	End Sub
	'End
	Private Sub DeleteRecord(mId As Guid)
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
			MSGBoxCtrl.Show("Alert!", "<BR>There are Scheduled jobs in this " & WOstr & " which may have been already complied,to change their status please use the Maintenance menu option" & ".<BR><BR>Do you want to continue?", "", MsgBoxStyle.YesNo, "IsScheduledJobExists")
		Else
			MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, " ", MsgBoxStyle.YesNo, "Delete")
		End If

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)
		If control.Enabled = False Or control.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub

	Private Sub SetTitle()

		mWOList = Session("mWOList")
		totcnt = mWOList.TotalWOCount
		Session("totcnt") = totcnt

		If mTransTypeID = Trans.SpareAssemblyWO Then

			lblTitle.Text = "List of Stock / Removed Assembly Work Order"

		ElseIf mTransTypeID = Trans.SpareComponentWO Then

			lblTitle.Text = "List of Stock / Removed Component Work Order"
			'End

		Else

			'Modified by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
			lblTitle.Text = " List of " & IIf(mTransTypeID = Trans.EngineeringWO,
											  "Engineering",
											  IIf(mTransTypeID = Trans.WOCAMO,
														   "CAMO",
														   "AMO")) & " Work Order"

		End If

	End Sub
	Private Sub SetPeriod(Index As Int32)

		If FromDate = "1/1/1900" Then
			txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
		Else
			txtFromDate.Text = FromDate
		End If
		If ToDate = "1/1/2200" Then
			txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
		Else
			txtToDate.Text = ToDate
		End If

	End Sub
	Private Sub SetVariables()

		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)         '--Changed By Utkarsh On 17-Jan-2011
		WOStatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
		StatusID = IIf(cmbDocStatus.SelectedIndex <= 0, 0, cmbDocStatus.SelectedValue)
		RegNo = IIf(cmbAircraft.SelectedIndex <= 0, "", cmbAircraft.SelectedValue)
		ModelName = IIf(cmbModel.SelectedIndex <= 0, "", cmbModel.SelectedValue) '--Changed By Utkarsh On 17-Jan-2011
		No = txtNo.Text.Trim
		ShowNoE = IIf(cmbShowE.SelectedIndex <= 0, 0, cmbShowE.SelectedValue) 'Ajay 17-08-2023

		If cmbCustomerList.SelectedIndex = 0 Then
			CustomerIDForSearchOnWOList = "{00000000-0000-0000-0000-000000000000}"
		Else
			CustomerIDForSearchOnWOList = cmbCustomerList.SelectedValue.ToString
		End If

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
		Session("CustomerIDForSearchOnWOList") = CustomerIDForSearchOnWOList
		Session("ShowNoE") = ShowNoE 'Ajay 17-08-2023

	End Sub
	Private Sub SetToolTip()

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso
			(AppSettings("ClientCode") = "TAAL" Or
			AppSettings("ClientCode") = "GlobalJet") Then

			lblTitle.Text = "List of Engineering Order"
			btnAddNewTop.ToolTip = "Click to Add New Engineering Order"
			btnPrintTop.ToolTip = "Click to Print the list of Engineering Order"
			btnCloseTop.ToolTip = "Click to close List of Engineering Order screen"
			btnSearch.ToolTip = "Click to find list of Engineering Order as per searching criteria"

		Else

			lblTitle.Text = "List of Work Order"
			btnAddNewTop.ToolTip = "Click to Add New Work Order"
			btnPrintTop.ToolTip = "Click to Print the list of Work Order"
			btnCloseTop.ToolTip = "Click to close List of Work Order screen"
			btnSearch.ToolTip = "Click to find list of Work Order as per searching criteria"

		End If

	End Sub

	Private Function IsInRole(CheckFor As Rights) As Boolean

		Dim IsInRoleString As String = ""

		'Deciding IsInRole String to check Rights
		Select Case mTransTypeID

			Case Trans.WO145
				IsInRoleString = "WorkOrder"
			Case Trans.WOCAMO
				IsInRoleString = "CAMOWO"
				'Added By Vikrant On 27-Jul-2020 For ALL27072020
			Case Trans.SpareAssemblyWO
				IsInRoleString = "SpareAssemblyWO"
			Case Trans.SpareComponentWO
				IsInRoleString = "SpareComponentWO"
				'End
			Case Trans.EngineeringWO    'Added by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
				IsInRoleString = "EngineeringOrder"
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

		End Select

	End Function

	Private Sub SetControl()

		SetPeriod(DateIndex)
		CallFindNow(SearchIndex)

		dgWOList.DataBind()
		cmbAircraft.SelectedValue = IIf(RegNo = "", "(ALL)", RegNo)
		cmbModel.SelectedValue = IIf(ModelName = "", "(ALL)", ModelName) '--Changed By Utkarsh On 17-Jan-2011
		cmbWO.SelectedValue = IIf(WOText = "", "(ALL)", WOText) '--Changed By Utkarsh On 17-Jan-2011
		txtNo.Text = No
		cmbStatus.SelectedValue = WOStatusID
		cmbDocStatus.SelectedValue = StatusID

		If ShowNoE Is Nothing Then

			cmbShowE.SelectedValue = "4"

		Else

			cmbShowE.SelectedValue = ShowNoE 'Ajay 17-08-2023

		End If

		If pageIndex Is Nothing Then

			dgWOList.PageIndex = 0

		Else

			dgWOList.PageIndex = pageIndex  'Ajay 17-08-2023

		End If

		If CustomerIDForSearchOnWOList Is Nothing Then

			cmbCustomerList.SelectedValue = Guid.Empty.ToString

		ElseIf CustomerIDForSearchOnWOList = "{00000000-0000-0000-0000-000000000000}" Then

			cmbCustomerList.SelectedValue = Guid.Empty.ToString

		Else

			cmbCustomerList.SelectedValue = CustomerIDForSearchOnWOList.ToString

		End If

		ControlVisibility(SearchIndex, DateIndex)

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso
			(AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

			dgWOList.Columns(2).HeaderText = "E.O.No."
			dgWOList.Columns(16).HeaderText = "E.O.Status"
			dgWOList.DataBind()

			lblResult.Text = "List of Engineering Order as per criteria : " & mWOList.Count & " Record(s) found."

		Else

			dgWOList.Columns(2).HeaderText = "W.O.No."
			dgWOList.Columns(16).HeaderText = "W.O.Status"
			dgWOList.DataBind()

			'Modified by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
			lblResult.Text = "List of " & IIf(mTransTypeID = Trans.EngineeringWO,
											  "Engineering",
											  IIf(mTransTypeID = Trans.WOCAMO,
														   "CAMO",
														   "AMO")) & " Work Order as per criteria : " & mWOList.Count & " Record(s) found."

		End If

		GridColumnsVisibility()

	End Sub

	Private Sub AddAttributes()

		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")

	End Sub

	Private Sub FindNow(Optional Text As String = "",
						Optional No As Int32 = 0,
						Optional FromDate As String = "",
						Optional ToDate As String = "",
						Optional RegNo As String = "",
						Optional ModelName As String = "",
						Optional WOStatusID As Integer = 0,
						Optional StatusID As Integer = 0,
						Optional AddTopItem As String = "",
						Optional AssemblyStatusID As String = "{00000000-0000-0000-0000-000000000000}",
						Optional CustomerID As String = "{00000000-0000-0000-0000-000000000000}")

		mWOList = Nothing
		dgWOList.DataSource = Nothing

		mWOList = nWOList.GetWOList(Text:=Text,
									No:=No,
									FromDate:=FromDate,
									ToDate:=ToDate,
									RegNo:=RegNo,
									ModelName:=ModelName,
									StatusID:=StatusID,
									WOStatusID:=WOStatusID,
									AddTopItem:=AddTopItem, ,
									TransTypeID:=mTransTypeID,
									AssemblyStatusID:=AssemblyStatusID,
									CustomerID:=CustomerID)

		dgWOList.DataSource = mWOList
		Session("mWOList") = mWOList
		totcnt = mWOList.TotalWOCount
		Session("totcnt") = totcnt
		dgWOList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 24-07-2023

	End Sub

	Private Sub CallFindNow(Index As Integer)

		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		Dim AssemblyStatusID As String
		If mTransTypeID = Trans.SpareAssemblyWO Then
			AssemblyStatusID = cmbAssembly.SelectedValue
		ElseIf mTransTypeID = Trans.SpareComponentWO Then
			AssemblyStatusID = cmbComponent.SelectedValue
		Else
			AssemblyStatusID = Guid.Empty.ToString
		End If
		'End

		FindNow(Text:=WOText,
				No:=CInt(Val(No)),
				FromDate:=txtFromDate.Text.ToString,
				ToDate:=txtToDate.Text.ToString,
				RegNo:=RegNo,
				ModelName:=ModelName,
				WOStatusID:=WOStatusID,
				StatusID:=StatusID,
				AddTopItem:="",
				AssemblyStatusID:=AssemblyStatusID,
				CustomerID:=IIf(CustomerIDForSearchOnWOList Is Nothing,
								"{00000000-0000-0000-0000-000000000000}",
								CustomerIDForSearchOnWOList))

	End Sub

	Private Sub ControlVisibility(SearchIndex As Int32, Optional DateIndex As Int32 = 0)

		txtFromDate.Enabled = True
		txtToDate.Enabled = True
		dgWOList.Columns(3).Visible = IIf(AppSettings("ClientCode") = "PAS", True, False)
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		If mTransTypeID = Trans.SpareAssemblyWO Then

			cmbAssembly.Visible = True
			lblAssembly.Visible = True
			cmbComponent.Visible = False
			lblComponent.Visible = False
			cmbAircraft.Visible = False
			cmbModel.Visible = False
			lblAircraft.Visible = False
			lblModel.Visible = False
			lblReadOnly.Visible = False

		ElseIf mTransTypeID = Trans.SpareComponentWO Then

			cmbAssembly.Visible = False
			lblAssembly.Visible = False
			cmbComponent.Visible = True
			lblComponent.Visible = True
			cmbAircraft.Visible = False
			cmbModel.Visible = False
			lblAircraft.Visible = False
			lblModel.Visible = False
			lblReadOnly.Visible = False
			'Added By Prashant On 30-Jun-2023

		ElseIf (mTransTypeID = Trans.WO145 And
			AppSettings("ShowMaintenanceForNewClients") = "True" And
			AppSettings("ShowAMOOnlyForNewClients") = "True") Then

			cmbAssembly.Visible = False
			lblAssembly.Visible = False
			cmbComponent.Visible = False
			lblComponent.Visible = False
			cmbAircraft.Visible = False
			cmbModel.Visible = False
			lblAircraft.Visible = False
			lblModel.Visible = False
			cmbDocStatus.Visible = False
			lblDocStatus.Visible = False
			lblReadOnly.Visible = False
			lblCustomer.Visible = True
			cmbCustomerList.Visible = True
			dgWOList.Columns(13).Visible = False 'DOC. Status

		Else
			cmbAssembly.Visible = False
			lblAssembly.Visible = False
			cmbComponent.Visible = False
			lblComponent.Visible = False
			lblReadOnly.Visible = True

			If AppSettings("ClientCode") = "Deccan" Then

				lblCustomer.Visible = True
				cmbCustomerList.Visible = True

			Else

				lblCustomer.Visible = False
				cmbCustomerList.Visible = False

			End If

			dgWOList.Columns(10).Visible = False

		End If

		'End
		cmbDocStatus.Visible = False
		lblDocStatus.Visible = False
		'**************

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
							MarkLog(Action.Delete, "Work Order", mWODetail, ErrorType.NoError, TempWOID, EventLogID)


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
							SetGrid()

							upnlGridView.Update()
							upnlActionBtnTop.Update()
							upnlActionBtnBottom.Update()
							upnlResult.Update()
							upnlTitle.Update() 'Added By Vikrant On 27-Jul-2020 For ALL27072020
						Catch ex As SqlException
							If ex.Number = 8145 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 547 Then
								MarkLog(Action.Delete, "Work Order", "Can't delete : " & mWODetail & " is Currently in use", ErrorType.NoError, TempWOID, EventLogID)
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 50000 Then
								MSGBoxCtrl.Show("Delete Alert!", "", ex.Message, MsgBoxStyle.OkOnly, "")
							End If
							DataFieldBind()
							SetControl()
							SetGrid() 'Added By Vikrant On 27-Jul-2020 For ALL27072020
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
					SetGrid() 'Added By Vikrant On 27-Jul-2020 For ALL27072020
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					DataFieldBind()
					SetGrid() 'Added By Vikrant On 27-Jul-2020 For ALL27072020
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
			Session("sender") = ""
			DataFieldBind()
			SetGrid() 'Added By Vikrant On 27-Jul-2020 For ALL27072020
		End If
	End Sub
	Private Sub GridColumnsVisibility()
		If (AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA") Then
			dgWOList.Columns(8).Visible = False
			dgWOList.Columns(12).Visible = False
			dgWOList.Columns(9).Visible = True
			dgWOList.Columns(19).Visible = True
		Else
			dgWOList.Columns(8).Visible = True
			dgWOList.Columns(12).Visible = True
			dgWOList.Columns(9).Visible = False
			dgWOList.Columns(19).Visible = False
		End If
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		If mTransTypeID = Trans.SpareAssemblyWO Then
			dgWOList.Columns(4).Visible = False
			dgWOList.Columns(5).Visible = True
			dgWOList.Columns(6).Visible = False
			dgWOList.Columns(7).Visible = False
			dgWOList.Columns(8).Visible = False
		ElseIf mTransTypeID = Trans.SpareComponentWO Then
			dgWOList.Columns(4).Visible = False
			dgWOList.Columns(5).Visible = False
			dgWOList.Columns(6).Visible = True
			dgWOList.Columns(7).Visible = False
			dgWOList.Columns(8).Visible = False
		ElseIf mTransTypeID <> Trans.WOCAMO Then
			dgWOList.Columns(15).Visible = False 'Service Provider name
			dgWOList.Columns(4).Visible = True
			dgWOList.Columns(5).Visible = False
			dgWOList.Columns(6).Visible = False
			dgWOList.Columns(7).Visible = True
			dgWOList.Columns(8).Visible = True
		Else
			dgWOList.Columns(4).Visible = True
			dgWOList.Columns(5).Visible = False
			dgWOList.Columns(6).Visible = False
			dgWOList.Columns(7).Visible = True
			dgWOList.Columns(8).Visible = True
			If AppSettings("ShowCAMOOnlyForNewClients") = "False" Then
				dgWOList.Columns(15).Visible = False
			End If
		End If
		dgWOList.Columns(3).Visible = IIf(AppSettings("ClientCode") = "PAS", True, False)
		'End
	End Sub
#End Region

#Region "DataFieldBind"

	Private Sub DataFieldBind()

		Session("totcnt") = totcnt 'Added by shweta on 11-1-12
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)
		DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)
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
		mWOStatusList = nWOStatusList.GetWOStatusListList(, "(ALL)", SkipNewStatusIDsForOldWOFlow:=True)
		cmbStatus.DataSource = mWOStatusList
		Session("mWOStatusList") = mWOStatusList
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		mRemovedAssemblyListForCombo = RemovedAssemblyListForCombo.GetAssemblyList(Today.Date.ToString, "(SELECT)")
		Session("mRemovedAssemblyListForCombo") = mRemovedAssemblyListForCombo
		cmbAssembly.DataSource = mRemovedAssemblyListForCombo
		mRemovedCompListForCombo = RemovedCompListForCombo.GetCompList(Today.Date.ToString, AddTopItem:="(SELECT)")
		Session("mRemovedCompListForCombo") = mRemovedCompListForCombo
		cmbComponent.DataSource = mRemovedCompListForCombo
		'End

		'Added By Prashant on 3-Jul-2023
		cmbCustomerList.DataSource = VendorList.GetVendorstList(0, , , , , , "(ALL)", True)
		'End
		DataBind()

		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		If mTransTypeID = Trans.SpareAssemblyWO Or mTransTypeID = Trans.SpareComponentWO Then
			HighlightSpareAssembly()
		End If
		'End

	End Sub

	Private Sub SetGrid()

		Dim P As Boolean

		For j As Integer = 0 To dgWOList.Rows.Count - 1

			If mTransTypeID = Trans.WO145 Then
				IsReadOnly = False
			Else

				If Me.dgWOList.Rows.Item(j).Cells(4).Text = "&nbsp;" Then
					IsReadOnly = False
				ElseIf mMachineNameValueList(Me.dgWOList.Rows.Item(j).Cells(4).Text) Is Nothing Then '    If mMachineNameValueList(Me.dgWOList.Rows.Item(j).Cells(4).Text) Is Nothing Then
					IsReadOnly = True
				Else
					IsReadOnly = mMachineNameValueList(Me.dgWOList.Rows.Item(j).Cells(4).Text).IsReadOnly 'Added by Saylee - Restrict User from using ReadOnly Aircraft
				End If

			End If

			dgWOList.Rows(j).Cells(20).Enabled = Not (IsReadOnly = True)

		Next
		IsReadOnly = Session("IsReadOnly") 'Added by Saylee
		If IsReadOnly = True Then
			lblReadOnly.Visible = True
		Else
			lblReadOnly.Visible = False
		End If
		dgWOList.Columns(22).Visible = IIf(AppSettings("ClientCode") = "Heligo", True, False)

	End Sub

#End Region

#Region "Events"

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		'Put user code to initialize the page here
		ClearAll()
		AddAttributes()
		GetSession()

		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

		If Not IsPostBack Then

			If Session("mTransTypeId") Is Nothing Then

				mTransTypeID = Request.QueryString("TransTypeId")
				Session("mTransTypeId") = mTransTypeID

			Else

				mTransTypeID = Session("mTransTypeId")

			End If

			Session("MiddleFrame") = "wfnWOList_AJAX.aspx?TransTypeID=" & mTransTypeID

			If (mTransTypeID = 88) Then
				lblAircraft.Visible = False
				cmbAircraft.Visible = False
			Else
				lblAircraft.Visible = True
				cmbAircraft.Visible = True
			End If
			'Added by Sachin on 14-09-23 for removing Aircraft filter from Third Party-work order

			If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "CAMO Work Order") Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
			Else
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
			End If

			'--------------------------
			If Session("ShowNoE") Is Nothing Then
				cmbShowE.SelectedValue = "4"
				Session("ShowNoE") = cmbShowE.SelectedValue 'Ajay 24-07-2023
				ShowNoE = cmbShowE.SelectedValue
			End If

			DataFieldBind()
			SetControl()

		End If

		If User.IsInRole("ShowWODashBoardView") = True Then 'user rights added by Saylee on 7-jun-2022
			hylnktWODashBoard.Visible = True
		Else
			hylnktWODashBoard.Visible = False
		End If

		SetToolTip()
		SetGrid()
		SetTitle()

	End Sub
	Private Sub dgWOList_RowCommand(sender As Object, e As Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOList.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
					'Exit Sub
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
				Dim mId As Guid = mWOList(Index).ID
				Dim mDate As String = mWOList(Index).WODateFormatted
				Dim mWorkOrderNo As String = mWOList(Index).WONumber
				Dim mCreatedBy As String = mWOList(Index).WOBy
				Dim mRegNo As String = IIf(mWOList(Index).RegNo = "", "", mWOList(Index).RegNo)
				Dim mModel As String = IIf(mWOList(Index).ModelName = "", "", mWOList(Index).ModelName)
				Dim mSerialNo As String = mWOList(Index).SerialNo
				mWODetail = mWorkOrderNo + " Dated : " + mDate + " Created By : " + mCreatedBy + IIf(mRegNo <> "", " Aircraft : " + mRegNo, "") + IIf(mModel <> "", " Model : " + mModel, "") + IIf(mSerialNo <> "", " Serial No. : " + mSerialNo, "")
				MarkLog(Action.Edit, "Work Order", mWODetail, ErrorType.NoError, mId, EventLogID)
				EditRecord(mId)
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

				Dim str As String
				str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
			Case "DeleteRec"
				If (Not IsInRole(Rights.Delete)) Then
					'  ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex 'CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
				Dim mId As Guid = mWOList(Index).ID
				DeleteRecord(mId)

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
				'    mID = mWOList(Idx).ID
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
				'    Dim pdfList As New Collections.ArrayList

				'    For j As Integer = 0 To mnWO.FileAttachments.Count - 1
				'        If mnWO.FileAttachments(j).Size > 0 And mnWO.FileAttachments(j).Extension = ".pdf" Then
				'            Dim ChildAttachment_path As String = "C:\Temp\" & mnWO.WONumber & PDFNoChild.ToString & mnWO.FileAttachments(j).Extension

				'            Dim fs As FileStream
				'            If File.Exists(AppSettings("DOCPath")) = False Then
				'                'Delete File if exist
				'                IO.File.Delete(ChildAttachment_path)
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
				Idx = CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex 'CInt(e.CommandArgument) + dgWOList.PageSize * dgWOList.PageIndex
				mID = mWOList(Idx).ID
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
							IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
							' Create the file.
							fs = File.Create(path)
							'' Add some information to the file.
							fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
							fs.Close()
							Session("DOCPath") = path
							ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
							Dim Detail As String = "Work Order Attachment( " + mFileAttach.FileName + ") viewed by  " + User.Identity.Name
							MarkLog(Action.View, "Work Order", Detail, ErrorType.HandledError, mWOList(Idx).ID, EventLogID)
						End If
					End If
				End If
		End Select
	End Sub
	Private Sub dgWOList_PageIndexChanging(source As Object, e As Web.UI.WebControls.GridViewPageEventArgs) Handles dgWOList.PageIndexChanging
		dgWOList.PageIndex = e.NewPageIndex
		dgWOList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
		dgWOList.DataSource = mWOList
		Session("mWOList") = mWOList
		Session("PageIndex") = dgWOList.PageIndex

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			dgWOList.Columns(2).HeaderText = "E.O. No."
			dgWOList.Columns(16).HeaderText = "E.O. Status"
		Else
			dgWOList.Columns(2).HeaderText = "W.O. No."
			dgWOList.Columns(16).HeaderText = "W.O. Status"
		End If
		GridColumnsVisibility()
		ControlVisibility(0)
		dgWOList.DataBind()
		SetGrid()

	End Sub

	Private Sub dgWOList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgWOList.RowDataBound
		If e.Row.RowType <> DataControlRowType.DataRow Then
			Return
		End If
		If (e.Row.RowType = DataControlRowType.DataRow) Then
			Dim TaskCompletionPercentage As Integer = (DataBinder.Eval(e.Row.DataItem, "TaskCompletionPercentage"))
			Dim tmpDiv As HtmlGenericControl = CType(e.Row.FindControl("prgbar"), HtmlGenericControl)
			Dim lblPercentage As HtmlGenericControl = CType(e.Row.FindControl("lblPercentage"), HtmlGenericControl)
			tmpDiv.Attributes.Add("style", "width:" + TaskCompletionPercentage.ToString + "%")
			tmpDiv.Attributes.Add("aria-valuenow", TaskCompletionPercentage.ToString)
			lblPercentage.InnerText = TaskCompletionPercentage.ToString + "%"
			If TaskCompletionPercentage = 0 Then
				lblPercentage.Attributes.Add("style", "color:black;")
			Else
				lblPercentage.Attributes.Add("style", "color:white;")
			End If



			'Compliance
			Dim TaskCompliancePercentage As Integer = (DataBinder.Eval(e.Row.DataItem, "TaskCompliancePercentage"))
			Dim tmpDivCompliance As HtmlGenericControl = CType(e.Row.FindControl("prgbarCompliance"), HtmlGenericControl)
			Dim lblCompliancePercentage As HtmlGenericControl = CType(e.Row.FindControl("lblCompliancePercentage"), HtmlGenericControl)
			tmpDivCompliance.Attributes.Add("style", "width:" + TaskCompliancePercentage.ToString + "%")
			tmpDivCompliance.Attributes.Add("aria-valuenow", TaskCompliancePercentage.ToString)
			lblCompliancePercentage.InnerText = TaskCompliancePercentage.ToString + "%"
			If TaskCompliancePercentage = 0 Then
				lblCompliancePercentage.Attributes.Add("style", "color:black;")
			Else
				lblCompliancePercentage.Attributes.Add("style", "color:white;")
			End If

		End If


	End Sub
	Private Sub FindNow_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

		ControlVisibility(0)
		SetVariables()
		CallFindNow(SearchIndex)
		dgWOList.DataBind()
		SetGrid()
		GetSession()

		If (AppSettings("ClientCode") IsNot Nothing) AndAlso
			(AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

			dgWOList.Columns(2).HeaderText = "E.O. No."
			dgWOList.Columns(16).HeaderText = "E.O. Status"
			lblResult.Text = "List of Engineering Order as per criteria : " & mWOList.Count & " Record(s) found."

		Else

			dgWOList.Columns(2).HeaderText = "W.O. No."
			dgWOList.Columns(16).HeaderText = "W.O. Status"

			'Modified by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
			lblResult.Text = "List of " & IIf(mTransTypeID = Trans.EngineeringWO,
											  "Engineering",
											  IIf(mTransTypeID = Trans.WOCAMO,
														   "CAMO",
														   "AMO")) & " Work Order as per criteria : " & mWOList.Count & " Record(s) found."

		End If

		GridColumnsVisibility()
		upnlGridView.Update()
		upnlActionBtnTop.Update()
		upnlActionBtnBottom.Update()
		upnlResult.Update()

	End Sub
	Private Sub AddNew_Click(sender As Object, e As EventArgs) Handles btnAddNewTop.Click

		'Added By vikrant On 18-July-2014
		If (Not IsInRole(Rights.New)) Then

			MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
							MSGBox.Message_text.Authorization,
							"",
							MsgBoxStyle.OkOnly,
							"")

			Exit Sub

		End If

		'End
		NewRecord()
		MarkLog(Action.[New],
				"Work Order",
				"",
				ErrorType.NoError,
				mnWO.ID,
				EventLogID)

		Dim str As String
		str = "openledgersame('wfnWODetail_Ajax.aspx?BackPage=index.aspx');"

		ScriptManager.RegisterStartupScript(Me,
											[GetType],
											"OpenScript",
											str,
											True)

	End Sub
	Private Sub btnCloseTop_Click(sender As Object, e As EventArgs) Handles btnCloseTop.Click ', btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Session.Remove("IsReadOnly")
		'ModuleName = Nothing
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgWOList_Sorting(sender As Object, e As Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOList.Sorting
		mWOList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOList.DataSource = mWOList
		Session("mWOList") = mWOList
		dgWOList.DataBind()
		SetGrid()
		GridColumnsVisibility()
		ControlVisibility(0)
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged
		IsReadOnly = mMachineNameValueList(cmbAircraft.SelectedValue).IsReadOnly 'Added by Saylee - Restrict User from using ReadOnly Aircraft
		Session("IsReadOnly") = IsReadOnly
		SetGrid()
		upnlSearchCriteria.Update()
		Session.Remove("IsReadOnly")
	End Sub
	Private Sub txtBarcode_TextChanged(sender As Object, e As EventArgs) Handles txtBarcode.TextChanged
		Dim BarcodeNoExists As nWOBarcodeNoExists = nWOBarcodeNoExists.GetBarcodeNoCount(txtBarcode.Text.Trim)
		If Not BarcodeNoExists.ID.Equals(Guid.Empty) Then
			Dim mnWOJob As nWOJob
			Dim mnWOJobTask As nWOJobTask
			Select Case BarcodeNoExists.Type
				Case "WO"
					EditRecord(BarcodeNoExists.ID)
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

					Dim str As String
					str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
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
			MSGBoxCtrl.Show("Alert..!!", "Invalid Barcode Number", "", MsgBoxStyle.OkOnly, "")
			txtBarcode.Text = ""
			Exit Sub
		End If
	End Sub

	'Ajay 06-Nov-2022
	Private Sub hdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 28-Des-2022
		MarkFavourite(HttpContext.Current.User.Identity.Name, "CAMO Work Order")

	End Sub

	Private Sub hdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 28-Des-2022
		RemoveFavourite(HttpContext.Current.User.Identity.Name, "CAMO Work Order")

	End Sub

	Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
		'Dim ExpiryDateList = ((From res In mWOList).ToList.Take(CInt(DropDownList1.SelectedItem.ToString))).ToList
		dgWOList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
		dgWOList.DataSource = mWOList
		dgWOList.DataBind()

		ControlVisibility(0)
		SetVariables()
		SetControl()
		upnlGridView.Update()
	End Sub
	'-----
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
	Private Function callZeroDifferenceValue(obj As Object, mLog As Log) As Boolean
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
				'MarkLog(Action.[New], "Log", "Aircraft Name ->" + mLog.Machine.RegNo + " Tank-> " + mTankList.Item(mTankList.CurrentIndex).Name, ErrorType.NoError, New Guid(cmbTankList.SelectedValue.ToString))
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

#Region "Commented Code"
	'Private Sub btnFindNow_Click( sender As Object,  e As EventArgs) Handles btnFindNow.Click
	'    If IsValid Then
	'        setVariables()
	'        CallFindNow(SearchIndex)
	'        dgWOList.DataBind()
	'        SetGrid()

	'        If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
	'            dgWOList.Columns(2).HeaderText = "E.O. No."
	'            dgWOList.Columns(15).HeaderText = "E.O. Status"
	'            lblResult.Text = "List of Engineering Order as per criteria :" & mWOList.Count & " Record(s) found."
	'        Else
	'            dgWOList.Columns(2).HeaderText = "W.O. No."
	'            dgWOList.Columns(15).HeaderText = "W.O. Status"
	'            lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
	'        End If
	'        GridColumnsVisibility()
	'        upnlGridView.Update()
	'        upnlActionBtnTop.Update()
	'        upnlActionBtnBottom.Update()
	'        upnlResult.Update()
	'    End If
	'    'lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
	'End Sub
	'Protected Sub cmbModel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbModel.SelectedIndexChanged, cmbAircraft.SelectedIndexChanged, cmbStatus.SelectedIndexChanged, cmbDocStatus.SelectedIndexChanged, txtNo.TextChanged, txtFromDate.TextChanged, txtToDate.TextChanged, cmbAssembly.SelectedIndexChanged, cmbComponent.SelectedIndexChanged
	'Private Sub cmbSearch_SelectedIndexChanged( sender As Object,  e As EventArgs) Handles cmbSearch.SelectedIndexChanged
	'    ClearControls()
	'    cmbDate.SelectedIndex = 0
	'    cmbWO.SelectedIndex = 0
	'    cmbAircraft.SelectedIndex = 0
	'    cmbModel.SelectedIndex = 0
	'    cmbStatus.SelectedIndex = 0
	'    cmbDocStatus.SelectedIndex = 0
	'    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
	'    'Commented & Added By Vikrant On 27-Jul-2020 For ALL27072020
	'    'ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
	'    ControlVisibility(CInt(cmbSearch.SelectedValue), DateIndex)
	'    'End
	'    setPeriod(DateIndex)

	'    'FindNow
	'    setVariables()
	'    CallFindNow(SearchIndex)
	'    dgWOList.DataBind()
	'    SetGrid()

	'    If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
	'        dgWOList.Columns(2).HeaderText = "E.O. No."
	'        dgWOList.Columns(15).HeaderText = "E.O. Status"
	'        lblResult.Text = "List of Engineering Order as per criteria :" & mWOList.Count & " Record(s) found."
	'    Else
	'        dgWOList.Columns(2).HeaderText = "W.O. No."
	'        dgWOList.Columns(15).HeaderText = "W.O. Status"
	'        lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
	'    End If
	'    GridColumnsVisibility()
	'    upnlGridView.Update()
	'    upnlActionBtnTop.Update()
	'    upnlActionBtnBottom.Update()
	'    upnlResult.Update()
	'    '--------------------------------------
	'    If cmbSearch.Enabled = True Then
	'        setFocus(cmbSearch)
	'    End If
	'End Sub
	'Private Sub cmbWO_SelectedIndexChanged( sender As Object,  e As EventArgs) Handles cmbWO.SelectedIndexChanged
	'    ClearControls()

	'    'Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
	'    'Commented & Added By Vikrant On 27-Jul-2020 For ALL27072020
	'    'ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
	'    'ControlVisibility(CInt(cmbSearch.SelectedValue), DateIndex)
	'    'End
	'    setPeriod(0)
	'    If cmbWO.Enabled = True Then
	'        setFocus(cmbWO)
	'    End If

	'    'FindNow
	'    setVariables()
	'    CallFindNow(SearchIndex)
	'    dgWOList.DataBind()
	'    SetGrid()

	'    If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
	'        dgWOList.Columns(2).HeaderText = "E.O. No."
	'        dgWOList.Columns(15).HeaderText = "E.O. Status"
	'        lblResult.Text = "List of Engineering Order as per criteria :" & mWOList.Count & " Record(s) found."
	'    Else
	'        dgWOList.Columns(2).HeaderText = "W.O. No."
	'        dgWOList.Columns(15).HeaderText = "W.O. Status"
	'        lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
	'    End If
	'    GridColumnsVisibility()
	'    upnlGridView.Update()
	'    upnlActionBtnTop.Update()
	'    upnlActionBtnBottom.Update()
	'    upnlResult.Update()
	'    '--------------------------------------
	'End Sub
	'Private Sub cmbDate_SelectedIndexChanged( sender As Object,  e As EventArgs) Handles cmbDate.SelectedIndexChanged
	'    ClearControls()
	'    'Commented & Added By Vikrant On 27-Jul-2020 For ALL27072020
	'    'Dim SearchIndex As Int32 = cmbSearch.SelectedIndex
	'    'Dim SearchIndex As Int32 = CInt(cmbSearch.SelectedValue)
	'    'End
	'    'Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)

	'    'Commented & Added By Vikrant On 27-Jul-2020 For ALL27072020
	'    'ControlVisibility(cmbSearch.SelectedIndex, DateIndex)
	'    'ControlVisibility(CInt(cmbSearch.SelectedValue), DateIndex)
	'    'End
	'    setPeriod(0)
	'    'If cmbDate.Enabled = True Then
	'    '    setFocus(cmbDate)
	'    'End If

	'    'FindNow
	'    setVariables()
	'    CallFindNow(SearchIndex)
	'    dgWOList.DataBind()
	'    SetGrid()

	'    If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
	'        dgWOList.Columns(2).HeaderText = "E.O. No."
	'        dgWOList.Columns(15).HeaderText = "E.O. Status"
	'        lblResult.Text = "List of Engineering Order as per criteria :" & mWOList.Count & " Record(s) found."
	'    Else
	'        dgWOList.Columns(2).HeaderText = "W.O. No."
	'        dgWOList.Columns(15).HeaderText = "W.O. Status"
	'        lblResult.Text = "List of Work Order as per criteria :" & mWOList.Count & " Record(s) found"
	'    End If
	'    GridColumnsVisibility()
	'    upnlGridView.Update()
	'    upnlActionBtnTop.Update()
	'    upnlActionBtnBottom.Update()
	'    upnlResult.Update()
	'    '--------------------------------------
	'End Sub
#End Region

End Class