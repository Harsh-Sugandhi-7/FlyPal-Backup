Imports System.Net
Imports System.Web.Http

Imports Newtonsoft.Json.Linq

Public Class ItemController
	Inherits ApiController

#Region " Item "

#Region " Variable Declaration "

	Dim mDateFormatString As String = ""

	Private _SQLExceptionHelper As New SQLExceptionHelper
	Private _MessageBox As New MSGBox

#End Region

#Region " GET Method(s) "

	<HttpGet>
	Public Function GetValues(Optional ItemName As String = "",
							  Optional ItemDescription As String = "",
							  Optional UnitName As String = "",
							  Optional CategoryName As String = "",
							  Optional Location As String = "",
							  Optional SerializedStatus As Integer = -1) As ItemList

		Try

			Return ItemList.GetItemListOnListPage(ItemName:=ItemName,
												  ItemDescription:=ItemDescription,
												  UnitName:=UnitName,
												  CategoryName:=CategoryName,
												  Location:=Location,
												  SerializedStatus:=SerializedStatus)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetItemList(LookinType As Integer,
								Optional ItemName As String = "",
								Optional ItemDescription As String = "",
								Optional Nomenclature As String = "",
								Optional CategoryName As String = "",
								Optional UnitName As String = "",
								Optional Location As String = "",
								Optional IsSelectTagRequired As Boolean = False,
								Optional SerialNo As String = "",
								Optional IsCustomPaging As Boolean = False,
								Optional CurrentPage As Integer = 0,
								Optional PageSize As Integer = 25,
								Optional BatchNo As String = "",
								Optional ItemIDToSkipForKitItem As String = "00000000-0000-0000-0000-000000000000",
								Optional CodeNo As String = "",
								Optional PrimaryCategoryIDs As String = "") As ItemList

		Try

			Return ItemList.GetItemList(LookinType:=LookinType,
										ItemName:=ItemName,
										ItemDescription:=ItemDescription,
										Nomenclature:=Nomenclature,
										CategoryName:=CategoryName,
										UnitName:=UnitName,
										Location:=Location,
										IsSelectTagRequired:=IsSelectTagRequired,
										SerialNo:=SerialNo,
										IsCustomPaging:=IsCustomPaging,
										CurrentPage:=CurrentPage,
										PageSize:=PageSize,
										BatchNo:=BatchNo,
										ItemIDToSkipForKitItem:=ItemIDToSkipForKitItem,
										CodeNo:=CodeNo,
										PrimaryCategoryIDs:=PrimaryCategoryIDs)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetValue(ID As String) As Item

		Try

			Return Item.GetItem(ID:=New Guid(ID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	<Route("api/Item/RFQMultiplePartsList")>
	<Route("api/Item/GetItems")>
	Public Function GetItems(Optional LookInType As Integer = 1,
							 Optional ItemName As String = "",
							 Optional ItemDescription As String = "",
							 Optional Nomenclature As String = "",
							 Optional CategoryName As String = "",
							 Optional UnitName As String = "",
							 Optional Location As String = "",
							 Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
							 Optional IsCustomPaging As Boolean = False,
							 Optional CurrentPage As Integer = 0,
							 Optional PageSize As Integer = 25) As Items

		Try

			Return Items.GetItems(LookinType:=LookInType,
								  ItemName:=ItemName,
								  ItemDescription:=ItemDescription,
								  Nomenclature:=Nomenclature,
								  CategoryName:=CategoryName,
								  UnitName:=UnitName,
								  Location:=Location,
								  MachineID:=MachineID,
								  IsCustomPaging:=IsCustomPaging,
								  CurrentPage:=CurrentPage,
								  PageSize:=PageSize)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetItemByName(Optional ItemName As String = "") As FetchItemByName

		Try

			Return FetchItemByName.GetItemByName(ItemName:=ItemName)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetNewItem() As Item

		Try

			Return Item.NewItem()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'Get New Item Applicable 
	<HttpGet>
	Public Function GetNewItemApplicable() As ItemApplicable

		Try

			Dim mItem As Item = Item.NewItem()
			mItem.ItemApplicables.Add(ID:=mItem.ID)

			Return mItem.ItemApplicables.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'Get New Item Alternate PartNo 
	<HttpGet>
	Public Function GetNewAlternatePartNumber() As AlternatePartNumber

		Try

			Dim mItem As Item = Item.NewItem()
			mItem.AlternatePartNos.Add(AlternatePartID:=mItem.ID,
									   LinkID:=mItem.ID)

			Return mItem.AlternatePartNos.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'Get New Opening Balance
	<HttpGet>
	Public Function GetNewOpeningBalance() As ReceiptInvoice

		Try

			Dim mItem As Item = Item.NewItem()
			mItem.OpeningBalances.Add(ItemID:=mItem.ID,
									  Serialized:=mItem.SerialisedStatus,
									  BaseUnitID:=mItem.UnitID)

			Return mItem.OpeningBalances.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'Get New Item Service Inspection
	<HttpGet>
	Public Function GetNewItemServiceInspection() As ItemServiceInspections

		Try

			Dim mItem As Item = Item.NewItem()
			mItem.ItemServiceInspectionsList.Add(ID:=mItem.ID)
			Return mItem.ItemServiceInspectionsList.CurrentItem

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetItemServiceInspectionsList(Optional ItemID As String = "{00000000-0000-0000-0000-000000000000}") As ItemServiceInspectionsList

		Try

			Return ItemServiceInspectionsList.GetItemServiceInspectionsList(ItemID:=New Guid(ItemID))

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'GET Item Tag List
	<HttpGet>
	Public Function GetItemTagList(Optional IsSelectTagRequired As Boolean = False) As ItemTagList

		Try

			Return ItemTagList.GetItemTagList(IsSelectTagRequired:=IsSelectTagRequired)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'GET Item By ID
	<HttpGet>
	Public Function GetItemByID(ID As Guid) As ItemByID
		Try
			Return ItemByID.GetItemByID(ID:=ID)
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try
	End Function

#End Region

#Region " Methods "

	Public Sub SetItem(jsonObject As JObject, Optional mItem As Item = Nothing)

		Try

			With mItem

				mDateFormatString = jsonObject(propertyName:="mNotInUseDate")("mFormat")

				mItem.Name = jsonObject("mName").ToString()
				mItem.Description = jsonObject("mDescription").ToString()
				mItem.Location = jsonObject("mLocation").ToString()
				mItem.Rate = CDec(jsonObject("mRate").ToString())
				mItem.Folio = CInt((jsonObject("mFolio").ToString()))
				mItem.StatusKit = CBool(jsonObject("mStatusKit"))
				mItem.ExpiryMonths = CInt(jsonObject("mExpiryMonths").ToString())
				mItem.ExpiryQuaters = CInt(jsonObject("mExpiryQuaters").ToString())
				mItem.BenchmarkMonths = CInt(jsonObject("mBenchmarkMonths").ToString())
				mItem.SerialisedStatus = CBool(jsonObject("mSerialisedStatus"))
				mItem.StockStatus = CBool(jsonObject("mStockStatus"))
				mItem.ValuationStatus = CBool(jsonObject("mValuationStatus"))
				mItem.MinStockLevel = CInt(jsonObject("mMinStockLevel").ToString())
				mItem.Note = jsonObject("mNote").ToString().Trim()
				mItem.ABCID = CInt(jsonObject("mABCID"))
				mItem.AltTypeID = CInt(jsonObject("mAltTypeID"))
				mItem.NomenclatureID = New Guid(jsonObject("mNomenclatureID").ToString)
				mItem.NomenclatureName = jsonObject("mNomenclatureName").ToString()
				mItem.UnitID = New Guid(jsonObject("mUnitID").ToString())
				mItem.UnitName = jsonObject("mUnitName").ToString()
				mItem.CategoryID = New Guid(jsonObject("mCategoryID").ToString())
				mItem.StatusEquipment = CBool(jsonObject("mStatusEquipment"))
				mItem.Specification = jsonObject("mSpecification").ToString()
				mItem.Make = jsonObject("mMake").ToString()
				mItem.MaxNoOfUses = CInt(jsonObject("mMaxNoOfUses").ToString())
				mItem.NotInUseDate = CDate(jsonObject(propertyName:="mNotInUseDate").First.First).ToString(format:=mDateFormatString)
				mItem.NotInUse = CBool(jsonObject("mNotInUse"))
				mItem.ATAID = New Guid(jsonObject("mATAID").ToString)
				mItem.MaxStockLevel = CInt(jsonObject("mMaxStockLevel").ToString())
				mItem.IPCReference = jsonObject("mIPCReference").ToString().Trim()
				mItem.StorageLife = CInt(jsonObject("mStorageLife").ToString())
				mItem.IsOneTimePurchase = CBool(jsonObject("mIsOneTimePurchase"))

				If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
					mItem.IsConsiderForReOrder = IIf(mItem.IsOneTimePurchase, False, True)
				Else
					mItem.IsConsiderForReOrder = CBool(jsonObject("mIsConsiderForReOrder"))
				End If

				mItem.BinCardNumber = jsonObject("mBinCardNumber").ToString()
				mItem.CalibrationStandard = jsonObject("mCalibrationStandard").ToString().Trim()
				mItem.AMMCMMReference = jsonObject("mAMMCMMReference").ToString().Trim()
				mItem.CalibrationPeriodInID = CInt(jsonObject("mCalibrationPeriodInID").ToString)
				mItem.ItemTagID = CInt(jsonObject("mItemTagID").ToString)
				mItem.IsAirworthiCheck = CBool(jsonObject("mIsAirworthiCheck"))

				If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
					Dim maxMinDiff As Integer = CInt(jsonObject("mMaxStockLevel").ToString()) - CInt(jsonObject("mMinStockLevel").ToString())
					If maxMinDiff >= 0 Then
						jsonObject("mMinReOrderLevel") = maxMinDiff.ToString()
					End If
				End If

				mItem.MinReOrderLevel = CInt(jsonObject("mMinReOrderLevel").ToString())
				mItem.IsConditionCheck = CBool(jsonObject("mIsConditionCheck"))
				mItem.IsServicedInspected = CBool(jsonObject("mIsServicedInspected"))

				If mItem.IsConditionCheck Then
					mItem.ConditionCheckInterval = CInt(jsonObject("mConditionCheckInterval").ToString())
					mItem.ConditionCheckIntervalIn = jsonObject("mConditionCheckIntervalIn")
				End If

				If mItem.IsServicedInspected Then
					mItem.ServicedInspectedInterval = CInt(jsonObject("mServicedInspectedInterval").ToString())
					mItem.ServicedInspectedIntervalIn = CInt(jsonObject("mServicedInspectedIntervalIn").ToString)
				End If

				mItem.ToolTypeID = CInt(jsonObject("mToolTypeID").ToString)
				mItem.ManufacturerID = New Guid(jsonObject("mManufacturerID").ToString)
				mItem.HSNACSID = New Guid(jsonObject("mHSNACSID").ToString)
				mItem.LifeComponent = CBool(jsonObject("mLifeComponent"))
				mItem.ContractedVendorID = New Guid(jsonObject("mContractedVendorID").ToString)
				mItem.ManuallyUpdated = CBool(jsonObject("mManuallyUpdated"))
				mItem.IsExpiryItem = CBool(jsonObject("mIsExpiryItem"))
				mItem.ReOrderQty = CDec(jsonObject("mReOrderQty").ToString())
				mItem.EssentialcategoryID = CInt(jsonObject("mEssentialcategoryID").ToString())
				mItem.AsOnDate = CDate(jsonObject(propertyName:="mAsOnDate").First.First).ToString(format:=mDateFormatString)

			End With

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub SetItemApplicable(jsonObject As Object,
								 mItemApplicableArray As JArray, mItem As Item)

		Try

			For i As Integer = 0 To mItemApplicableArray.Count - 1

				mItem.ItemApplicables.Add(ID:=mItem.ID)

				With mItem.ItemApplicables.CurrentItem

					.SrNo = CType(mItemApplicableArray(index:=i)(key:="mSrNo"), Integer)
					.ModelID = New Guid(mItemApplicableArray(index:=i)("mModelID").ToString())
					.ModelName = mItemApplicableArray(index:=i)(key:="mModelName").ToString()
					.ModelType = mItemApplicableArray(index:=i)(key:="mModelType").ToString()
					.GroundSupportEquipment = CBool(mItemApplicableArray(index:=i)(key:="mGroundSupportEquipment").ToString())

				End With

			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub SetAlternatePartNos(jsonObject As Object,
								   mAlternatePartNosArray As JArray, mItem As Item)

		Try

			For j As Integer = 0 To mAlternatePartNosArray.Count - 1
				mItem.AlternatePartNos.Add(AlternatePartID:=New Guid(mAlternatePartNosArray(index:=j)(key:="mAlternatePartID").ToString()),
										   LinkID:=New Guid(jsonObject("mLinkID").ToString()))
				With mItem.AlternatePartNos.CurrentItem

					.PartName = mAlternatePartNosArray(index:=j)(key:="mPartName").ToString()
					.PartDescription = mAlternatePartNosArray(index:=j)(key:="mDescription").ToString()
					.IsSelected = CBool(mAlternatePartNosArray(index:=j)(key:="mIsSelected").ToString())
					.Type = CInt(mAlternatePartNosArray(index:=j)(key:="mType").ToString())
					.AltTypeName = mAlternatePartNosArray(index:=j)(key:="mAltTypeName").ToString()
					.IPCReference = mAlternatePartNosArray(index:=j)(key:="mIPCReference").ToString()
					.UnitID = New Guid(mAlternatePartNosArray(index:=j)(key:="mUnitID").ToString())
					.IsFirstPriorityPart = CBool(mAlternatePartNosArray(index:=j)(key:="mIsFirstPriorityPart").ToString())

				End With
			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub SetOpeningBalance(jsonObject As JObject,
								 mOpeningBalancesArray As JArray, mItem As Item)

		Try

			For k As Integer = 0 To mOpeningBalancesArray.Count - 1

				mItem.OpeningBalances.Add(ItemID:=mItem.ID, Serialized:=mItem.SerialisedStatus, BaseUnitID:=mItem.UnitID)

				With mItem.OpeningBalances.CurrentItem

					mDateFormatString = jsonObject(propertyName:="mAsOnDate")("mFormat")

					mItem.OpeningBalances.CurrentItem.InvoiceText = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mText").ToString()
					mItem.OpeningBalances.CurrentItem.InvoiceNo = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mNo").ToString())
					mItem.OpeningBalances.CurrentItem.COtherCharges = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mCOtherCharges"))
					mItem.OpeningBalances.CurrentItem.AsOnDate = CDate(mOpeningBalancesArray(k)("mAsOnDate").First.First).ToString(format:=mDateFormatString)
					mItem.OpeningBalances.CurrentItem.VendorInvoiceNo = mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mVendorInvoiceNo").ToString()
					mItem.OpeningBalances.CurrentItem.InvoiceDate = CDate(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceDate").First.First).ToString(format:=mDateFormatString)
					mItem.OpeningBalances.CurrentItem.VendorInvoiceDate = CDate(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mVendorInvoiceDate").First.First).ToString(format:=mDateFormatString)
					mItem.OpeningBalances.CurrentItem.CurrencyID = New Guid(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mCurrencyID").ToString())
					mItem.OpeningBalances.CurrentItem.ConversionFactor = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mConversionFactor").ToString())
					mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.BaseUnitID = mItem.UnitID
					mItem.OpeningBalances.CurrentItem.DisplayQty = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mDisplayQty"))
					mItem.OpeningBalances.CurrentItem.Qty = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mQty"))
					mItem.OpeningBalances.CurrentItem.CRate = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mCRate"))
					mItem.OpeningBalances.CurrentItem.LandingCost = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mCEffRate"))
					mItem.OpeningBalances.CurrentItem.ReleaseNoteNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mReleaseNoteNo")
					mItem.OpeningBalances.CurrentItem.ReleaseNoteDate = CDate(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mReleaseNoteDate").First.First).ToString(format:=mDateFormatString)
					mItem.OpeningBalances.CurrentItem.StartDate = CDate(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mStartDate").First.First).ToString(format:=mDateFormatString)
					mItem.OpeningBalances.CurrentItem.ExpiryDate = CDate(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mExpiryDate").First.First).ToString(format:=mDateFormatString)
					mItem.OpeningBalances.CurrentItem.ItemTypeID = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mItemTypeID"))
					mItem.OpeningBalances.CurrentItem.Returnable = CBool(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mReturnable"))
					mItem.OpeningBalances.CurrentItem.TypeID = CInt(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mTypeID"))

					If mItem.OpeningBalances.CurrentItem.TypeID = 1 Or mItem.OpeningBalances.CurrentItem.TypeID = 14 Then
						mItem.OpeningBalances.CurrentItem.VendorID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mVendorID").ToString())
					ElseIf mItem.OpeningBalances.CurrentItem.TypeID = 2 Then
						mItem.OpeningBalances.CurrentItem.MachineID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mMachineID").ToString())
					ElseIf mItem.OpeningBalances.CurrentItem.TypeID = 18 Then
						mItem.OpeningBalances.CurrentItem.FromStoreID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mStoreID").ToString())
					ElseIf mItem.OpeningBalances.CurrentItem.TypeID = 16 Then
						mItem.OpeningBalances.CurrentItem.WorkShopID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mWorkShopID").ToString())
					End If

					mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.StoreID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mStoreID").ToString())
					mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.StoreName = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mStoreName").ToString()
					mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.SerialNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mSerialNo").ToString()
					mItem.OpeningBalances.CurrentItem.Location = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mLocation").ToString()
					mItem.OpeningBalances.CurrentItem.Remark = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mRemark").ToString()
					mItem.OpeningBalances.CurrentItem.Note = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mNote").ToString()
					mItem.OpeningBalances.CurrentItem.CureQtrs = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mCureQtrs").ToString())
					mItem.OpeningBalances.CurrentItem.CureYear = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mCureYear").ToString())
					mItem.OpeningBalances.CurrentItem.ExpQtrs = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mExpQtrs").ToString())
					mItem.OpeningBalances.CurrentItem.ExpYear = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mExpYear").ToString())
					mItem.OpeningBalances.CurrentItem.BatchNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mBatchNo").ToString()
					mItem.OpeningBalances.CurrentItem.Receipt.IntReceiptNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mIntReceiptNo").ToString()
					mItem.OpeningBalances.CurrentItem.CCommercialRate = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mCCommercialRate"))
					mItem.OpeningBalances.CurrentItem.CalibrationDoneOnDate = CDate(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mCalibrationDoneOnDate").First.First).ToString(format:=mDateFormatString)
					mItem.OpeningBalances.CurrentItem.IsExpiryNA = CBool(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mIsExpiryNA"))
					mItem.OpeningBalances.CurrentItem.IsExpiryUnlimited = CBool(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mIsExpiryUnlimited"))
					mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.CodeNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mCodeNo")

				End With

			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub SetItemServiceInspectionsList(jsonObject As Object,
											 mItemServiceInspectionsListArray As JArray, mItem As Item)

		Try

			For l As Integer = 0 To mItemServiceInspectionsListArray.Count - 1

				mItem.ItemServiceInspectionsList.Add(ID:=mItem.ID)

				With mItem.ItemServiceInspectionsList.CurrentItem

					.ItemID = New Guid(mItemServiceInspectionsListArray(index:=l)(key:="mItemID").ToString())
					.Description = mItemServiceInspectionsListArray(index:=l)(key:="mDescription").ToString()
					.Frequency = CInt(mItemServiceInspectionsListArray(index:=l)(key:="mFrequency").ToString())
					.FrequencyPeriod = CInt(mItemServiceInspectionsListArray(index:=l)(key:="mFrequencyPeriod").ToString())
					.ServiceInspectionNameID = New Guid(mItemServiceInspectionsListArray(index:=l)(key:="mServiceInspectionNameID").ToString())

				End With

			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try


	End Sub

#End Region

#Region " POST Method(s) "

	<HttpPost>
	Public Function SaveItem(<FromBody()> value As Object) As IHttpActionResult

		Try

			Dim jsonObject As JObject = JObject.Parse(value.ToString())
			Dim mIsNew As Boolean = jsonObject("mIsNew").ToObject(Of Boolean)()
			Dim returnstring As String = ""

			If mIsNew Then
				returnstring = SetNewItem(jsonObject)
			Else
				returnstring = SetExistingItem(jsonObject)
			End If

			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Item Saved Successfully!"))

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

	Private Function SetNewItem(jsonObject As JObject) As String

		Try

			Dim mItem As Item = Item.NewItem(ID:=New Guid(jsonObject("mID").ToString()))
			Dim mItemApplicableArray As JArray = CType(jsonObject(propertyName:="mItemApplicables"), JArray)
			Dim mAlternatePartNosArray As JArray = CType(jsonObject(propertyName:="mAlternatePartNos"), JArray)
			Dim mOpeningBalancesArray As JArray = CType(jsonObject(propertyName:="mOpeningBalances"), JArray)
			Dim mItemServiceInspectionsListArray As JArray = CType(jsonObject(propertyName:="mItemServiceInspectionsList"), JArray)

			SetItem(jsonObject, mItem)
			SetItemApplicable(jsonObject, mItemApplicableArray, mItem)
			SetAlternatePartNos(jsonObject, mAlternatePartNosArray, mItem)
			SetOpeningBalance(jsonObject, mOpeningBalancesArray, mItem)
			SetItemServiceInspectionsList(jsonObject, mItemServiceInspectionsListArray, mItem)

			mItem.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Item",
																						   ex:=ex)
			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingItem(jsonObject As JObject) As String

		Try

			Dim mItem As Item = Item.GetItem(ID:=New Guid(jsonObject("mID").ToString()))
			Dim mItemApplicableArray As JArray = CType(jsonObject(propertyName:="mItemApplicables"), JArray)
			Dim mAlternatePartNosArray As JArray = CType(jsonObject(propertyName:="mAlternatePartNos"), JArray)
			Dim mOpeningBalancesArray As JArray = CType(jsonObject(propertyName:="mOpeningBalances"), JArray)
			Dim mItemServiceInspectionsListArray As JArray = CType(jsonObject(propertyName:="mItemServiceInspectionsList"), JArray)

			SetItem(jsonObject, mItem)

			For i As Integer = 0 To mItemApplicableArray.Count - 1

				Dim mID As Guid = New Guid(mItemApplicableArray(i)("mID").ToString)
				Dim mIsNew As Boolean = CBool(mItemApplicableArray(i)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(mItemApplicableArray(i)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(mItemApplicableArray(i)("mIsDirty"))
				Dim mItemApplicable As ItemApplicable

				If mIsNew Then
					mItem.ItemApplicables.Add(mItem.ID)
					mItemApplicable = mItem.ItemApplicables.CurrentItem
				Else
					mItemApplicable = mItem.ItemApplicables(ID:=mID)
				End If

				If mIsDeleted Then
					mItem.ItemApplicables.Remove(mItemApplicable)
				End If

				If mIsNew Or mIsDirty Then

					With mItemApplicable

						.SrNo = CType(mItemApplicableArray(index:=i)(key:="mSrNo"), Integer)
						.ModelID = New Guid(mItemApplicableArray(index:=i)("mModelID").ToString())
						.ModelName = mItemApplicableArray(index:=i)(key:="mModelName").ToString()
						.ModelType = mItemApplicableArray(index:=i)(key:="mModelType").ToString()
						.GroundSupportEquipment = CBool(mItemApplicableArray(index:=i)(key:="mGroundSupportEquipment").ToString())

					End With

				End If

			Next

			For j As Integer = 0 To mAlternatePartNosArray.Count - 1

				Dim mAlternatePartID As Guid = New Guid(mAlternatePartNosArray(index:=j)(key:="mAlternatePartID").ToString())
				Dim mIsNew As Boolean = CBool(mAlternatePartNosArray(j)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(mAlternatePartNosArray(j)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(mAlternatePartNosArray(j)("mIsDirty"))
				Dim mAlternatePartNumber As AlternatePartNumber

				If mIsNew Then
					mItem.AlternatePartNos.Add(AlternatePartID:=mAlternatePartID, LinkID:=mItem.ID)
					mAlternatePartNumber = mItem.AlternatePartNos.CurrentItem
				Else
					mAlternatePartNumber = mItem.AlternatePartNos(AlternatePartID:=mAlternatePartID)
				End If

				If mIsDeleted Then
					mItem.AlternatePartNos.Remove(mAlternatePartNumber)
				End If

				If mIsNew Or mIsDirty Then

					With mAlternatePartNumber

						.PartName = mAlternatePartNosArray(index:=j)(key:="mPartName").ToString()
						.PartDescription = mAlternatePartNosArray(index:=j)(key:="mDescription").ToString()
						.IsSelected = CBool(mAlternatePartNosArray(index:=j)(key:="mIsSelected").ToString())
						.Type = CInt(mAlternatePartNosArray(index:=j)(key:="mType").ToString())
						.AltTypeName = mAlternatePartNosArray(index:=j)(key:="mAltTypeName").ToString()
						.IPCReference = mAlternatePartNosArray(index:=j)(key:="mIPCReference").ToString()
						.UnitID = New Guid(mAlternatePartNosArray(index:=j)(key:="mUnitID").ToString())
						.IsFirstPriorityPart = CBool(mAlternatePartNosArray(index:=j)(key:="mIsFirstPriorityPart").ToString())

					End With

				End If

			Next

			For k As Integer = 0 To mOpeningBalancesArray.Count - 1

				Dim mIsNew As Boolean = CBool(mOpeningBalancesArray(k)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(mOpeningBalancesArray(k)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(mOpeningBalancesArray(k)("mIsDirty"))
				Dim mReceiptInvoice As ReceiptInvoice

				If mIsNew Then
					mItem.OpeningBalances.Add(ItemID:=mItem.ID, Serialized:=mItem.SerialisedStatus, BaseUnitID:=mItem.UnitID)
					mReceiptInvoice = mItem.OpeningBalances.CurrentItem
				Else
					mReceiptInvoice = mItem.OpeningBalances(k)
				End If

				If mIsDeleted Then
					mItem.OpeningBalances.Remove(mReceiptInvoice)
				End If

				If mIsNew Or mIsDirty Then

					With mReceiptInvoice

						mDateFormatString = jsonObject(propertyName:="mAsOnDate")("mFormat")

						mItem.OpeningBalances.CurrentItem.InvoiceText = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mText").ToString()
						mItem.OpeningBalances.CurrentItem.InvoiceNo = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mNo").ToString())
						mItem.OpeningBalances.CurrentItem.COtherCharges = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mCOtherCharges"))
						mItem.OpeningBalances.CurrentItem.AsOnDate = CDate(mOpeningBalancesArray(k)("mAsOnDate").First.First).ToString(format:=mDateFormatString)
						mItem.OpeningBalances.CurrentItem.VendorInvoiceNo = mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mVendorInvoiceNo").ToString()
						mItem.OpeningBalances.CurrentItem.InvoiceDate = CDate(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceDate").First.First).ToString(format:=mDateFormatString)
						mItem.OpeningBalances.CurrentItem.VendorInvoiceDate = CDate(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mVendorInvoiceDate").First.First).ToString(format:=mDateFormatString)
						mItem.OpeningBalances.CurrentItem.CurrencyID = New Guid(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mCurrencyID").ToString())
						mItem.OpeningBalances.CurrentItem.ConversionFactor = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mConversionFactor").ToString())
						mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.BaseUnitID = mItem.UnitID
						mItem.OpeningBalances.CurrentItem.DisplayQty = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mDisplayQty"))
						mItem.OpeningBalances.CurrentItem.Qty = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mQty"))
						mItem.OpeningBalances.CurrentItem.CRate = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mCRate"))
						mItem.OpeningBalances.CurrentItem.LandingCost = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mCEffRate"))
						mItem.OpeningBalances.CurrentItem.ReleaseNoteNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mReleaseNoteNo")
						mItem.OpeningBalances.CurrentItem.ReleaseNoteDate = CDate(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mReleaseNoteDate").First.First).ToString(format:=mDateFormatString)
						mItem.OpeningBalances.CurrentItem.StartDate = CDate(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mStartDate").First.First).ToString(format:=mDateFormatString)
						mItem.OpeningBalances.CurrentItem.ExpiryDate = CDate(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mExpiryDate").First.First).ToString(format:=mDateFormatString)
						mItem.OpeningBalances.CurrentItem.ItemTypeID = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mItemTypeID"))
						mItem.OpeningBalances.CurrentItem.Returnable = CBool(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mReturnable"))
						mItem.OpeningBalances.CurrentItem.TypeID = CInt(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mTypeID"))

						If mItem.OpeningBalances.CurrentItem.TypeID = 1 Or mItem.OpeningBalances.CurrentItem.TypeID = 14 Then
							mItem.OpeningBalances.CurrentItem.VendorID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mVendorID").ToString())
						ElseIf mItem.OpeningBalances.CurrentItem.TypeID = 2 Then
							mItem.OpeningBalances.CurrentItem.MachineID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mMachineID").ToString())
						ElseIf mItem.OpeningBalances.CurrentItem.TypeID = 18 Then
							mItem.OpeningBalances.CurrentItem.FromStoreID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mStoreID").ToString())
						ElseIf mItem.OpeningBalances.CurrentItem.TypeID = 16 Then
							mItem.OpeningBalances.CurrentItem.WorkShopID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mWorkShopID").ToString())
						End If

						mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.StoreID = New Guid(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mStoreID").ToString())
						mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.StoreName = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mStoreName").ToString()
						mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.SerialNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mSerialNo").ToString()
						mItem.OpeningBalances.CurrentItem.Location = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mLocation").ToString()
						mItem.OpeningBalances.CurrentItem.Remark = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mRemark").ToString()
						mItem.OpeningBalances.CurrentItem.Note = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mNote").ToString()
						mItem.OpeningBalances.CurrentItem.CureQtrs = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mCureQtrs").ToString())
						mItem.OpeningBalances.CurrentItem.CureYear = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mCureYear").ToString())
						mItem.OpeningBalances.CurrentItem.ExpQtrs = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mExpQtrs").ToString())
						mItem.OpeningBalances.CurrentItem.ExpYear = CInt(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mExpYear").ToString())
						mItem.OpeningBalances.CurrentItem.BatchNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mBatchNo").ToString()
						mItem.OpeningBalances.CurrentItem.Receipt.IntReceiptNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mIntReceiptNo").ToString()
						mItem.OpeningBalances.CurrentItem.CCommercialRate = CDec(mOpeningBalancesArray(k).Item(key:="mInvoice").Item(key:="mInvoiceItems").Item(key:=0).Item(key:="mCCommercialRate"))
						mItem.OpeningBalances.CurrentItem.CalibrationDoneOnDate = CDate(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mCalibrationDoneOnDate").First.First).ToString(format:=mDateFormatString)
						mItem.OpeningBalances.CurrentItem.IsExpiryNA = CBool(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mIsExpiryNA"))
						mItem.OpeningBalances.CurrentItem.IsExpiryUnlimited = CBool(mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mIsExpiryUnlimited"))
						mItem.OpeningBalances.CurrentItem.Receipt.ReceiptItems.CurrentItem.CodeNo = mOpeningBalancesArray(k).Item(key:="mReceipt").Item(key:="mReceiptItems").Item(key:=0).Item(key:="mCodeNo")

					End With

				End If

			Next

			For l As Integer = 0 To mItemServiceInspectionsListArray.Count - 1

				Dim mID As Guid = New Guid(mItemServiceInspectionsListArray(l)("mID").ToString)
				Dim mIsNew As Boolean = CBool(mItemServiceInspectionsListArray(l)("mIsNew"))
				Dim mIsDeleted As Boolean = CBool(mItemServiceInspectionsListArray(l)("mIsDeleted"))
				Dim mIsDirty As Boolean = CBool(mItemServiceInspectionsListArray(l)("mIsDirty"))
				Dim mItemServiceInspections As ItemServiceInspections

				If mIsNew Then
					mItem.ItemApplicables.Add(mItem.ID)
					mItemServiceInspections = mItem.ItemServiceInspectionsList.CurrentItem
				Else
					mItemServiceInspections = mItem.ItemServiceInspectionsList(ID:=mID)
				End If

				If mIsDeleted Then
					mItem.ItemServiceInspectionsList.Remove(mItemServiceInspections)
				End If

				If mIsNew Or mIsDirty Then

					With mItemServiceInspections

						.ItemID = New Guid(mItemServiceInspectionsListArray(index:=l)(key:="mItemID").ToString())
						.Description = mItemServiceInspectionsListArray(index:=l)(key:="mDescription").ToString()
						.Frequency = CInt(mItemServiceInspectionsListArray(index:=l)(key:="mFrequency").ToString())
						.FrequencyPeriod = CInt(mItemServiceInspectionsListArray(index:=l)(key:="mFrequencyPeriod").ToString())
						.ServiceInspectionNameID = New Guid(mItemServiceInspectionsListArray(index:=l)(key:="mServiceInspectionNameID").ToString())

					End With

				End If

			Next

			mItem.Save()

			Return "Success"

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessage(ModuleName:="Item",
																						   ex:=ex)
			Return returnMessage

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " DELETE Method(s) "

	<HttpDelete>
	Public Function DeleteItem(ID As Guid) As IHttpActionResult

		Try

			Item.DeleteItem(ID:=ID)

			Return Ok(New ReturnMessage("Success", "Item Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Item",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))


		End Try

	End Function

#End Region

#End Region

#Region " Service Inspection "

#Region " GET Method(s) "

	<HttpGet>
	Public Function GetServiceInspectionList(Optional ServiceInspectionName As String = "") As ServiceInspectionNameList

		Try

			Return ServiceInspectionNameList.GetServiceInspectionList(ServiceInspectionName:=ServiceInspectionName)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	<HttpGet>
	Public Function GetServiceInspectionName(Optional ID As Guid = Nothing) As ServiceInspectionName

		Try

			Return ServiceInspectionName.GetServiceInspectionName(ID:=ID)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " POST Method(s) "

	<HttpPost>
	Public Function SaveServiceInspectionName(<FromBody()> value As Object) As IHttpActionResult

		Try

			Dim jsonObject As JObject = JObject.Parse(value.ToString())
			Dim mIsNew As Boolean = jsonObject("mIsNew").ToObject(Of Boolean)()
			Dim returnstring As String = ""

			If mIsNew Then
				returnstring = SetNewServiceInspectionName(jsonObject)
			Else
				returnstring = SetExistingServiceInspectionName(jsonObject)
			End If

			'If returnstring = "Success" Then
			'    Return New ReturnMessage("Success", "Service Inspection Name saved successfully!")
			'Else
			'    Return New ReturnMessage("Error", returnstring)
			'End If

			If returnstring = "Success" Then

				Return Ok(New ReturnMessage(Status:="Success",
												   Message:="Service Inspection Name Saved Successfully!"))

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

	Private Function SetNewServiceInspectionName(jsonObject As JObject) As String

		Try

			Dim mServiceInspectionName As ServiceInspectionName = ServiceInspectionName.NewServiceInspectionName(ID:=New Guid(jsonObject("mID").ToString()), Type:=0)
			mServiceInspectionName.ServiceInspectionName = jsonObject("mServiceInspectionName").ToString()
			mServiceInspectionName.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

	Private Function SetExistingServiceInspectionName(jsonObject As JObject) As String

		Try

			Dim mServiceInspectionName As ServiceInspectionName = ServiceInspectionName.GetServiceInspectionName(ID:=New Guid(jsonObject("mID").ToString()))
			mServiceInspectionName.ServiceInspectionName = jsonObject("mServiceInspectionName").ToString()
			mServiceInspectionName.Save()

			Return "Success"

		Catch ex As Exception
			Return ex.Message
		End Try

	End Function

#End Region

#Region " DELETE Method(s) "

	<HttpDelete>
	Public Function DeleteServiceInspectionName(ID As Guid) As IHttpActionResult

		Try

			ServiceInspectionName.DeleteServiceInspectionName(ID:=ID)

			Return Ok(New ReturnMessage("Success", "Service Inspection Name Deleted Successfully!"))

		Catch ex As SqlException

			Dim returnMessage As String = _SQLExceptionHelper.UserFriendlyExceptionMessageForDelete(ModuleName:="Item",
																									SqlException:=ex)

			Return Content(HttpStatusCode.BadRequest,
						   New ReturnMessage("Error",
												   returnMessage))


		End Try

	End Function

#End Region

#End Region

End Class
