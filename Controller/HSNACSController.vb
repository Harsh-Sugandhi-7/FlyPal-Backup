Imports System.Web.Http
Imports System.Web.Script.Services

Public Class HSNACSController
	Inherits ApiController

	' GET api/<controller>
	<CLSCompliant(False)>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValues(Optional ByVal Code As String = "", Optional ByVal Description As String = "",
								  Optional ByVal AddTopItem As String = "") As HSNACSList
		Return HSNACSList.GetHSNACSList(Code:=Code, Description:=Description, AddTopItem:=AddTopItem)
	End Function
	' GET api/<controller>/5
	<CLSCompliant(False)>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValue(ByVal id As Guid) As HSNACS
		Return HSNACS.GetHSNACS(id)
	End Function


	<CLSCompliant(False)>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetPercentage(ByVal TransactionDate As String,
								  ByVal Type As Integer,
								  Optional ByVal ItemID As String = "00000000-0000-0000-0000-000000000000",
								  Optional ByVal AccountHeadID As String = "00000000-0000-0000-0000-000000000000") As GSTPercentage
		Return GSTPercentage.GetPercentage(TransactionDate:=TransactionDate, Type:=Type, ItemID:=ItemID, AccountHeadID:=AccountHeadID)
	End Function
	'<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	'Public Function GetValue(ByVal id As Integer) As String
	'    Return "value"
	'End Function
	' POST api/<controller>
	Public Sub PostValue(<FromBody()> ByVal value As String)

	End Sub

	' PUT api/<controller>/5
	Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

	End Sub

	' DELETE api/<controller>/5
	Public Sub DeleteValue(ByVal id As Integer)

	End Sub
End Class
