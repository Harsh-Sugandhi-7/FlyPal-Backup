Imports System.Web.Http

Public Class StatusListController
	Inherits ApiController

	Public Function GetStatusList(Type As Integer,
								  Optional From As Integer = 0,
								  Optional IsSelectTagRequired As Boolean = False) As StatusList

		Try

			Return StatusList.GetStatusList(Type:=Type,
											From:=From,
											IsSelectTagRequired:=IsSelectTagRequired)

		Catch ex As Exception
			Throw ex.GetBaseException()
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
