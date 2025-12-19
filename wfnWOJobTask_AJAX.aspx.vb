'AJAX CREATED By : Saylee
'Dated           : 06-Nov-2013

Public Class wfnWOJobTask_AJAX
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	Protected mnWOJob As nWOJob
	Dim mTaskCard As TaskCard
	Dim mIndex As Int32
	Protected mnWO As nWO

	Public mATAList As ATAList 'Added By Shweta
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

#Region " Helper Methods "
	Public Sub GetSession()
		mnWOJob = Session("mnWOJob")
		mnWO = Session("mnWO")
		mTaskCard = Session("mTaskCard")
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
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
			'IsInRoleString = "WorkOrder"
			If mnWO.TransTypeID = Trans.WO145 Then
				IsInRoleString = "WorkOrder"
			ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Then
				IsInRoleString = "SpareAssemblyWO"
			ElseIf mnWO.TransTypeID = Trans.SpareComponentWO Then
				IsInRoleString = "SpareComponentWO"
			ElseIf mnWO.TransTypeID = Trans.EngineeringWO Then
				IsInRoleString = "EngineeringOrder"
			Else
				IsInRoleString = "CAMOWO"
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

	Private Sub CallUpdatePanel()
		'upnlDates.Update()
		'upnlTaskCardDet.Update()
		'upnlTaskCardHeading.Update()
		'upnlTaskCardSpares.Update()
		'upnlTaskCardTools.Update()
		'upnlTaskSteps.Update()
		'upnlWOJobTaskSpares.Update()
		upnlAttachFile.Update()
		'upnlTaskDetail.Update()
	End Sub
	Private Sub SaveFormtoObject()
		mnWOJob.WOJobTasks.CurrentItem.TaskAction = Trim(txtTaskAction.Text)

		If txtStartDate.Text.ToString <> "" Then


			If txtStartDateTime.Text <> "" Then
				mnWOJob.WOJobTasks.CurrentItem.ActualStartDate = CType(txtStartDate.Text.ToString.Trim + " " + txtStartDateTime.Text.ToString.Trim, DateTime)
			Else
				mnWOJob.WOJobTasks.CurrentItem.ActualStartDate = txtStartDate.Text
			End If

		Else
			mnWOJob.WOJobTasks.CurrentItem.ActualStartDate = System.DBNull.Value
		End If

		If txtEndDate.Text.ToString <> "" Then


			If txtEndDateTime.Text <> "" Then
				mnWOJob.WOJobTasks.CurrentItem.ActualEndDate = CType(txtEndDate.Text.ToString.Trim + " " + txtEndDateTime.Text.ToString.Trim, DateTime)
			Else
				mnWOJob.WOJobTasks.CurrentItem.ActualEndDate = txtEndDate.Text
			End If

		Else
			mnWOJob.WOJobTasks.CurrentItem.ActualEndDate = System.DBNull.Value
		End If

		mnWOJob.WOJobTasks.CurrentItem.ActualTime = Trim(txtTime.Text)
		mnWOJob.WOJobTasks.CurrentItem.IsDone = chkIsDone.Checked

		'Added By Utkarsh On 26-Apr-2011

		mnWOJob.WOJobTasks.CurrentItem.TaskCardNo = Trim(txtCardNo.Text)
		mnWOJob.WOJobTasks.CurrentItem.RevNo = Trim(txtRevNo.Text)

		If txtRevDate.Text.ToString <> "" Then
			mnWOJob.WOJobTasks.CurrentItem.RevDate = txtRevDate.Text
		Else
			mnWOJob.WOJobTasks.CurrentItem.RevDate = System.DBNull.Value
		End If

		If txtIssueDate.Text.ToString <> "" Then
			mnWOJob.WOJobTasks.CurrentItem.IssueDate = txtIssueDate.Text
		Else
			mnWOJob.WOJobTasks.CurrentItem.IssueDate = System.DBNull.Value
		End If

		mnWOJob.WOJobTasks.CurrentItem.Reference = Trim(txtReference.Text)
		mnWOJob.WOJobTasks.CurrentItem.Material = Trim(txtMaterial.Text)
		mnWOJob.WOJobTasks.CurrentItem.checks = Trim(txtCheck.Text)
		mnWOJob.WOJobTasks.CurrentItem.Equipment = Trim(txtEquipment.Text)
		mnWOJob.WOJobTasks.CurrentItem.EstimatedHours = Trim(txtEstimatedHr.Text)
		mnWOJob.WOJobTasks.CurrentItem.TaskDescription = Trim(txtDescription.Text)
		mnWOJob.WOJobTasks.CurrentItem.RelatedTaskCardsNo = Trim(txtRelatedTaskCardNo.Text)

		'Added by Vikrant on 05-Sept-2013 For BA04092013
		Dim txtValue As TextBox
		Dim i As Integer = 0
		For Each mWOJobTaskStepsSpare As nWOJobTaskSpare In mnWOJob.WOJobTasks.CurrentItem.WOJobTaskStepsSpares
			With mWOJobTaskStepsSpare
				Try
					txtValue = CType(Me.dgWOJobTaskSpares.Rows(i).FindControl("txtAdditionalSparesOffSerialNo"), TextBox)
					.OffSerialNo = Trim(txtValue.Text)
				Catch ex As Exception
					Throw ex
				End Try
			End With
			i = i + 1
		Next

		Dim txtValue1 As TextBox
		Dim j As Integer = 0
		For Each mWOJobTaskSpare As nWOJobTaskSpare In mnWOJob.WOJobTasks.CurrentItem.WOJobTaskSpares
			With mWOJobTaskSpare
				Try
					txtValue1 = CType(Me.dgTaskCardSpares.Rows(j).FindControl("txtOffSerialNo"), TextBox)
					.OffSerialNo = Trim(txtValue1.Text)
				Catch ex As Exception
					Throw ex
				End Try
			End With
			j = j + 1
		Next
		'End

		'Added By Vikrant on 03-Mar-2020 For ALL03032020
		i = 0
		For Each mWOJobTaskPartRemovals As nWOJobTaskSpare In mnWOJob.WOJobTasks.CurrentItem.WOJobTaskPartRemovals
			With mWOJobTaskPartRemovals
				Try
					txtValue = CType(Me.dgPartRemovals.Rows(i).FindControl("txtOffSerialNo"), TextBox)
					.OffSerialNo = Trim(txtValue.Text)
				Catch ex As Exception
					Throw ex
				End Try
			End With
			i = i + 1
		Next
		'End
		'''AttachMyFile()


	End Sub
	Private Sub addAttributes()
		txtTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtTime').value,event)")

		'Commented and Added By Saylee on 21-Feb-2013
		''txtEstimatedHr.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtEstimatedHr').value)")
		If (AppSettings("nWOShowHrsInDecimal") IsNot Nothing) AndAlso (AppSettings("nWOShowHrsInDecimal") = "True") Then
			txtEstimatedHr.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEstimatedHr').value,event)")
		Else
			txtEstimatedHr.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtEstimatedHr').value,event)")
		End If

	End Sub
	Public Sub CustomVailidity(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "txtTime" Then
			Try
				Dim ValueiInDecimal As String
				ValueiInDecimal = nWOPeriod.ConvertStringToDecimal(1, 1, txtTime.Text, False)
			Catch ex As Exception
				cvActualTime.ErrorMessage = ex.Message
				e.IsValid = False
			End Try
		ElseIf custValidator.ControlToValidate = "txtDescription" Then
			If Len(txtDescription.Text) > 1000 Then
				custValidator.ErrorMessage = "Max. length of Description should be 1000 char."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub
	Private Sub ControlVisibility()
		''If mnWOJob.WOJobTypeID = 2 And (Not mnWOJob.WOJobTasks.CurrentItem.IsNew) Then
		''    txtCardNo.Enabled = False
		''    txtRevNo.Enabled = False
		''    txtRevDate.Enabled = False
		''    txtIssueDate.Enabled = False
		''    txtReference.Enabled = False
		''    txtMaterial.Enabled = False
		''    txtCheck.Enabled = False
		''    txtEquipment.Enabled = False
		''    txtEstimatedHr.Enabled = False
		''    txtDescription.Enabled = False
		''    txtRelatedTaskCardNo.Enabled = False
		''End If

		txtTime.Enabled = mnWO.WOStatusID <> 3
		chkIsDone.Enabled = mnWO.WOStatusID <> 3
		txtCardNo.Enabled = mnWO.WOStatusID <> 3
		txtRevNo.Enabled = mnWO.WOStatusID <> 3
		txtRevDate.Enabled = mnWO.WOStatusID <> 3
		txtIssueDate.Enabled = mnWO.WOStatusID <> 3
		txtReference.Enabled = mnWO.WOStatusID <> 3
		txtMaterial.Enabled = mnWO.WOStatusID <> 3
		txtCheck.Enabled = mnWO.WOStatusID <> 3
		txtEquipment.Enabled = mnWO.WOStatusID <> 3
		txtEstimatedHr.Enabled = mnWO.WOStatusID <> 3
		txtDescription.Enabled = mnWO.WOStatusID <> 3
		txtRelatedTaskCardNo.Enabled = mnWO.WOStatusID <> 3
		txtTaskAction.Enabled = mnWO.WOStatusID <> 3
		btnOK.Enabled = mnWO.WOStatusID <> 3

		btnSelectFile.Disabled = (mnWO.WOStatusID = 3) Or (Not mnWOJob.WOJobTasks.CurrentItem.TaskCardID.Equals(Guid.Empty))

		If mnWOJob.WOJobTasks.CurrentItem.ImageSize > 0 Then
			ImageButton2.Visible = True
			If mnWO.WOStatusID = 3 Then
				btnDelAttach.Enabled = False
			Else
				btnDelAttach.Enabled = True
			End If
		Else
			ImageButton2.Visible = False
		End If

		'lblSpares.Visible = mnWOJob.WOJobTasks.CurrentItem.WOJobTaskSpares.Count > 0
		'If Not mnWOJob.WOJobTasks.CurrentItem.TaskCardID.Equals(Guid.Empty) Then
		'    lblTools.Visible = mTaskCard.TaskCardTools.Count > 0
		'    lblSteps.Visible = mTaskCard.TaskSteps.Count > 0
		'End If

		'lblAdditionalWorkSpares.Visible = mnWOJob.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Count > 0
		'lblPartRemovals.Visible = mnWOJob.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Count > 0 'Added By Vikrant on 03-Mar-2020 For ALL03032020

		'Added By Saylee ON 4-Feb-2013 for BA04022013
		If (AppSettings("ClientCode") = "BSA") Then 'Added By Saylee On 15-Oct-2014 For BSA15102014
			lblRelatedTaskCardNo.Text = "Other References"
		Else
			lblRelatedTaskCardNo.Text = "Related Task Card No."
		End If
		'Added By Vikrant on 03-Mar-2020 For ALL03032020
		If AppSettings("ClientCode") = "STR" Then
			dgTaskCardSpares.Columns(5).Visible = False
		Else
			dgTaskCardSpares.Columns(5).Visible = False
		End If
		'End
	End Sub
	Private Sub AttachMyFile()
		Try

			mnWOJob.WOJobTasks.CurrentItem.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
			mnWOJob.WOJobTasks.CurrentItem.ImageSize = Session("FileUpload.FileSize")
			mnWOJob.WOJobTasks.CurrentItem.FileExtension = Session("FileUpload.FileExtension")
			btnDelAttach.Enabled = True

		Catch ex As Exception

		End Try
		If mnWOJob.WOJobTasks.CurrentItem.ImageSize > 0 Then
			ImageButton2.Visible = True
			If mnWO.WOStatusID = 3 Then
				btnDelAttach.Enabled = False
			Else
				btnDelAttach.Enabled = True
			End If
		Else
			ImageButton2.Visible = False
		End If
		Session("mnWOJob") = mnWOJob
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		'Set the WOJob currently available.
		addAttributes()
		GetSession()
		lblJobLabel.Text = mnWOJob.SrNo
		txtWODate.Text = mnWO.WODateFormatted
		If Not Page.IsPostBack Then
			If txtTaskAction.Enabled = True Then
				setFocus(txtTaskAction)
			End If
			'Get the Index 
			If Request.QueryString("Index") IsNot Nothing Then
				Session("wfnWOJobTask.Index") = Request.QueryString("Index")
			Else
				Session("wfnWOJobTask.Index") = Session("mIndex")
				Session.Remove("mIndex")
			End If


			mnWOJob.BeginEdit()
			If CType(Session("wfnWOJobTask.Index"), Integer) = -1 Then    'when we come to add new record
				mnWOJob.WOJobTasks.Add(mnWOJob.ID)
				Session("mnWOJob") = mnWOJob
				'lblSteps.Visible = False
				'lnkViewAttachment.Enabled = False
				btnPrint.Enabled = False
				lblTools.Visible = False 'Added By Vikrant On 22-Jan-2013 For ALL21012013
				lblSpares.Visible = False 'Added By Shweta On 23-Jan-2013 For ALL21012013
				lblAdditionalWorkSpares.Visible = False 'Added by Vikrant on 06-Sept-2013 For BA04092013
				lblPartRemovals.Visible = False  'Added By Vikrant on 03-Mar-2020 For ALL03032020
				lblSteps.Visible = False
				dgTaskCardSpares.Visible = False
				dgTaskCardTools.Visible = False
				dgTaskSteps.Visible = False
				dgWOJobTaskSpares.Visible = False
				dgPartRemovals.Visible = False
				HideOnManaulTask.Visible = False
			Else                                                         'when we edit record
				HideOnManaulTask.Visible = True
				mnWOJob.WOJobTasks.CurrentIndex = (CInt(Session("wfnWOJobTask.Index")))
				'txtStartDate.Text = IIf(mnWOJob.WOJobTasks.CurrentItem.ActualStartDate.ToString = "", "", Format(CDate(mnWOJob.WOJobTasks.CurrentItem.ActualStartDateFormatted), AppSettings("DateFormat")))
				'txtEndDate.Text = IIf(mnWOJob.WOJobTasks.CurrentItem.ActualEndDate.ToString = "", "", Format(CDate(mnWOJob.WOJobTasks.CurrentItem.ActualEndDateFormatted), AppSettings("DateFormat")))

				If mnWOJob.WOJobTasks.CurrentItem.ActualStartDate.ToString = "" Then
					txtStartDate.Text = ""
					txtStartDateTime.Text = ""
				Else
					txtStartDate.Text = Format(CDate(mnWOJob.WOJobTasks.CurrentItem.ActualStartDateFormatted), AppSettings("DateFormat"))
					txtStartDateTime.Text = Format(CDate(mnWOJob.WOJobTasks.CurrentItem.ActualStartDateFormatted), AppSettings("TimeFormat"))
				End If

				If mnWOJob.WOJobTasks.CurrentItem.ActualEndDate.ToString = "" Then
					txtEndDate.Text = ""
					txtEndDateTime.Text = ""
				Else
					txtEndDate.Text = Format(CDate(mnWOJob.WOJobTasks.CurrentItem.ActualEndDateFormatted), AppSettings("DateFormat"))
					txtEndDateTime.Text = Format(CDate(mnWOJob.WOJobTasks.CurrentItem.ActualEndDateFormatted), AppSettings("TimeFormat"))
				End If

				txtRevDate.Text = IIf(mnWOJob.WOJobTasks.CurrentItem.RevDate.ToString = "", "", mnWOJob.WOJobTasks.CurrentItem.RevDateFormatted)
				txtIssueDate.Text = IIf(mnWOJob.WOJobTasks.CurrentItem.IssueDate.ToString = "", "", mnWOJob.WOJobTasks.CurrentItem.IssueDateFormatted)


				Session("mnWOJob") = mnWOJob


				'Added By Prashant 29-Dec-2008 --------------------------------------------------
				If Not mnWOJob.WOJobTasks.CurrentItem.TaskCardID.Equals(Guid.Empty) Then
					mTaskCard = TaskCard.GetTaskCard(mnWOJob.WOJobTasks.CurrentItem.TaskCardID)
					Session("mTaskCard") = mTaskCard
					dgTaskSteps.DataSource = mTaskCard.TaskSteps

					'Added By Vikrant On 22-Jan-2013 For ALL21012013
					dgTaskCardTools.DataSource = mTaskCard.TaskCardTools
					'End

					'Commented & Added by Vikrant on 05-Sept-2013 For BA04092013
					'dgTaskCardSpares.DataSource = mTaskCard.TaskCardSpares 'Added By Shweta On 23-Jan-2013 For ALL21012013
					dgTaskCardSpares.DataSource = mnWOJob.WOJobTasks.CurrentItem.WOJobTaskSpares
					dgWOJobTaskSpares.DataSource = mnWOJob.WOJobTasks.CurrentItem.WOJobTaskStepsSpares
					'End
					dgPartRemovals.DataSource = mnWOJob.WOJobTasks.CurrentItem.WOJobTaskPartRemovals  'Added By Vikrant on 03-Mar-2020 For ALL03032020
					mATAList = ATAList.GetATAList("", "(SELECT)")
					cmbATAChapter.DataSource = mATAList
					Session("mATAList") = mATAList

					'Commented By Utkarsh On 27-Apr-2011

					'With mTaskCard
					'    txtCardNo.Text = .TaskCardNo
					'    txtDescription.Text = .TaskDesc
					'    'txtModel.Text = .ModelName
					'    txtRevNo.Text = .RevNo

					'    'Commented By Utkarsh On 26-Apr-2011

					'    'txtRevDate.Text = .RevDate
					'    'txtIssueDate.Text = .IssueDate
					'    '------------------------------------
					'    'Added By Utkarsh On 26-Apr-2011

					'    txtRevDate.Value = .RevDate
					'    txtIssueDate.Value = .IssueDate

					'    '******************************
					'    txtReference.Text = .Reference
					'    txtEquipment.Text = .Equipment
					'    txtMaterial.Text = .Material
					'    txtEstimatedHr.Text = .EstimatedHours
					'    txtCheck.Text = .Check
					'    txtRelatedTaskCardNo.Text = .RelatedTaskCardsNo
					'End With

					'*************************************

					'lnkViewAttachment.Enabled = True

					With mTaskCard
						'Added by Shweta on 11-Jan-2013 
						txtAMPIssueRev.Text = mTaskCard.AMPIssueRev
						txtINSPTypeInterval.Text = mTaskCard.INSPTypeInterval
						txtZone.Text = mTaskCard.Zone
						txtArea.Text = mTaskCard.Area
						txtCategory.Text = mTaskCard.Category
						cmbATAChapter.SelectedValue = mTaskCard.ATAChapterID.ToString
						'Added By Shweta  on 18-Jan-2012 for   BA17012013
						txtInspCode.Text = mTaskCard.InspCode
						txtPublication.Text = mTaskCard.Publication
						txtSkill.Text = mTaskCard.Skill
						chkIsRII.Checked = mTaskCard.IsRII
						txtPanels.Text = mTaskCard.Panels
						'Added By Shweta  on 15-March-2013 for  BA14032013d
						txtHeading.Text = mTaskCard.TaskHeading
						txtSubject.Text = mTaskCard.TaskSubject
						txtRemark.Text = mTaskCard.Remark
					End With

					btnPrint.Enabled = True
					lblSteps.Visible = True
					lblTools.Visible = True 'Added By Vikrant On 22-Jan-2013 For ALL21012013
					lblSpares.Visible = True ''Added By Shweta On 23-Jan-2013 For ALL21012013
					lblAdditionalWorkSpares.Visible = True 'Added by Vikrant on 06-Sept-2013 For BA04092013
					lblPartRemovals.Visible = True 'Added By Vikrant on 03-Mar-2020 For ALL03032020
				End If
				'-------------------------------------------------------------------------------
			End If
		End If

		If Not Page.IsPostBack Then
			DataBind()
		End If
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			lblTitle.Text = "E.O. Job Task Detail"
		Else
			lblTitle.Text = "W.O. Job Task Detail"
		End If
		ControlVisibility() 'Added By Utkarsh On 27-Apr-2011
		'''AttachMyFile() 'Added By Utkarsh On 27-Apr-2011

		CallUpdatePanel()
	End Sub
	Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
		'If Not IsValid Then Exit Sub

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
		mnWOJob = Session("mnWOJob")
		SaveFormtoObject()
		If mnWOJob.WOJobTasks.CurrentItem.IsValid Then
			mnWOJob.ApplyEdit()
			Session("mnWOJob") = mnWOJob

			Dim mopenas As String = Request.QueryString("Type")
			If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
				Exit Sub
			End If
			Response.Redirect(Request.QueryString("BackPage2") & "?CPage1=" & Request.QueryString("CPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
		Else
			''cvControlValidator.ErrorMessage = mnWOJob.WOJobTasks.CurrentItem.GetBrokenRulesString
			''cvControlValidator.IsValid = mnWOJob.WOJobTasks.CurrentItem.IsValid
			Dim str As String = ""
			For j As Integer = 0 To mnWOJob.WOJobTasks.CurrentItem.GetBrokenRulesCollection.Count - 1
				str = str + mnWOJob.WOJobTasks.CurrentItem.GetBrokenRulesCollection(j).Description + "<BR>"
			Next

			If str <> "" Then
				cvControlValidator.ErrorMessage = str
				cvControlValidator.IsValid = mnWOJob.WOJobTasks.CurrentItem.IsValid
				upnlValidationSummary.Update()
			End If
		End If

	End Sub
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
		'
	End Sub
	Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
		mnWOJob.CancelEdit()
		'Response.Redirect(BackPage.Pop(Session("BackPage")))

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage2") & "?CPage1=" & Request.QueryString("CPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
	End Sub

	''Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
	''    Dim filesize1 As Integer = 0
	''    Dim file1(filesize1) As Byte
	''    mnWOJob.WOJobTasks.CurrentItem.ImageFile = file1
	''    mnWOJob.WOJobTasks.CurrentItem.ImageSize = 0
	''    ImageButton2.Visible = False
	''    btnDelAttach.Enabled = False
	''End Sub
	Protected Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelAttach.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		Dim filesize1 As Integer = 0
		Dim file1(filesize1) As Byte
		mnWOJob.WOJobTasks.CurrentItem.ImageFile = file1
		mnWOJob.WOJobTasks.CurrentItem.ImageSize = 0
		ImageButton2.Visible = False
		btnDelAttach.Enabled = False
	End Sub
	Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
		Dim no As New Random
		Dim strName As String = "abc" & no.Next.ToString
		If mnWOJob.WOJobTasks.CurrentItem.ImageSize > 0 Then
			Dim path As String = AppSettings("DOCPath") & "\" & strName & mnWOJob.WOJobTasks.CurrentItem.FileExtension
			Dim fs As FileStream
			If File.Exists(AppSettings("DOCPath")) = False Then
				'Delete File if exists
				System.IO.File.Delete(AppSettings("DOCPath") & strName & mnWOJob.WOJobTasks.CurrentItem.FileExtension)
				'Create the File
				fs = File.Create(path)
				'Add some information to file
				fs.Write(mnWOJob.WOJobTasks.CurrentItem.ImageFile, 0, mnWOJob.WOJobTasks.CurrentItem.ImageFile.Length)
				fs.Close()
				Session("DOCPath") = path
				Dim Str As String
				Str = "openFile();"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile1", Str, True)
			End If
		Else
			'''Dim msg1 As New SIMsgBox(Page, "Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly)
			'''msg1.ReplacePage = "wfnWOJobTask.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2")
			'''msg1.Show()
			MSGBoxCtrl.show("Attachment!", "No Attach File Present.", "", MsgBoxStyle.OkOnly, "")
			mIndex = Session("wfnWOJobTask.Index")
			Session("mIndex") = mIndex
			Exit Sub
		End If

	End Sub

	'********************************
	Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
		AttachMyFile()
	End Sub
	'Added By Saylee On 26-Sep-2018 For STR26092018
	Private Function IsValidTime(ByVal TimeValue As String) As Boolean
		Dim TimeRegulerExpression As String = ""
		If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
			'TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm)$"    '12 Hour Format
			TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
		Else
			TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
		End If

		If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
			Return True
		Else
			Return False
		End If
	End Function
	'End
	Private Sub txtStartDateTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStartDateTime.TextChanged
		If IsValidTime(txtStartDateTime.Text.ToString.Trim) = False Then
			txtStartDateTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = txtStartDate.Text.ToString + " " + txtStartDateTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mnWOJob.WOJobTasks.CurrentItem.ActualStartDateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
				' mnWO.WOStartDate = DateTime
				Session("mnWO") = mnWO
			End If
		End If
	End Sub
	Private Sub txtEndDateTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEndDateTime.TextChanged
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
#End Region
End Class