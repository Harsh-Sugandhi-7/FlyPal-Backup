Public Class betweenTimeValidation1
	Implements System.Web.IHttpHandler

	Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
		Try
			Dim fromDate As Date
			Dim ToDate As Date
			Dim fDateHour As Integer
			Dim TDateHour As Integer
			Dim fDateMin As Integer
			Dim TDateMin As Integer
			Dim strFromDate As String = context.Request.Params("FromDate").Trim
			Dim strToDate As String = context.Request.Params("ToDate").Trim
			If IsDate(strFromDate) AndAlso IsDate(strToDate) Then
				fromDate = CDate(strFromDate)
				ToDate = CDate(strToDate)
				fDateHour = DateAndTime.Hour(strFromDate)
				TDateHour = DateAndTime.Hour(strToDate)
				fDateMin = DateAndTime.Minute(strFromDate)
				TDateMin = DateAndTime.Minute(strToDate)
				'If fromDate > ToDate Then
				'    GoTo wrongInput
				If (fDateHour > 60 Or TDateHour > 60) And (fDateMin > 60 Or TDateMin > 60) Then
					GoTo WrongInput
				ElseIf fromDate > ToDate Then
					GoTo WrongInput

				Else
					context.Response.ContentType = "text/plain"
					context.Response.Write("True")
					Exit Sub
				End If
			Else
				GoTo WrongInput
			End If
WrongInput:
			context.Response.ContentType = "text/plain"
			context.Response.Write("False")

		Catch ex As Exception
			context.Response.ContentType = "text/plain"
			context.Response.Write("False")
		End Try

	End Sub

	ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
		Get
			Return False
		End Get
	End Property

End Class