Imports System.Web.Http
Imports System.Web.Script.Services


Public Class MeController
	Inherits ApiController

#Region " Get Method(s) "

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValue() As MeMessage

		Dim mMeMessage = New MeMessage()

		Return mMeMessage

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Sub PostValue(<FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(ID As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Sub DeleteValue(ID As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class

#Region " Message Class "

Public Class MeMessage

	Public IPrincipal As System.Security.Principal.IPrincipal
	Public EventLogID As Guid

	Public Sub New()

		HttpContext.Current.User = Thread.CurrentPrincipal
		IPrincipal = HttpContext.Current.User
		EventLogID = New Guid(HttpContext.Current.Session("EventLogID").ToString)

	End Sub

End Class

#End Region