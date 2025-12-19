'***********************************
' Modified by Harsh Sugandhi on 25th Feb 2025 FLYPAL-2221 Provision to add JOB NRC to WatchList.
'***********************************

Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Text
'Created By Saylee
'Dated : 6-Jun-2019

Public Class wfnWOJobNRC
	Inherits Page

#Region " Enumeration "

	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		Complete = 7
	End Enum

#End Region

#Region " Variable Declaration "

	Public mMELCategoryList As MELCategoryList
	Public mnWOJobStatusList As nWOJobStatusList
	Public mMELSnagPartList As MELSnagPartList
	Public mATAList As ATAList
	Public mnWOJob As nWOJob
	Dim mnWOJobs As nWOJobs
	Protected mnWO As nWO
	Dim mWOJobTypeID As Integer
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mWODetail As String
	'Added By Saylee On 27-Dec-2018
	Dim mFileJobAttach As FileAttach
	Dim IsAttachmentDeleted As Boolean = False
	'End
	Dim mRequisitionNew As RequisitionNew
	Dim mRequisitionItemsNew As RequisitionItemsNew
	Dim ReqItemIds As New StringBuilder
	Dim mMPDSkillList As MPDSkillList 'Added by Saylee on 3-Jul-2023

	Dim mModuleList As ModuleList

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		mnWOJobStatusList = Session("mnWOJobStatusList")
		mMELSnagPartList = Session("mMELSnagPartList")
		mATAList = Session("mATAList")
		mnWOJob = Session("nWOJobNRC")
		mnWO = Session("mnWO")
		mWOJobTypeID = CType(Session("WOJobTypeID"), Integer)
		'Added By Saylee On 27-Dec-2018
		mFileJobAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		'End
		mRequisitionItemsNew = Session("mRequisitionItemsNew")
		mModuleList = Session("mModuleList")

	End Sub

	Private Sub SetSession()

		Session("mMELSnagPartList") = mMELSnagPartList
		Session("WOJobTypeID") = mWOJobTypeID
		'Added By Saylee On 27-Dec-2018
		Session("mFileAttach") = mFileJobAttach
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
		'End

	End Sub

	Private Function IsInRole(CheckFor As Rights) As Boolean

		Dim IsInRoleString As String = ""

		If AppSettings("ShowNewWOFlow") = "True" Then

			If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then

				If mnWO.TransTypeID = Trans.WO145 Then
					IsInRoleString = "WOCreate"
				Else
					IsInRoleString = "CAMOWOCreate"
				End If

			ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then
				IsInRoleString = "WOPlanning"
			ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
				IsInRoleString = "WOExecution"
			ElseIf Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then
				IsInRoleString = "WOCompletion"
			ElseIf Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then
				IsInRoleString = "WOQCApproval"
			ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Then
				IsInRoleString = "WOCAMOUpdate"
			ElseIf Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then
				IsInRoleString = "WOBilling"
			End If

		Else

			If mnWO.TransTypeID = Trans.WO145 Then

				If Session("MiddleFrame") = "wfnWOJobListToComplete_AJAX.aspx" Then
					IsInRoleString = "WOJobListToComplete"
				Else
					IsInRoleString = "WorkOrder"
				End If

			ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then
				IsInRoleString = "SpareAssemblyWO"
			ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
				IsInRoleString = "SpareComponentWO"
			ElseIf mnWO.TransTypeID = Trans.EngineeringWO Then
				IsInRoleString = "EngineeringOrder"
			Else

				If Session("MiddleFrame") = "wfnWOJobListToComplete_AJAX.aspx" Then
					IsInRoleString = "WOJobListToComplete"
				Else
					IsInRoleString = "CAMOWO"
				End If

			End If

		End If

		Select Case CheckFor
			Case Rights.View
				Return User.IsInRole(IsInRoleString + "View")
			Case Rights.[New]
				Return User.IsInRole(IsInRoleString + "New")
			Case Rights.Edit
				Return User.IsInRole(IsInRoleString + "Edit")
			Case Rights.Save
				Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
			Case Rights.Delete
				Return User.IsInRole(IsInRoleString + "Delete")
			Case Rights.Print
				Return User.IsInRole(IsInRoleString + "Print")
			Case Rights.Complete
				Return User.IsInRole(IsInRoleString + "Completed")
		End Select

	End Function

	Private Sub CallUpdatePanels()
		upnlWOJobDetails.Update()
		upnlStartDetails.Update()
		upnlMELSnagDetails.Update()
		upnlTitle.Update()
	End Sub

	Private Sub ControlVisibility()

		Try

			chkIsUnderMEL.Enabled = (mnWOJob.IsUnderMEL <> True And mnWO.WOStatusID <> 3)

			If chkIsUnderMEL.Checked = True Then

				pnlMEL.Visible = True
				chkIsInHours.Enabled = IIf(cmbMELCategory.SelectedIndex = 1 And mnWO.WOStatusID <> 3, True, False)
				txtFrequencyInDay.Enabled = IIf(cmbMELCategory.SelectedIndex = 1 And chkIsInHours.Checked = False And mnWO.WOStatusID <> 3, True, False)
				txtFrequencyInHours.Enabled = IIf(cmbMELCategory.SelectedIndex = 1 And chkIsInHours.Checked = True And mnWO.WOStatusID <> 3, True, False)

				If mnWO.WOStatusID = 3 Then

					txtDateOfOccurrence.Enabled = False
					cmbComponent.Enabled = False
					cmbMELCategory.Enabled = False
					txtFrequencyInDay.Enabled = False
					txtFrequencyInHours.Enabled = False
					chkIsInHours.Enabled = False

				Else

					cmbComponent.Enabled = True
					cmbMELCategory.Enabled = True
					txtDateOfOccurrence.Enabled = True
					txtFrequencyInDay.Enabled = True
					txtFrequencyInHours.Enabled = True
					chkIsInHours.Enabled = True

				End If

			Else
				pnlMEL.Visible = False
			End If

			If mnWO.WOStatusID = 3 Then

				chkIsMajor.Enabled = False
				chkIsRepetitive.Enabled = False

			ElseIf mnWOJob.WOJobTypeID = 5 Then

				chkIsMajor.Enabled = True
				chkIsRepetitive.Enabled = True

			End If

			cmbATAChapter.Enabled = mnWO.WOStatusID <> 3
			btnSelectFile.Disabled = (mnWO.WOStatusID = 3)
			txtStartDate.Enabled = mnWO.WOStatusID <> 3
			txtEndDate.Enabled = mnWO.WOStatusID <> 3

			If pnlMEL.Visible Then

				pnlMEL.Enabled = mnWO.WOStatusID <> 3
				cmbComponent.Enabled = mnWO.WOStatusID <> 3
				cmbMELCategory.Enabled = mnWO.WOStatusID <> 3
				txtDateOfOccurrence.Enabled = mnWO.WOStatusID <> 3

			End If


			If mnWOJob.Size > 0 Then

				btnViewAttachment.Visible = True

				If mnWO.WOStatusID = 3 Then
					btnRemoveAttachment.Enabled = False
				Else
					btnRemoveAttachment.Enabled = True
				End If

			Else
				btnViewAttachment.Visible = False
			End If

			txtDueAsOf.Enabled = (mnWOJob.WOJobTypeID <> 2) 'Added By Vikrant On 15-May-2014 For ALL15052014

			If chkShowMEL.Checked Then
				pnlMELCategory.Visible = True
			Else
				pnlMELCategory.Visible = False
			End If

			ControlVisibilityForAttachment() 'Added by Saylee On 27-Dec-2018

			lnkViewIndent.Enabled = (mRequisitionItemsNew.Count > 0)
			Fieldset2.Visible = (((mnWO.StatusID = 2 And mnWO.WOStatusID = 1) Or (mnWO.WOStatusID = 4)) And (mnWO.WOStatusID <> 3) And (AppSettings("ShowCAMOOnlyForNewClients") = "False" Or AppSettings("ShowAMOOnlyForNewClients") = "True"))

			If mRequisitionItemsNew.Count > 0 Then
				lnkViewIndent.Text = "Requisition Item (" + mRequisitionItemsNew.Count.ToString + ")"
			End If

			If lnkCreateRequisition.Enabled Then
				lnkCreateRequisition.ToolTip = "Click to create Requisition of Job Spares Items(s)"
			Else
				lnkCreateRequisition.ToolTip = "Requisition already created against this WO."
			End If

			If Not mnWOJob.IsNew Then

				WOJobDetailsContainer.Tabs(1).Visible = IIf(AppSettings("ShowCAMOOnlyForNewClients") = "False" And AppSettings("ShowAMOOnlyForNewClients") = "False", True, False) 'Task Card link
				WOJobDetailsContainer.Tabs(2).Visible = IIf(AppSettings("ShowCAMOOnlyForNewClients") = "False" And AppSettings("ShowAMOOnlyForNewClients") = "False", True, False) 'Allocate link
				WOJobDetailsContainer.Tabs(3).Visible = IIf(AppSettings("ShowCAMOOnlyForNewClients") = "False" Or AppSettings("ShowAMOOnlyForNewClients") = "True", True, False) 'Spares link

				'Added by Harsh Sugandhi on 15th July 2024

				'TransTypeID 
				'88  => 145 WO
				'89  => CAMO WO
				'102 => Engineering Order
				'117 => CAMO Concession Task

				If (
						CBool(AppSettings("ShowMaintenanceForNewClients")) AndAlso
						CBool(AppSettings("ShowCAMOOnlyForNewClients")) AndAlso
						(
							Session("wfProject_Ajax") = "wfProject_Ajax" OrElse
							(
								mnWO.TransTypeID = 88 Or mnWO.TransTypeID = 89 OrElse
								mnWO.TransTypeID = 102 Or mnWO.TransTypeID = 117
							)
						)
					) Then

					WOJobDetailsContainer.Tabs(4).Visible = False 'Removal / Installation

				Else
					WOJobDetailsContainer.Tabs(4).Visible = True
				End If

				up.Update()

			Else

				WOJobDetailsContainer.Tabs(1).Visible = False 'Task Card link
				WOJobDetailsContainer.Tabs(2).Visible = False 'Allocate link
				WOJobDetailsContainer.Tabs(3).Visible = False 'Spares link
				WOJobDetailsContainer.Tabs(4).Visible = False 'Removal / Installation 
				up.Update()

			End If

			If AppSettings("ClientCode") = "IND" Then

				lblTitle.Text = "W.O. JOB OJS Details"
				lblNRCDet.InnerText = "OJS Details"
				btnSave.ToolTip = "Click to save Job OJS Details"
				btnPrint.ToolTip = "Click to print Job OJS Details"

			Else

				lblTitle.Text = "W.O. JOB NRC Details"
				lblNRCDet.InnerText = "NRC Details"
				btnSave.ToolTip = "Click to save Job NRC Details"
				btnPrint.ToolTip = "Click to print Job NRC Details"

			End If

			If Session("OpenFromWOJobListToCompleteForm") = "True" Then 'Added By Prashant On 11-Jul-2023

				WOJobDetailsContainer.Tabs(1).Visible = False 'Task Card link
				WOJobDetailsContainer.Tabs(2).Visible = False 'Allocate link
				WOJobDetailsContainer.Tabs(4).Visible = False 'Installation / Removal link

				If mnWOJob.IsNew = False Then

					btnSelectFile.Disabled = True
					btnRemoveAttachment.Visible = False

				Else

					btnSelectFile.Disabled = False
					btnRemoveAttachment.Visible = True

				End If

			End If

			phWatchListDetails.Visible = IIf(cmbWOStatusList.SelectedValue = 2 AndAlso
											 AppSettings("ShowMaintenanceForNewClients").ToString.Equals("True", StringComparison.InvariantCultureIgnoreCase) AndAlso
											 Not mnWO.MachineID.Equals(Guid.Empty),
											 True,
											 False)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetObject()

		Try

			If txtStartDate.Text.ToString <> "" Then
				mnWOJob.WOJobStartDate = txtStartDate.Text
			Else
				mnWOJob.WOJobStartDate = DBNull.Value
			End If

			If txtEndDate.Text.ToString <> "" Then
				mnWOJob.WOJobCloseDate = txtEndDate.Text
			Else
				mnWOJob.WOJobCloseDate = DBNull.Value
			End If

			mnWOJob.WOJobEstimatedTime = txtEstimatedTime.Text
			mnWOJob.WOJobActualTime = txtActualTime.Text
			mnWOJob.WOJobStatusID = cmbWOStatusList.SelectedValue
			mnWOJob.IsForBilling = chkIsForBilling.Checked
			mnWOJob.WOJobDescription = txtWOJobDescription.Text
			mnWOJob.WOJobAction = txtWOJobAction.Text
			mnWOJob.WOJobRemark = txtWOJobRemark.Text
			mnWOJob.IsUnderMEL = chkIsUnderMEL.Checked

			If (txtDateOfOccurrence.Text.ToString <> "") Then
				mnWOJob.DateOfOccurrence = txtDateOfOccurrence.Text
			Else
				mnWOJob.DateOfOccurrence = DBNull.Value
			End If

			mnWOJob.ATAChapterID = New Guid(cmbATAChapter.SelectedValue)
			mnWOJob.CompID = New Guid(cmbComponent.SelectedValue)
			mnWOJob.MELCategoryID = cmbMELCategory.SelectedValue
			mnWOJob.IsMajor = chkIsMajor.Checked
			mnWOJob.IsRepetitive = chkIsRepetitive.Checked
			mnWOJob.IsHours = chkIsInHours.Checked
			mnWOJob.FrequencyInDays = Val(txtFrequencyInDay.Text)
			mnWOJob.FrequencyInHours = txtFrequencyInHours.Text.Trim

			'Added By Vikrant On 06-Nov-2012 For ALL06112012-1
			mnWOJob.Zone = Trim(txtZone.Text)
			mnWOJob.AREA = Trim(txtArea.Text)
			mnWOJob.WorkPACKREF = Trim(txtWorkPackRef.Text)
			mnWOJob.Publication = Trim(txtPublication.Text)
			mnWOJob.Skill = Trim(txtSkill.Text)
			mnWOJob.SkillID = Val(cmbSkillcode.SelectedValue.ToString)

			If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then

				If (cmbSkillcode.SelectedIndex > 0) Then
					mnWOJob.Skill = cmbSkillcode.SelectedItem.ToString
				End If

			End If

			mnWOJob.Panels = Trim(txtPanels.Text)
			mnWOJob.InspCode = Trim(txtInspCode.Text)
			mnWOJob.IsRII = chkIsRII.Checked      'Added By Saylee on 18-Jan-2012 for BA17012013
			mnWOJob.AMPRevNo = txtAMPRevNo.Text  'Added By Saylee on 03-Apr-2013 for BA03032013

			If txtRevDate.Text.ToString <> "" Then             'Added By Saylee on 03-Apr-2013 for BA03032013
				mnWOJob.AMPRevDate = txtRevDate.Text
			Else
				mnWOJob.AMPRevDate = DBNull.Value
			End If

			mnWOJob.TaskSourceRef = Trim(txtTaskSourceRef.Text) 'Added By Vikrant On 23-May-2013 For BA23052013-1	
			mnWOJob.TaskCardNo = txtTaskNo.Text.Trim

			'Added By Vikrant On 15-May-2014 For ALL15052014
			If mnWOJob.WOJobTypeID <> 2 Then
				mnWOJob.DueAsOf = Trim(txtDueAsOf.Text)
			End If
			'End

			'Added By Saylee On 27-Dec-2018
			If mFileJobAttach IsNot Nothing Then

				If mFileJobAttach.Size > 0 Then
					mnWOJob.IsAttachmentAdded = True
				Else
					mnWOJob.IsAttachmentAdded = False
				End If

			End If

			If cmbWOStatusList.SelectedIndex = 1 Then 'Added By Prashant On 11-Jul-2023

				Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
				mnWOJob.CompletedBy = User.Identity.Name
				mnWOJob.CompletedByEmployeeID = mUser.EmployeeID
				mnWOJob.CompletedByEmployeeName = mUser.EmployeeName

			End If

			mnWOJob.AddToWatchList = IIf(cmbWOStatusList.SelectedIndex = 1,
										 chkAddToWatchList.Checked,
										 False)

			mnWOJob.WatchListInstructions = IIf(cmbWOStatusList.SelectedIndex = 1,
												Trim(txtWatchListInstructions.Text),
												String.Empty)

			Session("mnWO") = mnWO
			Session("nWOJobNRC") = mnWOJob

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function CustomAndObjectValidations() As Boolean

		Dim strMSG As String = ""

		If Not mnWO.IsValid Then
			For i As Integer = 0 To mnWO.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWO.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If

		If Not mnWOJob.IsValid Then
			For i As Integer = 0 To mnWOJob.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWOJob.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If

		If cmbWOStatusList.SelectedIndex = 1 Then 'Complete

			lblStarStartDate.Visible = True
			lblStarEndDate.Visible = True

			If Session("OpenFromWOJobListToCompleteForm") = "True" Then 'Added By Prashant On 11-Jul-2023
				lblStarAction.Visible = True
				If txtWOJobAction.Text.Trim = "" Then
					strMSG = strMSG + "Action required" & "<BR>"
				End If
			End If

			If txtStartDate.Text = "" And txtEndDate.Text = "" Then
				strMSG = strMSG + "Actual Start Date required" & "<BR>" & "Actual End Date required"
			ElseIf txtStartDate.Text = "" And txtEndDate.Text <> "" Then
				strMSG = strMSG + "Actual Start Date required"
			ElseIf txtStartDate.Text <> "" And txtEndDate.Text = "" Then
				strMSG = strMSG + "Actual End Date required"
			End If

		Else
			lblStarStartDate.Visible = False
			lblStarEndDate.Visible = False
			lblStarAction.Visible = False
		End If

		If Len(txtWOJobDescription.Text) > 1000 Then
			strMSG = strMSG + "Job Description must not be greater than 1000 Char."
		End If

		If Len(txtTaskSourceRef.Text) > 500 Then
			strMSG = strMSG + "Task Source Ref. must not be greater than 500 Char."
		End If

		Try
			Dim ValueiInDecimal As String
			If txtEstimatedTime.Text <> "" Then ValueiInDecimal = nWOPeriod.ConvertStringToDecimal(1, 1, txtEstimatedTime.Text, False)
		Catch ex As Exception
			strMSG = strMSG + ex.Message
		End Try

		If chkIsUnderMEL.Checked = True Then

			If txtDateOfOccurrence.Text = "" Then
				strMSG = strMSG + "Date Of Occurrence required as it is MEL"
			ElseIf CDate(txtDateOfOccurrence.Text) > CDate(CType(mnWO.WODate.ToString, String)) And (Not (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND")) Then
				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
					strMSG = strMSG + "Date Of Occurrence should be less than E.O. Date"
				Else
					strMSG = strMSG + "Date Of Occurrence should be less than Work Order Date"
				End If
			ElseIf CDate(CDate(txtDateOfOccurrence.Text) + " " + "23:59") > CDate(CType(mnWO.WODate.ToString, String)) And (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND") Then
				strMSG = strMSG + "Date Of Occurrence should be less than Work Order Date"
			End If

		End If

		If Len(txtWOJobRemark.Text) > 500 Then
			strMSG = strMSG + "Job Remark must not be greater than 500 Char."
		End If

		If IsDate(CType(mnWO.WODate.ToString, String)) Then

			If txtStartDate.Text <> "" Then

				If (CDate(txtStartDate.Text) < CDate(CType(mnWO.WODate.ToString, String))) And (Not (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND")) Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						strMSG = strMSG + "Actual Start Date should be greater than E.O. Date."
					Else
						strMSG = strMSG + "Actual Start Date should be greater than Work Order Date."
					End If

				ElseIf (CDate(CDate(txtStartDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String))) And (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND") Then
					strMSG = strMSG + "Actual Start Date should be greater than Work Order Date."
				ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

					If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
						strMSG = strMSG + "Actual Start Date cannot be greater than Actual End Date."
					End If

				ElseIf txtStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then 'Added by Saylee

					If CDate(txtStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
							strMSG = strMSG + "Actual Start Date should be equal to or greater than E.O. Start Date."
						Else
							strMSG = strMSG + "Actual Start Date should be equal to or greater than Work Order Start Date."
						End If

					End If

				End If

			End If

		ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

			If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
				strMSG = strMSG + "Actual Start Date cannot be greater than Actual End Date." 'mWO.GetBrokenRulesString
			End If

		ElseIf txtEndDate.Text <> "" And IsDate(CType(mnWO.WODate.ToString, String)) Then

			If CDate(txtEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) And (Not (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND")) Then

				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
					strMSG = strMSG + "Actual End Date should be greater than E.O. Date."
				Else
					strMSG = strMSG + "Actual End Date should be greater than Work Order Date."
				End If

			ElseIf CDate(CDate(txtEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) And (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND") Then
				strMSG = strMSG + "Actual End Date should be greater than Work Order Date."
			End If

		End If

		If txtEndDate.Text <> "" Then

			If IsDate(CType(mnWO.WODate.ToString, String)) Then

				If CDate(txtEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) And (Not (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND")) Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						strMSG = strMSG + "Actual End Date should be greater than E.O. Date."
					Else
						strMSG = strMSG + "Actual End Date should be greater than Work Order Date."
					End If

				ElseIf CDate(CDate(txtEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) And (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND") Then
					strMSG = strMSG + "Actual End Date should be greater than Work Order Date."
				ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

					If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
						strMSG = strMSG + "Actual End Date cannot be less than Actual Start Date."
					End If

				End If

			ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

				If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
					strMSG = strMSG + "Actual End Date cannot be less than Actual Start Date."
				End If

			End If

		End If

		'Added By Vikrant On 15-May-2014 For ALL15052014
		If Len(Trim(txtDueAsOf.Text)) > 50 Then
			strMSG = strMSG + "Due As Of must not be greater than 50 Char."
		End If

		If strMSG.Trim <> "" Then
			cvControlValidator.ErrorMessage = strMSG
			cvControlValidator.IsValid = False
			Return False
		End If

		Return True

	End Function

	Public Sub CustomValidations(s As Object, e As ServerValidateEventArgs)

		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)

		If custValidator.ControlToValidate = "cmbWOStatusList" Then

			If cmbWOStatusList.SelectedIndex = 1 Then

				lblStarStartDate.Visible = True
				lblStarEndDate.Visible = True

				If Session("OpenFromWOJobListToCompleteForm") = "True" Then 'Added By Prashant On 11-Jul-2023

					lblStarAction.Visible = True
					If txtWOJobAction.Text.Trim = "" Then
						custValidator.ErrorMessage = "Action required"
						e.IsValid = False
					End If

				End If

				If txtStartDate.Text = "" And txtEndDate.Text = "" Then
					custValidator.ErrorMessage = "Actual Start Date required" & "<BR>" & "Actual End Date required"
					e.IsValid = False
				ElseIf txtStartDate.Text = "" And txtEndDate.Text <> "" Then
					custValidator.ErrorMessage = "Actual Start Date required"
					e.IsValid = False
				ElseIf txtStartDate.Text <> "" And txtEndDate.Text = "" Then
					custValidator.ErrorMessage = "Actual End Date required"
					e.IsValid = False
				End If

			Else
				lblStarStartDate.Visible = False
				lblStarEndDate.Visible = False
				lblStarAction.Visible = False
			End If

			'--Added By Utkarsh On 17-Jan-2011
		ElseIf custValidator.ControlToValidate = "txtWOJobDescription" Then

			If Len(txtWOJobDescription.Text) > 1000 Then
				custValidator.ErrorMessage = "Job Description must not be greater than 1000 Char."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

			'Added By Vikrant On 23-May-2013 For BA23052013-1	
		ElseIf custValidator.ControlToValidate = "txtTaskSourceRef" Then

			If Len(txtTaskSourceRef.Text) > 500 Then
				custValidator.ErrorMessage = "Task Source Ref. must not be greater than 500 Char."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

		ElseIf custValidator.ControlToValidate = "txtEstimatedTime" Then

			Try
				Dim ValueiInDecimal As String
				ValueiInDecimal = nWOPeriod.ConvertStringToDecimal(1, 1, txtEstimatedTime.Text, False)
			Catch ex As Exception
				custValidator.ErrorMessage = ex.Message
				e.IsValid = False
			End Try

		ElseIf custValidator.ControlToValidate = "txtDateOfOccurrence" Then

			If chkIsUnderMEL.Checked = True Then

				If txtDateOfOccurrence.Text = "" Then
					custValidator.ErrorMessage = "Date Of Occurrence required as it is MEL"
					e.IsValid = False
				ElseIf CDate(txtDateOfOccurrence.Text) > CDate(CType(mnWO.WODate.ToString, String)) And (Not (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND")) Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						custValidator.ErrorMessage = "Date Of Occurrence should be less than E.O. Date"
						e.IsValid = False
					Else
						custValidator.ErrorMessage = "Date Of Occurrence should be less than Work Order Date"
						e.IsValid = False
					End If

				ElseIf CDate(CDate(txtDateOfOccurrence.Text) + " " + "23:59") > CDate(CType(mnWO.WODate.ToString, String)) And (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND") Then
					custValidator.ErrorMessage = "Date Of Occurrence should be less than Work Order Date"
					e.IsValid = False
				End If

			End If

		ElseIf custValidator.ControlToValidate = "txtWOJobRemark" Then

			If Len(txtWOJobRemark.Text) > 500 Then
				custValidator.ErrorMessage = "Job Remark must not be greater than 500 Char."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

		ElseIf custValidator.ControlToValidate = "txtStartDate" Then

			If IsDate(CType(mnWO.WODate.ToString, String)) Then

				If txtStartDate.Text <> "" Then

					If CDate(txtStartDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) And (Not (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND")) Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
							custValidator.ErrorMessage = "Actual Start Date should be greater than E.O. Date."
							e.IsValid = False
						Else
							custValidator.ErrorMessage = "Actual Start Date should be greater than Work Order Date."
							e.IsValid = False
						End If

					ElseIf CDate(CDate(txtStartDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) And (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND") Then
						custValidator.ErrorMessage = "Actual Start Date should be greater than Work Order Date."
						e.IsValid = False
					ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

						If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
							custValidator.ErrorMessage = "Actual Start Date cannot be greater than Actual End Date."
							e.IsValid = False
						Else
							e.IsValid = True
						End If

					ElseIf txtStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then 'Added by Saylee

						If CDate(txtStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then

							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								custValidator.ErrorMessage = "Actual Start Date should be equal to or greater than E.O. Start Date."
								e.IsValid = False
							Else
								custValidator.ErrorMessage = "Actual Start Date should be equal to or greater than Work Order Start Date."
								e.IsValid = False
							End If

						End If

					End If

				End If

			ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

				If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
					custValidator.ErrorMessage = "Actual Start Date cannot be greater than Actual End Date." 'mWO.GetBrokenRulesString
					e.IsValid = False
				Else
					e.IsValid = True
				End If

			ElseIf txtEndDate.Text <> "" And IsDate(CType(mnWO.WODate.ToString, String)) Then

				If CDate(txtEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						custValidator.ErrorMessage = "Actual End Date should be greater than E.O. Date."
						e.IsValid = False
					Else
						custValidator.ErrorMessage = "Actual End Date should be greater than Work Order Date."
						e.IsValid = False
					End If

				Else
					e.IsValid = True
				End If

			End If

		ElseIf custValidator.ControlToValidate = "txtEndDate" Then

			If txtEndDate.Text <> "" Then

				If IsDate(CType(mnWO.WODate.ToString, String)) Then

					If CDate(txtEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) And (Not (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND")) Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
							custValidator.ErrorMessage = "Actual End Date should be greater than E.O. Date."
							e.IsValid = False
						Else
							custValidator.ErrorMessage = "Actual End Date should be greater than Work Order Date."
							e.IsValid = False
						End If

					ElseIf CDate(CDate(txtEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) And (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND") Then
						custValidator.ErrorMessage = "Actual End Date should be greater than Work Order Date."
						e.IsValid = False
					ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

						If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
							custValidator.ErrorMessage = "Actual End Date cannot be less than Actual Start Date."
							e.IsValid = False
						Else
							e.IsValid = True
						End If

					End If

				ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

					If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
						custValidator.ErrorMessage = "Actual End Date cannot be less than Actual Start Date."
						e.IsValid = False
					Else
						e.IsValid = True
					End If

				End If

			End If

			'Added By Vikrant On 15-May-2014 For ALL15052014
		ElseIf custValidator.ControlToValidate = "txtDueAsOf" Then

			If Len(Trim(txtDueAsOf.Text)) > 50 Then
				custValidator.ErrorMessage = "Due As Of must not be greater than 50 Char."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

		End If

	End Sub

	Private Sub AttachMyFile()

		Try

			mnWOJob.AttachFileName = CType(Session("FileUpload.FileContent"), Byte())
			mnWOJob.Size = Session("FileUpload.FileSize")
			mnWOJob.FileExtension = Session("FileUpload.FileExtension")

			Session.Remove("FileUpload.FileSize")
			Session.Remove("FileUpload.FileContent")
			Session.Remove("FileUpload.FileExtension")

			btnRemoveAttachment.Enabled = True
			upnlAttach.Update()
			Session("mnWO") = mnWO

		Catch ex As Exception
			MSGBoxCtrl.Show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
		End Try

		If mnWOJob.Size > 0 Then

			btnViewAttachment.Visible = True

			If mnWO.WOStatusID = 3 Then
				btnRemoveAttachment.Enabled = False
			Else
				btnRemoveAttachment.Enabled = True
			End If

		Else
			btnViewAttachment.Visible = False
		End If

	End Sub

	Private Sub WOJobTasksDelete(Index As Int32)

		MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem,
						MSGBox.Message_Text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"WOJobTasksDelete")

	End Sub

	Private Sub WOJobDesignationAllocations(Index As Int32)
		mnWOJob.WOJobDesignationAllocations.CurrentIndex = Index
		MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem, MSGBox.Message_Text.RemoveItem, "", MsgBoxStyle.YesNo, "WOJobDesignationAllocationsDelete")
	End Sub

	Private Sub WOJobSpares(Index As Int32)
		mnWOJob.WOJobSpares.CurrentIndex = Index
		MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem, MSGBox.Message_Text.RemoveItem, "", MsgBoxStyle.YesNo, "WOJobSparesDelete")
	End Sub

	Private Sub WOJobComps(Index As Int32)
		mnWOJob.WOJobComps.CurrentIndex = Index
		MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem, MSGBox.Message_Text.RemoveItem, "", MsgBoxStyle.YesNo, "WOJobCompsDelete")
	End Sub

	'Modified by Harsh for FLYPAL-2221
	Private Sub MessageBoxResult()

		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then

			Select Case Result1
				Case MsgBoxResult.Yes

					If MSGBoxCtrl.Sender = "Close" Or
					   MSGBoxCtrl.Sender = "Save" Then  '' Close confirmation

						Session("sender") = ""

						If Not CustomAndObjectValidations() Then upnlValidationSummary.Update() : Exit Sub

						If mnWOJob.IsValid = True Then

							Session.Remove("IsValid")

							If Save() Then

								If Not CustomAndObjectValidations() Then upnlValidationSummary.Update() : Exit Sub
								If Not mnWO.IsValid Or Not mnWOJob.IsValid Then upnlValidationSummary.Update() : Exit Sub

								mnWOJob.Save()

								If mFileJobAttach IsNot Nothing Then SaveAttachment()

								mWODetail = mnWO.WONumber +
											" Dated : " + mnWO.WODateFormatted +
											" Created By : " + mnWO.WOBy

								If mnWO.StatusID = 2 Then

									MarkLog(Action.Authorize,
											"Work Order",
											mWODetail,
											ErrorType.NoError,
											mnWO.ID,
											EventLogID)

								ElseIf mnWO.StatusID = 3 Then

									MarkLog(Action.Amend,
											"Work Order",
											mWODetail,
											ErrorType.NoError,
											mnWO.ID,
											EventLogID)

								ElseIf mnWO.StatusID = 4 Then

									MarkLog(Action.Cancel,
											"Work Order",
											mWODetail,
											ErrorType.NoError,
											mnWO.ID,
											EventLogID)

								Else

									MarkLog(Action.Save,
											"Work Order",
											mWODetail,
											ErrorType.NoError,
											mnWO.ID,
											EventLogID)

								End If

								mnWOJob.MarkClean()
								mnWO.MarkClean()
								Session("mnWO") = mnWO
								ControlVisibility()

								'Added By Saylee On 27-Dec-2018 
								Session.Remove("mFileAttach")
								Session.Remove("IsAttachmentDeleted")
								Session.Remove("ActiveJobNRCDetailsTabIndex")
								'End

								Dim OpenAs As String = Request.QueryString("Type")

								If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then

									'Sending Email on completing NRC while saving details.
									If cmbWOStatusList.SelectedIndex = 1 AndAlso
									   chkAddToWatchList.Checked Then

										SendMail()

									End If

									ScriptManager.RegisterStartupScript(Me,
																		[GetType],
																		"CallParentCallback",
																		"CallParentCallback();",
																		True)

									Exit Sub

								Else 'Added By Prashant On 11-Jul-2023

									'Sending Email on completing NRC while saving details.
									If cmbWOStatusList.SelectedIndex = 1 AndAlso
									   chkAddToWatchList.Checked Then

										SendMail()

									End If

									Response.Redirect(Request.QueryString("BackPage1") &
													  "?BackPage=" &
													  Request.QueryString("BackPage"))
									Exit Sub 'End

								End If

							End If

						Else

							Session.Remove("IsValid")
							upnlValidationSummary.Update()

							ControlVisibility()
							DataFieldBind()
							CallUpdatePanels()

						End If

					End If

				Case MsgBoxResult.No

					If MSGBoxCtrl.Sender = "Close" Then

						Session("mnWO") = mnWO
						Session.Remove("IsValid")
						Session("Sender") = ""
						Session.Remove("Edit")
						Session.Remove("mnWOClone")
						Session.Remove("ActiveJobNRCDetailsTabIndex")

						Dim openAs As String = Request.QueryString("Type")

						If openAs IsNot Nothing AndAlso openAs = "pup" Then
							ScriptManager.RegisterStartupScript(Me, [GetType], "CallParentCallback", "CallParentCallback();", True)
							Exit Sub
						Else 'Added By Prashant On 11-Jul-2023
							Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
							Exit Sub 'End Of Added By Prashant On 11-Jul-2023
						End If

						Response.Redirect(Request.QueryString("CPage1") & "?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))

					ElseIf MSGBoxCtrl.Sender = "Save" Then

						If Session("Edit") = True Then
							mnWO = Session("mnWOClone")
						End If

						Session("mnWO") = mnWO
						Session.Remove("IsValid")
						Session("Sender") = ""
						Session.Remove("Edit")
						Session.Remove("mnWOClone")

						ControlVisibility()
						DataFieldBind()

					Else

						Session("sender") = ""

						ControlVisibility()
						DataFieldBind()

					End If

			End Select

		ElseIf Result1 = -1 Then

			Session("sender") = ""

			ControlVisibility()
			DataFieldBind()
			CallUpdatePanels()

		ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then
			Session("sender") = ""
			DataFieldBind()
		End If

	End Sub

	Private Sub AddAttributes()

		txtEstimatedTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtEstimatedTime').value,event)")
		txtActualTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtActualTime').value,event)")
		txtFrequencyInDay.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInDay').value,event)")
		txtFrequencyInHours.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtFrequencyInHours').value,event)")

	End Sub

	Private Function Save() As Boolean

		SetObject()
		Session("mnWO") = mnWO
		Session.Remove("Edit")
		Return True

	End Function

	Private Sub ClearControl() '---Added By Utkarsh On 18-Jan-2011

		cmbMELCategory.SelectedIndex = 0
		chkIsMajor.Checked = False
		chkIsRepetitive.Checked = False
		cmbComponent.SelectedIndex = 0
		cmbATAChapter.SelectedIndex = 0
		txtDateOfOccurrence.Text = ""
		chkIsInHours.Checked = False
		txtFrequencyInDay.Text = "0"
		txtFrequencyInHours.Text = ""

	End Sub

	'Added By Saylee On 27-Dec-2018
	Private Sub ControlVisibilityForAttachment()

		If mFileJobAttach IsNot Nothing Then

			If mFileJobAttach.Size > 0 Then

				btnViewAttachment.Visible = True
				btnRemoveAttachment.Enabled = True
				btnSelectFile.Disabled = True

			Else

				btnViewAttachment.Visible = False
				btnSelectFile.Disabled = False

			End If

		End If

	End Sub

	Private Sub GetAttachment()
		If mnWOJob.IsAttachmentAdded And mFileJobAttach Is Nothing Then
			mFileJobAttach = FileAttach.GetAttachment(mnWOJob.ID) 'Sort = 2 : Removal
			Session("mFileAttach") = mFileJobAttach
		End If
	End Sub

	Private Sub SaveAttachment()

		mFileJobAttach.ReferenceID = mnWOJob.ID

		If mFileJobAttach.Size > 0 Then

			Try

				If (Not mnWOJob.IsNew) And IsAttachmentDeleted Then
					FileAttach.DeleteAllAttachmentChilds(mnWOJob.ID)
				End If
				IsAttachmentDeleted = False
				Session("IsAttachmentDeleted") = IsAttachmentDeleted
				mFileJobAttach.Save()

			Catch ex As Exception
				ScriptManager.RegisterClientScriptBlock(Me, [GetType], "", MessageBox.Show(ex.InnerException.ToString, False), False)
			End Try

		Else

			If (Not mnWOJob.IsNew) And IsAttachmentDeleted Then
				FileAttach.DeleteAttachment(mFileJobAttach.ID, mnWOJob.ID)
			End If
			IsAttachmentDeleted = False
			Session("IsAttachmentDeleted") = IsAttachmentDeleted

		End If

	End Sub

	Private Sub ViewImage()

		Dim No As New Random
		Dim StrName As String = "abc" & No.Next.ToString

		GetAttachment()

		If mFileJobAttach.Size > 0 Then

			Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileJobAttach.Extension
			Dim fs As FileStream

			If File.Exists(AppSettings("DOCPath")) = False Then

				'Delete File if exist
				File.Delete(AppSettings("DOCPath") & StrName & mFileJobAttach.Extension)
				' Create the file.
				fs = File.Create(path)
				'' Add some information to the file.
				fs.Write(mFileJobAttach.ImageFile, 0, mFileJobAttach.ImageFile.Length)
				fs.Close()
				Session("DOCPath") = path
				ScriptManager.RegisterStartupScript(Me, [GetType], "DisplayAttachment", "DisplayAttachment();", True)

			End If

		End If

	End Sub
	'*********************************************************************

	'Added by Harsh Sugandhi on 25th Feb 2025 FLYPAL-2221 Provision to add JOB NRC to WatchList.
	Public Sub SetEmailRelatedDetails()

		Try

			Session("UserEmailID") = mModuleList.Item("Job NRC WatchList").SendToMailID
			Session("UserCcEmailID") = mModuleList.Item("Job NRC WatchList").SendCCMailID
			Session("MailsRequire") = mModuleList.Item("Job NRC WatchList").MailsRequire
			Session("SmtpHost") = mModuleList.Item("Job NRC WatchList").SmtpHost
			Session("SmtpPort") = mModuleList.Item("Job NRC WatchList").SmtpPort
			Session("SmtpUser") = mModuleList.Item("Job NRC WatchList").SmtpUser
			Session("SmtpPassword") = mModuleList.Item("Job NRC WatchList").SmtpPassword

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub SendMail()

		Dim EmailBody As String

		Try

			SetEmailRelatedDetails()

			EmailBody += $"<html> <head> </head> <body> 
                         <p><font face=""Calibri"">
                            Following completed Non-Routine Card (NRC) has Watchlist Instruction and requires Attention. 
                         </font></p></br>"
			EmailBody += "<p><font face=""Calibri"">"
			EmailBody += $"<b> Description : </b> {mnWOJob.WOJobDescription} "
			EmailBody += "</font></p>"
			EmailBody += "<p><font face=""Calibri"">"
			EmailBody += $"<b> Watchlist Instructions : </b>  {mnWOJob.WatchListInstructions} "
			EmailBody += "</font></p>"
			EmailBody += "</body></html>"
			EmailBody += ("</br><p><font face=""Calibri"">")
			EmailBody += ("<font face=""Calibri"">
                                    Kindly Login into FlyPal® for detailed Information. </font> ")
			EmailBody += ("</body></html>")

			SendMailFile.SendMailFile(UserName:=User.Identity.Name,
									  Subject:="Non-Routine Card (NRC) has Watchlist Instruction",
									  Info:=EmailBody,
									  ToMailID:=Session("UserEmailID"),
									  CCMailID:=Session("UserCcEmailID"),
									  SmtpHost:=Session("SmtpHost"),
									  SmtpPort:=Session("SmtpPort"),
									  SmtpUser:=Session("SmtpUser"),
									  SmtpPassword:=Session("SmtpPassword"))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	'End

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			mnWOJobStatusList = nWOJobStatusList.GetWOStatusList()
			cmbWOStatusList.DataSource = mnWOJobStatusList
			Session("mnWOJobStatusList") = mnWOJobStatusList
			mATAList = ATAList.GetATAList("", "(SELECT)")
			cmbATAChapter.DataSource = mATAList
			Session("mATAList") = mATAList
			mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateOfOccurrence.Text, , "(SELECT)")
			cmbComponent.DataSource = mMELSnagPartList
			Session("mMELSnagPartList") = mMELSnagPartList
			cmbMELCategory.DataSource = MELCategoryList.GetMELCategoryList("(SELECT)")

			If mnWOJob IsNot Nothing Then

				txtStartDate.Text = IIf(mnWOJob.WOJobStartDate.ToString = "", "", mnWOJob.WOJobStartDateFormatted)
				txtEndDate.Text = IIf(mnWOJob.WOJobCloseDate.ToString = "", "", mnWOJob.WOJobCloseDateFormatted) 'mnWOJob.WOJobCloseDate
				txtDateOfOccurrence.Text = IIf(mnWOJob.DateOfOccurrence.ToString = "", "", mnWOJob.DateOfOccurrenceFormatted) 'mnWOJob.DateOfOccurence '---Added By Utkarsh On 18-Jan-2011
				txtRevDate.Text = IIf(mnWOJob.AMPRevDate.ToString = "", "", mnWOJob.AMPRevDateFormatted) 'mnWOJob.AMPRevDate

			End If

			mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForWO(WOID:=mnWO.ID,
																				IsForWO:=True,
																				TransactionDate:=mnWO.WODateFormatted.ToString)

			Session("mRequisitionItemsNew") = mRequisitionItemsNew
			mMPDSkillList = MPDSkillList.GetSkillList(True)
			cmbSkillcode.DataSource = mMPDSkillList

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddMultipleTaskCards()

		Dim tmpTaskCard As TaskCard
		Dim mTaskCardList As TaskCardList = Session("mSelectTaskCardList")

		For Each tmpTaskCard In mTaskCardList

			If tmpTaskCard.IsSelect Then

				If Not mnWOJob.WOJobTasks.Contains(tmpTaskCard.ID, "") Then

					Dim mTaskCard As TaskCard
					mTaskCard = TaskCard.GetTaskCard(tmpTaskCard.ID)
					mnWOJob.WOJobTasks.Add(mnWOJob.ID, mTaskCard.ID.ToString)
					With mnWOJob.WOJobTasks.CurrentItem

						mnWOJob.WOJobTasks.CurrentItem.SrNo = mnWOJob.WOJobTasks.CurrentIndex + 1
						mnWOJob.WOJobTasks.CurrentItem.TaskCardNo = mTaskCard.TaskCardNo
						mnWOJob.WOJobTasks.CurrentItem.EstimatedHours = mTaskCard.EstimatedHours
						mnWOJob.WOJobTasks.CurrentItem.Reference = mTaskCard.Reference
						mnWOJob.WOJobTasks.CurrentItem.Equipment = mTaskCard.Equipment
						mnWOJob.WOJobTasks.CurrentItem.Material = mTaskCard.Material
						mnWOJob.WOJobTasks.CurrentItem.TaskDescription = mTaskCard.TaskDesc
						mnWOJob.WOJobTasks.CurrentItem.RevNo = mTaskCard.RevNo
						mnWOJob.WOJobTasks.CurrentItem.RevDate = mTaskCard.RevDate
						mnWOJob.WOJobTasks.CurrentItem.IssueDate = mTaskCard.IssueDate
						mnWOJob.WOJobTasks.CurrentItem.checks = mTaskCard.Check
						mnWOJob.WOJobTasks.CurrentItem.RelatedTaskCardsNo = mTaskCard.RelatedTaskCardsNo

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

				End If

			Else

				If mnWO.WOJobs.CurrentItem.WOJobTasks.Contains(tmpTaskCard.ID, "") Then
					mnWO.WOJobs.CurrentItem.WOJobTasks.Remove(tmpTaskCard.ID, "")
				End If

			End If

		Next

		Session("TaskCards") = "False"
		Session.Remove("mTaskCard")
		Session.Remove("mTaskCardList")

	End Sub

#End Region

#Region " Events "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		AddAttributes()

		If Not Page.IsPostBack Then

			If txtWOJobDescription.Enabled = True Then
				txtWOJobDescription.Focus()
			End If

			DataFieldBind()
			GetAttachment()

			If CType(Session("ActiveJobNRCDetailsTabIndex"), Integer) > 0 Then
				If Session("ActiveJobNRCDetailsTabIndex") IsNot Nothing Then WOJobDetailsContainer.ActiveTabIndex = CType(Session("ActiveJobNRCDetailsTabIndex"), Integer) : Session.Remove("ActiveJobNRCDetailsTabIndex")
				Call WOJobDetails_TabChanged(Nothing, Nothing)
			Else
				WOJobDetailsContainer.ActiveTabIndex = 0
			End If

		End If

		ControlVisibility()

	End Sub

	Private Sub SaveJobNRCDetails(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
			   (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Or
			   (Not IsInRole(Rights.Complete) And
				Not mnWO.IsNew And
				cmbWOStatusList.SelectedValue = 2 And
				Session("MiddleFrame") <> "wfnWOJobListToComplete_AJAX.aspx") Then

				SetSession()

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")

				Exit Sub

			End If

			SetObject()

			If Not IsValid Then upnlValidationSummary.Update() : Exit Sub

			If mnWOJob.IsDirty Then

				If mnWOJob.WOJobStatusID <> 1 Then

					Session("IsValid") = "True"
					MSGBoxCtrl.Show("Save Confirmation!",
									"You are about to Save JOB NRC Details.<Br>Do you want to Proceed ?",
									"",
									MsgBoxStyle.YesNo,
									"Save")

					Exit Sub

				Else

					Try

						If Not CustomAndObjectValidations() Then upnlValidationSummary.Update() : Exit Sub
						If Not mnWOJob.IsValid Then upnlValidationSummary.Update() : Exit Sub

						mnWOJob.Save()

						If mFileJobAttach IsNot Nothing Then SaveAttachment()

						mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy

						If mnWO.StatusID = 2 Then

							MarkLog(Action.Authorize,
									"Work Order",
									mWODetail,
									ErrorType.NoError,
									mnWO.ID,
									EventLogID)

						ElseIf mnWO.StatusID = 3 Then

							MarkLog(Action.Amend,
									"Work Order",
									mWODetail,
									ErrorType.NoError,
									mnWO.ID,
									EventLogID)

						ElseIf mnWO.StatusID = 4 Then

							MarkLog(Action.Cancel,
									"Work Order",
									mWODetail,
									ErrorType.NoError,
									mnWO.ID,
									EventLogID)

						Else

							MarkLog(Action.Save,
									"Work Order",
									mWODetail,
									ErrorType.NoError,
									mnWO.ID,
									EventLogID)

						End If

						mnWO.MarkClean()
						Session("mnWO") = mnWO

						If cmbMELCategory.SelectedIndex = 1 Then
							chkIsInHours.Enabled = True
							txtFrequencyInDay.Enabled = True
						Else

							If chkIsInHours.Checked = True Then
								chkIsInHours.Checked = False
							End If

							chkIsInHours.Enabled = False
							txtFrequencyInDay.Enabled = False

						End If

						ControlVisibility()
						upnlButtons.DataBind()

						MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
										MSGBox.Message_Text.SavedSuccessFully,
										"",
										MsgBoxStyle.OkOnly,
										"")

					Catch ex As SqlException

						If ex.Number = 8114 Or ex.Number = 8115 Then

							MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow,
											MSGBox.Message_Text.NumericOverFlow,
											"",
											MsgBoxStyle.OkOnly,
											"")

						ElseIf ex.Number = 8145 Then

							MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
											MSGBox.Message_Text.ProcedureError,
											ex.Procedure,
											MsgBoxStyle.OkOnly,
											"")

						ElseIf ex.Number = 2627 Then

							MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
											MSGBox.Message_Text.Duplicate,
											ex.Procedure,
											MsgBoxStyle.OkOnly,
											"")

						ElseIf ex.Number = 547 Then

							If InStr(ex.Message, "FK", CompareMethod.Text) Then
							ElseIf ex.Number = 8144 Then
								MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
												MSGBox.Message_Text.ReferenceDelete,
												ex.Procedure + "," + ex.Message,
												MsgBoxStyle.OkOnly,
												"")

							End If

						End If

					End Try

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ShowAttachment(sender As Object, e As ImageClickEventArgs) Handles btnViewAttachment.Click
		ViewImage()
	End Sub

	Private Sub ShowMEL_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowMEL.CheckedChanged

		cmbComponent.Items.Clear()

		If chkShowMEL.Checked = True Then

			pnlMELCategory.Visible = True
			mMELSnagPartList = Nothing
			mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateOfOccurrence.Text, mnWO.MachineID.ToString, "(SELECT)")
			cmbComponent.DataSource = mMELSnagPartList
			Session("mMELSnagPartList") = mMELSnagPartList
			If Not mMELSnagPartList.Contains("", mnWOJob.CompID) Then mnWOJob.CompID = Guid.Empty
			cmbComponent.DataBind()
			cmbMELCategory.Enabled = False

		Else

			pnlMELCategory.Visible = False
			mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateOfOccurrence.Text, , "(SELECT)")
			cmbComponent.DataSource = mMELSnagPartList
			Session("mMELSnagPartList") = mMELSnagPartList
			cmbComponent.DataBind()
			cmbMELCategory.Enabled = True

		End If

		txtFrequencyInDay.Enabled = False
		chkIsInHours.Enabled = False

		If chkIsInHours.Checked = True Then
			chkIsInHours.Checked = False
		End If

		txtFrequencyInHours.Enabled = False
		txtFrequencyInDay.Text = "0"
		txtFrequencyInHours.Text = ""
		cmbMELCategory.SelectedIndex = 0
		Session("ShowMEL") = chkShowMEL.Checked

	End Sub

	Private Sub RemoveAttachment(sender As Object, e As ImageClickEventArgs) Handles btnRemoveAttachment.Click

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte

		GetAttachment()
		mFileJobAttach.ImageFile = file1
		mFileJobAttach.Size = 0
		mnWOJob.IsAttachmentAdded = False
		IsAttachmentDeleted = True
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
		Session("mFileAttach") = mFileJobAttach
		btnViewAttachment.Visible = False
		btnRemoveAttachment.Enabled = False
		Session("nWOJobNRC") = mnWOJob
		ControlVisibilityForAttachment()
		upnlAttach.Update()

	End Sub

	Private Sub WOStatusChanged(sender As Object, e As EventArgs) Handles cmbWOStatusList.SelectedIndexChanged

		If cmbWOStatusList.SelectedValue = 2 Then

			lblStarStartDate.Visible = True
			lblStarEndDate.Visible = True

			If Session("OpenFromWOJobListToCompleteForm") = "True" Then 'Added By Prashant On 11-Jul-2023
				lblStarAction.Visible = True
			End If
			phWatchListDetails.Visible = IIf(AppSettings("ShowMaintenanceForNewClients").ToString.Equals("True", StringComparison.InvariantCultureIgnoreCase) AndAlso
											 Not mnWO.MachineID.Equals(Guid.Empty),
											 True,
											 False)

		Else

			lblStarStartDate.Visible = False
			lblStarEndDate.Visible = False
			lblStarAction.Visible = False
			phWatchListDetails.Visible = False

		End If

	End Sub

	Private Sub Component_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbComponent.SelectedIndexChanged
		If chkShowMEL.Checked Then
			pnlMELCategory.Visible = True
		Else
			pnlMELCategory.Visible = False

		End If

		If cmbComponent.SelectedIndex = 0 Then
			cmbMELCategory.Enabled = True
		Else
			With mMELSnagPartList(New Guid(cmbComponent.SelectedValue.ToString), "")
				cmbMELCategory.SelectedValue = .MELCategoryID
				cmbATAChapter.SelectedValue = .CompStatusATAID.ToString
				txtFrequencyInDay.Text = .MELFrequencyInDays
				txtFrequencyInHours.Text = .MELFrequencyInHours
				chkIsInHours.Checked = .MELIsHours

				If mMELSnagPartList(New Guid(cmbComponent.SelectedValue.ToString), "").MELCategoryID = 1 And mMELSnagPartList(New Guid(cmbComponent.SelectedValue.ToString), "").MELIsHours = True Then
					chkIsInHours.Enabled = True
					txtFrequencyInHours.Enabled = True
					txtFrequencyInDay.Enabled = False
				ElseIf mMELSnagPartList(New Guid(cmbComponent.SelectedValue.ToString), "").MELCategoryID = 1 And mMELSnagPartList(New Guid(cmbComponent.SelectedValue.ToString), "").MELIsHours = False Then
					txtFrequencyInDay.Enabled = True
					chkIsInHours.Enabled = False

					txtFrequencyInHours.Enabled = False
				ElseIf mMELSnagPartList(New Guid(cmbComponent.SelectedValue.ToString), "").MELCategoryID <> 1 And chkShowMEL.Checked = True Then
					chkIsInHours.Enabled = False
					txtFrequencyInDay.Enabled = False
					txtFrequencyInHours.Enabled = False
				End If
			End With
			cmbATAChapter.DataBind()
			upnlATA.Update()
		End If
		'----------------------------------------------------------------------
		chkIsInHours.Enabled = False
		cmbMELCategory.Enabled = False
		If chkIsInHours.Checked = True Then
			mnWOJob.FrequencyInHours = txtFrequencyInHours.Text
		Else : mnWOJob.FrequencyInDays = txtFrequencyInDay.Text
			mnWOJob.DateOfOccurrence = txtDateOfOccurrence.Text.ToString
		End If

	End Sub

	Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim da As New ObjectAdapter
		Dim mCompanyDetail As New CompanyDetail
		Dim mnWOTools As nWOTools
		Dim mnWOPeriods As nWOPeriods
		Dim mnWOJobTasks As nWOJobTasks
		Dim mnrptWOJobResourceDetails As nrptWOJobResourceDetails
		Dim mnWOJobSpares As nWOJobSpares
		Dim mnWOJobComps As nWOJobComps
		Dim SearchStr1 As String = New SmartDate(Today.Date).FormattedText
		Dim rpt As New crnWOJobDetail
		Dim ds As New dsnWODetail

		Dim myReport As Engine.ReportClass
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			myReport = New crnWOJobDetailTAAL
		ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Novo") Then
			myReport = New crnWOJobDetailNOVO
		ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "STR") Then 'Added by Saylee on 13-Aug-2018  for StarAir13082018-1
			myReport = New crnWOJobDetailSTR
		ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "IND") Then
			myReport = New crnOffJobSheet
		ElseIf AppSettings("ClientCode") = "AFC" Then 'Added by Saylee on 11-Jun-2025 for FLYPAL-2484 ( W.O. Report for Afcom ) 
			myReport = New crnWODetailForAfcom
		Else
			myReport = New crnWOJobDetail
		End If

		Dim mnWOJobParent As nWOJob = Session("mnWOJobParent")
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
				mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
				mCompanyDetail.WebSite, "NRC Details", SearchStr1, AppSettings("WO-NRCIssueRev"),
				mnWO.WONumber + "-" + mnWOJobParent.SrNo.ToString + "-" + mnWOJob.SrNo.ToString, mnWOJobParent.WorkPACKREF,
				"OpenFromJobNRCPage", AppSettings("Product Version"), AppSettings("SINote"), "", "",
				"", "", AppSettings("Logo"))

		mnWO = Session("mnWO")

		mnWOJobs = mnWO.WOJobs
		mnWOJob = mnWOJob
		mnWOTools = mnWO.WOTools
		mnWOPeriods = mnWO.WOPeriods

		mnWOJobTasks = mnWOJob.WOJobTasks
		mnrptWOJobResourceDetails = nrptWOJobResourceDetails.GetrptWOJobResourceDetails(mnWOJob.ID.ToString)
		mnWOJobSpares = mnWOJob.WOJobSpares
		mnWOJobComps = mnWOJob.WOJobComps

		da.Fill(ds, mnWO)

		If AppSettings("ClientCode") = "AFC" Then
			da.Fill(ds, "nWOJobs", mnWOJob)
		Else
			da.Fill(ds, mnWOJob)
			da.Fill(ds, mnWOJobs)
		End If

		da.Fill(ds, mnWOTools)
		da.Fill(ds, mnWOPeriods)
		da.Fill(ds, mnWOJobTasks)
		da.Fill(ds, mnrptWOJobResourceDetails)
		da.Fill(ds, mnWOJobSpares)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, Report)
		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, mrptImage)
		'rpt.SetDataSource(ds)
		myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport
		Dim Str As String
		Str = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", Str, True)

	End Sub

	Private Sub Back(sender As Object, e As EventArgs) Handles btnBack.Click

		SetObject()

		If mnWOJob.IsDirty Then
			Session("IsValid") = "True"
			MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm, MSGBox.Message_Text.Save, "", MsgBoxStyle.YesNo, "Close")
		Else

			'Added By Saylee On 27-Dec-2018 
			Session.Remove("mFileAttach")
			Session.Remove("IsAttachmentDeleted")
			Session.Remove("ActiveJobNRCDetailsTabIndex")
			'End
			Dim mopenas As String = Request.QueryString("Type")
			If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
				ScriptManager.RegisterStartupScript(Me, [GetType], "onclose", "CallParentCallback();", True)
				Exit Sub
			Else 'Added By Prashant On 11-Jul-2023
				Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
				Exit Sub 'End Of Added By Prashant On 11-Jul-2023
			End If

			Response.Redirect(Request.QueryString("CPage1") & "?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub

	Private Sub MELCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMELCategory.SelectedIndexChanged
		mnWOJob.MELCategoryID = cmbMELCategory.SelectedValue
		txtFrequencyInDay.Text = mnWOJob.FrequencyInDays

		If cmbMELCategory.SelectedIndex = 1 Then
			chkIsInHours.Enabled = True
			txtFrequencyInDay.Enabled = True
		Else
			If chkIsInHours.Checked = True Then
				chkIsInHours.Checked = False
			End If
			chkIsInHours.Enabled = False

			txtFrequencyInDay.Enabled = False
			If txtFrequencyInHours.Text <> "" Then
				txtFrequencyInHours.Text = ""
			End If
			txtFrequencyInHours.Enabled = False
		End If
		cmbMELCategory.Focus()
	End Sub

	Private Sub IsUnderMEL_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsUnderMEL.CheckedChanged
		If chkIsUnderMEL.Checked = True Then
			pnlMEL.Visible = True
			txtDateOfOccurrence.Text = mnWO.WODateFormatted.ToString
			txtFrequencyInHours.Text = ""
			If cmbMELCategory.SelectedIndex = 1 Then
				txtFrequencyInDay.Enabled = True
			End If
			txtFrequencyInHours.Enabled = False
		Else
			pnlMEL.Visible = False
			' lblMEL.Visible = False
			ClearControl() '---Added By Utkarsh On 18-Jan-2011
		End If
	End Sub

	Private Sub HdnBtnAddJobTaskDetail_Click(sender As Object, e As EventArgs) Handles hdnBtnAddJobTaskDetail.Click, hdnBtnAddSelectTasks.Click, hdnimgbtnDesignation.Click, hdnBtnAddResourceAllocation.Click

		If CType(Session("AddTaskCards"), String) = "True" Then
			'Add selected part(s) to Task's Items
			AddMultipleTaskCards()
			Session("mnWO") = mnWO
			Session("AddTaskCards") = "False"
		Else
			Session("AddTaskCards") = "False"
		End If

		If CType(Session("ActiveJobNRCDetailsTabIndex"), Integer) > 0 Then
			If Session("ActiveJobNRCDetailsTabIndex") IsNot Nothing Then WOJobDetailsContainer.ActiveTabIndex = CType(Session("ActiveJobNRCDetailsTabIndex"), Integer) : Session.Remove("ActiveJobNRCDetailsTabIndex")
			lblHeader.Text = mnWOJob.WOJobTasks.Count.ToString
			Label3.Text = mnWOJob.WOJobDesignationAllocations.Count.ToString
			Label4.Text = mnWOJob.WOJobSpares.Count.ToString
			Label5.Text = mnWOJob.WOJobComps.Count.ToString
			up.Update()
			Call WOJobDetails_TabChanged(Nothing, Nothing)
		Else
			WOJobDetailsContainer.ActiveTabIndex = 0
		End If

	End Sub

	Private Sub HdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click
		ControlVisibilityForAttachment()
		upnlAttach.Update()
	End Sub

	Private Sub SelectFile(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick
		If mnWOJob.IsAttachmentAdded Then
			mFileJobAttach = FileAttach.GetAttachment(mnWOJob.ID)
		Else
			mFileJobAttach = FileAttach.NewAttachment(Guid.NewGuid, mnWOJob.ID)
		End If
		Session("mFileAttach") = mFileJobAttach
	End Sub

	Private Sub RefreshTab(sender As Object, e As EventArgs) Handles refreshTabs.Click
		up.DataBind()
	End Sub

	Private Sub CreateRequisition(sender As Object, e As EventArgs) Handles lnkCreateRequisition.Click

		If (AppSettings("ClientCode") <> "STR" And Not User.IsInRole("EngineeringRequisitionNew")) Or (AppSettings("ClientCode") = "STR" And ((mnWO.WOJobs(0).WOJobTypeID = 1 And Not User.IsInRole("PlanningRequisitionNew")) Or (mnWO.WOJobs(0).WOJobTypeID <> 1 And Not User.IsInRole("EngineeringRequisitionNew")))) Then 'For Star Air For Unscheduled Job create Planning Req and for other jobs create Engg. Req.
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		Dim mRequisitionListNew As RequisitionListNew
		If AppSettings("ShowNewWOFlow") = "True" Then 'If AppSettings("ClientCode") = "IND" Then

			mRequisitionListNew = RequisitionListNew.GetRequisitionList(WOID:=mnWO.ID.ToString)

			If mRequisitionListNew.Count > 0 Then

				For i As Integer = 0 To mRequisitionListNew.Count - 1

					If mRequisitionListNew(i).StatusID = 1 Then
						mRequisitionNew = RequisitionNew.GetRequisition(mRequisitionListNew(i).ID)
						GoTo NextStatement
					End If

				Next

				mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
				mRequisitionNew.ReqDate = mnWO.WODate

			Else
				mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
				mRequisitionNew.ReqDate = mnWO.WODate
			End If

		Else

			If AppSettings("ClientCode") = "STR" Then
				If mnWO.WOJobs(0).WOJobTypeID = 1 Then
					mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
				Else
					mRequisitionNew = RequisitionNew.NewRequisition(Trans.PlanningRequisition)
				End If
			Else
				mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
			End If
			mRequisitionNew.ReqDate = mnWO.WODate

		End If
		'End

NextStatement:

		'Added by Shital on 18-Sep-2019
		If AppSettings("ClientCode") = "IND" And mRequisitionNew.IsNew Then 'mRequisitionNew.IsNew Added by vikrant on 19-Sep-2019
			Dim mWorkShopList As WorkShopList
			mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
			mRequisitionNew.LocationID = mWorkShopList(mnWO.WorkShopID).locationID
		End If

		'12-Jun-2019
		For i As Integer = 0 To mRequisitionItemsNew.Count - 1
			ReqItemIds.Append(mRequisitionItemsNew(i).ItemID.ToString + ",")
		Next
		'End

		For j As Integer = 0 To mnWOJob.WOJobSpares.Count - 1

			If Not ReqItemIds.ToString.TrimEnd(",").Contains(mnWOJob.WOJobSpares(j).ItemID.ToString) Then '12-Jun-2019

				Dim mItemList As ItemList
				mItemList = ItemList.GetItemList(1, ItemName:=mnWOJob.WOJobSpares(j).PartNo)

				If mItemList.Count > 0 Then

					If Not mRequisitionNew.RequisitionItemsNew.Contains(mItemList(0).ID) Then

						mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
						mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mItemList(0).ID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mItemList(0).Name
						mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mItemList(0).Description
						mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItemList(0).IPCReference
						mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = mnWOJob.WOJobSpares(j).RequiredQty
						mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mItemList(0).UnitID        'Added By Prashant On 07-May-2019 BA07052019
						mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mItemList(0).UnitName        'Added By Prashant On 07-May-2019 BA07052019
						mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = mItemList(0).IsOneTimePurchase
						mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
						mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber

						If Not mItemList(0).IsOneTimePurchase Then
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = mItemList(0).MinStockLevel
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = mItemList(0).MaxStockLevel
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = mItemList(0).MinReOrderLevel
						Else
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
							mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
						End If

					Else
						mRequisitionNew.RequisitionItemsNew(mItemList(0).ID, "").RequestedQty += mnWOJob.WOJobSpares(j).RequiredQty
					End If

				End If

			Else 'Added By Prashant On 18-Nov-2022 if job is added later with spares having same part no. to create its req. this code is

				If AppSettings("ClientCode") = "KAS" Then

					Dim TempItemWiseRequisitionItemQtySum, TempItemWiseRequiredQtySum, Diffrence As Decimal
					Dim mTempItemID As Guid = mnWOJob.WOJobSpares(j).ItemID
					Dim ItemWiseRequisitionItemQtySum = From c In mRequisitionItemsNew
														Where c.ItemID = mTempItemID
														Group c By ItemID = c.ItemID, PartNo = c.PartNo Into Group
														Select New With {Key .ItemID = ItemID, Key .PartNo = PartNo, Key .RequestedQty = Group.Sum(Function(x) x.RequestedQty)}

					Dim mnWOJobSpares As nWOJobSpares
					mnWOJobSpares = nWOJobSpares.GetWOSpares(mnWO.ID, "")
					Dim ItemWiseRequiredQtySum = From c In mnWOJobSpares
												 Where c.ItemID = mTempItemID
												 Group c By ItemID = c.ItemID, PartNo = c.PartNo Into Group
												 Select New With {Key .ItemID = ItemID, Key .PartNo = PartNo, Key .RequiredQty = Group.Sum(Function(x) x.RequiredQty)}

					For Each variable1 As Object In ItemWiseRequisitionItemQtySum
						TempItemWiseRequisitionItemQtySum = variable1.RequestedQty
					Next
					For Each variable2 As Object In ItemWiseRequiredQtySum
						TempItemWiseRequiredQtySum = variable2.RequiredQty
					Next

					Diffrence = (TempItemWiseRequiredQtySum - TempItemWiseRequisitionItemQtySum)

					If TempItemWiseRequiredQtySum > TempItemWiseRequisitionItemQtySum And Not Diffrence < 0 Then

						Dim mItemList As ItemList
						mItemList = ItemList.GetItemList(1, ItemName:=mnWOJob.WOJobSpares(j).PartNo)

						If mItemList.Count > 0 Then

							If Not mRequisitionNew.RequisitionItemsNew.Contains(mItemList(0).ID) Then

								mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
								mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mItemList(0).ID
								mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mItemList(0).Name
								mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mItemList(0).Description
								mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItemList(0).IPCReference
								mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = Diffrence
								mRequisitionNew.RequisitionItemsNew.CurrentItem.UnitID = mItemList(0).UnitID        'Added By Prashant On 07-May-2019 BA07052019
								mRequisitionNew.RequisitionItemsNew.CurrentItem.Unit = mItemList(0).UnitName        'Added By Prashant On 07-May-2019 BA07052019
								mRequisitionNew.RequisitionItemsNew.CurrentItem.IsOneTimePurchase = mItemList(0).IsOneTimePurchase
								mRequisitionNew.RequisitionItemsNew.CurrentItem.MachineID = mnWO.MachineID
								mRequisitionNew.RequisitionItemsNew.CurrentItem.RegNo = mnWO.RegNo
								mRequisitionNew.RequisitionItemsNew.CurrentItem.WOID = mnWO.ID
								mRequisitionNew.RequisitionItemsNew.CurrentItem.WONo = mnWO.WONumber

								If Not mItemList(0).IsOneTimePurchase Then
									mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = mItemList(0).MinStockLevel
									mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = mItemList(0).MaxStockLevel
									mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = mItemList(0).MinReOrderLevel
								Else
									mRequisitionNew.RequisitionItemsNew.CurrentItem.MinStockLevel = 0
									mRequisitionNew.RequisitionItemsNew.CurrentItem.MaxStockLevel = 0
									mRequisitionNew.RequisitionItemsNew.CurrentItem.MinReOrderLevel = 0
								End If

							End If

						End If

					End If
					'-----------
				End If

			End If

		Next

		Session("mRequisitionNew") = mRequisitionNew

		If AppSettings("ClientCode") = "STR" Then

			If mnWO.WOJobs(0).WOJobTypeID = 1 Then
				Session("TransTypeID") = Trans.EngineeringRequisition
				MarkLog(Action.[New], "Engineering Requisition", "", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
			Else
				Session("TransTypeID") = Trans.PlanningRequisition
				MarkLog(Action.[New], "Planning Requisition", "", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
			End If

		Else
			Session("TransTypeID") = Trans.EngineeringRequisition
			MarkLog(Action.[New], "Engineering Requisition", "", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
		End If

		Dim ReqURLFromWO As New Stack
		ReqURLFromWO.Push(Request.Url)
		Session("ReqURLFromWO") = ReqURLFromWO
		Session("MiddleFrameForWO") = Session("MiddleFrame")
		Session("TransTypeID") = CInt(Trans.EngineeringRequisition)
		Response.Redirect("wfRequisition_Ajax.aspx?BackPage=wfnWODetail_AJAX.aspx")

	End Sub

	Private Sub ViewIndent(sender As Object, e As EventArgs) Handles lnkViewIndent.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		SetObject()
		Session("mnWO") = mnWO
		Session("mWOID") = mnWO.ID
		ScriptManager.RegisterClientScriptBlock(Me, [GetType], "RequisitionView", "RequisitionView();", True)
	End Sub

#End Region

#Region " TAB's "

	Private Sub WOJobDetails_TabChanged(sender As Object, e As EventArgs) Handles WOJobDetailsContainer.ActiveTabChanged

		Session("mnWO") = mnWO
		Session("nWOJobNRC") = mnWOJob ' mnWO.WOJobs.CurrentItem
		Session("mnWOJob") = mnWOJob
		Session("mIndex") = "-1"
		lblHeader.Text = mnWOJob.WOJobTasks.Count.ToString
		Label3.Text = mnWOJob.WOJobDesignationAllocations.Count.ToString
		Label4.Text = mnWOJob.WOJobSpares.Count.ToString
		Label5.Text = mnWOJob.WOJobComps.Count.ToString

		Select Case WOJobDetailsContainer.ActiveTabIndex
			Case 0
				txtEstimatedTime.DataBind()
				txtActualTime.DataBind()
			Case 1      'tabWOJobTask
				Session("ActiveJobNRCDetailsTabIndex") = 1
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobTask", "CallWOJobTask();", True)
			Case 2      'tabWOJobDesignationAllocations
				Session("mDesignationAllocationEdit") = False
				Session("ActiveJobNRCDetailsTabIndex") = 2
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobDesignationAllocations", "CallWOJobDesignationAllocations();", True)
			Case 3      'tabWOJobSpares
				Session("ActiveJobNRCDetailsTabIndex") = 3
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobSpares", "CallWOJobSpares();", True)
			Case 4      'tabWOJobComps
				Session("Edit") = False
				Session("ActiveJobNRCDetailsTabIndex") = 4
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobComps", "CallWOJobComps();", True)
			Case 5      'tabWOJobNRC
				Session("ActiveJobNRCDetailsTabIndex") = 5
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobNRC", "CallWOJobNRC();", True)
		End Select

	End Sub

#End Region

End Class