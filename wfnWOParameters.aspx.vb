Public Class wfnWOParameters
	Inherits System.Web.UI.Page

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

#Region "Variable Declarations"
	Dim mcsTaskParameterList As ncsWOParametersList
	Dim mcsRequestsParameterList As ncsWOParametersList
	Dim mcsStatisticsParameterList As ncsWOParametersList

	Dim mnWOTaskParameterList As nWOParameterList
	Dim mnWORequestsParameterList As nWOParameterList
	Dim mnWOStatisticsParameterList As nWOParameterList

	Public mWOID As Guid = Guid.Empty
	Protected mnWO As nWO

#End Region


#Region " Business Methods "
	Private Sub GetSession()
		mcsTaskParameterList = Session("mcsTaskParameterList")
		mcsRequestsParameterList = Session("mcsRequestsParameterList")
		mcsStatisticsParameterList = Session("mcsStatisticsParameterList")

		mnWOTaskParameterList = Session("mnWOTaskParameterList")
		mnWORequestsParameterList = Session("mnWORequestsParameterList")
		mnWOStatisticsParameterList = Session("mnWOStatisticsParameterList")


		mWOID = Session("mWOID")
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
	Private Sub SetSession()
		Session("mcsTaskParameterList") = mcsTaskParameterList
		Session("mcsRequestsParameterList") = mcsRequestsParameterList
		Session("mcsStatisticsParameterList") = mcsStatisticsParameterList

		Session("mnWOTaskParameterList") = mnWOTaskParameterList
		Session("mnWORequestsParameterList") = mnWORequestsParameterList
		Session("mnWOStatisticsParameterList") = mnWOStatisticsParameterList

		Session("mWOID") = mWOID
		Session("mnWO") = mnWO
	End Sub
	Private Function CustomValidate1(ByVal index As Integer) As Boolean
		Dim strMSG As String = ""
		If Not mnWOTaskParameterList(index).IsValid Then
			For i As Integer = 0 To mnWOTaskParameterList(index).GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWOTaskParameterList(index).GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If

		If Not mnWORequestsParameterList(index).IsValid Then
			For i As Integer = 0 To mnWORequestsParameterList(index).GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWORequestsParameterList(index).GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If

		If Not mnWOStatisticsParameterList(index).IsValid Then
			For i As Integer = 0 To mnWOStatisticsParameterList(index).GetBrokenRulesCollection.Count - 1
				strMSG = strMSG + mnWOStatisticsParameterList(index).GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If

		If strMSG.Trim <> "" Then
			cvControlValidator.ErrorMessage = strMSG
			cvControlValidator.IsValid = False
			upnlValidationSummary.Update()
			Return False
		End If
		Return True
	End Function
	Private Sub SetCheckBoxes()
		'Tasks
		For i As Integer = 0 To chkTasksList.Items.Count - 1
			If mnWOTaskParameterList.Contains(chkTasksList.Items(i).Text) Then
				chkTasksList.Items(i).Selected = True
			End If
		Next

		'Requests
		For i As Integer = 0 To chkRequestsList.Items.Count - 1
			If mnWORequestsParameterList.Contains(chkRequestsList.Items(i).Text) Then
				chkRequestsList.Items(i).Selected = True
			End If
		Next

		'Statistics
		For i As Integer = 0 To chkStatistics.Items.Count - 1
			If mnWOStatisticsParameterList.Contains(chkStatistics.Items(i).Text) Then
				chkStatistics.Items(i).Selected = True
			End If
		Next
	End Sub
	Private Function SetObject() As Boolean
		'Tasks
		Try
			For i As Integer = 0 To chkTasksList.Items.Count - 1
				If (chkTasksList.Items(i).Selected = True) And (Not mnWOTaskParameterList.Contains(chkTasksList.Items(i).Text)) Then
					Dim mnWOTaskParameter As nWOParameter
					mnWOTaskParameter = nWOParameter.NewParameter(mnWO.ID)
					mnWOTaskParameter.SectionName = mcsTaskParameterList(CInt(Val(chkTasksList.Items(i).Value)), "").SectionName
					mnWOTaskParameter.WOParameterID = CInt(Val(chkTasksList.Items(i).Value))
					mnWOTaskParameter.Save()

				ElseIf (chkTasksList.Items(i).Selected = False) And (mnWOTaskParameterList.Contains(chkTasksList.Items(i).Text)) Then
					'Dim mnWOTaskParameter As nWOParameter
					nWOParameter.DeleteParameter(mnWOTaskParameterList(chkTasksList.Items(i).Text).ID)
				End If
			Next

			'Requests
			For i As Integer = 0 To chkRequestsList.Items.Count - 1
				If (chkRequestsList.Items(i).Selected = True) And (Not mnWORequestsParameterList.Contains(chkRequestsList.Items(i).Text)) Then
					Dim mnWORequestsParameter As nWOParameter
					mnWORequestsParameter = nWOParameter.NewParameter(mnWO.ID)
					mnWORequestsParameter.SectionName = mcsRequestsParameterList(CInt(Val(chkRequestsList.Items(i).Value)), "").SectionName
					mnWORequestsParameter.WOParameterID = CInt(Val(chkRequestsList.Items(i).Value))
					mnWORequestsParameter.Save()

				ElseIf (chkRequestsList.Items(i).Selected = False) And (mnWORequestsParameterList.Contains(chkRequestsList.Items(i).Text)) Then
					nWOParameter.DeleteParameter(mnWORequestsParameterList(chkRequestsList.Items(i).Text).ID)
				End If
			Next

			'Statistics
			For i As Integer = 0 To chkStatistics.Items.Count - 1
				If (chkStatistics.Items(i).Selected = True) And (Not mnWOStatisticsParameterList.Contains(chkStatistics.Items(i).Text)) Then
					Dim mnWOStatisticsParameter As nWOParameter
					mnWOStatisticsParameter = nWOParameter.NewParameter(mnWO.ID)
					mnWOStatisticsParameter.SectionName = mcsStatisticsParameterList(CInt(Val(chkStatistics.Items(i).Value)), "").SectionName
					mnWOStatisticsParameter.WOParameterID = CInt(Val(chkStatistics.Items(i).Value))
					mnWOStatisticsParameter.Save()

				ElseIf (chkStatistics.Items(i).Selected = False) And (mnWOStatisticsParameterList.Contains(chkStatistics.Items(i).Text)) Then
					nWOParameter.DeleteParameter(mnWOStatisticsParameterList(chkStatistics.Items(i).Text).ID)
				End If
			Next
			Return True
		Catch ex As Exception
			Return False
		End Try
		Return False
	End Function
#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()
		mcsTaskParameterList = ncsWOParametersList.GetWOParametersList("Tasks")
		mcsRequestsParameterList = ncsWOParametersList.GetWOParametersList("Requests")
		mcsStatisticsParameterList = ncsWOParametersList.GetWOParametersList("Statistics")

		chkTasksList.DataSource = mcsTaskParameterList
		chkRequestsList.DataSource = mcsRequestsParameterList
		chkStatistics.DataSource = mcsStatisticsParameterList

		DataBind()

		mnWOTaskParameterList = nWOParameterList.GetWOParameterList(mnWO.ID, "Tasks")
		mnWORequestsParameterList = nWOParameterList.GetWOParameterList(mnWO.ID, "Requests")
		mnWOStatisticsParameterList = nWOParameterList.GetWOParameterList(mnWO.ID, "Statistics")

		Session("mnWOTaskParameterList") = mnWOTaskParameterList
		Session("mnWORequestsParameterList") = mnWORequestsParameterList
		Session("mnWOStatisticsParameterList") = mnWOStatisticsParameterList

		SetSession()
		SetCheckBoxes()

	End Sub

#End Region

#Region "Events"
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		GetSession()
		If Not IsPostBack Then
			' mWOID = New Guid(CType(Request.QueryString("WOID"), String))
			Session("mWOID") = mnWO.ID
			SetFocus(chkTasksList)
			DataFieldBind()
			If Session("MiddleFrame") = "wfnWOCreateList.aspx?TransTypeID=" & mnWO.TransTypeID Then
				btnSave.Visible = IIf(mnWO.StatusID = 2, False, True)
			ElseIf Session("MiddleFrame") = "wfnWOPlannedList.aspx?" Or Session("MiddleFrame") = "wfnWOCompletionList.aspx?" Then
				btnSave.Visible = True
			ElseIf Session("MiddleFrame") = "wfnWOExecutionList.aspx" Or Session("MiddleFrame") = "wfnWOQCApprovalList.aspx?" Or Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=1" Or Session("MiddleFrame") = "wfnWOCAMOUpdatList.aspx?IsForCAMOUpdate=0" Then
				btnSave.Visible = False
			Else
				btnSave.Visible = IIf(mnWO.WOStatusID = 3, False, True) And IIf(mnWO.StatusID = 4, False, True)
			End If
		End If
	End Sub
	Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			Dim mWODetail As String
			'MarkLog(Util.Action.Save, "Work Order", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
			mWODetail = mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy
			MarkLog(Util.Action.Save, "Work Order", User.Identity.Name & " is not Authorized User to save " & mWODetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
		If SetObject() Then
			DataFieldBind()
			MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
		End If
		'Dim mopenas As String = Request.QueryString("Type")
		'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
		'    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
		'    Exit Sub
		'End If
	End Sub
#End Region

	Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End
	End Sub
End Class