Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq

Public Class VendorController
	Inherits ApiController

#Region " Variable Declaration "

	Dim mDateFormatString As String = ""
	Private _MessageBox As New MSGBox
	Private _SQLExceptionHelper As New SQLExceptionHelper

#End Region

#Region " Get Method(s) "

	<HttpGet>
	<Route("api/Vendor")>
	<Route("api/Vendor/VendorsList")>
	<Route("api/Vendor/RFQVendorsList")>
	Public Function VendorsList(LookInType As Integer,
								Optional Name As String = "",
								Optional City As String = "",
								Optional State As String = "",
								Optional Country As String = "",
								Optional ContactPerson As String = "",
								Optional IsSelectTagRequired As Boolean = False,
								Optional IsCustomer As Boolean = False,
								Optional IsSupplier As Boolean = False,
								Optional IsServiceProvider As Boolean = False) As VendorList
		Try

			Return VendorList.GetVendortList(LookInType:=LookInType,
											 Name:=Name,
											 City:=City,
											 State:=State,
											 Country:=Country,
											 ContactPerson:=ContactPerson,
											 IsSelectTagRequired:=IsSelectTagRequired,
											 IsCustomer:=IsCustomer,
											 IsSupplier:=IsSupplier,
											 IsServiceProvider:=IsServiceProvider)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetVendor(ID As String) As Vendor

		Try

			Return Vendor.GetVendor(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetVendorApprovals(VendorID As String,
									   Optional HasHistory As Boolean = False,
									   Optional VendorApprovalID As String = "{00000000-0000-0000-0000-000000000000}",
									   Optional IsFromOtherLink As Integer = 0,
									   Optional VendorName As String = "") As VendorApprovals
		Try

			Return VendorApprovals.GetVendorApprovalList(VendorID:=New Guid(VendorID),
														 HasHistory:=HasHistory,
														 VendorApprovalID:=VendorApprovalID,
														 IsFromOtherLink:=IsFromOtherLink,
														 VendorName:=VendorName)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetVendorApprovalListForDue(Optional AsOnDate As String = "1/1/3300",
												Optional VendorID As String = "{00000000-0000-0000-0000-000000000000}",
												Optional VendorTypeID As Integer = 0) As VendorApprovalListForDue

		Try

			Return VendorApprovalListForDue.GetVendorApprovalListForDue(AsOnDate:=AsOnDate,
																		VendorID:=VendorID,
																		VendorTypeID:=VendorTypeID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetVendorApproval(ID As String) As VendorApproval

		Try

			Return VendorApproval.GetVendorApproval(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function CustomerList(LookInType As Integer,
								 Optional Name As String = "",
								 Optional City As String = "",
								 Optional State As String = "",
								 Optional Country As String = "",
								 Optional ContactPerson As String = "",
								 Optional IsSelectTagRequired As Boolean = False,
								 Optional IsCustomer As Boolean = False,
								 Optional IsSupplier As Boolean = False,
								 Optional IsServiceProvider As Boolean = False) As VendorList

		Try

			Return VendorList.GetVendortList(LookInType:=LookInType,
											 Name:=Name,
											 City:=City,
											 State:=State,
											 Country:=Country,
											 ContactPerson:=ContactPerson,
											 IsSelectTagRequired:=IsSelectTagRequired,
											 IsCustomer:=IsCustomer,
											 IsSupplier:=IsSupplier,
											 IsServiceProvider:=IsServiceProvider)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function SupplierList(LookInType As Integer,
								 Optional Name As String = "",
								 Optional City As String = "",
								 Optional State As String = "",
								 Optional Country As String = "",
								 Optional ContactPerson As String = "",
								 Optional IsSelectTagRequired As Boolean = False,
								 Optional IsCustomer As Boolean = False,
								 Optional IsSupplier As Boolean = False,
								 Optional IsServiceProvider As Boolean = False) As Vendors
		Try

			Return Vendors.GetVendortList(LookInType:=LookInType,
										  Name:=Name,
										  City:=City,
										  State:=State,
										  Country:=Country,
										  ContactPerson:=ContactPerson,
										  IsSelectTagRequired:=IsSelectTagRequired,
										  IsCustomer:=IsCustomer,
										  IsSupplier:=IsSupplier,
										  IsServiceProvider:=IsServiceProvider)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function SaveVendor(<FromBody()> values As Object) As IHttpActionResult

		Dim jsonObject As JObject = JObject.Parse(values.ToString)
		Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
		Dim ReturnString As String

		Try

			If mIsNew Then
				ReturnString = SetNewVendorValues(jsonObject)
			Else
				ReturnString = SetExistingVendorValues(jsonObject)
			End If

			If ReturnString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Vendor Saved Successfully!"))

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

	' Logic to set new vendor values
	Private Function SetNewVendorValues(jsonObject As JObject) As String

		Try

			Dim mVendor As Vendor = Vendor.NewVendor(ID:=New Guid(jsonObject(propertyName:="mID").ToString))

			SetVendor(jsonObject, mVendor)

			mVendor.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Order",
																						   ex:=ex)

			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	' Logic to set existing vendor values
	Private Function SetExistingVendorValues(jsonObject As JObject) As String

		Try

			Dim mVendor As Vendor = Vendor.GetVendor(New Guid(jsonObject(propertyName:="mID").ToString))

			SetVendor(jsonObject, mVendor)

			mVendor.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Vendor",
																						   ex:=ex)

			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Public Sub SetVendor(jsonObject As JObject, Optional mVendor As Vendor = Nothing)

		With mVendor

			mDateFormatString = jsonObject(propertyName:="mNotInUseDate")("mFormat")

			.Name = jsonObject(propertyName:="mName").ToString()
			.IsSupplier = CBool(jsonObject(propertyName:="mIsSupplier"))
			.IsCustomer = CBool(jsonObject(propertyName:="mIsCustomer"))
			.IsServiceProvider = CBool(jsonObject(propertyName:="mIsServiceProvider"))
			.Address = jsonObject(propertyName:="mAddress").ToString()
			.CityID = New Guid(jsonObject(propertyName:="mCityID").ToString())
			.Zip = jsonObject(propertyName:="mZip").ToString()
			.Phone1 = jsonObject(propertyName:="mPhone1")
			.Phone2 = jsonObject(propertyName:="mPhone2")
			.Phone3 = jsonObject(propertyName:="mPhone3")
			.Fax = jsonObject(propertyName:="mFax")
			.Email = jsonObject(propertyName:="mEmail")
			.ContactPerson = jsonObject(propertyName:="mContactPerson")
			.NotInUseDate = CDate(jsonObject(propertyName:="mNotInUseDate").First.First).ToString(format:=mDateFormatString)
			.NotInUse = CBool(jsonObject(propertyName:="mNotInUse"))
			.VendorTypeID = CInt(jsonObject(propertyName:="mVendorTypeID"))
			.IsApprovalRequired = CBool(jsonObject(propertyName:="mIsApprovalRequired"))
			.Code = jsonObject(propertyName:="mCode").ToString()
			.NatureOfVendor = jsonObject(propertyName:="mNatureOfVendor").ToString()
			.RepairStationCertificate = jsonObject(propertyName:="mRepairStationCertificate").ToString()
			.VendorID = jsonObject(propertyName:="mVendorID").ToString()
			.GSTIN = jsonObject(propertyName:="mGSTIN").ToString()

		End With

	End Sub

#End Region

#Region " Approval Post Method(s) "

	Public Function SaveVendorApproval(<FromBody()> values As Object) As IHttpActionResult

		Dim jsonObject As JObject = JObject.Parse(values.ToString)
		Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))
		Dim ReturnString As String

		Try

			If mIsNew Then
				ReturnString = SetNewVendorApprovalValues(jsonObject)
			Else
				ReturnString = SetExistingVendorApprovalValues(jsonObject)
			End If

			'If ReturnString = "Success" Then
			'    Return New ReturnMessage("Success", "Vendor Approval saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", ReturnString)
			'End If

			If ReturnString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Vendor Approval Saved Successfully!"))

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

	' Logic to set new vendor approval values
	Private Function SetNewVendorApprovalValues(jsonObject As JObject) As String

		Try
			Dim mVendorApproval As VendorApproval = VendorApproval.NewVendorApproval(ID:=New Guid(jsonObject(propertyName:="mID").ToString),
																					 VendorID:=New Guid(jsonObject(propertyName:="mVendorID").ToString))
			SetVendorApproval(jsonObject, mVendorApproval)
			mVendorApproval.Save()
			Return "Success"
		Catch ex As Exception
			Return ex.Message
		End Try
	End Function

	' Logic to set existing vendor approval values
	Private Function SetExistingVendorApprovalValues(jsonObject As JObject) As String

		Try

			Dim mVendorApproval As VendorApproval = VendorApproval.GetVendorApproval(New Guid(jsonObject(propertyName:="mID").ToString))

			SetVendorApproval(jsonObject, mVendorApproval)

			mVendorApproval.Save()
			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Public Sub SetVendorApproval(jsonObject As JObject, Optional mVendorApproval As VendorApproval = Nothing)

		With mVendorApproval

			mDateFormatString = jsonObject(propertyName:="mFromDate")("mFormat")

			.Name = jsonObject(propertyName:="mName").ToString()
			.ApprovalNo = jsonObject(propertyName:="mApprovalNo").ToString()
			.IsOneTime = CBool(jsonObject(propertyName:="mIsOneTime"))
			.IsApplicable = CBool(jsonObject(propertyName:="mIsApplicable"))
			.FromDate = CDate(jsonObject(propertyName:="mFromDate").First.First).ToString(format:=mDateFormatString)
			.ToDate = CDate(jsonObject(propertyName:="mToDate").First.First).ToString(format:=mDateFormatString)
			.IsAttachmentAdded = CBool(jsonObject("mIsAttachmentAdded"))
			.Remark = jsonObject(propertyName:="mRemark").ToString()

		End With

	End Sub

#End Region

#Region " Put Method(s) "

	<HttpPut>
	Public Sub PutValue(id As Integer, <FromBody()> value As String)

	End Sub

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeleteVendor(ID As Guid) As IHttpActionResult

		Try

			Vendor.DeleteVendor(ID:=ID)

			Return Ok(New ReturnMessage("Success", "Vendor Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Vendor",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

	<HttpDelete>
	Public Function DeleteVendorApproval(ID As Guid) As IHttpActionResult

		Try

			VendorApproval.DeleteVendorApproval(ID:=ID)
			Return Ok(New ReturnMessage("Success", "Vendor Approval Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Vendor",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

End Class
