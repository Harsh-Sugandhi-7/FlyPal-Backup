'Created by Utkarsh on 30-Dec-2013 to validate Between dates(From Date - To Date)

Public Class BetweenDateValidationHandler
	Implements System.Web.IHttpHandler

	Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
		Try
			Dim fromDate As Date
			Dim ToDate As Date
			Dim strFromDate As String = context.Request.Params("FromDate").Trim
			Dim strToDate As String = context.Request.Params("ToDate").Trim
			If IsDate(strFromDate) AndAlso IsDate(strToDate) Then
				fromDate = CDate(strFromDate)
				ToDate = CDate(strToDate)
				If fromDate > ToDate Then
					GoTo wrongInput
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