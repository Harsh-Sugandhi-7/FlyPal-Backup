'***********************************
'Modified by Harsh Sugandhi
'***********************************


Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Web.Http
Imports System.Web.UI.DataVisualization.Charting

Imports Newtonsoft.Json.Linq


Public Class ReceiptCumInvoiceController
	Inherits ApiController


#Region " Variable(s) Declarations "

	Private mOrder As Order
	Private mVendor As Vendor
	Private _MessageBox As New MSGBox
	Private _RCIHelper As New RCIHelper
	Private _GSTPercentage As GSTPercentage
	Private _EmailHelper As New EmailHelper
	Private _ReportHelper As New ReportHelper
	Private _ModuleHelper As New ModuleHelper
	Private _ResponseWrapper As New ResponseWrapper
	Private _AttachmentHelper As New AttachmentHelper
	Private _BrokenRulesHelper As New BrokenRulesHelper
	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _StatusWiseReturnMessage As New StatusWiseReturnMessage
	Private _CheckForSubscriptionExpired As New CheckForSubscriptionExpired

	Dim Action As String = "Saved"
	Dim DateFormat As String = ""
	Dim _SerializedStatus As Boolean = False
	Dim _ReceiptQuantityExceedingOrderQuantityMessage As String = $"Validations, Receipt quantity exceeds Order quantity.{Environment.NewLine}If you proceed the Order details including the total Order amount will be updated.{Environment.NewLine}Do you want to continue & update ?"

#End Region

#Region " Function(s) "

	Public Function GetValues(Optional FromDate As String = "1-Jan-1900",
							  Optional ToDate As String = "1-Jan-4400",
							  Optional ReceiptText As String = "",
							  Optional ReceiptNo As Integer = 0,
							  Optional IntReceiptNo As String = "",
							  Optional VendorName As String = "",
							  Optional AircraftName As String = "",
							  Optional StoreName As String = "",
							  Optional DCNO As String = "",
							  Optional StatusID As Integer = 0,
							  Optional ItemName As String = "",
							  Optional OrderText As String = "",
							  Optional OrderNo As Integer = 0,
							  Optional IssueText As String = "",
							  Optional IssueNo As Integer = 0,
							  Optional ReleaseNoteNo As String = "",
							  Optional Type As Integer = 0,
							  Optional InvoiceText As String = "",
							  Optional InvoiceNo As Integer = 0,
							  Optional TransTypeID As Trans = Util.Trans.None,
							  Optional CustomerName As String = "",
							  Optional AWBNo As String = "",
							  Optional IsCustomPaging As Boolean = False,
							  Optional CurrentPage As Integer = 0,
							  Optional PageSize As Integer = 25,
							  Optional SerialNo As String = "",
							  Optional Description As String = "",
							  Optional ReceivedFromType As Integer = 0,
							  Optional WorkShopName As String = "",
							  Optional WOText As String = "",
							  Optional WONo As Integer = 0,
							  Optional BatchNo As String = "",
							  Optional ReceivedEmpName As String = "",
							  Optional CodeNo As String = "",
							  Optional CategoryID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional SearchText As String = "") As ReceiptCumInvoiceList

		Return ReceiptCumInvoiceList.GetReceiptCumInvoiceList(FromDate:=FromDate,
															  ToDate:=ToDate,
															  ReceiptText:=ReceiptText,
															  ReceiptNo:=ReceiptNo,
															  IntReceiptNo:=IntReceiptNo,
															  VendorName:=VendorName,
															  AircraftName:=AircraftName,
															  StoreName:=StoreName,
															  DCNO:=DCNO,
															  StatusID:=StatusID,
															  ItemName:=ItemName,
															  OrderText:=OrderText,
															  OrderNo:=OrderNo,
															  IssueText:=IssueText,
															  IssueNo:=IssueNo,
															  ReleaseNoteNo:=ReleaseNoteNo,
															  Type:=Type,
															  InvoiceText:=InvoiceText,
															  InvoiceNo:=InvoiceNo,
															  TransTypeID:=TransTypeID,
															  CustomerName:=CustomerName,
															  AWBNo:=AWBNo,
															  IsCustomPaging:=IsCustomPaging,
															  CurrentPage:=CurrentPage,
															  PageSize:=PageSize,
															  SerialNo:=SerialNo,
															  Description:=Description,
															  ReceivedFromType:=ReceivedFromType,
															  WorkShopName:=WorkShopName,
															  WOText:=WOText,
															  WONo:=WONo,
															  BatchNo:=BatchNo,
															  ReceivedEmpName:=ReceivedEmpName,
															  CodeNo:=CodeNo,
															  CategoryID:=CategoryID,
															  SearchText:=SearchText)
	End Function

	Public Function GetPendingToReceiveTransItemList(TransTypeID As Trans,
													 FromID As Guid,
													 Type As Integer,
													 ReceiptDate As String,
													 TransID As Guid,
													 Optional IsReturnableFromCustomer As Boolean = False,
													 Optional ItemID As String = "{00000000-0000-0000-0000-000000000000}",
													 Optional OrderItemID As String = "{00000000-0000-0000-0000-000000000000}",
													 Optional IsFromIssueBERParts As Boolean = False) As PendingToReceiveTransItemList

		Return PendingToReceiveTransItemList.GetPendingToReceiveTransItemList(TransTypeID:=TransTypeID,
																			  FromID:=FromID,
																			  Type:=Type,
																			  ReceiptDate:=ReceiptDate,
																			  TransID:=TransID,
																			  IsReturnableFromCustomer:=IsReturnableFromCustomer,
																			  ItemID:=ItemID,
																			  OrderItemID:=OrderItemID,
																			  IsFromIssueBERParts:=IsFromIssueBERParts)
	End Function

	Public Function GetReceiptCumInvoice(ReceiptID As String, InvoiceID As String) As ReceiptCumInvoice

		Return ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID:=New Guid(ReceiptID),
													  InvoiceID:=New Guid(InvoiceID))

	End Function

	Public Function GetReceiptTypeList(Optional ReceivedFrom As Integer = 0) As ReceiptTypeList

		Return ReceiptTypeList.GetReciptTypeList(ReceivedFrom:=ReceivedFrom)

	End Function

	Public Function GetPartListForRCIFromAircraftAsCoreUnitReturn(Optional ItemName As String = "",
																  Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
																  Optional [Date] As String = "") As PartListForRCIFromAircraftAsCoreUnitReturnList

		Return PartListForRCIFromAircraftAsCoreUnitReturnList.GetPartListForRCIFromAircraftAsCoreUnitReturn(ItemName:=ItemName,
																											MachineID:=MachineID,
																											[Date]:=[Date])
	End Function

	Public Function GetPendingLoanToRecover(Optional ToStoreID As String = "{00000000-0000-0000-0000-000000000000}",
											Optional ItemName As String = "",
											Optional ReceiptDate As String = "") As PendingLoanToRecover

		Return PendingLoanToRecover.GetPendingLoanToRecover(ToStoreID:=New Guid(ToStoreID),
															ItemName:=ItemName,
															ReceiptDate:=ReceiptDate)

	End Function

	Public Function GetPartListForReceivedFromWorkShopAsServiceableReturned(Optional ItemName As String = "",
																			Optional WorkShopID As String = "{00000000-0000-0000-0000-000000000000}",
																			Optional [Date] As String = "") As PartListForReceivedFromWorkShopAsServiceablReturned

		Return PartListForReceivedFromWorkShopAsServiceablReturned.GetPartListForRCIFromAircraftAsCoreUnitReturn(ItemName:=ItemName,
																												 WorkShopID:=WorkShopID,
																												 Date:=[Date])
	End Function

	Public Function GetOrderItemDetailForReceipt(OrderItemID As Guid) As OrderItemDetailForReceipt

		Return OrderItemDetailForReceipt.GetOrderItemDetailForReceipt(OrderItemID:=OrderItemID)

	End Function

	Public Function GetIssueItemDetailForReceipt(IssueItemID As Guid) As IssueItemDetailForReceipt

		Return IssueItemDetailForReceipt.GetIssueItemDetailForReceipt(IssueItemID:=IssueItemID)

	End Function

	Public Function GetReceiptBalanceQtyStatusForReceipt(OrderItemID As Guid, LookInType As Integer) As ReceiptBalanceQtyStatusForReceipt

		Return ReceiptBalanceQtyStatusForReceipt.GetReceiptBalanceQtyStatusForReceipt(ID:=OrderItemID,
																					  LookInType:=LookInType) ' 3 for Order  '4 - Issue

	End Function

	Public Function GetWarrantyStatusList(Optional IsSelectTagRequired As Boolean = False, Optional AddTopItem As String = "(SELECT)") As WarrantyStatusList
		Return WarrantyStatusList.GetWarrantyStatusList(IsSelectTagRequired:=IsSelectTagRequired, AddTopItem:=AddTopItem)
	End Function

	Public Function GetDateForPreMatureFailure(Optional ItemID As String = "{00000000-0000-0000-0000-000000000000}",
											   Optional SerialNo As String = "",
											   Optional ReceiptDate As String = "1-Jan-1999",
											   Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}") As DateForPreMatureFailure
		Return DateForPreMatureFailure.GetDateForPreMatureFailure(ItemID:=ItemID,
																  SerialNo:=SerialNo,
																  ReceiptDate:=ReceiptDate,
																  MachineID:=MachineID)
	End Function

	Public Function GetLastWarrantyInformation(Optional ItemID As String = "{00000000-0000-0000-0000-000000000000}",
											   Optional SerialNo As String = "") As LastWarrantyInformation
		Return LastWarrantyInformation.GetLastWarrantyInformation(ItemID:=ItemID,
																  SerialNo:=SerialNo)
	End Function

	Public Function GetLastServicedInspectedDoneOnDate(Optional ItemID As String = "{00000000-0000-0000-0000-000000000000}",
													   Optional SerialNo As String = "") As LastServicedInspectedDoneOnDate
		Return LastServicedInspectedDoneOnDate.GetLastServicedInspectedDoneOnDate(ItemID:=ItemID,
																				  SerialNo:=SerialNo)
	End Function

	Public Function GetReceiptItemKitItemsList(Optional ItemID As String = "{00000000-0000-0000-0000-000000000000}",
											   Optional SerialNo As String = "") As ReceiptItemKitItems
		Return ReceiptItemKitItems.GetReceiptItemKitItemsList(ItemID:=ItemID, SerialNo:=SerialNo.Trim)
	End Function

	Public Function GetTypeList([Of] As String, TransTypeID As Integer, Optional IsSelectTagRequired As Boolean = False) As TypeListForReceipt
		Return TypeListForReceipt.GetTypeList([Of]:=[Of], TransTypeID:=TransTypeID)
	End Function

	Public Function GetRateAndOtherChargeForRCI(IssueItemID As Guid) As RateAndOtherChargeForRCI
		Return RateAndOtherChargeForRCI.GetRateAndOtherChargeForRCI(IssueItemID:=IssueItemID)
	End Function

	Public Function GetNewCharges(ReceiptID As Guid,
								  InvoiceID As Guid) As InvoiceCharge

		Try

			Dim mReceiptCumInvoice As ReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID:=ReceiptID,
																								 InvoiceID:=InvoiceID)

			mReceiptCumInvoice.Invoice.InvoiceCharges.Add(ID:=InvoiceID)

			Return mReceiptCumInvoice.Invoice.InvoiceCharges.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'This method requires RecCumInvDate which is nothing but RecdDate from Receipt. Hence the Para name is same to correlate
	<HttpGet>
	Public Function IsInDate(RecdDate As Date) As Boolean

		Dim DueDay As Integer
		Dim ReceiptDate As Date
		Dim PrevReceiptDate As Date

		Try

			DueDay = AppSettings("dueDay")
			ReceiptDate = New Date(Year(RecdDate), Month(RecdDate) + 1, DueDay)
			PrevReceiptDate = New Date(Year(RecdDate), Month(RecdDate), 1)

			If Today.Date >= PrevReceiptDate And (Today.Date <= ReceiptDate) Then
				Return True
			Else
				Return False
			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function

	'If the Method return true the StatusID should be set to 2
	<HttpGet>
	Public Function CheckDateForTransactionLock(TransactionDate As Date) As Boolean

		Dim FirstDayofMonth As Date
		Dim FirstDayofLastMonth As Date

		Try

			FirstDayofMonth = DateSerial(Year(Today.Date), Month(Today.Date), 1)
			FirstDayofLastMonth = DateSerial(Year(Today.Date), Month(Today.Date), 1).AddMonths(-1)

			If (TransactionDate >= FirstDayofLastMonth) Then

				If (TransactionDate < FirstDayofMonth) And (Day(Today.Date) > 10) Then

					Return True

				Else

					Return False

				End If

			Else

				Return True

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function

	<HttpGet>
	Public Function CheckForConditionCheckItemComply(TransTypeID As Integer,
													 StatusID As Integer,
													 ReceiptID As String,
													 InvoiceID As String) As Boolean

		Dim mReceiptCumInvoice As ReceiptCumInvoice
		Dim mReceiptItemServiceInspection As ReceiptItemServiceInspection
		Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
		Try

			mReceiptCumInvoice = GetReceiptCumInvoice(ReceiptID:=ReceiptID,
													  InvoiceID:=InvoiceID)


			If (StatusID = 2 And TransTypeID = 10) Then

				For Each mReceiptCumInvoiceItem In mReceiptCumInvoice.ReceiptCumInvoiceItems

					For Each mReceiptItemServiceInspection In mReceiptCumInvoiceItem.ReceiptItem.ReceiptItemServiceInspections

						If Not IsDBNull(mReceiptItemServiceInspection.ServiedInspectedCheckDoneOnDate) Then

							Return True
							Exit Function

						End If

					Next

				Next

			End If

			Return False

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function

	' GET New ReceiptCumInvoice
	Public Function GetNewReceiptCumInvoice(Optional TransTypeID As Trans = Util.Trans.None) As ReceiptCumInvoice

		Return ReceiptCumInvoice.NewReceiptCumInvoice(ID:=Guid.Empty, TransTypeID:=TransTypeID)

	End Function

	' GET  New ReceiptCumInvoice Item
	<HttpGet>
	<Route("api/ReceiptCumInvoice/GetNewReceiptCumInvoiceItem")>
	Public Function GetNewReceiptCumInvoiceItem(ReceiptCumInvoiceID As Guid, TransTypeID As Integer) As ReceiptCumInvoiceItem

		Dim _ReceiptCumInvoiceItem As ReceiptCumInvoiceItem
		Try

			Dim _ReceiptCumInvoice As ReceiptCumInvoice = ReceiptCumInvoice.NewReceiptCumInvoice(ID:=New Guid,
																								 TransTypeID:=TransTypeID)

			_ReceiptCumInvoice.ReceiptCumInvoiceItems.Add(ID:=ReceiptCumInvoiceID)

			_ReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.Add(ReceiptItemID:=_ReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																									  ItemID:=Guid.Empty,
																									  SerialNo:="")

			_ReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.Add(ReceiptItemPeriod.NewReceiptItemPeriod(ReceiptItemID:=_ReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																																				 TransType:=_ReceiptCumInvoice.Receipt.TransTypeID,
																																				 PeriodID:=0))

			_ReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Add(ReceiptItemID:=_ReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																												ItemServiceInspectionsID:=Guid.Empty,
																												ItemServiceInspectionDescription:="",
																												ItemServiceInspectionFrequency:=0,
																												ItemServiceInspectionFrequencyPeriod:=0,
																												ItemID:=Guid.Empty.ToString)

			_ReceiptCumInvoiceItem = _ReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem

			Return _ReceiptCumInvoiceItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	' GET  New ReceiptCumInvoice Charge
	Public Function GetNewReceiptCumInvoiceCharge(ReceiptCumInvoiceID As Guid, TransTypeID As Integer) As InvoiceCharge

		Dim mReceiptCumInvoice As ReceiptCumInvoice = ReceiptCumInvoice.NewReceiptCumInvoice(ID:=New Guid, TransTypeID:=TransTypeID)

		mReceiptCumInvoice.ReceiptCumInvoiceCharges.Add(ID:=ReceiptCumInvoiceID)

		Return mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem

	End Function

	' GET  New Receipt Item Period
	Public Function GetNewReceiptItemPeriod(ReceiptCumInvoiceItemID As Guid, TransTypeID As Integer, PeriodID As Integer) As ReceiptItemPeriod

		Return ReceiptItemPeriod.NewReceiptItemPeriod(ReceiptCumInvoiceItemID, TransTypeID, PeriodID)

	End Function

	Public Function GetPendingTools(Optional ItemName As String = "",
									Optional ReceiptDate As String = "",
									Optional EmployeeName As String = "",
									Optional BarcodeNo As String = "",
									Optional FromDate As String = "1-1-1900",
									Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional WOID As String = "{00000000-0000-0000-0000-000000000000}",
									Optional CodeNo As String = "",
									Optional UserName As String = "",
									Optional ToolsCheckInAgainstID As Integer = 0) As PendingToolsToReceiveFromEmployee

		Return PendingToolsToReceiveFromEmployee.GetPendingTools(ItemName:=ItemName,
																 ReceiptDate:=ReceiptDate,
																 EmployeeName:=EmployeeName,
																 BarcodeNo:=BarcodeNo,
																 FromDate:=FromDate,
																 MachineID:=MachineID,
																 WOID:=WOID,
																 CodeNo:=CodeNo,
																 UserName:=UserName,
																 ToolsCheckInAgainstID:=ToolsCheckInAgainstID)

	End Function

	Public Function GetRequisitionList(Optional SelectTag As String = "") As ToolsCheckInAgainstList

		Return ToolsCheckInAgainstList.GetRequisitionList(SelectTag:=SelectTag)

	End Function

#End Region

#Region " Save RCI "

	Public Function PostValue(<FromBody()> JObject As JObject) As IHttpActionResult

		Dim returnMessage As ReturnMessage
		Dim IsNew As Boolean = CBool(JObject("mIsNew"))
		Try

			If IsNew Then
				returnMessage = SetNewReceiptCumInvoiceValues(JObject)
			Else
				returnMessage = SetExistingReceiptCumInvoiceValues(JObject)
			End If

			_ResponseWrapper = _StatusWiseReturnMessage.GenerateResponseMessage(returnMessage:=returnMessage)
			Return Content(_ResponseWrapper.StatusCode, _ResponseWrapper.ReturnMessage)

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Exception",
												   Message:=$"Exception Occurred. Message: {ex.GetBaseException}"))

		End Try

	End Function

	Private Function SetNewReceiptCumInvoiceValues(JObject As JObject) As ReturnMessage

		Try


			Dim returnMessage As String =
				_CheckForSubscriptionExpired.
					CheckForSubscriptionExpired(TransactionDate:=CDate(JObject(propertyName:="mReceipt")("mRecdDate").First.First))

			If returnMessage <> "Success" Then
				Return New ReturnMessage(Status:="Validation",
										 Message:=returnMessage)
			End If

			Dim mReceiptCumInvoice As ReceiptCumInvoice =
				ReceiptCumInvoice.NewReceiptCumInvoice(ReceiptID:=New Guid(JObject(propertyName:="mReceipt")(key:="mID").ToString()),
													   InvoiceID:=New Guid(JObject(propertyName:="mInvoice")(key:="mID").ToString()),
													   TransTypeID:=CInt(JObject(propertyName:="mReceipt")(key:="mTransTypeID").ToString()))

			Dim ItemArray As JArray = CType(JObject(propertyName:="mReceiptCumInvoiceItems"), JArray)
			Dim InvoiceItemArray As JArray = CType(JObject(propertyName:="mInvoice")(key:="mInvoiceItems"), JArray)
			Dim ChargeArray As JArray = CType(JObject(propertyName:="mInvoice")(key:="mInvoiceCharges"), JArray)
			Dim AttachmentArray As JArray = CType(JObject(propertyName:="mFileAttachments"), JArray)

			SetReceiptCumInvoice(JObject:=JObject,
								 ReceiptCumInvoice:=mReceiptCumInvoice)

			For i As Integer = 0 To ItemArray.Count - 1

				mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(ID:=mReceiptCumInvoice.ID,
															  ReceiptItemID:=New Guid(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mID").ToString),
															  InvoiceItemID:=New Guid(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mInvoiceItem").Item(key:="mID").ToString))

				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SrNo = CInt(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mSrNo"))

				SetReceiptCumInvoiceItem(i:=i,
										 JObject:=JObject,
										 ReceiptCumInvoice:=mReceiptCumInvoice,
										 ReceiptCumInvoiceItem:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)

				'Added On 7-Jul-2025
				With mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem

					If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 3 Then
						_SerializedStatus = CBool((JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mOrderItemDetailForReceipt").Item(key:="mSerializedStatus").ToString))
					ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 4 Then
						_SerializedStatus = CBool((JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mIssueItemDetailForReceipt").Item(key:="mSerializedStatus").ToString))
					ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList Or
						   (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID <> 3 And
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID <> 4) Then
						_SerializedStatus = CBool((JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mIsPartFromListisSerialized").ToString))
					End If

					If _SerializedStatus Then

						If .DuplicateSerialNo() Then
							Return New ReturnMessage(Status:="Validation",
													 Message:=$"Serial number { .SerialNo} already exist for the Item { .ItemName}. Cannot add Duplicate.")
						End If

						If .PrimaryCategoryID = 2 AndAlso CBool(AppSettings("CodeNo")) Then

							If mReceiptCumInvoice.TransTypeID = 7 Then

								If .DuplicateCodeNo(TypeToRead:=1) Then
									Return New ReturnMessage(Status:="Validation",
															 Message:=$"Code No. entered for Item { .ItemName} ({ .SerialNo}) already exist. Please enter another Code No.")
								End If

							Else

								If .DuplicateCodeNo(TypeToRead:=2) OrElse .DuplicateCodeNo(TypeToRead:=3) OrElse .DuplicateCodeNo(TypeToRead:=4) Then '2 Duplication checking with CodeNo,ItemID,Serial No.
									Return New ReturnMessage(Status:="Validation",
															 Message:=$"Code No. entered for Item { .ItemName} ( { .SerialNo} ) already exist. Please enter another Code No.")

								End If

							End If

						End If

					End If

				End With
				'-------------------

				'---------------------------------ReceiptItemKitItems-------------------------------------------------------------------
				Dim ReceiptItemKitItemsArray As JArray = CType(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mReceiptItemKitItems"), JArray)

				For l As Integer = 0 To ReceiptItemKitItemsArray.Count - 1

					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.Add(ID:=New Guid(ReceiptItemKitItemsArray(l)("mID").ToString),
																											  ReceiptItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																											  ItemID:=New Guid(ReceiptItemKitItemsArray(l)("mItemID").ToString),
																											  SerialNo:=ReceiptItemKitItemsArray(l)("mSerialNo"))

					With mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem

						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.ItemIDFromKitItem = New Guid(ReceiptItemKitItemsArray(l)("mItemIDFromKitItem").ToString)
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.SerialNoForItemIDOfKitItem = ReceiptItemKitItemsArray(l)("mSerialNoForItemIDOfKitItem")
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.KitItemID = New Guid(ReceiptItemKitItemsArray(l)("mKitItemID").ToString)
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.Remark = ReceiptItemKitItemsArray(l)("mRemark").ToString
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.ItemName = ReceiptItemKitItemsArray(l)("mItemName").ToString
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.ItemDescription = ReceiptItemKitItemsArray(l)("mItemDescription").ToString
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.KitItemQty = CDec(ReceiptItemKitItemsArray(l)("mKitItemQty"))

					End With

				Next
				'---------------------------------End OF ReceiptItemKitItems-------------------------------------------------------------------

				'---------------------------------ReceiptItemPeriods-------------------------------------------------------------------
				Dim ReceiptItemPeriodsArray As JArray = CType(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mReceiptItemPeriods"), JArray)

				For m As Integer = 0 To ReceiptItemPeriodsArray.Count - 1

					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.Add(ID:=New Guid(ReceiptItemPeriodsArray(m)("mID").ToString),
																											ReceiptItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																											TransType:=mReceiptCumInvoice.Receipt.TransTypeID,
																											PeriodID:=CInt(ReceiptItemPeriodsArray(m)("mPeriodID").ToString))

					With mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem

						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.TSNValue = ReceiptItemPeriodsArray(m)("mTSNValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mTSNValue").ToString
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.TSOValue = ReceiptItemPeriodsArray(m)("mTSOValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mTSOValue").ToString
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.TSIValue = ReceiptItemPeriodsArray(m)("mTSIValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mTSIValue").ToString
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.CSNValue = ReceiptItemPeriodsArray(m)("mCSNValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mCSNValue").ToString
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.CSOValue = ReceiptItemPeriodsArray(m)("mCSOValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mCSOValue").ToString
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.CSIValue = ReceiptItemPeriodsArray(m)("mCSIValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mCSIValue").ToString

					End With

				Next
				'---------------------------------End OF ReceiptItemPeriods-------------------------------------------------------------------

				'---------------------------------ReceiptItemServiceInspections-------------------------------------------------------------------
				Dim ReceiptItemServiceInspectionsArray As JArray = CType(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mReceiptItemServiceInspections"), JArray)

				For n As Integer = 0 To ReceiptItemServiceInspectionsArray.Count - 1

					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Add(ID:=New Guid(ReceiptItemServiceInspectionsArray(n)("mID").ToString),
																														ReceiptItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																														ItemServiceInspectionsID:=New Guid(ReceiptItemServiceInspectionsArray(n)("mItemServiceInspectionsID").ToString),
																														ItemServiceInspectionDescription:=ReceiptItemServiceInspectionsArray(n)("mItemServiceInspectionDescription"),
																														ItemServiceInspectionFrequency:=CInt(ReceiptItemServiceInspectionsArray(n)("mItemServiceInspectionFrequency").ToString),
																														ItemServiceInspectionFrequencyPeriod:=CInt(ReceiptItemServiceInspectionsArray(n)("mItemServiceInspectionFrequencyPeriod").ToString),
																														ItemID:=ReceiptItemServiceInspectionsArray(n)("mItemID").ToString)

					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.CurrentItem.ServiedInspectedCheckDoneOnDate = CDate(ReceiptItemServiceInspectionsArray(n)("mServiedInspectedCheckDoneOnDate").First.First).ToString(format:=DateFormat)

				Next
				'---------------------------------End OF ReceiptItemServiceInspections-------------------------------------------------------------------

			Next

			For k As Integer = 0 To ChargeArray.Count - 1

				mReceiptCumInvoice.Invoice.InvoiceCharges.Add(ID:=mReceiptCumInvoice.Invoice.ID,
															  InvoiceChargeID:=New Guid(ChargeArray(k)("mID").ToString))

				With mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem

					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.SrNo = CInt(ChargeArray(k)("mSrNo"))
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.InvoiceID = New Guid(ChargeArray(k)("mInvoiceID").ToString)
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.ChargeID = New Guid((ChargeArray(k)("mChargeID").ToString))
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.StatusBasic = CBool(ChargeArray(k)("mStatusBasic"))
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.Percentage = CDec(ChargeArray(k)("mPercentage"))
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.CChargeAmount = CDec(ChargeArray(k)("mCChargeAmount"))
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.Currency = ChargeArray(k)("mCurrency")
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.BasicAmount = CDec(ChargeArray(k)("mBasicAmount"))
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.TotalAmount = CDec(ChargeArray(k)("mTotalAmount"))
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem.ConversionFactor = CDec(ChargeArray(k)("mConversionFactor"))

				End With

			Next

			Dim result = _AttachmentHelper.SaveAttachments(AttachmentArray:=AttachmentArray,
														   ModuleObject:=mReceiptCumInvoice,
														   ModuleName:="RCI")

			returnMessage = result.Item2

			If Not String.IsNullOrEmpty(returnMessage) Then
				Return New ReturnMessage(Status:="Validation",
										 Message:=returnMessage)
			End If

			mReceiptCumInvoice = CType(result.Item1, ReceiptCumInvoice)

			mReceiptCumInvoice.Invoice.CalculateTotal()

			If mReceiptCumInvoice.IsRoundOff Then
				mReceiptCumInvoice.Invoice.RoundCGrandTotal()
			End If

			If mReceiptCumInvoice.IsValid Then

				HttpContext.Current.Session("ReceiptCumInvoice") = mReceiptCumInvoice

				If mReceiptCumInvoice.TransTypeID = Trans.ReceiptcumInvoiceAgainstPuchaseOrder AndAlso
				   (_RCIHelper.CheckIfReceiptQuantityExceedsOrderQuantity(ReceiptCumInvoice:=mReceiptCumInvoice)) Then

					Return New ReturnMessage(Status:="Validation",
											 Message:=$"{_ReceiptQuantityExceedingOrderQuantityMessage}")
				End If

				mReceiptCumInvoice.Save()

				_ModuleHelper.SendEmailToBytzSoft(TransTypeID:=mReceiptCumInvoice.TransTypeID,
												  Username:=User.Identity.Name,
												  ModuleFrom:="ReceiptCumInvoice",
												  Action:=Action,
												  ClientCode:=AppSettings("ClientCode"),
												  TransactionNo:=mReceiptCumInvoice.ReceiptNo,
												  TransactionDate:=mReceiptCumInvoice.RecCumInvDateFormatted)
			Else
				Return New ReturnMessage(Status:="Validation",
										 Message:=$"{_BrokenRulesHelper.FetchBrokenRules(CommonObject:=mReceiptCumInvoice, ModuleName:="RCI")}")
			End If

			Return New ReturnMessage(Status:="Success",
									 Message:=$"RCI saved Successfully!")

		Catch ex As SqlException
			Return New ReturnMessage(Status:="SqlException",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="RCI", ex:=ex)}")
		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="RCI", ex:=ex, UseAsException:=True)}")
		End Try

	End Function

	Private Function SetExistingReceiptCumInvoiceValues(JObject As JObject) As ReturnMessage

		Dim complyMessage As ReturnMessage
		Try

			Dim mReceiptCumInvoice As ReceiptCumInvoice =
				ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID:=New Guid(JObject(propertyName:="mReceipt")(key:="mID").ToString),
													   InvoiceID:=New Guid(JObject(propertyName:="mInvoice")(key:="mID").ToString))

			Dim ReceiptStatusID As Integer = CInt(JObject(propertyName:="mReceipt")(key:="mStatusID").ToString)
			Dim InvoiceStatusID As Integer = CInt(JObject(propertyName:="mInvoice")(key:="mStatusID").ToString)
			Dim TransTypeID As Integer = CInt(JObject(propertyName:="mReceipt")(key:="mTransTypeID").ToString)

			Dim ItemArray As JArray = CType(JObject(propertyName:="mReceiptCumInvoiceItems"), JArray)
			Dim ChargeArray As JArray = CType(JObject(propertyName:="mInvoice")(key:="mInvoiceCharges"), JArray)
			Dim AttachmentArray As JArray = CType(JObject(propertyName:="mFileAttachments"), JArray)

			SetReceiptCumInvoice(JObject:=JObject,
								 ReceiptCumInvoice:=mReceiptCumInvoice)

			For i As Integer = 0 To ItemArray.Count - 1

				Dim mReceiptCumInvoiceItem As ReceiptCumInvoiceItem
				Dim mID As New Guid(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mID").ToString)
				Dim mIsNew As Boolean = CBool(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mIsNew"))
				Dim mIsDeleted As Boolean = CBool(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mIsDirty"))

				If mIsNew Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(ID:=mReceiptCumInvoice.ID)
					mReceiptCumInvoiceItem = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem
				Else
					mReceiptCumInvoiceItem = mReceiptCumInvoice.ReceiptCumInvoiceItems(mID)
				End If

				If mIsDeleted Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentIndex = i
					mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoiceItem)
				End If

				If mIsNew Or mIsDirty Then

					With mReceiptCumInvoiceItem

						SetReceiptCumInvoiceItem(i:=i,
												 JObject:=JObject,
												 ReceiptCumInvoice:=mReceiptCumInvoice,
												 ReceiptCumInvoiceItem:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)

						With mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem

							If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 3 Then
								_SerializedStatus = CBool((JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mOrderItemDetailForReceipt").Item(key:="mSerializedStatus").ToString))
							ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 4 Then
								_SerializedStatus = CBool((JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mIssueItemDetailForReceipt").Item(key:="mSerializedStatus").ToString))
							ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList Or
								   (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID <> 3 And
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID <> 4) Then
								_SerializedStatus = CBool((JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mIsPartFromListisSerialized").ToString))
							End If

							If _SerializedStatus Then

								If .DuplicateSerialNo() Then
									Return New ReturnMessage(Status:="Validation",
															 Message:=$"Serial number { .SerialNo} already exist for the Item { .ItemName}. Cannot add Duplicate.")
								End If

								If .PrimaryCategoryID = 2 AndAlso CBool(AppSettings("CodeNo")) Then

									If mReceiptCumInvoice.TransTypeID = 7 Then

										If .DuplicateCodeNo(TypeToRead:=1) Then
											Return New ReturnMessage(Status:="Validation",
																	 Message:=$"Code No. entered for Item { .ItemName} ({ .SerialNo}) already exist. Please enter another Code No.")
										End If

									Else

										If .DuplicateCodeNo(TypeToRead:=2) OrElse .DuplicateCodeNo(TypeToRead:=3) OrElse .DuplicateCodeNo(TypeToRead:=4) Then '2 Duplication checking with CodeNo,ItemID,Serial No.
											Return New ReturnMessage(Status:="Validation",
																	 Message:=$"Code No. entered for Item { .ItemName} ( { .SerialNo} ) already exist. Please enter another Code No.")

										End If

									End If

								End If

							End If

						End With

						'---------------------------------ReceiptItemKitItems-------------------------------------------------------------------
						Dim ReceiptItemKitItemsArray As JArray = CType(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mReceiptItemKitItems"), JArray)

						Dim mReceiptItemKitItem As ReceiptItemKitItem

						For l As Integer = 0 To ReceiptItemKitItemsArray.Count - 1

							If CBool(ReceiptItemKitItemsArray(l)("mIsNew")) = True Then

								mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.Add(ReceiptItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																  ItemID:=New Guid(ReceiptItemKitItemsArray(l)("mItemID").ToString),
																  SerialNo:=ReceiptItemKitItemsArray(l)("mSerialNo"))

								With mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem

									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.ItemIDFromKitItem = New Guid(ReceiptItemKitItemsArray(l)("mItemIDFromKitItem").ToString)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.SerialNoForItemIDOfKitItem = ReceiptItemKitItemsArray(l)("mSerialNoForItemIDOfKitItem")
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.KitItemID = New Guid(ReceiptItemKitItemsArray(l)("mKitItemID").ToString)
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.Remark = ReceiptItemKitItemsArray(l)("mRemark").ToString
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.ItemName = ReceiptItemKitItemsArray(l)("mItemName").ToString
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.ItemDescription = ReceiptItemKitItemsArray(l)("mItemDescription").ToString
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.CurrentItem.KitItemQty = CDec(ReceiptItemKitItemsArray(l)("mKitItemQty"))

								End With

							Else

								mReceiptItemKitItem = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems(New Guid(ReceiptItemKitItemsArray(l)("mID").ToString))

								If CBool(ReceiptItemKitItemsArray(l)("mIsDirty")) = True Then

									With mReceiptItemKitItem

										.ReceiptItemID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID
										.ItemID = New Guid(ReceiptItemKitItemsArray(l)("mItemID").ToString)
										.SerialNo = ReceiptItemKitItemsArray(l)("mSerialNo")
										.ItemIDFromKitItem = New Guid(ReceiptItemKitItemsArray(l)("mItemIDFromKitItem").ToString)
										.SerialNoForItemIDOfKitItem = ReceiptItemKitItemsArray(l)("mSerialNoForItemIDOfKitItem")
										.KitItemID = New Guid(ReceiptItemKitItemsArray(l)("mKitItemID").ToString)
										.Remark = ReceiptItemKitItemsArray(l)("mRemark").ToString
										.ItemName = ReceiptItemKitItemsArray(l)("mItemName").ToString
										.ItemDescription = ReceiptItemKitItemsArray(l)("mItemDescription").ToString
										.KitItemQty = CDec(ReceiptItemKitItemsArray(l)("mKitItemQty"))

									End With

								End If

								If mReceiptItemKitItem.IsDeleted Then
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemKitItems.Remove(mReceiptItemKitItem)
								End If

							End If

						Next
						'---------------------------------End OF ReceiptItemKitItems-------------------------------------------------------------------

						'---------------------------------ReceiptItemPeriods-------------------------------------------------------------------
						Dim ReceiptItemPeriodsArray As JArray = CType(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mReceiptItemPeriods"), JArray)

						Dim mReceiptItemPeriod As ReceiptItemPeriod

						For m As Integer = 0 To ReceiptItemPeriodsArray.Count - 1

							If CBool(ReceiptItemPeriodsArray(m)("mIsNew")) = True Then

								mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.Add(ReceiptItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																								 TransType:=mReceiptCumInvoice.Receipt.TransTypeID,
																								 PeriodID:=CInt(ReceiptItemPeriodsArray(m)("mPeriodID").ToString)
																								 )
								With mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem

									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.TSNValue = ReceiptItemPeriodsArray(m)("mTSNValue")("mValue").ToString
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.TSOValue = ReceiptItemPeriodsArray(m)("mTSOValue")("mValue").ToString
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.TSIValue = ReceiptItemPeriodsArray(m)("mTSIValue")("mValue").ToString
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.CSNValue = ReceiptItemPeriodsArray(m)("mCSNValue")("mValue").ToString
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.CSOValue = ReceiptItemPeriodsArray(m)("mCSOValue")("mValue").ToString
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.CurrentItem.CSIValue = ReceiptItemPeriodsArray(m)("mCSIValue")("mValue").ToString

								End With

							Else

								mReceiptItemPeriod = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods(New Guid(ReceiptItemPeriodsArray(m)("mID").ToString))

								If CBool(ReceiptItemPeriodsArray(m)("mIsDirty")) = True Then

									With mReceiptItemPeriod

										.ReceiptItemID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID
										.PeriodID = CInt(ReceiptItemPeriodsArray(m)("mPeriodID").ToString)
										.TSNValue = ReceiptItemPeriodsArray(m)("mTSNValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mTSNValue").ToString
										.TSOValue = ReceiptItemPeriodsArray(m)("mTSOValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mTSOValue").ToString
										.TSIValue = ReceiptItemPeriodsArray(m)("mTSIValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mTSIValue").ToString
										.CSNValue = ReceiptItemPeriodsArray(m)("mCSNValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mCSNValue").ToString
										.CSOValue = ReceiptItemPeriodsArray(m)("mCSOValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mCSOValue").ToString
										.CSIValue = ReceiptItemPeriodsArray(m)("mCSIValue")("mValue").ToString 'ReceiptItemPeriodsArray(m)("mCSIValue").ToString

									End With

								End If

								If mReceiptItemPeriod.IsDeleted Then
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemPeriods.Remove(mReceiptItemPeriod)
								End If

							End If

						Next
						'---------------------------------End of ReceiptItemPeriods-------------------------------------------------------------------

						'---------------------------------ReceiptItemServiceInspections-------------------------------------------------------------------
						Dim mReceiptItemServiceInspection As ReceiptItemServiceInspection
						Dim ReceiptItemServiceInspectionsArray As JArray = CType(JObject(propertyName:="mReceiptCumInvoiceItems").Item(i).Item(key:="mReceiptItem").Item(key:="mReceiptItemServiceInspections"), JArray)

						For n As Integer = 0 To ReceiptItemServiceInspectionsArray.Count - 1

							If CBool(ReceiptItemServiceInspectionsArray(n)("mIsNew")) = True Then

								mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Add(ReceiptItemID:=mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ID,
																																	ItemServiceInspectionsID:=New Guid(ReceiptItemServiceInspectionsArray(n)("mItemServiceInspectionsID").ToString),
																																	ItemServiceInspectionDescription:=ReceiptItemServiceInspectionsArray(n)("mItemServiceInspectionDescription"),
																																	ItemServiceInspectionFrequency:=CInt(ReceiptItemServiceInspectionsArray(n)("mItemServiceInspectionFrequency").ToString),
																																	ItemServiceInspectionFrequencyPeriod:=CInt(ReceiptItemServiceInspectionsArray(n)("mItemServiceInspectionFrequencyPeriod").ToString),
																																	ItemID:=ReceiptItemServiceInspectionsArray(n)("mItemID").ToString)

								mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.CurrentItem.ServiedInspectedCheckDoneOnDate = CDate(ReceiptItemServiceInspectionsArray(n)("mServiedInspectedCheckDoneOnDate").First.First).ToString(format:=DateFormat)

							Else

								mReceiptItemServiceInspection = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections(New Guid(ReceiptItemServiceInspectionsArray(n)("mID").ToString))

								If CBool(ReceiptItemServiceInspectionsArray(n)("mIsDirty")) = True Then

									With mReceiptItemServiceInspection
										.ServiedInspectedCheckDoneOnDate = CDate(ReceiptItemServiceInspectionsArray(n)("mServiedInspectedCheckDoneOnDate").First.First).ToString(DateFormat)
									End With

								End If

								If mReceiptItemServiceInspection.IsDeleted Then
									mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReceiptItem.ReceiptItemServiceInspections.Remove(mReceiptItemServiceInspection)
								End If

							End If

						Next
						'---------------------------------End OF ReceiptItemServiceInspections-------------------------------------------------------------------

					End With

				End If

			Next

			For k As Integer = 0 To ChargeArray.Count - 1

				Dim mID As New Guid(ChargeArray(k)("mID").ToString)
				Dim mIsNew As Boolean = CBool(ChargeArray(k)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(ChargeArray(k)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(ChargeArray(k)("mIsDirty"))
				Dim mInvoiceCharge As InvoiceCharge

				If mIsNew Then
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.Add(mReceiptCumInvoice.ID)
					mInvoiceCharge = mReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem
				Else
					mInvoiceCharge = mReceiptCumInvoice.ReceiptCumInvoiceCharges(mID)
				End If

				If mReceiptCumInvoice.IsRoundOff = False Then

					If New Guid(ChargeArray(k)("mChargeID").ToString).Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or
					   New Guid(ChargeArray(k)("mChargeID").ToString).Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then

						mIsDeleted = False  'We have set these two variables false as in PayLoad for these two charges it these two are true.
						mIsDirty = False

						mReceiptCumInvoice.ReceiptCumInvoiceCharges.Remove(mInvoiceCharge)

					End If

				End If


				If mIsDeleted Then
					mReceiptCumInvoice.ReceiptCumInvoiceCharges.Remove(mInvoiceCharge)
				End If

				If mIsNew Or mIsDirty Then

					With mInvoiceCharge

						mInvoiceCharge.SrNo = CInt(ChargeArray(k)("mSrNo"))
						mInvoiceCharge.InvoiceID = New Guid(ChargeArray(k)("mInvoiceID").ToString)
						mInvoiceCharge.ChargeID = New Guid((ChargeArray(k)("mChargeID").ToString))
						mInvoiceCharge.StatusBasic = CBool(ChargeArray(k)("mStatusBasic"))
						mInvoiceCharge.Percentage = CDec(ChargeArray(k)("mPercentage"))
						mInvoiceCharge.CChargeAmount = CDec(ChargeArray(k)("mCChargeAmount"))
						mInvoiceCharge.Currency = ChargeArray(k)("mCurrency")
						mInvoiceCharge.BasicAmount = CDec(ChargeArray(k)("mBasicAmount"))
						mInvoiceCharge.TotalAmount = CDec(ChargeArray(k)("mTotalAmount"))
						mInvoiceCharge.ConversionFactor = CDec(ChargeArray(k)("mConversionFactor"))

					End With

				End If

			Next

			Dim result = _AttachmentHelper.SaveAttachments(AttachmentArray:=AttachmentArray,
														   ModuleObject:=mReceiptCumInvoice,
														   ModuleName:="RCI")

			Dim returnMessage As String = result.Item2

			If Not String.IsNullOrEmpty(returnMessage) Then
				Return New ReturnMessage(Status:="Validation",
										 Message:=$"{returnMessage}")
			End If

			mReceiptCumInvoice = CType(result.Item1, ReceiptCumInvoice)

			mReceiptCumInvoice.Invoice.CalculateTotal()

			If mReceiptCumInvoice.IsRoundOff Then
				mReceiptCumInvoice.Invoice.RoundCGrandTotal()
			End If

			If mReceiptCumInvoice.IsValid Then

				HttpContext.Current.Session("ReceiptCumInvoice") = mReceiptCumInvoice

				If mReceiptCumInvoice.TransTypeID = Trans.ReceiptcumInvoiceAgainstPuchaseOrder AndAlso
				   (_RCIHelper.CheckIfReceiptQuantityExceedsOrderQuantity(ReceiptCumInvoice:=mReceiptCumInvoice)) Then

					Return New ReturnMessage(Status:="Validation",
											 Message:=$"{_ReceiptQuantityExceedingOrderQuantityMessage}")
				End If

				mReceiptCumInvoice.Save()

				If ReceiptStatusID = 2 AndAlso InvoiceStatusID = 2 Then

					Action = "Authorized"
					complyMessage = _RCIHelper.CheckForCalibratedAndEquipmentMaintenanceItems(StatusID:=ReceiptStatusID,
																							  TransTypeID:=TransTypeID,
																							  ReceiptCumInvoiceItems:=ItemArray)

					Return New ReturnMessage(Status:="Validation",
											 Message:=$"{complyMessage.Message}")

				End If

				_ModuleHelper.SendEmailToBytzSoft(TransTypeID:=mReceiptCumInvoice.TransTypeID,
												  Username:=User.Identity.Name,
												  ModuleFrom:="ReceiptCumInvoice",
												  Action:=Action,
												  ClientCode:=AppSettings("ClientCode"),
												  TransactionNo:=mReceiptCumInvoice.ReceiptNo,
												  TransactionDate:=mReceiptCumInvoice.RecCumInvDateFormatted)

			Else
				Return New ReturnMessage(Status:="Validation",
										 Message:=$"{_BrokenRulesHelper.FetchBrokenRules(CommonObject:=mReceiptCumInvoice, ModuleName:="RCI")}")
			End If

			Return New ReturnMessage(Status:="Success",
									 Message:=$"RCI saved Successfully!")

		Catch ex As SqlException
			Return New ReturnMessage(Status:="SqlException",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="RCI", ex:=ex)}")
		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="RCI", ex:=ex, UseAsException:=True)}")
		End Try

	End Function

#End Region

#Region " Delete RCI "

	<HttpDelete>
	Public Function DeleteRCI(ReceiptID As Guid,
							  InvoiceID As Guid) As IHttpActionResult

		Try

			Dim mReceiptCumInvoice As ReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID:=ReceiptID,
																								 InvoiceID:=InvoiceID)

			ReceiptCumInvoice.DeleteReceiptInvoice(ReceiptID:=ReceiptID,
												   InvoiceID:=InvoiceID)
			mReceiptCumInvoice.Save()

			_ModuleHelper.SendEmailToBytzSoft(TransTypeID:=mReceiptCumInvoice.TransTypeID,
											  Username:=User.Identity.Name,
											  ModuleFrom:="ReceiptCumInvoice",
											  Action:="Delete",
											  ClientCode:=AppSettings("ClientCode"),
											  TransactionNo:=mReceiptCumInvoice.ReceiptNo,
											  TransactionDate:=mReceiptCumInvoice.RecCumInvDateFormatted)

			Return Ok(New ReturnMessage("Success", "Receipt deleted successfully!"))

		Catch ex As SqlException
			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   $"{_SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="RCI", SqlException:=ex)}"))
		End Try

	End Function

#End Region

#Region " Report "

	Public Function GetDetailReport(ReceiptID As Guid,
									InvoiceID As Guid) As IHttpActionResult

		Try

			If ReceiptID = Guid.Empty OrElse InvoiceID = Guid.Empty Then

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:="ReceiptID & InvoiceID are Required."))

			End If

			Dim Result = _ReportHelper.GetReceiptCumInvoiceDetailedReport(ReceiptID:=ReceiptID,
																		  InvoiceID:=InvoiceID,
																		  RequestFromAPI:=True)
			If $"{Result.Item1}" = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Report displayed Successfully!!",
												   ReportData:=Result.Item5))

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

	Public Function GetTagReport(ReceiptID As Guid) As IHttpActionResult

		Try

			Return Ok(content:=_ReportHelper.GetReceiptCumInvoiceTagReport(ReceiptID:=ReceiptID))

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Exception",
												   Message:=ex.GetBaseException.ToString()))
		End Try

	End Function

#End Region

#Region " Email "

	<HttpPost>
	<Route("api/ReceiptCumInvoice/SendEmail")>
	Public Function SendEmail(<FromBody()> requestBody As EmailRequest) As IHttpActionResult

		Try

			If String.IsNullOrWhiteSpace(requestBody.ReceiptID) Then
				Return BadRequest("Receipt ID is required.")
			End If

			If String.IsNullOrWhiteSpace(requestBody.InvoiceID) Then
				Return BadRequest("Invoice ID is required.")
			End If

			If String.IsNullOrWhiteSpace(requestBody.ToMailID) Then
				Return BadRequest("To Email Address is required.")
			End If

			Dim ReceiptID As String = requestBody.ReceiptID
			Dim InvoiceID As String = requestBody.InvoiceID
			Dim TransTypeID As Integer = requestBody.TransTypeID
			Dim Remark As String = IIf(requestBody.Remark IsNot Nothing, requestBody.Remark, "")
			Dim ToMailID As String = IIf(requestBody.ToMailID IsNot Nothing, requestBody.ToMailID, "")
			Dim CCMailID As String = IIf(requestBody.CCMailID IsNot Nothing, requestBody.CCMailID, "")
			Dim BCCMailID As String = IIf(requestBody.BCCMailID IsNot Nothing, requestBody.BCCMailID, "")
			Dim AttachmentName As String = IIf(requestBody.AttachmentName IsNot Nothing, requestBody.AttachmentName, "")
			Dim ReportGeneratedBy As String = IIf(requestBody.ReportGeneratedBy IsNot Nothing, requestBody.ReportGeneratedBy, "")

			Dim response As ReturnMessage = _EmailHelper.SendEmail(Remark:=Remark,
																   ModuleName:="RCI",
																   ToMailID:=ToMailID,
																   CCMailID:=CCMailID,
																   BCCMailID:=BCCMailID,
																   InvoiceID:=InvoiceID,
																   ReceiptID:=ReceiptID,
																   Text:=AttachmentName,
																   TransTypeID:=TransTypeID,
																   AttachmentName:=AttachmentName,
																   ReportGeneratedBy:=ReportGeneratedBy)

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

#Region " Method(s) "

	Public Sub SetReceiptCumInvoice(JObject As JObject,
									ReceiptCumInvoice As ReceiptCumInvoice)

		Try

			With ReceiptCumInvoice

				DateFormat = JObject("mReceipt")("mRecdDate")("mFormat")

				ReceiptCumInvoice.RecCumInvDate = CDate(JObject("mReceipt")("mRecdDate").First.First).ToString(format:=DateFormat)
				ReceiptCumInvoice.RecText = JObject("mReceipt")("mText").ToString()
				ReceiptCumInvoice.RecNo = JObject("mReceipt")("mNo").ToString()
				ReceiptCumInvoice.IntReceiptNo = JObject("mReceipt")("mIntReceiptNo").ToString()
				ReceiptCumInvoice.FromTypeID = CInt(JObject("mReceipt")("mFromTypeID").ToString())
				ReceiptCumInvoice.VendorID = New Guid(JObject("mReceipt")("mVendorID").ToString())
				ReceiptCumInvoice.VendorName = JObject("mReceipt")("mVendorName")
				ReceiptCumInvoice.AircraftID = New Guid(JObject("mReceipt")("mMachineID").ToString())
				ReceiptCumInvoice.AircraftName = JObject("mReceipt")("mAircraftName")
				ReceiptCumInvoice.DCNO = JObject("mReceipt")("mDCNO")
				ReceiptCumInvoice.DCDate = CDate(JObject("mReceipt")("mDCDate").First.First).ToString(format:=DateFormat)
				ReceiptCumInvoice.UserName = JObject("mReceipt")("mUserName")
				ReceiptCumInvoice.AuthorizedBy = JObject("mReceipt")("mAuthorizedBy")
				ReceiptCumInvoice.StatusID = CInt(JObject("mReceipt")("mStatusID"))
				ReceiptCumInvoice.EnableNewButton = CBool(JObject("mReceipt")("mEnableNewButton"))
				ReceiptCumInvoice.TransTypeID = CInt(JObject("mReceipt")("mTransTypeID"))
				ReceiptCumInvoice.StoreID = New Guid(JObject("mReceipt")("mStoreID").ToString())
				ReceiptCumInvoice.StoreName = JObject("mReceipt")("mStoreName")
				ReceiptCumInvoice.IssueID = New Guid(JObject("mReceipt")("mIssueID").ToString())
				ReceiptCumInvoice.WorkShopID = New Guid(JObject("mReceipt")("mWorkShopID").ToString())
				ReceiptCumInvoice.WorkShopName = JObject("mReceipt")("mWorkShopName")
				ReceiptCumInvoice.RegNo = JObject("mReceipt")("mRegNo")
				ReceiptCumInvoice.AWBNo = JObject("mReceipt")("mAWBNo")
				ReceiptCumInvoice.ReturnInDays = Val(JObject("mReceipt")("mReturnInDays"))
				ReceiptCumInvoice.IsSync = CInt(JObject("mReceipt")("mIsSync"))
				ReceiptCumInvoice.WOID = New Guid(JObject("mReceipt")("mWOID").ToString())
				ReceiptCumInvoice.WONumber = JObject("mReceipt")("mWONumber")
				ReceiptCumInvoice.BarcodeNo = JObject("mReceipt")("mBarcodeNo")
				ReceiptCumInvoice.IsReturnFromOHRepair = CBool(JObject("mReceipt")("mIsReturnFromOHRepair"))
				ReceiptCumInvoice.IsAttachmentAdded = CBool(JObject("mReceipt")("mIsAttachmentAdded"))
				ReceiptCumInvoice.ToolsReceivedByEmployeeID = New Guid(JObject("mReceipt")("mToolsReceivedByEmployeeID").ToString())
				ReceiptCumInvoice.ToolsReceivedByEmployeeName = JObject("mReceipt")("mToolsReceivedByEmployeeName")
				ReceiptCumInvoice.ToolsSubmittedByEmployeeID = New Guid(JObject("mReceipt")("mToolsSubmittedByEmployeeID").ToString())
				ReceiptCumInvoice.ToolsSubmittedByEmployeeName = JObject("mReceipt")("mToolsSubmittedByEmployeeName")
				ReceiptCumInvoice.OrderID = New Guid(JObject("mReceipt")("mOrderID").ToString())
				ReceiptCumInvoice.IsForAttachmentAfterAuthorized = CBool(JObject("mReceipt")("mIsForAttachmentAfterAuthorized"))
				ReceiptCumInvoice.ToolsCheckInAgainstID = CInt(JObject("mReceipt")("mToolsCheckInAgainstID"))
				ReceiptCumInvoice.InvText = JObject("mInvoice")("mText")
				ReceiptCumInvoice.InvNo = JObject("mInvoice")("mNo").ToString()
				ReceiptCumInvoice.VendorInvoiceNo = JObject("mInvoice")("mVendorInvoiceNo")
				ReceiptCumInvoice.VendorInvoiceDate = CDate(JObject("mInvoice")("mVendorInvoiceDate").First.First).ToString(format:=DateFormat)
				ReceiptCumInvoice.CurrencyID = New Guid(JObject("mInvoice")("mCurrencyID").ToString())
				ReceiptCumInvoice.ConversionFactor = CDec(JObject("mInvoice")("mConversionFactor"))
				ReceiptCumInvoice.Remark = JObject("mInvoice")("mRemark")
				ReceiptCumInvoice.IsRoundOff = CBool(JObject("mInvoice")("mIsRoundOff"))

			End With

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub SetReceiptCumInvoiceItem(i As Integer,
										JObject As JObject,
										ReceiptCumInvoice As ReceiptCumInvoice,
										ReceiptCumInvoiceItem As ReceiptCumInvoiceItem)

		Try

			ReceiptCumInvoiceItem.ItemID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mItemID").ToString)
			ReceiptCumInvoiceItem.FromItemTypeID = CInt(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mFromItemTypeID"))
			ReceiptCumInvoiceItem.OrderItemID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mOrderItemID").ToString)
			ReceiptCumInvoiceItem.IssueItemID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIssueItemID").ToString)
			ReceiptCumInvoiceItem.ReleaseNoteNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReleaseNoteNo")
			ReceiptCumInvoiceItem.ReleaseNoteDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReleaseNoteDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.Qty = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mQty"))
			ReceiptCumInvoiceItem.SerialNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mSerialNo")
			ReceiptCumInvoiceItem.StartDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mStartDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.ExpiryDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mExpiryDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.StoreID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mStoreID").ToString)
			ReceiptCumInvoiceItem.Location = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mLocation")
			ReceiptCumInvoiceItem.Remark = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRemark")
			ReceiptCumInvoiceItem.Note = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mNote")
			ReceiptCumInvoiceItem.StockBalanceQty = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mStockBalanceQty"))
			ReceiptCumInvoiceItem.StoreName = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mStoreName")
			ReceiptCumInvoiceItem.FromPartList = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mFromPartList"))
			ReceiptCumInvoiceItem.Part = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mPart")
			ReceiptCumInvoiceItem.PartDescription = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mPartDescription")
			ReceiptCumInvoiceItem.VisibleStar = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mVisibleStar"))
			ReceiptCumInvoiceItem.Returnable = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReturnable"))
			ReceiptCumInvoiceItem.LoanIssueItemID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mLoanIssueItemID").ToString)
			ReceiptCumInvoiceItem.ItemTypeID = CInt(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mItemTypeID"))
			ReceiptCumInvoiceItem.IsWarranty = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsWarranty"))
			ReceiptCumInvoiceItem.WarrantyInDays = CInt(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mWarrantyInDays"))
			ReceiptCumInvoiceItem.WarrantyStartDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mWarrantyStartDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.WarrantyExpiryDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mWarrantyExpiryDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.AlternateItemID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mAlternateItemID").ToString)
			ReceiptCumInvoiceItem.ReceiptItem.AlternateItemName = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mAlternateItemName").ToString
			ReceiptCumInvoiceItem.ReceiptItem.AlternateItemDescription = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mAlternateItemDescription").ToString
			ReceiptCumInvoiceItem.CureQtrs = Val(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCureQtrs"))
			ReceiptCumInvoiceItem.CureYear = Val(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCureYear"))
			ReceiptCumInvoiceItem.ExpQtrs = Val(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mExpQtrs"))
			ReceiptCumInvoiceItem.ExpYear = Val(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mExpYear"))
			ReceiptCumInvoiceItem.BatchNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mBatchNo")
			ReceiptCumInvoiceItem.OriginalReceiptTextNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mOriginalReceiptTextNo")
			ReceiptCumInvoiceItem.RequestedBy = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRequestedBy")
			ReceiptCumInvoiceItem.CalibrationDoneOnDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCalibrationDoneOnDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.DisplayUnitID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mDisplayUnitID").ToString)
			ReceiptCumInvoiceItem.DisplayQty = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mDisplayQty"))
			ReceiptCumInvoiceItem.BaseUnitID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mBaseUnitID").ToString)
			ReceiptCumInvoiceItem.DisplayUnitName = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mDisplayUnitName")
			ReceiptCumInvoiceItem.WOJobCompID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mWOJobCompID").ToString)
			ReceiptCumInvoiceItem.BarcodeNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mBarcodeNo")
			ReceiptCumInvoiceItem.IsExpiryNA = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsExpiryNA"))
			ReceiptCumInvoiceItem.IsExpiryUnlimited = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsExpiryUnlimited"))
			ReceiptCumInvoiceItem.IsConsiderAsAsset = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsConsiderAsAsset"))
			ReceiptCumInvoiceItem.ReceiptItem.RemovedAsReturnableFromAircraft = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsRemovedAsReturnableFromAircraft"))
			ReceiptCumInvoiceItem.AircraftRemovedQty = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mAircraftRemovedQty"))
			ReceiptCumInvoiceItem.ReceiptItem.IssueCount = CInt(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIssueCount"))
			ReceiptCumInvoiceItem.ReceiptItem.PrimaryCategoryID = CInt(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mPrimaryCategoryID"))
			ReceiptCumInvoiceItem.ReceiptItem.IsAttachmentAdded = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsAttachmentAdded"))
			ReceiptCumInvoiceItem.ReceiptItem.IsTransitDamage = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsTransitDamage"))
			ReceiptCumInvoiceItem.ReceiptItem.CodeNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCodeNo")
			ReceiptCumInvoiceItem.CompStatusID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCompStatusID").ToString)
			ReceiptCumInvoiceItem.ExcessQty = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mExcessQty"))
			ReceiptCumInvoiceItem.ShortQty = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mShortQty"))
			ReceiptCumInvoiceItem.RejectedQty = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRejectedQty"))
			ReceiptCumInvoiceItem.ItemTagID = CInt(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mItemTagID"))
			ReceiptCumInvoiceItem.ItemTagName = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mItemTagName").ToString
			ReceiptCumInvoiceItem.ReceiptItem.IsAirworthiness = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsAirworthiness"))
			ReceiptCumInvoiceItem.ConditionCheckDoneOnDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mConditionCheckDoneOnDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.ReceiptItem.WarrantyApplicableStatus = CInt(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mWarrantyApplicableStatus"))
			ReceiptCumInvoiceItem.ReceiptItem.IsWarrantyApplicableCheckedInOrderItem = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIsWarrantyApplicableCheckedInOrderItem"))
			ReceiptCumInvoiceItem.PreviousWorkScope = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mPreviousWorkScope")
			ReceiptCumInvoiceItem.FaultFound = CInt(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mFaultFound"))
			ReceiptCumInvoiceItem.ReqEmployeeID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReqEmployeeID").ToString)
			ReceiptCumInvoiceItem.ReqEmployeeName = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReqEmployeeName")
			ReceiptCumInvoiceItem.ReqEmployeeEmailIDs = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReqEmployeeEmailIDs").ToString
			ReceiptCumInvoiceItem.ReceiptItem.ReqNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReqNo").ToString
			ReceiptCumInvoiceItem.ReceiptItem.ReqQty = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReqQty").ToString)
			ReceiptCumInvoiceItem.ReceiptItem.ReqDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mReqDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.ReceiptItem.OrderCurrencyID = New Guid(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mOrderCurrencyID").ToString)
			ReceiptCumInvoiceItem.ManufacturingDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mManufacturingDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.ReceiptItem.PartCategory = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mPartCategory").ToString
			ReceiptCumInvoiceItem.CompID = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCompID")
			ReceiptCumInvoiceItem.HazmatID = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mHazmatID")
			ReceiptCumInvoiceItem.CertificateNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCertificateNo")
			ReceiptCumInvoiceItem.RevisionNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRevisionNo")
			ReceiptCumInvoiceItem.RevisionDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRevisionDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.CertifyingRemarks = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCertifyingRemarks")
			ReceiptCumInvoiceItem.WorkOrderRONo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mWorkOrderRONo")
			ReceiptCumInvoiceItem.WorkCardNoRepVendor = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mWorkCardNoRepVendor")
			ReceiptCumInvoiceItem.CertificateType = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCertificateType")
			ReceiptCumInvoiceItem.ApprovalNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mApprovalNo")
			ReceiptCumInvoiceItem.Warehouse = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mWarehouse")
			ReceiptCumInvoiceItem.ManfLot = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mManfLot")
			ReceiptCumInvoiceItem.InspectedDate = CDate(JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mInspectedDate").First.First).ToString(format:=DateFormat)
			ReceiptCumInvoiceItem.InspectedBy = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mInspectedBy")
			ReceiptCumInvoiceItem.LastRemovalPosition = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mLastRemovalPosition")
			ReceiptCumInvoiceItem.RemovalReason = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRemovalReason")
			ReceiptCumInvoiceItem.NHAPartNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mNHAPartNo")
			ReceiptCumInvoiceItem.NHASerialNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mNHASerialNo")
			ReceiptCumInvoiceItem.PackageWONo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mPackageWONo")
			ReceiptCumInvoiceItem.CR = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCR")
			ReceiptCumInvoiceItem.StationWC = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mStationWC")
			ReceiptCumInvoiceItem.RemovalType = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRemovalType")
			ReceiptCumInvoiceItem.RemovedBy = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRemovedBy")
			ReceiptCumInvoiceItem.InstallPart = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mInstallPart")
			ReceiptCumInvoiceItem.InstallSerial = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mInstallSerial")
			ReceiptCumInvoiceItem.InstallBy = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mInstallBy")
			ReceiptCumInvoiceItem.DiscrepancyNo = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mDiscrepancyNo")
			ReceiptCumInvoiceItem.RepeatDiscrepancy = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mRepeatDiscrepancy")
			ReceiptCumInvoiceItem.Incident = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mIncident")
			ReceiptCumInvoiceItem.CausedDelay = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mCausedDelay")
			ReceiptCumInvoiceItem.DiscrepancyDescription = JObject("mReceiptCumInvoiceItems").Item(i).Item("mReceiptItem").Item("mDiscrepancyDescription")
			ReceiptCumInvoice.Invoice.InvoiceItems.CurrentItem.TransTypeID = CInt(JObject("mReceipt")("mTransTypeID"))
			ReceiptCumInvoiceItem.ConversionFactor = CDec(JObject("mInvoice")("mConversionFactor"))
			ReceiptCumInvoiceItem.Currency = JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mCurrency")
			ReceiptCumInvoice.Invoice.InvoiceItems.CurrentItem.IsReturnFromOHRepair = CBool(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mIsReturnFromOHRepair"))
			ReceiptCumInvoiceItem.CRate = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mCRate"))
			ReceiptCumInvoiceItem.COtherCharges = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mCOtherCharges"))
			ReceiptCumInvoiceItem.CAmount = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mCAmount"))
			ReceiptCumInvoiceItem.CEffRate = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mCEffRate"))
			ReceiptCumInvoiceItem.Remark = JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mRemark")
			ReceiptCumInvoiceItem.Note = JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mNote")
			ReceiptCumInvoiceItem.CCommercialRate = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mCCommercialRate"))
			ReceiptCumInvoiceItem.CCommercialRate = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mCCommercialRate"))
			ReceiptCumInvoiceItem.GROCRate = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mGROCRate"))
			ReceiptCumInvoiceItem.DisplayCRate = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mDisplayCRate"))
			ReceiptCumInvoiceItem.DisplayCAmount = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mDisplayCAmount"))
			ReceiptCumInvoiceItem.CCommercialRate = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mCCommercialRate"))
			ReceiptCumInvoiceItem.DisplayCCommercialRate = CDec(JObject("mReceiptCumInvoiceItems").Item(i).Item("mInvoiceItem").Item("mDisplayCCommercialRate"))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Other Method(s) "

	<HttpGet>
	Public Function ComplyCalibratedAndEquipmentMaintenanceItems(ReceiptID As Guid,
																 InvoiceID As Guid) As IHttpActionResult

		Dim returnMessage As String
		Dim CalibratedItemsMessage As StringBuilder
		Dim EquipmentMaintenancePartMessage As StringBuilder
		Try

			Dim ReceiptCumInvoice As ReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID:=ReceiptID,
																								InvoiceID:=InvoiceID)

			CalibratedItemsMessage = _RCIHelper.ComplyCalibratedItems(ReceiptCumInvoiceItems:=ReceiptCumInvoice.ReceiptCumInvoiceItems)
			EquipmentMaintenancePartMessage = _RCIHelper.ComplyConditionCheckItems(ReceiptCumInvoiceItems:=ReceiptCumInvoice.ReceiptCumInvoiceItems)


			If CalibratedItemsMessage.ToString().Trim() = "Item(s) has already been Complied." AndAlso
			   EquipmentMaintenancePartMessage.ToString().Trim() = "Item(s) has already been Complied." Then
				returnMessage = "Item(s) has already been Complied."
			Else
				returnMessage = $"{CalibratedItemsMessage} {Environment.NewLine} {EquipmentMaintenancePartMessage}"
			End If

			Return Ok(New ReturnMessage(Status:="Success", Message:=returnMessage))

		Catch ex As Exception

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Exception",
												   $"{ex.Message}"))

		End Try

	End Function

	<HttpGet>
	Public Function UpdateOrderQuantity() As IHttpActionResult

		Dim OrderItemQuantityUpdated As Boolean
		Try

			Dim ReceiptCumInvoice As ReceiptCumInvoice = CType(HttpContext.Current.Session("ReceiptCumInvoice"), ReceiptCumInvoice)

			If ReceiptCumInvoice IsNot Nothing Then
				OrderItemQuantityUpdated = _RCIHelper.UpdateOrderQuantity(ReceiptCumInvoice:=ReceiptCumInvoice)
				ReceiptCumInvoice.Save()
			End If

			If OrderItemQuantityUpdated Then
				Return Ok(New ReturnMessage(Status:="Success",
												   Message:=$"Order Item quantity updated successfully!{Environment.NewLine}RCI saved Successfully!",
												   TransactionID:=$"{ReceiptCumInvoice.Receipt.ID}",
												   TransactionID1:=$"{ReceiptCumInvoice.Invoice.ID}"))
			Else
				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="",
													   Message:=$"Order Item quantity was not updated.",
													   TransactionID:=$"{ReceiptCumInvoice.Receipt.ID}",
													   TransactionID1:=$"{ReceiptCumInvoice.Invoice.ID}"))
			End If

			HttpContext.Current.Session.Remove("ReceiptCumInvoice")

		Catch ex As Exception

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Exception",
												   $"{ex.Message}"))

		End Try

	End Function

#End Region

#Region " Docket Charges "   ' SANKALP Docket Charges from the RCI Page only Since Invoice is single and set from the RCI page

	<HttpGet>
	Public Function GetNewDocketCharges(ReceiptID As String,
										InvoiceID As String) As OtherCharge

		Dim mOtherCharge As OtherCharge
		Try

			Dim mReceiptCumInvoice As ReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID:=New Guid(ReceiptID),
																								 InvoiceID:=New Guid(InvoiceID))

			Dim mOtherChargeListByInvoiceID As OtherChargeListByInvoiceID =
				OtherChargeListByInvoiceID.GetOtherChargeListByInvoiceID(InvoiceID:=mReceiptCumInvoice.InvoiceID.ToString)

			If mOtherChargeListByInvoiceID.Count = 0 Then  'Then Add new docket    'New

				mOtherCharge = OtherCharge.NewOtherCharge
				mOtherCharge.Date = Today.Date

				mOtherCharge.OtherChargeInvoices.Add(ID:=mOtherCharge.ID)
				mOtherCharge.OtherChargeInvoices.CurrentItem.InvoiceID = mReceiptCumInvoice.InvoiceID

			Else                                            'Then Only add new  charges for docket   'Edit
				mOtherCharge = OtherCharge.GetOtherCharge(mOtherChargeListByInvoiceID.Item(0).ID)
			End If

			Return mOtherCharge

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetNewOtherChargeDetails(OtherChargeID As String) As OtherChargeDetail

		Dim mOtherCharge As OtherCharge
		Try

			mOtherCharge = OtherCharge.NewOtherCharge
			mOtherCharge.OtherChargeDetails.Add(ID:=New Guid(OtherChargeID))

			Return mOtherCharge.OtherChargeDetails.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Private Function CheckOtherChargeDuplicate(OtherCharge As OtherCharge) As String

		Dim alreadyExists = OtherCharge.OtherChargeDetails.Contains(item:=OtherCharge.OtherChargeDetails.CurrentItem)

		If alreadyExists Then Return $"You are trying to add duplicate record. Only unique record is allowed."

		Return Nothing

	End Function

	<HttpGet>
	Public Function CalculateDocketCharges(ReceiptID As String,
										   InvoiceID As String) As Object

		Dim InvoiceDocketCharge As Decimal
		Dim TotalEffectiveAmount As Decimal
		Dim _ReceiptCumInvoice As ReceiptCumInvoice
		Dim TotalDocketCharge As OtherChargeListByInvoiceID
		Try

			_ReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(ReceiptID:=New Guid(ReceiptID),
																		InvoiceID:=New Guid(InvoiceID))

			For i As Integer = 0 To _ReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1

				If (
						_ReceiptCumInvoice.TransTypeID = 10 Or
						_ReceiptCumInvoice.TransTypeID = 48 Or
						_ReceiptCumInvoice.TransTypeID = 54 Or
						(
							_ReceiptCumInvoice.TransTypeID = 67 And
							_ReceiptCumInvoice.IsReturnFromOHRepair
						)
				   ) Then

					TotalEffectiveAmount = TotalEffectiveAmount + (
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).GROCEffRate *
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).Qty
																  ) -
																  (
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).CGSTCAmount +
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).SGSTCAmount +
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).IGSTCAmount
																  )
				Else

					TotalEffectiveAmount = TotalEffectiveAmount + (
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayCEffRate *
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayQty
																  ) -
																  (
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayCGSTCAmount +
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplaySGSTCAmount +
																	_ReceiptCumInvoice.ReceiptCumInvoiceItems(i).DisplayIGSTCAmount
																  )
				End If

			Next

			InvoiceDocketCharge = TotalEffectiveAmount - _ReceiptCumInvoice.CTotalAmount - _ReceiptCumInvoice.CTotalCharges
			TotalDocketCharge = OtherChargeListByInvoiceID.GetOtherChargeListByInvoiceID(InvoiceID:=InvoiceID)

			If TotalDocketCharge.Count = 0 Then

				Return New With {
					.InvoiceDocketCharge = 0D,
					.TotalDocketCharge = 0D
				}

			Else

				Return New With {
					.InvoiceDocketCharge = CDec(Format(InvoiceDocketCharge, "##0.00##")),
					.TotalDocketCharge = CDec(Format(TotalDocketCharge(0).CGrandTotal))
				}

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpPost>
	Public Function SaveDocketCharges(<FromBody()> requestBody As JObject) As IHttpActionResult

		Dim ReturnMessage As ReturnMessage
		Try

			ReturnMessage = SetDocketCharges(requestBody:=requestBody)

			_ResponseWrapper = _StatusWiseReturnMessage.GenerateResponseMessage(returnMessage:=ReturnMessage)
			Return Content(_ResponseWrapper.StatusCode, _ResponseWrapper.ReturnMessage)

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Error",
												   Message:=ex.GetBaseException.ToString))

		End Try

	End Function

	Private Function SetDocketCharges(requestBody As JObject) As ReturnMessage

		Dim SubscriptionMessage As String
		Dim DocketChargesIsNew As Boolean = CBool(requestBody("mIsNew"))

		Dim mOtherCharge As OtherCharge
		Dim _OtherChargeListByInvoiceID As OtherChargeListByInvoiceID

		Dim ID As New Guid(requestBody("mID").ToString)
		Dim DateFormat As String = requestBody(propertyName:="mDate")("mFormat")
		Dim OtherChargeDetails As JArray = CType(requestBody("mOtherChargeDetails"), JArray)
		Dim OtherChargeInvoices As JArray = CType(requestBody("mOtherChargeInvoices"), JArray)
		Dim InvoiceID As New Guid(requestBody("mOtherChargeInvoices").First("mInvoice")("mID").ToString)
		Try

			SubscriptionMessage = _CheckForSubscriptionExpired.
						CheckForSubscriptionExpired(TransactionDate:=CDate(requestBody(propertyName:="mDate").First.First),
													ModuleName:="Docket Charges")

			If SubscriptionMessage <> "Success" Then
				Return New ReturnMessage(Status:="Validation",
										 Message:=SubscriptionMessage)
			End If

			_OtherChargeListByInvoiceID = OtherChargeListByInvoiceID.GetOtherChargeListByInvoiceID(InvoiceID:=InvoiceID.ToString)

			If _OtherChargeListByInvoiceID.Count = 0 Then

				mOtherCharge = OtherCharge.NewOtherCharge
				mOtherCharge.Date = Today.Date
				mOtherCharge.OtherChargeInvoices.Add(ID:=mOtherCharge.ID)
				mOtherCharge.OtherChargeInvoices.CurrentItem.InvoiceID = InvoiceID ' Need to set Invoice Id to it

			Else
				mOtherCharge = OtherCharge.GetOtherCharge(ID:=ID)
			End If

			With mOtherCharge

				.Date = CDate(requestBody(propertyName:="mDate").First.First).ToString(format:=DateFormat)
				.BillEntryNo = requestBody(propertyName:="mBillEntryNo")
				.MasterAirwayBillNo = requestBody(propertyName:="mMasterAirwayBillNo")
				.HouseAirwayBillNo = requestBody(propertyName:="mHouseAirwayBillNo")
				.BillEntryDate = CDate(requestBody(propertyName:="mBillEntryDate").First.First).ToString(format:=DateFormat)
				.MasterAirwayBillDate = CDate(requestBody(propertyName:="mMasterAirwayBillDate").First.First).ToString(format:=DateFormat)
				.HouseAirwayBillDate = CDate(requestBody(propertyName:="mHouseAirwayBillDate").First.First).ToString(format:=DateFormat)
				.Text = requestBody(propertyName:="mText")
				.No = CInt(requestBody(propertyName:="mNo"))

			End With

			Dim ReturnMessage As ReturnMessage = SetOtherChargeDetails(DateFormat:=DateFormat,
																	   mOtherCharge:=mOtherCharge,
																	   DocketChargesIsNew:=DocketChargesIsNew,
																	   OtherChargeDetailsArray:=OtherChargeDetails)

			If ReturnMessage.Result IsNot Nothing And ReturnMessage.Status = "Success" Then
				mOtherCharge = CType(ReturnMessage.Result, OtherCharge)
			Else
				Return New ReturnMessage(Status:="Exception",
										 Message:=ReturnMessage.Message)
			End If

			If mOtherCharge.OtherChargeInvoices.Count = 0 And mOtherCharge.OtherChargeDetails.Count = 0 Then
				Return New ReturnMessage(Status:="Validation",
										 Message:="Other Charge cannot be saved without Invoice & Charge.")
			End If

			If mOtherCharge.IsValid Then

				mOtherCharge.Save()

				Return New ReturnMessage(Status:="Success",
										 Message:="Docket Charges Saved Successfully!")

			Else
				Return New ReturnMessage(Status:="Validation",
										 Message:="Other Charge Object is not valid.")
			End If

		Catch ex As SqlException
			Return New ReturnMessage(Status:="SqlException",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Other Docket Charges", ex:=ex)}")
		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Other Docket Charges", ex:=ex,
																								  UseAsException:=True)}")
		End Try

	End Function

	Private Function SetOtherChargeDetails(DateFormat As String,
										   mOtherCharge As OtherCharge,
										   DocketChargesIsNew As Boolean,
										   OtherChargeDetailsArray As JArray) As ReturnMessage

		Dim OtherChargeDetail As OtherChargeDetail
		Try

			For i As Integer = 0 To OtherChargeDetailsArray.Count - 1

				Dim OtherChargeDetailsID As New Guid(OtherChargeDetailsArray(i)("mID").ToString)

				Dim IsOtherChargeDetailsNew As Boolean = CBool(OtherChargeDetailsArray(i)("mIsNew"))
				Dim IsOtherChargeDetailsDeleted As Boolean = CBool(OtherChargeDetailsArray(i)("mIsDeleted"))
				Dim IsOtherChargeDetailsDirty As Boolean = CBool(OtherChargeDetailsArray(i)("mIsDirty"))

				If IsOtherChargeDetailsNew Then
					mOtherCharge.OtherChargeDetails.Add(mOtherCharge.ID)
					OtherChargeDetail = mOtherCharge.OtherChargeDetails.CurrentItem
				Else
					OtherChargeDetail = mOtherCharge.OtherChargeDetails(ID:=OtherChargeDetailsID)
				End If

				If IsOtherChargeDetailsDeleted Then
					mOtherCharge.OtherChargeDetails.Remove(OtherChargeDetail)
				End If

				If IsOtherChargeDetailsNew Or IsOtherChargeDetailsDirty Then

					With OtherChargeDetail

						.SrNo = CInt(OtherChargeDetailsArray(i)("mSrNo"))
						.ChargeID = New Guid(OtherChargeDetailsArray(i)("mChargeID").ToString)
						.VendorID = New Guid(OtherChargeDetailsArray(i)("mVendorID").ToString)
						.CurrencyID = New Guid(OtherChargeDetailsArray(i)("mCurrencyID").ToString)
						.OtherChargeTypeID = OtherChargeDetailsArray(i)("mOtherChargeTypeID")
						.InvoiceNo = OtherChargeDetailsArray(i)("mInvoiceNo")
						.InvoiceDate = If(Not IsDate(OtherChargeDetailsArray(i)("mInvoiceDate").First.First), DBNull.Value, CDate(OtherChargeDetailsArray(i)("mInvoiceDate").First.First).ToString(DateFormat))
						.ConversionFactor = Val(OtherChargeDetailsArray(i)("mConversionFactor"))
						.CServiceCharges = Val(OtherChargeDetailsArray(i)("mServiceCharges"))
						.CAmount = Val(OtherChargeDetailsArray(i)("mCAmount"))

					End With

				End If

			Next

			Return New ReturnMessage(Status:="Success",
									 Message:="Other Charge Details Set Successfully!",
									 Result:=mOtherCharge)

		Catch ex As SqlException
			Return New ReturnMessage(Status:="SqlException",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Other Docket Charges", ex:=ex)}")
		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Other Docket Charges", ex:=ex,
																								  UseAsException:=True)}")
		End Try

	End Function

	<HttpGet>
	Public Function DisplayDocketChargeReport(DocketChargeID As Guid) As IHttpActionResult

		Dim ReturnMessage As ReturnMessage
		Try

			If DocketChargeID = Guid.Empty OrElse DocketChargeID.ToString Is Nothing Then

				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:="Docket-Charge ID is Required."))

			End If

			ReturnMessage = _ReportHelper.DisplayDocketChargeReport(DocketChargeID:=DocketChargeID)

			_ResponseWrapper = _StatusWiseReturnMessage.GenerateResponseMessage(returnMessage:=ReturnMessage)
			Return Content(_ResponseWrapper.StatusCode, _ResponseWrapper.ReturnMessage)

		Catch ex As Exception
			Return Content(HttpStatusCode.BadRequest, ex.Message)
		End Try

	End Function

#End Region

End Class
