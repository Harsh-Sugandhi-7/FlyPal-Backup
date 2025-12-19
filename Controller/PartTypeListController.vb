Imports System.Web.Http

Public Class PartTypeListController
	Inherits ApiController


	Public Function GetValues(Optional IsSelectTagRequired As Boolean = False) As PartTypeList
		Return PartTypeList.GetPartTypeList(IsSelectTagRequired)
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
