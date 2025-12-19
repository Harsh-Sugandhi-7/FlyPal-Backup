'CREATED By : Saylee
'Dated      : 29-May-2019

Public Class wfnWOJobCompList
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
			IsInRoleString = "CAMOWOCreate"
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
		upnldgWOJobComps.Update()
	End Sub
	Private Sub ControlVisibility()
		btnAddWORemInst.Enabled = IIf(mnWO.IsThirdParty, False, True) And mnWO.WOStatusID <> 3
		dgWOJobComps.Columns(8).Visible = IIf(mnWO.IsThirdParty, False, True)
		dgWOJobComps.Columns(9).Visible = IIf(mnWO.IsThirdParty, False, True) And mnWO.WOStatusID <> 3
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
	Private Sub WOJobCompsDelete(ByVal Index As Int32)
		'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.RemoveItem, SIMsgBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo)
		''msg1.ReplacePage = "wfnWOJobDetail_AJAX.aspx?BackPage1=wfnWODetail.aspx" & "&BackPage=" & Request.QueryString("BackPage")
		'msg1.ReplacePage = "wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
		'Session("sender") = "WOJobCompsDelete"
		'msg1.Show()
		mnWO.WOJobs.CurrentItem.WOJobComps.CurrentIndex = Index
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "WOJobCompsDelete")
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
					If MSGBoxCtrl.Sender = "WOJobCompsDelete" Then                      'WO Job Tasks Delete
						Try
							Session("Sender") = ""
							mnWO.WOJobs.CurrentItem.WOJobComps.Remove(mnWO.WOJobs.CurrentItem.WOJobComps.CurrentIndex)
							Session("mnWOJob") = mnWO.WOJobs.CurrentItem

							''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
							ControlVisibility()
							DataFieldBind()
							upnldgWOJobComps.Update()
						Catch ex As Exception
							ex.GetBaseException()
						End Try

					ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
						Session("sender") = ""
						If mnWO.WOJobs.CurrentItem.IsValid = True Then
							Session.Remove("IsValid")
						Else
							Session.Remove("IsValid")
							''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
							ControlVisibility()
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
						If mnWO.WOJobs.CurrentItem.IsNew And mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 Then
							mnWO.WOJobs.Remove(mnWO.WOJobs.CurrentItem)
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
						DataFieldBind()
						''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
					End If

			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			ControlVisibility()
			DataFieldBind()
			CallUpdatePanels()
			''Response.Redirect("wfnWOJobDetail_AJAX.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage"))
		ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then
			Session("sender") = ""
			DataFieldBind()
		End If
	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()
		dgWOJobComps.DataSource = mnWO.WOJobs.CurrentItem.WOJobComps
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
	End Sub
	Private Sub dgWOJobComps_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgWOJobComps.RowCommand

		Select Case e.CommandName
			Case "EditRecord"
				Dim Index As Integer = CType(e.CommandArgument, Integer)

				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If

				mnWO.WOJobs.CurrentItem.WOJobComps.CurrentIndex = Index
				Session("mWOJobCompsEdit") = True
				Session("mnWOJob") = mnWO.WOJobs.CurrentItem
				'Response.Redirect("wfnWOJobTask_AJAX.aspx?BackPage2=wfnWOJobDetail_AJAX.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&Index=" & index)
				ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddJobCompDetail", "OpenToAddJobCompDetail();", True)
			Case "DeleteRecord"
				'Added by Saylee on 7-Mar-2014 for ALL07032014
				If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
					SetSession()
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
					Exit Sub
				End If
				Dim Index As Integer = CType(e.CommandArgument, Integer)

				WOJobCompsDelete(Index)

		End Select
	End Sub

	Private Sub btnAddWORemInst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddWORemInst.Click
		'Added by Saylee on 7-Mar-2014 for ALL07032014
		If (Not IsInRole(Rights.[New]) And mnWO.IsNew) Or (Not IsInRole(Rights.Edit) And Not mnWO.IsNew) Then
			SetSession()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		'Response.Redirect("wfnWOJobTask_AJAX.aspx?BackPage2=wfnWOJobDetail_AJAX.aspx" & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&Index=-1")
		Session("mIndex") = "-1"
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToAddRemInstDetail", "OpenToAddRemInstDetail();", True)


	End Sub

	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

		If mnWO.WOJobs.CurrentItem.IsNew And mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 Then
			mnWO.WOJobs.Remove(mnWO.WOJobs.CurrentItem)
		End If

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
			Exit Sub
		End If

		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))

	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub

#End Region


End Class