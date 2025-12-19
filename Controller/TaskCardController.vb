'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Web.Http


Public Class TaskCardController
	Inherits ApiController


#Region " Get Method(s) "

	<HttpGet>
	Public Function GetValues() As String

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetValue(id As Integer) As String

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

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
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Sub DeleteValue(id As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class