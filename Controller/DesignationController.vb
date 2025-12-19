'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq


Public Class DesignationController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _MessageBox As New MSGBox

#End Region

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetDesignationList(Optional Name As String = "",
									   Optional AddTopItem As String = "") As DesignationList

		Try

			Return DesignationList.GetDesignationList(Name:=Name,
													  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetDesignationByID(ID As String) As Designation

		Try

			Return Designation.GetDesignation(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetDesignationByName(Name As String) As Designation

		Try

			Return Designation.GetDesignation(Name:=Name)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function NewDesignation() As Designation

		Try

			Return Designation.NewDesignation()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewDesignation(ID As String,
								   Name As String) As Designation

		Try

			Return Designation.NewDesignation(ID:=New Guid(ID),
											  Name:=Name)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function SaveDesignation(<FromBody()> value As Object) As IHttpActionResult

		Dim JObject As JObject = JObject.Parse(value.ToString)
		Dim _IsNew As Boolean = CBool(JObject("mIsNew"))
		Dim _Designation As Designation
		Dim Status As String
		Dim SuccessMessage As String

		Try

			If _IsNew Then

				_Designation = Designation.NewDesignation()
				SuccessMessage = "New Designation Added Successfully!"

			Else

				_Designation = Designation.GetDesignation(New Guid(JObject("mID").ToString))
				SuccessMessage = "Designation Saved Successfully!"

			End If

			Status = SetDesignationDetails(_Designation,
										   JObject)

			If Status = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:=SuccessMessage))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:=Status))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

	Private Function SetDesignationDetails(_Designation As Designation,
										   JObject As JObject) As String

		Try

			With _Designation
				.Name = JObject("mName").ToString
			End With

			_Designation.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Designation",
																						   ex:=ex)

			Return returnMessage

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

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeleteDesignation(ID As String) As IHttpActionResult

		Try

			Designation.DeleteDesignation(ID:=New Guid(ID))

			Return Ok(New ReturnMessage("Success", "Designation Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Designation",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
