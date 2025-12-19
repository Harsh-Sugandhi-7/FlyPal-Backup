Imports System.Web.Http
Imports System.Web.Script.Services

Imports SHROInformatics

Public Class AmountINWordController
	Inherits ApiController

#Region "GET Function"
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValues(ByVal mCTotal As Decimal, ByVal mCurrencyID As Guid) As String
		Return AmountINWord.AmountINWords(mCTotal:=mCTotal, mCurrencyID:=mCurrencyID)
	End Function

	' GET api/<controller>/5
	Public Function GetValue(ByVal id As Integer) As String
		Return "value"
	End Function
#End Region

#Region " POST Method(s) "
	' POST api/<controller>
	Public Sub PostValue(<FromBody()> ByVal value As String)

	End Sub
#End Region

#Region " PUT Method(s) "
	' PUT api/<controller>/5
	Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

	End Sub
#End Region

#Region " DELETE Method(s) "
	' DELETE api/<controller>/5
	Public Sub DeleteValue(ByVal id As Integer)

	End Sub
#End Region

End Class
