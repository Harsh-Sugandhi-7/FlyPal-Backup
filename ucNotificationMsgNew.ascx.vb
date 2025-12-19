Public Class NotificationMsgNew
	Inherits UserControl

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

	End Sub

	Public Sub ShowNotification(_Title As String, _Message As String)

		Try

			lblNotificationTitle.Text = _Title
			lblNotification.Text = _Message

			Dim Script As String = $"ShowMessage('{_Title}', '{_Message}');"
			ScriptManager.RegisterStartupScript(Me, [GetType], "Focus Script", Script, True)
			upnlNotifier.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

End Class