'************************************
'Created By : Saylee
'Date:     : 7-Feb-2019
'************************************


Public Class AttachmentListPage
	Inherits Page

#Region " Variable(s) "

	Public TaskCard As TaskCard
	Public FileAttachments As FileAttachments
	Public AttachmentHelper As New AttachmentHelper

	Dim EventLogID As Guid
	Dim TransactionName As String
	Dim TransactionDetails As String
	Dim TransactionNameMarkLog As String
	Dim IsFromWOJobTask As Boolean = False

#End Region

#Region " Helper Method(s) "

	Private Sub GetSession()

		FileAttachments = Session("mFileAttachments")
		TransactionNameMarkLog = Session("TransactionNameMarkLog")
		TransactionName = Session("TransactionName")
		TransactionDetails = Session("TransactionDetails")
		TaskCard = Session("mTaskCard")
		IsFromWOJobTask = IIf(Session("IsFromWOJobTask") Is Nothing Or Session("IsFromWOJobTask") = "", False, True)

	End Sub

	Private Sub RemoveSession()

		Session.Remove("mFileAttachments")
		Session.Remove("TransactionNameMarkLog")
		Session.Remove("TransactionName")
		Session.Remove("TransactionDetails")
		Session.Remove("mTaskCard")
		Session.Remove("IsFromWOJobTask")

	End Sub

	Private Sub MessageBoxResult()

		Try

			Dim MsgBoxResult As MsgBoxResult
			MsgBoxResult = MSGBoxCtrl.Result
			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

					Case MsgBoxResult.Ok

					Case MsgBoxResult.No

				End Select

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " DataFieldBind "

	Private Sub DataFieldBind(Optional GetList As Boolean = True)

		Try

			If IsFromWOJobTask Then
				dgAttachment.DataSource = TaskCard.TaskCardAttachments
			Else
				dgAttachment.DataSource = FileAttachments
			End If

			DataBind()

			lblNo2.Text = TransactionName
			lblNo1.Text = TransactionDetails
			lblRecords.Text = $"List of Attachments ( {dgAttachment.Rows.Count} )"

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Event(s) "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try

			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid)

			If Not Page.IsPostBack Then

				DataFieldBind()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnBack.Click

		Try

			RemoveSession()
			Dim Type As String = Request.QueryString("Type")

			If Type IsNot Nothing AndAlso Type = "pup" Then
				ScriptManager.RegisterStartupScript(Me, [GetType], "On Close", "CallParentCallback();", True)
				Exit Sub
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

	Private Sub GV_Attachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgAttachment.RowCommand

		Try

			Dim Detail As String
			Dim CommonObject As Object
			Dim FileStream As FileStream
			Dim ModuleName As String = ""
			Dim Index As Integer = CInt(e.CommandArgument)

			Select Case e.CommandName

				Case "View"

					If IsFromWOJobTask Then

						ModuleName = "WOJobTask"
						CommonObject = TaskCard.TaskCardAttachments
						Detail = $"Attachment( { TaskCard.TaskCardAttachments.CurrentItem.FileName} ) viewed by {User.Identity.Name}"

					Else

						FileAttachments = Session("mFileAttachments")
						TransactionNameMarkLog = Session("TransactionNameMarkLog")

						ModuleName = "Multiple Attachments"
						CommonObject = FileAttachments
						Detail = $"Attachment( {FileAttachments.CurrentItem.FileName} ) viewed by {User.Identity.Name}"

					End If

					AttachmentHelper.DownloadAttachmentWithName(Index:=Index,
													   ModuleName:=ModuleName,
													   AttachmentObject:=CommonObject)

					ScriptManager.RegisterStartupScript(Me, [GetType], "Open File", "openFile();", True)

					MarkLog(Action.View,
							TransactionNameMarkLog,
							Detail,
							ErrorType.HandledError,
							FileAttachments.CurrentItem.ReferenceID,
							EventLogID)

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class