'***********************************
'Created by Utkarsh on 27-Nov-2013 to validate date...
'Modified by Harsh Sugandhi for FLYPAL-2439 Highlight Not Working Employee.
'***********************************

Public Class DateValidationHandler
	Implements IHttpHandler

#Region " Propertie(s) "

	ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
		Get
			Return False
		End Get
	End Property

#End Region

#Region " Method(s) "

	Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest

		Try

			Dim DateString As String = String.Empty
			DateString = context.Request.Params("Date").Trim
			DateString = DateString.Replace(" ", "") 'Remove spaces In-Between date string
			Dim ReturnDefaultOnInvalidOrBlank As Boolean = False
			ReturnDefaultOnInvalidOrBlank = CBool(context.Request.Params("SetDefault").Trim)
			Dim returnDateString As String = String.Empty

			If IsDate(DateString) Then

				If DateString = String.Empty AndAlso ReturnDefaultOnInvalidOrBlank Then
					returnDateString = Today.Date.ToString(AppSettings("DateFormat"))
				Else

					If ReturnDefaultOnInvalidOrBlank Then

						If CDate(DateString).CompareTo(CDate("1-Jan-1753")) >= 0 And CDate(DateString).CompareTo(CDate("31-Dec-9999")) <= 0 Then
							returnDateString = CDate(DateString).ToString(AppSettings("DateFormat"))
						Else
							returnDateString = Today.Date.ToString(AppSettings("DateFormat"))
						End If

					Else

						If CDate(DateString).CompareTo(CDate("1-Jan-1753")) > 0 And CDate(DateString).CompareTo(CDate("31-Dec-9999")) <= 0 Then
							returnDateString = CDate(DateString).ToString(AppSettings("DateFormat"))
						End If

					End If

				End If

			Else

				If ReturnDefaultOnInvalidOrBlank Then
					returnDateString = Today.Date.ToString(AppSettings("DateFormat"))
				End If

			End If

			context.Response.ContentType = "text/plain"
			context.Response.Write(returnDateString)

		Catch ex As Exception
			context.Response.ContentType = "text/plain"
			context.Response.Write("")
		End Try

	End Sub

#End Region

End Class