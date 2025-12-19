

Public Class wfMELSnagCorrectiveActionLogInfo_AJAX
	Inherits Page

#Region "Variable Declaration"

	Public MELSnagCorrectiveActionLog As MELSnagCorrectiveActionLog

	Dim LogID As String

#End Region

#Region "Business Methods"

	Private Sub GetSession()
		MELSnagCorrectiveActionLog = CType(Session("MELSnagCorrectiveActionLog"), MELSnagCorrectiveActionLog)
		LogID = Session("mTempLogID")
	End Sub

	Private Sub SetSession()
		Session("MELSnagCorrectiveActionLog") = MELSnagCorrectiveActionLog
	End Sub

	Private Sub RemoveSession()
		MELSnagCorrectiveActionLog = Nothing
	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			MELSnagCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(LogID)
			dgLogList.DataSource = MELSnagCorrectiveActionLog
			Session("MELSnagCorrectiveActionLog") = MELSnagCorrectiveActionLog
			dgLogList.Columns(1).HeaderText = IIf(Expression:=AppSettings("ClientCode") = "7AR", "Log Date (UTC)", "Log Date")
			dgLogList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()

			If Not IsPostBack Then
				DataFieldBind()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub CloseModal(sender As Object, e As EventArgs) Handles btnClose.Click

		Session("mlnkCheckStatus") = True
		Try

			Dim Type As String = Request.QueryString("Type")
			If Type IsNot Nothing AndAlso Type = "pup" Then

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"On Close",
													"CallParentCallback();",
													True)
				Exit Sub

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

End Class