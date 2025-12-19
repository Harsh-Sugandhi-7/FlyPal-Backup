'***********************************
'Created by:  Harsh Sugandhi
'Created on:  8th April 2025
'Created for: FLYPAL-2295 API Creation for Flight Log Module.
'***********************************


Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq


Public Class ATAController
	Inherits ApiController

#Region " Variable Declaration "

	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetATAList(Optional ATANomenclature As String = "",
							   Optional AddTopItem As String = "") As ATAList

		Try

			Return ATAList.GetATAList(ATANomenclature:=ATANomenclature,
									  AddTopItem:=AddTopItem)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetATA(ID As String) As ATA

		Try

			Return ATA.GetATA(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetSubATAList(ATAID As String,
								  Optional SubATANomenclature As String = "",
								  Optional AddTopItem As String = "",
								  Optional IsForAPI As Integer = 0) As SubATAList

		Try

			Return SubATAList.GetSubATAList(ATAID:=New Guid(ATAID),
											SubATANomenclature:=SubATANomenclature,
											AddTopItem:=AddTopItem,
											IsForAPI:=IsForAPI)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewATA(ID As String) As ATA

		Try

			Return ATA.NewATA(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewSUBATA(ID As Guid,
							  ATAID As Guid,
							  ATACode As Integer,
							  ATANomenclature As String,
							  SubATACode As Integer,
							  SubATANomenclature As String,
							  Optional SubATADescription As String = "",
							  Optional SubCode As Integer = Nothing) As SubATA

		Try

			Return SubATA.NewSubATA(ID:=ID,
									ATAID:=ATAID,
									ATACode:=ATACode,
									ATANomenclature:=ATANomenclature,
									SubATACode:=SubATACode,
									SubATANomenclature:=SubATANomenclature,
									SubATADescription:=SubATADescription,
									SubCode:=SubCode)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function SaveATA(<FromBody()> value As Object) As IHttpActionResult

		Dim JObject As JObject = JObject.Parse(value.ToString)
		Dim _IsNew As Boolean = CBool(JObject("mIsNew"))
		Dim _ATA As ATA
		Dim Status As String
		Dim SuccessMessage As String

		Try

			If _IsNew Then

				_ATA = ATA.NewATA(Guid.NewGuid)

				SetATADetails(_ATA:=_ATA,
							  JObject:=JObject)

				Status = SetNewSubATADetails(_ATA:=_ATA,
											 JObject:=JObject)

				SuccessMessage = "New ATA Added Successfully!"

			Else

				_ATA = ATA.GetATA(New Guid(JObject("mID").ToString))

				SetATADetails(_ATA:=_ATA,
							  JObject:=JObject)

				Status = SetExistingSubATADetails(_ATA:=_ATA,
												  JObject:=JObject)

				SuccessMessage = "ATA Saved Successfully!"

			End If


			'If Status = "Success" Then
			'    Return New ReturnMessage(Status:="Success",
			'                             Message:=SuccessMessage)
			'Else
			'    Return New ReturnMessage(Status:="Error",
			'                             Message:=Status)
			'End If

			If Status = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="WorkShop Saved Successfully!"))

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

	Private Function SetATADetails(_ATA As ATA,
								   JObject As JObject) As String

		Try

			With _ATA

				.ATACode = JObject("mATACode").ToString
				.ATANomenclature = JObject("mATANomenclature").ToString
				.DispATACode = JObject("mDispATACode").ToString

			End With

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetNewSubATADetails(_ATA As ATA,
										 JObject As JObject) As String

		Try

			Dim SubATAs As JArray = CType(JObject("mSubATAs"), JArray)


			For j As Integer = 0 To SubATAs.Count - 1

				_ATA.SubATAs.Add(ID:=Guid.NewGuid,
								 ATAID:=_ATA.ID,
								 ATACode:=_ATA.ATACode,
								 ATANomenclature:=_ATA.ATANomenclature,
								 SubATACode:=CInt(SubATAs(j)("mSubATACode")),
								 SubATANomenclature:=SubATAs(j)("mSubATANomenclature").ToString,
								 SubATADescription:=SubATAs(j)("mSubATADescription").ToString,
								 SubCode:=CInt(SubATAs(j)("mSubCode")))

				With _ATA.SubATAs.CurrentItem

					.SubATACode = CInt(SubATAs(j)("mSubATACode"))
					.SubATANomenclature = SubATAs(j)("mSubATANomenclature").ToString
					.SubATADescription = SubATAs(j)("mSubATADescription").ToString
					.SubCode = CInt(SubATAs(j)("mSubCode"))

				End With

			Next

			_ATA.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingSubATADetails(_ATA As ATA,
											  JObject As JObject) As String

		Try

			Dim SubATAs As JArray = CType(JObject("mSubATAs"), JArray)

			For j As Integer = 0 To SubATAs.Count - 1

				Dim _SubATAID As Guid = New Guid(SubATAs(j)("mID").ToString)
				Dim _SubATAIsNew As Boolean = CBool(SubATAs(j)("mIsNew"))
				Dim _SubATAIsDeleted As Boolean = CBool(SubATAs(j)("mIsDeleted"))
				Dim _SubATAIsDirty As Boolean = CBool(SubATAs(j)("mIsDirty"))
				Dim _SubATA As SubATA

				If _SubATAIsNew Then

					_ATA.SubATAs.Add(ID:=Guid.NewGuid,
									 ATAID:=_ATA.ID,
									 ATACode:=_ATA.ATACode,
									 ATANomenclature:=_ATA.ATANomenclature,
									 SubATACode:=CInt(SubATAs(j)("mSubATACode")),
									 SubATANomenclature:=SubATAs(j)("mSubATANomenclature").ToString,
									 SubATADescription:=SubATAs(j)("mSubATADescription").ToString,
									 SubCode:=CInt(SubATAs(j)("mSubCode")))

					_SubATA = _ATA.SubATAs.CurrentItem

				Else
					_SubATA = _ATA.SubATAs(_SubATAID)
				End If

				If _SubATAIsDeleted Then
					_ATA.SubATAs.Remove(_SubATA)
				End If

				If _SubATAIsNew Or _SubATAIsDirty Then

					With _SubATA

						.SubATACode = CInt(SubATAs(j)("mSubATACode"))
						.SubATANomenclature = SubATAs(j)("mSubATANomenclature").ToString
						.SubATADescription = SubATAs(j)("mSubATADescription").ToString
						.SubCode = CInt(SubATAs(j)("mSubCode"))

					End With

				End If

			Next

			_ATA.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(ID As Integer, <FromBody()> Value As String)

		Try

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeleteATA(ID As Guid) As IHttpActionResult

		Try

			ATA.DeleteATA(ID:=ID)

			Return Ok(New ReturnMessage("Success", "ATA Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="ATA",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
