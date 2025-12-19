Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq

Public Class CategoryController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " GET Method(s) "
	' GET api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValues(ByVal AddTopItem As String, Optional ByVal IsForTool As Boolean = False) As CategoryList
		Return CategoryList.GetCategoryList(AddTopItem, IsForTool)
	End Function

	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetValuesByID(Optional ByVal IsSelectTagRequired As Boolean = False) As CategoryList
		Return CategoryList.GetCategoryList(IsSelectTagRequired:=IsSelectTagRequired)
	End Function
	' GET api/<controller>/5
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetCategory(ByVal ID As Guid) As Category
		Return Category.GetCategory(ID)
	End Function
	Public Function GetValue(ByVal id As Integer) As String
		Return "value"
	End Function
#End Region

#Region " Methods "

#End Region

#Region " POST Method(s) "
	' POST api/<controller>
	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function Savecategory(<FromBody()> value As Object) As IHttpActionResult
		Try
			Dim jsonObject As JObject = JObject.Parse(value.ToString())
			Dim mIsNew As Boolean = jsonObject("mIsNew").ToObject(Of Boolean)()
			Dim returnstring As String = ""
			If mIsNew Then
				returnstring = SetNewCategory(jsonObject)
			Else
				returnstring = SetExistingCategory(jsonObject)
			End If
			'If returnstring = "Success" Then
			'    Return New ReturnMessage("Success", "Category saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", returnstring)
			'End If
			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Category Saved Successfully!"))

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
	Public Function SetNewCategory(jsonObject As JObject) As String
		Try
			Dim mCategory As Category = Category.NewCategory(New Guid(jsonObject("mID").ToString()))
			mCategory.Name = jsonObject("mName").ToString()
			mCategory.GLCode = jsonObject("mGLCode").ToString()
			mCategory.PrimaryCategoryID = CInt(jsonObject("mPrimaryCategoryID").ToString())
			mCategory.Save()
			Return "Success"
		Catch ex As Exception
			Return ex.Message
		End Try
	End Function
	Private Function SetExistingCategory(jsonObject As JObject) As String
		Try
			Dim mCategory As Category = Category.GetCategory(New Guid(jsonObject("mID").ToString()))
			mCategory.Name = jsonObject("mName").ToString()
			mCategory.GLCode = jsonObject("mGLCode").ToString()
			mCategory.PrimaryCategoryID = CInt(jsonObject("mPrimaryCategoryID").ToString())
			mCategory.Save()
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

	<HttpDelete>
	Public Function DeleteCategory(ByVal ID As Guid) As IHttpActionResult

		Try

			Category.DeleteCategory(ID:=ID)
			Return Ok(New ReturnMessage("Success", "Category Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Category",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
