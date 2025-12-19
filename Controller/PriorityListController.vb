Imports System.Web.Http
Imports System.Web.Script.Services

Public Class PriorityListController
	Inherits ApiController

#Region " Get Method(s) "

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	<Route("api/PriorityList/GetValues")>
	<Route("api/PriorityList/List")>
	<Route("api/PriorityList")>
	Public Function GetValues(TypeID As Integer,
							  Optional Name As String = "",
							  Optional AddTopItem As String = "<SELECT>") As PriorityList

		Return PriorityList.GetPriorityList(TypeID,
											Name,
											AddToppitem:=AddTopItem)
	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValue(id As Guid) As String
		Return "value"
	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Sub Save(<FromBody()> value As String)

	End Sub

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(ID As Integer, <FromBody()> value As String)

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Sub DeleteValue(id As Integer)

	End Sub

#End Region

End Class
