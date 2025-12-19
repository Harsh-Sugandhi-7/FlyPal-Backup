'************************************
'Created by:	Saylee
'Created on:	4-Apr-2025
'Created for:	Place Master
'************************************


Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq


Public Class PlaceController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _MessageBox As New MSGBox

#End Region

#Region " Get / New Methods "


	<HttpGet>
	Public Function NewPlace() As Place
		Return Place.NewPlace(Guid.NewGuid)
	End Function

	<HttpGet>
	Public Function GetPlaceList(Optional PlaceName As String = "",
								 Optional CityName As String = "",
								 Optional AddTopItem As String = "",
								 Optional Show100Records As Boolean = False) As PlaceList

		Return PlaceList.GetPlaceList(PlaceName,
									  CityName,
									  AddTopItem,
									  Show100Records)

	End Function

	<HttpGet>
	Public Function GetPlace(ID As String) As Place
		Return Place.GetPlace(New Guid(ID))
	End Function

#End Region

#Region " Method(s) "

	Public Function SetPlace(jsonObject As JObject, IsNew As Boolean) As String

		Dim mPlace As Place

		Try

			If IsNew Then
				mPlace = Place.NewPlace(ID:=Guid.NewGuid)
			Else
				mPlace = Place.GetPlace(ID:=New Guid(jsonObject("mID").ToString))
			End If

			mPlace.Code = jsonObject(propertyName:="mCode")
			mPlace.Name = jsonObject(propertyName:="mName")

			If jsonObject(propertyName:="mCityID") = "" Then

			Else
				mPlace.CityID = New Guid(jsonObject(propertyName:="mCityID").ToString)
			End If

			mPlace.ICAO = jsonObject(propertyName:="mICAO")

			mPlace.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Part Type",
																						   ex:=ex)
			Return returnMessage

		End Try

	End Function

#End Region

#Region " Save Method "

	<HttpPost>
	Public Function PostPlace(<FromBody()> value As Object) As IHttpActionResult

		Dim jsonObject As JObject = JObject.Parse(value.ToString)
		Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
		Dim ReturnString As String

		Try

			ReturnString = SetPlace(jsonObject, mIsNew)

			If ReturnString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Place Saved Successfully!"))

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

#Region "Delete Method"

	<HttpDelete>
	Public Function DeletePlace(ID As String) As IHttpActionResult

		Try

			Place.DeletePlace(New Guid(ID))

			Return Ok(New ReturnMessage("Success", "Place Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Place",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
