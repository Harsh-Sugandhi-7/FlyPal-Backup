'***********************************
'Created by:  Harsh Sugandhi
'Created on:  4th June 2025
'Created for: FLYPAL-2463 API Creation for Purchase Enquiry (RFQ) module.
'***********************************


Imports System.Net
Imports System.Web.Http

Imports CrystalDecisions.[Shared].Json

Imports Newtonsoft.Json.Linq


Public Class EnquiryController
	Inherits ApiController


#Region " Enumaration "

	Private Enum RequestFor

		Supplier = 0
		Customer = 1

	End Enum

#End Region

#Region " Varriable(s) "

	Private _MessageBox As New MSGBox
	Private _EmailHelper As New EmailHelper
	Private _ReportHelper As New ReportHelper
	Private _ModuleHelper As New ModuleHelper
	Private _BrokenRulesHelper As New BrokenRulesHelper
	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _CheckForSubscriptionExpired As New CheckForSubscriptionExpired


#End Region

#Region " Get Method(s) "

	<HttpGet>
	<Route("api/Enquiry/GetListOfOverHaulOrRepairEnquiry")>
	<Route("api/Enquiry/GetListOfRentalOrLeaseEnquiry")>
	<Route("api/Enquiry/GetListOfRequestForQuotation")>
	Public Function GetListOfRequestForQuotation(Optional ItemName As String = "",
												 Optional Text As String = "",
												 Optional No As Integer = 0,
												 Optional FromDate As String = "",
												 Optional ToDate As String = "",
												 Optional StatusID As Integer = 0,
												 Optional VendorName As String = "",
												 Optional TransTypeID As Trans = Trans.Enquiry,
												 Optional VendorNo As String = "",
												 Optional IsFromQuotationComparison As Integer = 0,
												 Optional DoneOrder As Boolean = False) As EnquiryList

		Dim _EnquiryList As EnquiryList

		Try

			_EnquiryList = EnquiryList.GetEnquiryList(ItemName:=ItemName,
													  Text:=Text,
													  No:=No,
													  FromDate:=FromDate,
													  ToDate:=ToDate,
													  StatusID:=StatusID,
													  VendorName:=VendorName,
													  TransTypeID:=TransTypeID,
													  VendorNo:=VendorNo,
													  IsFromQuotationComparison:=IsFromQuotationComparison,
													  DoneOrder:=DoneOrder)

			Return _EnquiryList

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/Enquiry/GetOverHaulOrRepairEnquiry")>
	<Route("api/Enquiry/GetRentalOrLeaseEnquiry")>
	<Route("api/Enquiry/GetQuotation")>
	<Route("api/Enquiry/GetEnquiry")>
	Public Function GetEnquiry(ID As String) As Enquiry

		Try

			Dim _Enquiry = Enquiry.GetEnquiry(ID:=New Guid(ID))

			Return _Enquiry

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/Enquiry/NewOverHaulOrRepairEnquiry")>
	<Route("api/Enquiry/NewRentalOrLeaseEnquiry")>
	<Route("api/Enquiry/NewQuotation")>
	<Route("api/Enquiry/NewEnquiry")>
	Public Function NewEnquiry(TransTypeID As Trans) As Enquiry

		Try

			Return Enquiry.NewEnquiry(TransTypeID:=TransTypeID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewEnquiry(Optional ID As String = "{00000000-0000-0000-0000-000000000000}",
							   Optional TransTypeID As Trans = Trans.Enquiry) As Enquiry

		Dim Enquiry As Enquiry
		Try

			Enquiry = Enquiry.NewEnquiry(ID:=New Guid(ID),
									  TransTypeID:=TransTypeID)


			Return Enquiry

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewEnquiryItem(EnquiryID As String,
								   TransTypeID As Trans) As EnquiryItem

		Dim Enquiry As Enquiry
		Try

			Enquiry = Enquiry.NewEnquiry(TransTypeID:=TransTypeID)

			Enquiry.EnquiryItems.Add(EnquiryID:=New Guid(EnquiryID))

			Enquiry.EnquiryItems.CurrentItem.RequisitionItemEnquiryItems.Add(EnquiryItemID:=Enquiry.EnquiryItems.CurrentItem.ID,
																			 RequisitionItemID:=Guid.Empty,
																			 Qty:=0.0,
																			 RequisitionNo:="")

			Return Enquiry.EnquiryItems.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewEnquiryTerm(EnquiryID As String,
								   TransTypeID As Trans) As EnquiryTerm

		Dim Enquiry As Enquiry
		Try

			Enquiry = Enquiry.NewEnquiry(TransTypeID:=TransTypeID)

			Enquiry.EnquiryTerms.Add(EnquiryID:=New Guid(EnquiryID))

			Return Enquiry.EnquiryTerms.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function NewEnquirySupplier(EnquiryID As String,
									   TransTypeID As Trans) As EnquirySupplier

		Dim Enquiry As Enquiry
		Try

			Enquiry = Enquiry.NewEnquiry(TransTypeID:=TransTypeID)

			Enquiry.EnquirySuppliers.Add(EnquiryID:=New Guid(EnquiryID))

			Return Enquiry.EnquirySuppliers.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/Enquiry/RequisitionItemsList")>
	Public Function RequisitionItemsList(EnquiryDate As String,
										 PartName As String,
										 Optional No As Integer = 0,
										 Optional Text As String = "",
										 Optional ReqTypeID As Integer = 0,
										 Optional ClientCode As String = "",
										 Optional TransTypeID As Integer = 0,
										 Optional ToDate As String = "1/1/4400",
										 Optional FromDate As String = "1/1/1900",
										 Optional RequisitionTypeID As Integer = 0,
										 Optional WOID As String = "{00000000-0000-0000-0000-000000000000}",
										 Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
										 Optional CustomerID As String = "{00000000-0000-0000-0000-000000000000}",
										 Optional RequisitionID As String = "{00000000-0000-0000-0000-000000000000}") As RequisitionItemsNew

		Try

			Return RequisitionItemsNew.GetRequisitionItemsForList(No:=No,
																  Text:=Text,
																  WOID:=WOID,
																  ListFor:=0,
																  ToDate:=ToDate,
																  FromDate:=FromDate,
																  PartName:=PartName,
																  ItemID:=Guid.Empty,
																  ReqTypeID:=ReqTypeID,
																  MachineID:=MachineID,
																  TransDate:=EnquiryDate,
																  ClientCode:=ClientCode,
																  CustomerID:=CustomerID,
																  RequisitionID:=RequisitionID,
																  TransTypeID:=RequisitionTypeID,
																  ExchangeAsRequisitionItems:=(TransTypeID = 34))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function SaveEnquiry(<FromBody()> requestBody As JObject) As IHttpActionResult

		Dim ReturnMessage As ReturnMessage

		Try

			ReturnMessage = SetEnquiryDetails(requestBody:=requestBody)

			If ReturnMessage.Status = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Enquiry Saved Successfully!",
												   TransactionID:=ReturnMessage.TransactionID.ToString))

			ElseIf returnMessage.Status = "Validations" Then

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Validations",
													  Message:=ReturnMessage.Message))

			ElseIf returnMessage.Status = "Exception" Then

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Exception",
													  Message:=ReturnMessage.Message))
			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Exception",
												   Message:=ex.GetBaseException.ToString))

		End Try

	End Function

	Private Function SetEnquiryDetails(requestBody As JObject) As ReturnMessage

		Dim _Enquiry As Enquiry
		Dim SubscriptionMessage As String

		Dim EnquiryIsNew As Boolean = CBool(requestBody("mIsNew"))
		Dim DateFormat As String = requestBody(propertyName:="mDate")("mFormat")

		Dim EnquiryItems As JArray = CType(requestBody("mEnquiryItems"), JArray)
		Dim EnquiryTerms As JArray = CType(requestBody("mEnquiryTerms"), JArray)
		Dim EnquirySuppliers As JArray = CType(requestBody("mEnquirySuppliers"), JArray)

		Try

			If EnquiryIsNew Then

				SubscriptionMessage = _CheckForSubscriptionExpired.
										CheckForSubscriptionExpired(TransactionDate:=CDate(requestBody(propertyName:="mDate").First.First),
																	ModuleName:="Enquiry")

				If SubscriptionMessage <> "Success" Then

					Return New ReturnMessage(Status:="Error",
											 Message:=SubscriptionMessage)

				End If

				_Enquiry = Enquiry.NewEnquiry(TransTypeID:=CInt(requestBody("mTransTypeID")))

			Else
				_Enquiry = Enquiry.GetEnquiry(ID:=New Guid(requestBody("mID").ToString))
			End If

			With _Enquiry

				.Date = CDate(requestBody(propertyName:="mDate").First.First).ToString(DateFormat)
				.Text = requestBody(propertyName:="mText")
				.No = requestBody(propertyName:="mNo")
				.UserName = User.Identity.Name
				.VendorEnqNo = requestBody(propertyName:="mVendorEnqNo")
				.CustomerID = New Guid(requestBody(propertyName:="mCustomerID").ToString)
				.VendorEnqDate = CDate(requestBody(propertyName:="mVendorEnqDate").First.First).ToString(DateFormat)
				.OpeningLine = requestBody(propertyName:="mOpeningLine")
				.IsCustomer = CBool(requestBody(propertyName:="mIsCustomer"))
				.StatusID = CInt(requestBody("mStatusID"))
				.StatusName = CStr(requestBody("mStatusName"))

			End With

			'************************* Enquiry Items *************************
			If EnquiryItems.Count > 0 Then

				Dim result = SetEnquiryItems(Enquiry:=_Enquiry,
											 EnquiryItemsArray:=EnquiryItems,
											 EnquiryIsNew:=EnquiryIsNew,
											 DateFormat:=DateFormat)

				If result.Item1 IsNot Nothing AndAlso result.Item2.ToString = "Success" Then
					_Enquiry = result.Item1
				Else

					Return New ReturnMessage(Status:="Exception",
											 Message:=result.Item2.ToString)

				End If

			End If

			'************************* Enquiry Terms *************************
			If EnquiryTerms.Count > 0 Then

				_Enquiry = SetEnquiryTerms(_Enquiry:=_Enquiry,
										   EnquiryTermsArray:=EnquiryTerms,
										   EnquiryIsNew:=EnquiryIsNew)
			End If

			'************************* Enquiry Suppliers *************************
			If EnquirySuppliers.Count > 0 Then

				_Enquiry = SetEnquirySuppliers(_Enquiry:=_Enquiry,
											  EnquirySuppliersArray:=EnquirySuppliers,
											  EnquiryIsNew:=EnquiryIsNew)
			End If

			Dim _VendorList = VendorList.GetVendortList(LookInType:=0, , , , , ,
														IsSelectTagRequired:=True,
														IsCustomer:=VendorStatus(_Enquiry.TransTypeID, RequestFor.Customer),
														IsSupplier:=VendorStatus(_Enquiry.TransTypeID, RequestFor.Supplier))

			If _Enquiry.IsValid Then

				If _Enquiry.EnquiryItems.Count > 0 Then

					Dim ReturnMessage As String

					ReturnMessage = CheckIfCustomerAndSupplierAreSame(Enquiry:=_Enquiry)

					ReturnMessage = CheckIfVendorIsApplicableWhileSave(Enquiry:=_Enquiry, VendorList:=_VendorList)

					ReturnMessage = CheckIfEnquiryIsForCustomer(Enquiry:=_Enquiry, VendorList:=_VendorList)

					If ReturnMessage IsNot Nothing Then

						Return New ReturnMessage(Status:="Validations",
												 Message:=ReturnMessage.Replace("<br />", ""))

					Else

						_Enquiry.Save()

						_ModuleHelper.SendEmailToBytzSoft(TransTypeID:=_Enquiry.TransTypeID,
														  Username:=User.Identity.Name,
														  ModuleFrom:="Enquiry",
														  Action:=IIf(_Enquiry.StatusID = 2, "Authorized", "Saved"),
														  ClientCode:=AppSettings("ClientCode"),
														  TransactionNo:=_Enquiry.EnquiryNo,
														  TransactionDate:=_Enquiry.DateFormatted)

						Return New ReturnMessage(Status:="Success",
												 Message:="",
												 TransactionID:=_Enquiry.ID.ToString)

					End If

				Else

					Return New ReturnMessage(Status:="Validations",
											 Message:="Record cannot be saved. At least One Enquiry Item is required to save the record.")

				End If

			Else

				Return New ReturnMessage(Status:="Validations",
										 Message:=$"{_BrokenRulesHelper.FetchBrokenRules(CommonObject:=_Enquiry, ModuleName:="Enquiry")}")
			End If

		Catch ex As SqlException

			Return New ReturnMessage(Status:="Exception",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Enquiry", ex:=ex)}")

		Catch ex As Exception

			Return New ReturnMessage(Status:="Exception",
									 Message:=ex.GetBaseException.ToString)

		End Try

	End Function

	Private Function SetEnquiryItems(Enquiry As Enquiry,
									 EnquiryItemsArray As JArray,
									 EnquiryIsNew As Boolean,
									 DateFormat As String) As (Enquiry, String)

		Dim returnMessage As String

		Try

			For i As Integer = 0 To EnquiryItemsArray.Count - 1

				Dim EnquiryItemsID As Guid = IIf(EnquiryIsNew,
												 Guid.Empty,
												 New Guid(EnquiryItemsArray(i)("mID").ToString))

				Dim RequisitionItemAsEnquiryItem As JArray = CType(EnquiryItemsArray(i)("mRequisitionItemEnquiryItems"), JArray)

				Dim EnquiryItemsIsNew As Boolean = CBool(EnquiryItemsArray(i)("mIsNew"))
				Dim EnquiryItemsIsDeleted As Boolean = CBool(EnquiryItemsArray(i)("mIsDeleted"))
				Dim EnquiryItemsIsDirty As Boolean = CBool(EnquiryItemsArray(i)("mIsDirty"))

				Dim EnquiryItemsDetails = EnquiryItemsArray(i)("mItemDetailForEnquiry")
				Dim EnquiryItemID As New Guid(EnquiryItemsDetails("mItemID").ToString)
				Dim EnquiryItemName As String = EnquiryItemsDetails("mItemName").ToString
				Dim EnquiryItemDescription As String = EnquiryItemsDetails("mItemDescription").ToString

				Dim EnquiryItem As EnquiryItem

				If Enquiry.StatusID = 2 Then

					returnMessage = CheckForPartsNeedsToBeAddedToMaster(EnquiryItemID:=EnquiryItemID,
																		EnquiryItemName:=EnquiryItemName,
																		EnquiryItemDescription:=EnquiryItemDescription)

					If returnMessage IsNot Nothing Then
						Return (Nothing, returnMessage)
					End If

				End If

				If EnquiryIsNew Then

					Enquiry.EnquiryItems.Add(Enquiry.ID)
					EnquiryItem = Enquiry.EnquiryItems.CurrentItem

					GoTo SetEnquiryItemsData

				Else

					If EnquiryItemsIsNew Then

						Enquiry.EnquiryItems.Add(Enquiry.ID)
						EnquiryItem = Enquiry.EnquiryItems.CurrentItem

					Else
						EnquiryItem = Enquiry.EnquiryItems(ID:=EnquiryItemsID)
					End If

					If EnquiryItemsIsDeleted Then
						Enquiry.EnquiryItems.Remove(EnquiryItem)
					End If

				End If

				If EnquiryItemsIsNew Or EnquiryItemsIsDirty Then

SetEnquiryItemsData: With EnquiryItem

						.SrNo = EnquiryItemsArray(i)("mSrNo")
						.ItemID = New Guid(EnquiryItemsArray(i)("mItemID").ToString)
						.ItemDescription = EnquiryItemsArray(i)("mDescription")
						.ItemName = EnquiryItemsArray(i)("mPartNo")
						.ItemTypeID = EnquiryItemsArray(i)("mItemTypeID")
						.Qty = EnquiryItemsArray(i)("mQty")
						.Remark = EnquiryItemsArray(i)("mRemark")
						.Note = EnquiryItemsArray(i)("mNote")
						.RequiredInDays = If(IsNothing(EnquiryItemsArray(i)("mRequiredInDays")), 0, CInt(EnquiryItemsArray(i)("mRequiredInDays")))
						.IPCReference = EnquiryItemsArray(i)("mIPCReference")
						.PriorityID = EnquiryItemsArray(i)("mPriorityID")
						.ModelName = EnquiryItemsArray(i)("mModelName")
						.ModelID = If(EnquiryItemsArray(i)("mModelID") = "", Guid.Empty, CType(EnquiryItemsArray(i)("mModelID"), Guid))
						.ReqItemUnitID = New Guid(EnquiryItemsArray(i)("mReqItemUnitID").ToString)
						.ReqItemUnitName = EnquiryItemsArray(i)("mReqItemUnitName").ToString
						.RequisitionNo = CInt(EnquiryItemsArray(i)("mRequisitionNo"))
						.RequisitionText = EnquiryItemsArray(i)("mRequisitionText").ToString
						.RequisitionNumber = EnquiryItemsArray(i)("mRequisitionNumber").ToString

						'****************************** Requisition Item as Enquiry Item ******************************
						EnquiryItem = SetRequisitionItem(Enquiry:=Enquiry,
														 EnquiryItem:=EnquiryItem,
														 EnquiryItemsIsNew:=EnquiryItemsIsNew,
														 EnquiryItemID:=EnquiryItemID,
														 DateFormat:=DateFormat,
														 RequisitionItemArray:=RequisitionItemAsEnquiryItem)

					End With

				End If

			Next

			Return (Enquiry, "Success")

		Catch ex As Exception
			Return (Nothing, $"{ex.GetBaseException}")
		End Try

	End Function

	Private Function SetEnquiryTerms(_Enquiry As Enquiry,
									 EnquiryTermsArray As JArray,
									 EnquiryIsNew As Boolean) As Enquiry

		Try

			For i As Integer = 0 To EnquiryTermsArray.Count - 1

				Dim EnquiryTermsID As Guid = IIf(EnquiryIsNew,
												 Guid.Empty,
												 New Guid(EnquiryTermsArray(i)("mID").ToString))

				Dim EnquiryTermsIsNew As Boolean = CBool(EnquiryTermsArray(i)("mIsNew"))
				Dim EnquiryTermsIsDeleted As Boolean = CBool(EnquiryTermsArray(i)("mIsDeleted"))
				Dim EnquiryTermsIsDirty As Boolean = CBool(EnquiryTermsArray(i)("mIsDirty"))
				Dim EnquiryTerm As EnquiryTerm

				If EnquiryIsNew Then

					_Enquiry.EnquiryTerms.Add(_Enquiry.ID)
					EnquiryTerm = _Enquiry.EnquiryTerms.CurrentItem

					GoTo SetEnquiryTermData

				Else

					If EnquiryTermsIsNew Then

						_Enquiry.EnquiryTerms.Add(_Enquiry.ID)
						EnquiryTerm = _Enquiry.EnquiryTerms.CurrentItem

					Else
						EnquiryTerm = _Enquiry.EnquiryTerms(ID:=EnquiryTermsID)
					End If

					If EnquiryTermsIsDeleted Then
						_Enquiry.EnquiryTerms.Remove(EnquiryTerm)
					End If

				End If

				If EnquiryTermsIsNew Or EnquiryTermsIsDirty Then

SetEnquiryTermData: With EnquiryTerm

						.SrNo = CInt(EnquiryTermsArray(i)("mSrNo"))
						.EnquiryID = New Guid(EnquiryTermsArray(i)("mEnquiryID").ToString)
						.TermID = New Guid(EnquiryTermsArray(i)("mTermID").ToString)
						.Terms = EnquiryTermsArray(i)("mTerms").ToString

					End With

				End If

			Next

			Return _Enquiry

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function SetEnquirySuppliers(_Enquiry As Enquiry,
										 EnquirySuppliersArray As JArray,
										 EnquiryIsNew As Boolean) As Enquiry

		Try

			For i As Integer = 0 To EnquirySuppliersArray.Count - 1

				Dim EnquirySuppliersID As Guid = IIf(EnquiryIsNew,
													 Guid.Empty,
													 New Guid(EnquirySuppliersArray(i)("mID").ToString))

				Dim EnquirySuppliersIsNew As Boolean = CBool(EnquirySuppliersArray(i)("mIsNew"))
				Dim EnquirySuppliersIsDeleted As Boolean = CBool(EnquirySuppliersArray(i)("mIsDeleted"))
				Dim EnquirySuppliersIsDirty As Boolean = CBool(EnquirySuppliersArray(i)("mIsDirty"))
				Dim EnquirySupplier As EnquirySupplier

				If EnquiryIsNew Then

					_Enquiry.EnquirySuppliers.Add(_Enquiry.ID)
					EnquirySupplier = _Enquiry.EnquirySuppliers.CurrentItem

					GoTo SetEnquirySuppliersData

				Else

					If EnquirySuppliersIsNew Then

						_Enquiry.EnquirySuppliers.Add(_Enquiry.ID)
						EnquirySupplier = _Enquiry.EnquirySuppliers.CurrentItem

					Else
						EnquirySupplier = _Enquiry.EnquirySuppliers(ID:=EnquirySuppliersID)
					End If

					If EnquirySuppliersIsDeleted Then
						_Enquiry.EnquirySuppliers.Remove(EnquirySupplier)
					End If

				End If

				If EnquirySuppliersIsNew Or EnquirySuppliersIsDirty Then

SetEnquirySuppliersData: With EnquirySupplier

						.EnquiryID = New Guid(EnquirySuppliersArray(i)("mEnquiryID").ToString)
						.VendorID = New Guid(EnquirySuppliersArray(i)("mVendorID").ToString)
						.VendorName = EnquirySuppliersArray(i)("mVendorName").ToString
						.ContactPerson = EnquirySuppliersArray(i)("mContactPerson").ToString
						.VendorAddress = EnquirySuppliersArray(i)("mVendorAddress").ToString
						.Phone = EnquirySuppliersArray(i)("mPhone").ToString
						.VendorMail = EnquirySuppliersArray(i)("mVendorMail").ToString

					End With

				End If

			Next

			Return _Enquiry

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function SetRequisitionItem(Enquiry As Enquiry,
										EnquiryItem As EnquiryItem,
										EnquiryItemsIsNew As Boolean,
										EnquiryItemID As Guid,
										DateFormat As String,
										RequisitionItemArray As JArray) As EnquiryItem

		Try

			For i As Integer = 0 To RequisitionItemArray.Count - 1

				Dim ID As Guid = IIf(EnquiryItemsIsNew,
									 Guid.Empty,
									 New Guid(RequisitionItemArray(i)("mID").ToString))

				Dim RequisitionItemAsEnquiryItem As RequisitionItemEnquiryItem

				Dim RequisitionItemIsNew As Boolean = CBool(RequisitionItemArray(i)("mIsNew"))
				Dim RequisitionItemIsDeleted As Boolean = CBool(RequisitionItemArray(i)("mIsDeleted"))
				Dim RequisitionItemIsDirty As Boolean = CBool(RequisitionItemArray(i)("mIsDirty"))

				Dim RequisitionItemID As New Guid(RequisitionItemArray(i)("mReqItemID").ToString)
				Dim RequisitionItemQty As Decimal = CDec(RequisitionItemArray(i)("mQty"))
				Dim RequisitionItemRequisitionNo As String = RequisitionItemArray(i)("mRequisitionNo").ToString
				Dim RequisitionItemRequisitionDate = CDate(RequisitionItemArray(0)("mRequisitionDate").First.First).ToString(DateFormat)

				If RequisitionItemID = Guid.Empty Then Return EnquiryItem

				If EnquiryItemsIsNew Then

					Enquiry.EnquiryItems.CurrentItem.RequisitionItemEnquiryItems.Add(EnquiryItemID:=EnquiryItemID,
																					 RequisitionItemID:=RequisitionItemID,
																					 Qty:=RequisitionItemQty,
																					 RequisitionNo:=RequisitionItemRequisitionNo)
				Else

					If RequisitionItemIsNew Then

						EnquiryItem.RequisitionItemEnquiryItems.Add(EnquiryItemID:=EnquiryItemID,
																	RequisitionItemID:=RequisitionItemID,
																	Qty:=RequisitionItemQty,
																	RequisitionNo:=RequisitionItemRequisitionNo)

					Else

						RequisitionItemAsEnquiryItem = EnquiryItem.RequisitionItemEnquiryItems(ID:=ID)

						If RequisitionItemIsDeleted Then
							EnquiryItem.RequisitionItemEnquiryItems.Remove(RequisitionItemAsEnquiryItem)
						End If

						If RequisitionItemIsDirty Then

							With RequisitionItemAsEnquiryItem

								.EnquiryItemID = EnquiryItem.ID
								.ReqItemID = RequisitionItemID
								.Qty = RequisitionItemQty
								.RequisitionDate = RequisitionItemRequisitionDate

							End With

						End If

					End If

				End If

			Next

			Return EnquiryItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeleteEnquiry(EnquiryID As String) As IHttpActionResult

		Try

			Dim Enquiry As Enquiry = Enquiry.GetEnquiry(ID:=New Guid(EnquiryID))

			Enquiry.Delete()
			Enquiry.Save()

			_ModuleHelper.SendEmailToBytzSoft(TransTypeID:=Enquiry.TransTypeID,
											  Username:=User.Identity.Name,
											  ModuleFrom:="Enquiry",
											  Action:="Delete",
											  ClientCode:=AppSettings("ClientCode"),
											  TransactionNo:=Enquiry.EnquiryNo,
											  TransactionDate:=Enquiry.DateFormatted)

			Return Ok(New ReturnMessage("Success",
											   "Enquiry Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Enquiry",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

#Region " Display Report(s) "

	<HttpPost>
	<Route("api/Enquiry/DisplayReport")>
	Public Function DisplayReport(<FromBody()> requestBody As JObject) As IHttpActionResult

		If requestBody Is Nothing Then

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage(Status:="Error",
												   Message:="Request body cannot be null."))

		End If

		Try

			Dim SuppliersCount As Integer = CInt(requestBody("SuppliersCount"))
			Dim IsVendorDetailsRequired() As Boolean = requestBody("IsVendorDetailsRequired").ToObject(Of Boolean())()
			Dim ID As String = CStr(requestBody("ID"))

			Dim result As ReturnMessage = _ReportHelper.GetRequestForQuotationDetailedReport(ID:=ID,
																							 RequestFromAPI:=True,
																							 SuppliersCount:=SuppliersCount,
																							 IsVendorDetailsRequired:=IsVendorDetailsRequired)
			If result.Status = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Report displayed Successfully!!",
												   ReportData:=result.ReportData))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:="Error occurred while displaying report."))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Exception",
												   Message:=ex.GetBaseException.ToString))

		End Try

	End Function

	<HttpPost>
	<Route("api/Enquiry/DisplayListReport")>
	Public Function DisplayListReport(<FromBody()> requestBody As EnquiryListReportRequest) As IHttpActionResult

		If requestBody Is Nothing Then

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage(Status:="Error",
												   Message:="Request body cannot be null."))

		End If

		Try

			Dim columnHeaders() As String = requestBody.ColumnHeaders
			Dim ItemName As String = requestBody.ItemName
			Dim Text As String = requestBody.Text
			Dim No As Integer = requestBody.No
			Dim FromDate As String = requestBody.FromDate
			Dim ToDate As String = requestBody.ToDate
			Dim StatusID As Integer = requestBody.StatusID
			Dim VendorName As String = requestBody.VendorName
			Dim VendorNo As String = requestBody.VendorNo
			Dim TransTypeID As Integer = requestBody.TransTypeID

			Dim EnquiryList As EnquiryList = EnquiryList.GetEnquiryList(ItemName:=ItemName,
																		Text:=Text,
																		No:=No,
																		FromDate:=FromDate,
																		ToDate:=ToDate,
																		StatusID:=StatusID,
																		VendorName:=VendorName,
																		TransTypeID:=TransTypeID,
																		VendorNo:=VendorNo)

			Dim result = _ReportHelper.ListReport(List:=EnquiryList,
												  ColumnHeaders:=columnHeaders,
												  IsForAPI:=True,
												  ReportOf:="EnquiryList")

			If result.Item2.ToString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Report displayed Successfully!!",
												   ReportData:=result.Item1))

			Else

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:="Error occurred while displaying report."))

			End If

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Exception",
												   Message:=ex.GetBaseException.ToString))

		End Try

	End Function

#End Region

#Region " Send Email "

	<HttpPost>
	<Route("api/Enquiry/SendEmail")>
	Public Function SendEmail(<FromBody()> requestBody As EmailRequest) As IHttpActionResult

		Try

			If String.IsNullOrWhiteSpace(requestBody.EnquiryID) Then
				Return BadRequest("Enquiry ID is required.")
			End If

			If String.IsNullOrWhiteSpace(requestBody.ToMailID) Then
				Return BadRequest("To Email Address is required.")
			End If

			Dim EnquiryID As String = requestBody.EnquiryID
			Dim TransTypeID As Integer = requestBody.TransTypeID
			Dim SuppliersCount As Integer = requestBody.SuppliersCount
			Dim IsVendorDetailsRequired() As Boolean = requestBody.IsVendorDetailsRequired
			Dim Remark As String = IIf(requestBody.Remark IsNot Nothing, requestBody.Remark, "")
			Dim ToMailID As String = IIf(requestBody.ToMailID IsNot Nothing, requestBody.ToMailID, "")
			Dim CCMailID As String = IIf(requestBody.CCMailID IsNot Nothing, requestBody.CCMailID, "")
			Dim BCCMailID As String = IIf(requestBody.BCCMailID IsNot Nothing, requestBody.BCCMailID, "")
			Dim AttachmentName As String = IIf(requestBody.AttachmentName IsNot Nothing, requestBody.AttachmentName, "")
			Dim ReportGeneratedBy As String = IIf(requestBody.ReportGeneratedBy IsNot Nothing, requestBody.ReportGeneratedBy, "")

			Dim response As ReturnMessage = _EmailHelper.SendEmail(Remark:=Remark,
																   ToMailID:=ToMailID,
																   CCMailID:=CCMailID,
																   BCCMailID:=BCCMailID,
																   EnquiryID:=EnquiryID,
																   Text:=AttachmentName,
																   ModuleName:="Enquiry",
																   TransTypeID:=TransTypeID,
																   AttachmentName:=AttachmentName,
																   SuppliersCount:=SuppliersCount,
																   ReportGeneratedBy:=ReportGeneratedBy,
																   IsVendorDetailsRequired:=IsVendorDetailsRequired)

			If response.Status = "Success" Then
				Return Ok(New ReturnMessage($"{response.Status}", $"{response.Message}"))
			Else
				Return Content(HttpStatusCode.BadRequest, New ReturnMessage($"{response.Status}", $"{response.Message}"))
			End If

		Catch ex As Exception
			Return Content(HttpStatusCode.BadRequest, ex.Message)
		End Try

	End Function

#End Region

#Region " Helper Method(s) "

	Private Function CheckIfCustomerAndSupplierAreSame(Enquiry As Enquiry) As String

		Try

			For i As Integer = 0 To Enquiry.EnquirySuppliers.Count - 1

				If Enquiry.EnquirySuppliers(i).VendorID.Equals(Enquiry.CustomerID) Then

					Return "Record can not be saved. Supplier & Customer are same. Select another Customer from list."

				End If

			Next

			Return Nothing

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function CheckIfVendorIsApplicableWhileSave(Enquiry As Enquiry, VendorList As VendorList) As String

		Dim ReturnMessage As String
		Try

			For i As Integer = 0 To Enquiry.EnquirySuppliers.Count - 1

				If VendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUse = True Then

					ReturnMessage = CheckIfVendorIsApplicable(EnquiryDate:=Enquiry.Date,
															  VendorNotInUseDate:=VendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUseDate,
															  VendorNotInUseDateFormatted:=VendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUseDateFormatted,
															  VendorName:=VendorList(Enquiry.EnquirySuppliers(i).VendorID).Name)

					Return IIf(ReturnMessage IsNot Nothing, ReturnMessage, Nothing)

				End If

			Next

			Return Nothing

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function CheckIfVendorIsApplicableWhileAuthorize(Enquiry As Enquiry, VendorList As VendorList) As String

		Dim ReturnMessage As String
		Try

			If VendorList(Enquiry.VendorID).NotInUse = True Then

				ReturnMessage = CheckIfVendorIsApplicable(EnquiryDate:=Enquiry.Date,
														  VendorNotInUseDate:=VendorList(Enquiry.VendorID).NotInUseDate,
														  VendorNotInUseDateFormatted:=VendorList(Enquiry.VendorID).NotInUseDateFormatted,
														  VendorName:=VendorList(Enquiry.VendorID).Name)

				Return IIf(ReturnMessage IsNot Nothing, ReturnMessage, Nothing)

			End If

			Return Nothing

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function CheckIfVendorIsApplicable(EnquiryDate As Object,
											   VendorNotInUseDate As Object,
											   VendorNotInUseDateFormatted As Object,
											   VendorName As String) As String

		Try

			If CDate(VendorNotInUseDate) <= CDate(EnquiryDate) Then

				Return $"Record can not be saved. 
						Supplier {VendorName} 
                        is not applicable since 
                        {VendorNotInUseDateFormatted}  
                        Select another Supplier from list or select Date before 
                        {VendorNotInUseDateFormatted}
                        Please Try Again."

			End If

			Return Nothing

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function CheckIfEnquiryIsForCustomer(Enquiry As Enquiry, VendorList As VendorList) As String

		Try

			If Enquiry.IsCustomer = True Then

				If VendorList(Enquiry.CustomerID).NotInUse = True Then

					If CDate(VendorList(Enquiry.CustomerID).NotInUseDate) <= CDate(Enquiry.Date) Then

						Return $"Record can not be saved. Customer is not Applicable since 
                                {VendorList(Enquiry.CustomerID).NotInUseDateFormatted} 
                                Select another Customer from list or select Date before 
                                {VendorList(Enquiry.VendorID).NotInUseDateFormatted} 
                                Please Try Again."

					End If

				End If

			End If

			Return Nothing

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function CheckForPartsNeedsToBeAddedToMaster(EnquiryItemID As Guid,
														 EnquiryItemName As String,
														 EnquiryItemDescription As String) As String

		Dim PartList As String = ""
		Dim BlankPartsCount As Integer = 0

		Try

			If EnquiryItemID.Equals(Guid.Empty) Then
				BlankPartsCount = BlankPartsCount + 1
				PartList = $"{PartList} {BlankPartsCount} ) {EnquiryItemName}  ( {EnquiryItemDescription} )"
			End If

			If BlankPartsCount > 0 And PartList <> "" Then
				Return $"Following Part(s) needs to be added to Part Master - {PartList}"
			End If

			Return Nothing

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function VendorStatus(TransTypeID As Integer, Type As Integer) As Boolean

		Try

			If Type = 0 Then                                  ''Purchase Enquiry 

				Select Case CType(TransTypeID, Trans)
					Case Trans.RequestingForQuotation
						Return True
					Case Trans.OverHaulRepairEnquiry
						Return True
					Case Trans.RentialLeaseEnquiry
						Return True
					Case Else
						Return False
				End Select

			ElseIf Type = 1 Then                              'Sales Enquiry

				Select Case CType(TransTypeID, Trans)
					Case Trans.Enquiry
						Return True
					Case Else
						Return False
				End Select

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function CheckIfPartIsApplicable(Enquiry As Enquiry, VendorList As VendorList) As String

		Dim ReturnMessage As String
		Try

			For i As Integer = 0 To Enquiry.EnquirySuppliers.Count - 1

				If VendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUse = True Then

					ReturnMessage = CheckIfVendorIsApplicable(EnquiryDate:=Enquiry.Date,
															  VendorNotInUseDate:=VendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUseDate,
															  VendorNotInUseDateFormatted:=VendorList(Enquiry.EnquirySuppliers(i).VendorID).NotInUseDateFormatted,
															  VendorName:=VendorList(Enquiry.EnquirySuppliers(i).VendorID).Name)

					Return IIf(ReturnMessage IsNot Nothing, ReturnMessage, Nothing)

				End If

			Next

			Return Nothing

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class