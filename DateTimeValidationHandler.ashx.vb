'Created by Saylee on 26-Aug-2014 to validate date Time...

Public Class DateTimeValidationHandler
	Implements System.Web.IHttpHandler

	Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
		Try
			Dim datestring As String = String.Empty
			datestring = context.Request.Params("Date").Trim
			Dim ReturnDefaultOnInvalidOrBlank As Boolean = False
			ReturnDefaultOnInvalidOrBlank = CBool(context.Request.Params("SetDefault").Trim)
			Dim returnDateString As String = String.Empty
			If IsDate(datestring) Then
				If datestring = String.Empty AndAlso ReturnDefaultOnInvalidOrBlank Then
					returnDateString = Today.Date.ToString(AppSettings("DateTimeFormatLOG"))
				Else
					If ReturnDefaultOnInvalidOrBlank Then
						If CDate(datestring).CompareTo(CDate("1-Jan-1753")) >= 0 And CDate(datestring).CompareTo(CDate("31-Dec-9999")) <= 0 Then
							returnDateString = CDate(datestring).ToString(AppSettings("DateTimeFormatLOG"))
						Else
							returnDateString = Today.Date.ToString(AppSettings("DateTimeFormatLOG"))
						End If

					Else
						If CDate(datestring).CompareTo(CDate("1-Jan-1753")) > 0 And CDate(datestring).CompareTo(CDate("31-Dec-9999")) <= 0 Then
							returnDateString = CDate(datestring).ToString(AppSettings("DateTimeFormatLOG"))
						End If
					End If
				End If
			Else
				If ReturnDefaultOnInvalidOrBlank Then
					returnDateString = Today.Date.ToString(AppSettings("DateTimeFormatLOG"))
				End If
			End If
			context.Response.ContentType = "text/plain"
			context.Response.Write(returnDateString)
		Catch ex As Exception
			context.Response.ContentType = "text/plain"
			context.Response.Write("")
		End Try
	End Sub
	ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
		Get
			Return False
		End Get
	End Property

End Class