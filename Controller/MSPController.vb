Imports System.Web.Http
Imports System.Web.Script.Services

Public Class MSPController
	Inherits ApiController

	' GET api/<controller>
	Public Function GetValues() As String
		Return ""
	End Function

	' GET api/<controller>/5
	Public Function GetValue(ByVal id As Integer) As String
		Return "value"
	End Function

	' POST api/<controller>
	Public Sub PostValue(<FromBody()> ByVal value As String)

	End Sub

	' PUT api/<controller>/5
	Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

	End Sub

	' DELETE api/<controller>/5
	Public Sub DeleteValue(ByVal id As Integer)

	End Sub


	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	<CLSCompliant(False)>
	Public Function GetMSPAssemblyListForSelection(Optional ByVal MSPID As String = "{00000000-0000-0000-0000-000000000000}", Optional AsOnDate As String = "1-Jan-1900") As MSPAssemblyListForSelection
		Return MSPAssemblyListForSelection.GetMSPAssemblyListForSelection(MSPID, AsOnDate)
	End Function


End Class
