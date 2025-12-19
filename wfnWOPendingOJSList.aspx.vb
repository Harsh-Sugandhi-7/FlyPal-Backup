Public Class wfnWOPendingOJSList
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Public mTransTypeID As Trans
	Public mOJSWOList As nWOOJSList
	Dim totcnt As Integer
	Public mnWO As nWO
	Dim mWODetail As String
	Dim SearchIndex, DateIndex, FromDate, ToDate, WOText, WOJobTypeID, No, WOJobStatusID As String
	Dim mDistinctWOText As nDistinctWOText
	Dim mtmpTransTypeID As Integer
	Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
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
#Region "Events"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		ClearAll()

		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		If Not IsPostBack Then
			If Session("mTransTypeId") Is Nothing Then
				mTransTypeID = Request.QueryString("TransTypeId")
				Session("mTransTypeId") = mTransTypeID
			Else
				mTransTypeID = Session("mTransTypeId")
			End If

			'If mTransTypeID = Trans.WO145 Then
			'    Session("MiddleFrame") = "wfnWOPendingOJSList.aspx?TransTypeID=" & Trans.OJS145
			'Else
			'    Session("MiddleFrame") = "wfnWOPendingOJSList.aspx?TransTypeID=" & Trans.OJSCAMO
			'End If


			mOJSWOList = nWOOJSList.GetWOOJSList(mtmpTransTypeID)
			Session("mOJSWOList") = mOJSWOList
			dgWOPendingOJS.DataSource = mOJSWOList
			dgWOPendingOJS.DataBind()

			mDistinctWOText = nDistinctWOText.GetDistinctWOText("(All)")
			cmbWO.DataSource = mDistinctWOText
			Session("mDistinctWOText") = mDistinctWOText
			cmbWO.DataBind()
			upnlSearchCriteria.Update()
			ControlVisibility()
		End If
		SetTitle()
	End Sub
	Private Sub dgWOPendingOJS_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOPendingOJS.RowCommand
		Select Case e.CommandName
			Case "Select"
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				Dim Index As Integer = CInt(e.CommandArgument) + dgWOPendingOJS.PageSize * dgWOPendingOJS.PageIndex

				Dim mId As Guid = mOJSWOList(Index).WOID
				Dim mDate As String = mOJSWOList(Index).WODateFormatted
				Dim mWorkOrderNo As String = mOJSWOList(Index).WONumber

				Dim ntempWOJob As nWOOJSList.nWOOJSListInfo = mOJSWOList(Index)


				mWODetail = mWorkOrderNo + " Dated : " + mDate
				MarkLog(Util.Action.Edit, "Work Order", mWODetail, Util.ErrorType.NoError, mId, EventLogID)

				NewRecord(ntempWOJob)
				Session("Edit") = True
				Dim str As String
				str = "openledgersame('wfnWODetail_AJAX.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
		End Select

	End Sub
	Private Sub cmbDate_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
		ClearControls()
		Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
		ControlVisibility(PeriodIndex:=DateIndex)
		setPeriod(DateIndex)
	End Sub
	Private Sub cmbWO_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbWO.SelectedIndexChanged
		If cmbWO.SelectedIndex = 0 Then
			txtNo.Visible = False
		Else
			txtNo.Visible = True
		End If
		upnlSearchCriteria.Update()
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		setVariables()
		CallFindNow(SearchIndex)
		dgWOPendingOJS.DataBind()



		lbldgGridResult.Text = "Work Order Pending OJS List  [Total No of Record(s):-" + mOJSWOList.Count.ToString() + "]"

	End Sub
#End Region

#Region " Business Methods "
	Private Sub CallFindNow(ByVal Index As Integer)
		FindNow(WOText, CInt(Val(No)), txtFromDate.Text.ToString, txtToDate.Text.ToString, , WOJobTypeID)
		dgWOPendingOJS.PageIndex = 0
	End Sub

	Private Sub FindNow(Optional ByVal Text As String = "", Optional ByVal No As Int32 = 0, Optional ByVal FromDate As String = "1/1/1900",
						Optional ByVal ToDate As String = "1/1/2200", Optional ByVal WOJobStatusID As Integer = 0,
						Optional ByVal WOJobTypeID As Integer = 0)
		mOJSWOList = Nothing
		dgWOPendingOJS.DataSource = Nothing

		mOJSWOList = nWOOJSList.GetWOOJSList(mtmpTransTypeID, Text, No, FromDate, ToDate, WOJobStatusID)

		dgWOPendingOJS.DataSource = mOJSWOList
		dgWOPendingOJS.DataBind()
		upnldgGrid.Update()
		Session("mOJSWOList") = mOJSWOList


	End Sub
	Private Sub setVariables()

		DateIndex = IIf(cmbDate.SelectedIndex <= 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		WOText = IIf(cmbWO.SelectedIndex <= 0, "", cmbWO.SelectedValue)
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
		' WOJobTypeID = IIf(cmbWOJobType.SelectedIndex <= 0, 0, cmbWOJobType.SelectedValue)
		No = txtNo.Text.Trim

		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchIndex") = SearchIndex
		Session("DateIndex") = DateIndex
		Session("WOJobTypeIDFromWOExecutionList") = WOJobTypeID
		Session("WOJobStatusID") = WOJobStatusID
		Session("No") = No
		Session("WOText") = WOText

	End Sub
	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		'Deciding IsInRole String to check Rights
		IsInRoleString = "OJSWorkOrder"

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
	Private Sub GetSession()
		mOJSWOList = Session("mOJSWOList")
		mTransTypeID = Session("mTransTypeId")
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		mtmpTransTypeID = Session("mtmpTransTypeID")
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mOJSWOList")
		Session.Remove("mTransTypeId")
	End Sub
	Private Sub ClearAll()
		mTransTypeID = Session("mTransTypeId")
		If InStr(Session("MiddleFrame"), "wfnWOPendingOJSList.aspx?TransTypeID=" & Request.QueryString("TransTypeId")) <= 0 Then

		End If
	End Sub
	Private Sub SetTitle()
		mOJSWOList = Session("mOJSWOList")
		totcnt = mOJSWOList.Count
		Session("totcnt") = totcnt
		' lblList.Text = "Work Order Pending OJS List  [Total No of Record(s):-" + totcnt.ToString() + "]"
	End Sub
	Private Sub NewRecord(ntmpWOJob As nWOOJSList.nWOOJSListInfo)
		Dim mnWOCopy As nWO
		Dim mWOJobNRCList As WOJobNRCList
		Dim tmpAssemblyStatusList As AssemblyStatusList
		'mnWOCopy = nWO.GetWO(mId, False)

		'mWOJobNRCList = WOJobNRCList.GetWOJobNRCList(mId, mJobId)
		'Session("mWOJobNRCList") = mWOJobNRCList

		mnWO = nWO.NewWO(, mTransTypeID)
		mnWO.SerialNo = ntmpWOJob.SerialNo
		mnWO.RegNo = ntmpWOJob.RegNo
		mnWO.ModelName = ntmpWOJob.ModelName


		If mTransTypeID <> 90 Then
			mnWO.MachineID = ntmpWOJob.MachineID
			tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(ntmpWOJob.WODate, ntmpWOJob.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
			AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)

		End If
		If mTransTypeID = 88 Then
			mnWO.TransTypeID = 90 'For 145 OJS W.O.
		ElseIf mTransTypeID = 89 Then
			mnWO.TransTypeID = 91 'For CAMO OJS W.O.
		Else
			mnWO.TransTypeID = mTransTypeID
		End If

		mnWO.WOJobs.Add(mnWO.ID, 5, ntmpWOJob.ID.ToString)
		'  mnWO.WONRCJobs.Add(mnWO.ID, 5, )
		mnWO.WOJobs.CurrentItem.RefWOJobID = ntmpWOJob.ID
		mnWO.WOJobs.CurrentItem.WOJobDescription = ntmpWOJob.WOJobDescription
		mnWO.WOJobs.CurrentItem.DueAsOf = ntmpWOJob.DueAsOf
		mnWO.WOJobs.CurrentItem.TSNCSN = ntmpWOJob.TSNCSN
		mnWO.WOJobs.CurrentItem.SBADNO = ntmpWOJob.SBADNO
		mnWO.WOJobs.CurrentItem.InspCode = ntmpWOJob.InspCode
		mnWO.WOJobs.CurrentItem.TaskSourceRef = ntmpWOJob.TaskSourceRef


		mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = ntmpWOJob.WOMaintenanceEvent
		mnWO.WOJobs.CurrentItem.Zone = ntmpWOJob.Zone
		mnWO.WOJobs.CurrentItem.AREA = ntmpWOJob.AREA
		mnWO.WOJobs.CurrentItem.IsRII = ntmpWOJob.IsRII
		mnWO.WOJobs.CurrentItem.WOJobRemark = ntmpWOJob.WOJobRemark
		mnWO.WOJobs.CurrentItem.IsUnderMEL = ntmpWOJob.IsUnderMEL
		mnWO.WOJobs.CurrentItem.DateOfOccurrence = ntmpWOJob.DateOfOccurence
		mnWO.WOJobs.CurrentItem.ATAChapterID = ntmpWOJob.ATAChapterID
		mnWO.WOJobs.CurrentItem.CompID = ntmpWOJob.CompID
		mnWO.WOJobs.CurrentItem.MELCategoryID = ntmpWOJob.MELCategoryID
		mnWO.WOJobs.CurrentItem.IsMajor = ntmpWOJob.IsMajor
		mnWO.WOJobs.CurrentItem.IsRepetitive = ntmpWOJob.IsRepetitive
		mnWO.WOJobs.CurrentItem.IsHours = ntmpWOJob.IsHours
		mnWO.WOJobs.CurrentItem.FrequencyInDays = ntmpWOJob.FrequencyInDays
		mnWO.WOJobs.CurrentItem.FrequencyInHours = ntmpWOJob.FrequencyInHours

		Dim mWOJob As nWOJob = nWOJob.GetWOJobNRC(ntmpWOJob.ID, 5)

		'Task Cards
		For Each mWOJobTask As nWOJobTask In mWOJob.WOJobTasks
			mnWO.WOJobs.CurrentItem.WOJobTasks.Add(mnWO.WOJobs.CurrentItem.ID)

			With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem
				.TaskAction = ""
				.ActualStartDate = mWOJobTask.ActualStartDate
				.ActualEndDate = mWOJobTask.ActualEndDate
				.IsDone = False
				.TaskCardID = mWOJobTask.TaskCardID

				.TaskCardNo = mWOJobTask.TaskCardNo
				.TaskDescription = mWOJobTask.TaskDescription
				.RevNo = mWOJobTask.RevNo
				.RevDate = mWOJobTask.RevDate
				.IssueDate = mWOJobTask.IssueDate
				.Reference = mWOJobTask.Reference

				.Equipment = mWOJobTask.Equipment
				.Material = mWOJobTask.Material
				.EstimatedHours = mWOJobTask.EstimatedHours
				.checks = mWOJobTask.checks
				.RelatedTaskCardsNo = mWOJobTask.RelatedTaskCardsNo
				.ImageSize = mWOJobTask.ImageSize
				.ImageFile = mWOJobTask.ImageFile
				.FileExtension = mWOJobTask.FileExtension

				For Each mWOJobTaskSpare As nWOJobTaskSpare In mWOJobTask.WOJobTaskSpares
					.WOJobTaskSpares.Add(mWOJobTask.ID)
					With .WOJobTaskSpares.CurrentItem
						.ItemID = mWOJobTaskSpare.ItemID
						.RequiredQty = mWOJobTaskSpare.RequiredQty
						.PartNo = mWOJobTaskSpare.PartNo
						.Description = mWOJobTaskSpare.Description
						.Remark = mWOJobTaskSpare.Remark
						.OnSerialNo = mWOJobTaskSpare.OnSerialNo
						.OffSerialNo = mWOJobTaskSpare.OffSerialNo
						.IsForSteps = False
					End With

				Next
				For Each mWOJobTaskStepsSpare As nWOJobTaskSpare In mWOJobTask.WOJobTaskStepsSpares
					.WOJobTaskStepsSpares.Add(mWOJobTask.ID)
					With .WOJobTaskStepsSpares.CurrentItem
						.ItemID = mWOJobTaskStepsSpare.ItemID
						.RequiredQty = mWOJobTaskStepsSpare.RequiredQty
						.PartNo = mWOJobTaskStepsSpare.PartNo
						.Description = mWOJobTaskStepsSpare.Description
						.Remark = mWOJobTaskStepsSpare.Remark
						.OnSerialNo = mWOJobTaskStepsSpare.OnSerialNo
						.OffSerialNo = mWOJobTaskStepsSpare.OffSerialNo
						.IsForSteps = True
					End With
				Next
				'Added By Vikrant on 03-Mar-2020 For ALL03032020
				For Each mWOJobTaskSpare As nWOJobTaskSpare In mWOJobTask.WOJobTaskPartRemovals
					mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
					With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.CurrentItem
						.ItemID = mWOJobTaskSpare.ItemID
						.RequiredQty = mWOJobTaskSpare.RequiredQty
						.PartNo = mWOJobTaskSpare.PartNo
						.Description = mWOJobTaskSpare.Description
						.Remark = mWOJobTaskSpare.Remark
						.OnSerialNo = mWOJobTaskSpare.OnSerialNo
						.OffSerialNo = mWOJobTaskSpare.OffSerialNo
						.IsForSteps = False
						.IsPartRemoval = True
						.Position = mWOJobTaskSpare.Position
					End With

				Next
				'End
			End With

		Next

		'Designation Allocation
		For Each mWOJobDesignationAllocation As nWOJobDesignationAllocation In mWOJob.WOJobDesignationAllocations

			mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.Add(mnWO.WOJobs.CurrentItem.ID)

			With mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.CurrentItem
				.DesignationID = mWOJobDesignationAllocation.DesignationID
				.EstimatedTime = mWOJobDesignationAllocation.EstimatedTime
				.ActualTime = mWOJobDesignationAllocation.ActualTime
				.Rate = mWOJobDesignationAllocation.Rate

				For Each mResourceAlocation As nWOJobResourceAllocation In mWOJobDesignationAllocation.WOJobResourceAllocations
					.WOJobResourceAllocations.Add(mWOJobDesignationAllocation.ID, mResourceAlocation.ID)
					With .WOJobResourceAllocations.CurrentItem
						.ResourceID = mResourceAlocation.ID
						.WOJobDesignationAllocationID = mResourceAlocation.WOJobDesignationAllocationID
						.ResourceActualTime = mResourceAlocation.ResourceActualTime

						For Each mResourceDetail As nWOJobResourceDetail In mResourceAlocation.WOJobResourceDetails
							.WOJobResourceDetails.Add(mResourceAlocation.ID)
							With .WOJobResourceDetails.CurrentItem
								.StartDateTime = mResourceDetail.StartDateTime
								.EndDateTime = mResourceDetail.EndDateTime
								.TotalTime = mResourceDetail.TotalTime
							End With
						Next
					End With
				Next
			End With
		Next

		'Spares
		For Each mWOJobSpare As nWOJobSpare In mWOJob.WOJobSpares
			mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)
			With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
				.ItemID = mWOJobSpare.ItemID
				.PartNo = mWOJobSpare.PartNo
				.Description = mWOJobSpare.Description
				.RequiredQty = mWOJobSpare.RequiredQty
				.IsForBilling = mWOJobSpare.IsForBilling
				.Remark = mWOJobSpare.Remark
				.EffRate = mWOJobSpare.EffRate
				.EstimatedCost = mWOJobSpare.EstimatedCost
			End With
		Next

		'Removal/Installations
		For Each WOJobComp As nWOJobComp In mWOJob.WOJobComps
			mnWO.WOJobs.CurrentItem.WOJobComps.Add(mnWO.WOJobs.CurrentItem.ID, mWOJob.WOJobTypeID)
			With mnWO.WOJobs.CurrentItem.WOJobComps.CurrentItem
				.IsAssembly = WOJobComp.IsAssembly
				.IsForRemoval = WOJobComp.IsForRemoval
				.OffPartID = WOJobComp.OffPartID
				.OffRemark = WOJobComp.OffRemark
				.RemovalReasonID = WOJobComp.RemovalReasonID
				.OffTSN = WOJobComp.OffTSN
				.OffCSN = WOJobComp.OffCSN
				.IsForInstall = WOJobComp.IsForInstall
				.OnRemark = WOJobComp.OnRemark
				.OnTSN = WOJobComp.OnTSN
				.OnCSN = WOJobComp.OnCSN
				.OnSerialNo = WOJobComp.OnSerialNo
				.OffDescription = WOJobComp.OffDescription
				.OffSerialNo = WOJobComp.OffSerialNo
				.OnPartID = WOJobComp.OnPartID
				.OnPartNo = WOJobComp.OnPartNo
				.OnDescription = WOJobComp.OnDescription
				.OffPartNo = WOJobComp.OffPartNo
				.OffPosition = WOJobComp.OffPosition
				.OnPosition = WOJobComp.OnPosition
			End With
		Next



		mnWO.MarkClean()
		Session("mnWO") = mnWO
		Session("mTransTypeId") = mTransTypeID
	End Sub
	Private Sub ClearControls()
		txtNo.Text = ""
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
					txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
				End If
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 6 'Between Dates
				FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
				ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString))
				txtFromDate.Text = FromDate
				txtToDate.Text = ToDate
		End Select
	End Sub
#End Region

	Private Sub btnBackTop_Click(sender As Object, e As System.EventArgs) Handles btnBackTop.Click, btnBack.Click
		RemoveSession()
		Response.Redirect("index.aspx")
	End Sub
End Class