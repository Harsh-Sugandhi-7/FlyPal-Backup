Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Services

Imports Newtonsoft.Json.Linq


Public Class IssueController
	Inherits ApiController

#Region " Variable Declaration "

	Private _MessageBox As New MSGBox
	Private _ReportHelper As New ReportHelper
	Private _EmailHelper As New EmailHelper
	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _CheckForSubscriptionExpired As New CheckForSubscriptionExpired

#End Region

#Region " Get Method(s) "

	<HttpGet>
	Public Function GetIssueTypeList(Optional IssueTo As Integer = 0) As IssueTypeList
		Try
			Return IssueTypeList.GetIssueTypeList(IssueTo)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetAlternateStockItemList(StoreID As Guid, Optional ItemName As String = "",
											  Optional ItemDesc As String = "",
											  Optional ItemCategory As String = "",
											  Optional ItemNomenclature As String = "",
											  Optional Store As String = "",
											  Optional IssueDate As String = "",
											  Optional TransTypeID As Trans = Util.Trans.None,
											  Optional ItemID As String = "{00000000-0000-0000-0000-000000000000}",
											  Optional AircraftID As String = "{00000000-0000-0000-0000-000000000000}",
											  Optional IsBERPart As Boolean = True,
											  Optional BarCodeNo As String = "",
											  Optional ToTypeIDOfIssue As Integer = 0) As AlternateStockItemList
		Try

			Return AlternateStockItemList.GetAlternateStockItemList(StoreID:=StoreID,
																	ItemName:=ItemName,
																	ItemDesc:=ItemDesc,
																	ItemCategory:=ItemCategory,
																	ItemNomenclature:=ItemNomenclature,
																	Store:=Store,
																	IssueDate:=IssueDate,
																	TransTypeID:=TransTypeID,
																	ItemID:=ItemID,
																	AircraftID:=AircraftID,
																	IsBERPart:=IsBERPart,
																	BarCodeNo:=BarCodeNo,
																	ToTypeIDOfIssue:=ToTypeIDOfIssue)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try
	End Function

	<HttpGet>
	Public Function GetPendingItemList(StoreID As Guid, Optional ItemName As String = "",
									   Optional IssueDate As String = "",
									   Optional TransTypeID As Trans = Util.Trans.None,
									   Optional IsBERPart As Boolean = True,
									   Optional GetRowCount As Boolean = False,
									   Optional IssueToDiscardAsExpired As Integer = 0,
									   Optional ItemPrimaryCategory As Integer = 0,
									   Optional CodeNo As String = "",
									   Optional CategoryID As String = "{00000000-0000-0000-0000-000000000000}") As PendingToIssueItemList
		Try

			Return PendingToIssueItemList.GetPendingItemList(StoreID,
															 ItemName,
															 IssueDate,
															 TransTypeID,
															 IsBERPart,
															 GetRowCount,
															 IssueToDiscardAsExpired,
															 ItemPrimaryCategory,
															 CodeNo,
															 CategoryID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/Issue/GetIssueList")>
	Public Function GetValues(Optional Text As String = "",
							  Optional No As Integer = 0,
							  Optional FromDate As String = "",
							  Optional ToDate As String = "",
							  Optional StoreName As String = "",
							  Optional VendorName As String = "",
							  Optional RegNo As String = "",
							  Optional IssueToType As Integer = 0,
							  Optional StatusID As Integer = 0,
							  Optional ReceiptText As String = "",
							  Optional ReceiptNo As Integer = 0,
							  Optional ReleaseNoteNo As String = "",
							  Optional SerialNo As String = "",
							  Optional ItemName As String = "",
							  Optional TransTypeID As Trans = Util.Trans.None,
							  Optional mIsVendor As Integer = 0,
							  Optional WorkShop As String = "",
							  Optional WOText As String = "",
							  Optional WONo As Integer = 0,
							  Optional IsForWO As Boolean = False,
							  Optional IsUnusedReturnItem As Boolean = False,
							  Optional CustomerName As String = "",
							  Optional IsCustomerName As Boolean = False,
							  Optional ReqText As String = "",
							  Optional ReqNo As Integer = 0,
							  Optional OrderText As String = "",
							  Optional OrderNo As Integer = 0,
							  Optional Amend As String = "",
							  Optional IsCustomPaging As Boolean = False,
							  Optional CurrentPage As Integer = 0,
							  Optional PageSize As Integer = 25,
							  Optional ToStoreName As String = "",
							  Optional BatchNo As String = "",
							  Optional IssueToEmpName As String = "",
							  Optional CategoryID As String = "{00000000-0000-0000-0000-000000000000}",
							  Optional Description As String = "",
							  Optional SearchText As String = "") As IssueList
		Try

			Return IssueList.GetIssueList(Text:=Text,
										  No:=No,
										  FromDate:=FromDate,
										  ToDate:=ToDate,
										  StoreName:=StoreName,
										  VendorName:=VendorName,
										  RegNo:=RegNo,
										  IssueToType:=IssueToType,
										  StatusID:=StatusID,
										  ReceiptText:=ReceiptText,
										  ReceiptNo:=ReceiptNo,
										  ReleaseNoteNo:=ReleaseNoteNo,
										  SerialNo:=SerialNo,
										  ItemName:=ItemName,
										  TransTypeID:=TransTypeID,
										  mIsVendor:=mIsVendor,
										  WorkShop:=WorkShop,
										  WOText:=WOText,
										  WONo:=WONo,
										  IsForWO:=IsForWO,
										  IsUnusedReturnItem:=IsUnusedReturnItem,
										  CustomerName:=CustomerName,
										  IsCustomerName:=IsCustomerName,
										  ReqText:=ReqText,
										  ReqNo:=ReqNo,
										  OrderText:=OrderText,
										  OrderNo:=OrderNo,
										  Amend:=Amend,
										  IsCustomPaging:=IsCustomPaging,
										  CurrentPage:=CurrentPage,
										  PageSize:=PageSize,
										  ToStoreName:=ToStoreName,
										  BatchNo:=BatchNo,
										  IssueToEmpName:=IssueToEmpName,
										  CategoryID:=CategoryID,
										  Description:=Description,
										  SearchText:=SearchText)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetPendingIssueList(Optional Text As String = "",
										Optional No As Integer = 0,
										Optional FromDate As String = "1/1/1800",
										Optional ToDate As String = "1/1/5500",
										Optional IssueToType As Integer = 0,
										Optional ReceiptText As String = "",
										Optional ReceiptNo As Integer = 0,
										Optional ReleaseNoteNo As String = "",
										Optional SerialNo As String = "",
										Optional ItemName As String = "",
										Optional ReceiptTransTypeID As Trans = Util.Trans.None,
										Optional FromID As String = "{00000000-0000-0000-0000-000000000000}",
										Optional IssueID As String = "{00000000-0000-0000-0000-000000000000}",
										Optional IsReturnableFromCustomer As Boolean = False,
										Optional IsCustomPaging As Boolean = False,
										Optional CurrentPage As Integer = 0,
										Optional PageSize As Integer = 25,
										Optional IssueToEmpName As String = "") As IssueList

		Return IssueList.GetPendingIssueList(Text:=Text, No:=No,
											 FromDate:=FromDate,
											 ToDate:=ToDate,
											 IssueToType:=IssueToType,
											 ReceiptText:=ReceiptText,
											 ReceiptNo:=ReceiptNo,
											 ReleaseNoteNo:=ReleaseNoteNo,
											 SerialNo:=SerialNo,
											 ItemName:=ItemName,
											 ReceiptTransTypeID:=ReceiptTransTypeID,
											 FromID:=FromID, IssueID:=IssueID,
											 IsReturnableFromCustomer:=IsReturnableFromCustomer,
											 IsCustomPaging:=IsCustomPaging,
											 CurrentPage:=CurrentPage,
											 PageSize:=PageSize,
											 IssueToEmpName:=IssueToEmpName)

	End Function

	<HttpGet>
	<Route("api/Issue/GetIssue")>
	Public Function GetValue(id As Guid) As Issue

		Return Issue.GetIssue(id)

	End Function

	<HttpGet>
	Public Function GetNewIssue(Optional TransTypeID As Trans = Util.Trans.None,
								Optional IsRequisitionTransaction As Boolean = False) As Issue
		Dim mIssue As Issue

		mIssue = Issue.NewIssue(TransTypeID:=TransTypeID,
								IsRequisitionTransaction:=IsRequisitionTransaction)
		mIssue.IDate = Today.Date

		Return mIssue

	End Function

	<HttpGet>
	Public Function GetNewIssueItem(IssueID As Guid,
									TransTypeID As Integer,
									Optional IsRequisitionTransaction As Boolean = False) As IssueItem

		Dim mIssue As Issue = Issue.NewIssue(TransTypeID:=TransTypeID,
											 IsRequisitionTransaction:=IsRequisitionTransaction)

		mIssue.IssueItems.Add(ID:=IssueID,
							  TransType:=TransTypeID)

		mIssue.IssueItems.CurrentItem.RequisitionItemIssueItems.Add(IssueItemID:=mIssue.IssueItems.CurrentItem.ID,
																	RequisitionItemID:=Guid.Empty,
																	Qty:=0D,
																	RequisitionNo:="")

		Return mIssue.IssueItems.CurrentItem

	End Function

	<HttpGet>
	Public Function GetNewIssueTerm(IssueID As Guid,
									TransTypeID As Integer,
									Optional IsRequisitionTransaction As Boolean = False) As IssueTerm

		Dim mIssue As Issue = Issue.NewIssue(TransTypeID:=TransTypeID,
											 IsRequisitionTransaction:=IsRequisitionTransaction)
		mIssue.IssueTerms.Add(ID:=IssueID)

		Return mIssue.IssueTerms.CurrentItem

	End Function

	<HttpGet>
	Public Function GetTypeList([Of] As String,
								TransTypeID As Integer) As TypeList1

		Try

			Return TypeList1.GetTypeList([Of]:=[Of],
										 TransTypeID:=TransTypeID)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

#End Region

#Region " Post Method(s) "

	<HttpPost>
	Public Function PostValue(<FromBody()> value As Object) As ReturnMessage

		Try

			Dim jsonObject As JObject = JObject.Parse(json:=value.ToString)

			Dim mIsNew As Boolean = CBool(jsonObject("mIsNew"))

			Dim ReturnString As String

			If mIsNew Then
				ReturnString = SetNewIssueValues(jsonObject:=jsonObject)
			Else
				ReturnString = SetExistingIssueValues(jsonObject:=jsonObject)
			End If

			If ReturnString = "Success" Then
				Return New ReturnMessage("Success", "Issue saved successfully!")
			Else
				Return New ReturnMessage("Error", ReturnString)
			End If

		Catch ex As Exception
			Return New ReturnMessage(Status:="Error", Message:=ex.Message)
		End Try

	End Function

	Private Function SetNewIssueValues(jsonObject As JObject) As String

		Try

			Dim ReturnMessage As String = _CheckForSubscriptionExpired.CheckForSubscriptionExpired(TransactionDate:=CDate(jsonObject(propertyName:="mDate").First.First))

			If ReturnMessage <> "Success" Then
				Return ReturnMessage
				Exit Function
			End If

			Dim mIssue As Issue = Issue.NewIssue(TransTypeID:=CInt(jsonObject("mTransTypeID")),
												 IsRequisitionTransaction:=CBool(jsonObject("mIsRequisitionTransaction")))

			Dim ItemArray As JArray = CType(jsonObject("mIssueItems"), JArray)
			Dim TermArray As JArray = CType(jsonObject("mIssueTerms"), JArray)

			Dim mDateFormatString As String = ""

			With mIssue
				.Text = jsonObject(propertyName:="mText")
				.No = CInt(jsonObject(propertyName:="mNo"))
				mDateFormatString = jsonObject(propertyName:="mDate")("mFormat")
				.IDate = CDate(jsonObject(propertyName:="mDate").First.First).ToString(format:=mDateFormatString)
				.StoreID = New Guid(jsonObject(propertyName:="mStoreID").ToString)
				.ToTypeID = CInt(jsonObject(propertyName:="mToTypeID"))
				.VendorID = New Guid(jsonObject(propertyName:="mVendorID").ToString)
				.MachineID = New Guid(jsonObject(propertyName:="mMachineID").ToString)
				.RegNo = jsonObject(propertyName:="mRegNo")
				.Person = jsonObject(propertyName:="mPerson")
				.Remark = jsonObject(propertyName:="mRemark")
				.UserName = jsonObject(propertyName:="mUserName")
				.AuthorizedBy = jsonObject(propertyName:="mAuthorizedBy")
				.StatusID = CInt(jsonObject(propertyName:="mStatusID"))
				.ToStoreID = New Guid(jsonObject(propertyName:="mToStoreID").ToString)
				.TransTypeID = CInt(jsonObject(propertyName:="mTransTypeID"))
				.WOID = New Guid(jsonObject(propertyName:="mWOID").ToString)
				.nWOID = New Guid(jsonObject(propertyName:="mnWOID").ToString)
				.WorkShopID = New Guid(jsonObject(propertyName:="mWorkShopID").ToString)
				.AWBNo = jsonObject(propertyName:="mAWBNo")
				.VoucherNo = jsonObject(propertyName:="mVoucherNo")
				.WorkShopName = (jsonObject(propertyName:="mWorkShopName"))
				.IsSync = CInt(jsonObject(propertyName:="mIsSync"))
				.BarcodeNo = jsonObject(propertyName:="mBarcodeNo")
				.RequisitionID = New Guid(jsonObject(propertyName:="mRequisitionID").ToString)
				.ReferenceNo = jsonObject(propertyName:="mReferenceNo")
				.ToolsIssuedToEmployeeID = New Guid(jsonObject(propertyName:="mToolsIssuedToEmployeeID").ToString)
				.ToolsIssuedToEmployeeName = jsonObject(propertyName:="mToolsIssuedToEmployeeName")
				.ToolsReceivedByEmployeeID = New Guid(jsonObject(propertyName:="mToolsReceivedByEmployeeID").ToString)
				.ToolsReceivedByEmployeeName = jsonObject(propertyName:="mToolsReceivedByEmployeeName")
				.ToolsCollectedByEmployeeID = New Guid(jsonObject(propertyName:="mToolsCollectedByEmployeeID").ToString)
				.ToolsCollectedByEmployeeName = jsonObject(propertyName:="mToolsCollectedByEmployeeName")
				.ReqTextNo = jsonObject(propertyName:="mReqTextNo")
				.ReqDate = CDate(jsonObject(propertyName:="mReqDate").First.First).ToString(format:=mDateFormatString)
				.ReqEmployeeID = New Guid(jsonObject(propertyName:="mReqEmployeeID").ToString)
				.ReqEmployeeName = jsonObject(propertyName:="mReqEmployeeName")
				.IssueTo = jsonObject(propertyName:="mIssueTo")
				.IsRequisitionTransaction = CBool(jsonObject(propertyName:="mIsRequisitionTransaction"))
			End With

			For i As Integer = 0 To ItemArray.Count - 1
				mIssue.IssueItems.Add(ID:=mIssue.ID, TransType:=mIssue.TransTypeID)
				With mIssue.IssueItems.CurrentItem
					.IssueID = New Guid(ItemArray(i)("mIssueID").ToString)
					.SRNo = CInt(ItemArray(i)("mSrNo"))
					.ItemID = New Guid(ItemArray(i)("mItemID").ToString)
					.ReceiptItemID = New Guid(ItemArray(i)("mReceiptItemID").ToString)
					.Qty = CDec(ItemArray(i)("mQty"))
					.nWOPendingQty = CDec(ItemArray(i)("mnWOPendingQty"))
					.DisplayQty = CDec(ItemArray(i)("mDisplayQty"))
					.DisplayUnitID = New Guid(ItemArray(i)("mDisplayUnitID").ToString)
					.Returnable = CBool(ItemArray(i)("mReturnable"))
					.Remark = ItemArray(i)("mRemark")
					.Note = ItemArray(i)("mNote")
					.ReceiptBalanceQty = CDec(ItemArray(i)("mReceiptBalanceQty"))
					.InvoiceBalanceQty = CDec(ItemArray(i)("mInvoiceBalanceQty"))
					.LoanQty = CDec(ItemArray(i)("mLoanQty"))
					.LoanReceiptItemID = New Guid(ItemArray(i)("mLoanReceiptItemID").ToString)
					.WOReqPartID = New Guid(ItemArray(i)("mWOReqPartID").ToString)
					.WOReturnDate = CDate(ItemArray(i)("mWOReturnDate").First.First).ToString(format:=mDateFormatString)
					.WOReturnQty = CDec(ItemArray(i)("mWOReturnQty"))
					.SalesOrderItemID = New Guid(ItemArray(i)("mSalesOrderItemID").ToString)
					.OrderItemID = New Guid(ItemArray(i)("mOrderItemID").ToString)
					.RequisitionItemID = New Guid(ItemArray(i)("mRequisitionItemID").ToString)
					.KitItemID = New Guid(ItemArray(i)("mKitItemID").ToString)
					.OutGoingReleaseNoteNo = ItemArray(i)("mOutGoingReleaseNoteNo")
					.RequisitionItemTypeID = CInt(ItemArray(i)("mRequisitionItemTypeID"))
					.RequisitionItemTypeName = ItemArray(i)("mRequisitionItemTypeName")
					.DisplayUnitName = ItemArray(i)("mDisplayUnitName")
					.WOUsedQty = CDec(ItemArray(i)("mWOUsedQty"))
					.DiscardAmt = CDec(ItemArray(i)("mDiscardAmt"))
					.IsReturnableFromAircraft = CBool(ItemArray(i)("mIsReturnableFromAircraft"))
					.PendingExportInvoiceQty = CDec(ItemArray(i)("mPendingExportInvoiceQty"))
					.RemovalReceiptItemID = New Guid(ItemArray(i)("mRemovalReceiptItemID").ToString)
					.IsCapitalize = CBool(ItemArray(i)("mIsCapitalize"))
					.PrimaryCategoryID = CInt(ItemArray(i)("mPrimaryCategoryID"))
					.BarcodeNo = ItemArray(i)("mBarcodeNo")
					.ItemTagID = CInt(ItemArray(i)("mItemTagID"))
					.ItemTagName = ItemArray(i)("mItemTagName")
					.StatusKit = CBool(ItemArray(i)("mStatusKit"))
					.CalibrationDueDate = CDate(ItemArray(i)("mCalibrationDueDate").First.First).ToString(format:=mDateFormatString)
					.CodeNo = ItemArray(i)("mCodeNo")
					.CountOf = CDec(ItemArray(i)("mCountOf"))
					.TotalConsumableAndExpendableUsedQty = CDec(ItemArray(i)("mTotalConsumableAndExpendableUsedQty"))
					.BaseUnitID = New Guid(ItemArray(i)("mBaseUnitID").ToString)
					.Location = ItemArray(i)("mLocation")
					.ManufacturingDate = CDate(ItemArray(i)("mManufacturingDate").First.First).ToString(format:=mDateFormatString)
					.IsAsPerAllocation = CBool(ItemArray(i)("mIsAsPerAllocation"))

					If ((mIssue.TransTypeID = 14 Or mIssue.TransTypeID = 44) And mIssue.ToTypeID = 18) Then

						Dim RequisitionItemIssueItemsArray As JArray = CType(ItemArray(i)("mRequisitionItemIssueItems"), JArray)
						mIssue.IssueItems.CurrentItem.RequisitionItemIssueItems.Add(IssueItemID:=mIssue.IssueItems.CurrentItem.ID,
														   RequisitionItemID:=New Guid(RequisitionItemIssueItemsArray(0)("mReqItemID").ToString),
														   Qty:=CDbl(RequisitionItemIssueItemsArray(0)("mQty").ToString),
														   RequisitionNo:=RequisitionItemIssueItemsArray(0)("mRequisitionNo"))
					End If

				End With
			Next

			For j As Integer = 0 To TermArray.Count - 1

				mIssue.IssueTerms.Add(mIssue.ID)

				With mIssue.IssueTerms.CurrentItem
					.SRNo = CInt(TermArray(j)("mSrNo"))
					.IssueID = New Guid(TermArray(j)("mIssueID").ToString)
					.TermID = New Guid((TermArray(j)("mTermID").ToString))
					.Terms = TermArray(j)("mTerms")
				End With
			Next

			mIssue.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingIssueValues(jsonObject As JObject) As String

		Try

			Dim mIssue As Issue = Issue.GetIssue(ID:=New Guid(jsonObject("mID").ToString))

			Dim ItemArray As JArray = CType(jsonObject("mIssueItems"), JArray)
			Dim TermArray As JArray = CType(jsonObject("mIssueTerms"), JArray)

			Dim mDateFormatString As String = ""

			With mIssue
				.Text = jsonObject(propertyName:="mText")
				.No = CInt(jsonObject(propertyName:="mNo"))
				mDateFormatString = jsonObject(propertyName:="mDate")("mFormat")
				.IDate = CDate(jsonObject(propertyName:="mDate").First.First).ToString(format:=mDateFormatString)
				.StoreID = New Guid(jsonObject(propertyName:="mStoreID").ToString)
				.ToTypeID = CInt(jsonObject(propertyName:="mToTypeID"))
				.VendorID = New Guid(jsonObject(propertyName:="mVendorID").ToString)
				.MachineID = New Guid(jsonObject(propertyName:="mMachineID").ToString)
				.RegNo = jsonObject(propertyName:="mRegNo")
				.Person = jsonObject(propertyName:="mPerson")
				.Remark = jsonObject(propertyName:="mRemark")
				.UserName = jsonObject(propertyName:="mUserName")
				.AuthorizedBy = jsonObject(propertyName:="mAuthorizedBy")
				.StatusID = CInt(jsonObject(propertyName:="mStatusID"))
				.ToStoreID = New Guid(jsonObject(propertyName:="mToStoreID").ToString)
				.TransTypeID = CInt(jsonObject(propertyName:="mTransTypeID"))
				.WOID = New Guid(jsonObject(propertyName:="mWOID").ToString)
				.nWOID = New Guid(jsonObject(propertyName:="mnWOID").ToString)
				.WorkShopID = New Guid(jsonObject(propertyName:="mWorkShopID").ToString)
				.AWBNo = jsonObject(propertyName:="mAWBNo")
				.VoucherNo = jsonObject(propertyName:="mVoucherNo")
				.WorkShopName = (jsonObject(propertyName:="mWorkShopName"))
				.IsSync = CInt(jsonObject(propertyName:="mIsSync"))
				.BarcodeNo = jsonObject(propertyName:="mBarcodeNo")
				.RequisitionID = New Guid(jsonObject(propertyName:="mRequisitionID").ToString)
				.ReferenceNo = jsonObject(propertyName:="mReferenceNo")
				.ToolsIssuedToEmployeeID = New Guid(jsonObject(propertyName:="mToolsIssuedToEmployeeID").ToString)
				.ToolsIssuedToEmployeeName = jsonObject(propertyName:="mToolsIssuedToEmployeeName")
				.ToolsReceivedByEmployeeID = New Guid(jsonObject(propertyName:="mToolsReceivedByEmployeeID").ToString)
				.ToolsReceivedByEmployeeName = jsonObject(propertyName:="mToolsReceivedByEmployeeName")
				.ToolsCollectedByEmployeeID = New Guid(jsonObject(propertyName:="mToolsCollectedByEmployeeID").ToString)
				.ToolsCollectedByEmployeeName = jsonObject(propertyName:="mToolsCollectedByEmployeeName")
				.ReqTextNo = jsonObject(propertyName:="mReqTextNo")
				.ReqDate = CDate(jsonObject(propertyName:="mReqDate").First.First).ToString(format:=mDateFormatString)
				.ReqEmployeeID = New Guid(jsonObject(propertyName:="mReqEmployeeID").ToString)
				.ReqEmployeeName = jsonObject(propertyName:="mReqEmployeeName")
				.IssueTo = jsonObject(propertyName:="mIssueTo")
				.IsRequisitionTransaction = CBool(jsonObject(propertyName:="mIsRequisitionTransaction"))
			End With

			For i As Integer = 0 To ItemArray.Count - 1

				Dim mID As Guid = New Guid(ItemArray(i)("mID").ToString)

				Dim mIsNew As Boolean = CBool(ItemArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(ItemArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(ItemArray(i)("mIsDirty"))

				Dim mIssueItem As IssueItem

				If mIsNew Then
					mIssue.IssueItems.Add(mIssue.ID, mIssue.TransTypeID)
					mIssueItem = mIssue.IssueItems.CurrentItem
				Else
					mIssueItem = mIssue.IssueItems(mID)
				End If


				If mIsDeleted Then
					mIssue.IssueItems.Remove(mIssueItem)
				End If

				If mIsNew Or mIsDirty Then

					With mIssueItem

						.IssueID = New Guid(ItemArray(i)("mIssueID").ToString)
						.SRNo = CInt(ItemArray(i)("mSrNo"))
						.ItemID = New Guid(ItemArray(i)("mItemID").ToString)
						.ReceiptItemID = New Guid(ItemArray(i)("mReceiptItemID").ToString)
						.Qty = CDec(ItemArray(i)("mQty"))
						.nWOPendingQty = CDec(ItemArray(i)("mnWOPendingQty"))
						.DisplayQty = CDec(ItemArray(i)("mDisplayQty"))
						.DisplayUnitID = New Guid(ItemArray(i)("mDisplayUnitID").ToString)
						.Returnable = CBool(ItemArray(i)("mReturnable"))
						.Remark = ItemArray(i)("mRemark")
						.Note = ItemArray(i)("mNote")
						.ReceiptBalanceQty = CDec(ItemArray(i)("mReceiptBalanceQty"))
						.InvoiceBalanceQty = CDec(ItemArray(i)("mInvoiceBalanceQty"))
						.LoanQty = CDec(ItemArray(i)("mLoanQty"))
						.LoanReceiptItemID = New Guid(ItemArray(i)("mLoanReceiptItemID").ToString)
						.WOReqPartID = New Guid(ItemArray(i)("mWOReqPartID").ToString)
						.WOReturnDate = CDate(ItemArray(i)("mWOReturnDate").First.First).ToString(format:=mDateFormatString)
						.WOReturnQty = CDec(ItemArray(i)("mWOReturnQty"))
						.SalesOrderItemID = New Guid(ItemArray(i)("mSalesOrderItemID").ToString)
						.OrderItemID = New Guid(ItemArray(i)("mOrderItemID").ToString)
						.RequisitionItemID = New Guid(ItemArray(i)("mRequisitionItemID").ToString)
						.KitItemID = New Guid(ItemArray(i)("mKitItemID").ToString)
						.OutGoingReleaseNoteNo = ItemArray(i)("mOutGoingReleaseNoteNo")
						.RequisitionItemTypeID = CInt(ItemArray(i)("mRequisitionItemTypeID"))
						.RequisitionItemTypeName = ItemArray(i)("mRequisitionItemTypeName")
						.DisplayUnitName = ItemArray(i)("mDisplayUnitName")
						.WOUsedQty = CDec(ItemArray(i)("mWOUsedQty"))
						.DiscardAmt = CDec(ItemArray(i)("mDiscardAmt"))
						.IsReturnableFromAircraft = CBool(ItemArray(i)("mIsReturnableFromAircraft"))
						.PendingExportInvoiceQty = CDec(ItemArray(i)("mPendingExportInvoiceQty"))
						.RemovalReceiptItemID = New Guid(ItemArray(i)("mRemovalReceiptItemID").ToString)
						.IsCapitalize = CBool(ItemArray(i)("mIsCapitalize"))
						.PrimaryCategoryID = CInt(ItemArray(i)("mPrimaryCategoryID"))
						.BarcodeNo = ItemArray(i)("mBarcodeNo")
						.ItemTagID = CInt(ItemArray(i)("mItemTagID"))
						.ItemTagName = ItemArray(i)("mItemTagName")
						.StatusKit = CBool(ItemArray(i)("mStatusKit"))
						.CalibrationDueDate = CDate(ItemArray(i)("mCalibrationDueDate").First.First).ToString(format:=mDateFormatString)
						.CodeNo = ItemArray(i)("mCodeNo")
						.CountOf = CDec(ItemArray(i)("mCountOf"))
						.TotalConsumableAndExpendableUsedQty = CDec(ItemArray(i)("mTotalConsumableAndExpendableUsedQty"))
						.BaseUnitID = New Guid(ItemArray(i)("mBaseUnitID").ToString)
						.Location = ItemArray(i)("mLocation")
						.ManufacturingDate = CDate(ItemArray(i)("mManufacturingDate").First.First).ToString(format:=mDateFormatString)
						.IsAsPerAllocation = CBool(ItemArray(i)("mIsAsPerAllocation"))


						If ((mIssue.TransTypeID = 14 Or mIssue.TransTypeID = 44) And mIssue.ToTypeID = 18) Then
							Dim RequisitionItemIssueItemsArray As JArray = CType(ItemArray(i)("mRequisitionItemIssueItems"), JArray)

							Dim mRequisitionItemIssueItem As RequisitionItemIssueItem

							If CBool(RequisitionItemIssueItemsArray(0)("mIsNew")) = True Then

								mIssueItem.RequisitionItemIssueItems.Add(IssueItemID:=mIssueItem.ID,
																							RequisitionItemID:=New Guid(RequisitionItemIssueItemsArray(0)("mReqItemID").ToString),
																							Qty:=CDbl(RequisitionItemIssueItemsArray(0)("mQty").ToString),
																							RequisitionNo:=RequisitionItemIssueItemsArray(0)("mRequisitionNo"))

							Else
								mRequisitionItemIssueItem = mIssueItem.RequisitionItemIssueItems(New Guid(RequisitionItemIssueItemsArray(0)("mID").ToString))

								If CBool(RequisitionItemIssueItemsArray(0)("mIsDirty")) = True Then

									With mRequisitionItemIssueItem
										.IssueItemID = mIssueItem.ID
										.ReqItemID = New Guid(RequisitionItemIssueItemsArray(0)("mReqItemID").ToString)
										.Qty = CDbl(RequisitionItemIssueItemsArray(0)("mQty").ToString)
										.RequisitionDate = CDate(RequisitionItemIssueItemsArray(0)("mRequisitionDate").First.First).ToString(format:=mDateFormatString)
									End With
								End If

								If mRequisitionItemIssueItem.IsDeleted Then
									mIssueItem.RequisitionItemIssueItems.Remove(mRequisitionItemIssueItem)
								End If

							End If
						End If

					End With

				End If

			Next

			For i As Integer = 0 To TermArray.Count - 1

				Dim mID As Guid = New Guid(TermArray(i)("mID").ToString)

				Dim mIsNew As Boolean = CBool(TermArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(TermArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(TermArray(i)("mIsDirty"))

				Dim mIssueTerm As IssueTerm

				If mIsNew Then
					mIssue.IssueTerms.Add(mIssue.ID)
					mIssueTerm = mIssue.IssueTerms.CurrentItem
				Else
					mIssueTerm = mIssue.IssueTerms(mID)
				End If


				If mIsDeleted Then
					mIssue.IssueTerms.Remove(mIssueTerm)
				End If

				If mIsNew Or mIsDirty Then

					With mIssueTerm
						.SRNo = CInt(TermArray(i)("mSrNo"))
						.IssueID = New Guid(TermArray(i)("mIssueID").ToString)
						.TermID = New Guid((TermArray(i)("mTermID").ToString))
						.Terms = TermArray(i)("mTerms")
					End With

				End If

			Next

			mIssue.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " Report Method(s) "

	<HttpPost>
	<Route("api/Issue/DisplayDetailedReport")>
	Public Function GetDetailReport(<FromBody()> requestBody As JObject) As IHttpActionResult

		If requestBody Is Nothing Then
			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage(Status:="Error",
												   Message:="Request body cannot be null."))
		End If
		Try
			Dim id As Guid = requestBody("Id")
			Dim result = _ReportHelper.GetIssueDetailedReport(id, True)
			If result.Item2.ToString = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Report displayed Successfully!!",
												   ReportData:=result.Item1))
			Else
				Return Content(HttpStatusCode.BadRequest,
							   New ReturnMessage(Status:="Error",
													   Message:="Error occurred while displaying report.",
													   ReportData:=result.Item1))
			End If
		Catch ex As Exception
			Return Content(HttpStatusCode.BadRequest, ex.Message)
		End Try

	End Function

	<HttpPost>
	<Route("api/Issue/DisplayListReport")>
	Public Function GetListReport(<FromBody()> requestBody As IssueListReportRequest) As IHttpActionResult
		If requestBody Is Nothing Then
			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage(Status:="Error",
												   Message:="Request body cannot be null."))
		End If
		Try
			Dim columnHeaders() As String = requestBody.ColumnHeaders
			Dim IssueList As IssueList = IssueList.GetIssueList(Text:=requestBody.Text, No:=requestBody.No,
																FromDate:=requestBody.FromDate, ToDate:=requestBody.ToDate,
																StoreName:=requestBody.StoreName, VendorName:=requestBody.VendorName,
																RegNo:=requestBody.RegNo, IssueToType:=requestBody.IssueToType,
																StatusID:=requestBody.StatusID, ReceiptText:=requestBody.ReceiptText,
																ReceiptNo:=requestBody.ReceiptNo, ReleaseNoteNo:=requestBody.ReleaseNoteNo,
																SerialNo:=requestBody.SerialNo, ItemName:=requestBody.ItemName,
																TransTypeID:=requestBody.TransTypeID, mIsVendor:=requestBody.mIsVendor,
																WorkShop:=requestBody.WorkShop,
																WOText:=requestBody.WOText, WONo:=requestBody.WONo,
																IsForWO:=requestBody.IsForWO,
																IsUnusedReturnItem:=requestBody.IsUnusedReturnItem,
																CustomerName:=requestBody.CustomerName,
																IsCustomerName:=requestBody.IsCustomerName,
																ReqText:=requestBody.ReqText, ReqNo:=requestBody.ReqNo,
																OrderText:=requestBody.OrderText, OrderNo:=requestBody.OrderNo,
																Amend:=requestBody.Amend, IsCustomPaging:=requestBody.IsCustomPaging,
																CurrentPage:=requestBody.CurrentPage,
																PageSize:=requestBody.PageSize,
																ToStoreName:=requestBody.ToStoreName,
																BatchNo:=requestBody.BatchNo, IssueToEmpName:=requestBody.IssueToEmpName,
																CategoryID:=requestBody.CategoryID,
																Description:=requestBody.Description, SearchText:=requestBody.SearchText)
			Dim result = _ReportHelper.ListReport(List:=IssueList, ColumnHeaders:=columnHeaders,
													IsForAPI:=True, ReportOf:="IssueList")
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
			Return Content(HttpStatusCode.BadRequest, ex.Message)
		End Try

	End Function

#End Region

#Region " Delete Method(s) "

	<HttpDelete>
	Public Function DeleteValue(IssueID As Guid) As IHttpActionResult

		Try

			Dim mIssue As Issue = Issue.GetIssue(ID:=IssueID)
			mIssue.Delete()
			mIssue.Save()

			Return Ok(New ReturnMessage("Success", "Issue deleted successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Issue",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))

		End Try

	End Function

#End Region

#Region " Email Method(s) "

	<HttpPost>
	<Route("api/Issue/SendEmail")>
	Public Function SendEmail(<FromBody()> requestBody As JObject) As IHttpActionResult

		Try

			Dim ToMailID As String = IIf(CStr(requestBody("ToMailID")) IsNot Nothing, CStr(requestBody("ToMailID")), "")
			Dim CCMailID As String = IIf(CStr(requestBody("CCMailID")) IsNot Nothing, CStr(requestBody("CCMailID")), "")
			Dim BCCMailID As String = IIf(CStr(requestBody("BCCMailID")) IsNot Nothing, CStr(requestBody("BCCMailID")), "")
			Dim TransTypeID As Integer = CInt(requestBody("TransTypeID"))
			Dim TempReportPath As String = ""
			Dim User As User = UserManagerController.FetchUser()
			Dim UserName = User.Name
			Dim ID As Guid = CType(requestBody("ID"), Guid)
			Dim AttachmentName As String = IIf(CStr(requestBody("AttachmentName")) IsNot Nothing, CStr(requestBody("AttachmentName")), "")
			Dim Remark As String = IIf(CStr(requestBody("Remark")) IsNot Nothing, CStr(requestBody("Remark")), "")
			Dim ReportGeneratedBy As String = IIf(CStr(requestBody("ReportGeneratedBy")) IsNot Nothing, CStr(requestBody("ReportGeneratedBy")), "")
			Dim Info As String = String.Empty
			Dim Subject As String = String.Empty
			Dim CompanyName As String = String.Empty
			Dim Array() As String

			Dim UserEmailDetails As TransactionList = TransactionList.GetTransactionList("Select")
			Dim SmtpHost = UserEmailDetails.Item(TransTypeID).SmtpHost
			Dim SmtpPort = UserEmailDetails.Item(TransTypeID).SmtpPort
			Dim SmtpUser = UserEmailDetails.Item(TransTypeID).SmtpUser
			Dim SmtpPassword = UserEmailDetails.Item(TransTypeID).SmtpPassword

			Dim result = _ReportHelper.GetIssueDetailedReport(ID, IsForAPI:=True, ByMail:=True)

			If result.Item2.ToString = "Success" Then
				TempReportPath = _EmailHelper.SaveReportToTempFile(ReportBytes:=result.Item1,
																   AttachmentName:=AttachmentName)
			End If

			Array = result.Item3.Split({", "}, StringSplitOptions.RemoveEmptyEntries)

			If Array.Length >= 1 Then
				Info = Array(0)
			End If

			If Array.Length >= 2 Then
				CompanyName = Array(1)
			End If

			Subject = $"{CompanyName}  {AttachmentName}"

			SendMailFile.SendMailFile(rpt:=Nothing,
									  UserName:=UserName,
									  Subject:=Subject,
									  Text:=AttachmentName,
									  Info:=Info,
									  ToMailID:=ToMailID,
									  CCMailID:=CCMailID,
									  ReportPath:=TempReportPath,
									  BCCMailID:=BCCMailID,
									  Remark:=Remark,
									  ReportGeneratedBy:=ReportGeneratedBy,
									  SmtpHost:=SmtpHost,
									  SmtpPort:=SmtpPort,
									  SmtpUser:=SmtpUser,
									  SmtpPassword:=SmtpPassword,
									  TransTypeID:=TransTypeID)

			Return Ok(New ReturnMessage("Success", "Email Sent Successfully!"))

		Catch ex As Exception
			Return Content(HttpStatusCode.BadRequest, ex.Message)
		End Try

	End Function

#End Region

#Region " Helper Method(s) "

	<HttpGet>
	<Route("api/Issue/CheckIfUserHasStoreRights")>
	Public Function CheckIfUserHasStoreRights(TransTypeID As Integer,
											  Optional ToStoreID As String = "{00000000-0000-0000-0000-000000000000}",
											  Optional ToStoreName As String = "",
											  Optional StoreID As String = "{00000000-0000-0000-0000-000000000000}",
											  Optional StoreName As String = "") As IHttpActionResult

		Dim UserHasNoStoreRights As UserHasNoStoreRights
		Dim ID As String = "{00000000-0000-0000-0000-000000000000}"
		Dim Name As String = ""

		Try

			If (TransTypeID = 8 Or TransTypeID = 12) Then
				ID = ToStoreID
				Name = ToStoreName
			ElseIf (TransTypeID = 13 Or TransTypeID = 27 Or TransTypeID = 28 Or TransTypeID = 47 Or TransTypeID = 62) Then
				ID = StoreID
				Name = StoreName
			End If

			UserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(UserName:=User.Identity.Name,
																				StoreID:=ID)

			If UserHasNoStoreRights.Count > 0 Then
				Return Ok(New ReturnMessage(Status:="Success",
												   Message:=$"We're sorry, but you do not have the necessary rights or permissions to access the store: {Name}.{Environment.NewLine} To gain access, please contact your Administrator."))
			End If

			Return Ok(New ReturnMessage("", $""))

		Catch ex As Exception

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Exception",
												   $"{ex.Message}"))

		End Try

	End Function

#End Region

End Class
