Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq

Public Class StoreController
	Inherits ApiController

#Region " Variable Declaration "

	Dim mDateFormatString As String = ""
	Private _MessageBox As New MSGBox
	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " GET Method(s) "

	Public Function GetStoreList(LookInType As Integer,
								 Optional StoreName As String = "",
								 Optional IsSelectTagRequired As Boolean = False,
								 Optional IsForUserStoreRights As Boolean = False) As StoreList
		Try

			Return StoreList.GetStoreList(LookInType:=0,
										  StoreName:="",
										  IsSelectTagRequired:=True,
										  IsForUserStoreRights:=IsForUserStoreRights)


		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Public Function GetValue(ID As Guid) As Store
		Return Store.GetStore(ID:=ID)
	End Function

	Public Function GetNewStore(ID As Guid) As Store

		Return Store.NewStore(ID:=ID)

	End Function

	Public Function GetNewStoreTag(StoreID As Guid) As StoreTag

		Dim mStore As Store = Store.NewStore()

		mStore.StoreTags.Add(ID:=Guid.NewGuid, StoreID:=StoreID, ItemTagID:=0)


		Return mStore.StoreTags.CurrentItem

	End Function

#End Region

#Region " Store Methods "

	Public Sub SetStore(jsonObject As JObject, Optional mStore As Store = Nothing)

		With mStore

			mDateFormatString = jsonObject(propertyName:="mNotInUseDate")("mFormat")
			.Name = jsonObject(propertyName:="mName").ToString()
			.LocationID = New Guid(jsonObject(propertyName:="mLocationID").ToString())
			.LocationName = jsonObject(propertyName:="mLocationName").ToString()
			.IsValued = CBool(jsonObject(propertyName:="mIsValued"))
			.IsOwnedByCustomer = CBool(jsonObject(propertyName:="mIsOwnedByCustomer"))
			.VendorID = New Guid(jsonObject(propertyName:="mVendorID").ToString())
			.VendorName = jsonObject(propertyName:="mVendorName").ToString()
			.NotInUseDate = CDate(jsonObject(propertyName:="mNotInUseDate").First.First).ToString(format:=mDateFormatString)
			.NotInUse = CBool(jsonObject(propertyName:="mNotInUse"))

		End With

	End Sub

#End Region

#Region " POST Method(s) "

	Public Function SaveStore(<FromBody()> values As Object) As IHttpActionResult

		Try

			Dim jsonObject As JObject = JObject.Parse(values.ToString())
			Dim mIsNew As Boolean = jsonObject("mIsNew").ToObject(Of Boolean)()
			Dim ReturnString As String = ""

			If mIsNew Then
				returnstring = SetNewStoreValues(jsonObject:=jsonObject)
			Else
				returnstring = SetexistingStoreValues(jsonObject:=jsonObject)
			End If

			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Store Saved Successfully!"))

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

	Private Function SetNewStoreValues(jsonObject As JObject) As String

		Try

			Dim mStore As Store = Store.NewStore(ID:=New Guid(jsonObject(propertyName:="mID").ToString))
			Dim StoreTagArray As JArray = CType(jsonObject("mStoreTags"), JArray)

			SetStore(jsonObject:=jsonObject, mStore:=mStore)

			For i As Integer = 0 To StoreTagArray.Count - 1
				mStore.StoreTags.Add(ID:=Guid.NewGuid,
									 StoreID:=New Guid(StoreTagArray(i)("mStoreID").ToString),
									 ItemTagID:=CInt(StoreTagArray(i)("mItemTagID"))
									 )
				With mStore.StoreTags.CurrentItem
					.StoreName = StoreTagArray(i)("mStoreName")
					.ItemTagName = StoreTagArray(i)("mImageTagName")
				End With
			Next

			mStore.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Store",
																						   ex:=ex)
			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingStoreValues(jsonObject As JObject) As String

		Try

			Dim mStore As Store = Store.GetStore(ID:=New Guid(jsonObject(propertyName:="mID").ToString))
			Dim StoreTagArray As JArray = CType(jsonObject("mStoreTags"), JArray)

			SetStore(jsonObject:=jsonObject, mStore:=mStore)

			For i As Integer = 0 To StoreTagArray.Count - 1

				Dim mID As Guid = New Guid(StoreTagArray(i)("mID").ToString)
				Dim mIsNew As Boolean = CBool(StoreTagArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(StoreTagArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(StoreTagArray(i)("mIsDirty"))
				Dim mStoreTag As StoreTag

				If mIsNew Then
					mStore.StoreTags.Add(ID:=New Guid,
										 StoreID:=mStore.ID,
										 ItemTagID:=CInt(StoreTagArray(i)("mItemTagID"))
										 )
					mStoreTag = mStore.StoreTags.CurrentItem
				Else
					mStoreTag = mStore.StoreTags(mID)
				End If

				If mIsDeleted Then
					mStore.StoreTags.Remove(mStoreTag)
				End If

				If mIsNew Or mIsDirty Then

					With mStoreTag
						.StoreName = StoreTagArray(i)("mStoreName")
						.ItemTagID = CInt(StoreTagArray(i)("mItemTagID"))
						.ItemTagName = StoreTagArray(i)("mImageTagName")
					End With

				End If

			Next

			mStore.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Store",
																						   ex:=ex)
			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " PUT Method(s) "

	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub

#End Region

#Region " DELETE Method(s) "

	Public Function DeleteStore(ID As Guid) As IHttpActionResult

		Try

			Store.DeleteStore(ID:=ID)

			Return Ok(New ReturnMessage("Success", "Store Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Store",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
