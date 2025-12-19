'******************************************
'Modified by Harsh Sugandhi on 24th July 2024 for FLYPAL-1771 Facility to add Multiple Job Actions W.O. Job
'Modified by Harsh Sugandhi on 25th Feb 2025 FLYPAL-2221 Provision to add JOB NRC to WatchList.
'******************************************


Imports System.Text

Imports CrystalDecisions.CrystalReports.Engine

Public Class wfnWOJobDetail
	Inherits Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub


	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As Object

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Enumeration "

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

	Public mMELCategoryList As MELCategoryList
	Public mnWOJobStatusList As nWOJobStatusList
	Public mMELSnagPartList As MELSnagPartList
	Public mATAList As ATAList
	Public mnWOJob As nWOJob
	Public mnWO As nWO
	Dim mWOJobTypeID As Integer
	Dim EventLogID As Guid
	Dim mWODetail As String
	Dim mWOJobNRCList As WOJobNRCList
	Dim mnWOJobNRC As nWOJob
	Dim mFileJobAttach As FileAttach
	Dim IsAttachmentDeleted As Boolean = False
	Dim mRequisitionNew As RequisitionNew
	Dim mRequisitionItemsNew As RequisitionItemsNew
	Dim ReqItemIds As New StringBuilder
	Dim mMPDSkillList As MPDSkillList 'Added by Saylee on 3-Jul-2023
	Public mProject As Project 'Added By Prashant On 16-May-2024
	Public mWOJobActions As nWOJobActions

	Dim mModuleList As ModuleList 'Added by Harsh for FLYPAL-2221

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		mnWOJobStatusList = Session("mnWOJobStatusList")
		mMELSnagPartList = Session("mMELSnagPartList")
		mATAList = Session("mATAList")
		mnWOJob = Session("mnWOJob")
		mnWO = Session("mnWO")
		mWOJobTypeID = CType(Session("WOJobTypeID"), Integer)
		mWOJobNRCList = CType(Session("mWOJobNRCList"), WOJobNRCList)
		mFileJobAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		mRequisitionItemsNew = Session("mRequisitionItemsNew")
		mMPDSkillList = Session("mMPDSkillList")
		mProject = Session("mProject")
		mModuleList = Session("mModuleList")

	End Sub

	Private Sub SetSession()
		Session("mMELSnagPartList") = mMELSnagPartList
		Session("WOJobTypeID") = mWOJobTypeID
		Session("mFileAttach") = mFileJobAttach
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
	End Sub

	Private Function IsInRole(CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		If AppSettings("ShowNewWOFlow") = "True" Then
			'IsInRoleString = "CAMOWOCreate"
			If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then
				If mnWO.TransTypeID = Trans.WO145 Then
					IsInRoleString = "WOCreate"
				ElseIf mnWO.TransTypeID = Trans.OJS145 Then
					IsInRoleString = "OJSWorkOrder"
				ElseIf mnWO.TransTypeID = Trans.OJSCAMO Then
					IsInRoleString = "OJSCAMOWorkOrder"
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
				'IsInRoleString = "CAMOWO"
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
		End Select
	End Function

	Private Sub ControlVisibilityForAttachment()

		If mFileJobAttach Is Nothing Then
			GetAttachment()
		End If
		If mFileJobAttach IsNot Nothing Then
			If mFileJobAttach.Size > 0 Then
				ImageButton1.Visible = True
				imgDelAttach1.Enabled = True
				btnSelectFile.Disabled = True
			Else
				ImageButton1.Visible = False
				btnSelectFile.Disabled = False
			End If
		End If
	End Sub

	'Added By Saylee On 26-Sep-2018 For STR26092018
	Private Function IsValidTime(TimeValue As String) As Boolean
		Dim TimeRegulerExpression As String = ""
		If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
			'TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm)$"    '12 Hour Format
			TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
		Else
			TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
		End If

		If (RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
			Return True
		Else
			Return False
		End If
	End Function
	'End

	Private Sub GetAttachment()
		If mnWO.WOJobs.CurrentItem.IsAttachmentAdded And mFileJobAttach Is Nothing Then
			mFileJobAttach = FileAttach.GetAttachment(mnWO.WOJobs.CurrentItem.ID) 'Sort = 2 : Removal
			Session("mFileAttach") = mFileJobAttach
		End If
	End Sub

	Private Sub SaveAttachment() '
		mFileJobAttach.ReferenceID = mnWO.WOJobs.CurrentItem.ID
		If mFileJobAttach.Size > 0 Then
			Try
				If (Not mnWO.WOJobs.CurrentItem.IsNew) And IsAttachmentDeleted Then
					FileAttach.DeleteAllAttachmentChilds(mnWO.WOJobs.CurrentItem.ID)
				End If
				IsAttachmentDeleted = False
				Session("IsAttachmentDeleted") = IsAttachmentDeleted
				mFileJobAttach.Save()
			Catch ex As Exception
				ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), False)
			End Try
		Else
			If (Not mnWO.WOJobs.CurrentItem.IsNew) And IsAttachmentDeleted Then
				FileAttach.DeleteAttachment(mFileJobAttach.ID, mnWO.WOJobs.CurrentItem.ID)
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
				ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFilel();", True)
			End If
		End If
	End Sub

	Private Sub CallUpdatePanels()

		upnlWOJobDetails.Update()
		upnlJobCompletionDetails.Update()
		upnlMELSnagDetails.Update()
		upnlTitle.Update()
		upnlButtons.Update()

	End Sub

	Private Sub ControlVisibility()

		Try

			pnlMELSnagDetails.Visible = (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3)
			chkIsUnderMEL.Visible = (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3 And mnWO.WOJobs.CurrentItem.IsUnderMEL = True)
			lblIsUnderMEL.Visible = (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3 And mnWO.WOJobs.CurrentItem.IsUnderMEL = True)
			lblIsUnderMELNote.Visible = (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3 And mnWO.WOJobs.CurrentItem.IsUnderMEL = True)
			pnlSnag.Visible = (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3 Or mnWO.WOJobs.CurrentItem.WOJobTypeID = 1)

			If AppSettings("ClientCode") = "STR" And mnWO.StatusID >= 2 Then txtWOJobDescription.Enabled = False

			If chkIsUnderMEL.Checked = True Then

				pnlMELCategory.Visible = True
				mMELSnagPartList = Nothing
				mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateOfOccurrence.Text, mnWO.MachineID.ToString, "(SELECT)")
				cmbComponent.DataSource = mMELSnagPartList
				Session("mMELSnagPartList") = mMELSnagPartList
				cmbComponent.DataBind()
				cmbMELCategory.Enabled = False
				If Not mMELSnagPartList.Contains(mnWO.WOJobs.CurrentItem.CompID) Then mnWO.WOJobs.CurrentItem.CompID = Guid.Empty Else cmbComponent.SelectedValue = mnWO.WOJobs.CurrentItem.CompID.ToString

			Else

				pnlMELCategory.Visible = False
				mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateOfOccurrence.Text, , "(SELECT)")
				cmbComponent.DataSource = mMELSnagPartList
				Session("mMELSnagPartList") = mMELSnagPartList
				cmbComponent.DataBind()
				If Not mMELSnagPartList.Contains(mnWO.WOJobs.CurrentItem.CompID) Then mnWO.WOJobs.CurrentItem.CompID = Guid.Empty Else cmbComponent.SelectedValue = mnWO.WOJobs.CurrentItem.CompID.ToString

			End If

			If mnWO.WOJobs.CurrentItem.WOJobTypeID = 3 Then

				chkIsMajor.Enabled = False
				chkIsRepetitive.Enabled = False
				txtDateOfOccurrence.Visible = True
				cmbComponent.Visible = True
				txtDateOfOccurrence.Enabled = False
				cmbComponent.Enabled = False
				If chkIsUnderMEL.Checked = True Then txtFrequencyInDay.Visible = True
				If chkIsUnderMEL.Checked = True Then txtFrequencyInHours.Visible = True
				If chkIsUnderMEL.Checked = True Then txtFrequencyInDay.Enabled = False
				If chkIsUnderMEL.Checked = True Then txtFrequencyInHours.Enabled = False

			Else

				chkIsMajor.Enabled = True
				chkIsRepetitive.Enabled = True
				txtDateOfOccurrence.Visible = False
				cmbComponent.Visible = False
				If chkIsUnderMEL.Checked = False Then txtFrequencyInDay.Visible = False
				If chkIsUnderMEL.Checked = False Then txtFrequencyInHours.Visible = False

			End If

			cmbATAChapter.Enabled = mnWO.WOStatusID <> 3
			btnSelectFile.Disabled = (mnWO.WOStatusID = 3)

			If (Session("MiddleFrame") = "wfnWOExecutionList.aspx" Or
				Session("MiddleFrame") = "wfnWOCompletionList.aspx?") And
			   (mnWO.WOJobs.CurrentItem.WOJobStatusID = 2 Or
				mnWO.WOJobs.CurrentItem.WOJobStatusID = 3 Or
				mnWO.WOJobs.CurrentItem.WOJobStatusID = 4) Then 'Added By Prashant 16-Aug-2019

				txtStartDate.Enabled = False
				txtEndDate.Enabled = False
				cmbWOStatusList.Enabled = False

			Else

				If AppSettings("ShowNewWOFlow") = "True" Then
					txtStartDate.Enabled = mnWO.WOStatusID <> 3 And mnWO.StatusID <> 1 And mnWO.WOStatusID = 4
					txtEndDate.Enabled = mnWO.WOStatusID <> 3 And mnWO.StatusID <> 1 And mnWO.WOStatusID = 4
					cmbWOStatusList.Enabled = mnWO.WOStatusID <> 3 And mnWO.StatusID <> 1 And mnWO.WOStatusID = 4
				Else
					txtStartDate.Enabled = mnWO.WOStatusID <> 3 And mnWO.StatusID <> 1
					txtEndDate.Enabled = mnWO.WOStatusID <> 3 And mnWO.StatusID <> 1
					cmbWOStatusList.Enabled = mnWO.WOStatusID <> 3 And mnWO.StatusID <> 1
				End If

			End If

			Dim WONo As String = ""
			WONo = " [ " + mnWO.WONumber + " Dated: " + mnWO.WODateFormatted + " ]"

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso
			   (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

				lblTitle.Text = " E.O. JOB Details" + WONo
			Else
				lblTitle.Text = " W.O. JOB Details" + WONo
			End If

			If mnWO.WOJobs.CurrentItem.Size > 0 Then

				ImageButton1.Visible = True
				If mnWO.WOStatusID = 3 Then
					imgDelAttach1.Enabled = False
				Else
					imgDelAttach1.Enabled = True
				End If

			Else
				ImageButton1.Visible = False
			End If

			txtDueAsOf.Enabled = (mnWO.WOJobs.CurrentItem.WOJobTypeID <> 2) 'Added By Vikrant On 15-May-2014 For ALL15052014

			ControlVisibilityForAttachment() 'Added by Saylee On 27-Dec-2018

			If Not mnWO.WOJobs.CurrentItem.IsNew Then

				'Added By Prashant On 30-Jun-2023
				'Modified by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
				If (AppSettings("ShowMaintenanceForNewClients") = "True" And AppSettings("ShowCAMOOnlyForNewClients") = "True" And (mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102 Or Session("wfProject_Ajax") = "wfProject_Ajax")) Then '89 Camo WO

					If AppSettings("ShowMaintenanceForNewClientsWithTaskCard").ToUpper = "True".ToUpper Then 'Added By Prashant on 24-Sep-2024
						WOJobDetailsTabContainer.Tabs(1).Visible = True   'Task Card link
					Else
						WOJobDetailsTabContainer.Tabs(1).Visible = False 'Task Card link
					End If

					WOJobDetailsTabContainer.Tabs(2).Visible = False 'Allocate link
					WOJobDetailsTabContainer.Tabs(3).Visible = False 'Spares link 

					If AppSettings("ShowAMOOnlyForNewClients") = "True" Then WOJobDetailsTabContainer.Tabs(3).Visible = True 'if both keys are True

					WOJobDetailsTabContainer.Tabs(4).Visible = False  'Installation/Removal link

				ElseIf (AppSettings("ShowMaintenanceForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "True" And mnWO.TransTypeID = 88 Or Session("wfProject_Ajax") = "wfProject_Ajax") Then '88 Third Party WO

					If AppSettings("ShowMaintenanceForNewClientsWithTaskCard").ToUpper = "True".ToUpper Then 'Added By Prashant on 24-Sep-2024
						WOJobDetailsTabContainer.Tabs(1).Visible = True   'Task Card link
					Else
						WOJobDetailsTabContainer.Tabs(1).Visible = False 'Task Card link
					End If
					WOJobDetailsTabContainer.Tabs(2).Visible = False   'Allocate link
					WOJobDetailsTabContainer.Tabs(3).Visible = True  'Spares link 
					WOJobDetailsTabContainer.Tabs(4).Visible = False 'Installation/Removal link

				Else

					WOJobDetailsTabContainer.Tabs(1).Visible = True 'Task Card link
					WOJobDetailsTabContainer.Tabs(2).Visible = True 'Allocate link
					WOJobDetailsTabContainer.Tabs(3).Visible = True 'Spares link 
					WOJobDetailsTabContainer.Tabs(4).Visible = True  'Installation/Removal link

				End If

				WOJobDetailsTabContainer.Tabs(5).Visible = True 'NRC link

				If mnWO.StatusID >= 4 Or (mnWO.WOStatusID = 3) Or
				   (mnWO.WOJobs.CurrentItem.WOJobStatusID = 3 Or mnWO.WOJobs.CurrentItem.WOJobStatusID = 4) Then 'WOStatusID= 3 Completed,StatusID=4 Cancelled, WOJobStatusID =3 Deferred,WOJobStatusID=4 Cancelled

					WOJobDetailsTabContainer.Tabs(1).Enabled = False 'Task Card link
					WOJobDetailsTabContainer.Tabs(2).Enabled = False 'Allocate link
					WOJobDetailsTabContainer.Tabs(3).Enabled = False  'Spares link
					WOJobDetailsTabContainer.Tabs(4).Enabled = False 'Installation/Removal link 
					WOJobDetailsTabContainer.Tabs(5).Enabled = False 'NRC link

					If Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Then

						btnSave.Enabled = False
						WOJobDetailsTabContainer.Tabs(1).Enabled = True 'Task Card link
						WOJobDetailsTabContainer.Tabs(2).Enabled = True 'Allocate link
						WOJobDetailsTabContainer.Tabs(3).Enabled = True  'Spares link
						WOJobDetailsTabContainer.Tabs(4).Enabled = True 'Installation/Removal link
						WOJobDetailsTabContainer.Tabs(5).Enabled = True 'NRC link

					Else

						btnSave.Enabled = Not (mnWO.WOStatusID = 3)
						WOJobDetailsTabContainer.Tabs(1).Enabled = False 'Task Card link
						WOJobDetailsTabContainer.Tabs(2).Enabled = False 'Allocate link
						WOJobDetailsTabContainer.Tabs(3).Enabled = False 'Spares link
						WOJobDetailsTabContainer.Tabs(4).Enabled = False 'Installation/Removal link
						WOJobDetailsTabContainer.Tabs(5).Enabled = False 'NRC link

					End If

				Else

					If Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Or Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then
						WOJobDetailsTabContainer.Tabs(4).Visible = False 'Installation/Removal link
						WOJobDetailsTabContainer.Tabs(5).Visible = False 'NRC link

						If Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then WOJobDetailsTabContainer.Tabs(1).Enabled = IIf(mnWO.WOJobs.IsAleastOneJobCompleted = True, False, True) 'Task Card link
						If Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then WOJobDetailsTabContainer.Tabs(2).Enabled = IIf(mnWO.WOJobs.IsAleastOneJobCompleted = True, False, True) 'Allocate link
						If Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Then WOJobDetailsTabContainer.Tabs(3).Enabled = IIf(mnWO.WOJobs.IsAleastOneJobCompleted = True, False, True) 'Spares link

					Else

						WOJobDetailsTabContainer.Tabs(1).Enabled = True 'Task Card link
						WOJobDetailsTabContainer.Tabs(2).Enabled = True  'Allocate link
						WOJobDetailsTabContainer.Tabs(3).Enabled = True 'Spares link

						'Added By Prashant On 30-Jun-2023
						'Modified by Harsh Sugandhi on 20th June 2024 for FLYPAL-1703 Engineering Work Order
						If (AppSettings("ShowMaintenanceForNewClients") = "True" And AppSettings("ShowCAMOOnlyForNewClients") = "True" And (mnWO.TransTypeID = 89 Or mnWO.TransTypeID = 102)) Then '89 Camo WO
							WOJobDetailsTabContainer.Tabs(4).Visible = False 'Installation/Removal link
						ElseIf (AppSettings("ShowMaintenanceForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "True" And mnWO.TransTypeID = 88) Then '88 Third Party WO
							WOJobDetailsTabContainer.Tabs(4).Visible = False 'Installation/Removal link
						End If
						WOJobDetailsTabContainer.Tabs(5).Visible = True 'NRC link
					End If
				End If
			Else
				WOJobDetailsTabContainer.Tabs(1).Visible = False 'Task Card link
				WOJobDetailsTabContainer.Tabs(2).Visible = False 'Allocate link
				WOJobDetailsTabContainer.Tabs(3).Visible = False 'Installation/Removal link
				WOJobDetailsTabContainer.Tabs(4).Visible = False 'Spares link
				WOJobDetailsTabContainer.Tabs(5).Visible = False 'NRC link
			End If

			lnkViewIndent.Enabled = (mRequisitionItemsNew.Count > 0)
			Fieldset2.Visible = (((mnWO.StatusID = 2 And mnWO.WOStatusID = 1) Or (mnWO.WOStatusID = 4)) And (mnWO.WOStatusID <> 3) And (AppSettings("ShowCAMOOnlyForNewClients") = "False" Or AppSettings("ShowAMOOnlyForNewClients") = "True"))

			If mRequisitionItemsNew.Count > 0 Then
				lnkViewIndent.Text = "Requisition Item (" + mRequisitionItemsNew.Count.ToString + ")"
			End If
			'End

			If lnkCreateRequisition.Enabled Then
				lnkCreateRequisition.ToolTip = "Click to create Requisition of Job Spares Items(s)"
			Else
				lnkCreateRequisition.ToolTip = "Requisition already created against this WO."
			End If

			If AppSettings("ClientCode") = "IND" Then
				lblWOJobNRCTab.Text = "OJS"
			Else
				lblWOJobNRCTab.Text = "NRC"
			End If

			If Session("OpenFromWOJobListToCompleteForm") = "True" Then 'Added By Prashant On 11-Jul-2023

				WOJobDetailsTabContainer.Tabs(1).Visible = False 'Task Card link
				WOJobDetailsTabContainer.Tabs(2).Visible = False 'Allocate link
				WOJobDetailsTabContainer.Tabs(4).Visible = False 'Installation/Removal link
				btnSelectFile.Disabled = True
				txtTaskNo.Enabled = False
				txtInspCode.Enabled = False
				cmbATAChapter.Enabled = False
				txtWOJobDescription.Enabled = False
				txtSkill.Enabled = False
				cmbSkillcode.Enabled = False
				txtEstimatedTime.Enabled = False
				txtPublication.Enabled = False
				txtTaskSourceRef.Enabled = False
				txtZone.Enabled = False
				txtArea.Enabled = False
				txtPanels.Enabled = False
				txtWorkPackRef.Enabled = False
				chkIsForBilling.Enabled = False
				txtAMPRevNo.Enabled = False
				txtRevDate.Enabled = False
				txtDueAsOf.Enabled = False
				chkIsRII.Enabled = False
				chkOtherJob.Enabled = False
				txtOtherJobSpecification.Enabled = False
				imgDelAttach1.Visible = False

			End If

			If Session("ToDisbleJobControlsAsCompletedRightNotGiven") IsNot Nothing Then

				If Session("ToDisbleJobControlsAsCompletedRightNotGiven") = "True" Then

					WOJobDetailsTabContainer.Tabs(1).Enabled = False 'Task Card link
					WOJobDetailsTabContainer.Tabs(2).Enabled = False 'Allocate link
					WOJobDetailsTabContainer.Tabs(3).Enabled = False 'Spares link
					WOJobDetailsTabContainer.Tabs(4).Enabled = False 'Installation/Removal link
					WOJobDetailsTabContainer.Tabs(5).Enabled = False 'NRC link

				End If

			End If

			phWatchListDetails.Visible = IIf(cmbWOStatusList.SelectedValue = 2 AndAlso
											 AppSettings("ShowMaintenanceForNewClients").ToString.Equals("True", StringComparison.InvariantCultureIgnoreCase) AndAlso
											 Not mnWO.MachineID.Equals(Guid.Empty),
											 True,
											 False)
			'Sankalp 07/Aug/2025
			phMethodOfCompliance.Visible = IIf(mnWO.TransTypeID = 102 AndAlso mnWO.WOJobTypeID = 2,
								 True,
								 False)

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

	Private Sub SetObject()

		Try

			If txtStartDate.Text.ToString <> "" Then

				If txtStartDateTime.Text <> "" Then
					mnWO.WOJobs.CurrentItem.WOJobStartDate = CType(txtStartDate.Text.ToString.Trim + " " + txtStartDateTime.Text.ToString.Trim, DateTime)
				Else
					mnWO.WOJobs.CurrentItem.WOJobStartDate = txtStartDate.Text
				End If

			Else
				mnWO.WOJobs.CurrentItem.WOJobStartDate = DBNull.Value
			End If

			If txtEndDate.Text.ToString <> "" Then

				If txtEndDateTime.Text <> "" Then
					mnWO.WOJobs.CurrentItem.WOJobCloseDate = CType(txtEndDate.Text.ToString.Trim + " " + txtEndDateTime.Text.ToString.Trim, DateTime)
				Else
					mnWO.WOJobs.CurrentItem.WOJobCloseDate = txtEndDate.Text
				End If

			Else
				mnWO.WOJobs.CurrentItem.WOJobCloseDate = DBNull.Value
			End If

			mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = txtEstimatedTime.Text
			mnWO.WOJobs.CurrentItem.WOJobActualTime = txtActualTime.Text
			mnWO.WOJobs.CurrentItem.WOJobStatusID = cmbWOStatusList.SelectedValue
			mnWO.WOJobs.CurrentItem.IsForBilling = chkIsForBilling.Checked
			mnWO.WOJobs.CurrentItem.WOJobDescription = txtWOJobDescription.Text.Trim
			mnWO.WOJobs.CurrentItem.WOJobAction = txtWOJobAction.Text
			mnWO.WOJobs.CurrentItem.MethodOfCompliance = txtMethodOfCompliance.Text 'Sankalp 08-08-25
			mnWO.WOJobs.CurrentItem.WOJobRemark = txtWOJobRemark.Text
			mnWO.WOJobs.CurrentItem.IsUnderMEL = chkIsUnderMEL.Checked

			If (txtDateOfOccurrence.Text.ToString <> "") Then
				mnWO.WOJobs.CurrentItem.DateOfOccurrence = txtDateOfOccurrence.Text
			Else
				mnWO.WOJobs.CurrentItem.DateOfOccurrence = DBNull.Value
			End If

			mnWO.WOJobs.CurrentItem.ATAChapterID = New Guid(cmbATAChapter.SelectedValue)
			mnWO.WOJobs.CurrentItem.CompID = New Guid(cmbComponent.SelectedValue)
			mnWO.WOJobs.CurrentItem.MELCategoryID = cmbMELCategory.SelectedValue
			mnWO.WOJobs.CurrentItem.IsMajor = chkIsMajor.Checked
			mnWO.WOJobs.CurrentItem.IsRepetitive = chkIsRepetitive.Checked
			mnWO.WOJobs.CurrentItem.IsHours = chkIsInHours.Checked
			mnWO.WOJobs.CurrentItem.FrequencyInDays = Val(txtFrequencyInDay.Text)
			mnWO.WOJobs.CurrentItem.FrequencyInHours = txtFrequencyInHours.Text.Trim
			mnWO.WOJobs.CurrentItem.WOJobTypeID = mWOJobTypeID

			If mnWO.WOStartDate.ToString = "" And txtStartDate.Text.ToString <> "" Then

				mnWO.WOStartDate = txtStartDate.Text

				If (AppSettings("ClientCode") = "IND" Or
					AppSettings("ClientCode") = "YA" Or
					AppSettings("ClientCode") = "AFC" Or
					AppSettings("ClientCode") = "ARA" Or
					AppSettings("ClientCode") = "BAP" Or
					AppSettings("ClientCode") = "RPS" Or
					AppSettings("ClientCode") = "GLD") Then

					If txtStartDate.Text.ToString <> "" Then

						If txtStartDateTime.Text <> "" Then
							mnWO.WOStartDate = CType(txtStartDate.Text.ToString.Trim + " " + txtStartDateTime.Text.ToString.Trim, DateTime)
						Else
							mnWO.WOStartDate = txtStartDate.Text
						End If

					Else
						mnWO.WOStartDate = DBNull.Value
					End If

				End If

				'Added by Vikrant for Heligo Change
				If Not mnWO.MachineID.Equals(Guid.Empty) Then

					Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
					Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.
																				GetMachineListWithInstallation(txtStartDate.Text.ToString,
																											   mnWO.MachineID.ToString, , , , , , , , , ,
																											   True, , , ,
																											   "Airframe").Item(0), MachineInfo).AssemblyStatusList
					AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList

					If mnWO.WOPeriods.Count <> 0 Then

						For i As Integer = mnWO.WOPeriods.Count - 1 To 0 Step -1
							mnWO.WOPeriods.RemoveAt(i)
						Next

					End If

					mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)

				End If

			End If

			'Added By Vikrant On 06-Nov-2012 For ALL06112012-1
			mnWO.WOJobs.CurrentItem.Zone = Trim(txtZone.Text)
			mnWO.WOJobs.CurrentItem.AREA = Trim(txtArea.Text)
			mnWO.WOJobs.CurrentItem.WorkPACKREF = Trim(txtWorkPackRef.Text)
			mnWO.WOJobs.CurrentItem.Publication = Trim(txtPublication.Text)

			If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or
			   AppSettings("ShowAMOOnlyForNewClients") = "True" Then

				mnWO.WOJobs.CurrentItem.SkillID = Val(cmbSkillcode.SelectedValue.ToString)

				If (cmbSkillcode.SelectedIndex > 0) Then
					mnWO.WOJobs.CurrentItem.Skill = mMPDSkillList(mnWO.WOJobs.CurrentItem.SkillID, "").Name
				End If

				mnWO.WOJobs.CurrentItem.SkillCode = mMPDSkillList(mnWO.WOJobs.CurrentItem.SkillID, "").Code

			ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "False" Or
				   AppSettings("ShowAMOOnlyForNewClients") = "False" Then

				mnWO.WOJobs.CurrentItem.Skill = Trim(txtSkill.Text)
				mnWO.WOJobs.CurrentItem.SkillCode = Trim(txtSkill.Text)

			End If

			mnWO.WOJobs.CurrentItem.Panels = Trim(txtPanels.Text)
			mnWO.WOJobs.CurrentItem.InspCode = Trim(txtInspCode.Text)
			'End
			mnWO.WOJobs.CurrentItem.IsRII = chkIsRII.Checked      'Added By Saylee on 18-Jan-2012 for BA17012013
			mnWO.WOJobs.CurrentItem.AMPRevNo = txtAMPRevNo.Text  'Added By Saylee on 03-Apr-2013 for BA03032013

			If txtRevDate.Text.ToString <> "" Then             'Added By Saylee on 03-Apr-2013 for BA03032013
				mnWO.WOJobs.CurrentItem.AMPRevDate = txtRevDate.Text
			Else
				mnWO.WOJobs.CurrentItem.AMPRevDate = DBNull.Value
			End If

			mnWO.WOJobs.CurrentItem.TaskSourceRef = Trim(txtTaskSourceRef.Text) 'Added By Vikrant On 23-May-2013 For BA23052013-1	

			'Added By Vikrant On 15-May-2014 For ALL15052014
			If mnWO.WOJobs.CurrentItem.WOJobTypeID <> 2 Then
				mnWO.WOJobs.CurrentItem.DueAsOf = Trim(txtDueAsOf.Text)
			End If
			'End

			mnWO.WOJobs.CurrentItem.IsAttachmentAdded = IIf(mnWO.WOJobs.CurrentItem.FileAttachments.Count > 0, True, False)
			'End
			mnWO.WOJobs.CurrentItem.OtherJob = chkOtherJob.Checked 'Added By Prashant 14-Aug-2020 STR14082020
			mnWO.WOJobs.CurrentItem.OtherJobSpecification = txtOtherJobSpecification.Text 'Added by Shital on 31-Aug-2020 STR31082020
			mnWO.WOJobs.CurrentItem.TaskCardNo = txtTaskNo.Text.Trim

			If cmbWOStatusList.SelectedIndex = 1 Then 'Added By Prashant On 11-Jul-2023

				Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
				mnWO.WOJobs.CurrentItem.CompletedBy = User.Identity.Name
				mnWO.WOJobs.CurrentItem.CompletedByEmployeeID = mUser.EmployeeID
				mnWO.WOJobs.CurrentItem.CompletedByEmployeeName = mUser.EmployeeName

			End If
			'******************************

			For i As Integer = 0 To mnWO.WOJobs.CurrentItem.FileAttachments.Count - 1

				Dim txtValue As TextBox
				txtValue = CType(Me.dgWOJobAttachment.Rows(i).FindControl("txtFileName"), TextBox)
				mnWO.WOJobs.CurrentItem.FileAttachments(i).FileName = txtValue.Text.Trim

			Next

			mnWO.WOJobs.CurrentItem.AddToWatchList = IIf(cmbWOStatusList.SelectedIndex = 1,
														 chkAddToWatchList.Checked,
														 False)

			mnWO.WOJobs.CurrentItem.WatchListInstructions = IIf(cmbWOStatusList.SelectedIndex = 1,
																Trim(txtWatchListInstructions.Text),
																String.Empty)

			Session("mnWO") = mnWO

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function CustomValidateJob() As Boolean

		Dim strMSG As String = ""

		If Not mnWO.WOJobs.CurrentItem.IsValid Then

			For i As Integer = 0 To mnWO.WOJobs.CurrentItem.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWO.WOJobs.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
			Next

		End If

		If strMSG <> "" Then

			cvCurrentValue.ErrorMessage = strMSG
			cvCurrentValue.IsValid = False

			Return False

		End If

		Return True

	End Function

	Private Function CustomValidationAndWO() As Boolean

		Dim strMSG As String = ""

		If Not mnWO.IsValid Then

			For i As Integer = 0 To mnWO.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWO.GetBrokenRulesCollection(i).Description + "<Br>"
			Next

		End If

		If Not mnWO.WOJobs.CurrentItem.IsValid Then

			For i As Integer = 0 To mnWO.WOJobs.CurrentItem.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWO.WOJobs.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
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

		If Len(txtTaskSourceRef.Text) > 500 Then
			strMSG = strMSG + "Task Source Ref. must not be Greater than 500 Char."
		End If

		Try
			Dim ValueiInDecimal As String
			If txtEstimatedTime.Text <> "" Then ValueiInDecimal = nWOPeriod.ConvertStringToDecimal(1, 1, txtEstimatedTime.Text, False)

			Dim ValueiInDecimal2 As String
			If txtActualTime.Text <> "" Then ValueiInDecimal2 = nWOPeriod.ConvertStringToDecimal(1, 1, txtActualTime.Text, False)
		Catch ex As Exception
			strMSG = strMSG + ex.Message
		End Try

		If chkIsUnderMEL.Checked = True Then

			If txtDateOfOccurrence.Text = "" Then
				strMSG = strMSG + "Date Of Occurrence required as it is " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL")
			Else

				If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Then

					If CDate(txtDateOfOccurrence.Text + " " + "00:00") > CDate(CType(mnWO.WODate.ToString, String)) Then
						strMSG = strMSG + "Date Of Occurrence should be less than Work Order Date"
					End If

				Else

					If CDate(txtDateOfOccurrence.Text) > CDate(CType(mnWO.WODate.ToString, String)) Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
							strMSG = strMSG + "Date Of Occurrence should be less than E.O. Date"
						Else
							strMSG = strMSG + "Date Of Occurrence should be less than Work Order Date"
						End If

					End If

				End If

			End If

		End If

		If IsDate(CType(mnWO.WODate.ToString, String)) Then

			If txtStartDate.Text <> "" Then

				If (AppSettings("ClientCode") = "IND" Or
					AppSettings("ClientCode") = "YA" Or
					AppSettings("ClientCode") = "AFC" Or
					AppSettings("ClientCode") = "ARA" Or
					AppSettings("ClientCode") = "BAP" Or
					AppSettings("ClientCode") = "RPS" Or
					AppSettings("ClientCode") = "GLD") Then

					'If CDate(CDate(txtStartDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
					If CDate(CDate(txtStartDate.Text) + " " + txtStartDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

						strMSG = strMSG + "Start Date should be Equal or Greater than W.O. Date."

					ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

						'If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
						If CDate(CDate(txtStartDate.Text) + " " + txtStartDateTime.Text) > CDate(CDate(txtEndDate.Text) + " " + txtEndDateTime.Text) Then
							strMSG = strMSG + "Start Date & Time cannot be Greater than End Date & Time."
						End If

					ElseIf txtStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then 'Added by Saylee

						'If CDate(txtStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then
						If CDate(CDate(txtStartDate.Text) + " " + txtStartDateTime.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then
							strMSG = strMSG + "Start Date should be Greater than W.O. Start Date."
						End If

					End If

				Else

					If CDate(txtStartDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso
						   (AppSettings("ClientCode") = "TAAL" Or
							AppSettings("ClientCode") = "GlobalJet") Then
							strMSG = strMSG + "Start Date should be Equal or Greater than E.O. Date."
						Else
							strMSG = strMSG + "Start Date should be Equal or Greater than W.O. Date."
						End If

					ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

						If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
							strMSG = strMSG + "Start Date cannot be Greater than End Date."
						End If

					ElseIf txtStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then 'Added by Saylee

						If CDate(txtStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then

							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								strMSG = strMSG + "Start Date should be Equal or Greater than E.O. Start Date."
							Else
								strMSG = strMSG + "Start Date should be Equal or Greater than W.O. Start Date."
							End If

						End If

					End If

				End If

			End If

		ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

			If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
				strMSG = strMSG + "Start Date cannot be Greater than End Date." 'mWO.GetBrokenRulesString
			Else
			End If

		ElseIf txtEndDate.Text <> "" And IsDate(CType(mnWO.WODate.ToString, String)) Then

			'If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Then
			If (AppSettings("ClientCode") = "IND" Or
				AppSettings("ClientCode") = "YA" Or
				AppSettings("ClientCode") = "AFC" Or
				AppSettings("ClientCode") = "ARA" Or
				AppSettings("ClientCode") = "BAP" Or
				AppSettings("ClientCode") = "RPS" Or
				AppSettings("ClientCode") = "GLD") Then

				'If CDate(CDate(txtEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
				If CDate(CDate(txtEndDate.Text) + " " + txtEndDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then
					strMSG = strMSG + "End Date should be Greater than W.O. Date."
				End If

			Else

				If CDate(txtEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						strMSG = strMSG + "End Date should be Greater than E.O. Date."
					Else
						strMSG = strMSG + "End Date should be Greater than W.O. Date."
					End If

				End If

			End If

		End If

		If txtEndDate.Text <> "" Then

			If IsDate(CType(mnWO.WODate.ToString, String)) Then

				'If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Then
				If (AppSettings("ClientCode") = "IND" Or
					AppSettings("ClientCode") = "YA" Or
					AppSettings("ClientCode") = "AFC" Or
					AppSettings("ClientCode") = "ARA" Or
					AppSettings("ClientCode") = "BAP" Or
					AppSettings("ClientCode") = "RPS" Or
					AppSettings("ClientCode") = "GLD") Then

					'If CDate(CDate(txtEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
					If CDate(CDate(txtEndDate.Text) + " " + txtEndDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

						strMSG = strMSG + "End Date should be Greater than W.O. Date."

					ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

						'If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
						If CDate(CDate(txtStartDate.Text) + " " + txtStartDateTime.Text) > CDate(CDate(txtEndDate.Text) + " " + txtEndDateTime.Text) Then
							strMSG = strMSG + "End Date should be Greater than Start Date."
						End If

					End If

				Else

					If CDate(txtEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso
						   (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
							strMSG = strMSG + "End Date should be Greater than E.O. Date."
						Else
							strMSG = strMSG + "End Date should be Greater than W.O. Date."
						End If

					ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

						If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
							strMSG = strMSG + "End Date should be Greater than Start Date."

						End If

					End If

				End If

			ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

				If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
					strMSG = strMSG + "End Date should be Greater than Start Date."
				End If

			End If

		End If

		'Added By Vikrant On 15-May-2014 For ALL15052014
		If Len(Trim(txtDueAsOf.Text)) > 150 Then
			strMSG = strMSG + "Due As Of must not be Greater than 150 Char."
		End If

		'Added by Shital on 31Aug2020 for STR31082020
		If Len(txtOtherJobSpecification.Text) > 50 Then
			strMSG = strMSG + "Other Job Specification must not be Greater than 50 Char."
		End If

		' commented code open because msgbox yes Broken Rules not display Ajay 21-09-2023
		If strMSG.Trim <> "" Then

			cvControlValidator.ErrorMessage = strMSG
			cvControlValidator.IsValid = False

			Return False

		End If

		Return True

	End Function

	Public Sub CustomVailidity(s As Object, e As ServerValidateEventArgs)

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

			'Added By Vikrant On 23-May-2013 For BA23052013-1	
		ElseIf custValidator.ControlToValidate = "txtTaskSourceRef" Then

			If Len(txtTaskSourceRef.Text) > 500 Then
				custValidator.ErrorMessage = "Task Source Ref. must not be Greater than 500 Char."
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
					custValidator.ErrorMessage = "Date Of Occurrence required as it is " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL")
					e.IsValid = False
				Else

					If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Then

						If CDate(txtDateOfOccurrence.Text + " " + "00:00") > CDate(CType(mnWO.WODate.ToString, String)) Then
							custValidator.ErrorMessage = "Date Of Occurrence should be less than Work Order Date"
							e.IsValid = False
						End If

					Else

						If CDate(txtDateOfOccurrence.Text) > CDate(CType(mnWO.WODate.ToString, String)) Then
							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								custValidator.ErrorMessage = "Date Of Occurrence should be less than E.O. Date"
								e.IsValid = False
							Else
								custValidator.ErrorMessage = "Date Of Occurrence should be less than Work Order Date"
								e.IsValid = False
							End If
						End If

					End If

				End If

			End If

		ElseIf custValidator.ControlToValidate = "txtStartDate" Then

			If IsDate(CType(mnWO.WODate.ToString, String)) Then

				If txtStartDate.Text <> "" Then

					'If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Then
					If (AppSettings("ClientCode") = "IND" Or
						AppSettings("ClientCode") = "YA" Or
						AppSettings("ClientCode") = "AFC" Or
						AppSettings("ClientCode") = "ARA" Or
						AppSettings("ClientCode") = "BAP" Or
						AppSettings("ClientCode") = "RPS" Or
						AppSettings("ClientCode") = "GLD") Then

						'If CDate(CDate(txtStartDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
						If CDate(CDate(txtStartDate.Text) + " " + txtStartDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

							custValidator.ErrorMessage = "Start Date should be Equal or Greater than W.O. Date."
							e.IsValid = False

						ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

							'If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
							If CDate(CDate(txtStartDate.Text) + " " + txtStartDateTime.Text) > CDate(CDate(txtEndDate.Text) + " " + txtEndDateTime.Text) Then
								custValidator.ErrorMessage = "Start Date cannot be Greater than End Date."
								e.IsValid = False
							Else
								e.IsValid = True
							End If

						ElseIf txtStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then 'Added by Saylee

							If CDate(txtStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then
								custValidator.ErrorMessage = "Start Date should be Equals or Greater W.O. Start Date."
								e.IsValid = False
							End If

						End If

					Else

						If CDate(txtStartDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								custValidator.ErrorMessage = "Start Date should be Equals or Greater than E.O. Date."
								e.IsValid = False
							Else
								custValidator.ErrorMessage = "Start Date should be Equal or Greater than W.O. Date."
								e.IsValid = False
							End If

						ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

							If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
								custValidator.ErrorMessage = "Start Date cannot be greater than End Date."
								e.IsValid = False
							Else
								e.IsValid = True
							End If

						ElseIf txtStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then 'Added by Saylee

							If CDate(txtStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then

								If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
									custValidator.ErrorMessage = "Start Date should be Equals or Greater E.O. Start Date."
									e.IsValid = False
								Else
									custValidator.ErrorMessage = "Start Date should be Equals or Greater W.O. Start Date."
									e.IsValid = False
								End If

							End If

						End If

					End If

				End If

			ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

				If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
					custValidator.ErrorMessage = "Start Date cannot be Greater than End Date."
					e.IsValid = False
				Else
					e.IsValid = True
				End If

			ElseIf txtEndDate.Text <> "" And IsDate(CType(mnWO.WODate.ToString, String)) Then

				If (AppSettings("ClientCode") = "IND" Or
					AppSettings("ClientCode") = "YA" Or
					AppSettings("ClientCode") = "AFC" Or
					AppSettings("ClientCode") = "ARA" Or
					AppSettings("ClientCode") = "BAP" Or
					AppSettings("ClientCode") = "RPS" Or
					AppSettings("ClientCode") = "GLD") Then

					'If CDate(CDate(txtEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
					If CDate(CDate(txtEndDate.Text) + " " + txtEndDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

						custValidator.ErrorMessage = "End Date should be greater than W.O. Date."
						e.IsValid = False

					Else
						e.IsValid = True
					End If

				Else

					If CDate(txtEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

						If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
							custValidator.ErrorMessage = "End Date should be greater than E.O. Date."
							e.IsValid = False
						Else
							custValidator.ErrorMessage = "End Date should be greater than W.O. Date."
							e.IsValid = False
						End If

					Else
						e.IsValid = True
					End If

				End If

			End If

		ElseIf custValidator.ControlToValidate = "txtEndDate" Then

			If txtEndDate.Text <> "" Then

				If IsDate(CType(mnWO.WODate.ToString, String)) Then

					'If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Then
					If (AppSettings("ClientCode") = "IND" Or
						AppSettings("ClientCode") = "YA" Or
						AppSettings("ClientCode") = "AFC" Or
						AppSettings("ClientCode") = "ARA" Or
						AppSettings("ClientCode") = "BAP" Or
						AppSettings("ClientCode") = "RPS" Or
						AppSettings("ClientCode") = "GLD") Then

						'If CDate(CDate(txtEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
						If CDate(CDate(txtEndDate.Text) + " " + txtEndDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

							custValidator.ErrorMessage = "End Date should be greater than W.O. Date."
							e.IsValid = False

						ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

							'If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
							If CDate(CDate(txtStartDate.Text) + " " + txtStartDateTime.Text) > CDate(CDate(txtEndDate.Text) + " " + txtEndDateTime.Text) Then
								custValidator.ErrorMessage = "End Date & Time cannot be earlier than Start Date & Time."
								e.IsValid = False
							Else

								If IsDate(mnWO.WOCloseDateFormatted.ToString) Then

									If CDate(txtEndDate.Text) > CDate(CType(mnWO.WOCloseDateFormatted.ToString, String)) Then
										custValidator.ErrorMessage = "End Date cannot be greater than Work Order Close Date."
										e.IsValid = False
									End If

								End If

							End If

						End If

					Else

						If CDate(txtEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
								custValidator.ErrorMessage = "End Date should be greater than E.O. Date."
								e.IsValid = False
							Else
								custValidator.ErrorMessage = "End Date should be greater than W.O. Date."
								e.IsValid = False
							End If

						ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

							If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
								custValidator.ErrorMessage = "End Date should be Greater than Start Date."
								e.IsValid = False
							Else

								If IsDate(mnWO.WOCloseDateFormatted.ToString) Then

									If CDate(txtEndDate.Text) > CDate(CType(mnWO.WOCloseDateFormatted.ToString, String)) Then

										If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
											custValidator.ErrorMessage = "End Date cannot be greater than E.O. Close Date."
											e.IsValid = False
										Else
											custValidator.ErrorMessage = "End Date cannot be greater than Work Order Close Date."
											e.IsValid = False
										End If

									End If

								End If

							End If

						End If

					End If

				ElseIf txtStartDate.Text <> "" And txtEndDate.Text <> "" Then

					If CDate(txtStartDate.Text) > CDate(txtEndDate.Text) Then
						custValidator.ErrorMessage = "End Date should be Greater than Start Date."
						e.IsValid = False
					Else
						e.IsValid = True
					End If

				End If

			End If
			'Added By Vikrant On 15-May-2014 For ALL15052014
		ElseIf custValidator.ControlToValidate = "txtDueAsOf" Then

			If Len(Trim(txtDueAsOf.Text)) > 150 Then
				custValidator.ErrorMessage = "Due As Of must not be greater than 150 Char."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

		ElseIf custValidator.ControlToValidate = "txtOtherJobSpecification" Then

			If Len(Trim(txtOtherJobSpecification.Text)) > 50 Then
				custValidator.ErrorMessage = "Other Job Specification must not be greater than 50 Char."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

		End If

	End Sub

	Private Sub AttachMyFile()

		Dim BackupPath As String = ""
		BackupPath = AppSettings("DOCPath") & "New.PDF"
		mnWO = Session("mnWO")

		Try

			If Not mnWO.WOJobs.CurrentItem.FileAttachments.Contains(mnWO.WOJobs.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) Then

				mnWO.WOJobs.CurrentItem.FileAttachments.Add(mnWO.WOJobs.CurrentItem.ID, CType(Session("FileUpload.FileName"), String))
				mnWO.WOJobs.CurrentItem.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
				mnWO.WOJobs.CurrentItem.FileAttachments.CurrentItem.Size = Session("Size")
				mnWO.WOJobs.CurrentItem.FileAttachments.CurrentItem.Extension = Session("Extension")

				Session("mnWO") = mnWO
				dgWOJobAttachment.DataSource = mnWO.WOJobs.CurrentItem.FileAttachments
				dgWOJobAttachment.DataBind()

				For i As Integer = 0 To mnWO.WOJobs.CurrentItem.FileAttachments.Count - 1
					Dim txtValue As TextBox
					txtValue = CType(Me.dgWOJobAttachment.Rows(i).FindControl("txtFileName"), TextBox)
					txtValue.Text = mnWO.WOJobs.CurrentItem.FileAttachments(i).FileName
				Next

				Session.Remove("Size")
				Session.Remove("ImageFile")
				Session.Remove("Extension")
				Session.Remove("FileUpload.FileName")
				upnlWOAttachment.Update()
				upnldgWOJobAttachment.Update()

			Else

				Session("mnWO") = mnWO
				MSGBoxCtrl.show(MSGBox.Message_title.Duplicate,
								MSGBox.Message_text.Duplicate,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

		Catch ex As Exception
		End Try

	End Sub

	Private Sub DeleteJobAttachment(Index As Int32)

		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem,
						MSGBox.Message_text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"RemoveAttachment")

		mnWO.WOJobs.CurrentItem.FileAttachments.CurrentIndex = Index
		Session("mnWO") = mnWO

	End Sub

	Private Sub WOJobTasksDelete(Index As Int32)
		mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentIndex = Index
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem,
						MSGBox.Message_text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"WOJobTasksDelete")
	End Sub

	Private Sub WOJobDesignationAllocations(Index As Int32)

		mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.CurrentIndex = Index
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem,
						MSGBox.Message_text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"WOJobDesignationAllocationsDelete")

	End Sub

	Private Sub WOJobSpares(Index As Int32)

		mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentIndex = Index
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem,
						MSGBox.Message_text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"WOJobSparesDelete")

	End Sub

	Private Sub WOJobComps(Index As Int32)

		mnWO.WOJobs.CurrentItem.WOJobComps.CurrentIndex = Index
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem,
						MSGBox.Message_text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"WOJobCompsDelete")

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		Dim str As String
		Try

			If control.Enabled = False Or control.Visible = False Then Exit Sub

			str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"

			ClientScript.RegisterStartupScript([GetType],
											   "focusscript",
											   str)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	'Modified by Harsh for FLYPAL-2221
	Private Sub MessageBoxResult()

		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then

			Select Case Result1

				Case MsgBoxResult.Yes

					If MSGBoxCtrl.Sender = "WOJobTasksDelete" Then                      'WO Job Tasks Delete

						Try

							Session("Sender") = ""
							mnWO.WOJobs.CurrentItem.WOJobTasks.Remove(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentIndex)
							Session("mnWOJob") = mnWO.WOJobs.CurrentItem
							ControlVisibility()
							DataFieldBind()



						Catch ex As Exception
							ex.GetBaseException()
						End Try

					ElseIf MSGBoxCtrl.Sender = "WOJobDesignationAllocationsDelete" Then 'WO Job Designation Allocations Delete

						Try

							Session("Sender") = ""
							Dim CurrentDesgAllocationEstTime As Decimal = nWOPeriod.ConvertStringToDecimal(1, 1, mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.CurrentItem.EstimatedTime, False)
							Dim WOJobEstimatedTime As Period = New Period(1, mnWO.WOJobs.CurrentItem.WOJobEstimatedTime, 1, False)

							mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.Remove(mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.CurrentIndex)

							WOJobEstimatedTime.DBValue -= CurrentDesgAllocationEstTime
							mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = WOJobEstimatedTime.Value
							Session("mnWOJob") = mnWO.WOJobs.CurrentItem
							ControlVisibility()

							DataFieldBind()

							upnlJobCompletionDetails.Update()

						Catch ex As Exception
							ex.GetBaseException()
						End Try

					ElseIf MSGBoxCtrl.Sender = "WOJobSparesDelete" Then              'WO Job Spares Delete

						If mnWO.WOJobs.CurrentItem.WOJobSpares.Item(mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentIndex).WOIssuedSparesCount > 0 Then

							MSGBoxCtrl.Show("Alert!",
											"You cannot remove this record, as Issue against this part has been already done!",
											"",
											MsgBoxStyle.OkOnly,
											"")
							Exit Sub

						End If

						Try

							Session("Sender") = ""
							mnWO.WOJobs.CurrentItem.WOJobSpares.Remove(mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentIndex)
							Session("mnWOJob") = mnWO.WOJobs.CurrentItem
							ControlVisibility()

							DataFieldBind()


						Catch ex As Exception
							ex.GetBaseException()
						End Try

					ElseIf MSGBoxCtrl.Sender = "WOJobCompsDelete" Then                 'WO Job Comps Delete

						Try

							Session("Sender") = ""
							mnWO.WOJobs.CurrentItem.WOJobComps.Remove(mnWO.WOJobs.CurrentItem.WOJobComps.CurrentIndex)
							Session("mnWOJob") = mnWO.WOJobs.CurrentItem
							ControlVisibility()

							DataFieldBind()


						Catch ex As Exception
							ex.GetBaseException()
						End Try

					ElseIf MSGBoxCtrl.Sender = "Close" Or MSGBoxCtrl.Sender = "Save" Then  '' Close confirmation, Completion Confirmation

						Session("sender") = ""

						If Not CustomValidationAndWO() Then upnlValidationSummary.Update() : Exit Sub

						If Session("wfProject_Ajax") = "wfProject_Ajax" Then
							Session("OpenFromProject") = Nothing
							Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("TransTypeID").ToString
						End If

						If mnWO.WOJobs.CurrentItem.IsValid = True Then

							Session.Remove("IsValid")

							If Save() Then

								If Not CustomValidationAndWO() Then upnlValidationSummary.Update() : Exit Sub
								If Not mnWO.IsValid Then upnlValidationSummary.Update() : Exit Sub

								'Here if All Jobs are "Cancelled" then WO gets Cancelled automatically"
								If mnWO.WOJobs.IsALLJobsCancelled Then
									mnWO.StatusID = 4
								End If

								mnWO.Save()
								SaveDigitalAttachment()

								If mFileJobAttach IsNot Nothing Then SaveAttachment()

								'Added By Prashant On 16-May-2024
								If mProject IsNot Nothing Then

									If (mnWO.WorkOrderCountInProject = 0 And Not mnWO.ProjectID.Equals(Guid.Empty)) Then
										mProject.Save()
									End If

								End If

								'End of Added By Prashant On 16-May-2024
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
								ControlVisibility()

								'Added By Saylee On 27-Dec-2018 
								Session.Remove("mFileAttach")
								Session.Remove("IsAttachmentDeleted")
								'End
								Session.Remove("ActiveJobDetailsTabIndex")
								Session.Remove("ToDisbleJobControlsAsCompletedRightNotGiven") 'Added By Vikrant on 30-Jun-2021 For ALL30062021

								If mnWO.StatusID = 2 And
								   (mnWO.WOJobs.Count + mnWO.WONRCJobs.Count = mnWO.CompletedJobsCount) And
								   AppSettings("ShowNewWOFlow") = "True" Then  'Added By Prashant 16-Aug-2019" Then

									'Sending Email on completing NRC while saving details.
									If cmbWOStatusList.SelectedIndex = 1 AndAlso
									   chkAddToWatchList.Checked Then

										SendMail()

									End If

									ScriptManager.RegisterClientScriptBlock(Me,
																			[GetType],
																			"OpenToAddWODetail",
																			"OpenToAddWODetail();",
																			True)

									Exit Sub

								ElseIf Request.QueryString("BackPage1") = "index.aspx" Then 'Added By Prashant 8-Dec-2010

									'Sending Email on completing NRC while saving details.
									If cmbWOStatusList.SelectedIndex = 1 AndAlso
									   chkAddToWatchList.Checked Then

										SendMail()

									End If

									Response.Redirect("index.aspx")

								Else

									'Sending Email on completing NRC while saving details.
									If cmbWOStatusList.SelectedIndex = 1 AndAlso
									   chkAddToWatchList.Checked Then

										SendMail()

									End If

									Response.Redirect("wfnWODetail_AJAX.aspx")
									DataFieldBind()

								End If

							End If

						Else

							Session.Remove("IsValid")
							ControlVisibility()
							DataFieldBind()
							CallUpdatePanels()

						End If

					ElseIf MSGBoxCtrl.Sender = "WOStatus" Then

						'Added By Saylee On 4-Mar-2020 For Approval Reject history
						If AppSettings("ShowNewWOFlow") = "True" Then

							Try

								If Not mnWO.IsValid Then upnlValidationSummary.Update() : Exit Sub

								Dim mnWOApproveReject As nWOApproveReject
								mnWOApproveReject = Session("mnWOApproveReject")

								If mnWOApproveReject IsNot Nothing Then

									Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
									Dim mEmployee As Employee
									If Not mUser.EmployeeID.Equals(Guid.Empty) Then
										mEmployee = Employee.GetEmployee(mUser.EmployeeID)
										mnWOApproveReject.DoneBy = mEmployee.Name
									End If

									mnWOApproveReject.Save()
									mnWO.Save()

								End If

								Session.Remove("mnWOApproveReject")
								mnWO.Save()
								Session("mnWO") = mnWO
								mWODetail = "Rejected at Execution Stage " & mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Rejected By : " + mnWOApproveReject.DoneBy
								MarkLog(Action.Save, "Work Order", mWODetail, ErrorType.NoError, mnWO.ID, EventLogID)
								Session.Remove("mFileAttach")
								Session.Remove("IsAttachmentDeleted")
								Session.Remove("ActiveJobDetailsTabIndex")
								Response.Redirect("index.aspx")

							Catch ex As Exception

							End Try

						End If

						'Added By Vikrant For WO NRC
					ElseIf MSGBoxCtrl.Sender = "RemoveWOJobNRC" Then

						Dim mFileAttachments As New FileAttachments
						Dim index As Integer = CType(Session("JobNRCIndex"), Integer)

						Session.Remove("JobNRCIndex")

						nWOJob.DeleteWOJobNRC(mWOJobNRCList(index).ID)
						mFileAttachments.DeleteAllByRefID(mWOJobNRCList(index).ID)

						mWOJobNRCList = WOJobNRCList.GetWOJobNRCList(mnWO.ID, mnWO.WOJobs.CurrentItem.ID)

						Session("mWOJobNRCList") = mWOJobNRCList

						'End

					ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then

						Try

							Session("Sender") = ""
							Dim mnWO As nWO
							mnWO = CType(Session("mnWO"), nWO)
							mnWO.WOJobs.CurrentItem.FileAttachments.Remove(mnWO.WOJobs.CurrentItem.FileAttachments.CurrentItem)
							dgWOJobAttachment.DataSource = mnWO.WOJobs.CurrentItem.FileAttachments
							dgWOJobAttachment.DataBind()
							upnldgWOJobAttachment.Update()
							upnlWOAttachment.Update()
							Session("mnWO") = mnWO

						Catch ex As SqlException

							If ex.Number = 8145 Then

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

								MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
												MSGBox.Message_Text.ReferenceDelete,
												ex.Procedure,
												MsgBoxStyle.OkOnly, "")

							End If

						End Try

					End If

					If MSGBoxCtrl.Sender = "DeleteCompletionDetail" Then

						Try

							mnWO.WOJobs.CurrentItem.WOJobActions.Remove(mnWO.WOJobs.CurrentItem.WOJobActions.CurrentIndex)

							DataFieldBind()

							'Dumping the values of Last record from the grid into the controls
							Dumping_CompletionDetailValues()

							Session("mnWO") = mnWO

							upnlCompletionDetailsList.Update()
							upnlJobCompletionDetails.Update()

						Catch ex As Exception
							ex.GetBaseException()
						End Try

					End If

				Case MsgBoxResult.No

					If MSGBoxCtrl.Sender = "Close" Then

						If Session("Edit") = True Then
							mnWO = IIf(Session("mnWOClone") Is Nothing, mnWO, Session("mnWOClone"))
						End If

						Session("mnWO") = mnWO
						Session.Remove("IsValid")
						Session("Sender") = ""
						Session.Remove("Edit")
						Session.Remove("mnWOClone")
						Session.Remove("ActiveJobDetailsTabIndex")

						If mnWO.WOJobs.CurrentItem.IsNew And (mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 Or mnWO.WOJobs.CurrentItem.WOJobTypeID = 7) Then
							mnWO.WOJobs.Remove(mnWO.WOJobs.CurrentItem)
						End If

						If Session("wfProject_Ajax") = "wfProject_Ajax" Then
							Session("OpenFromProject") = Nothing
							Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("TransTypeID").ToString
						End If
						Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
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


					ElseIf MSGBoxCtrl.Sender = "RemoveWOJobNRC" Then

						'Do Nothing
						Session.Remove("JobNRCIndex")
						'End

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

		Try

			txtEstimatedTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtEstimatedTime').value,event)")
			txtActualTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtActualTime').value,event)")
			txtFrequencyInDay.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInDay').value,event)")
			txtFrequencyInHours.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtFrequencyInHours').value,event)")
			txtCompletionDetailActualTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtCompletionDetailActualTime').value,event)")

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Function Save() As Boolean

		'Added by shital on 21-May-2020
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
		   (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
							MSGBox.Message_Text.Authorization,
							"",
							MsgBoxStyle.OkOnly,
							"Authorization")
			Exit Function

		End If
		'---------

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

	Private Sub DeleteWOJobNRC(Index As Int32)

		MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem,
						MSGBox.Message_Text.RemoveItem,
						"",
						MsgBoxStyle.YesNo,
						"RemoveWOJobNRC")

		Session("JobNRCIndex") = Index

	End Sub
	'End

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

	Public Sub SaveDigitalAttachment()
		Dim mFileAttachnWO As FileAttach
		If mnWO.IsDigitalSignatureAdded = True Then
			mFileAttachnWO = Session("mFileAttachnWO")
			If mFileAttachnWO IsNot Nothing Then
				If mFileAttachnWO.Size > 0 Then
					Try
						mFileAttachnWO.Save()
					Catch ex As Exception
						ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
					End Try
				End If
			End If
		End If
	End Sub

	'Added by Harsh Sugandhi on 24th July 2024 for FLYPAL-1771 Facility to add Multiple Job Actions W.O. Job
	Public Sub CustomValidation_CompletionDetail(sender As Object, e As ServerValidateEventArgs)

		Dim customValidator As CustomValidator
		customValidator = CType(sender, CustomValidator)

		Try

			If customValidator.ControlToValidate = "txtCompletionDetailStartDate" Then

				If IsDate(CType(mnWO.WODate.ToString, String)) Then

					If txtCompletionDetailStartDate.Text <> "" Then

						If (AppSettings("ClientCode") = "IND" Or
							AppSettings("ClientCode") = "YA" Or
							AppSettings("ClientCode") = "AFC" Or
							AppSettings("ClientCode") = "ARA" Or
							AppSettings("ClientCode") = "BAP" Or
							AppSettings("ClientCode") = "GLD") Then

							'If CDate(CDate(txtCompletionDetailStartDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
							If CDate(CDate(txtCompletionDetailStartDate.Text) + " " + txtCompletionDetailStartDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

								customValidator.ErrorMessage = "Start Date should be Equal or Greater than W.O. Date."
								e.IsValid = False

							ElseIf txtCompletionDetailStartDate.Text <> "" And txtCompletionDetailEndDate.Text <> "" Then

								'If CDate(txtCompletionDetailStartDate.Text) > CDate(txtCompletionDetailEndDate.Text) Then
								If CDate(CDate(txtCompletionDetailStartDate.Text) + " " + txtCompletionDetailStartDateTime.Text) > CDate(CDate(txtCompletionDetailEndDate.Text) + " " + txtCompletionDetailEndDateTime.Text) Then

									customValidator.ErrorMessage = "Start Date & Time cannot be Greater than End Date & Time."
									e.IsValid = False

								Else
									e.IsValid = True
								End If

							ElseIf txtCompletionDetailStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then

								'If CDate(txtCompletionDetailStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then
								If CDate(CDate(txtCompletionDetailStartDate.Text) + " " + txtCompletionDetailStartDateTime.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then

									customValidator.ErrorMessage = "Start Date should be Equals or Greater W.O. Start Date."
									e.IsValid = False

								End If

							End If

						Else

							If CDate(txtCompletionDetailStartDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

								If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

									customValidator.ErrorMessage = "Start Date should be Equals or Greater E.O. Date."
									e.IsValid = False

								Else

									customValidator.ErrorMessage = "Start Date should be Equals or Greater W.O. Date."
									e.IsValid = False

								End If

							ElseIf txtCompletionDetailStartDate.Text <> "" And txtCompletionDetailEndDate.Text <> "" Then

								If CDate(txtCompletionDetailStartDate.Text) > CDate(txtCompletionDetailEndDate.Text) Then

									customValidator.ErrorMessage = "Start Date cannot be greater than End Date."
									e.IsValid = False

								Else
									e.IsValid = True
								End If

							ElseIf txtCompletionDetailStartDate.Text <> "" And IsDate(CType(mnWO.WOStartDate.ToString, String)) Then

								If CDate(txtCompletionDetailStartDate.Text) < CDate(CType(mnWO.WOStartDate.ToString, String)) Then

									If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

										customValidator.ErrorMessage = "Start Date should be Equals or Greater E.O. Start Date."
										e.IsValid = False

									Else

										customValidator.ErrorMessage = "Start Date should be Equals or Greater W.O. Start Date."
										e.IsValid = False

									End If

								End If

							End If

						End If

					End If

				ElseIf txtCompletionDetailStartDate.Text <> "" And txtCompletionDetailEndDate.Text <> "" Then

					'If CDate(txtCompletionDetailStartDate.Text) > CDate(txtCompletionDetailEndDate.Text) Then
					If CDate(CDate(txtCompletionDetailStartDate.Text) + " " + txtCompletionDetailStartDateTime.Text) < CDate(CDate(txtCompletionDetailEndDate.Text) + " " + txtCompletionDetailEndDateTime.Text) Then

						customValidator.ErrorMessage = "Start Date cannot be greater than End Date."
						e.IsValid = False

					Else
						e.IsValid = True
					End If

				ElseIf txtCompletionDetailEndDate.Text <> "" And IsDate(CType(mnWO.WODate.ToString, String)) Then

					'If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Then
					If (AppSettings("ClientCode") = "IND" Or
						AppSettings("ClientCode") = "YA" Or
						AppSettings("ClientCode") = "AFC" Or
						AppSettings("ClientCode") = "ARA" Or
						AppSettings("ClientCode") = "BAP" Or
						AppSettings("ClientCode") = "GLD") Then

						'If CDate(CDate(txtCompletionDetailEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
						If CDate(CDate(txtCompletionDetailStartDate.Text) + " " + txtCompletionDetailStartDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

							customValidator.ErrorMessage = "End Date should be greater than W.O. Date."
							e.IsValid = False

						Else
							e.IsValid = True
						End If

					Else

						If CDate(txtCompletionDetailEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

							If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then

								customValidator.ErrorMessage = "End Date should be greater than E.O. Date."
								e.IsValid = False

							Else

								customValidator.ErrorMessage = "End Date should be greater than W.O. Date."
								e.IsValid = False

							End If

						Else
							e.IsValid = True
						End If

					End If

				End If

			ElseIf customValidator.ControlToValidate = "txtCompletionDetailEndDate" Then

				If txtCompletionDetailEndDate.Text <> "" Then

					If IsDate(CType(mnWO.WODate.ToString, String)) Then

						'If AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND" Then
						If (AppSettings("ClientCode") = "IND" Or
							AppSettings("ClientCode") = "YA" Or
							AppSettings("ClientCode") = "AFC" Or
							AppSettings("ClientCode") = "ARA" Or
							AppSettings("ClientCode") = "BAP" Or
							AppSettings("ClientCode") = "GLD") Then

							'If CDate(CDate(txtCompletionDetailEndDate.Text) + " " + "23:59") < CDate(CType(mnWO.WODate.ToString, String)) Then
							If CDate(CDate(txtCompletionDetailEndDate.Text) + " " + txtCompletionDetailEndDateTime.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

								customValidator.ErrorMessage = "End Date should be Greater than W.O. Date."
								e.IsValid = False

							ElseIf txtCompletionDetailStartDate.Text <> "" And txtCompletionDetailEndDate.Text <> "" Then

								'If CDate(txtCompletionDetailStartDate.Text) > CDate(txtCompletionDetailEndDate.Text) Then
								If CDate(CDate(txtCompletionDetailStartDate.Text) + " " + txtCompletionDetailStartDateTime.Text) > CDate(CDate(txtCompletionDetailEndDate.Text) + " " + txtCompletionDetailEndDateTime.Text) Then

									customValidator.ErrorMessage = "End Date cannot be Smaller than Start Date."
									e.IsValid = False

								Else

									If IsDate(mnWO.WOCloseDateFormatted.ToString) Then

										If CDate(txtCompletionDetailEndDate.Text) > CDate(CType(mnWO.WOCloseDateFormatted.ToString, String)) Then

											customValidator.ErrorMessage = "End Date cannot be Greater than W.O. Close Date."
											e.IsValid = False

										End If

									End If

								End If

							End If

						Else

							If CDate(txtCompletionDetailEndDate.Text) < CDate(CType(mnWO.WODate.ToString, String)) Then

								If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
									customValidator.ErrorMessage = "End Date should be Greater than E.O. Date."
									e.IsValid = False
								Else
									customValidator.ErrorMessage = "End Date should be Greater than W.O. Date."
									e.IsValid = False
								End If

							ElseIf txtCompletionDetailStartDate.Text <> "" And txtCompletionDetailEndDate.Text <> "" Then

								If CDate(txtCompletionDetailStartDate.Text) > CDate(txtCompletionDetailEndDate.Text) Then
									customValidator.ErrorMessage = "End Date should be Greater than Start Date."
									e.IsValid = False
								Else

									If IsDate(mnWO.WOCloseDateFormatted.ToString) Then

										If CDate(txtCompletionDetailEndDate.Text) > CDate(CType(mnWO.WOCloseDateFormatted.ToString, String)) Then

											If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
												customValidator.ErrorMessage = "End Date cannot be Greater than E.O. Close Date."
												e.IsValid = False
											Else
												customValidator.ErrorMessage = "End Date cannot be Greater than W.O. Close Date."
												e.IsValid = False
											End If

										End If

									End If

								End If

							End If

						End If

					ElseIf txtCompletionDetailStartDate.Text <> "" And txtCompletionDetailEndDate.Text <> "" Then

						If CDate(txtCompletionDetailStartDate.Text) > CDate(txtCompletionDetailEndDate.Text) Then
							customValidator.ErrorMessage = "End Date cannot be Smaller than Start Date."
							e.IsValid = False
						Else
							e.IsValid = True
						End If

					End If

				End If

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Function DateTimeValidation_CompletionDetail() As Boolean

		Dim ErrorMessage As String = String.Empty
		Try

			If txtCompletionDetailStartDateTime.Text <> "00:00" And txtCompletionDetailEndDateTime.Text = "00:00" Then

				ErrorMessage = "Enter End Date Time."
				cvCompletionDetailsEndDateTime.ErrorMessage = ErrorMessage
				cvCompletionDetailsEndDateTime.IsValid = False

			End If

			If txtCompletionDetailStartDateTime.Text = "00:00" And txtCompletionDetailEndDateTime.Text <> "00:00" Then

				ErrorMessage = "Enter Start Date Time."
				cvCompletionDetailsStartDateTime.ErrorMessage = ErrorMessage
				cvCompletionDetailsStartDateTime.IsValid = False

			End If

			If ErrorMessage.Trim <> "" Then
				Return False
			End If

			Return True

		Catch ex As Exception
			ex.GetBaseException()
			Return False
		End Try

	End Function

	Public Sub SetObject_CompletionDetail()

		Try

			If txtCompletionDetailStartDate.Text.ToString <> "" Then

				If txtCompletionDetailStartDateTime.Text.ToString <> "" Then
					mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.StartDate = CType(txtCompletionDetailStartDate.Text.ToString.Trim + " " +
																					   txtCompletionDetailStartDateTime.Text.ToString.Trim, DateTime)
				Else
					mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.StartDate = txtCompletionDetailStartDate.Text
				End If

			Else
				mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.StartDate = DBNull.Value
			End If

			If txtCompletionDetailEndDate.Text.ToString <> "" Then

				If txtCompletionDetailEndDateTime.Text.ToString <> "" Then
					mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.CloseDate = CType(txtCompletionDetailEndDate.Text.ToString.Trim + " " +
																					   txtCompletionDetailEndDateTime.Text.ToString.Trim, DateTime)
				Else
					mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.CloseDate = txtCompletionDetailEndDate.Text
				End If

			Else
				mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.CloseDate = DBNull.Value
			End If

			mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.Action = txtCompletionDetailAction.Text
			mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.ActualTime = txtCompletionDetailActualTime.Text

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub SetControls_CompletionDetail()

		Try

			txtCompletionDetailEmployee.Text = mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.EmployeeName.ToString
			txtCompletionDetailStartDate.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.StartDateFormatted),
													   AppSettings("DateFormat"))
			txtCompletionDetailStartDateTime.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.StartDateFormatted),
														   AppSettings("TimeFormat"))
			txtCompletionDetailEndDate.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.CloseDateFormatted),
													 AppSettings("DateFormat"))
			txtCompletionDetailEndDateTime.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.CloseDateFormatted),
														 AppSettings("TimeFormat"))
			txtCompletionDetailAction.Text = mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.Action.ToString
			txtCompletionDetailActualTime.Text = mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.ActualTime.ToString

			CompletionDetailControls_DataBind()

			upnlCompletionDetails.Update()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub ClearControls_CompletionDetail()

		Try

			txtCompletionDetailStartDate.Text = ""
			txtCompletionDetailStartDateTime.Text = ""
			txtCompletionDetailEndDate.Text = ""
			txtCompletionDetailEndDateTime.Text = ""
			txtCompletionDetailAction.Text = ""
			txtCompletionDetailActualTime.Text = ""

			DataBind()
			upnlCompletionDetails.Update()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub Dumping_CompletionDetailValues()

		Try

			If mnWO.WOJobs.CurrentItem.WOJobActions.Count > 0 Then

				txtStartDate.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobActions(0).StartDateFormatted),
										   AppSettings("DateFormat"))
				txtStartDateTime.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobActions(0).StartDateFormatted),
											   AppSettings("TimeFormat"))
				txtEndDate.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobActions(mnWO.WOJobs.CurrentItem.WOJobActions.Count - 1).CloseDateFormatted),
										 AppSettings("DateFormat"))
				txtEndDateTime.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobActions(mnWO.WOJobs.CurrentItem.WOJobActions.Count - 1).CloseDateFormatted),
											 AppSettings("TimeFormat"))
				txtWOJobAction.Text = mnWO.WOJobs.CurrentItem.WOJobActions(mnWO.WOJobs.CurrentItem.WOJobActions.Count - 1).Action
				txtActualTime.Text = mnWO.WOJobs.CurrentItem.TotalActionManHrs

				mnWO.WOJobs.CurrentItem.WOJobActualTime = mnWO.WOJobs.CurrentItem.TotalActionManHrs
				mnWO.WOJobs.CurrentItem.WOJobAction = mnWO.WOJobs.CurrentItem.WOJobActions(mnWO.WOJobs.CurrentItem.WOJobActions.Count - 1).Action

			Else

				txtStartDate.Text = ""
				txtStartDateTime.Text = ""
				txtEndDate.Text = ""
				txtEndDateTime.Text = ""
				txtWOJobAction.Text = ""
				txtActualTime.Text = ""
				txtMethodOfCompliance.Text = "" 'Sankalp 08-08-25
				mnWO.WOJobs.CurrentItem.WOJobActualTime = ""
				mnWO.WOJobs.CurrentItem.WOJobAction = ""

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub
	'End

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
			EmailBody += $"<b> Description : </b> {mnWO.WOJobs.CurrentItem.WOJobDescription} "
			EmailBody += "</font></p>"
			EmailBody += "<p><font face=""Calibri"">"
			EmailBody += $"<b> Watchlist Instructions : </b>  {mnWO.WOJobs.CurrentItem.WatchListInstructions} "
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

		dgWOJobAttachment.DataSource = mnWO.WOJobs.CurrentItem.FileAttachments

		If mnWO.WOJobs.CurrentItem IsNot Nothing Then

			If mnWO.WOJobs.CurrentItem.WOJobStartDate.ToString = "" Then
				txtStartDate.Text = ""
				txtStartDateTime.Text = ""
			Else
				txtStartDate.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobStartDateFormatted), AppSettings("DateFormat"))
				txtStartDateTime.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobStartDateFormatted), AppSettings("TimeFormat"))
			End If

			If mnWO.WOJobs.CurrentItem.WOJobCloseDate.ToString = "" Then
				txtEndDate.Text = ""
				txtEndDateTime.Text = ""
			Else
				txtEndDate.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobCloseDateFormatted), AppSettings("DateFormat"))
				txtEndDateTime.Text = Format(CDate(mnWO.WOJobs.CurrentItem.WOJobCloseDateFormatted), AppSettings("TimeFormat"))
			End If

			txtDateOfOccurrence.Text = IIf(mnWO.WOJobs.CurrentItem.DateOfOccurrence.ToString = "", "", mnWO.WOJobs.CurrentItem.DateOfOccurrenceFormatted) '---Added By Utkarsh On 18-Jan-2011
			txtRevDate.Text = IIf(mnWO.WOJobs.CurrentItem.AMPRevDate.ToString = "", "", mnWO.WOJobs.CurrentItem.AMPRevDateFormatted)

		End If

		'Added By Vikrant For WO NRC
		mWOJobNRCList = WOJobNRCList.GetWOJobNRCList(mnWO.ID, mnWO.WOJobs.CurrentItem.ID)
		Session("mWOJobNRCList") = mWOJobNRCList
		'End

		mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForWO(WOID:=mnWO.ID,
																			IsForWO:=True,
																			TransactionDate:=mnWO.WODateFormatted.ToString)
		Session("mRequisitionItemsNew") = mRequisitionItemsNew

		mMPDSkillList = MPDSkillList.GetSkillList(True)
		cmbSkillcode.DataSource = mMPDSkillList
		Session("mMPDSkillList") = mMPDSkillList

		gvCompletionDetails.DataSource = mnWO.WOJobs.CurrentItem.WOJobActions

		DataBind()

	End Sub

	Private Sub CompletionDetailControls_DataBind()

		Try

			txtCompletionDetailEmployee.DataBind()
			txtCompletionDetailStartDate.DataBind()
			txtCompletionDetailStartDateTime.DataBind()
			txtCompletionDetailEndDate.DataBind()
			txtCompletionDetailEndDateTime.DataBind()
			txtCompletionDetailAction.DataBind()
			txtCompletionDetailActualTime.DataBind()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		'Put user code to initialize the page here
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		AddAttributes()

		If Not Page.IsPostBack Then

			If txtWOJobDescription.Enabled = True Then
				SetFocus(txtWOJobDescription)
			End If

			DataFieldBind()
			GetAttachment()

			If CType(Session("ActiveJobDetailsTabIndex"), Integer) > 0 Then

				If Session("ActiveJobDetailsTabIndex") IsNot Nothing Then WOJobDetailsTabContainer.ActiveTabIndex = CType(Session("ActiveJobDetailsTabIndex"), Integer) : Session.Remove("ActiveJobDetailsTabIndex")
				Call WOJobDetailsActiveTabChanged(Nothing, Nothing)

			Else
				WOJobDetailsTabContainer.ActiveTabIndex = 0
			End If

		Else

		End If

		ControlVisibility()

	End Sub

	Private Sub SaveJobDetails(sender As Object, e As EventArgs) Handles btnSave.Click

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or
		   (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then

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
		If Not CustomValidateJob() Then upnlValidationSummary.Update() : Exit Sub
		If Not CustomValidationAndWO() Then upnlValidationSummary.Update() : Exit Sub
		If Not mnWO.IsValid Then upnlValidationSummary.Update() : Exit Sub

		If mnWO.WOJobs.CurrentItem.IsDirty Then

			If mnWO.WOJobs.CurrentItem.WOJobStatusID <> 1 Then

				Session("IsValid") = "True"
				MSGBoxCtrl.Show("Confirmation!",
								"Do you want to " + mnWO.WOJobs.CurrentItem.WOJobStatusName + " this job?",
								"",
								MsgBoxStyle.YesNo,
								"Save")

				Exit Sub

			Else
				'Dim _IsNewStatus As Boolean = False
				Try

					mnWO.Save()

					SaveDigitalAttachment()

					If mFileJobAttach IsNot Nothing Then SaveAttachment()

					'Added By Prashant On 16-May-2024
					If mProject IsNot Nothing Then

						If (mnWO.WorkOrderCountInProject = 0 And Not mnWO.ProjectID.Equals(Guid.Empty)) Then

							mProject.Save()
							Session("mProject") = mProject

						End If

					End If
					'End of Added By Prashant On 16-May-2024

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
					upnlButtons.Update()
					up.Update()

					If Request.QueryString("BackPage1") = "index.aspx" Or
					   Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then 'Added By Prashant 8-Dec-2010

						Response.Redirect("index.aspx")
					Else

						MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
										MSGBox.Message_Text.SavedSuccessFully,
										"",
										MsgBoxStyle.OkOnly,
										"")

					End If

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

				Catch ex As Exception

					MSGBoxCtrl.Show(MSGBox.Message_Title.ErrorMessage,
									MSGBox.Message_Text.ErrorMessage,
									"Error Occurred while trying to Save the Job.",
									MsgBoxStyle.OkOnly,
									"")

				End Try

			End If

		End If

	End Sub

	Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
		ViewImage()
	End Sub

	Private Sub MELCategory_Changed(sender As Object, e As EventArgs) Handles cmbMELCategory.SelectedIndexChanged
		mnWO.WOJobs.CurrentItem.MELCategoryID = cmbMELCategory.SelectedValue
		txtFrequencyInDay.Text = mnWO.WOJobs.CurrentItem.FrequencyInDays

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
		SetFocus(cmbMELCategory)
	End Sub

	Private Sub DelAttach(sender As Object, e As ImageClickEventArgs) Handles imgDelAttach1.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte

		GetAttachment()
		'mEmployee.ImageFile = file1
		'mEmployee.ImageSize = 0
		mFileJobAttach.ImageFile = file1
		mFileJobAttach.Size = 0
		mnWO.WOJobs.CurrentItem.IsAttachmentAdded = False
		IsAttachmentDeleted = True
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
		Session("mFileAttach") = mFileJobAttach
		ImageButton1.Visible = False
		imgDelAttach1.Enabled = False
		Session("mnWOJob") = mnWO.WOJobs.CurrentItem
		ControlVisibilityForAttachment()
		upnlAttach.Update()
	End Sub

	Private Sub IsUnderMEL_Changed(sender As Object, e As EventArgs) Handles chkIsUnderMEL.CheckedChanged
		If chkIsUnderMEL.Checked = True Then
			pnlMEL.Visible = True
			'lblMEL.Visible = True
			txtDateOfOccurrence.Text = mnWO.WODateFormatted.ToString

			txtFrequencyInHours.Text = ""
			If cmbMELCategory.SelectedIndex = 1 Then
				txtFrequencyInDay.Enabled = True
			End If
			txtFrequencyInHours.Enabled = False
		Else
			pnlMEL.Visible = False
			ClearControl()
		End If
	End Sub

	Private Sub WOStatusChanged(sender As Object, e As EventArgs) Handles cmbWOStatusList.SelectedIndexChanged

		Try

			If cmbWOStatusList.SelectedValue = 2 Then 'Complete

				lblStarStartDate.Visible = True
				lblStarEndDate.Visible = True
				phWatchListDetails.Visible = IIf(AppSettings("ShowMaintenanceForNewClients").ToString.Equals("True", StringComparison.InvariantCultureIgnoreCase) AndAlso
												 Not mnWO.MachineID.Equals(Guid.Empty),
												 True,
												 False)

				If Session("OpenFromWOJobListToCompleteForm") = "True" Then 'Added By Prashant On 11-Jul-2023
					lblStarAction.Visible = True
				End If

				'Added By Vikrant For WO NRC
				If mWOJobNRCList IsNot Nothing Then

					If mWOJobNRCList.Count > 0 Then

						For i As Integer = 0 To mWOJobNRCList.Count - 1

							If mWOJobNRCList(i).WOJobStatusID < 2 Then

								cmbWOStatusList.DataSource = mnWOJobStatusList
								cmbWOStatusList.DataBind()
								MSGBoxCtrl.Show("Alert!",
												"WO job status can not be Complete.",
												"As all Jobs " +
													IIf(AppSettings("ClientCode") = "IND", "OJS", "NRC") +
													" are not completed yet.You can not complete WO job",
												MsgBoxStyle.OkOnly,
												"")
								Exit Sub

							End If

						Next

					End If

				End If
				'End

			Else

				lblStarStartDate.Visible = False
				lblStarEndDate.Visible = False
				lblStarAction.Visible = False
				phWatchListDetails.Visible = False

			End If

			If cmbWOStatusList.SelectedValue = 1 Then
				btnReject.Enabled = True
			Else
				btnReject.Enabled = False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If Not IsInRole(Rights.Print) Then

			SetSession()
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
							MSGBox.Message_Text.Authorization,
							"",
							MsgBoxStyle.OkOnly,
							"Authorization")

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

		Dim myReport As ReportClass
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			myReport = New crnWOJobDetailTAAL
		Else
			myReport = New crnWOJobDetail
		End If

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
					   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
					   mCompanyDetail.WebSite, "Job Details", SearchStr1, "", "", AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "",
					   SearchStr9:=AppSettings("ClientCode"), SearchStr10:=AppSettings("Logo"))

		mnWO = Session("mnWO")

		mnWOJob = mnWO.WOJobs.CurrentItem
		mnWOTools = mnWO.WOTools
		mnWOPeriods = mnWO.WOPeriods

		mnWOJobTasks = mnWO.WOJobs.CurrentItem.WOJobTasks
		mnrptWOJobResourceDetails = nrptWOJobResourceDetails.GetrptWOJobResourceDetails(mnWO.WOJobs.CurrentItem.ID.ToString)
		mnWOJobSpares = mnWO.WOJobs.CurrentItem.WOJobSpares
		mnWOJobComps = mnWO.WOJobs.CurrentItem.WOJobComps

		da.Fill(ds, mnWO)
		da.Fill(ds, mnWOJob)
		da.Fill(ds, mnWOTools)
		da.Fill(ds, mnWOPeriods)
		da.Fill(ds, mnWOJobTasks)
		da.Fill(ds, mnrptWOJobResourceDetails)
		da.Fill(ds, mnWOJobSpares)
		da.Fill(ds, mnWOJobComps)
		da.Fill(ds, Report)
		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, mrptImage)
		myReport.SetDataSource(ds)
		Session("CrystalReport") = myReport

		Dim Str As String
		Str = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", Str, True)

	End Sub

	Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
		SetObject()
		If mnWO.WOJobs.CurrentItem.IsDirty Then
			Session("IsValid") = "True"
			MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm, MSGBox.Message_Text.Save, "", MsgBoxStyle.YesNo, "Close")
		Else
			'SetObject()
			'Added By Saylee On 27-Dec-2018 
			Session.Remove("mFileAttach")
			Session.Remove("IsAttachmentDeleted")
			'End
			Session.Remove("ActiveJobDetailsTabIndex")
			Session.Remove("ToDisbleJobControlsAsCompletedRightNotGiven") 'Added By Vikrant on 30-Jun-2021 For ALL30062021 
			'Session.Remove("OpenFromWOJobListToCompleteForm")  'Added By Prashant On 11-Jul-2023
			If mnWO.WOJobs.CurrentItem.IsNew And (mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 Or mnWO.WOJobs.CurrentItem.WOJobTypeID = 7) Then
				mnWO.WOJobs.Remove(mnWO.WOJobs.CurrentItem)
			End If
			If Session("wfProject_Ajax") = "wfProject_Ajax" Then
				Session("OpenFromProject") = Nothing
				Session("MiddleFrame") = "wfProjectList_Ajax.aspx?TransTypeID=" & Session("TransTypeID").ToString
			End If
			Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub

	Private Sub chkShowMEL_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowMEL.CheckedChanged
		cmbComponent.Items.Clear()

		If chkShowMEL.Checked = True Then
			pnlMELCategory.Visible = True
			mMELSnagPartList = Nothing
			mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateOfOccurrence.Text, mnWO.MachineID.ToString, "(SELECT)")
			cmbComponent.DataSource = mMELSnagPartList
			Session("mMELSnagPartList") = mMELSnagPartList
			If Not mMELSnagPartList.Contains(mnWO.WOJobs.CurrentItem.CompID) Then mnWO.WOJobs.CurrentItem.CompID = Guid.Empty
			'cmbComponent.SelectedIndex = 0
			cmbComponent.DataBind()
			'cmbATAChapter.SelectedIndex = 0
			'cmbATAChapter.Enabled = False
			cmbMELCategory.Enabled = False

		Else
			pnlMELCategory.Visible = False
			mMELSnagPartList = MELSnagPartList.GetMELSnagPartList(txtDateOfOccurrence.Text, , "(SELECT)")
			cmbComponent.DataSource = mMELSnagPartList
			Session("mMELSnagPartList") = mMELSnagPartList
			cmbComponent.DataBind()
			'cmbATAChapter.SelectedIndex = 0
			'cmbATAChapter.Enabled = True
			cmbMELCategory.Enabled = True
		End If

		txtFrequencyInDay.Enabled = False
		chkIsInHours.Enabled = False
		If chkIsInHours.Checked = True Then
			chkIsInHours.Checked = False
		End If
		txtFrequencyInHours.Enabled = False

		'txtATAChapter.Text = ""
		txtFrequencyInDay.Text = "0"
		txtFrequencyInHours.Text = ""
		cmbMELCategory.SelectedIndex = 0
		Session("ShowMEL") = chkShowMEL.Checked

	End Sub

	Private Sub chkIsInHours_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsInHours.CheckedChanged
		If chkIsInHours.Checked = True Then
			txtFrequencyInDay.Text = "0"
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = True
		Else
			txtFrequencyInHours.Text = ""
			If cmbMELCategory.SelectedIndex = 1 Then
				txtFrequencyInDay.Enabled = True
			End If
			txtFrequencyInHours.Enabled = False
		End If
	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub

	Private Sub hdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click
		'ControlVisibilityForAttachment()
		'upnlAttach.Update()
		AttachMyFile()
		upnlWOAttachment.Update()
		upnldgWOJobAttachment.Update()
	End Sub

	Private Sub hdnBtnAddJobTaskDetail_Click(sender As Object, e As EventArgs) Handles hdnBtnAddJobTaskDetail.Click, hdnBtnAddSelectTasks.Click, hdnBtnAddWOJobNRCDetail.Click, hdnimgbtnDesignation.Click, hdnBtnAddResourceAllocation.Click

		If CType(Session("AddTaskCards"), String) = "True" Then
			'Add selected part(s) to Task's Items
			AddMultipleTaskCards()
			Session("mnWO") = mnWO
			Session("AddTaskCards") = "False"
		Else
			Session("AddTaskCards") = "False"
		End If

		If CType(Session("ActiveJobDetailsTabIndex"), Integer) > 0 Then
			If Session("ActiveJobDetailsTabIndex") IsNot Nothing Then WOJobDetailsTabContainer.ActiveTabIndex = CType(Session("ActiveJobDetailsTabIndex"), Integer) : Session.Remove("ActiveJobDetailsTabIndex")
			lblHeader.Text = mnWO.WOJobs.CurrentItem.WOJobTasks.Count.ToString
			Label3.Text = mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.Count.ToString
			Label4.Text = mnWO.WOJobs.CurrentItem.WOJobSpares.Count.ToString
			Label5.Text = mnWO.WOJobs.CurrentItem.WOJobComps.Count.ToString
			Label6.Text = mnWO.WOJobs.CurrentItem.WOJobNRCCountForLinK.ToString
			up.Update()
			Call WOJobDetailsActiveTabChanged(Nothing, Nothing)
		Else
			WOJobDetailsTabContainer.ActiveTabIndex = 0
		End If
	End Sub

	Private Sub refreshTabs_Click(sender As Object, e As EventArgs) Handles refreshTabs.Click
		' WOJobDetailsTabContainer.DataBind()
		up.DataBind()
		' up.Update()
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
			mnWO.WOJobs.CurrentItem.FrequencyInHours = txtFrequencyInHours.Text
		Else : mnWO.WOJobs.CurrentItem.FrequencyInDays = txtFrequencyInDay.Text
			mnWO.WOJobs.CurrentItem.DateOfOccurrence = txtDateOfOccurrence.Text.ToString
		End If

	End Sub

	Private Sub btnSelectFile_ServerClick(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick
		If mnWO.WOJobs.CurrentItem.IsAttachmentAdded Then
			mFileJobAttach = FileAttach.GetAttachment(mnWO.WOJobs.CurrentItem.ID)
		Else
			mFileJobAttach = FileAttach.NewAttachment(Guid.NewGuid, mnWO.WOJobs.CurrentItem.ID)
		End If
		Session("mFileAttach") = mFileJobAttach
	End Sub

	Private Sub lnkCreateRequisition_Click(sender As Object, e As EventArgs) Handles lnkCreateRequisition.Click
		If (AppSettings("ClientCode") <> "STR" And Not User.IsInRole("EngineeringRequisitionNew")) Or (AppSettings("ClientCode") = "STR" And ((mnWO.WOJobs(0).WOJobTypeID = 1 And Not User.IsInRole("PlanningRequisitionNew")) Or (mnWO.WOJobs(0).WOJobTypeID <> 1 And Not User.IsInRole("EngineeringRequisitionNew")))) Then 'For Star Air For Unscheduled Job create Planning Req and for other jobs create Engg. Req.
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		'Commneted & Added by Vikrant on 19-Sep-2019
		'mRequisitionNew = RequisitionNew.NewRequisition(Trans.EngineeringRequisition)
		'mRequisitionNew.ReqDate = mnWO.WODate
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
		If AppSettings("ClientCode") = "IND" Then
			Dim mWorkShopList As WorkShopList
			mWorkShopList = WorkShopList.GetWorkShopList(0, , , True, "(SELECT)")
			mRequisitionNew.LocationID = mWorkShopList(mnWO.WorkShopID).locationID
		End If

		'12-Jun-2019
		For i As Integer = 0 To mRequisitionItemsNew.Count - 1
			ReqItemIds.Append(mRequisitionItemsNew(i).ItemID.ToString + ",")
		Next
		'End

		For j As Integer = 0 To mnWO.WOJobs.CurrentItem.WOJobSpares.Count - 1
			If Not ReqItemIds.ToString.TrimEnd(",").Contains(mnWO.WOJobs.CurrentItem.WOJobSpares(j).ItemID.ToString) Then '12-Jun-2019
				Dim mItemList As ItemList
				mItemList = ItemList.GetItemList(1, ItemName:=mnWO.WOJobs.CurrentItem.WOJobSpares(j).PartNo)
				If mItemList.Count > 0 Then
					If Not mRequisitionNew.RequisitionItemsNew.Contains(mItemList(0).ID) Then
						mRequisitionNew.RequisitionItemsNew.Add(mRequisitionNew.ID, Guid.Empty)
						mRequisitionNew.RequisitionItemsNew.CurrentItem.ItemID = mItemList(0).ID
						mRequisitionNew.RequisitionItemsNew.CurrentItem.PartNo = mItemList(0).Name
						mRequisitionNew.RequisitionItemsNew.CurrentItem.Description = mItemList(0).Description
						mRequisitionNew.RequisitionItemsNew.CurrentItem.IPCReference = mItemList(0).IPCReference
						mRequisitionNew.RequisitionItemsNew.CurrentItem.RequestedQty = mnWO.WOJobs.CurrentItem.WOJobSpares(j).RequiredQty
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
						mRequisitionNew.RequisitionItemsNew(mItemList(0).ID, "").RequestedQty += mnWO.WOJobs.CurrentItem.WOJobSpares(j).RequiredQty
					End If

				End If
			End If
		Next

		Session("mRequisitionNew") = mRequisitionNew
		'Session("TransTypeID") = Trans.EngineeringRequisition
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

		'MarkLog(Action.[New], "Engineering Requisition", "", ErrorType.NoError, mRequisitionNew.ID, EventLogID)
		Dim ReqURLFromWO As New Stack
		ReqURLFromWO.Push(Request.Url)
		Session("ReqURLFromWO") = ReqURLFromWO
		Session("MiddleFrameForWO") = Session("MiddleFrame")
		Session("TransTypeID") = CInt(Trans.EngineeringRequisition)
		Response.Redirect("wfRequisition_Ajax.aspx?BackPage=wfnWODetail_AJAX.aspx")
	End Sub

	Private Sub lnkViewIndent_Click(sender As Object, e As EventArgs) Handles lnkViewIndent.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		SetObject()
		Session("mnWO") = mnWO
		Session("mWOID") = mnWO.ID
		ScriptManager.RegisterClientScriptBlock(Me, [GetType], "RequisitionView", "RequisitionView();", True)
	End Sub

	Private Sub txtStartDateTime_TextChanged(sender As Object, e As EventArgs) Handles txtStartDateTime.TextChanged
		If IsValidTime(txtStartDateTime.Text.ToString.Trim) = False Then
			txtStartDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = txtStartDate.Text.ToString + " " + txtStartDateTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mnWO.WOJobs.CurrentItem.WOJobStartDateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
				' mnWO.WOStartDate = DateTime
				Session("mnWO") = mnWO
			End If
		End If
	End Sub

	Private Sub txtEndDateTime_TextChanged(sender As Object, e As EventArgs) Handles txtEndDateTime.TextChanged
		If IsValidTime(txtEndDateTime.Text.ToString.Trim) = False Then
			txtEndDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = txtEndDate.Text.ToString + " " + txtEndDateTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mnWO.WOJobs.CurrentItem.WOJobCloseDateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
				' mnWO.WOStartDate = DateTime
				Session("mnWO") = mnWO
			End If
		End If
	End Sub

	Private Sub btnReject_Click(sender As Object, e As EventArgs) Handles btnReject.Click
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim mnWOApproveReject As nWOApproveReject
		If txtWOJobRemark.Text = "" Then
			MSGBoxCtrl.Show("Alert!", "Please enter the Remark before rejecting a Work Order", "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		'Added By Saylee On 4-Mar-2020 For Approval Reject history
		mnWOApproveReject = nWOApproveReject.NewApproval(mnWO.ID)

		If (AppSettings("ClientCode") = "IND") Then
			mnWOApproveReject.Date = CType(DateTime.Now.ToString.Trim, DateTime)
		Else
			mnWOApproveReject.Date = CDate(DateTime.Now.ToString.Trim)
		End If

		mnWOApproveReject.ApprovedRejectStatus = 2
		mnWOApproveReject.Remark = txtWOJobRemark.Text

		If Session("MiddleFrame") = "wfnWOExecutionList.aspx" Then
			mnWOApproveReject.WOStatusID = 7
			mnWO.StatusID = 2
			mnWO.WOStatusID = 1 'reverted to submitted state
			mnWO.WOPlanedDate = DBNull.Value
		End If
		Session("IsValid") = IsValid
		Session("mnWO") = mnWO
		Session("mnWOApproveReject") = mnWOApproveReject
		MSGBoxCtrl.show(MSGBox.Message_title.RejectWO, MSGBox.Message_text.RejectWO, "<strong>Work Order</strong>", MsgBoxStyle.YesNo, "WOStatus")
		'**************************************************************
	End Sub

	Private Sub chkOtherJob_CheckedChanged(sender As Object, e As EventArgs) Handles chkOtherJob.CheckedChanged
		If chkOtherJob.Checked Then
			txtOtherJobSpecification.Enabled = True
		Else
			txtOtherJobSpecification.Enabled = False
			txtOtherJobSpecification.Text = ""
		End If
		upnlOtherJob.Update()
	End Sub

	Private Sub dgwoAttachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgWOJobAttachment.RowCommand
		Dim mFileAttachments As FileAttachments
		Select Case e.CommandName
			Case "View"
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex

				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				mFileAttachments = mnWO.WOJobs.CurrentItem.FileAttachments
				'mFileAttachments.CurrentIndex = Index - 1

				If mFileAttachments.Count = 1 Then
					mFileAttachments.CurrentIndex = 0
				Else
					mFileAttachments.CurrentIndex = Index - 1
				End If

				If mFileAttachments.CurrentItem.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFilel();", True)
					End If
				End If
				dgWOJobAttachment.DataSource = mnWO.WOJobs.CurrentItem.FileAttachments
				dgWOJobAttachment.DataBind()
				ControlVisibility()
				upnlWOAttachment.Update()
				upnldgWOJobAttachment.Update()
			Case "Remove"
				'Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOJobAttachment.PageSize * dgWOJobAttachment.PageIndex
				' DeleteAttachment(Index)
				mFileAttachments = mnWO.WOJobs.CurrentItem.FileAttachments
				If mFileAttachments.Count = 1 Then
					DeleteJobAttachment(0)
				Else
					DeleteJobAttachment(Index - 1)
				End If
		End Select

	End Sub

	Private Sub btnSelectFiles_Click(sender As Object, e As ImageClickEventArgs) Handles btnSelectFiles.Click
		SetObject()
		Session("mnWO") = mnWO
		Session("mFileAttach") = Nothing
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
	End Sub

	'Added by Harsh Sugandhi on 24th July 2024 for FLYPAL-1771 Facility to add Multiple Job Actions W.O. Job
	Private Sub Calculate_CompletionDetails_ActualManHours(sender As Object, e As EventArgs) Handles txtCompletionDetailStartDate.TextChanged,
																									 txtCompletionDetailEndDate.TextChanged

		Try

			If txtCompletionDetailStartDate.Text <> "" And txtCompletionDetailEndDate.Text <> "" Then

				Dim TotalManHours As Decimal

				Dim StartDate As DateTime = New SmartDate(txtCompletionDetailStartDate.Text.ToString).Date
				Dim EndDate As DateTime = New SmartDate(txtCompletionDetailEndDate.Text.ToString).Date

				TotalManHours = DateDiff(DateInterval.Minute, StartDate, EndDate)

				txtCompletionDetailActualTime.Text = (New Period(1, TotalManHours, 0)).Value
				txtCompletionDetailActualTime.Enabled = True

			End If

			If DirectCast(sender, Control).ID = "txtCompletionDetailStartDate" Then
				txtCompletionDetailStartDateTime.Focus()
			Else
				txtCompletionDetailEndDateTime.Focus()
			End If

			DataFieldBind()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub Calculate_CompletionDetails_ActualManHoursWithTime(sender As Object, e As EventArgs) Handles txtCompletionDetailStartDateTime.TextChanged,
																											 txtCompletionDetailEndDateTime.TextChanged

		Try

			If txtCompletionDetailStartDate.Text <> "" And txtCompletionDetailEndDate.Text <> "" Then

				Dim TotalManHours As Decimal

				If txtCompletionDetailStartDateTime.Text <> "00:00" And txtCompletionDetailEndDateTime.Text <> "00:00" Then

					Dim StartDateTime As String = txtCompletionDetailStartDate.Text & " " & txtCompletionDetailStartDateTime.Text
					Dim EndDateTime As String = txtCompletionDetailEndDate.Text & " " & txtCompletionDetailEndDateTime.Text

					Dim StartDate As DateTime = DateTime.Parse(StartDateTime)
					Dim EndDate As DateTime = DateTime.Parse(EndDateTime)

					TotalManHours = DateDiff(DateInterval.Minute, StartDate, EndDate)

				End If

				txtCompletionDetailActualTime.Text = (New Period(1, TotalManHours, 0)).Value
				txtCompletionDetailActualTime.Enabled = False

			End If

			If DirectCast(sender, Control).ID = "txtCompletionDetailStartDateTime" Then
				txtCompletionDetailEndDate.Focus()
			End If

			DataFieldBind()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub AddCompletionDetails(sender As Object, e As ImageClickEventArgs) Handles btnAddCompletionDetails.Click

		Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
		Try

			gvCompletionDetails.DataSource = mnWO.WOJobs.CurrentItem.WOJobActions
			gvCompletionDetails.DataBind()
			upnlCompletionDetailsList.Update()

			txtCompletionDetailEmployee.Text = mUser.EmployeeName

			DataFieldBind()

			mdlPopupCompletionDetails.Show()
			ClearControls_CompletionDetail()

			Session("IsNew") = True

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub SaveCompletionDetails(sender As Object, e As EventArgs) Handles btnSaveCompletionDetails.Click

		Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
		Try

			'Check for Validations before saving data
			Page.Validate()
			If Not IsValid Then upnlCompletionDetailsErrorList.Update() : Exit Sub

			If Not DateTimeValidation_CompletionDetail() Then upnlCompletionDetailsErrorList.Update() : Exit Sub

			'While editing a records do not Add New entry
			If Session("IsNew") Then
				mnWO.WOJobs.CurrentItem.WOJobActions.Add(mnWO.WOJobs.CurrentItem.ID)
			End If

			mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.EmployeeID = mUser.EmployeeID
			mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.EmployeeName = mUser.EmployeeName

			SetObject_CompletionDetail()

			gvCompletionDetails.DataSource = mnWO.WOJobs.CurrentItem.WOJobActions
			gvCompletionDetails.DataBind()
			upnlCompletionDetailsList.Update()

			'Dumping the values of Last record from the grid into the controls
			Dumping_CompletionDetailValues()

			mdlPopupCompletionDetails.Hide()

			Session("mnWO") = mnWO

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub CloseCompletionDetailsModal(sender As Object, e As EventArgs) Handles btnCloseCompletionDetails.Click

		Try

			mdlPopupCompletionDetails.Hide()

			'To not add the record if user clicks on Add New and closes the Modal Pop up 
			If mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.IsNew() Then

				mnWO.WOJobs.CurrentItem.WOJobActions.Remove(mnWO.WOJobs.CurrentItem.WOJobActions.CurrentItem.ID)

			End If

			Session.Remove("IsNew")

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub CompletionDetails_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCompletionDetails.RowCommand

		Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)

		Select Case e.CommandName

			Case "EditRecord"

				Dim Index As Integer = CInt(e.CommandArgument) + gvCompletionDetails.PageSize * gvCompletionDetails.PageIndex
				mnWO.WOJobs.CurrentItem.WOJobActions.CurrentIndex = Index

				If Not mnWO.WOJobs.CurrentItem.WOJobActions(Index).EmployeeID.Equals(mUser.EmployeeID) Then

					MarkLog(Action.Authorize,
							"Work Order",
							User.Identity.Name & " is not Authorized User to Submit " & mWODetail,
							ErrorType.HandledError,
							Guid.Empty,
							EventLogID)

					MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
									MSGBox.Message_text.Authorization,
									"",
									MsgBoxStyle.OkOnly,
									"Authorization")

					Exit Sub

				End If

				gvCompletionDetails.DataSource = mnWO.WOJobs.CurrentItem.WOJobActions
				gvCompletionDetails.DataBind()
				upnlCompletionDetailsList.Update()

				mdlPopupCompletionDetails.Show()
				SetControls_CompletionDetail()

				Session("IsNew") = False

			Case "DeleteRecord"

				Dim Index As Integer = CInt(e.CommandArgument) + gvCompletionDetails.PageSize * gvCompletionDetails.PageIndex
				mnWO.WOJobs.CurrentItem.WOJobActions.CurrentIndex = Index

				If Not mnWO.WOJobs.CurrentItem.WOJobActions(Index).EmployeeID.Equals(mUser.EmployeeID) Then

					MarkLog(Action.Authorize,
							"Work Order",
							User.Identity.Name & " is not Authorized User to Submit " & mWODetail,
							ErrorType.HandledError,
							Guid.Empty,
							EventLogID)

					MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
												MSGBox.Message_text.Authorization,
												"",
												MsgBoxStyle.OkOnly,
												"Authorization")

					Exit Sub

				End If

				MSGBoxCtrl.show(MSGBox.Message_title.Delete,
								MSGBox.Message_text.Delete,
								" ",
								MsgBoxStyle.YesNo,
								"DeleteCompletionDetail")

		End Select

	End Sub
	'End

#End Region

#Region " TAB's "

	Private Sub WOJobDetailsActiveTabChanged(sender As Object, e As EventArgs) Handles WOJobDetailsTabContainer.ActiveTabChanged

		Session("mnWO") = mnWO
		Session("mnWOJob") = mnWO.WOJobs.CurrentItem
		Session("mIndex") = "-1"
		lblHeader.Text = mnWO.WOJobs.CurrentItem.WOJobTasks.Count.ToString
		Label3.Text = mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.Count.ToString
		Label4.Text = mnWO.WOJobs.CurrentItem.WOJobSpares.Count.ToString
		Label5.Text = mnWO.WOJobs.CurrentItem.WOJobComps.Count.ToString
		Label6.Text = mnWO.WOJobs.CurrentItem.WOJobNRCCountForLinK.ToString

		Select Case WOJobDetailsTabContainer.ActiveTabIndex
			Case 0
				txtEstimatedTime.DataBind()
				txtActualTime.DataBind()
			Case 1      'tabWOJobTask
				Session("ActiveJobDetailsTabIndex") = 1
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobTask", "CallWOJobTask();", True)
			Case 2      'tabWOJobDesignationAllocations
				Session("mDesignationAllocationEdit") = False
				Session("ActiveJobDetailsTabIndex") = 2
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobDesignationAllocations", "CallWOJobDesignationAllocations();", True)
			Case 3      'tabWOJobSpares
				Session("ActiveJobDetailsTabIndex") = 3
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobSpares", "CallWOJobSpares();", True)
			Case 4      'tabWOJobComps
				Session("Edit") = False
				Session("ActiveJobDetailsTabIndex") = 4
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobComps", "CallWOJobComps();", True)
			Case 5      'tabWOJobNRC
				Session("ActiveJobDetailsTabIndex") = 5
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallWOJobNRC", "CallWOJobNRC();", True)
		End Select

	End Sub

#End Region

End Class