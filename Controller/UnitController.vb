Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq

Public Class UnitController
	Inherits ApiController


#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " GET Method(s) "

	<HttpGet>
	Public Function GetValues(Optional ItemID As String = "",
							  Optional AddTopItem As String = "") As UnitConverterList

		Try

			Return UnitConverterList.GetUnitConverterList(ItemID:=New Guid(ItemID),
													  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetUnitList(Optional IsSelectTagRequired As Boolean = False) As UnitList

		Try

			Return UnitList.GetUnitList(IsSelectTagRequired:=IsSelectTagRequired)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetUnit(ID As Guid) As Unit

		Try

			Return Unit.GetUnit(ID:=ID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetValue(id As Integer) As String

		Try

			Return "value"

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " POST Method(s) "

	<HttpPost>
	Public Function SaveUnit(<FromBody()> value As Object) As IHttpActionResult

		Try

			Dim jsonObject As JObject = JObject.Parse(value.ToString())
			Dim mIsNew As Boolean = jsonObject("mIsNew").ToObject(Of Boolean)()
			Dim returnstring As String = ""

			If mIsNew Then
				returnstring = SetNewUnit(jsonObject)
			Else
				returnstring = SetExistingUnit(jsonObject)
			End If

			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Unit Saved Successfully!"))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:=returnstring))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

	Private Function SetNewUnit(jsonObject As JObject) As String

		Try

			Dim mUnit As Unit = Unit.NewUnit(New Guid(jsonObject("mID").ToString()))
			mUnit.Name = jsonObject("mName").ToString()
			mUnit.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Unit",
																						   ex:=ex)

			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingUnit(jsonObject As JObject) As String

		Try

			Dim mUnit As Unit = Unit.GetUnit(New Guid(jsonObject("mID").ToString()))
			mUnit.Name = jsonObject("mName").ToString()
			mUnit.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Unit",
																						   ex:=ex)

			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " PUT Method(s) "

	<HttpPut>
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " DELETE Method(s) "

	<HttpDelete>
	Public Function DeleteUnit(ID As Guid) As IHttpActionResult

		Try

			Unit.DeleteUnit(ID:=ID)

			Return Ok(New ReturnMessage("Success",
											   "Unit Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Unit",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
