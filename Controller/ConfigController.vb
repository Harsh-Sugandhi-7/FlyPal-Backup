Imports System.Web.Http


Public Class ConfigController
	Inherits ApiController

	Public Function GetValues() As WebConfigKeys

		Try

			Return New WebConfigKeys()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function GetValue(ID As Integer) As String
		Return "value"
	End Function

	Public Sub PostValue(<FromBody()> value As String)

	End Sub

	Public Sub PutValue(ID As Integer, <FromBody()> value As String)

	End Sub

	Public Sub DeleteValue(ID As Integer)

	End Sub

End Class
