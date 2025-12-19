'************************************
'Created by:	Saylee
'Created on:	4-Apr-2025
'Created for:	City Master
'************************************

Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq


Public Class CityController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Variable(s) "

	Dim mMSGBox As New MSGBox

#End Region

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetCityList(Optional Name As String = "",
								Optional AddTopItem As String = "") As CityList

		Try

			Return CityList.GetCityList(Name:=Name,
										AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetCity(ID As String) As City

		Try

			Return City.GetCity(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

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

#Region " Post Method(s) "

	<HttpPost>
	Public Function SaveCity(<FromBody()> value As Object) As IHttpActionResult

		Try

			Dim jsonObject As JObject = JObject.Parse(value.ToString)
			Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
			Dim ReturnString As String

			ReturnString = SetCity(jsonObject, mIsNew)

			If ReturnString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="City Saved Successfully!"))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:=ReturnString))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeleteCity(ID As String) As IHttpActionResult

		Try

			City.DeleteCity(New Guid(ID))

			Return Ok(New ReturnMessage("Success", "City Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="City",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

#Region " Set Method(s) "

	Public Function SetCity(jsonObject As JObject, IsNew As Boolean) As String

		Try

			Dim mCity As City

			If IsNew Then
				mCity = City.NewCity(ID:=Guid.NewGuid)
			Else
				mCity = City.GetCity(ID:=New Guid(jsonObject("mID").ToString))
			End If

			mCity.GMT = jsonObject(propertyName:="mGMT")
			mCity.Name = jsonObject(propertyName:="mName")

			mCity.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="City",
																						   ex:=ex)

			Return returnMessage

		End Try

	End Function

#End Region

End Class
