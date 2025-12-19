Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq

Public Class ItemTypeListController
	Inherits ApiController


#Region " Variable Declaration "

	Private _MessageBox As New MSGBox
	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetValues(Optional IsSelectTagRequired As Boolean = False) As PartTypeList

		Try

			Return PartTypeList.GetPartTypeList(IsSelectTagRequired)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetValue(Id As Integer) As PartType

		Try

			Return PartType.GetPartType(ID:=Id)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetNewPartType(Id As Integer) As PartType

		Try

			Return PartType.NewPartType(ID:=Id)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetPartStatusList(Optional IsSelectTagRequired As Boolean = False) As PartStatusList

		Try

			Return PartStatusList.GetPartStatusList(IsSelectTagRequired:=IsSelectTagRequired)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function ListOfPartType() As ItemTypeList

		Try

			Return ItemTypeList.GetItemTypeList

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function PostValue(<FromBody()> value As Object) As IHttpActionResult

		Dim jsonObject As JObject = JObject.Parse(value.ToString)
		Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
		Dim ReturnString As String

		Try

			If mIsNew Then
				ReturnString = SetNewPartTypeValues(jsonObject)
			Else
				ReturnString = SetExistingPartTypeValues(jsonObject)
			End If

			If ReturnString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Part Type Saved Successfully!"))

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

	Private Function SetNewPartTypeValues(jsonObject As JObject) As String

		Try

			Dim mPartType As PartType = PartType.NewPartType()

			With mPartType

				.Name = jsonObject(propertyName:="mName").ToString.Trim
				.Code = jsonObject(propertyName:="mCode").ToString.Trim
				.Color = jsonObject(propertyName:="mColor")
				.PartStatusID = CInt(jsonObject(propertyName:="mPartStatusID"))

			End With

			mPartType.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Part Type",
																						   ex:=ex)
			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingPartTypeValues(jsonObject As JObject) As String

		Try

			Dim mPartType As PartType = PartType.GetPartType(CInt(jsonObject("mID").ToString))

			With mPartType

				.Name = jsonObject(propertyName:="mName").ToString.Trim
				.Code = jsonObject(propertyName:="mCode").ToString.Trim
				.Color = jsonObject(propertyName:="mColor")
				.PartStatusID = CInt(jsonObject(propertyName:="mPartStatusID"))

			End With

			mPartType.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Part Type",
																						   ex:=ex)
			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(ID As Integer, <FromBody()> value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeletePartType(ID As Integer) As IHttpActionResult

		Try

			Dim mPartType As PartType = PartType.GetPartType(ID:=ID)

			PartType.DeletePartType(ID:=ID)
			Return Ok(New ReturnMessage("Success",
											   "Part Type Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Part Type",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
