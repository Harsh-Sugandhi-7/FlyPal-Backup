Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq

Public Class ManufacturerController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " GET Method(s) "
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValues(Optional ByVal Name As String = "", Optional ByVal AddTopItem As String = "") As ManufacturerList
		Return ManufacturerList.GetManufacturerList(Name, AddTopItem)
	End Function

	' GET api/<controller>/5
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetManufacturer(ByVal ID As Guid) As Manufacturer
		Return Manufacturer.GetManufacturer(ID)
	End Function
#End Region

#Region " Methods "

#End Region

#Region " POST Method(s) "
	' POST api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function SaveManufacturer(<FromBody()> value As Object) As IHttpActionResult
		Try
			Dim jsonObject As JObject = JObject.Parse(value.ToString())
			Dim mIsNew As Boolean = jsonObject("mIsNew").ToObject(Of Boolean)()
			Dim returnstring As String = ""
			If mIsNew Then
				returnstring = SetNewManufacturer(jsonObject)
			Else
				returnstring = SetExistingManufacturer(jsonObject)
			End If
			'If returnstring = "Success" Then
			'    Return New ReturnMessage("Success", "Manufacturer saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", returnstring)
			'End If
			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Manufacturer Saved Successfully!"))

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
	Private Function SetNewManufacturer(jsonObject As JObject) As String
		Try
			Dim mManufacturer As Manufacturer = Manufacturer.NewManufacturer(New Guid(jsonObject("mID").ToString()))
			mManufacturer.Name = jsonObject("mName").ToString()
			mManufacturer.Save()
			Return "Success"
		Catch ex As Exception
			Return ex.Message
		End Try
	End Function
	Private Function SetExistingManufacturer(jsonObject As JObject) As String
		Try
			Dim mManufacturer As Manufacturer = Manufacturer.GetManufacturer(New Guid(jsonObject("mID").ToString()))
			mManufacturer.Name = jsonObject("mName").ToString()
			mManufacturer.Save()
			Return "Success"
		Catch ex As Exception
			Return ex.Message
		End Try
	End Function
#End Region

#Region " PUT Method(s) "
	' PUT api/<controller>/5
	Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

	End Sub
#End Region

#Region " DELETE Method(s) "

	Public Function DeleteManufacturer(ByVal ID As Guid) As IHttpActionResult

		Try

			Manufacturer.DeleteManufacturer(ID:=ID)
			Return Ok(New ReturnMessage("Success", "WorkShop Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Manufacturer",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
