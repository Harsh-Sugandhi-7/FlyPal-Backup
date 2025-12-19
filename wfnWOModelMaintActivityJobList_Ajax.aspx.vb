'Added By Vikrant on 24-Apr-2018 For All24042018

Imports System.Collections.Generic
Imports System.Text

Public Class wfnWOModelMaintActivityJobList_Ajax
	Inherits System.Web.UI.Page

#Region "Variable Declaration"
	Public mModelMaintActivityList As ModelMaintActivityList
	Public mnWO As nWO
	Dim mIsSelected As Boolean = False
	Private checkedIds As New List(Of String)()
#End Region

#Region "Business Methods"
	Private Sub GetSession()
		mnWO = Session("mnWO")
		mModelMaintActivityList = Session("mModelMaintActivityList")
	End Sub
	Private Sub SetTitle()
		lblResult.Text = "List of Jobs as per criteria :" & mModelMaintActivityList.Count & " Record(s) found."
	End Sub
	Private Sub AddJobs()
		Dim builder = New StringBuilder()
		builder.Append("You have selected the following checks :<br/>")
		' get the selected checkboxes from the form data
		Dim checkString = Request.Form("chkSelect")
		If checkString Is Nothing Then
			MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			' we'll need a split to get the individual ids
			Dim values As String() = checkString.Split(","c)
			If (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR") And values.Length > 1 Then
				MSGBoxCtrl.show("Selection Alert!", "Multiple Jobs can not be added in single WO.", "", MsgBoxStyle.OkOnly, "RestrictMultJobs")
				Exit Sub
			End If

			For Each value As String In values
				builder.Append("<br/>")
				builder.Append(value)
				checkedIds.Add(value)
				mModelMaintActivityList(New Guid(value)).IsSelected = True
			Next

			For i As Integer = 0 To mModelMaintActivityList.Count - 1
				If mModelMaintActivityList(i).IsSelected = True And Array.IndexOf(values, mModelMaintActivityList(i).ID.ToString) = -1 Then
					mModelMaintActivityList(i).IsSelected = False
				End If
			Next
			'values = ""
			checkString = Nothing
		End If

		For i As Integer = 0 To mModelMaintActivityList.Count - 1
			If mModelMaintActivityList(i).IsSelected = False Then
				If mnWO.WOJobs.Contains(mModelMaintActivityList.Item(i).ID, "") Then
					mnWO.WOJobs.Remove(mModelMaintActivityList.Item(i).ID, "")
				End If
			End If
		Next
		Session("mnWO") = mnWO
		Session("mModelMaintActivityList") = mModelMaintActivityList
	End Sub
	Private Sub setObject()
		Dim i As Integer = 0
		While i < mModelMaintActivityList.Count
			If mModelMaintActivityList.Item(i).IsSelected = True Then
				mIsSelected = True
				If mnWO.WOJobs.Contains(mModelMaintActivityList.Item(i).ID, "") = False Then
					Dim Description As String = ""
					Dim AssemblyTypeWithPosition As String = ""
					With mModelMaintActivityList.Item(i)
						Description = "Description: " + mModelMaintActivityList.Item(i).Description + " Code/Form No.: " + mModelMaintActivityList.Item(i).Code + " Ref.: " + mModelMaintActivityList.Item(i).Reference + " Desc.: " + mModelMaintActivityList.Item(i).Description
					End With

					'WOJOB:
					mnWO.WOJobs.Add(mnWO.ID, Val(Session("WOJobTypeID")))
					mnWO.WOJobs.CurrentItem.PreviousTransID = mModelMaintActivityList.Item(i).ID
					mnWO.WOJobs.CurrentItem.WOJobDescription = Description
					mnWO.WOJobs.CurrentItem.DueAsOf = ""

					'If Not mSelectDueJobs.Item(i).StartDate Is DBNull.Value Then mnWO.WOJobs.CurrentItem.WOJobStartDate = mSelectDueJobs.Item(i).StartDate
					mnWO.WOJobs.CurrentItem.SBADNO = mModelMaintActivityList.Item(i).ModNo
					mnWO.WOJobs.CurrentItem.ATAChapterID = mModelMaintActivityList.Item(i).ATAID


					If mModelMaintActivityList.Item(i).MaintActivityTypeID = 1 Then
						mnWO.WOJobs.CurrentItem.MonitorTypeID = 1
					ElseIf mModelMaintActivityList.Item(i).MaintActivityTypeID = 2 Then
						mnWO.WOJobs.CurrentItem.MonitorTypeID = 2
					ElseIf mModelMaintActivityList.Item(i).MaintActivityTypeID = 3 Then
						mnWO.WOJobs.CurrentItem.MonitorTypeID = 3
					End If
					mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = mModelMaintActivityList.Item(i).RequiredManHours
					mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = mModelMaintActivityList.Item(i).Description

					With mnWO.WOJobs.CurrentItem
						Dim mMaintenanceTask As MaintenanceTask
						Dim mMaintenanceTaskDetail As MaintenanceTaskDetail

						mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForModelActivity(.PreviousTransID, .MonitorTypeID, .PreviousTransID, True, True)

						For Each mMaintenanceTaskDetail In mMaintenanceTask.MaintenanceTaskDetails
							mnWO.WOJobs.CurrentItem.WOJobTasks.Add(mnWO.WOJobs.CurrentItem.ID)

							With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem
								'.TaskAction = "No action taken." 'mMaintenanceTaskDetail.Task 'Commented By Prashant 12-Mar-2010
								.TaskAction = ""  'Added By Prashant 12-Mar-2010
								.ActualStartDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
								.ActualEndDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
								.IsDone = False
								.TaskCardID = mMaintenanceTaskDetail.TaskCardID  'Added By Prashant 29-Dec-2008

								'Added By Utkarsh On 27-Apr-2011

								Dim mTaskCard As TaskCard
								mTaskCard = TaskCard.GetTaskCard(mMaintenanceTaskDetail.TaskCardID)
								.TaskCardNo = mTaskCard.TaskCardNo
								.TaskDescription = mTaskCard.TaskDesc
								.RevNo = mTaskCard.RevNo
								.RevDate = mTaskCard.RevDate
								.IssueDate = mTaskCard.IssueDate

								''Added by Saylee on 4-Feb-2013
								''If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Then
								''    .Reference = mSelectDueJobs.Item(i).Reference
								''Else
								''    .Reference = mTaskCard.Reference
								''End If
								'***************************
								''Commentedby Saylee on 15-Feb-2013
								.Reference = mTaskCard.Reference

								.Equipment = mTaskCard.Equipment
								.Material = mTaskCard.Material
								.EstimatedHours = mTaskCard.EstimatedHours
								.checks = mTaskCard.Check
								.RelatedTaskCardsNo = mTaskCard.RelatedTaskCardsNo
								.ImageSize = mTaskCard.ImageSize
								.ImageFile = mTaskCard.ImageFile
								.FileExtension = mTaskCard.FileExtension

								'Added by Vikrant on 06-Sept-2013 For BA04092013
								Dim mTaskCardSpare As TaskCardSpare
								Dim mTaskCardStepsSpare As TaskCardSpare

								For Each mTaskCardSpare In mTaskCard.TaskCardSpares
									mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
									With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.CurrentItem
										.ItemID = mTaskCardSpare.ItemID
										.RequiredQty = mTaskCardSpare.RequiredQty
										.PartNo = mTaskCardSpare.PartNo
										.Description = mTaskCardSpare.Description
										.Remark = mTaskCardSpare.Remark
										.OnSerialNo = mTaskCardSpare.OnSerialNo
										.OffSerialNo = mTaskCardSpare.OffSerialNo
										.IsForSteps = False
									End With

								Next

								For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
									mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
									With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.CurrentItem
										.ItemID = mTaskCardStepsSpare.ItemID
										.RequiredQty = mTaskCardStepsSpare.RequiredQty
										.PartNo = mTaskCardStepsSpare.PartNo
										.Description = mTaskCardStepsSpare.Description
										.Remark = mTaskCardStepsSpare.Remark
										.OnSerialNo = mTaskCardStepsSpare.OnSerialNo
										.OffSerialNo = mTaskCardStepsSpare.OffSerialNo
										.IsForSteps = True
									End With
								Next
								'End
								'Added By Vikrant on 03-Mar-2020 For ALL03032020
								For Each mTaskCardSpare In mTaskCard.TaskCardPartRemovals
									mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
									With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.CurrentItem
										.ItemID = mTaskCardSpare.ItemID
										.RequiredQty = mTaskCardSpare.RequiredQty
										.PartNo = mTaskCardSpare.PartNo
										.Description = mTaskCardSpare.Description
										.Remark = mTaskCardSpare.Remark
										.OnSerialNo = mTaskCardSpare.OnSerialNo
										.OffSerialNo = mTaskCardSpare.OffSerialNo
										.IsForSteps = False
										.IsPartRemoval = True
										.Position = mTaskCardSpare.Position
									End With

								Next
								'End
							End With
						Next

						'KIT(s):
						Dim mMaintenanceKit As MaintenanceKit

						mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForModelActivity(.PreviousTransID, .MonitorTypeID, .PreviousTransID, True, False, True)

						'Commented and Added by Saylee on 23-July-2013 for BA22072013 	
						''''For Each mMaintenanceKitDetail In mMaintenanceKit.MaintenanceKitDetails
						''''    mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

						''''    With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
						''''        .ItemID = mMaintenanceKitDetail.ItemID
						''''        .RequiredQty = mMaintenanceKitDetail.Qty
						''''        Dim mItem As Item = Item.GetItem(mMaintenanceKitDetail.ItemID)
						''''        .PartNo = mItem.Name
						''''        .Description = mItem.Description
						''''        mItem = Nothing
						''''    End With
						''''Next
						'''''-----------------------------------------------------------------------
						'Added by Saylee on 23-July-2013 for BA22072013 	
						Dim mMaintenanceSpares As MaintenanceKit
						Dim mMaintenanceSparesDetail As MaintenanceKitDetail

						Dim mMaintenanceTools As MaintenanceKit
						Dim mMaintenanceToolsDetail As MaintenanceKitDetail

						mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForModelActivity(.PreviousTransID, .MonitorTypeID, .PreviousTransID, True, False, True)
						mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForModelActivity(.PreviousTransID, .MonitorTypeID, .PreviousTransID, True, True, True)

						For Each mMaintenanceSparesDetail In mMaintenanceSpares.MaintenanceKitDetails
							mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

							With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
								.ItemID = mMaintenanceSparesDetail.ItemID
								.RequiredQty = mMaintenanceSparesDetail.Qty
								Dim mItem As Item = Item.GetItem(mMaintenanceSparesDetail.ItemID)
								.PartNo = mItem.Name
								.Description = mItem.Description
								mItem = Nothing
								.Remark = mMaintenanceSparesDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
							End With
						Next

						For Each mMaintenanceToolsDetail In mMaintenanceTools.MaintenanceKitDetails
							If Not mnWO.WOTools.Contains(mMaintenanceToolsDetail.ItemID) Then
								mnWO.WOTools.Add(mnWO.ID)

								With mnWO.WOTools.CurrentItem
									.ItemID = mMaintenanceToolsDetail.ItemID
									.RequiredQty = mMaintenanceToolsDetail.Qty
									Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
									.PartNo = mItem.Name
									.Description = mItem.Description
									mItem = Nothing
									.WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
								End With
							Else
								mnWO.WOTools.CurrentIndex = mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").SrNo - 1
								If mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty = 0 Then

								Else
									If (mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty <= mMaintenanceToolsDetail.Qty) Or (mMaintenanceToolsDetail.Qty = 0) Then
										With mnWO.WOTools.CurrentItem
											.ItemID = mMaintenanceToolsDetail.ItemID
											.RequiredQty = mMaintenanceToolsDetail.Qty
											Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
											.PartNo = mItem.Name
											.Description = mItem.Description
											mItem = Nothing
											.WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
										End With
									End If
								End If
							End If
						Next
						'-----------------------------------------------------------------------
					End With
				End If
			Else
				''If mnWO.WOJobs.Contains(mSelectDueJobs.Item(i).ID, "") Then
				''    mnWO.WOJobs.Remove(mSelectDueJobs.Item(i).ID, "")
				''End If
			End If
			i = i + 1
		End While
		Session("mnWO") = mnWO
	End Sub
	Private Sub addAttributes()
		txtATACode.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtATACode').value,event)")
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mModelMaintActivityList = ModelMaintActivityList.GetList(mnWO.ModelName)
		If mModelMaintActivityList IsNot Nothing Then
			For Each Child As ModelMaintActivity In mModelMaintActivityList
				Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")
				If mnWO.WOJobs.Contains(Child.ID, "") Then
					checkedIds.Add(Child.ID.ToString)
				End If
			Next
		End If
		dgDueJob.DataSource = mModelMaintActivityList
		Session("mModelMaintActivityList") = mModelMaintActivityList
		DataBind()

		If mModelMaintActivityList.Count > 10 Then btnDoneTop.Visible = True
		If mModelMaintActivityList.Count > 10 Then btnBackTop.Visible = True
	End Sub
#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		addAttributes()
		If Not IsPostBack Then
			DataFieldBind()
			SetTitle()
			lblTitle.Text = "Jobs for " & mnWO.ModelName
		End If
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
		If IsValid Then
			dgDueJob.PageIndex = 0

			mModelMaintActivityList = ModelMaintActivityList.GetList(mnWO.ModelName, Val(txtATACode.Text), Trim(txtDescription.Text), Trim(txtReference.Text))

			If mModelMaintActivityList IsNot Nothing Then
				For Each Child As ModelMaintActivity In mModelMaintActivityList
					Child.IsSelected = mnWO.WOJobs.Contains(Child.ID, "")
					If mnWO.WOJobs.Contains(Child.ID, "") Then
						checkedIds.Add(Child.ID.ToString)
					End If
				Next
			End If

			dgDueJob.DataSource = mModelMaintActivityList
			Session("mModelMaintActivityList") = mModelMaintActivityList
			DataBind()
			SetTitle()
			UpnlResult.Update()
			UpnlGrid.Update()
		End If
	End Sub
	Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoneTop.Click, btnDone.Click
		'' If Not CustomValidate1() Then Exit Sub
		AddJobs()
		setObject()
		Dim checkString = Request.Form("chkSelect")
		If checkString Is Nothing Then
			MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please select atleast one job", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			Dim values As String() = checkString.Split(","c)
			If (AppSettings("ClientCode") = "IND" Or AppSettings("ClientCode") = "STR") And values.Length > 1 Then
				Exit Sub
			End If
			Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
		End If

	End Sub
	Private Sub dgDueJob_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDueJob.PageIndexChanged
		dgDueJob.PageIndex = e.NewPageIndex
		dgDueJob.DataSource = mModelMaintActivityList
		Session("mModelMaintActivityList") = mModelMaintActivityList
		dgDueJob.DataBind()
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub
#End Region

#Region "Checked Selection"

	Public Function NumeroChequeInclus(ByVal numero As String) As String

		If (checkedIds.Contains(numero)) Then
			Return "checked"
		Else
			Return String.Empty
		End If
	End Function
#End Region


End Class