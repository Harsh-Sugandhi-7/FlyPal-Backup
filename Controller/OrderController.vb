Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Web.Http
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq


Public Class OrderController
	Inherits ApiController

#Region " Varriable(s) "

	Private _MessageBox As New MSGBox
	Private _EmailHelper As New EmailHelper
	Private _ReportHelper As New ReportHelper
	Private _ModuleHelper As New ModuleHelper
	Private _ReceiptCumInvoice As ReceiptCumInvoice
	Private _ResponseWrapper As New ResponseWrapper
	Private _AttachmentHelper As New AttachmentHelper
	Private _BrokenRulesHelper As New BrokenRulesHelper
	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _StatusWiseReturnMessage As New StatusWiseReturnMessage
	Private _CheckForSubscriptionExpired As New CheckForSubscriptionExpired

	Private _DateFormat As String = ""
	Private OrderID As String = "{00000000-0000-0000-0000-000000000000}"

#End Region

#Region " GET Method(s) "

	<HttpGet>
	Public Function GetValues(Optional ItemName As String = "",
							  Optional Text As String = "",
							  Optional No As Integer = 0,
							  Optional Amend As String = "",
							  Optional IntOrderNo As String = "",
							  Optional FromDate As String = "1/1/1900",
							  Optional ToDate As String = "1/1/2200",
							  Optional StatusID As Integer = 0,
							  Optional QuotationNo As String = "",
							  Optional VendorName As String = "",
							  Optional TransTypeID As Trans = Trans.PurchaseOrder,
							  Optional PrimaryOrderType As Integer = 1,
							  Optional IsOverhaul As Boolean = False,
							  Optional PriorityID As Integer = 0,
							  Optional AircraftReg As String = "",
							  Optional POTowardsID As Integer = 0,
							  Optional ReqText As String = "",
							  Optional ReqNo As Integer = 0,
							  Optional IsPBHPurchase As Boolean = False,
							  Optional SearchText As String = "") As OrderList

		Try

			Return OrderList.GetOrderList(ItemName:=ItemName,
										  Text:=Text,
										  No:=No,
										  Amend:=Amend,
										  IntOrderNo:=IntOrderNo,
										  FromDate:=FromDate,
										  ToDate:=ToDate,
										  StatusID:=StatusID,
										  QuotationNo:=QuotationNo,
										  VendorName:=VendorName,
										  TransTypeID:=TransTypeID,
										  PrimaryOrderType:=PrimaryOrderType,
										  IsOverhaul:=IsOverhaul,
										  PriorityID:=PriorityID,
										  AircraftReg:=AircraftReg,
										  POTowardsID:=POTowardsID,
										  ReqText:=ReqText,
										  ReqNo:=ReqNo,
										  IsPBHPurchase:=IsPBHPurchase,
										  SearchText:=SearchText)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetValue(Id As Guid) As Order

		Try

			Return Order.GetOrder(Id)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	' GET New Order
	<HttpGet>
	Public Function GetNewOrder(TransTypeID As Integer,
								Optional IsCustomer As Boolean = False) As Order

		Try

			Return Order.NewOrder(ID:=New Guid,
								  TransTypeID:=TransTypeID,
								  IsCustomer:=IsCustomer)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	' GET  New Order Item
	<HttpGet>
	Public Function GetNewOrderItem(OrderID As Guid, TransTypeID As Integer,
									Optional IsCustomer As Boolean = False) As OrderItem

		Dim mOrder As Order = Order.NewOrder(TransTypeID, IsCustomer)

		mOrder.OrderItems.Add(OrderID)

		mOrder.OrderItems.CurrentItem.OrderItemQuotationItems.Add(OrderItemID:=mOrder.OrderItems.CurrentItem.ID,
																  QuotationItemID:=Guid.Empty,
																  Qty:=0D, QuotationNo:="",
																  QuotationDate:="",
																  QuotationID:=Guid.Empty)

		mOrder.OrderItems.CurrentItem.OrderItemSalesOrderItems.Add(OrderItemID:=mOrder.OrderItems.CurrentItem.ID,
																   SalesOrderItemID:=Guid.Empty,
																   Qty:=0D,
																   SalesOrderNo:="")

		mOrder.OrderItems.CurrentItem.RequisitionItemOrderItems.Add(OrderItemID:=mOrder.OrderItems.CurrentItem.ID,
																	RequisitionItemID:=Guid.Empty,
																	Qty:=0D,
																	RequisitionNo:="")

		Return mOrder.OrderItems.CurrentItem

	End Function

	' GET  New Order Term
	<HttpGet>
	Public Function GetNewOrderTerm(OrderID As Guid,
									TransTypeID As Integer,
									Optional IsCustomer As Boolean = False) As OrderTerm

		Dim mOrder As Order = Order.NewOrder(TransTypeID:=TransTypeID,
											 IsCustomer:=IsCustomer)

		mOrder.OrderTerms.Add(OrderID)

		Return mOrder.OrderTerms.CurrentItem

	End Function

	' GET  New Order Charge
	<HttpGet>
	Public Function GetNewOrderCharge(OrderID As Guid,
									  TransTypeID As Integer,
									  Optional IsCustomer As Boolean = False) As OrderCharge

		Dim mOrder As Order = Order.NewOrder(TransTypeID:=TransTypeID,
											 IsCustomer:=IsCustomer)
		mOrder.OrderCharges.Add(OrderID)

		Return mOrder.OrderCharges.CurrentItem

	End Function

	' GET  PO Towards
	<HttpGet>
	Public Function GetPOTowards(Optional AddTopItem As String = "(All)") As POTowards

		Return POTowards.GetPOTowards(AddTopItem:=AddTopItem)

	End Function

	'GET Top Amended OrderNo
	<HttpGet>
	Public Function GetTopAmendedOrderNo(Text As String,
										 No As Integer,
										 Optional Type As Integer = 0) As ShowTopAmendedOrderNoForAPI

		Return ShowTopAmendedOrderNoForAPI.GetTopAmendedOrderNo(Text:=Text,
																No:=No,
																Type:=Type)

	End Function

	' GET  PendingOrderList
	Public Function GetPendingOrderList(Optional ItemName As String = "",
										Optional Text As String = "",
										Optional No As Integer = 0,
										Optional Amend As String = "",
										Optional IntOrderNo As String = "",
										Optional FromDate As String = "1/1/1900",
										Optional ToDate As String = "1/1/2200",
										Optional StatusID As Integer = 0,
										Optional QuotationNo As String = "",
										Optional VendorID As String = "{00000000-0000-0000-0000-000000000000}",
										Optional TransTypeID As Trans = Util.Trans.PurchaseOrder,
										Optional PrimaryOrderType As Integer = 1,
										Optional OrderID As String = "{00000000-0000-0000-0000-000000000000}",
										Optional CurrencyID As String = "{00000000-0000-0000-0000-000000000000}") As OrderList

		Return OrderList.GetPendingOrderList(ItemName:=ItemName,
											 Text:=Text,
											 No:=No,
											 Amend:=Amend,
											 IntOrderNo:=IntOrderNo,
											 FromDate:=FromDate,
											 ToDate:=ToDate,
											 StatusID:=StatusID,
											 QuotationNo:=QuotationNo,
											 VendorID:=VendorID,
											 TransTypeID:=TransTypeID,
											 PrimaryOrderType:=PrimaryOrderType,
											 OrderID:=OrderID,
											 CurrencyID:=CurrencyID)

	End Function

	'GetBillToShipToTypeList
	<HttpGet>
	Public Function GetBillToShipToTypeList(Optional AddTopItem As String = "(All)") As BillToShipToTypeList

		Return BillToShipToTypeList.GetBillToShipToTypeList()

	End Function

	<HttpGet>
	Public Function GetRegInformation() As RegInformation

		Return RegInformation.GetRegInformation()

	End Function

	<HttpGet>
	Public Function GetSumOfReceiptBalanceQtyFromOrderItemTab(OrderID As String) As SumOfReceiptBalanceQtyFromOrderItemTab

		Try

			Return SumOfReceiptBalanceQtyFromOrderItemTab.GetSumOfReceiptBalanceQtyFromOrderItemTab(OrderID:=New Guid(OrderID))

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	<HttpGet>
	Public Function GetQtyDetailsForOrderList(ItemID As Guid,
											  mSearchType As Integer) As QtyDetailsForOrder

		Return QtyDetailsForOrder.GetQtyDetailsForOrderList(ItemID:=ItemID,
															mSearchType:=mSearchType)

	End Function

	<HttpGet>
	Public Function GetRecordOfLastOrder(TransTypeID As Integer,
										 Optional VendorID As String = "{00000000-0000-0000-0000-000000000000}") As RecordOfLastOrder

		Return RecordOfLastOrder.GetRecordOfLastOrder(TransTypeID:=TransTypeID,
													  VendorID:=VendorID)

	End Function

	<HttpGet>
	Public Function GetInvoiceItemListForFinalApprovalList(ItemID As Guid) As InvoiceItemListForFinanceApproval

		Return InvoiceItemListForFinanceApproval.GetInvoiceItemListForFinalApprovalList(ItemID)

	End Function

	<ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
	Public Function GetItemStockStatusList(Optional ItemName As String = "",
										   Optional ToDate As String = "1/1/3300",
										   Optional IsCalibrationOrder As Boolean = False) As ItemStockStatusList

		Return ItemStockStatusList.GetItemStockStatusList(ItemName:=ItemName,
														  ToDate:=ToDate,
														  IsCalibrationOrder:=IsCalibrationOrder)

	End Function

	'New Order from Pending Purchase Quotations ------------------------------
	<HttpGet>
	Public Function GetPendingPurchaseQuotationList(OrderDate As String,
													VendorID As Guid,
													PrevTransID As Guid,
													Optional OrderTransTypeID As Integer = 0) As PendingPurchaseQuotationList 'Order Type 1 outright/ex/ov 2 Rental Lease

		Return PendingPurchaseQuotationList.GetPendingPurchaseQuotationList(OrderDate:=OrderDate,
																			VendorID:=VendorID,
																			PrevTransID:=PrevTransID,
																			OrderTransTypeID:=OrderTransTypeID)

	End Function

	<HttpGet>
	Public Function GetPendingQuotationList(QuotationID As Guid,
											Optional OrderTransTypeID As Integer = 0) As PendingPurchaseQuotationItems

		Return PendingPurchaseQuotationItems.GetPendingQuotationList(QuotationID:=QuotationID,
																	 OrderTransTypeID:=OrderTransTypeID)

	End Function
	'--------------------------------------------------

	'New Order from Pending Sales Order ---------------------------------------------------
	<HttpGet>
	Public Function GetSalesOrderForPurchaseOrderList(OrderDate As String,
													  Optional VendorID As String = "{00000000-0000-0000-0000-000000000000}",
													  Optional PrevTransID As String = "{00000000-0000-0000-0000-000000000000}") As SalesOrderForPurchaseOrderList

		Return SalesOrderForPurchaseOrderList.GetSalesOrderForPurchaseOrderList(OrderDate:=OrderDate,
																				VendorID:=VendorID,
																				PrevTransID:=PrevTransID)

	End Function

	<HttpGet>
	Public Function GetSalesOrderForPurchaseOrder(SalesOrderID As Guid) As SalesOrderItemsForPurchaseOrder

		Return SalesOrderItemsForPurchaseOrder.GetSalesOrderForPurchaseOrder(SalesOrderID:=SalesOrderID)

	End Function
	'--------------------------------------------------

	'New Order from Pending Requisition

	<HttpGet>
	Public Function GetRequisitionItemsForList(ItemID As Guid,
											   PartName As String,
											   ListFor As Integer,
											   TransDate As String,
											   Optional No As Integer = 0,
											   Optional Text As String = "",
											   Optional ReqTypeID As Integer = 0,
											   Optional ClientCode As String = "",
											   Optional TransTypeID As Integer = 0,
											   Optional ToDate As String = "1/1/4400",
											   Optional FromDate As String = "1/1/1900",
											   Optional ExchangeAsRequisitionItems As Boolean = False,
											   Optional WOID As String = "{00000000-0000-0000-0000-000000000000}",
											   Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
											   Optional CustomerID As String = "{00000000-0000-0000-0000-000000000000}",
											   Optional RequisitionID As String = "{00000000-0000-0000-0000-000000000000}") As RequisitionItemsNew

		Try

			Dim RequisitionItemsList As RequisitionItemsNew =
					RequisitionItemsNew.GetRequisitionItemsForList(No:=No,
																   Text:=Text,
																   WOID:=WOID,
																   ToDate:=ToDate,
																   ItemID:=ItemID,
																   ListFor:=ListFor,
																   FromDate:=FromDate,
																   PartName:=PartName,
																   TransDate:=TransDate,
																   ReqTypeID:=ReqTypeID,
																   MachineID:=MachineID,
																   ClientCode:=ClientCode,
																   CustomerID:=CustomerID,
																   TransTypeID:=TransTypeID,
																   RequisitionID:=RequisitionID,
																   ExchangeAsRequisitionItems:=ExchangeAsRequisitionItems)

			Return RequisitionItemsList

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'New Order from Pending Enquiry ---------------------------------------
	<HttpGet>
	Public Function GetPendingEnquiryItemsForOrder(Optional ItemName As String = "",
												   Optional Text As String = "",
												   Optional No As Integer = 0,
												   Optional TransDate As String = "",
												   Optional VendorName As String = "") As PendingEnquiryItemsForOrder

		Return PendingEnquiryItemsForOrder.GetPendingEnquiryItemsForOrder(ItemName,
																		  Text,
																		  No,
																		  TransDate,
																		  VendorName)
	End Function

	<HttpGet>
	Public Function GetQuotationItemsForOrderAgainstEnqItems(EnquiryItemID As Guid,
															 Optional VendorID As String = "{00000000-0000-0000-0000-000000000000}") As QuotationItems

		Return QuotationItems.GetQuotationItemsForOrderAgainstEnqItems(EnquiryItemID:=EnquiryItemID,
																	   VendorID:=VendorID)

	End Function
	'----------------------------------------------------------------

	<HttpGet>
	Public Function GetTotalReceiptQtyAgainstOrderItem(OrderItemID As Guid,
													   ReceiptItemID As String) As IHttpActionResult

		Try

			Dim TotalRecQty As Decimal
			TotalRecQty = Order.GetTotalReceiptQtyAgainstOrderItem(OrderItemID:=OrderItemID,
																   SkipRecItemID:=ReceiptItemID)

			Return Json(New With {.mTotalRecQty = TotalRecQty})

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Function
	'----------------------------------------------------------------

	'Get Module name for MarkLog for Purchase Order module

	<HttpGet>
	Public Function GetCRateOfLastOrderedItem(ItemID As Guid,
											  TransTypeID As Integer) As CRateOfLastOrderedItem

		Try

			Dim CRateOfLastOrderedItem As CRateOfLastOrderedItem =
					 CRateOfLastOrderedItem.GetCRateOfLastOrderedItem(TransTypeID:=TransTypeID,
																	  ItemID:=ItemID.ToString)

			Return CRateOfLastOrderedItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " POST Method(s) "

	<HttpPost>
	Public Function PostValue(<FromBody()> requestBody As JObject) As IHttpActionResult

		Dim returnMessage As ReturnMessage
		Dim IsNew As Boolean = CBool(requestBody("mIsNew"))
		Try

			If IsNew Then
				returnMessage = SetNewOrderValues(requestBody)
			Else
				returnMessage = SetExistingOrderValues(requestBody)
			End If

			_ResponseWrapper = _StatusWiseReturnMessage.GenerateResponseMessage(returnMessage:=returnMessage)
			Return Content(_ResponseWrapper.StatusCode, _ResponseWrapper.ReturnMessage)

		Catch ex As Exception

			Return Content(HttpStatusCode.InternalServerError,
						   New ReturnMessage(Status:="Exception",
												   Message:=$"Exception Occurred. Message: {ex.GetBaseException}"))

		End Try

	End Function

	Private Function SetNewOrderValues(requestBody As JObject) As ReturnMessage

		Try

			Dim returnMessage As String =
				_CheckForSubscriptionExpired.
					CheckForSubscriptionExpired(TransactionDate:=CDate(requestBody(propertyName:="mDate").First.First))

			If returnMessage <> "Success" Then
				Return New ReturnMessage(Status:="Validation",
										 Message:=returnMessage)
			End If

			Dim mOrder As Order = Order.NewOrder(ID:=New Guid(requestBody("mID").ToString),
												 TransTypeID:=CInt(requestBody("mTransTypeID")),
												 IsCustomer:=CBool(requestBody("mIsCustomer")))

			Dim ItemArray As JArray = CType(requestBody("mOrderItems"), JArray)
			Dim TermArray As JArray = CType(requestBody("mOrderTerms"), JArray)
			Dim ChargeArray As JArray = CType(requestBody("mOrderCharges"), JArray)
			Dim AttachmentArray As JArray = CType(requestBody("mFileAttachments"), JArray)

			With mOrder

				_DateFormat = requestBody(propertyName:="mDate")("mFormat")

				.OrderDate = CDate(requestBody(propertyName:="mDate").First.First).ToString(format:=_DateFormat)
				.DeliveryWithinDays = requestBody(propertyName:="mDeliveryWithinDays")
				.IntOrderNo = requestBody(propertyName:="mIntOrderNo")
				.VendorID = New Guid(requestBody(propertyName:="mVendorID").ToString)
				.Attention = requestBody("mAttention")
				.QuotationNo = requestBody("mQuotationNo")
				.QuotationDate = CDate(requestBody(propertyName:="mQuotationDate").First.First).ToString(format:=_DateFormat)
				.CurrencyID = New Guid(requestBody("mCurrencyID").ToString)
				.ConversionFactor = CDec(requestBody("mConversionFactor"))
				.UserName = requestBody(propertyName:="mUserName")
				.AuthorizedBy = requestBody(propertyName:="mAuthorizedBy")
				.StatusID = CInt(requestBody(propertyName:="mStatusID"))
				.StatusName = requestBody(propertyName:="mStatusName")
				.TransTypeID = requestBody(propertyName:="mTransTypeID")
				.BillToTypeID = CInt(requestBody("mBillToTypeID"))
				.ShipToTypeID = CInt(requestBody("mShipToTypeID"))
				.LocationID = New Guid(requestBody("mLocationID").ToString)
				.CustomerID = New Guid(requestBody("mCustomerID").ToString)
				.BillingAddress = requestBody("mBillingAddress")
				.ShippingAddress = requestBody("mShippingAddress")
				.IsFOC = CBool(requestBody("mIsFOC"))
				.IsCustomer = CBool(requestBody("mIsCustomer"))
				.Text = requestBody(propertyName:="mText")
				.No = requestBody(propertyName:="mNo")
				.Amend = requestBody(propertyName:="mAmend")
				.AmendedStatus = CBool(requestBody(propertyName:="mAmendedStatus"))
				.OpeningLine = requestBody(propertyName:="mOpeningLine")
				.AircraftReg = requestBody(propertyName:="mAircraftReg")
				.OrderConfirmationNo = requestBody(propertyName:="mOrderConfirmationNo")
				.IsRoundOff = CBool(requestBody("mIsRoundOff"))
				.CAdvancePayment = CDec(requestBody("mCAdvancePayment"))
				.ShipInVia = requestBody(propertyName:="mShipInVia")
				.ShipOutVia = requestBody(propertyName:="mShipOutVia")
				.IsCalibrationOrder = requestBody(propertyName:="mIsCalibrationOrder")
				.POTowardsID = CInt(requestBody(propertyName:="mPOTowardsID"))
				.POTowards = requestBody(propertyName:="mPOTowards")
				.Remark = requestBody(propertyName:="mRemark")
				.IsPBHPurchase = requestBody(propertyName:="mIsPBHPurchase")
				.IsOverhaul = CBool(requestBody("mIsOverhaul"))
				.AgainstTypeID = CInt(requestBody("mAgainstTypeID"))
				.AmendCount = CInt(requestBody("mAmendCount"))
				.ReceiptCount = CInt(requestBody("mReceiptCount"))
				.IssueCount = CInt(requestBody("mIssueCount"))
				.IsAttachmentAdded = CBool(requestBody("mIsAttachmentAdded"))
				.StateCode = requestBody("mStateCode")
				.ClientStateCode = requestBody("mClientStateCode")
				.VendorCountry = requestBody("mVendorCountry")
				.VendorGSTIN = requestBody("mVendorGSTIN")
				.Visibility = requestBody("mVisibility")
				.IsPADone = CBool(requestBody("mIsPADone"))
				.ExchangeOrderTypeID = CInt(requestBody("mExchangeOrderTypeID"))
				.MSPID = New Guid(requestBody("mMSPID").ToString)
				.MSPAssemblyID = New Guid(requestBody("mMSPAssemblyID").ToString)
				.AssemblyName = requestBody("mAssemblyName")
				.MSPPORemark = requestBody("mMSPPORemark")
				.PlanName = requestBody("mPlanName")
				.ContractNo = requestBody("mContractNo")

			End With

			For i As Integer = 0 To ItemArray.Count - 1

				mOrder.OrderItems.Add(mOrder.ID)

				With mOrder.OrderItems.CurrentItem

					.ItemID = New Guid(ItemArray(i)("mItemID").ToString)
					.FromItemID = New Guid(ItemArray(i)("mFromItemID").ToString)
					.FromNo = ""
					.FromDate = ""
					.IsSerializedPart = ItemArray(i)("mIsSerializedPart")
					.Qty = CDec(ItemArray(i)("mQty"))
					.CRate = CDec(ItemArray(i)("mCRate"))
					.Remark = ItemArray(i)("mRemark")
					.Note = ItemArray(i)("mNote")
					.UnitID = New Guid(ItemArray(i)("mUnitID").ToString)
					.UnitName = ItemArray(i)("mUnitName")
					.AmendedOrderItemID = New Guid(ItemArray(i)("mAmendedOrderItemID").ToString)
					.SrNo = CInt(ItemArray(i)("mSrNo"))
					.OrderID = New Guid(ItemArray(i)("mOrderID").ToString)
					.ModelID = New Guid(ItemArray(i)("mModelID").ToString)
					.ModelName = ItemArray(i)("mModelName")
					.CBillBackRate = CDec(ItemArray(i)("mCBillBackRate"))
					.DeliveryInDays = CInt(ItemArray(i)("mDeliveryInDays"))
					.ItemTypeID = CInt(ItemArray(i)("mItemTypeID"))
					.DeliveryStatusID = CInt(ItemArray(i)("mDeliveryStatusID"))
					.AWBillNo = ItemArray(i)("mAWBillNo")
					.NAWBillNo = ItemArray(i)("mNAWBillNo")
					.DocketNo = ItemArray(i)("mDocketNo")
					.PriorityID = CInt(ItemArray(i)("mPriorityID"))
					.PriorityName = ItemArray(i)("mPriorityName")
					.ReceiptItemID = New Guid(ItemArray(i)("mReceiptItemID").ToString)
					.SerialNo = ItemArray(i)("mSerialNo")
					.IssueBalanceQty = CDec(ItemArray(i)("mIssueBalanceQty"))
					.EROQty = CDec(ItemArray(i)("mEROQty"))
					.IsInWarranty = CBool(ItemArray(i)("mIsInWarranty"))
					.WarrantyInDays = CInt(ItemArray(i)("mWarrantyInDays"))
					.WarrantyStartDate = CDate(ItemArray(i)("mWarrantyStartDate").First.First).ToString(format:=_DateFormat)
					.WarrantyExpiryDate = CDate(ItemArray(i)("mWarrantyExpiryDate").First.First).ToString(format:=_DateFormat)
					.PerDiscount = CDec(ItemArray(i)("mPerDiscount"))
					.RequestedBy = ItemArray(i)("mRequestedBy")
					.CanceledQty = CDec(ItemArray(i)("mCanceledQty"))
					.IsScheduleExpenses = CBool(ItemArray(i)("mIsScheduleExpenses"))
					.StoreID = New Guid(ItemArray(i)("mStoreID").ToString)
					.PreviousOrdQty = CDec(ItemArray(i)("mPreviousOrdQty"))
					.TempEROQtyForEnableDisable = CDec(ItemArray(i)("mTempEROQtyForEnableDisable"))
					.IsScheduleExpensesYes = CBool(ItemArray(i)("mIsScheduleExpensesYes"))
					.IsScheduleExpensesNo = CBool(ItemArray(i)("mIsScheduleExpensesNo"))
					.CGSTPercentage = CDec(ItemArray(i)("mCGSTPercentage"))
					.CGSTCAmount = CDec(ItemArray(i)("mCGSTCAmount"))
					.SGSTPercentage = CDec(ItemArray(i)("mSGSTPercentage"))
					.SGSTCAmount = CDec(ItemArray(i)("mSGSTCAmount"))
					.IGSTPercentage = CDec(ItemArray(i)("mIGSTPercentage"))
					.IGSTCAmount = CDec(ItemArray(i)("mIGSTCAmount"))
					.TotalCAmount = CDec(ItemArray(i)("mTotalCAmount"))
					.HSNACSCode = ItemArray(i)("mHSNACSCode")
					.IsWarrantyApplicable = CBool(ItemArray(i)("mIsWarrantyApplicable"))
					.AlternateItemID = New Guid(ItemArray(i)("mAlternateItemID").ToString)
					.AlternateItemName = ItemArray(i)("mAlternateItemName")
					.AlternateItemDescription = ItemArray(i)("mAlternateItemDescription")
					.CompStatusID = New Guid(ItemArray(i)("mCompStatusID").ToString)
					.TechDirectionCount = CInt(ItemArray(i)("mTechDirectionCount"))
					.TechDirectionDate = CDate(ItemArray(i)("mTechDirectionDate").First.First).ToString(format:=_DateFormat)
					.TechDirectionRegNo = ItemArray(i)("mTechDirectionRegNo")
					.MachineID = New Guid(ItemArray(i)("mMachineID").ToString)
					.RequisitionTextNo = ItemArray(i)("mRequisitionTextNo")
					.ReceiptBalanceQtyToShowOnGrid = CDec(ItemArray(i)("mReceiptBalanceQtyToShowOnGrid"))

					'New Purchase/OverHaul Order Against Quotation  'New Rental lease Order Against Quotation
					If (
						 (mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 2) Or 'New Purchase Order against Quotation
						 (mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 7) Or 'New Purchase Order against Enquiry i.e. in directly against Quotation
						 (mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 2 And mOrder.IsOverhaul = True) Or   'Overhaul Order against Quotation
						 (mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 2 And mOrder.IsOverhaul = False) Or 'Repair Order against Quotation
						 (mOrder.TransTypeID = 39 And mOrder.AgainstTypeID = 2) 'Rental Lease Order against Quotation
					   ) Then

						Dim OrderItemQuotationItemsArray As JArray = CType(ItemArray(i)("mOrderItemQuotationItems"), JArray)
						mOrder.OrderItems.CurrentItem.OrderItemQuotationItems.Add(OrderItemID:=mOrder.OrderItems.CurrentItem.ID,
																				  QuotationItemID:=New Guid(OrderItemQuotationItemsArray(0)("mQuotationItemID").ToString),
																				  Qty:=CDbl(OrderItemQuotationItemsArray(0)("mQty").ToString),
																				  QuotationNo:=OrderItemQuotationItemsArray(0)("mQuotationNo"),
																				  QuotationDate:=CDate(OrderItemQuotationItemsArray(0)("mQuotationDate").First.First).ToString(format:=_DateFormat),
																				  QuotationID:=New Guid(OrderItemQuotationItemsArray(0)("mQuotationID").ToString))
					End If

					'New Purchase Order Against Sales Order
					If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 4 Then

						Dim OrderItemSalesOrderItemsArray As JArray = CType(ItemArray(i)("mOrderItemSalesOrderItems"), JArray)
						mOrder.OrderItems.CurrentItem.OrderItemSalesOrderItems.Add(OrderItemID:=mOrder.OrderItems.CurrentItem.ID,
																				   SalesOrderItemID:=New Guid(OrderItemSalesOrderItemsArray(0)("mSalesOrderItemID").ToString),
																				   Qty:=CDbl(OrderItemSalesOrderItemsArray(0)("mQty").ToString),
																				   SalesOrderNo:=OrderItemSalesOrderItemsArray(0)("mSalesOrderNo"))

					End If

					'New Purchase/Exchange/OverHaul/Repair Order Against Requisition Item
					If (
						(mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 6) Or 'New Purchase Order against Requisition Items
						(mOrder.TransTypeID = 31 And mOrder.AgainstTypeID = 5 And mOrder.ExchangeOrderTypeID = 2) Or 'Exchange Order against Requisition Items
						(mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 5 And mOrder.IsOverhaul = True And mOrder.ExchangeOrderTypeID = 2) Or 'Overhaul Order against Requisition Items
						(mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 5 And mOrder.IsOverhaul = False And mOrder.ExchangeOrderTypeID = 2)   'Repair Order against Requisition Items
					   ) Then

						Dim RequisitionItemOrderItemsArray As JArray = CType(ItemArray(i)("mRequisitionItemOrderItems"), JArray)
						mOrder.OrderItems.CurrentItem.RequisitionItemOrderItems.Add(OrderItemID:=mOrder.OrderItems.CurrentItem.ID,
														   RequisitionItemID:=New Guid(RequisitionItemOrderItemsArray(0)("mReqItemID").ToString),
														   Qty:=CDbl(RequisitionItemOrderItemsArray(0)("mQty").ToString),
														   RequisitionNo:=RequisitionItemOrderItemsArray(0)("mRequisitionNo"))

					End If

				End With

			Next

			For j As Integer = 0 To TermArray.Count - 1

				mOrder.OrderTerms.Add(mOrder.ID)

				With mOrder.OrderTerms.CurrentItem

					.SrNo = CInt(TermArray(j)("mSrNo"))
					.OrderID = New Guid(TermArray(j)("mOrderID").ToString)
					.TermID = New Guid((TermArray(j)("mTermID").ToString))
					.Terms = TermArray(j)("mTerms")

				End With

			Next

			For k As Integer = 0 To ChargeArray.Count - 1

				mOrder.OrderCharges.Add(mOrder.ID)

				With mOrder.OrderCharges.CurrentItem

					.SrNo = CInt(ChargeArray(k)("mSrNo"))
					.OrderID = New Guid(ChargeArray(k)("mOrderID").ToString)
					.ChargeID = New Guid((ChargeArray(k)("mChargeID").ToString))
					.StatusBasic = CBool(ChargeArray(k)("mStatusBasic"))
					.Percentage = CDec(ChargeArray(k)("mPercentage"))
					.CChargeAmount = CDec(ChargeArray(k)("mCChargeAmount"))
					.Currency = ChargeArray(k)("mCurrency")
					.BasicAmount = CDec(ChargeArray(k)("mBasicAmount"))
					.TotalAmount = CDec(ChargeArray(k)("mTotalAmount"))
					.ConversionFactor = CDec(ChargeArray(k)("mConversionFactor"))

				End With

			Next

			Dim result = _AttachmentHelper.SaveAttachments(AttachmentArray:=AttachmentArray,
														   ModuleObject:=mOrder,
														   ModuleName:="Order")

			returnMessage = $"{result.Item2}"

			If Not String.IsNullOrEmpty(returnMessage) Then
				Return New ReturnMessage(Status:="Validation",
										 Message:=returnMessage)
			End If

			mOrder = CType(result.Item1, Order)
			mOrder.CalculateTotal()

			If mOrder.IsRoundOff Then
				mOrder.RoundCGrandTotal()
			End If

			If mOrder.IsValid Then
				mOrder.Save()
				OrderID = $"{mOrder.ID}"
			Else
				Return New ReturnMessage(Status:="Validation",
										 Message:=$"{_BrokenRulesHelper.FetchBrokenRules(CommonObject:=mOrder, ModuleName:="Order")}")
			End If

			_ModuleHelper.SendEmailToBytzSoft(TransTypeID:=mOrder.TransTypeID,
											  Username:=User.Identity.Name,
											  ModuleFrom:="Order",
											  Action:=IIf(mOrder.StatusID = 2, "Authorized", "Saved"),
											  ClientCode:=AppSettings("ClientCode"),
											  TransactionNo:=mOrder.OrderNo,
											  TransactionDate:=mOrder.OrderDateFormatted)

			Return New ReturnMessage(Status:="Success",
									 Message:=$"Order Saved Successfully!",
									 TransactionID:=OrderID)

		Catch ex As SqlException
			Return New ReturnMessage(Status:="SqlException",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Order", ex:=ex)}")
		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Order", ex:=ex, UseAsException:=True)}")
		End Try

	End Function

	Private Function SetExistingOrderValues(requestBody As JObject) As ReturnMessage

		Try

			Dim mOrder As Order = Order.GetOrder(ID:=New Guid(requestBody("mID").ToString))

			Dim ItemArray As JArray = CType(requestBody("mOrderItems"), JArray)
			Dim TermArray As JArray = CType(requestBody("mOrderTerms"), JArray)
			Dim ChargeArray As JArray = CType(requestBody("mOrderCharges"), JArray)
			Dim AttachmentArray As JArray = CType(requestBody("mFileAttachments"), JArray)

			With mOrder

				_DateFormat = requestBody(propertyName:="mDate")("mFormat")

				.OrderDate = CDate(requestBody(propertyName:="mDate").First.First).ToString(format:=_DateFormat)
				.DeliveryWithinDays = requestBody(propertyName:="mDeliveryWithinDays")
				.IntOrderNo = requestBody(propertyName:="mIntOrderNo")
				.VendorID = New Guid(requestBody(propertyName:="mVendorID").ToString)
				.Attention = requestBody("mAttention")
				.QuotationNo = requestBody("mQuotationNo")
				.QuotationDate = CDate(requestBody(propertyName:="mQuotationDate").First.First).ToString(format:=_DateFormat)
				.CurrencyID = New Guid(requestBody("mCurrencyID").ToString)
				.ConversionFactor = CDec(requestBody("mConversionFactor"))
				.UserName = requestBody(propertyName:="mUserName")
				.AuthorizedBy = requestBody(propertyName:="mAuthorizedBy")
				.StatusID = CInt(requestBody(propertyName:="mStatusID"))
				.StatusName = requestBody(propertyName:="mStatusName")
				.TransTypeID = requestBody(propertyName:="mTransTypeID")
				.BillToTypeID = CInt(requestBody("mBillToTypeID"))
				.ShipToTypeID = CInt(requestBody("mShipToTypeID"))
				.LocationID = New Guid(requestBody("mLocationID").ToString)
				.CustomerID = New Guid(requestBody("mCustomerID").ToString)
				.BillingAddress = requestBody("mBillingAddress")
				.ShippingAddress = requestBody("mShippingAddress")
				.IsFOC = CBool(requestBody("mIsFOC"))
				.IsCustomer = CBool(requestBody("mIsCustomer"))
				.Text = requestBody(propertyName:="mText")
				.No = requestBody(propertyName:="mNo")
				.Amend = requestBody(propertyName:="mAmend")
				.AmendedStatus = CBool(requestBody(propertyName:="mAmendedStatus"))
				.OpeningLine = requestBody(propertyName:="mOpeningLine")
				.AircraftReg = requestBody(propertyName:="mAircraftReg")
				.OrderConfirmationNo = requestBody(propertyName:="mOrderConfirmationNo")
				.IsRoundOff = CBool(requestBody("mIsRoundOff"))
				.CAdvancePayment = CDec(requestBody("mCAdvancePayment"))
				.ShipInVia = requestBody(propertyName:="mShipInVia")
				.ShipOutVia = requestBody(propertyName:="mShipOutVia")
				.IsCalibrationOrder = requestBody(propertyName:="mIsCalibrationOrder")
				.POTowardsID = CInt(requestBody(propertyName:="mPOTowardsID"))
				.POTowards = requestBody(propertyName:="mPOTowards")
				.Remark = requestBody(propertyName:="mRemark")
				.IsPBHPurchase = requestBody(propertyName:="mIsPBHPurchase")
				.IsOverhaul = CBool(requestBody("mIsOverhaul"))
				.AgainstTypeID = CInt(requestBody("mAgainstTypeID"))
				.AmendCount = CInt(requestBody("mAmendCount"))
				.ReceiptCount = CInt(requestBody("mReceiptCount"))
				.IssueCount = CInt(requestBody("mIssueCount"))
				.IsAttachmentAdded = CBool(requestBody("mIsAttachmentAdded"))
				.StateCode = requestBody("mStateCode")
				.ClientStateCode = requestBody("mClientStateCode")
				.VendorCountry = requestBody("mVendorCountry")
				.VendorGSTIN = requestBody("mVendorGSTIN")
				.Visibility = requestBody("mVisibility")
				.IsPADone = CBool(requestBody("mIsPADone"))
				.ExchangeOrderTypeID = CInt(requestBody("mExchangeOrderTypeID"))
				.MSPID = New Guid(requestBody("mMSPID").ToString)
				.MSPAssemblyID = New Guid(requestBody("mMSPAssemblyID").ToString)
				.AssemblyName = requestBody("mAssemblyName")
				.MSPPORemark = requestBody("mMSPPORemark")
				.PlanName = requestBody("mPlanName")
				.ContractNo = requestBody("mContractNo")

			End With

			For i As Integer = 0 To ItemArray.Count - 1

				Dim ID As New Guid(ItemArray(i)("mID").ToString)
				Dim mIsNew As Boolean = CBool(ItemArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(ItemArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(ItemArray(i)("mIsDirty"))
				Dim mOrderItem As OrderItem

				If mIsNew Then
					mOrder.OrderItems.Add(mOrder.ID)
					mOrderItem = mOrder.OrderItems.CurrentItem
				Else
					mOrderItem = mOrder.OrderItems(ID)
				End If

				If mIsDeleted Then
					mOrder.OrderItems.Remove(mOrderItem)
				End If

				If mIsNew Or mIsDirty Then

					With mOrderItem

						.ItemID = New Guid(ItemArray(i)("mItemID").ToString)
						.FromItemID = New Guid(ItemArray(i)("mFromItemID").ToString)
						.FromNo = ""
						.FromDate = ""
						.IsSerializedPart = ItemArray(i)("mIsSerializedPart")
						.Qty = CDec(ItemArray(i)("mQty"))
						.CRate = CDec(ItemArray(i)("mCRate"))
						.Remark = ItemArray(i)("mRemark")
						.Note = ItemArray(i)("mNote")
						.UnitID = New Guid(ItemArray(i)("mUnitID").ToString)
						.UnitName = ItemArray(i)("mUnitName")
						.AmendedOrderItemID = New Guid(ItemArray(i)("mAmendedOrderItemID").ToString)
						.SrNo = CInt(ItemArray(i)("mSrNo"))
						.OrderID = New Guid(ItemArray(i)("mOrderID").ToString)
						.ModelID = New Guid(ItemArray(i)("mModelID").ToString)
						.ModelName = ItemArray(i)("mModelName")
						.CBillBackRate = CDec(ItemArray(i)("mCBillBackRate"))
						.DeliveryInDays = CInt(ItemArray(i)("mDeliveryInDays"))
						.ItemTypeID = CInt(ItemArray(i)("mItemTypeID"))
						.DeliveryStatusID = CInt(ItemArray(i)("mDeliveryStatusID"))
						.AWBillNo = ItemArray(i)("mAWBillNo")
						.NAWBillNo = ItemArray(i)("mNAWBillNo")
						.DocketNo = ItemArray(i)("mDocketNo")
						.PriorityID = CInt(ItemArray(i)("mPriorityID"))
						.PriorityName = ItemArray(i)("mPriorityName")
						.ReceiptItemID = New Guid(ItemArray(i)("mReceiptItemID").ToString)
						.SerialNo = ItemArray(i)("mSerialNo")
						.IssueBalanceQty = CDec(ItemArray(i)("mIssueBalanceQty"))
						.EROQty = CDec(ItemArray(i)("mEROQty"))
						.IsInWarranty = CBool(ItemArray(i)("mIsInWarranty"))
						.WarrantyInDays = CInt(ItemArray(i)("mWarrantyInDays"))
						.WarrantyStartDate = CDate(ItemArray(i)("mWarrantyStartDate").First.First).ToString(format:=_DateFormat)
						.WarrantyExpiryDate = CDate(ItemArray(i)("mWarrantyExpiryDate").First.First).ToString(format:=_DateFormat)
						.PerDiscount = CDec(ItemArray(i)("mPerDiscount"))
						.RequestedBy = ItemArray(i)("mRequestedBy")
						.CanceledQty = CDec(ItemArray(i)("mCanceledQty"))
						.IsScheduleExpenses = CBool(ItemArray(i)("mIsScheduleExpenses"))
						.StoreID = New Guid(ItemArray(i)("mStoreID").ToString)
						.PreviousOrdQty = CDec(ItemArray(i)("mPreviousOrdQty"))
						.TempEROQtyForEnableDisable = CDec(ItemArray(i)("mTempEROQtyForEnableDisable"))
						.IsScheduleExpensesYes = CBool(ItemArray(i)("mIsScheduleExpensesYes"))
						.IsScheduleExpensesNo = CBool(ItemArray(i)("mIsScheduleExpensesNo"))
						.CGSTPercentage = CDec(ItemArray(i)("mCGSTPercentage"))
						.CGSTCAmount = CDec(ItemArray(i)("mCGSTCAmount"))
						.SGSTPercentage = CDec(ItemArray(i)("mSGSTPercentage"))
						.SGSTCAmount = CDec(ItemArray(i)("mSGSTCAmount"))
						.IGSTPercentage = CDec(ItemArray(i)("mIGSTPercentage"))
						.IGSTCAmount = CDec(ItemArray(i)("mIGSTCAmount"))
						.TotalCAmount = CDec(ItemArray(i)("mTotalCAmount"))
						.HSNACSCode = ItemArray(i)("mHSNACSCode")
						.IsWarrantyApplicable = CBool(ItemArray(i)("mIsWarrantyApplicable"))
						.AlternateItemID = New Guid(ItemArray(i)("mAlternateItemID").ToString)
						.AlternateItemName = ItemArray(i)("mAlternateItemName")
						.AlternateItemDescription = ItemArray(i)("mAlternateItemDescription")
						.CompStatusID = New Guid(ItemArray(i)("mCompStatusID").ToString)
						.TechDirectionCount = CInt(ItemArray(i)("mTechDirectionCount"))
						.TechDirectionDate = CDate(ItemArray(i)("mTechDirectionDate").First.First).ToString(format:=_DateFormat)
						.TechDirectionRegNo = ItemArray(i)("mTechDirectionRegNo")
						.MachineID = New Guid(ItemArray(i)("mMachineID").ToString)
						.RequisitionTextNo = ItemArray(i)("mRequisitionTextNo")
						.ReceiptBalanceQtyToShowOnGrid = CDec(ItemArray(i)("mReceiptBalanceQtyToShowOnGrid"))

						'New Purchase/OverHaul Order Against Quotation / Enquiry  'New Rental lease Order Against Quotation / Enquiry
						If (
								(mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 2) Or 'New Purchase Order against Quotation
								(mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 7) Or 'New Purchase Order against Enquiry i.e. in directly against Quotation
								(mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 2 And mOrder.IsOverhaul = True) Or   'Overhaul Order against Quotation
								(mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 2 And mOrder.IsOverhaul = False) Or 'Repair Order against Quotation
								(mOrder.TransTypeID = 39 And mOrder.AgainstTypeID = 2) 'Rental Lease Order against Quotation
							) Then

							Dim OrderItemQuotationItemsArray As JArray = CType(ItemArray(i)("mOrderItemQuotationItems"), JArray)
							Dim mOrderItemQuotationItem As OrderItemQuotationItem

							If CBool(OrderItemQuotationItemsArray(0)("mIsNew")) = True Then

								mOrderItem.OrderItemQuotationItems.Add(OrderItemID:=mOrderItem.ID,
																	   QuotationItemID:=New Guid(OrderItemQuotationItemsArray(0)("mQuotationItemID").ToString),
																	   Qty:=CDbl(OrderItemQuotationItemsArray(0)("mQty").ToString),
																	   QuotationNo:=OrderItemQuotationItemsArray(0)("mQuotationNo"),
																	   QuotationDate:=CDate(OrderItemQuotationItemsArray(0)("mQuotationDate").First.First).ToString(format:=_DateFormat),
																	   QuotationID:=New Guid(OrderItemQuotationItemsArray(0)("mQuotationID").ToString))

							Else

								mOrderItemQuotationItem = mOrderItem.OrderItemQuotationItems(New Guid(OrderItemQuotationItemsArray(0)("mID").ToString))

								If CBool(OrderItemQuotationItemsArray(0)("mIsDirty")) = True Then

									With mOrderItemQuotationItem

										.OrderItemID = mOrderItem.ID
										.QuotationItemID = New Guid(OrderItemQuotationItemsArray(0)("mQuotationItemID").ToString)
										.Qty = CDbl(OrderItemQuotationItemsArray(0)("mQty").ToString)
										.QuotationDate = CDate(OrderItemQuotationItemsArray(0)("mQuotationDate").First.First).ToString(format:=_DateFormat)
										.QuotationID = New Guid(OrderItemQuotationItemsArray(0)("mQuotationID").ToString)

									End With

								End If

								If mOrderItemQuotationItem.IsDeleted Then
									mOrderItem.OrderItemQuotationItems.Remove(mOrderItemQuotationItem)
								End If

							End If

						End If

						'New Purchase Order Against Sales Order
						If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 4 Then

							Dim OrderItemSalesOrderItemsArray As JArray = CType(ItemArray(i)("mOrderItemSalesOrderItems"), JArray)
							Dim mOrderItemSalesOrderItem As OrderItemSalesOrderItem

							If CBool(OrderItemSalesOrderItemsArray(0)("mIsNew")) = True Then

								mOrderItem.OrderItemSalesOrderItems.Add(OrderItemID:=mOrderItem.ID,
																		SalesOrderItemID:=New Guid(OrderItemSalesOrderItemsArray(0)("mSalesOrderItemID").ToString),
																		Qty:=CDbl(OrderItemSalesOrderItemsArray(0)("mQty").ToString),
																		SalesOrderNo:=OrderItemSalesOrderItemsArray(0)("mSalesOrderNo"))

							Else

								mOrderItemSalesOrderItem = mOrderItem.OrderItemSalesOrderItems(New Guid(OrderItemSalesOrderItemsArray(0)("mID").ToString))

								If CBool(OrderItemSalesOrderItemsArray(0)("mIsDirty")) = True Then

									With mOrderItemSalesOrderItem

										.OrderItemID = mOrderItem.ID
										.SalesOrderItemID = New Guid(OrderItemSalesOrderItemsArray(0)("mSalesOrderItemID").ToString)
										.Qty = CDbl(OrderItemSalesOrderItemsArray(0)("mQty").ToString)
										.SalesOrderDate = CDate(OrderItemSalesOrderItemsArray(0)("mSalesOrderDate").First.First).ToString(format:=_DateFormat)

									End With

								End If

								If mOrderItemSalesOrderItem.IsDeleted Then
									mOrderItem.OrderItemSalesOrderItems.Remove(mOrderItemSalesOrderItem)
								End If

							End If

						End If

						'New Purchase/Exchange/OverHaul/Repair Order Against Requisition Item
						If (
								(mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 6) Or 'New Purchase Order against Requisition Items
								(mOrder.TransTypeID = 31 And mOrder.AgainstTypeID = 5 And mOrder.ExchangeOrderTypeID = 2) Or 'Exchange Order against Requisition Items
								(mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 5 And mOrder.IsOverhaul = True And mOrder.ExchangeOrderTypeID = 2) Or 'Overhaul Order against Requisition Items
								(mOrder.TransTypeID = 38 And mOrder.AgainstTypeID = 5 And mOrder.IsOverhaul = False And mOrder.ExchangeOrderTypeID = 2)   'Repair Order against Requisition Items
							) Then

							Dim RequisitionItemOrderItemsArray As JArray = CType(ItemArray(i)("mRequisitionItemOrderItems"), JArray)
							Dim mRequisitionItemOrderItem As RequisitionItemOrderItem

							If CBool(RequisitionItemOrderItemsArray(0)("mIsNew")) = True Then

								mOrderItem.RequisitionItemOrderItems.Add(OrderItemID:=mOrderItem.ID,
																		 RequisitionItemID:=New Guid(RequisitionItemOrderItemsArray(0)("mReqItemID").ToString),
																		 Qty:=CDbl(RequisitionItemOrderItemsArray(0)("mQty").ToString),
																		 RequisitionNo:=RequisitionItemOrderItemsArray(0)("mRequisitionNo"))

							Else

								mRequisitionItemOrderItem = mOrderItem.RequisitionItemOrderItems(New Guid(RequisitionItemOrderItemsArray(0)("mID").ToString))

								If CBool(RequisitionItemOrderItemsArray(0)("mIsDirty")) = True Then

									With mRequisitionItemOrderItem

										.OrderItemID = mOrderItem.ID
										.ReqItemID = New Guid(RequisitionItemOrderItemsArray(0)("mReqItemID").ToString)
										.Qty = CDbl(RequisitionItemOrderItemsArray(0)("mQty").ToString)
										.RequisitionDate = CDate(RequisitionItemOrderItemsArray(0)("mRequisitionDate").First.First).ToString(format:=_DateFormat)

									End With

								End If

								If mRequisitionItemOrderItem.IsDeleted Then
									mOrderItem.RequisitionItemOrderItems.Remove(mRequisitionItemOrderItem)
								End If

							End If

						End If

					End With

				End If

			Next

			For i As Integer = 0 To TermArray.Count - 1

				Dim ID As New Guid(TermArray(i)("mID").ToString)
				Dim mIsNew As Boolean = CBool(TermArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(TermArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(TermArray(i)("mIsDirty"))
				Dim mOrderTerm As OrderTerm

				If mIsNew Then
					mOrder.OrderTerms.Add(mOrder.ID)
					mOrderTerm = mOrder.OrderTerms.CurrentItem
				Else
					mOrderTerm = mOrder.OrderTerms(ID)
				End If

				If mIsDeleted Then
					mOrder.OrderTerms.Remove(mOrderTerm)
				End If

				If mIsNew Or mIsDirty Then

					With mOrderTerm

						.SrNo = CInt(TermArray(i)("mSrNo"))
						.OrderID = New Guid(TermArray(i)("mOrderID").ToString)
						.TermID = New Guid((TermArray(i)("mTermID").ToString))
						.Terms = TermArray(i)("mTerms")

					End With

				End If

			Next

			For i As Integer = 0 To ChargeArray.Count - 1

				Dim ID As New Guid(ChargeArray(i)("mID").ToString)
				Dim mIsNew As Boolean = CBool(ChargeArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(ChargeArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(ChargeArray(i)("mIsDirty"))
				Dim mOrderCharge As OrderCharge

				If mIsNew Then
					mOrder.OrderCharges.Add(mOrder.ID)
					mOrderCharge = mOrder.OrderCharges.CurrentItem
				Else
					mOrderCharge = mOrder.OrderCharges(ID)
				End If

				If mOrder.IsRoundOff = False Then
					If New Guid(ChargeArray(i)("mChargeID").ToString).Equals(New Guid("{40000000-0000-0000-0000-000000000000}")) Or
						New Guid(ChargeArray(i)("mChargeID").ToString).Equals(New Guid("{50000000-0000-0000-0000-000000000000}")) Then

						mIsDeleted = False  'We have set these two variables false as in PayLoad for these two charges it these two are true.
						mIsDirty = False

						mOrder.OrderCharges.Remove(mOrderCharge)
					End If

				End If

				If mIsDeleted Then
					mOrder.OrderCharges.Remove(mOrderCharge)
				End If


				If mIsNew Or mIsDirty Then

					With mOrderCharge

						.SrNo = CInt(ChargeArray(i)("mSrNo"))
						.OrderID = New Guid(ChargeArray(i)("mOrderID").ToString)
						.ChargeID = New Guid((ChargeArray(i)("mChargeID").ToString))
						.StatusBasic = CBool(ChargeArray(i)("mStatusBasic"))
						.Percentage = CDec(ChargeArray(i)("mPercentage"))
						.CChargeAmount = CDec(ChargeArray(i)("mCChargeAmount"))
						.Currency = ChargeArray(i)("mCurrency")
						.BasicAmount = CDec(ChargeArray(i)("mBasicAmount"))
						.TotalAmount = CDec(ChargeArray(i)("mTotalAmount"))
						.ConversionFactor = CDec(ChargeArray(i)("mConversionFactor"))

					End With

				End If

			Next

			Dim result = _AttachmentHelper.SaveAttachments(AttachmentArray:=AttachmentArray,
														   ModuleObject:=mOrder,
														   ModuleName:="Order")

			Dim returnMessage As String = $"{result.Item2}"

			If Not String.IsNullOrEmpty(returnMessage) Then
				Return New ReturnMessage(Status:="Validation",
										 Message:=returnMessage)
			End If

			mOrder = CType(result.Item1, Order)

			'===================================================
			If CBool(AppSettings("AddChargesInRCI")) Then 'Added By Prashant

				If mOrder.TransTypeID = 31 And mOrder.StatusID = 2 Then 'Exchange Order

					If (mOrder.OrderItems.Count = 1 And mOrder.OrderItems.CurrentItem.Qty = 1 And mOrder.OrderItems.CurrentItem.ReceiptBalanceQty = 0) Then

						_ReceiptCumInvoice = ReceiptCumInvoice.GetReceiptCumInvoice(OrderID:=mOrder.ID, IsByOrderID:=True)

						If mOrder.OrderCharges.Count > 0 Then 'Order has charges

							' --- Add / Update ---
							For Each oCharge As OrderCharge In mOrder.OrderCharges

								Dim rCharge As InvoiceCharge = Nothing

								' Find existing charge
								For Each c As InvoiceCharge In _ReceiptCumInvoice.Invoice.InvoiceCharges
									If c.ChargeID = oCharge.ChargeID Then
										rCharge = c
										Exit For
									End If
								Next

								If rCharge Is Nothing Then
									' --- Add new charge ---
									_ReceiptCumInvoice.Invoice.InvoiceCharges.Add(_ReceiptCumInvoice.Invoice.ID)

									With _ReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentItem

										.SrNo = _ReceiptCumInvoice.ReceiptCumInvoiceCharges.CurrentIndex + 1
										.ChargeID = oCharge.ChargeID
										.ConversionFactor = _ReceiptCumInvoice.ConversionFactor
										.Percentage = oCharge.Percentage
										.CChargeAmount = oCharge.CChargeAmount

										If _ReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then
											.BasicAmount = _ReceiptCumInvoice.ReceiptCumInvoiceItems.CGrandTotalAmountItem
										End If

									End With

								Else
									' --- Update if amount differs ---
									If rCharge.CChargeAmount <> oCharge.CChargeAmount Then
										rCharge.CChargeAmount = oCharge.CChargeAmount
									End If
								End If

							Next

							' --- Delete charges that are not in Order anymore ---
							For i As Integer = _ReceiptCumInvoice.Invoice.InvoiceCharges.Count - 1 To 0 Step -1

								Dim rCharge As InvoiceCharge = _ReceiptCumInvoice.Invoice.InvoiceCharges(i)
								Dim existsInOrder As Boolean = False

								For Each oCharge As OrderCharge In mOrder.OrderCharges
									If oCharge.ChargeID = rCharge.ChargeID Then
										existsInOrder = True
										Exit For
									End If
								Next

								If Not existsInOrder Then
									_ReceiptCumInvoice.Invoice.InvoiceCharges.RemoveAt(i)
								End If

							Next

							' --- Recalculate totals ---
							If _ReceiptCumInvoice.Invoice.InvoiceCharges.IsDirty Then
								_ReceiptCumInvoice.Invoice.CalculateTotal()
								_ReceiptCumInvoice.Save()
							End If

						End If

					End If

				End If

			End If
			'===================================================

			mOrder.CalculateTotal()

			If mOrder.IsRoundOff Then
				mOrder.RoundCGrandTotal()
			End If

			If mOrder.IsValid Then
				mOrder.Save()
				OrderID = $"{mOrder.ID}"
			Else
				Return New ReturnMessage(Status:="Validation",
										 Message:=$"{_BrokenRulesHelper.FetchBrokenRules(CommonObject:=mOrder, ModuleName:="Order")}")
			End If

			_ModuleHelper.SendEmailToBytzSoft(TransTypeID:=mOrder.TransTypeID,
											  Username:=User.Identity.Name,
											  ModuleFrom:="Order",
											  Action:=IIf(mOrder.StatusID = 2, "Authorized", "Saved"),
											  ClientCode:=AppSettings("ClientCode"),
											  TransactionNo:=mOrder.OrderNo,
											  TransactionDate:=mOrder.OrderDateFormatted)

			Return New ReturnMessage(Status:="Success",
									 Message:=$"Order Saved Successfully!",
									 TransactionID:=OrderID)

		Catch ex As SqlException
			Return New ReturnMessage(Status:="SqlException",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Order", ex:=ex)}")
		Catch ex As Exception
			Return New ReturnMessage(Status:="Exception",
									 Message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Order", ex:=ex, UseAsException:=True)}")
		End Try

	End Function

	<HttpPost>
	Public Function SaveAmendOrder(OrderID As Guid) As Order

		Dim Order As Order
		Dim AmendedOrder As Order

		Try

			Order = Order.GetOrder(ID:=OrderID)
			Order.StatusID = 1
			Order.AmendedStatus = True
			Order.AmendCount += 1

			AmendedOrder = Order.GetAmendedOrder(amendedOrder:=Order)
			Order = CType(Order.Save(), Order)

			AmendedOrder.IsAttachmentAdded = False
			AmendedOrder = CType(AmendedOrder.Save(), Order)

			Return Order

		Catch ex As SqlException
			Throw New ApplicationException(message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ex:=ex,
																										ModuleName:="Order")}",
										   innerException:=ex)

		Catch ex As Exception
			Throw New ApplicationException(message:=$"{_SQLExceptionHelper.UserFriendlyExceptionMessage(ex:=ex,
																										ModuleName:="Order",
																										UseAsException:=True)}",
										   innerException:=ex)
		End Try

	End Function

#End Region

#Region " DELETE Method(s) "

	<HttpDelete>
	Public Function DeleteValue(OrderID As Guid) As IHttpActionResult

		Try

			Dim mOrder As Order = Order.GetOrder(OrderID)

			mOrder.Delete()
			mOrder.Save()

			_ModuleHelper.SendEmailToBytzSoft(TransTypeID:=mOrder.TransTypeID,
											  Username:=User.Identity.Name,
											  ModuleFrom:="Order",
											  Action:="Delete",
											  ClientCode:=AppSettings("ClientCode"),
											  TransactionNo:=mOrder.OrderNo,
											  TransactionDate:=mOrder.OrderDateFormatted)

			Return Ok(New ReturnMessage("Success",
											   "Order Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Order",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

#Region " Report Method(s) "

	'Detail Report API
	<HttpGet>
	Public Function GetDetailReport(ID As Guid,
									Optional IsPROCUREMENTANDPAYMENTFORM As Boolean = False) As IHttpActionResult

		Try

			Return Ok(_ReportHelper.GetPODetailedReport(OrderID:=ID,
															   IsPROCUREMENTANDPAYMENTFORM:=IsPROCUREMENTANDPAYMENTFORM))

		Catch ex As Exception
			Return Content(HttpStatusCode.BadRequest, ex.Message)
		End Try

	End Function

#End Region

#Region " Send Email "

	<HttpPost>
	Public Function SendEmail(<FromBody> requestBody As EmailRequest) As IHttpActionResult

		Try

			If String.IsNullOrWhiteSpace(requestBody.OrderID) Then
				Return BadRequest("Order ID is required.")
			End If

			If String.IsNullOrWhiteSpace(requestBody.ToMailID) Then
				Return BadRequest("To Email Address is required.")
			End If

			Dim response As ReturnMessage = _EmailHelper.SendEmail(ModuleName:="Order",
																   Text:=requestBody.Text,
																   Info:=requestBody.Info,
																   RegNo:=requestBody.RegNo,
																   Remark:=requestBody.Remark,
																   OrderID:=requestBody.OrderID,
																   Subject:=requestBody.Subject,
																   MailBody:=requestBody.MailBody,
																   ToMailID:=requestBody.ToMailID,
																   CCMailID:=requestBody.CCMailID,
																   AttachmentName:=requestBody.Text,
																   BCCMailID:=requestBody.BCCMailID,
																   FromAudit:=requestBody.FromAudit,
																   ClientCode:=requestBody.ClientCode,
																   TransTypeID:=requestBody.TransTypeID,
																   AttachedFile:=requestBody.AttachedFile,
																   ReportByMail:=requestBody.ReportByMail,
																   VendorEmailID:=requestBody.VendorEmailID,
																   ShowCompanyName:=requestBody.ShowCompanyName,
																   ReportGeneratedBy:=requestBody.ReportGeneratedBy,
																   MultipleAttachment:=requestBody.MultipleAttachment,
																   IsMailForLockedUser:=requestBody.IsMailForLockedUser,
																   MailBodyForLockedUser:=requestBody.MailBodyForLockedUser)

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

#Region " Other Method(s) "

	<HttpPost>
	Public Function MethodAutoIssueCreation(OrderID As Guid) As String

		Dim mOrder As Order = Order.GetOrder(OrderID)
		Dim IssueDetail As String
		Dim NumberOfIssusDetails As New StringBuilder
		Dim StoreWiseItem = (From c In mOrder.OrderItems
							 Where c.EROQty <> 0
							 Group By StoreID = c.StoreID Into Group
							 Select New With {.StoreID = StoreID, .ReceiptItemCollection = Group})
		Dim variable

		Try

			' Create a collection to store issues details
			Dim issuesList As New List(Of Object)

			For Each variable In StoreWiseItem

				If Not variable.StoreID.Equals(Guid.Empty) Then

					Dim mIssue As Issue = Issue.NewIssue(Util.Trans.ExchangeRepairIssueToVendor)
					mIssue.IDate = mOrder.OrderDate
					mIssue.VendorID = mOrder.VendorID
					mIssue.StoreID = variable.StoreID
					mIssue.MachineID = Guid.Empty
					mIssue.ToStoreID = Guid.Empty
					mIssue.WorkShopID = Guid.Empty
					mIssue.nWOID = Guid.Empty
					mIssue.UserName = User.Identity.Name
					mIssue.StatusID = 2

					Dim receiptitemchildcol
					For Each receiptitemchildcol In variable.ReceiptItemCollection

						mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
						mIssue.IssueItems.CurrentItem.ReceiptItemID = receiptitemchildcol.ReceiptItemID
						mIssue.IssueItems.CurrentItem.DisplayQty = receiptitemchildcol.Qty
						mIssue.IssueItems.CurrentItem.OrderItemID = receiptitemchildcol.ID

					Next

					Try

						If mIssue.IsValid Then

							mIssue.Save()
							IssueDetail = "Issue : " + mIssue.IssueNo + " Dated : " + mIssue.IDateFormatted
							NumberOfIssusDetails.Append(IssueDetail)

							' Add issue details to issuesList
							issuesList.Add(New With {mIssue.IssueNo, .IssueDate = mIssue.IDateFormatted})

							MarkLog(Action.Save,
									"Issue",
									IssueDetail.Replace("<BR>", "") & " Authorized By: " & mOrder.AuthorizedBy,
									ErrorType.NoError,
									mIssue.ID, EventLogID)

						End If

					Catch ex As Exception
						' Handle exception if needed
					End Try

				End If

			Next

			'Create response object
			Dim responseObject As New With {.Issues = issuesList}
			Dim serializer As New JavaScriptSerializer()
			Dim jsonResponse As String = serializer.Serialize(responseObject)
			' Set the response content type to "application/json"
			HttpContext.Current.Response.ContentType = "application/json"

			' Write the JSON string to the response output
			HttpContext.Current.Response.Write(jsonResponse)
			HttpContext.Current.Response.End()

		Catch ex As Exception
			Throw ex
		End Try

	End Function

#End Region

End Class