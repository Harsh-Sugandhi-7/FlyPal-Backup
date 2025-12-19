Public Class wfDamageTypeList
	Inherits Page

#Region " Variable Declaration "

	Public mDamageType As DamageType
	Public mDamageTypeList As DamageTypeList
	Dim EventLogID As Guid

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		mDamageType = CType(Session("mDamageType"), DamageType)
		mDamageTypeList = CType(Session("mDamageTypeList"), DamageTypeList)

	End Sub

	Private Sub SetSession()

		Session("mDamageType") = mDamageType
		Session("mDamageTypeList") = mDamageTypeList

	End Sub

	Private Sub NewRecord()

		mDamageType = DamageType.NewDamageType(Guid.NewGuid)
		Session("mDamageType") = mDamageType
		txtName.Text = ""
		lblTitle.Text = "Damage Type [New]"
		upnlValidationSummary.Update()

	End Sub

	Private Sub EditRecord(mId As Guid)

		mDamageType = DamageType.GetDamageType(mId)
		Session("mDamageType") = mDamageType

		If Len(mDamageType.Name) > 15 Then
			lblTitle.Text = "Damage Type [" & mDamageType.Name.Substring(0, 15) & "...]"
		Else
			lblTitle.Text = "Damage Type [" & mDamageType.Name & "]"
		End If

		upnlValidationSummary.Update()

	End Sub

	Private Sub DeleteRecord(mId As Guid)

		MCategoryGridBind()
		mDamageType = DamageType.GetDamageType(mId)
		Session("mDamageType") = mDamageType

		MSGBoxCtrl.show(MSGBox.Message_title.Delete,
						MSGBox.Message_text.Delete,
						"",
						MsgBoxStyle.YesNo,
						"Delete")

	End Sub

	Private Sub SetObject()

		mDamageType.Name = Trim(txtName.Text)

	End Sub

	Private Sub MessageBoxResult()

		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then

			Select Case Result1

				Case MsgBoxResult.Yes

					If MSGBoxCtrl.Sender = "Delete" Then

						Try

							DamageType.DeleteDamageType(mDamageType.ID)
							NewRecord()
							DataFieldBind()
							upnlDamageType.Update()

						Catch ex As Exception

							MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
											MSGBox.Message_text.ReferenceDelete,
											"",
											MsgBoxStyle.OkOnly,
											"")
							NewRecord()

						Finally

							MarkLog(Action.Delete,
									"DamageType",
									mDamageType.Name,
									ErrorType.NoError,
									mDamageType.ID,
									EventLogID)

							NewRecord()

						End Try

					End If

				Case MsgBoxResult.No

					If MSGBoxCtrl.Sender = "Close" Then
						DataFieldBind()
					End If

					If MSGBoxCtrl.Sender = "Delete" Then

						txtName.Text = ""
						NewRecord()
						DataFieldBind()
						upnlDamageType.Update()

					End If

					MCategoryGridBind()

				Case MsgBoxResult.Ok

					MCategoryGridBind()

			End Select

		End If

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		mDamageTypeList = DamageTypeList.GetDamageTypeList()
		dgDamageTypeList.DataSource = mDamageTypeList
		Session("mDamageTypeList") = mDamageTypeList
		txtName.DataBind()
		upnlDamageType.Update()
		MCategoryGridBind()

	End Sub

	Private Sub MCategoryGridBind()

		dgDamageTypeList.DataSource = mDamageTypeList
		dgDamageTypeList.DataBind()
		lblResult.Text = "Damage Type List: " & mDamageTypeList.Count & " Record(s) Found."
		upnlGridView.Update()

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		txtName.Focus()

		If Not IsPostBack Then

			NewRecord()
			DataFieldBind()

		End If

	End Sub

	Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If IsValid Then

				SetObject()
				mDamageType.Save()

				MarkLog(Action.Save,
						"Damage Type",
						mDamageType.Name,
						ErrorType.NoError,
						mDamageType.ID,
						EventLogID)

				NewRecord()
				DataFieldBind()
				SetSession()

			Else
				upnlValidationSummary.Update()
			End If

		Catch ex As Exception

			MSGBoxCtrl.show(MSGBox.Message_title.Duplicate,
							MSGBox.Message_text.Duplicate,
							"You can not add duplicate entry in Damage Type.",
							MsgBoxStyle.OkOnly,
							"")

			DataFieldBind()
			Exit Sub

		End Try

	End Sub

	Private Sub GV_DamageTypeList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgDamageTypeList.RowCommand

		Select Case e.CommandName

			Case "EditView"

				Dim index As Integer = CInt(e.CommandArgument) + dgDamageTypeList.PageIndex * dgDamageTypeList.PageSize
				Dim mID As Guid = mDamageTypeList(index).ID
				'Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				Dim mName As String = mDamageTypeList(mID).Name

				EditRecord(mID)

				txtName.Focus()
				txtName.Text = mName
				txtName.DataBind()
				upnlDamageType.Update()
				MCategoryGridBind()

				MarkLog(Action.Edit,
						"mDamageType",
						mDamageType.Name,
						ErrorType.NoError,
						mDamageType.ID,
						EventLogID)

			Case "Remove"

				Dim index As Integer = CInt(e.CommandArgument) + dgDamageTypeList.PageIndex * dgDamageTypeList.PageSize
				Dim mID As Guid = mDamageTypeList(index).ID
				'Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				Dim mName As String = mDamageTypeList(mID).Name

				DeleteRecord(mID)

		End Select

	End Sub

	Private Sub GV_DamageTypeList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgDamageTypeList.PageIndexChanging

		dgDamageTypeList.PageIndex = e.NewPageIndex
		dgDamageTypeList.DataSource = mDamageTypeList
		dgDamageTypeList.DataBind()
		Session("mDamageTypeList") = mDamageTypeList
		upnlGridView.Update()

	End Sub

	Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAdd.Click

		MarkLog(Action.[New],
				"Damage Type",
				"", ErrorType.NoError,
				mDamageType.ID,
				EventLogID)

		NewRecord()
		DataFieldBind()
		txtName.Focus()

	End Sub

	Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click

		MarkLog(Action.Close, "DamageType", "", ErrorType.NoError, Guid.Empty, EventLogID)
		MCategoryGridBind()

		Dim mopenas As String = Request.QueryString("Type")

		If Not mopenas Is Nothing AndAlso mopenas = "pup" Then

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"onclose",
												"CallParentCallback();",
												True)

			Exit Sub

		End If

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

		MSGBoxCtrl.HideControl()
		MessageBoxResult()

	End Sub

#End Region

End Class