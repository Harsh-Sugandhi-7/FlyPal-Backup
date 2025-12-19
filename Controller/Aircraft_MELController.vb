'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Web.Http
Imports System.Web.Script.Services


Public Class Aircraft_MELController
	Inherits ApiController

#Region " Get Method(s) "

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELCategoryList(AddTopItem As String,
									   Optional IsHours As Boolean = False) As MELCategoryList

		Try

			Return MELCategoryList.GetMELCategoryList(AddTopItem:=AddTopItem,
													  IsHours:=IsHours)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetMELCategoryByID(MELCategoryID As Integer,
									   IsByID As Boolean) As MELCategoryList

		Try

			Return MELCategoryList.GetMELCategoryByID(MELCategoryID:=MELCategoryID,
													  IsByID:=IsByID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetListOfMELPart(Optional ModelID As String = "{00000000-0000-0000-0000-000000000000}",
									 Optional ATAID As String = "{00000000-0000-0000-0000-000000000000}",
									 Optional SubATA As String = "{00000000-0000-0000-0000-000000000000}",
									 Optional ItemSequenceNo As String = "",
									 Optional Description As String = "",
									 Optional MELCategoryID As Integer = -1,
									 Optional RevisionNo As String = "",
									 Optional PrimaryModelID As String = "{00000000-0000-0000-0000-000000000000}") As MELList

		Try

			Return MELList.GetListOfMELPart(ModelID:=ModelID,
											ATAID:=ATAID,
											SubATA:=SubATA,
											ItemSequenceNo:=ItemSequenceNo,
											Description:=Description,
											MELCategoryID:=MELCategoryID,
											RevisionNo:=RevisionNo,
											PrimaryModelID:=PrimaryModelID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetDeviationCategoryList(AddTopItem As String,
											 Optional IsHours As Boolean = False) As DeviationCategoryList

		Try

			Return DeviationCategoryList.GetDeviationCategoryList(AddTopItem:=AddTopItem,
																  IsHours:=IsHours)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetDeviationLists(Optional ModelID As String = "{00000000-0000-0000-0000-000000000000}",
									  Optional ATAID As String = "{00000000-0000-0000-0000-000000000000}",
									  Optional SubATA As String = "{00000000-0000-0000-0000-000000000000}",
									  Optional Description As String = "",
									  Optional DeviationCategoryID As Integer = -1,
									  Optional ItemSequenceNo As String = "",
									  Optional RevisionNo As String = "") As DeviationLists

		Try

			Return DeviationLists.GetDeviationLists(ModelID:=ModelID,
													ATAID:=ATAID,
													SubATA:=SubATA,
													Description:=Description,
													DeviationCategoryID:=DeviationCategoryID,
													ItemSequenceNo:=ItemSequenceNo,
													RevisionNo:=RevisionNo)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub PostValue(<FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Put Method(s) "

	<HttpPut>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Sub DeleteValue(id As Integer)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

End Class
