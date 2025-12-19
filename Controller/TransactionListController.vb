Imports System.Web.Http

Public Class TransactionListController
	Inherits ApiController

	<HttpGet>
	Public Function GetTransactionList(Optional AddTopItem As String = "<SELECT>") As TransactionListForAPI

		Try

			Return TransactionListForAPI.GetTransactionListForAPI(AddTopItem:="Select")

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	Public Function GetValue(id As Integer) As String
		Return "value"
	End Function

	Public Sub PostValue(<FromBody()> value As String)

	End Sub

	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub

	Public Sub DeleteValue(id As Integer)

	End Sub

End Class
