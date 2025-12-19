'CREATED By : Saylee
'Dated      : 29-May-2019

Public Class wfnWOJobTaskList
	Inherits System.Web.UI.Page

#Region " Variable Declaration "

	Public mnWOJob As nWOJob
	Protected mnWO As nWO
	Dim mWOJobTypeID As Integer
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mWODetail As String
	'Added By Vikrant For WO NRC
	Dim mWOJobNRCList As WOJobNRCList
	Dim mnWOJobNRC As nWOJob
	'End
	'Added By Saylee On 27-Dec-2018
	Dim mFileJobAttach As FileAttach
	Dim IsAttachmentDeleted As Boolean = False
	'End
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

#Region " Business Methods "
	Private Sub GetSession()
		mnWOJob = Session("mnWOJob")
		mnWO = Session("mnWO")
		mWOJobTypeID = CType(Session("WOJobTypeID"), Integer)
		mWOJobNRCList = CType(Session("mWOJobNRCList"), WOJobNRCList) 'Added By Vikrant For WO NRC
	End Sub
	Private Sub SetSession()
		Session("WOJobTypeID") = mWOJobTypeID
		'Added By Saylee On 27-Dec-2018
		Session("mFileAttach") = mFileJobAttach
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
		'End
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
	Private Sub CallUpdatePanels()
		upnlWOJobDetails.Update()
		upnlTitle.Update()
		upnlWOJobTask.Update()
	End Sub
	Private Sub ControlVisibility()
		btnAddWOJobTask.Enabled = IIf(mnWO.IsThirdParty, False, True) And mnWO.WOStatusID <> 3
		dgWOJobTask.Columns(8).Visible = IIf(mnWO.IsThirdParty, False, True)
		dgWOJobTask.Columns(9).Visible = IIf(mnWO.IsThirdParty, False, True) And mnWO.WOStatusID <> 3
		lblPlannedTask.Enabled = IIf(mnWO.IsThirdParty, False, True)




		'If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
		'    lblTitle.Text = " E.O. Job Detail"
		'    lblWO.Text = "E.O. No."
		'    lblWODate.Text = "E.O. Date"
		'Else
		'    lblTitle.Text = " W.O. Job Detail"
		'    lblWO.Text = "W.O. No."
		'    lblWODate.Text = "W.O. Date"
		'End If
	End Sub
	Private Sub WOJobTasksDelete(ByVal Index As Int32)
		'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.RemoveItem, SIMsgBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo)
		''msg1.ReplacePage = "wfnWOJobDetail_AJAX.aspx?BackPage1=wfnWODetail.aspx" & "&BackPage=" & Request.QueryString("BackPage")
		'msg1.ReplacePage = "wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
		'Session("sender") = "WOJobTasksDelete"
		'msg1.Show()
		mnWOJob.WOJobTasks.CurrentIndex = Index
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "WOJobTasksDelete")
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "WOJobTasksDelete" Then                      'WO Job Tasks Delete
						Try
							Session("Sender") = ""
							'mnWOJob.WOJobTasks.Remove(mnWOJob.WOJobTasks.CurrentIndex)
							mnWOJob.WOJobTasks.Remove(mnWOJob.WOJobTasks.CurrentIndex)
							'Session("mnWOJob") = mnWOJob

							''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
							ControlVisibility()
							DataFieldBind()
							upnlWOJobTask.Update()
							SetGrid()
							If Request.QueryString("Type") = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobTasks.Count.ToString + "');", True)
						Catch ex As Exception
							ex.GetBaseException()
						End Try

					ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
						Session("sender") = ""
						If mnWOJob.IsValid = True Then
							Session.Remove("IsValid")
						Else
							Session.Remove("IsValid")
							''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
							ControlVisibility()
							SetGrid()
							DataFieldBind()
							CallUpdatePanels()
						End If
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						If Session("Edit") = True Then
							mnWO = Session("mnWOClone")
						End If
						Session("mnWO") = mnWO
						Session.Remove("IsValid")
						Session("Sender") = ""
						Session.Remove("Edit")
						Session.Remove("mnWOClone")
						If mnWOJob.IsNew And mnWOJob.WOJobTypeID = 1 Then
							mnWO.WOJobs.Remove(mnWOJob)
						End If

						Dim mopenas As String = Request.QueryString("Type")
						If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
							'Session.Remove("MiddleFrame")
							ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
							Exit Sub
						End If

						Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
					Else
						Session("sender") = ""
						ControlVisibility()
						SetGrid()
						DataFieldBind()
						''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
					End If

			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			ControlVisibility()
			SetGrid()
			DataFieldBind()
			CallUpdatePanels()
			''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
		ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then
			Session("sender") = ""
			DataFieldBind()
		End If
	End Sub
	Private Sub SetGrid()
		Dim P As Integer
		Dim btnView As ImageButton
		For j As Integer = 0 To dgWOJobTask.Rows.Count - 1
			''P = IIf(Me.dgWOJobTask.Rows.Item(j).Cells(11).Text = "", 0, CType(Me.dgWOJobTask.Rows.Item(j).Cells(11).Text, Integer))
			'If Me.dgWOJobTask.Rows.Item(j).Cells(11).Text = "" Then
			'    P = 0
			'Else
			'    P = CType(Me.dgWOJobTask.Rows.Item(j).Cells(11).Text, Integer)
			'End If
			If Not mnWOJob.WOJobTasks(j).TaskCardID.Equals(Guid.Empty) Then
				P = mnWOJob.WOJobTasks(j).AttachmentCount
			Else
				If Me.dgWOJobTask.Rows.Item(j).Cells(9).Text = "" Then '11=> 9
					P = 0
				Else
					P = CType(Me.dgWOJobTask.Rows.Item(j).Cells(9).Text, Integer) '11=> 9
				End If
			End If
			'Ajay 5 - 6 - 2023
			'If P <= 0 Then
			'    lnkWOJobTaskView = CType(dgWOJobTask.Rows.Item(j).Cells(10).FindControl("lnkWOJobTaskView"), LinkButton)
			'    lnkWOJobTaskView.Enabled = False
			'End If
			If P <= 0 Then
				btnView = CType(dgWOJobTask.Rows.Item(j).Cells(8).FindControl("btnView"), ImageButton)
				btnView.Visible = False
			End If
		Next
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
						'mnWOJob.WOJobTasks.CurrentItem.AttachmentCount = mTaskCard.TaskCardAttachments.Count

						Dim mTaskCardSpare As TaskCardSpare
						Dim mTaskCardStepsSpare As TaskCardSpare

						For Each mTaskCardSpare In mTaskCard.TaskCardSpares
							mnWOJob.WOJobTasks.CurrentItem.WOJobTaskSpares.Add(mnWOJob.WOJobTasks.CurrentItem.ID)
							With mnWOJob.WOJobTasks.CurrentItem.WOJobTaskSpares.CurrentItem
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
							mnWOJob.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Add(mnWOJob.WOJobTasks.CurrentItem.ID)
							With mnWOJob.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.CurrentItem
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
					'Else
				End If
			Else
				If mnWOJob.WOJobTasks.Contains(tmpTaskCard.ID, "") Then
					mnWOJob.WOJobTasks.Remove(tmpTaskCard.ID, "")
				End If
			End If
		Next
		Session("TaskCards") = "False"
		Session.Remove("mTaskCard")
		Session.Remove("mTaskCardList")
	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()
		dgWOJobTask.DataSource = mnWOJob.WOJobTasks
		DataBind()
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		If Not Page.IsPostBack Then
			DataFieldBind()
		End If

		ControlVisibility()
		SetGrid()
	End Sub
	Private Sub dgWOJobTask_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOJobTask.RowCommand
		Dim mopenas As String = Request.QueryString("Type")
		Select Case e.CommandName
			Case "EditRecord"
				Dim Index As Integer = CType(e.CommandArgument, Integer)

				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				mnWOJob.WOJobTasks.CurrentIndex = Index
				Session("mnWOJob") = mnWOJob

				'Response.Redirect("wfnWOJobTask_AJAX.aspx?BackPage2=wfnWOJobDetail_AJAX.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&Index=" & index)
				Session("mIndex") = Index
				If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddJobTaskDetail", "OpenToAddJobTaskDetail('" + Index.ToString + "');", True)
				ElseIf mopenas IsNot Nothing AndAlso mopenas = "childpup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentOpenToAddJobTaskDetail", "CallParentOpenToAddJobTaskDetail('" + Index.ToString + "');", True)
				End If
			Case "DeleteRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Dim Index As Integer = CType(e.CommandArgument, Integer)

				WOJobTasksDelete(Index)
			Case "Attach"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
				Dim rowIndex As Integer = gvr.RowIndex
				Dim Index As Integer = rowIndex
				Session("mnWOJob") = mnWOJob
				mnWOJob = Session("mnWOJob")
				mnWOJob.WOJobTasks.CurrentIndex = Index
				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString

				Dim AttachmentCount As Integer = 0
				Dim mTaskCard As TaskCard
				If Not mnWOJob.WOJobTasks.CurrentItem.TaskCardID.Equals(Guid.Empty) Then
					mTaskCard = TaskCard.GetTaskCard(mnWOJob.WOJobTasks.CurrentItem.TaskCardID)
					AttachmentCount = mTaskCard.TaskCardAttachments.Count
				End If
				If AttachmentCount > 1 Then
					Session("mTaskCard") = mTaskCard
					Session("IsFromWOJobTask") = "True"
					Session("TransactionNameMarkLog") = "Task Card" 'used for marklog
					Session("TransactionName") = "Task Card No."
					Session("TransactionDetails") = mTaskCard.TaskCardNo
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAttachWindow", "OpenAttachWindow();", True)
				Else
					If Not mnWOJob.WOJobTasks.CurrentItem.TaskCardID.Equals(Guid.Empty) Then
						If mTaskCard.TaskCardAttachments(0).ImageSize > 0 Then
							Dim path As String = AppSettings("DOCPath") & StrName & mTaskCard.TaskCardAttachments(0).FileExtension
							Dim fs As FileStream
							If File.Exists(AppSettings("DOCPath")) = False Then
								'Delete File if exist
								System.IO.File.Delete(AppSettings("DOCPath") & StrName & mTaskCard.TaskCardAttachments(0).FileExtension)
								' Create the file.
								fs = File.Create(path)
								'' Add some information to the file.
								fs.Write(mTaskCard.TaskCardAttachments(0).ImageFile, 0, mTaskCard.TaskCardAttachments(0).ImageFile.Length)
								fs.Close()
								Session("DOCPath") = path
								'Session.Remove("mnWOJob")
								ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
							End If
						End If
					Else
						If mnWOJob.WOJobTasks.CurrentItem.ImageSize > 0 Then
							Dim path As String = AppSettings("DOCPath") & StrName & mnWOJob.WOJobTasks.CurrentItem.FileExtension
							Dim fs As FileStream
							If File.Exists(AppSettings("DOCPath")) = False Then
								'Delete File if exist
								System.IO.File.Delete(AppSettings("DOCPath") & StrName & mnWOJob.WOJobTasks.CurrentItem.FileExtension)
								' Create the file.
								fs = File.Create(path)
								'' Add some information to the file.
								fs.Write(mnWOJob.WOJobTasks.CurrentItem.ImageFile, 0, mnWOJob.WOJobTasks.CurrentItem.ImageFile.Length)
								fs.Close()
								Session("DOCPath") = path
								'Session.Remove("mnWOJob")
								ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
							End If
						End If
					End If
				End If


		End Select
	End Sub

	Private Sub btnAddWOJobTask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddWOJobTask.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		'  Session("mnWOJob") = mnWOJob

		Dim mopenas As String = Request.QueryString("Type")
		Dim Index As Integer = -1
		'''''''''If mnWOJob.WOJobTypeID = 1 Then 'For UnScheduled Jobs  ---------''''''Commented by Saylee, as now Scheduled Job can also select Task Cards
		Session("IsOpenFrom") = "WorkOrder"
		Session("AddTaskCards") = "False"
		Session.Remove("mSelectTaskCardList")
		Session.Remove("mTaskCardNo")
		Session.Remove("mInspInterval")
		Session.Remove("mModelID")

		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddSelectTasks", "OpenToAddSelectTasks();", True)
		ElseIf mopenas IsNot Nothing AndAlso mopenas = "childpup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentOpenToAddSelectTasks", "CallParentOpenToAddSelectTasks();", True)
		End If

		''''''Commented by Saylee, as now Scheduled Job can also select Task Cards
		'''''''''Else
		'''''''''    'Response.Redirect("wfnWOJobTask_AJAX.aspx?BackPage2=wfnWOJobDetail_AJAX.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&Index=-1")
		'''''''''    Session("mIndex") = "-1"
		'''''''''    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
		'''''''''        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddJobTaskDetail", "OpenToAddJobTaskDetail('" + Index.ToString + "');", True)
		'''''''''    ElseIf Not mopenas Is Nothing AndAlso mopenas = "childpup" Then
		'''''''''        ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentOpenToAddJobTaskDetail", "CallParentOpenToAddJobTaskDetail('" + Index.ToString + "');", True)
		'''''''''    End If

		'''''''''End If

	End Sub

	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

		If mnWOJob.IsNew And mnWOJob.WOJobTypeID = 1 Then
			mnWO.WOJobs.Remove(mnWOJob)
		End If

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		ElseIf mopenas IsNot Nothing AndAlso mopenas = "childpup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobTasks.Count.ToString + "');", True)
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub

	Private Sub hdnBtnAddSelectTasks_Click(sender As Object, e As System.EventArgs) Handles hdnBtnAddSelectTasks.Click, hdnBtnAddJobTaskDetail.Click
		If CType(Session("AddTaskCards"), String) = "True" Then
			'Add selected part(s) to Task's Items
			AddMultipleTaskCards()
			Session("AddTaskCards") = "False"
		Else
			Session("AddTaskCards") = "False"
		End If
		dgWOJobTask.DataSource = mnWOJob.WOJobTasks
		dgWOJobTask.DataBind()
		SetGrid()
		upnlWOJobTask.Update()
	End Sub

#End Region

End Class