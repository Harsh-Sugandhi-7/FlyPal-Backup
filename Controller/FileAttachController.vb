Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq


Public Class FileAttachController
	Inherits ApiController


#Region " Variable(s) "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Get Method(s) "

	<HttpGet>
	<Route("api/FileAttach/GetAttachment")>
	Public Function GetAttachment(ReferenceID As Guid,
								  Optional Sort As Integer = 0,
								  Optional FileName As String = "",
								  Optional FilePath As String = "",
								  Optional DataTable As String = "") As FileAttach

		Try

			Return FileAttach.GetAttachment(ReferenceID:=ReferenceID,
											Sort:=Sort,
											FileName:=FileName,
											DataSet:=Nothing,
											FilePath:=FilePath,
											DataTable:=DataTable)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/FileAttach/NewAttachment")>
	Public Function NewAttachment(ID As Guid,
								  ReferenceID As Guid,
								  Optional ImageFile As Byte() = Nothing,
								  Optional Size As Integer = 0,
								  Optional Extension As String = "",
								  Optional Sort As Integer = 0) As FileAttach

		Try

			Return FileAttach.NewAttachment(ID:=ID,
											ReferenceID:=ReferenceID,
											ImageFile:=ImageFile,
											Size:=Size,
											Extension:=Extension,
											Sort:=Sort)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/FileAttach/GetChildFileAttachments")>
	Public Function GetChildFileAttachments(ReferenceID As Guid,
											Optional Sort As Integer = 0) As FileAttachments

		Try

			Return FileAttachments.GetChildFileAttachments(ReferenceID:=ReferenceID,
														   Sort:=Sort)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/FileAttach/NewFileAttachments")>
	Public Function NewFileAttachments() As FileAttachments

		Try

			Return FileAttachments.NewFileAttachments()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	<Route("api/FileAttach/SaveAttachment")>
	Public Function SaveAttachment(<FromBody()> requestBody As JObject) As IHttpActionResult

		Try

			Dim FileAttachment As FileAttach = FileAttach.NewAttachment(ReferenceID:=New Guid(),
																		FileName:="")

			FileAttachment.FileName = requestBody("mFileName")
			FileAttachment.ReferenceID = requestBody("mReferenceID")
			FileAttachment.ImageFile = requestBody("mImageFile")
			FileAttachment.Size = requestBody("mSize")
			FileAttachment.Extension = requestBody("mExtension")
			FileAttachment.Sort = requestBody("mSort")
			FileAttachment.SrNo = requestBody("mSrNo")

			FileAttachment.Save()

			Return Ok(New ReturnMessage(Status:="Success",
											   Message:="Attachment Saved Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="File Attachment",
																						   ex:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage(Status:="Error",
												 Message:=returnMessage))

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

	<HttpPost>
	<Route("api/FileAttach/SaveMultipleAttachments")>
	Public Function SaveMultipleAttachments(<FromBody()> requestBody As JArray) As IHttpActionResult

		Try

			For Each JObject As JObject In requestBody

				Dim FileAttachment As FileAttach = FileAttach.NewAttachment(ReferenceID:=Guid.NewGuid(),
																			FileName:="")

				FileAttachment.FileName = JObject("mFileName").ToString()
				FileAttachment.ReferenceID = Guid.Parse(JObject("mReferenceID").ToString())
				FileAttachment.ImageFile = JObject("mImageFile")
				FileAttachment.Size = CInt(JObject("mSize"))
				FileAttachment.Extension = JObject("mExtension").ToString()
				FileAttachment.Sort = CInt(JObject("mSort"))
				FileAttachment.SrNo = CInt(JObject("mSrNo"))

				FileAttachment.Save()

			Next

			Return Ok(New ReturnMessage(Status:="Success",
											   Message:="All Attachments are saved Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="File Attachment",
																						   ex:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage(Status:="Error",
												 Message:=returnMessage))

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString()))

		End Try

	End Function

#End Region

#Region " Put Method(s) "


#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	<Route("api/FileAttach/RemoveAttachment")>
	Public Function RemoveAttachment(ReferenceID As String,
									 Optional ID As String = "{00000000-0000-0000-0000-000000000000}",
									 Optional Sort As Integer = 0) As IHttpActionResult

		Try

			FileAttach.DeleteAttachment(ID:=New Guid(ID),
										ReferenceID:=New Guid(ReferenceID),
										Sort:=Sort)

			Return Ok(New ReturnMessage("Success",
											   "Attachment Removed Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Item",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))


		End Try

	End Function

#End Region

End Class