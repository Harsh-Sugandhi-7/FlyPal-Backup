'************************************
'Created by:	Harsh Sugandhi
'Created on:	16th October 2025
'Created for:	common class for returning status wise messages.
'************************************


Imports System.Net
Imports System.Net.Http
Imports System.Web.Http


Friend Class StatusWiseReturnMessage
	Inherits ApiController


#Region " Method(s) "

	Protected Friend Function GenerateResponseMessage(returnMessage As ReturnMessage) As ResponseWrapper

		Dim request = HttpContext.Current?.Request
		Dim config = GlobalConfiguration.Configuration
		Try

			Select Case returnMessage.Status
				Case "Success"
					Return New ResponseWrapper With {
					.StatusCode = HttpStatusCode.OK,
					.ReturnMessage = returnMessage
				}

				Case "Validation"
					Return New ResponseWrapper With {
					.StatusCode = HttpStatusCode.BadRequest,
					.ReturnMessage = returnMessage
				}

				Case "Exception", "SqlException"
					Return New ResponseWrapper With {
					.StatusCode = HttpStatusCode.InternalServerError,
					.ReturnMessage = returnMessage
				}

				Case Else
					Return New ResponseWrapper With {
					.StatusCode = HttpStatusCode.InternalServerError,
					.ReturnMessage = New ReturnMessage(Status:="Unknown", Message:=$"Unexpected status: {returnMessage.Status}")
				}
			End Select

		Catch ex As Exception
			Throw ex
		End Try

	End Function

#End Region

End Class


