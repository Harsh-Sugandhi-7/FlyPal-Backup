Public Class wfnWOJobResourceDetail_AJAX
	Inherits System.Web.UI.Page

#Region "Variable Declaration"
	Public mnWOJobResourceAllocation As nWOJobResourceAllocation
	Dim mDesignationName As String = ""

	Protected mnWO As nWO
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
		mnWOJobResourceAllocation = Session("mnWOJobResourceAllocation")
		mDesignationName = Session("mDesignationName")
		mnWO = Session("mnWO")
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

	Private Sub ControlVisibility()
		dgResouceDet.Columns(6).Visible = mnWO.WOStatusID <> 3 'ajay 7=6
		txtStartDate.Enabled = mnWO.WOStatusID <> 3
		txtEndDate.Enabled = mnWO.WOStatusID <> 3

		If txtStartDate.Text <> String.Empty And txtEndDate.Text <> String.Empty Then
			txtTotalTime.Enabled = False
		Else
			txtTotalTime.Enabled = True
		End If
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Dim str As String
		str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
		ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
	End Sub
	Private Sub SetObject()
		'  mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.WOJobResourceAllocationID = mnWOJobResourceAllocation.WOJobDesignationAllocationID
		If txtStartDate.Text.ToString <> String.Empty Then
			mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.StartDateTime = txtStartDate.Text.ToString
		Else
			mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.StartDateTime = System.DBNull.Value
		End If
		If txtEndDate.Text.ToString <> String.Empty Then
			mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.EndDateTime = txtEndDate.Text.ToString
		Else
			mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.EndDateTime = System.DBNull.Value
		End If
		mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.TotalTime = txtTotalTime.Text
	End Sub

	Private Sub EditRecord(ByVal Index As Int32)
		mnWOJobResourceAllocation.WOJobResourceDetails.CurrentIndex = Index
		txtStartDate.Text = IIf(mnWOJobResourceAllocation.WOJobResourceDetails.Item(Index).StartDateTime.ToString = "", "", mnWOJobResourceAllocation.WOJobResourceDetails.Item(Index).StartDateTimeFormatted) '
		txtEndDate.Text = IIf(mnWOJobResourceAllocation.WOJobResourceDetails.Item(Index).EndDateTime.ToString = "", "", mnWOJobResourceAllocation.WOJobResourceDetails.Item(Index).EndDateTimeFormatted) ''mnWOJobResourceAllocation.WOJobResourceDetails.Item(Index).EndDateTime
		'--Added By Utkarsh On 19-Jan-2011
		If txtStartDate.Text <> String.Empty And txtEndDate.Text <> String.Empty Then
			txtTotalTime.Enabled = False
		Else
			txtTotalTime.Enabled = True
		End If
		'--------------------------------
		txtTotalTime.Text = mnWOJobResourceAllocation.WOJobResourceDetails.Item(Index).TotalTime
		dgResouceDet.DataSource = mnWOJobResourceAllocation.WOJobResourceDetails
		'txtTotalTime.DataBind()
		upnlResourceDet.Update()
	End Sub
	Private Sub DeleteRecord(ByVal Index As Int32)
		'''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
		'''msg1.ReplacePage = "wfnWOJobResourceDetail.aspx?BackPage4=wfnWOJobResourceAllocation.aspx" & "&BackPage3=" & Request.QueryString("BackPage3") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
		'''Session("sender") = "Delete"
		'''msg1.Show()
		MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
		mnWOJobResourceAllocation.WOJobResourceDetails.CurrentIndex = Index
		Session("mnWOJobResourceAllocation") = mnWOJobResourceAllocation
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							mnWOJobResourceAllocation = Session("mnWOJobResourceAllocation")
							mnWOJobResourceAllocation.WOJobResourceDetails.Remove(mnWOJobResourceAllocation.WOJobResourceDetails.CurrentIndex)
							For i As Integer = 0 To mnWOJobResourceAllocation.WOJobResourceDetails.Count - 1
								mnWOJobResourceAllocation.WOJobResourceDetails(i).SrNo = i + 1
							Next
							Session("mnWOJobResourceAllocation") = mnWOJobResourceAllocation
							Session("mResourceDetailEdit") = False
							DataFieldBind()
							txtStartDate.Text = ""
							txtEndDate.Text = ""
							txtTotalTime.Text = ""
							ControlVisibility()
							updatePanels()
						Catch ex As SqlException
						End Try
					End If
				Case MsgBoxResult.No
					ControlVisibility()
					If MSGBoxCtrl.Sender = "Delete" Then Session.Remove("mResourceDetailEdit")
					DataFieldBind()
					txtStartDate.Text = ""
					txtEndDate.Text = ""
					txtTotalTime.Text = ""
					updatePanels()
				Case MsgBoxResult.OK
					Session("sender") = ""
					DataFieldBind()
					ControlVisibility()
					updatePanels()
				Case MsgBoxResult.OK And Session("sender") = "Authorization"
					Session("sender") = ""
					DataFieldBind()
					ControlVisibility()
					updatePanels()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			DataFieldBind()
			ControlVisibility()
			updatePanels()
		ElseIf Result1 = 0 Then
			'Session("sender") = ""
			'DataFieldBind()
		End If
	End Sub
	'-- Added By Utkarsh On 21-Jan-2011
	Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		Dim ValueiInDecimal As String
		Dim Val As Decimal
		If custValidator.ControlToValidate = "txtEndDate" Then
			If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
				Dim hour As Decimal
				hour = DateDiff(DateInterval.Minute, New SmartDate(txtStartDate.Text.ToString).Date, New SmartDate(txtEndDate.Text.ToString).Date)
				Try
					Val = nWOPeriod.ConvertStringToDecimal(1, 1, (New Period(1, hour, 1)).Value, False)
					If Val <= 0 Then
						custValidator.ErrorMessage = "End Date and Time should be later to Start Date and Time." '"Total Time can not be negative/zero."
						e.IsValid = False
					End If
				Catch ex As Exception
					custValidator.ErrorMessage = ex.Message
					e.IsValid = False
				End Try

			End If
		ElseIf custValidator.ControlToValidate = "txtTotalTime" Then
			Try
				ValueiInDecimal = nWOPeriod.ConvertStringToDecimal(1, 1, txtTotalTime.Text, False)

				If ValueiInDecimal < 0 Then
					custValidator.ErrorMessage = "Total Time can not be negative/zero."
					e.IsValid = False
				End If
			Catch ex As Exception
				custValidator.ErrorMessage = ex.Message
				e.IsValid = False
			End Try
		End If
	End Sub
	'---------------------------------
	Private Function CustomValidate1() As Boolean
		Dim strMSG As String = ""
		If Not mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.IsValid Then
			For i As Integer = 0 To mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
			Next

		End If

		If strMSG.Trim <> "" Then
			cvControlValidator.ErrorMessage = strMSG
			cvControlValidator.IsValid = False
			Return False
		End If
		Return True
	End Function
	Private Sub addAttributes()
		txtTotalTime.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtTotalTime').value,event)")
		'txtStartDate.Attributes.Add("onchange",  "ValidateDateText(document.getElementById('txtStartDate'),'txtStartDate_CalendarExtender')")
		'txtEndDate.Attributes.Add("onchange", "ValidateDateText(document.getElementById('txtEndDate'),'txtEndDate_CalendarExtender')")
	End Sub
	Public Sub updatePanels()
		upnlGridView.Update()
		upnlResourceDet.Update()
	End Sub
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		If mnWOJobResourceAllocation IsNot Nothing Then
			dgResouceDet.DataSource = mnWOJobResourceAllocation.WOJobResourceDetails
		End If
		txtDesignation.Text = mDesignationName
		DataBind()
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		GetSession()
		addAttributes()
		If Not IsPostBack And Session("sender") = "" Then
			DataFieldBind()
		End If
		ControlVisibility()
		updatePanels()
	End Sub

	Private Sub btnAddTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTop.Click

		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If Not Page.IsValid Then upnlValidationSummary.Update() : Exit Sub

		If Session("mResourceDetailEdit") = False Then

			If mnWOJobResourceAllocation.WOJobResourceDetails.Contains(txtStartDate.Text.ToString, txtEndDate.Text.ToString) Then
				''Dim msg1 As New SIMsgBox(Page, "Alert!", "Record is already entered between  date and time span for this Resource..", "", MsgBoxStyle.OkOnly)
				''msg1.ReplacePage = "wfnWOJobResourceDetail.aspx?BackPage4=wfnWOJobResourceAllocation.aspx" & "&BackPage3=" & Request.QueryString("BackPage3") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
				''msg1.Show()
				MSGBoxCtrl.show("Alert!", "Record is already entered between  date and time span for this Resource..", "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			mnWOJobResourceAllocation.WOJobResourceDetails.Add(mnWOJobResourceAllocation.ID)
			SetObject()
			If mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.IsValid Then
				dgResouceDet.DataSource = mnWOJobResourceAllocation.WOJobResourceDetails
				dgResouceDet.DataBind()
				Session("mnWOJobResourceAllocation") = mnWOJobResourceAllocation
				txtStartDate.Text = ""
				txtEndDate.Text = ""
				txtTotalTime.Text = ""
				upnlValidationSummary.Update()
			Else
				If Not CustomValidate1() Then

					mnWOJobResourceAllocation.WOJobResourceDetails.Remove(mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem)
					upnlValidationSummary.Update()
					Exit Sub
				End If
			End If
			'Else Contains
			'End if Contains

		Else
			If mnWOJobResourceAllocation.WOJobResourceDetails.Contains(txtStartDate.Text.ToString, txtEndDate.Text.ToString, mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.ID) Then
				''Dim msg1 As New SIMsgBox(Page, "Alert!", "Record is already entered between  date and time span for this Resource..", "", MsgBoxStyle.OkOnly)
				''msg1.ReplacePage = "wfnWOJobResourceDetail.aspx?BackPage4=wfnWOJobResourceAllocation.aspx" & "&BackPage3=" & Request.QueryString("BackPage3") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
				''msg1.Show()
				MSGBoxCtrl.show("Alert!", "Record is already entered between  date and time span for this Resource..", "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			'Clone
			Dim clnnWOJobResourceAllocation As New nWOJobResourceAllocation
			clnnWOJobResourceAllocation = mnWOJobResourceAllocation.Clone

			SetObject()
			If Not mnWOJobResourceAllocation.WOJobResourceDetails.CurrentItem.IsValid Then
				If Not CustomValidate1() Then
					mnWOJobResourceAllocation = clnnWOJobResourceAllocation
					Session("mnWOJobResourceAllocation") = clnnWOJobResourceAllocation
					dgResouceDet.DataSource = mnWOJobResourceAllocation.WOJobResourceDetails
					dgResouceDet.DataBind()
					clnnWOJobResourceAllocation = Nothing
					'SetFocus(dgResouceDet)
					upnlValidationSummary.Update()
					Exit Sub
				End If
			End If

			dgResouceDet.DataSource = mnWOJobResourceAllocation.WOJobResourceDetails

			dgResouceDet.DataBind()
			Session("mnWOJobResourceAllocation") = mnWOJobResourceAllocation
			SetFocus(dgResouceDet)
			Session("mResourceDetailEdit") = False

			txtStartDate.Text = ""
			txtEndDate.Text = ""
			txtTotalTime.Text = ""
		End If

		ControlVisibility()
		updatePanels()
		''Response.Redirect("wfnWOJobResourceDetail.aspx?BackPage4=wfnWOJobResourceAllocation.aspx" & "&BackPage3=" & Request.QueryString("BackPage3") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
	End Sub
	Private Sub dgResouceDet_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgResouceDet.RowCommand
		Dim Index As Int32 = dgResouceDet.PageIndex * dgResouceDet.PageSize + CInt(e.CommandArgument)
		Dim mID As Guid = mnWOJobResourceAllocation.WOJobResourceDetails(Index).ID
		Select Case e.CommandName
			Case "EditRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				Session("mResourceDetailEdit") = True
				EditRecord(Index)
				dgResouceDet.DataSource = mnWOJobResourceAllocation.WOJobResourceDetails
				dgResouceDet.DataBind()
				upnlGridView.Update()
			Case "DeleteRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				DeleteRecord(Index)
		End Select
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
		'Session.Remove("mnWOJobResourceAllocation")
		Session.Remove("mDesignationName")

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage4") & "?CPage1=" & Request.QueryString("CPage1") & "&BackPage3=" & Request.QueryString("BackPage3") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
	End Sub
	Private Sub txtStartDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStartDate.TextChanged
		'If txtStartDate.Text <> "" Then ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ValidateDateText", "ValidateDateText(document.getElementById('txtStartDate'),'txtStartDate_CalendarExtender');", True)

		If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
			'If Not IsDate(txtStartDate.Text) Then
			'    If CDate(txtStartDate.Text).CompareTo(CDate("1-Jan-1753")) > 0 And CDate(txtStartDate.Text).CompareTo(CDate("31-Dec-9999")) <= 0 Then
			'        txtStartDate.Text = CDate(txtStartDate.Text).ToString(AppSettings("DateFormat"))
			'    End If
			'End If

			'txtTotalTime.Text = DateDiff(DateInterval.Hour, New SmartDate(txtStartDate.Value.ToString).Date, New SmartDate(txtEndDate.Value.ToString).Date)
			'--Added By Utkarsh On 19-Jan-2011
			Dim hour As Decimal
			hour = DateDiff(DateInterval.Minute, New SmartDate(txtStartDate.Text.ToString).Date, New SmartDate(txtEndDate.Text.ToString).Date)
			txtTotalTime.Text = (New Period(1, hour, 0)).Value
			txtTotalTime.Enabled = False
			'----------------------------
		Else
			txtTotalTime.Enabled = True
		End If
	End Sub
	Private Sub txtEndDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEndDate.TextChanged
		If txtStartDate.Text <> "" And txtEndDate.Text <> "" Then
			'txtTotalTime.Text = DateDiff(DateInterval.Hour, New SmartDate(txtStartDate.Value.ToString).Date, New SmartDate(txtEndDate.Value.ToString).Date)

			'--Added By Utkarsh On 19-Jan-2011
			Dim hour As Decimal
			hour = DateDiff(DateInterval.Minute, New SmartDate(txtStartDate.Text.ToString).Date, New SmartDate(txtEndDate.Text.ToString).Date)
			txtTotalTime.Text = (New Period(1, hour, 0)).Value
			'----------------------------
			txtTotalTime.Enabled = False
		Else
			txtTotalTime.Enabled = True
		End If
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub
#End Region

End Class