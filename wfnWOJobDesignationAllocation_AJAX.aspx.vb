'CREATED By : Saylee
'Dated      : 12-Nov-2013

Public Class wfnWOJobDesignationAllocation_AJAX
	Inherits System.Web.UI.Page


#Region " Variable Declaration "
	Public mTempDesignationList As DesignationList
	Protected mnWO As nWO
	Protected mnWOJob As nWOJob 'V
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
		mnWO = Session("mnWO")
		mTempDesignationList = Session("mTempDesignationList")
		mnWOJob = Session("mnWOJob") 'V
		''mnWOJobDesignationAllocationList = Session("mnWOJobDesignationAllocationList")
	End Sub
	Private Sub SetSession()
		Session("mnWO") = mnWO
		Session("mTempDesignationList") = mTempDesignationList
		''Session("mnWOJobDesignationAllocationList") = mnWOJobDesignationAllocationList
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
	Public Function Total() As Object
		If txtEstimatedTime.Text = "" Then
			Return 0
		Else
			Dim hour As Integer = 0
			hour = CInt(txtEstimatedTime.Text.Split(":")(0).Trim) 'CInt(txtEstimatedTime.Text.Substring(0, txtEstimatedTime.Text.IndexOf(":")))
			Dim minute As Integer = CInt(txtEstimatedTime.Text.Split(":")(1).Trim) 'CInt(Substring(txtEstimatedTime.Text.IndexOf(":"c) + 1))
			Dim busytimes = New TimeSpan(hour, minute, 0)
			Dim result = busytimes.TotalHours * CDec(Val(txtRate.Text))
			Return Format(result, "##0.00##")
		End If
	End Function
	Private Sub EditRecord(ByVal Index As Int32)
		mnWOJob.WOJobDesignationAllocations.CurrentIndex = Index
		txtEstimatedTime.Text = mnWOJob.WOJobDesignationAllocations.Item(Index).EstimatedTime
		txtActualTime.Text = mnWOJob.WOJobDesignationAllocations.Item(Index).WOTotalActualTime
		txtRate.Text = mnWOJob.WOJobDesignationAllocations.Item(Index).Rate

		txtTotal.Text = Total()

		cmbDesignationList.SelectedValue = mnWOJob.WOJobDesignationAllocations.Item(Index).DesignationID.ToString
		setFocus(cmbDesignationList)
		'Added By Utkarsh On 21-Jan-2011
		If mnWOJob.WOJobDesignationAllocations.CurrentItem.WOJobResourceAllocations.Count > 0 Then
			Session("mDesignationAllocationEdit") = False
			cmbDesignationList.Enabled = False
		End If
		'---------------------------------  
	End Sub
	Private Sub ControlVisibility()
		dgWOJobDesignationAllocation.Columns(6).Visible = mnWO.WOStatusID <> 3
		'Added By Vikrant On 24-June-2013 For Indamer21062013
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
			If mnWOJob.WOJobDesignationAllocations.Count >= 3 Then
				btnAddTop.Enabled = False
			End If
		End If
		'End
	End Sub
	Private Sub DeleteRecord(ByVal Index As Int32)
		''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
		''msg1.ReplacePage = "wfnWOJobDesignationAllocation.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
		''Session("sender") = "Delete"
		''msg1.Show()
		MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
		mnWOJob.WOJobDesignationAllocations.CurrentIndex = Index
		Session("mnWO") = mnWO
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							Session("sender") = ""
							mnWO = Session("mnWO")
							mnWOJob.WOJobDesignationAllocations.Remove(mnWOJob.WOJobDesignationAllocations.CurrentIndex)
							For i As Integer = 0 To mnWOJob.WOJobDesignationAllocations.Count - 1
								mnWOJob.WOJobDesignationAllocations(i).SrNo = i + 1
							Next
							Session("mnWO") = mnWO
							Session("mDesignationAllocationEdit") = False
							DataFieldBind()
							ControlVisibility()
							txtEstimatedTime.Text = ""
							upnlGrid.Update()
							Dim mopenas As String = Request.QueryString("Type")
							If mopenas IsNot Nothing AndAlso mopenas = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobDesignationAllocations.Count.ToString + "');", True)
						Catch ex As SqlException
						End Try
					End If
				Case MsgBoxResult.No
					Session("sender") = ""
					If MSGBoxCtrl.Sender = "Delete" Then Session.Remove("mDesignationAllocationEdit")
					DataFieldBind()
					ControlVisibility()
					txtEstimatedTime.Text = ""
					upnlAdd.Update()
					upnlGrid.Update()
					'Response.Redirect("wfnWOJobDesignationAllocation.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
				Case MsgBoxResult.Ok
					Session("sender") = ""
					DataFieldBind()
					ControlVisibility()
					upnlGrid.Update()
					'Response.Redirect("wfnWOJobDesignationAllocation.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"
					Session("sender") = ""
					DataFieldBind()
					ControlVisibility()
					upnlGrid.Update()
					'Response.Redirect("wfnWOJobDesignationAllocation.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			DataFieldBind()
			ControlVisibility()
			upnlGrid.Update()
			'Response.Redirect("wfnWOJobDesignationAllocation.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
		ElseIf Result1 = 0 Then
			'Session("sender") = ""
			'DataFieldBind()
		End If
	End Sub
	Private Sub SetTitle()
	End Sub
	Private Sub SetObject()
		mnWOJob.WOJobDesignationAllocations.CurrentItem.DesignationID = New Guid(cmbDesignationList.SelectedValue)
		mnWOJob.WOJobDesignationAllocations.CurrentItem.EstimatedTime = txtEstimatedTime.Text
		mnWOJob.WOJobDesignationAllocations.CurrentItem.ActualTime = txtActualTime.Text
		mnWOJob.WOJobDesignationAllocations.CurrentItem.Rate = CDec(Val(txtRate.Text))
	End Sub
	Private Function CustomValidate1() As Boolean
		Dim strMSG As String = ""
		If Not mnWO.IsValid Then
			For i As Integer = 0 To mnWOJob.WOJobDesignationAllocations.CurrentItem.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWOJob.WOJobDesignationAllocations.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If
		If strMSG.Trim <> "" Then
			cvControlValidator.ErrorMessage = strMSG
			cvControlValidator.IsValid = False
			Return False
		End If
		upnlValidationSummary.Update()
		Return True

	End Function
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub addAttributes()
		txtEstimatedTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtEstimatedTime').value,event)")
		txtActualTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtActualTime').value,event)")
		txtRate.Attributes.Add("onKeyPress", "validateDecimalNo(this,event)")
	End Sub
	'-- Added By Utkarsh On 21-Jan-2011
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		Dim ValueiInDecimal As String
		If custValidator.ControlToValidate = "txtEstimatedTime" Then
			Try
				ValueiInDecimal = nWOPeriod.ConvertStringToDecimal(1, 1, txtEstimatedTime.Text, False)
			Catch ex As Exception
				custValidator.ErrorMessage = ex.Message
				e.IsValid = False
			End Try
			'ElseIf custValidator.ControlToValidate = "cmbDesignationList" Then
			'    If cmbDesignationList.SelectedIndex = 0 Then
			'        custValidator.ErrorMessage = "Designation Required."
			'        e.IsValid = False
			'    Else
			'        e.IsValid = True
			'    End If
		End If
	End Sub
	'------------------------------------
	Private Sub updatePanels()
		upnlGrid.Update()
		upnlAdd.Update()
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mTempDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
		cmbDesignationList.DataSource = mTempDesignationList
		Session("mTempDesignationList") = mTempDesignationList

		dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
		DataBind()
		txtActualTime.Text = "0:00"

	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		addAttributes()
		SetTitle()
		If Not IsPostBack And Session("sender") = "" Then
			If cmbDesignationList.Enabled = True Then
				setFocus(cmbDesignationList)
			End If
			DataFieldBind()
			txtEstimatedTime.Text = "0:00"
			If Session("mDesignationAllocationEdit") = True Then
				EditRecord(mnWOJob.WOJobDesignationAllocations.CurrentIndex)
				dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
			End If
		End If
		ControlVisibility()
		updatePanels()
	End Sub
	Private Sub btnAddTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTop.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		Dim mopenas As String = Request.QueryString("Type")
		If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
		If Page.IsValid Then
			If Session("mDesignationAllocationEdit") = False Then
				mnWOJob.WOJobDesignationAllocations.Add(mnWOJob.ID)
				'If Not mnWOJob.WOJobDesignationAllocations.Contains(mnWOJob.WOJobDesignationAllocations.CurrentItem.WOJobID, New Guid(cmbDesignationList.SelectedValue)) Then
				If Not mnWOJob.WOJobDesignationAllocations.Contains(New Guid(cmbDesignationList.SelectedValue)) Then
					SetObject()
					If mnWOJob.WOJobDesignationAllocations.CurrentItem.IsValid Then
						dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
						dgWOJobDesignationAllocation.DataBind()
						upnlGrid.Update()
						Session("mnWO") = mnWO
					Else
						If Not CustomValidate1() Then
							mnWOJob.WOJobDesignationAllocations.Remove(mnWOJob.WOJobDesignationAllocations.CurrentItem)
							Exit Sub
						End If
					End If
				Else
					mnWOJob.WOJobDesignationAllocations.Remove(mnWOJob.WOJobDesignationAllocations.CurrentItem)
					''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
					''msg1.ReplacePage = "wfnWOJobDesignationAllocation.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
					''msg1.Show()
					MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			Else
				'--Added by Utkarsh On 20-Jan-2011 
				'Clone
				Dim clnmnWO As nWO
				clnmnWO = mnWO.Clone

				SetObject()
				If mnWOJob.WOJobDesignationAllocations.CurrentItem.IsDirty Then
					If Not mnWOJob.WOJobDesignationAllocations.Contains(mnWOJob.WOJobDesignationAllocations.CurrentItem.ID, mnWOJob.WOJobDesignationAllocations.CurrentItem.WOJobID, New Guid(cmbDesignationList.SelectedValue)) Then
						'    If Not mnWOJob.WOJobDesignationAllocations.Contains(New Guid(cmbDesignationList.SelectedValue)) Then

						If Not CustomValidate1() Then
							Exit Sub
						End If
						dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
						DataBind()
						updatePanels()
						Session("mnWO") = mnWO
						setFocus(cmbDesignationList)
						Session("mDesignationAllocationEdit") = False
					ElseIf Not mnWOJob.WOJobDesignationAllocations.Contains(New Guid(cmbDesignationList.SelectedValue)) Then
						If Not CustomValidate1() Then
							Exit Sub
						End If
						dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
						DataBind()
						updatePanels()
						Session("mnWO") = mnWO
						setFocus(cmbDesignationList)
						Session("mDesignationAllocationEdit") = False
					Else
						mnWO = clnmnWO
						dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
						DataBind()
						updatePanels()
						Session("mnWO") = mnWO
						''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Duplicate, SIMsgBox.Message_text.Duplicate, "", MsgBoxStyle.OKOnly)
						''msg1.ReplacePage = "wfnWOJobDesignationAllocation.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
						''msg1.Show()
						MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If
				End If
			End If
			If Session("mDesignationAllocationEdit") = True Then
				EditRecord(mnWOJob.WOJobDesignationAllocations.CurrentIndex)
				dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
			End If
			DataFieldBind()

			If mopenas IsNot Nothing AndAlso mopenas = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobDesignationAllocations.Count.ToString + "');", True)
			txtEstimatedTime.Text = "0:00"
			txtRate.Text = ""
			txtTotal.Text = ""
			ControlVisibility()
			updatePanels()
		End If
		If mopenas IsNot Nothing AndAlso mopenas = "childpup" Then ScriptManager.RegisterStartupScript(Me, Me.GetType, "autoWOJobDesignationAllocationList", "autoWOJobDesignationAllocationList();", True)

		'''Response.Redirect("wfnWOJobDesignationAllocation.aspx?BackPage2=wfnWOJobDetail.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
	End Sub
	Private Sub dgWOJobDesignationAllocation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOJobDesignationAllocation.RowCommand
		'Dim Index As Int32 = dgWOJobDesignationAllocation.CurrentPageIndex * dgWOJobDesignationAllocation.PageSize + e.Item.ItemIndex
		Dim Index As Integer = dgWOJobDesignationAllocation.PageIndex * dgWOJobDesignationAllocation.PageSize + CInt(e.CommandArgument)
		Dim mID = mnWOJob.WOJobDesignationAllocations(Index).ID
		mnWOJob.WOJobDesignationAllocations.CurrentIndex = Index
		Dim mDesignationName As String = mnWOJob.WOJobDesignationAllocations(Index).DesignationName

		Select Case e.CommandName
			Case "EditRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				'Dim mID As New Guid(dgWOJobDesignationAllocation.Rows(Index).Cells(0).Text)
				'Dim mDesignationName As String = dgWOJobDesignationAllocation.Rows(Index).Cells(2).Text
				Session("mDesignationAllocationEdit") = True
				EditRecord(Index)
				dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
				upnlGrid.Update()
				upnlAdd.Update()
			Case "DeleteRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				''Dim mID As New Guid(dgWOJobDesignationAllocation.Rows(Index).Cells(0).Text)
				''Dim mDesignationName As String = dgWOJobDesignationAllocation.Rows(Index).Cells(2).Text
				DeleteRecord(Index)
			Case "AssignResource"
				''Dim mID As New Guid(dgWOJobDesignationAllocation.Rows(Index).Cells(0).Text)
				''Dim mDesignationName As String = dgWOJobDesignationAllocation.Rows(Index).Cells(2).Text

				mnWOJob.WOJobDesignationAllocations.CurrentItem.DesignationName = mDesignationName '''dgWOJobDesignationAllocation.Rows(Index).Cells(2).Text 'e.Item.Cells(2).Text
				Session("mWOJobDesignationAllocations") = mnWOJob.WOJobDesignationAllocations.CurrentItem
				Session("mnWO") = mnWO


				'Response.Redirect("wfnWOJobResourceAllocation_AJAX.aspx?CPage1=" & Request.QueryString("CPage1") & "&BackPage3=wfnWOJobDesignationAllocation_AJAX.aspx" & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
				Dim mopenas As String = Request.QueryString("Type")
				If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddResourceAllocation", "OpenToAddResourceAllocation();", True)
				ElseIf mopenas IsNot Nothing AndAlso mopenas = "childpup" Then
					ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentOpenToAddResourceAllocation", "CallParentOpenToAddResourceAllocation();", True)
				End If

		End Select
	End Sub
	Private Sub hdnimgbtnDesignation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgbtnDesignation.Click
		DataFieldBind()
	End Sub
	Private Sub imgDesignation_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDesignation.Click
		'SetObject()
		Session("FromwfnWOJobDesignationAllocation") = "FromwfnWOJobDesignationAllocation"

		''Response.Redirect("wfDesignation_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfnWOJobDesignationAllocation.aspx")
		ControlVisibility()
		updatePanels()

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDesignationWindow", "OpenDesignationWindow();", True)
		ElseIf mopenas IsNot Nothing AndAlso mopenas = "childpup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "CallParentDesignationWindow", "CallParentDesignationWindow();", True)
		End If



	End Sub
	Private Sub btnCloseBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseBottom.Click, btnCloseTop.Click
		SetSession()

		Dim mopenas As String = Request.QueryString("Type")

		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		ElseIf mopenas IsNot Nothing AndAlso mopenas = "childpup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetTabCount", "SetTabCount('" + mnWOJob.WOJobDesignationAllocations.Count.ToString + "');", True)
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CallCloseChildPage", "CallCloseChildPage();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage2") & "?CPage1=" & Request.QueryString("CPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
	End Sub
	'--Added By Utkarsh On 18-Jan-2011
	Private Sub dgWOJobDesignationAllocation_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgWOJobDesignationAllocation.Sorting
		mnWOJob.WOJobDesignationAllocations.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
		'mnWOJobDesignationAllocationList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		'dgWOJobDesignationAllocation.DataSource = mnWOJobDesignationAllocationList
		Session("mnWO") = mnWO
		dgWOJobDesignationAllocation.DataBind()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub
	Private Sub hdnBtnAddResourceAllocation_Click(sender As Object, e As System.EventArgs) Handles hdnBtnAddResourceAllocation.Click
		dgWOJobDesignationAllocation.DataSource = mnWOJob.WOJobDesignationAllocations
		dgWOJobDesignationAllocation.DataBind()
		upnlGrid.Update()
	End Sub
	Private Sub txtEstimatedTime_TextChanged(sender As Object, e As System.EventArgs) Handles txtEstimatedTime.TextChanged
		'Dim hour As Integer = 0
		'Dim str1 As String = txtEstimatedTime.Text.Substring(txtEstimatedTime.Text.IndexOf(":"))
		'hour = CInt(txtEstimatedTime.Text.Substring(0, txtEstimatedTime.Text.IndexOf(":")))
		'Dim minute As Integer = 30
		'Dim busytimes = New TimeSpan(hour, minute, 0)
		'Dim result = busytimes.TotalHours * CDec(Val(txtRate.Text))
		txtTotal.Text = Total()
	End Sub
	Private Sub txtRate_TextChanged(sender As Object, e As System.EventArgs) Handles txtRate.TextChanged
		'Dim hour As Integer = 0
		'Dim str1 As String = txtEstimatedTime.Text.Substring(txtEstimatedTime.Text.IndexOf(":"))
		'hour = CInt(txtEstimatedTime.Text.Substring(0, txtEstimatedTime.Text.IndexOf(":")))
		'Dim minute As Integer = 30
		'Dim busytimes = New TimeSpan(hour, minute, 0)
		'Dim result = busytimes.TotalHours * CDec(Val(txtRate.Text))
		txtTotal.Text = Total()
	End Sub
#End Region
End Class