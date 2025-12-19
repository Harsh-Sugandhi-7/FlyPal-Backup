'Created By Saylee on 19-Apr-2024

Public Class wfnWOJobCompRemInstList
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
	Public mnWOJobCompRemInstList As nWOJobCompRemInstList
	Dim mMachineNameValueList As MachineNameValueList
	Dim mWOModelNameValueList As nWOModelNameValueList
	Dim mDistinctWOText As nDistinctWOText
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, StatusID, No, WOStatusID, RegNo, ModelName, ShowNoE, pageIndex As String
	Dim EventLogID As Guid
	Dim mWODetail As String
	Dim totcnt As Integer

	Dim IsReadOnly As Boolean

	Public mRemovedAssemblyListForCombo As RemovedAssemblyListForCombo
	Public mRemovedCompListForCombo As RemovedCompListForCombo

#End Region

#Region " Business Methods "
	Private Sub GetSession()

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


		mRemovedAssemblyListForCombo = Session("mRemovedAssemblyListForCombo")
		mRemovedCompListForCombo = Session("mRemovedCompListForCombo")

		ShowNoE = Session("ShowNoE")
		pageIndex = Session("PageIndex")
		mnWOJobCompRemInstList = Session("mnWOJobCompRemInstList")
	End Sub
	Private Sub SetSession()
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


		Session("mRemovedAssemblyListForCombo") = mRemovedAssemblyListForCombo
		Session("mRemovedCompListForCombo") = mRemovedCompListForCombo

		Session("ShowNoE") = ShowNoE
		Session("PageIndex") = pageIndex
		Session("mnWOJobCompRemInstList") = mnWOJobCompRemInstList
	End Sub
	Private Sub RemoveSession()

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
		Session.Remove("mTransTypeId")

		Session.Remove("mRemovedAssemblyListForCombo")
		Session.Remove("mRemovedCompListForCombo")

		Session.Remove("ShowNoE")
		Session.Remove("PageIndex")
		Session.Remove("mnWOJobCompRemInstList")
	End Sub
	Private Sub ClearAll()
		If InStr(Session("MiddleFrame"), "wfnWOJobCompRemInstList.aspx?") <= 0 Then
			RemoveSession()

		End If
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub setPeriod(ByVal Index As Int32)

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
	Private Sub setVariables()
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")

		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)
		WOStatusID = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)

		RegNo = IIf(cmbAircraft.SelectedIndex <= 0, "", cmbAircraft.SelectedValue)
		ModelName = IIf(cmbModel.SelectedIndex <= 0, "", cmbModel.SelectedValue)
		No = txtNo.Text.Trim
		ShowNoE = IIf(cmbShowE.SelectedIndex <= 0, 0, cmbShowE.SelectedValue)


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

		Session("ShowNoE") = ShowNoE
	End Sub
	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		IsInRoleString = "CAMORemoval/Installation"

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
		dgnWOJobCompRemInstList.DataBind()
		cmbAircraft.SelectedValue = IIf(RegNo = "", "(ALL)", RegNo)

		cmbModel.SelectedValue = IIf(ModelName = "", "(ALL)", ModelName)
		cmbWO.SelectedValue = IIf(WOText = "", "(ALL)", WOText)
		txtNo.Text = No
		cmbStatus.SelectedValue = WOStatusID

		If ShowNoE Is Nothing Then
			cmbShowE.SelectedValue = "4"
		Else
			cmbShowE.SelectedValue = ShowNoE '
		End If

		If pageIndex Is Nothing Then
			dgnWOJobCompRemInstList.PageIndex = 0
		Else
			dgnWOJobCompRemInstList.PageIndex = pageIndex
		End If

		ControlVisibility(SearchIndex, DateIndex)

		lblResult.Text = "List of Removal/Installations from Work Order as per criteria :" & mnWOJobCompRemInstList.Count & " Record(s) found."
	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")

	End Sub
	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "",
					  Optional ByVal ToDate As String = "", Optional ByVal RegNo As String = "", Optional ByVal ModelName As String = "",
					  Optional ByVal WOStatusID As Integer = 0, Optional ByVal StatusID As Integer = 0, Optional ByVal AddTopItem As String = "",
					  Optional ByVal AssemblyStatusID As String = "{00000000-0000-0000-0000-000000000000}", Optional ByVal CustomerID As String = "{00000000-0000-0000-0000-000000000000}")
		mnWOJobCompRemInstList = Nothing
		dgnWOJobCompRemInstList.DataSource = Nothing

		mnWOJobCompRemInstList = nWOJobCompRemInstList.GetJobCompRemInstList(Text, No, FromDate, ToDate, RegNo)
		dgnWOJobCompRemInstList.DataSource = mnWOJobCompRemInstList
		Session("mnWOJobCompRemInstList") = mnWOJobCompRemInstList
		totcnt = mnWOJobCompRemInstList.Count
		Session("totcnt") = totcnt
		dgnWOJobCompRemInstList.PageSize = CInt(cmbShowE.SelectedItem.ToString) 'Ajay 24-07-2023
	End Sub
	Private Sub CallFindNow(ByVal Index As Integer)
		Dim AssemblyStatusID As String
		'If mTransTypeID = Trans.SpareAssemblyWO Then
		'    AssemblyStatusID = cmbAssembly.SelectedValue
		'ElseIf mTransTypeID = Trans.SpareComponentWO Then
		'    AssemblyStatusID = cmbComponent.SelectedValue
		'Else
		'    AssemblyStatusID = Guid.Empty.ToString
		'End If
		'End
		FindNow()

	End Sub
	Private Sub ControlVisibility(ByVal SearchIndex As Int32, Optional ByVal DateIndex As Int32 = 0)
		txtFromDate.Enabled = True
		txtToDate.Enabled = True
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
					DataFieldBind()
					SetGrid()
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"
					DataFieldBind()
					SetGrid()

			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
			Session("sender") = ""
			DataFieldBind()
			SetGrid()
		End If
	End Sub
#End Region

#Region "DataFieldBind"
	Private Sub DataFieldBind()

		Session("totcnt") = totcnt
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)
		SearchIndex = IIf(IsNothing(SearchIndex), 1, SearchIndex)

		DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

		StatusID = Session("StatusID")
		WOStatusID = Session("WOStatusID")
		WOText = Session("WOText")
		RegNo = Session("RegNo")
		ModelName = Session("ModelName")

		mDistinctWOText = nDistinctWOText.GetDistinctWOText("(ALL)", TransTypeID:=89)
		cmbWO.DataSource = mDistinctWOText
		Session("mDistinctWOText") = mDistinctWOText

		mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, IsTagRequired:=True, TagText:="(ALL)", SkipIsForInventoryAircarft:=True)
		cmbAircraft.DataSource = mMachineNameValueList
		Session("mMachineNameValueList") = mMachineNameValueList

		mWOModelNameValueList = nWOModelNameValueList.GetModelList("(ALL)")
		cmbModel.DataSource = mWOModelNameValueList
		Session("mWOModelNameValueList") = mWOModelNameValueList

		mRemovedAssemblyListForCombo = RemovedAssemblyListForCombo.GetAssemblyList(Today.Date.ToString, "(SELECT)")
		Session("mRemovedAssemblyListForCombo") = mRemovedAssemblyListForCombo
		cmbAssembly.DataSource = mRemovedAssemblyListForCombo

		mRemovedCompListForCombo = RemovedCompListForCombo.GetCompList(Today.Date.ToString, AddTopItem:="(SELECT)")
		Session("mRemovedCompListForCombo") = mRemovedCompListForCombo
		cmbComponent.DataSource = mRemovedCompListForCombo



		DataBind()

	End Sub
	Private Sub SetGrid()

	End Sub
	Private Sub RemoveInstallComp(mnWO As nWO, mnWOJobComp As nWOJobComp)
		''Removal
		'Dim mMachine As Machine = Machine.GetMachine(mnWO.MachineID)
		'Dim mMachineID As Guid = Guid.Empty
		'If Not mnWO.IsSpareAssemblyWO Then
		'    mMachineID = mMachine.ID
		'End If

		''AssemblyID

		If mnWOJobComp.IsForRemoval Then


			Dim mPartListForSerialNos As PartListForSerialNos
			mPartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(mnWOJobComp.OffPartNo, mnWOJobComp.OffSerialNo, Today.Date.ToString)
			Session("mPartListForSerialNos") = mPartListForSerialNos

			If mPartListForSerialNos IsNot Nothing Then


				Dim mCompStatusList As CompStatusList = CompStatusList.GetCompStatusList(Guid.Empty, CurrentDate:=Today.Date.ToString,
																								 CompID:=mPartListForSerialNos(mnWOJobComp.OffPartID, mnWOJobComp.OffSerialNo).CompID.ToString,
																								 PartName:=mnWOJobComp.OffPartNo,
																								 CompSerialNo:=mnWOJobComp.OffSerialNo,
																								 IsCompInstalled:=True, IsCompPeriodsRequired:=False)


				Dim mCompStatusInfo As CompStatusInfo
				Dim mRemovedCompStatus As CompStatus

				mCompStatusInfo = mCompStatusList(New Guid(mnWOJobComp.CompStatusOffID.ToString))

				mRemovedCompStatus = CompStatus.GetCompStatus(mCompStatusInfo.ID, mCompStatusInfo.AssemblyStatusID, mnWO.WOCloseDateFormatted)
				''''



				Dim mtmpInstalledCompList As tmpInstalledCompList
				mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(mnWO.WOCloseDateFormatted, mCompStatusInfo.MachineID.ToString, mnWOJobComp.OffPartNo.ToString,
																				  mnWOJobComp.OffSerialNo, mRemovedCompStatus.AssemblyID,
																				  IsSpareAssembly:=IIf(mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO, True, False))
				'' Session("mInstalledCompList") = mInstalledCompList

				Dim mRemCompStatus As CompStatus
				mRemCompStatus = CompStatus.NewRemovalCompStatus(mtmpInstalledCompList(mnWOJobComp.CompStatusOffID).CompStatusID, mnWO.WOCloseDateFormatted,
																 mtmpInstalledCompList(mnWOJobComp.CompStatusOffID).AssemblyStatusID, Guid.Empty.ToString)

				Session("From_Remove") = 1 'NewRemove

				Dim mPrevCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpInstalledCompList(mnWOJobComp.CompStatusOffID).CompStatusID,
																			 mtmpInstalledCompList(mnWOJobComp.CompStatusOffID).AssemblyStatusID,
																			 mtmpInstalledCompList(mnWOJobComp.CompStatusOffID).InstalledOnDBValue)

				mRemCompStatus.RemovalWONo = mnWO.WONumber
				mRemCompStatus.RemovalReasonID = mnWOJobComp.RemovalReasonID
				mPrevCompStatus.RemovalWONo = mnWO.WONumber

				Session("mRemCompStatus") = mRemCompStatus
				Dim mRemAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mtmpInstalledCompList(mnWOJobComp.CompStatusOffID).AssemblyStatusID)
				Session("mRemAssemblyStatus") = mRemAssemblyStatus
				Session("mPrevCompStatus") = mPrevCompStatus
				Session("From_Remove") = 1
				Session("From_Inst") = 1
				Session("mtmpInstalledCompList") = mtmpInstalledCompList
				Session("mnWOJobComp") = mnWOJobComp
				Session("mnWO") = mnWO
				Session("mnWOJobCompRemInstList") = mnWOJobCompRemInstList
				If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then
					Session("IsFromSpareWO") = "True"
				End If

				If mRemAssemblyStatus.IsRemoved Then
					' MSGBoxCtrl.show(MSGBox.Message_title.AssemblyRemoved, MSGBox.Message_text.AssemblyRemoved, "", MsgBoxStyle.OkOnly, "")
					MSGBoxCtrl.show(MSGBox.Message_title.AssemblyRemoved, MSGBox.Message_text.AssemblyRemoved, mRemAssemblyStatus.AssemblyTypeName & " is currently removed, first revert the removal and then Remove/Install required Component", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

				Dim URLForWOCompliance As New Stack
				URLForWOCompliance.Push(Request.Url)
				Session("URLForWOCompliance") = URLForWOCompliance


				Session.Remove("mDoneOnCompliance")
				'' Response.Redirect("wfRemInstComp_AJAX.aspx?BackPage=" & Request.QueryString("BackPage1"))
				Dim str As String
				str = "openledgersame('wfnWOJobCompRemInst.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
			End If
		ElseIf mnWOJobComp.IsForInstall And mnWOJobComp.IsForRemoval = False Then

			Session("mnWOJobComp") = mnWOJobComp
			Session("mnWO") = mnWO
			Session("From_Inst") = 1
			Dim str As String
			str = "openledgersame('wfnWOJobCompRemInst.aspx?BackPage=index.aspx');"
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

		End If
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		ClearAll()
		addAttributes()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)

		If Not IsPostBack Then

			Session("MiddleFrame") = "wfnWOJobCompRemInstList.aspx?"

			If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "WOJobCompRemInst") Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
			Else
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
			End If
			'--------------------------
			If Session("ShowNoE") Is Nothing Then
				cmbShowE.SelectedValue = "4"
				Session("ShowNoE") = cmbShowE.SelectedValue
				ShowNoE = cmbShowE.SelectedValue
			End If

			DataFieldBind()
			SetControl()

		End If


		SetGrid()


	End Sub

	Private Sub dgnWOJobCompRemInstList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgnWOJobCompRemInstList.RowCommand
		Select Case e.CommandName

			Case "RemInst"
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Dim mID As Guid
				mID = CType(dgnWOJobCompRemInstList.DataKeys(CInt(e.CommandArgument)).Value, Guid)
				Dim mnWO As nWO = nWO.GetWO(mnWOJobCompRemInstList(mID).WOID)
				mnWO.WOJobs.CurrentIndex = mnWO.WOJobs(mnWOJobCompRemInstList(mID).WOJobID).SrNo - 1
				mnWO.WOJobs.CurrentItem.WOJobComps.CurrentIndex = mnWO.WOJobs.CurrentItem.WOJobComps(mID).SrNo - 1
				'Session("mWOJobCompsEdit") = True
				Session("mnWO") = mnWO
				'Session("mnWOJob") = mnWO.WOJobs.CurrentItem
				'Session("mnWOJobComps") = mnWO.WOJobs.CurrentItem.WOJobComps.CurrentItem
				'Session("mIsActualRemovalInst") = True
				'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenRemInstDetail", "OpenRemInstDetail();", True)

				'COMPONENT
				If mnWO.WOJobs.CurrentItem.WOJobComps.CurrentItem.IsAssembly = 0 Then
					RemoveInstallComp(mnWO, mnWO.WOJobs.CurrentItem.WOJobComps.CurrentItem)
				End If


		End Select
	End Sub
	Private Sub dgnWOJobCompRemInstList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgnWOJobCompRemInstList.PageIndexChanging
		dgnWOJobCompRemInstList.PageIndex = e.NewPageIndex
		dgnWOJobCompRemInstList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
		dgnWOJobCompRemInstList.DataSource = mnWOJobCompRemInstList
		Session("mnWOJobCompRemInstList") = mnWOJobCompRemInstList
		Session("PageIndex") = dgnWOJobCompRemInstList.PageIndex
		ControlVisibility(0)
		dgnWOJobCompRemInstList.DataBind()
		SetGrid()

	End Sub

	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
		setVariables()
		''  CallFindNow(SearchIndex)
		FindNow(Text:=WOText, No:=CInt(Val(No)), RegNo:=RegNo, FromDate:=FromDate, ToDate:=ToDate)
		dgnWOJobCompRemInstList.DataBind()
		SetGrid()
		GetSession()
		upnlGridView.Update()
	End Sub

	Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click ', btnClose.Click
		RemoveSession()
		Session("MiddleFrame") = ""
		Session.Remove("IsReadOnly")
		'ModuleName = Nothing
		Session.Remove("mIsActualRemovalInst")
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub dgnWOJobCompRemInstList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgnWOJobCompRemInstList.Sorting
		mnWOJobCompRemInstList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgnWOJobCompRemInstList.DataSource = mnWOJobCompRemInstList
		Session("mnWOJobCompRemInstList") = mnWOJobCompRemInstList
		dgnWOJobCompRemInstList.DataBind()
		SetGrid()
		ControlVisibility(0)
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
		IsReadOnly = mMachineNameValueList(cmbAircraft.SelectedValue).IsReadOnly 'Added by Saylee - Restrict User from using ReadOnly Aircraft
		Session("IsReadOnly") = IsReadOnly
		SetGrid()
		upnlSearchCriteria.Update()
		Session.Remove("IsReadOnly")
	End Sub
	Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click
		MarkFavourite(HttpContext.Current.User.Identity.Name, "CAMO Removal Install")

	End Sub

	Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click
		RemoveFavourite(HttpContext.Current.User.Identity.Name, "CAMO Removal Install")

	End Sub
	Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

		dgnWOJobCompRemInstList.PageSize = CInt(cmbShowE.SelectedItem.ToString)
		dgnWOJobCompRemInstList.DataSource = mnWOJobCompRemInstList
		dgnWOJobCompRemInstList.DataBind()

		ControlVisibility(0)
		setVariables()
		SetControl()
		upnlGridView.Update()
	End Sub
#End Region

End Class