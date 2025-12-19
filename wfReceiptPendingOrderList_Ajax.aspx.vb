Public Class wfReceiptPendingOrderList_Ajax
	Inherits System.Web.UI.Page

#Region " Enumeration "
	Public Enum FromToTypeID
		Vendor = 1
		Aircraft = 2
		Store = 8
		WorkShop = 16
		Work = 17
	End Enum
	Public Enum Transaction
		Order = 3
		Issue = 4
		Receipt = 5
	End Enum
#End Region

#Region " Variable Declaration "
	Public mReceipt As Receipt
	Public mReceiptCumInvoice As ReceiptCumInvoice
	Public mInvoice As Invoice
	Public mPendingToReceiveTransItemList As PendingToReceiveTransItemList
	Public mPendingReceiptItemList As PendingInvoiceList
	Public mFromID As Guid
	Public mPrevTransID As Guid
	Public mTransTypeID As Trans
	Public mTransaction As Transaction
	Public mPrimaryOrderType As Integer
	Public mOrderList As OrderList
	Private mIssueList As IssueList
	Private mReceiptList As ReceiptList
	Public mType As Integer
	Public mFromToTypeID As FromToTypeID
	Public mIsAll As Boolean
	Dim mFromPartList As Boolean
	Public mItemName As String
	Public mSelectList() As Boolean
	Public mVendorID As Guid
	Public mDCNo As String      'Added by Saylee on 20-june-2011
	Public mDCDate As String
	Public mAWBNo As String
	Public mReceiptID As Guid   '***************************
	Dim ItemID As Guid          'Added By Vikrant On 20-Feb-2013 For  All20022013-1
	Dim mLastWarrantyInformation As LastWarrantyInformation
	Dim mOrderTranstypeID As Integer
	Public mUserHasNoStoreRights As UserHasNoStoreRights
	Public mDistinctTextListForOrder As DistinctTextListForOrder
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mReceipt = CType(Session("mReceipt"), Receipt)
		mInvoice = CType(Session("mInvoice"), Invoice)
		mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
		mOrderList = CType(Session("mOrderList"), OrderList)
		mPendingReceiptItemList = CType(Session("mPendingReceiptItemList"), PendingInvoiceList)
		mPendingToReceiveTransItemList = CType(Session("mPendingToReceiveTransItemList"), PendingToReceiveTransItemList)
		mOrderList = Session("mOrderList")
		mIssueList = Session("mIssueList")
		mReceiptList = Session("mReceiptList")
		mFromID = Session("mFromID")
		mPrevTransID = Session("mPrevTransID")
		mTransTypeID = Session("mTransTypeID")
		mTransaction = Session("mTransaction")
		mPrimaryOrderType = Session("mPrimaryOrderType")
		mFromPartList = Session("mFromPartList")
		mFromToTypeID = Session("mFromToTypeID")
		mVendorID = Session("mVendorID")
		mDCNo = Session("mDCNo")    'Added by Saylee on 20-june-2011
		mDCDate = Session("mDCDate")
		mReceiptID = Session("mReceiptID")
		mAWBNo = Session("mAWBNo")  '**********************************
		ItemID = Session("ItemID")  'Added By Vikrant On 20-Feb-2013 For  All20022013-1
		mOrderTranstypeID = Session("mOrderTranstypeID")
	End Sub
	Private Sub SetSession()
		Session("mReceipt") = mReceipt
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		Session("mOrderList") = mOrderList
		Session("mIssueList") = mIssueList
		Session("mReceiptList") = mReceiptList
		Session("mPendingReceiptItemList") = mPendingReceiptItemList
		Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList
		Session("mFromID") = mFromID
		Session("mPrevTransID") = mPrevTransID
		Session("mTransTypeID") = mTransTypeID
		Session("mTransaction") = mTransaction
		Session("mPrimaryOrderType") = mPrimaryOrderType
		Session("mFromPartList") = mFromPartList
		Session("mFromToTypeID") = mFromToTypeID
		Session("mVendorID") = mVendorID
		Session("mDCNo") = mDCNo    'Added by Saylee on 20-june-2011
		Session("mDCDate") = mDCDate
		Session("mReceiptID") = mReceiptID
		Session("mAWBNo") = mAWBNo  '***********************************
		Session("mOrderTranstypeID") = mOrderTranstypeID
	End Sub
	Private Sub ClearAll()
		Session("mPendingReceiptItemList") = Nothing
		Session("mPendingToReceiveTransItemList") = Nothing
	End Sub
	Private Sub ControlVisibilityReceipt()
		If mReceipt.ReceiptItems.Count - 1 = 0 Then
			txtDate.Enabled = True
		Else
			txtDate.Enabled = False
		End If
		lblDate.Text = "Receipt Date"
		txtDate.Text = mReceipt.RecdDateFormatted.ToString
		If mReceipt.TransTypeID = 6 Then
			btnCreateOrder.Visible = True
			txtSearch.Visible = True    'Added by Shweta on 10-May-2012 for 10052012-11
			lblSearch.Visible = True
			cmbOrderText.Visible = True
			txtNo.Visible = True
			txtAmend.Visible = True
			lblOrderNo.Visible = True
		ElseIf mReceipt.TransTypeID = 7 Or mReceipt.TransTypeID = 10 Then
			txtSearch.Visible = True
			lblSearch.Visible = True    '**************
			cmbOrderText.Visible = True
			txtNo.Visible = True
			txtAmend.Visible = True
			lblOrderNo.Visible = True
		Else
			btnCreateOrder.Visible = False
		End If
	End Sub
	Private Sub ControlVisibilityRCI()
		If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1 = 0 Then
			txtDate.Enabled = True
		Else
			txtDate.Enabled = False
		End If
		lblDate.Text = "Date"
		txtDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted
		If mReceiptCumInvoice.TransTypeID = 7 Then
			btnCreateOrder.Visible = True
			txtSearch.Visible = True    'Added by Shweta on 10-May-2012 for 10052012-11
			lblSearch.Visible = True
			cmbOrderText.Visible = True
			txtNo.Visible = True
			txtAmend.Visible = True
			lblOrderNo.Visible = True
		ElseIf (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 54) Then
			txtSearch.Visible = True
			lblSearch.Visible = True    '**************
			cmbOrderText.Visible = True
			txtNo.Visible = True
			txtAmend.Visible = True
			lblOrderNo.Visible = True
		Else
			btnCreateOrder.Visible = False
		End If

		If mReceiptCumInvoice.TransTypeID = 28 Then 'Added by Vikrant On 21-May-2012 FOR ALL21052012-05
			chkReturnableBackFromCustomer.Visible = True
		Else
			chkReturnableBackFromCustomer.Visible = False
		End If                                      'END
	End Sub
	Private Sub ControlVisibilityInvoice()
		If mInvoice.InvoiceItems.Count = 0 Then
			txtDate.Enabled = True
		Else
			txtDate.Enabled = False
		End If
		lblDate.Text = "Invoice Date"
		txtDate.Text = mInvoice.InvoiceDateFormatted
		'Added by Vikrant On 01-Jul-2020
		If mInvoice.TransTypeID = 21 Then
			txtSearch.Visible = True
			lblSearch.Visible = True
		End If
		'End
	End Sub
	Private Sub FindNow()
		mTransTypeID = mReceipt.TransTypeID

		If (mReceipt.FromTypeID = 14) Or (mReceipt.FromTypeID = 1) Then mFromID = mReceipt.VendorID 'From Vendor        '1->14 30-08-2006
		If mReceipt.FromTypeID = 2 Then mFromID = mReceipt.MachineID 'For Aircraft
		If mReceipt.FromTypeID = 8 Then mFromID = mReceipt.StoreID 'For Store
		Session("mFromID") = mFromID

		GetSession()
		'Get List From the Database as per Criteria 
		If mTransaction = Transaction.Order Then
			dgOrderList.Visible = True
			dgIssueList.Visible = False
			dgReceiptList.Visible = False
			If rdbFromAllPendingOrder.Checked Then  'mIsAll
				If mTransTypeID = Trans.ReceiptcumInvoiceAgainstPuchaseOrder Then
					mOrderList = OrderList.GetPendingOrderList(txtSearch.Text.Trim, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text)), txtAmend.Text.Trim, "", "1/1/1800", txtDate.Text.ToString, 2, "", mFromID.ToString, , mPrimaryOrderType, Guid.Empty.ToString, CurrencyID:=mReceiptCumInvoice.CurrencyID.ToString, ListFor:=1)  'Parameter ItemName is added by Shweta on 10-May-2012 for 10052012-11
				Else
					If mReceipt.ReceiptItems.Count > 0 Then
						mOrderList = OrderList.GetPendingOrderList(txtSearch.Text.Trim, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text)), txtAmend.Text.Trim, "", "1/1/1800", txtDate.Text.ToString, 2, "", mFromID.ToString, , mPrimaryOrderType, Guid.Empty.ToString, CurrencyID:=mReceipt.ReceiptItems(0).OrderCurrencyID.ToString, ListFor:=1)  'Parameter ItemName is added by Shweta on 10-May-2012 for 10052012-11
					Else
						mOrderList = OrderList.GetPendingOrderList(txtSearch.Text.Trim, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text)), txtAmend.Text.Trim, "", "1/1/1800", txtDate.Text.ToString, 2, "", mFromID.ToString, , mPrimaryOrderType, Guid.Empty.ToString, ListFor:=1)  'Parameter ItemName is added by Shweta on 10-May-2012 for 10052012-11
					End If
				End If

			Else
				mOrderList = OrderList.GetPendingOrderList(txtSearch.Text.Trim, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text)), txtAmend.Text.Trim, "", "1/1/1800", txtDate.Text.ToString, 2, "", mFromID.ToString, , mPrimaryOrderType, mPrevTransID.ToString, ListFor:=1) 'Parameter ItemName is added by Shweta on 10-May-2012 for 10052012-11
			End If
			dgOrderList.DataSource = mOrderList
			dgOrderList.DataBind()
			Session("mOrderList") = mOrderList
			lblLabel.Text = "Enter date to create Receipt and click Find Now button to get Order list accordingly."
			upnlOrderList.Update()
		Else
			dgOrderList.Visible = False
			dgIssueList.Visible = True
			dgReceiptList.Visible = False
			If rdbFromAllPendingOrder.Checked Then    'mIsAll
				mIssueList = IssueList.GetPendingIssueList("", 0, "1/1/1800", txtDate.Text.ToString, mFromToTypeID, "", 0, "", "", "", mTransTypeID, mFromID.ToString, Guid.Empty.ToString, IIf(chkReturnableBackFromCustomer.Checked, True, False)) 'ALL21052012-05
			Else
				mIssueList = IssueList.GetPendingIssueList("", 0, "1/1/1800", txtDate.Text.ToString, mFromToTypeID, "", 0, "", "", "", mTransTypeID, mFromID.ToString, mPrevTransID.ToString, IIf(chkReturnableBackFromCustomer.Checked, True, False)) 'ALL21052012-05
			End If
			dgIssueList.DataSource = mIssueList
			dgIssueList.DataBind()
			Session("mIssueList") = mIssueList
			lblLabel.Text = "Enter date to create Receipt and click Find Now button to get Issue list accordingly."
			upnlIssueList.Update()
		End If
		dgTransItemList.Visible = False
		lblTransItemListResult.Visible = False
		Session("mTransaction") = mTransaction
		If mTransaction = Transaction.Order Then
			lblResult.Text = "List of Order : " + mOrderList.Count.ToString + " Record (s) found"
		Else
			lblResult.Text = "List of Issue : " + mIssueList.Count.ToString + " Record (s) found"
		End If
		upnlDetails.Update()
		upnlTransItemList.Update()
	End Sub
	Private Sub FindNow1()
		GetSession()
		mTransTypeID = mReceiptCumInvoice.TransTypeID
		If (mReceiptCumInvoice.FromTypeID = 14) Or (mReceiptCumInvoice.FromTypeID = 1) Then mFromID = mReceiptCumInvoice.VendorID 'From Vendor        '1->14 30-08-2006
		If mReceiptCumInvoice.FromTypeID = 2 Then mFromID = mReceiptCumInvoice.AircraftID 'For Aircraft
		If mReceiptCumInvoice.FromTypeID = 8 Then mFromID = mReceiptCumInvoice.StoreID 'For Store
		If mReceiptCumInvoice.FromTypeID = 16 Then mFromID = mReceiptCumInvoice.WorkShopID 'For WorkShop
		If mReceiptCumInvoice.FromTypeID = 17 Then mFromID = mReceiptCumInvoice.WOID 'For WorkOrder

		Session("mFromID") = mFromID
		'Get List From the Database as per Criteria  
		If mTransaction = Transaction.Order Then
			dgOrderList.Visible = True
			dgIssueList.Visible = False
			dgReceiptList.Visible = False
			If rdbFromAllPendingOrder.Checked Then   'mIsAll
				If mTransTypeID = Trans.ReceiptcumInvoiceAgainstPuchaseOrder Then
					mOrderList = OrderList.GetPendingOrderList(txtSearch.Text.Trim, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text)), txtAmend.Text.Trim, "", "1/1/1800", txtDate.Text.ToString, 2, "", mFromID.ToString, , mPrimaryOrderType, Guid.Empty.ToString, CurrencyID:=mReceiptCumInvoice.CurrencyID.ToString)
				Else
					If mReceipt.ReceiptItems.Count > 0 Then
						mOrderList = OrderList.GetPendingOrderList(txtSearch.Text.Trim, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text)), txtAmend.Text.Trim, "", "1/1/1800", txtDate.Text.ToString, 2, "", mFromID.ToString, , mPrimaryOrderType, Guid.Empty.ToString, CurrencyID:=mReceipt.ReceiptItems(0).OrderCurrencyID.ToString)
					Else '
						mOrderList = OrderList.GetPendingOrderList(txtSearch.Text.Trim, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text)), txtAmend.Text.Trim, "", "1/1/1800", txtDate.Text.ToString, 2, "", mFromID.ToString, , mPrimaryOrderType, Guid.Empty.ToString)
					End If
				End If

			Else
				mOrderList = OrderList.GetPendingOrderList(txtSearch.Text.Trim, IIf(cmbOrderText.SelectedIndex = 0, "", cmbOrderText.SelectedItem.Text), IIf(txtNo.Text = "", 0, Val(txtNo.Text)), txtAmend.Text.Trim, "", "1/1/1800", txtDate.Text.ToString, 2, "", mFromID.ToString, , mPrimaryOrderType, mPrevTransID.ToString)
			End If
			dgOrderList.DataSource = mOrderList
			Session("mOrderList") = mOrderList
			dgOrderList.DataBind()
			'If AppSettings("ClientCode") = "CE" Then
			lblLabel.Text = "Enter date to create Goods Receipt and click Find Now button to get Order list accordingly."
			'Else
			'    lblLabel.Text = "Enter date to create Receipt-Cum-Invoice and click Find Now button to get Order list accordingly."
			'End If
			upnlOrderList.Update()
		Else
			dgOrderList.Visible = False
			dgIssueList.Visible = True
			If rdbFromAllPendingOrder.Checked Then  'mIsAll
				mIssueList = IssueList.GetPendingIssueList("", 0, "1/1/1800", txtDate.Text.ToString, mFromToTypeID, "", 0, "", "", "", mTransTypeID, mFromID.ToString, Guid.Empty.ToString, IIf(chkReturnableBackFromCustomer.Checked, True, False))   'ALL21052012-05
			Else
				mIssueList = IssueList.GetPendingIssueList("", 0, "1/1/1800", txtDate.Text.ToString, mFromToTypeID, "", 0, "", "", "", mTransTypeID, mFromID.ToString, mPrevTransID.ToString, IIf(chkReturnableBackFromCustomer.Checked, True, False)) 'ALL21052012-05
			End If
			dgIssueList.DataSource = mIssueList
			Session("mIssueList") = mIssueList
			ColumnsVisibility()
			dgIssueList.DataBind()
			'If AppSettings("ClientCode") = "CE" Then
			lblLabel.Text = "Enter date to create Goods Receipt and click Find Now button to get Issue list accordingly."
			'Else
			'    lblLabel.Text = "Enter date to create Receipt-Cum-Invoice and click Find Now button to get Issue list accordingly."
			'End If
			upnlIssueList.Update()
		End If

		dgTransItemList.Visible = False
		lblTransItemListResult.Visible = False

		Session("mTransaction") = mTransaction
		lblResult.Visible = True
		If mTransaction = Transaction.Order Then
			lblResult.Text = "List of Order : " + mOrderList.Count.ToString + " Record (s) found"
		Else
			lblResult.Text = "List of Issue : " + mIssueList.Count.ToString + " Record (s) found"
		End If
		lnkSelectAll.Visible = False 'Added By Utkarsh ON 23-Feb-2012 For ALL22022012
		upnlDetails.Update()
		upnlTransItemList.Update()
	End Sub
	Private Sub ColumnsVisibility()
		mFromToTypeID = Session("mFromToTypeID")
		Select Case mFromToTypeID
			Case FromToTypeID.Aircraft
				dgIssueList.Columns(3).Visible = True
			Case FromToTypeID.WorkShop
				dgIssueList.Columns(6).Visible = True
			Case FromToTypeID.Store
				dgIssueList.Columns(4).Visible = True
				dgIssueList.Columns(5).Visible = True
			Case FromToTypeID.Vendor
				If Session("mReceivedFrom") = "0" Then
					dgIssueList.Columns(2).HeaderText = "Supplier"
				ElseIf Session("mReceivedFrom") = "3" Then
					dgIssueList.Columns(2).HeaderText = "Customer"
				End If
				dgIssueList.Columns(2).Visible = True
			Case FromToTypeID.Work
				dgIssueList.Columns(7).Visible = True
		End Select
	End Sub
	Private Sub FindNow2()
		mFromID = mInvoice.VendorID
		Session("mFromID") = mFromID
		mTransTypeID = mInvoice.TransTypeID
		'Get List From the Database as per Criteria  
		If mTransaction = Transaction.Receipt Then
			dgOrderList.Visible = False
			dgIssueList.Visible = False
			dgTransItemList.Visible = False
			dgReceiptList.Visible = True
			dgItemReceiptDetail.Visible = True
			btnDone.Visible = True
			If CType(mInvoice.TransTypeID, Trans) = Util.Trans.PurchaseInvoice Then
				If rdbFromAllPendingOrder.Checked Then   'mIsAll
					mReceiptList = ReceiptList.GetPendingRecepitList("1/1/1800", txtDate.Text.ToString, "", 0, "", mFromID.ToString, "", "", 2, txtSearch.Text.Trim, 0, "", 0, "", "", 1, Util.Trans.ReceiptAgainstPuchaseOrder, Guid.Empty.ToString, OrderTransTypeID:=mOrderTranstypeID)
				Else
					mReceiptList = ReceiptList.GetPendingRecepitList("1/1/1800", txtDate.Text.ToString, "", 0, "", mFromID.ToString, "", "", 2, txtSearch.Text.Trim, 0, "", 0, "", "", 1, Util.Trans.ReceiptAgainstPuchaseOrder, mPrevTransID.ToString, OrderTransTypeID:=mOrderTranstypeID)
				End If
			ElseIf CType(mInvoice.TransTypeID, Trans) = Util.Trans.ExchangeRepairReceivedFromVendor Then
				If rdbFromAllPendingOrder.Checked Then   'mIsAll
					mReceiptList = ReceiptList.GetPendingRecepitList("1/1/1800", txtDate.Text.ToString, "", 0, "", mFromID.ToString, "", "", 2, txtSearch.Text.Trim, 0, "", 0, "", "", 1, Util.Trans.ExchangeRepairReceivedFromVendor, Guid.Empty.ToString, OrderTransTypeID:=mOrderTranstypeID)
				Else
					mReceiptList = ReceiptList.GetPendingRecepitList("1/1/1800", txtDate.Text.ToString, "", 0, "", mFromID.ToString, "", "", 2, txtSearch.Text.Trim, 0, "", 0, "", "", 1, Util.Trans.ExchangeRepairReceivedFromVendor, mPrevTransID.ToString, OrderTransTypeID:=mOrderTranstypeID)
				End If
			ElseIf CType(mInvoice.TransTypeID, Trans) = Util.Trans.RCIFromSupplierAsNone Then
				If rdbFromAllPendingOrder.Checked Then   'mIsAll
					mReceiptList = ReceiptList.GetPendingRecepitList("1/1/1800", txtDate.Text.ToString, "", 0, "", mFromID.ToString, "", "", 2, txtSearch.Text.Trim, 0, "", 0, "", "", 1, Util.Trans.RCIFromSupplierAsNone, Guid.Empty.ToString, OrderTransTypeID:=mOrderTranstypeID)
				Else
					mReceiptList = ReceiptList.GetPendingRecepitList("1/1/1800", txtDate.Text.ToString, "", 0, "", mFromID.ToString, "", "", 2, txtSearch.Text.Trim, 0, "", 0, "", "", 1, Util.Trans.RCIFromSupplierAsNone, mPrevTransID.ToString, OrderTransTypeID:=mOrderTranstypeID)
				End If
			End If
			dgReceiptList.DataSource = mReceiptList
			Session("mReceiptList") = mReceiptList
			dgReceiptList.DataBind()

			dgTransItemList.Visible = False
			lblTransItemListResult.Visible = False
			lblLabel.Text = "Enter date to create Invoice and click Find Now button to get Receipt list accordingly."
			upnlReceiptList.Update()
		End If
		Session("mTransaction") = mTransaction
		If (mTransaction = Transaction.Receipt) Then
			lblResult.Text = "List of Receipt : " + mReceiptList.Count.ToString + " Record (s) found"
		End If
		dgItemReceiptDetail.DataSource = mPendingReceiptItemList
		btnDone.Enabled = (dgItemReceiptDetail IsNot Nothing) AndAlso (dgItemReceiptDetail.Rows.Count > 0)
		'Added By Vikrant On 01-Jul-2020
		dgItemReceiptDetail.Visible = False
		lblItemReceiptDetailResult.Visible = False
		'End
		upnlItemReceiptDetail.Update()
		upnlButtons.Update()
	End Sub
	Private Sub ItemSelectionForReceipt(ByVal Index As Integer)
		'Open the Selected Record in Details Form.
		' mReceipt.ReceiptItems.CurrentItem.No = mPendingToReceiveTransItemList(Index).No
		mFromPartList = Session("mFromPartList")
		mTransTypeID = mReceipt.TransTypeID
		Dim Index1 As Integer
		Index1 = Session("Index1")
		If mReceipt.IsNew Then
			mReceipt.RecdDate = txtDate.Text
		End If
		If mReceipt.FromTypeID = 1 Then
			mReceipt.VendorID = mOrderList.Item(Index1).VendorID 'VendorList.GetVendortList(0).Item(mOrderList.Item(Index1).VendorName).ID 'Added By Prashant On 31-May-2018 For ALL31052018
			mReceipt.OrderID = mOrderList.Item(Index1).ID 'ALL30082018
		End If
		mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = mPendingToReceiveTransItemList(Index).PrimaryCategoryID 'Added By Prashant On 07-Oct-2015 For ALL06102015
		If ((mPendingToReceiveTransItemList(Index).Type = 3 Or mPendingToReceiveTransItemList(Index).Type = 12 Or mPendingToReceiveTransItemList(Index).Type = 13) And mPendingToReceiveTransItemList.Item(Index).IsSerialized = False) Or mPendingToReceiveTransItemList(Index).Type = 4 Then  'If Order or Issue
			mReceipt.ReceiptItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemList(Index).Type

			If mPendingToReceiveTransItemList(Index).Type = 3 Then mReceipt.ReceiptItems.CurrentItem.OrderItemID = mPendingToReceiveTransItemList(Index).OrderItemID
			If mPendingToReceiveTransItemList(Index).Type = 4 Then mReceipt.ReceiptItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemList(Index).IssueItemID

			'This Checks whether Item Is Selected from PartList or Not
			mReceipt.ReceiptItems.CurrentItem.FromPartList = False

			'Item of Aircraft/Store against Issue
			If (mPendingToReceiveTransItemList(Index).Type = 12 Or mPendingToReceiveTransItemList(Index).Type = 13) And (mReceipt.ReceiptItems.CurrentItem.FromPartList = False) Then
				If mPendingToReceiveTransItemList(Index).Type = 12 Then mReceipt.ReceiptItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemList(Index).IssueItemID
				If mPendingToReceiveTransItemList(Index).Type = 13 Then mReceipt.ReceiptItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemList(Index).IssueItemID
			End If

			'This will Returns PartName and PartDescription For FromType Aircraft Or Store
			If (mPendingToReceiveTransItemList(Index).Type = 12 Or mPendingToReceiveTransItemList(Index).Type = 13) And (mReceipt.ReceiptItems.CurrentItem.FromPartList = True) Then
				mReceipt.ReceiptItems.CurrentItem.ItemID = mPendingToReceiveTransItemList(Index).ItemID
				mReceipt.ReceiptItems.CurrentItem.Part = mPendingToReceiveTransItemList(Index).ItemName
				mReceipt.ReceiptItems.CurrentItem.PartDescription = mPendingToReceiveTransItemList(Index).ItemDescription
			End If
			If mReceipt.TransTypeID = 6 Then 'Added By Prashant 5-Feb-2019 ALL04022019
				mReceipt.ReceiptItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).OrderItemUnitID
			Else
				mReceipt.ReceiptItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).UnitID
			End If
			mReceipt.ReceiptItems.CurrentItem.DisplayQty = mPendingToReceiveTransItemList(Index).PendingItemQty   'Added By Prashant 11-May2010

			'Kalpesh   - IF receipt is against Issue then SerialNo should come Automaticaly
			If mPendingToReceiveTransItemList(Index).Type = 4 Then
				mReceipt.ReceiptItems.CurrentItem.SerialNo = mReceipt.ReceiptItems.CurrentItem.IssueItemDetailForReceipt.SerialNo
			ElseIf mPendingToReceiveTransItemList(Index).Type = 3 Then
				mReceipt.ReceiptItems.CurrentItem.SerialNo = ""
			End If

			mReceipt.ReceiptItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemList(Index).OriginalReceiptDate
			mReceipt.ReceiptItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemList(Index).OriginalReceiptTextNo

			'if ReceiptItem is New and Receipt is against Order and Receiveing Part is Serialized
		ElseIf mReceipt.ReceiptItems.CurrentItem.IsNew And mPendingToReceiveTransItemList(Index).Type = 3 And mPendingToReceiveTransItemList(Index).IsSerialized = True Then
			mReceipt.ReceiptItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemList(Index).Type
			mReceipt.ReceiptItems.CurrentItem.OrderItemID = mPendingToReceiveTransItemList(Index).OrderItemID
			mReceipt.ReceiptItems.CurrentItem.ItemTypeID = mPendingToReceiveTransItemList(Index).ItemTypeID 'Added By Prashant On 26-Nov-2014 For ALL24112014
			mReceipt.ReceiptItems.CurrentItem.FromPartList = False
			mReceipt.ReceiptItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).UnitID
			mReceipt.ReceiptItems.CurrentItem.DisplayQty = CDec(IIf(mPendingToReceiveTransItemList(Index).IsSerialized, 1, mPendingToReceiveTransItemList(Index).PendingItemQty))  'CDec(mPendingToReceiveTransItemList(Index).PendingItemQty)

			'If mReceipt.ReceiptItems.CurrentItem.ExpiryMonth > 0 Then
			'    mReceipt.ReceiptItems.CurrentItem.StartDate = mReceipt.RecdDate
			'    If Not (mReceipt.ReceiptItems.CurrentItem.StartDate) Is System.DBNull.Value Then
			'        mReceipt.ReceiptItems.CurrentItem.ExpiryDate = CDate(mReceipt.ReceiptItems.CurrentItem.StartDate).AddMonths(mPendingToReceiveTransItemList(Index).ExpiryMonth)
			'    End If
			'End If

			'If mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter > 0 Then '============== Added By Rajnish On 25-03-2008
			'    mReceipt.ReceiptItems.CurrentItem.StartDate = mReceipt.RecdDate
			'End If '===============

			mReceipt.ReceiptItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemList(Index).OriginalReceiptDate
			mReceipt.ReceiptItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemList(Index).OriginalReceiptTextNo
		End If

		If mReceipt.TransTypeID = 10 Then 'Added By Vikrant on 20-Aug-2012 All14082012-1
			If mReceipt.ReceiptItems.CurrentItem.IsSerialized = True Then
				mReceipt.ReceiptItems.CurrentItem.SerialNo = mPendingToReceiveTransItemList(Index).SerialNo
			End If

			If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then  'Added By Vikrant On 02-May-2013 For BA30042013-2'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
				mReceipt.ReceiptItems.CurrentItem.BatchNo = mPendingToReceiveTransItemList(Index).OrderTextNo
			End If 'End
		End If 'End
		'vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
		'Added By Prashant 17-Feb-2015-----------------------
		mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceipt.ReceiptItems.CurrentItem.ItemID.ToString, mReceipt.ReceiptItems.CurrentItem.SerialNo)

		If (mReceipt.TransTypeID = 6 Or mReceipt.TransTypeID = 10) Then
			If mReceipt.ReceiptItems.CurrentItem.IsSerialized = True Then 'Serialized
				If (mOrderList.Item(Index1).OrderType = "New Purchase" Or mOrderList.Item(Index1).OrderType = "Overhaul") Then
					mReceipt.ReceiptItems.CurrentItem.IsWarranty = True
					mReceipt.ReceiptItems.CurrentItem.WarrantyInDays = Val(AppSettings("WarrantyForNewOH"))
					mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate = mReceipt.RecdDate
					mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate = CDate(DateAdd(DateInterval.Day, Val(AppSettings("WarrantyForNewOH")), mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate)).ToString(AppSettings("DateFormat").ToString)
				ElseIf (mOrderList.Item(Index1).OrderType = "Exchange" Or mOrderList.Item(Index1).OrderType = "Repair" Or mOrderList.Item(Index1).OrderType = "Lease") Then
					mReceipt.ReceiptItems.CurrentItem.IsWarranty = True
					mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate = mReceipt.RecdDate
					mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate = CDate(DateAdd(DateInterval.Month, Val(AppSettings("WarrantyForExchangeRepaired")), mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate))
					mReceipt.ReceiptItems.CurrentItem.WarrantyInDays = DateDiff(DateInterval.Day, mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate, mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate)
				End If
				If (mReceipt.TransTypeID <> 6 And mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = 2) Then 'Added By Prashant On 07-Oct-2015 For ALL06102015
					If mLastWarrantyInformation.Count > 0 Then
						mReceipt.ReceiptItems.CurrentItem.CodeNo = mLastWarrantyInformation(0).CodeNo

					End If
				End If
			Else 'Nonserialized
				mReceipt.ReceiptItems.CurrentItem.IsWarranty = False
				mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate = System.DBNull.Value
				mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate = System.DBNull.Value
				mReceipt.ReceiptItems.CurrentItem.WarrantyInDays = 0
			End If
		End If
		mReceipt.ReceiptItems.CurrentItem.ItemTagID = mPendingToReceiveTransItemList(Index:=Index).ItemTagID
		mReceipt.ReceiptItems.CurrentItem.ItemTagName = mPendingToReceiveTransItemList(Index:=Index).ItemTagName
		'----------------------------------------------------
		'Added by shital on 07-Sep-2016
		mReceipt.ReceiptItems.CurrentItem.IsAirworthiness = mPendingToReceiveTransItemList(Index).IsAirworthiness
		mReceipt.ReceiptItems.CurrentItem.IsWarrantyApplicableCheckedInOrderItem = mPendingToReceiveTransItemList(Index:=Index).IsWarrantyApplicable
		'vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
		'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
		mReceipt.ReceiptItems.CurrentItem.ReqEmployeeEmailIDs = mPendingToReceiveTransItemList(Index).ReqEmployeeEmailIDs
		mReceipt.ReceiptItems.CurrentItem.ReqNo = mPendingToReceiveTransItemList(Index).ReqNo
		mReceipt.ReceiptItems.CurrentItem.ReqEmployeeName = mPendingToReceiveTransItemList(Index).ReqEmployeeName
		mReceipt.ReceiptItems.CurrentItem.ReqQty = mPendingToReceiveTransItemList(Index).ReqQty
		mReceipt.ReceiptItems.CurrentItem.ReqDate = mPendingToReceiveTransItemList(Index).ReqDateFormatted.ToString
		mReceipt.ReceiptItems.CurrentItem.ReqEmployeeID = mPendingToReceiveTransItemList(Index).ReqEmployeeID
		mReceipt.ReceiptItems.CurrentItem.ReqItemID = mPendingToReceiveTransItemList(Index).ReqItemID
		'End
		mReceipt.ReceiptItems.CurrentItem.OrderCurrencyID = mPendingToReceiveTransItemList(Index).OrderCurrencyID 'Added By Vikrant On 12-Jun-2020 For All12062020



		'''Added by Saylee on 9-Mar-2021 for Heligo10032021
		If mReceipt.ReceiptItems.CurrentItem.IsSerialized = True Then 'Serialized
			If mLastWarrantyInformation.Count > 0 Then
				mReceipt.ReceiptItems.CurrentItem.ManufacturingDate = mLastWarrantyInformation(0).ManufacturingDate
			End If
		End If
		''***************************
		mLastWarrantyInformation = Nothing
		Session("mReceipt") = mReceipt
		Session("mTotalPendingItemQty") = CDec(mPendingToReceiveTransItemList(Index).PendingItemQty())
		Session("TotalCount") = CDec(mPendingToReceiveTransItemList(Index).PendingItemQty())
		Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=" & "wfReceipt_Ajax.aspx" & "&ChildPage1=" & "wfReceiptPendingOrderList_Ajax.aspx" & "&ItemNo=" & HttpUtility.UrlEncode(mItemName) & "&mType=" & mType)
	End Sub
	Private Sub ItemSelectionForRCI(ByVal Index As Integer, Optional ByVal IsSelectAll As Boolean = False) 'Parameter Added By Utkarsh On 22-Feb-2012 For ALL22022012
		mFromPartList = Session("mFromPartList")
		mPendingToReceiveTransItemList = Session("mPendingToReceiveTransItemList")
		mTransTypeID = mReceiptCumInvoice.TransTypeID
		Dim Index1 As Integer
		Index1 = Session("Index1")
		If mReceiptCumInvoice.IsNew Then
			mReceiptCumInvoice.RecCumInvDate = txtDate.Text
		End If

		'If (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 54 Or mReceiptCumInvoice.TransTypeID = 7) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1 'mReceiptCumInvoice.TransTypeID = 7 Added By Vikrant on 12-Jun-2020 For ALL12062020
		If (mReceiptCumInvoice.TransTypeID = 10 Or mReceiptCumInvoice.TransTypeID = 54) Then 'mReceiptCumInvoice.TransTypeID = 7 removed by vikrant on 10-Aug-2020 as per Heligo requirement as per maill discussed in meeting which was added for ALL12062020
			mReceiptCumInvoice.CurrencyID = mOrderList.Item(Index1).CurrencyID
			mReceiptCumInvoice.ConversionFactor = mOrderList.Item(Index1).ConversionFactor
		End If '---------------------------------------------

		If mReceiptCumInvoice.FromTypeID = 14 Then
			If mTransaction = Transaction.Order Then
				mReceiptCumInvoice.VendorID = mOrderList.Item(Index1).VendorID 'VendorList.GetVendortList(0).Item(mOrderList.Item(Index1).VendorName).ID 'Added By Prashant On 31-May-2018 For ALL31052018
				'Kalpesh (Supplier Loan Recovery_)
				mReceiptCumInvoice.VendorName = mOrderList.Item(Index1).VendorName
				mReceiptCumInvoice.OrderID = mOrderList.Item(Index1).ID 'ALL30082018
			ElseIf mTransaction = Transaction.Issue Then
				mReceiptCumInvoice.VendorID = mIssueList.Item(Index1).VendorID 'VendorList.GetVendortList(0).Item(mIssueList.Item(Index1).VendorName).ID 'Added By Prashant On 31-May-2018 For ALL31052018
				'Kalpesh (Supplier Loan Recovery_)
				mReceiptCumInvoice.VendorName = mIssueList.Item(Index1).VendorName
			End If
		ElseIf mReceiptCumInvoice.FromTypeID = 2 Then
			'mReceiptCumInvoice.AircraftID = tmpMachineList.GetMachineList().Item(mIssueList.Item(Index1).RegNo).ID
			mReceiptCumInvoice.AircraftID = MachineNameValueList.GetMachineList(Today.Date.ToString, , , , , , , , , ForInventory:=True).Item(mIssueList.Item(Index1).RegNo).ID
			'Kalpesh (Supplier Loan Recovery_)
			mReceiptCumInvoice.AircraftName = mIssueList.Item(Index1).RegNo
		ElseIf mReceiptCumInvoice.FromTypeID = 16 Then
			mReceiptCumInvoice.WorkShopID = mIssueList.Item(Index1).WorkShopID 'WorkShopList.GetWorkShopList(0).Item(mIssueList.Item(Index1).WorkShop).ID
			mReceiptCumInvoice.WorkShopName = mIssueList.Item(Index1).WorkShopName
		ElseIf mReceiptCumInvoice.FromTypeID = 17 Then
			mReceiptCumInvoice.WOID = nWO.GetWO(mIssueList.Item(Index1).WOID).ID
			mReceiptCumInvoice.WONumber = mIssueList.Item(Index1).WorkOrderNo
		ElseIf mReceiptCumInvoice.FromTypeID Then  ''-- 8
			mReceiptCumInvoice.StoreID = mIssueList.Item(Index1).StoreID 'StoreList.GetStoreList(0).Item(mIssueList.Item(Index1).StoreName).ID'Added By Prashant On 31-May-2018 For ALL31052018
			'Kalpesh (Supplier Loan Recovery_)
			mReceiptCumInvoice.StoreName = mIssueList.Item(Index1).StoreName
		End If
		'------------------------------------------------------------------
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = mPendingToReceiveTransItemList(Index).PrimaryCategoryID 'Added By Prashant On 07-Oct-2015 For ALL06102015
		If ((mPendingToReceiveTransItemList(Index).Type = 3 Or mPendingToReceiveTransItemList(Index).Type = 12 Or _
			 mPendingToReceiveTransItemList(Index).Type = 13 Or mPendingToReceiveTransItemList(Index).Type = 47) And _
		 mPendingToReceiveTransItemList(Index).IsSerialized = False) Or mPendingToReceiveTransItemList(Index).Type = 4 And _
		 mFromPartList = False Then    'If Order or Issue '19-06-2006

			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemList(Index).Type ''--
			If mPendingToReceiveTransItemList(Index).Type = 3 Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemID = mPendingToReceiveTransItemList(Index).OrderItemID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mPendingToReceiveTransItemList(Index).ItemTypeID 'Added By Prashant On 26-Nov-2014 For ALL24112014
				If (mReceiptCumInvoice.TransTypeID = 10) Then 'Added By Prashant 28-Oct-2013 --ALL25102013-1
					'AppSettings("ClientCode") = "BA" Added by Prashant on 4-May-2021 Heligo04052021
					If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 1 And AppSettings("ClientCode") = "BA" Then 'Rotables  'Added By Prashant 31-Jul-2019
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OriginalInvoiceRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OriginalInvoiceEffRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OriginalInvoiceRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCEffRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OriginalInvoiceEffRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor
					End If
					If AppSettings("ClientCode") = "Heligo" Then 'Added by Prashant on 4-May-2021 Heligo04052021
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
					Else
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CommercialRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor ''Added By Utkarsh ON 08-Oct-2013 FOR ALL07102013 
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
					End If
				ElseIf (mReceiptCumInvoice.TransTypeID = 54) Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
				Else
					'Added By Prashant 5-Feb-2019 ALL04022019
					'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
					'Commneted and added On 30-Jan-2020
					'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate * mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.Factor
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
					'End of Commneted and added On 30-Jan-2020

					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
					'End
				End If
				'Added By Prashant 5-Feb-2019 ALL04022019
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Factor = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.Factor
			End If
			If mPendingToReceiveTransItemList(Index).Type = 4 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemList(Index).IssueItemID ''--
			'This Checks whether Item Is Selected from PartList or Not
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = mFromPartList
			'Item of Aircraft/Store against Issue
			If (mPendingToReceiveTransItemList(Index).Type = 12 Or mPendingToReceiveTransItemList(Index).Type = 13) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = False) Then
				If mPendingToReceiveTransItemList(Index).Type = 12 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemList(Index).IssueItemID
				If mPendingToReceiveTransItemList(Index).Type = 13 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemList(Index).IssueItemID
				If mPendingToReceiveTransItemList(Index).Type = 47 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemList(Index).IssueItemID
			End If
			'This will Returns PartName and PartDescription For Item Selected from PartList
			If (mPendingToReceiveTransItemList(Index).Type = 12 Or mPendingToReceiveTransItemList(Index).Type = 13 Or mPendingToReceiveTransItemList(Index).Type = 47) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = True) Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPendingToReceiveTransItemList(Index).ItemID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPendingToReceiveTransItemList(Index).ItemName
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPendingToReceiveTransItemList(Index).ItemDescription
			End If


			'Kalpesh   - IF receipt is against Issue then SerialNo should come Automaticaly
			If mPendingToReceiveTransItemList(Index).Type = 4 Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.SerialNo

				'Added by Shital on 20-Sep-2019 Suggested by Prashant
				mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)
				If mLastWarrantyInformation.Count > 0 Then
					If (mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 61 Or mReceiptCumInvoice.TransTypeID = 62) Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = mLastWarrantyInformation(0).LastCalibrationDoneOnDateFormatted.ToString
						''mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConditionCheckDoneOnDate = mLastWarrantyInformation(0).DoneOnDateForConditionCheckFormatted.ToString
						''mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ServiedInspectedCheckDoneOnDate = mLastWarrantyInformation(0).DoneOnDateForServiceInspectedCheckFormatted.ToString
					End If
					mLastWarrantyInformation = Nothing
				End If
				'-----------------
				If IsSelectAll Then 'Added By Utkarsh On 22-Feb-2012 For ALL22022012
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.CalibrationDoneOnDate
				End If 'End

			ElseIf mPendingToReceiveTransItemList(Index).Type = 3 Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = ""
			End If
			If mReceiptCumInvoice.TransTypeID = 7 Then 'Added By Prashant 5-Feb-2019 ALL04022019
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).OrderItemUnitID 'Added By Prashant 5-Feb-2019 ALL04022019
			ElseIf mPendingToReceiveTransItemList(Index).Type = 4 Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).IssueItemUnitID
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).UnitID
			End If
			If mReceiptCumInvoice.TransTypeID = 8 Then 'Store transfer then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(mPendingToReceiveTransItemList(Index).IssueItemDisplayQty) 'Added By Prashant 11-May-2010
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(mPendingToReceiveTransItemList(Index).PendingItemQty) 'Added By Prashant 11-May-2010
			End If

			If mPendingToReceiveTransItemList(Index).Type = 4 Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InvoiceItemRateForRCI 'Added by Prashant 30-Aug-2013 ALL30082013-1
				'Added By Prashant 5-Feb-2019 ALL04022019
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate
				'Added By Prashant On 27-Apr-2021 ALL26042021
				If mReceiptCumInvoice.TransTypeID = 8 Then 'Store transfer then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RateEffRateDiffrenceForRCI * CDec(mPendingToReceiveTransItemList(Index).IssueItemDisplayQty))
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RateEffRateDiffrenceForRCI * CDec(mPendingToReceiveTransItemList(Index).PendingItemQty))
				End If
				'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RateEffRateDiffrenceForRCI 'Added by Prashant 30-Aug-2013 ALL30082013-1
				'End of Added By Prashant On 27-Apr-2021 ALL26042021
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteNo = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ReleaseNoteNo
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReleaseNoteDate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ReleaseNoteDate
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ItemTypeID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ExpiryDate
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpQtrs = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ExpQtrs
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpYear = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ExpYear
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureQtrs = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.CureQtrs
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CureYear = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.CureYear
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BatchNo = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.BatchNo
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.StartDate
			End If

			'19-10-2006
			If mTransTypeID = Util.Trans.ReceiptAgainstLoanIssuedToStore Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ToStoreID
			ElseIf mTransTypeID = Util.Trans.LoanTakenFromStore Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ToStoreID
			ElseIf mTransTypeID = Util.Trans.ReceiptAgainstLoanIssuedToAircraft Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
			ElseIf mTransTypeID = Util.Trans.ReceivedFromOtherStore Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ToStoreID
				'Added By Utkarsh On 22-Feb-2012 For ALL22022012
				If IsSelectAll Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreName = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ToStoreName
				End If
				'End
				'Kalpesh (Supplier Loan Recovery_32)
			ElseIf mTransTypeID = Util.Trans.ReceiptAgainstLoanIssueToVendor Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
				'Kalpesh (Supplier Loan Recovery_34)
			ElseIf mTransTypeID = Util.Trans.ReceiptAgainstLoanIssueToCustomer Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
			ElseIf mTransTypeID = Util.Trans.ReceiptAgainstLoanIssuedToWorkShop Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
			ElseIf mTransTypeID = Util.Trans.RCIFromWorkOrderAsReturn Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
			End If

			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemList(Index).OriginalReceiptDate
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemList(Index).OriginalReceiptTextNo
			'if ReceiptItem is New and Receipt is against Order and Receiveing Part is Serialized
		ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew And mPendingToReceiveTransItemList(Index).Type = 3 And mPendingToReceiveTransItemList(Index).IsSerialized = True And mFromPartList = False Then '19-06-2006  
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemList(Index).Type
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemID = mPendingToReceiveTransItemList(Index).OrderItemID
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mPendingToReceiveTransItemList(Index).ItemTypeID 'Added By Prashant On 26-Nov-2014 For ALL24112014
			If (mReceiptCumInvoice.TransTypeID = 10) Then
				'AppSettings("ClientCode") = "BA" Added by Prashant on 4-May-2021 Heligo04052021
				If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 1 And AppSettings("ClientCode") = "BA" Then 'Rotables  'Added By Prashant 31-Jul-2019
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OriginalInvoiceRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CEffRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OriginalInvoiceEffRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OriginalInvoiceRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCEffRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OriginalInvoiceEffRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor
				End If
				If AppSettings("ClientCode") = "Heligo" Then 'Added by Prashant on 4-May-2021 Heligo04052021
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
				Else
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CommercialRate / mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.OrderConversionFactor ''Added By Utkarsh ON 08-Oct-2013 FOR ALL07102013 
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
				End If
			ElseIf (mReceiptCumInvoice.TransTypeID = 54) Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.GROCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
			Else
				'Added By Prashant 5-Feb-2019 ALL04022019
				'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
				'Commneted and added On 30-Jan-2020
				'mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate * mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.Factor
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
				'End of Commneted and added On 30-Jan-2020
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.CRate
				'End
			End If
			'Added By Prashant 5-Feb-2019 ALL04022019
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Factor = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemDetailForReceipt.Factor
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = mFromPartList
			If mReceiptCumInvoice.TransTypeID = 7 Then 'Added By Prashant 5-Feb-2019 ALL04022019
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).OrderItemUnitID 'Added By Prashant 5-Feb-2019 ALL04022019
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).UnitID
			End If
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(IIf(mPendingToReceiveTransItemList(Index).IsSerialized, 1, mPendingToReceiveTransItemList(Index).PendingItemQty)) 'CDec(mPendingToReceiveTransItemList(Index).PendingItemQty)
			'05-09-2006
			'If mPendingToReceiveTransItemList(Index).ExpiryMonth > 0 Then
			'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mReceiptCumInvoice.Receipt.RecdDate
			'    If Not (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate) Is System.DBNull.Value Then
			'        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = CDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate).AddMonths(mPendingToReceiveTransItemList(Index).ExpiryMonth)
			'    End If
			'End If
			'19-10-2006
			If mTransTypeID = Util.Trans.ReceiptAgainstLoanIssuedToStore Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ToStoreID
			ElseIf mTransTypeID = Util.Trans.LoanTakenFromStore Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ToStoreID
			ElseIf mTransTypeID = Util.Trans.ReceiptAgainstLoanIssuedToAircraft Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
				'Kalpesh (Supplier Loan Recovery_32)
			ElseIf mTransTypeID = Util.Trans.ReceiptAgainstLoanIssuedToWorkShop Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
			ElseIf mTransTypeID = Util.Trans.ReceiptAgainstLoanIssueToVendor Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
				'Kalpesh (Supplier Loan Recovery_34)
			ElseIf mTransTypeID = Util.Trans.ReceiptAgainstLoanIssueToCustomer Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.FromStoreID
			End If
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemList(Index).OriginalReceiptDate
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemList(Index).OriginalReceiptTextNo
		ElseIf mFromPartList = True Then '19-06-2006
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemList(Index).Type
			'This Checks whether Item Is Selected from PartList or Not
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = mFromPartList
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartFromListisSerialized = mPendingToReceiveTransItemList(Index).IsSerialized
			'This will Returns PartName and PartDescription For Item Selected from PartList
			If (mPendingToReceiveTransItemList(Index).Type = 12 Or mPendingToReceiveTransItemList(Index).Type = 13 Or mPendingToReceiveTransItemList(Index).Type = 47) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = True) Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPendingToReceiveTransItemList(Index).ItemID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPendingToReceiveTransItemList(Index).ItemName
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPendingToReceiveTransItemList(Index).ItemDescription
			End If
			If mReceiptCumInvoice.TransTypeID = 7 Then 'Added By Prashant 5-Feb-2019 ALL04022019
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).OrderItemUnitID 'Added By Prashant 5-Feb-2019 ALL04022019
			Else
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).UnitID
			End If
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(IIf(mPendingToReceiveTransItemList(Index).IsSerialized, 1, mPendingToReceiveTransItemList(Index).PendingItemQty))
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemList(Index).OriginalReceiptDate
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemList(Index).OriginalReceiptTextNo
		End If
		'Added By Vikrant on 16-Aug-2012 All14082012-1
		If mReceiptCumInvoice.TransTypeID = 10 Then
			If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True Then
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = mPendingToReceiveTransItemList(Index).SerialNo
			End If
			'Added By Vikrant On 02-May-2013 For BA30042013-2
			If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BatchNo = mPendingToReceiveTransItemList(Index).OrderTextNo
			End If
			'End
		End If
		'End

		'vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
		'Added By Prashant 17-Feb-2015-----------------------
		If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True Then 'Serialized
			mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)

			If (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10) Then
				If (mOrderList.Item(Index1).OrderType = "New Purchase" Or mOrderList.Item(Index1).OrderType = "Overhaul") Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = True
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = Val(AppSettings("WarrantyForNewOH"))
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = mReceiptCumInvoice.RecCumInvDate
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = CDate(DateAdd(DateInterval.Day, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate)).ToString(AppSettings("DateFormat").ToString)
				ElseIf (mOrderList.Item(Index1).OrderType = "Exchange" Or mOrderList.Item(Index1).OrderType = "Repair" Or mOrderList.Item(Index1).OrderType = "Lease") Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = True
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = mReceiptCumInvoice.RecCumInvDate
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = CDate(DateAdd(DateInterval.Month, Val(AppSettings("WarrantyForExchangeRepaired")), mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate))
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = DateDiff(DateInterval.Day, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate)
				End If
			ElseIf (mReceiptCumInvoice.TransTypeID = 8 Or mReceiptCumInvoice.TransTypeID = 11 Or mReceiptCumInvoice.TransTypeID = 12 Or mReceiptCumInvoice.TransTypeID = 13 Or mReceiptCumInvoice.TransTypeID = 27 Or mReceiptCumInvoice.TransTypeID = 28 Or mReceiptCumInvoice.TransTypeID = 47 Or mReceiptCumInvoice.TransTypeID = 54) Then
				If mLastWarrantyInformation.Count > 0 Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = mLastWarrantyInformation(0).IsWarranty
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = mLastWarrantyInformation(0).WarrantyInDays
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = mLastWarrantyInformation(0).WarrantyStartDateFormatted.ToString
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = mLastWarrantyInformation(0).WarrantyExpiryDateFormatted.ToString

				End If
			End If
			If (mReceiptCumInvoice.TransTypeID <> 7 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2) Then 'Added By Prashant On 07-Oct-2015 For ALL06102015
				' mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)
				If mLastWarrantyInformation.Count > 0 Then
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = mLastWarrantyInformation(0).CodeNo
					'mLastWarrantyInformation = Nothing
				End If
			End If
			If mLastWarrantyInformation.Count > 0 Then ''Added by Saylee on 9-Mar-2021 for Heligo10032021
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ManufacturingDate = mLastWarrantyInformation(0).ManufacturingDate
			End If
			mLastWarrantyInformation = Nothing
		Else 'Nonserialized
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = False
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = System.DBNull.Value
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = System.DBNull.Value
			mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = 0
		End If
		'----------------------------------------------------
		'If (AppSettings("ClientCode") = "BA"  Or AppSettings("ClientCode") = "Novo"  And mReceiptCumInvoice.TransTypeID = 10 And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 1 Or mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2)) Then
		'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsConsiderAsAsset = True
		'End If
		'vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
		'Added by shital on 07-Sep-2016
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAirworthinss = mPendingToReceiveTransItemList(Index).IsAirworthiness
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID = mPendingToReceiveTransItemList(Index:=Index).ItemTagID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName = mPendingToReceiveTransItemList(Index:=Index).ItemTagName
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarrantyApplicableCheckedInOrderItem = mPendingToReceiveTransItemList(Index:=Index).IsWarrantyApplicable

		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTPercentage = mPendingToReceiveTransItemList(Index:=Index).CGSTPercentage
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTPercentage = mPendingToReceiveTransItemList(Index:=Index).SGSTPercentage
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTPercentage = mPendingToReceiveTransItemList(Index:=Index).IGSTPercentage

		'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeEmailIDs = mPendingToReceiveTransItemList(Index).ReqEmployeeEmailIDs
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqNo = mPendingToReceiveTransItemList(Index).ReqNo
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeName = mPendingToReceiveTransItemList(Index).ReqEmployeeName
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqQty = mPendingToReceiveTransItemList(Index).ReqQty
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqDate = mPendingToReceiveTransItemList(Index).ReqDateFormatted.ToString
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeID = mPendingToReceiveTransItemList(Index).ReqEmployeeID
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqItemID = mPendingToReceiveTransItemList(Index).ReqItemID
		'End
		mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartCategory = mPendingToReceiveTransItemList(Index).PartCategory 'Added by Vikrant on 14-Apr-2021 for ALL14042021
		mReceiptCumInvoice.ApplyEdit()
		Session("mFromPartList") = mFromPartList
		Session("mReceiptCumInvoice") = mReceiptCumInvoice
		Session("mTotalPendingItemQty") = CDec(mPendingToReceiveTransItemList(Index).PendingItemQty())
		Session("TotalCount") = CDec(mPendingToReceiveTransItemList(Index).PendingItemQty())
		mItemName = mPendingToReceiveTransItemList(Index).ItemName
		Session("Edit") = False
		If Not IsSelectAll Then 'Changed By Utkarsh On 22-Feb-2012 For ALL22022012
			Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?ChildPage1=" & "wfReceiptPendingOrderList_Ajax.aspx&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage"))
		End If 'End
	End Sub
#End Region

#Region " DataFieldBind "
	Private Sub DataFieldBind() 'Added By Prashant 29-Jun-2023
		mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
		cmbOrderText.DataSource = mDistinctTextListForOrder
		cmbOrderText.DataBind()
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		GetSession()
		mType = Request.QueryString("mType")
		Session("mType") = mType
		If txtDate.Text.ToString = "" Then
			txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
		End If
		If Not IsPostBack And Session("Sender") = "" Then
			ClearAll()
			DataFieldBind()
			If mPrevTransID.Equals(Guid.Empty) Then
				rdbFromAllPendingOrder.Checked = True
			Else
				rdbFromLastOrder.Checked = True
				If mReceiptCumInvoice IsNot Nothing Then
					If (mReceiptCumInvoice.TransTypeID = 62) Then
						rdbFromAllPendingOrder.Enabled = False
					End If
				End If
			End If
			If mTransaction = Transaction.Order Then
				rdbFromLastOrder.Text = "From Last Order"
				rdbFromAllPendingOrder.Text = "From All Pending Order (s)"
				lblLedgerList.Text = "List of Pending Orders"
				btnFindNow.ToolTip = "Click to find list of Orders till date"
			ElseIf mTransaction = Transaction.Issue Then
				rdbFromLastOrder.Text = "From Last Issue"
				rdbFromAllPendingOrder.Text = "From All Pending Issue (s)"
				lblLedgerList.Text = "List of Pending Issues"
				btnFindNow.ToolTip = "Click to find list of Issues till date"
			ElseIf mTransaction = Transaction.Receipt Then
				rdbFromLastOrder.Text = "From Last Receipt"
				rdbFromAllPendingOrder.Text = "From All Pending Receipt (s)"
				lblLedgerList.Text = "List of Pending Receipts"
				btnFindNow.ToolTip = "Click to find list of Receipts till date"
			End If

			If mType = 1 Then       'Receipt
				ControlVisibilityReceipt()
				FindNow()
			ElseIf mType = 2 Then   'ReceiptcumInvoice
				ControlVisibilityRCI()
				FindNow1()
			ElseIf mType = 3 Then   'for Invoice
				ControlVisibilityInvoice()
				FindNow2()
			End If
		End If
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click, txtDate.TextChanged
		dgOrderList.PageIndex = 0
		dgIssueList.PageIndex = 0
		dgReceiptList.PageIndex = 0
		dgTransItemList.PageIndex = 0
		dgItemReceiptDetail.PageIndex = 0
		If mType = 1 Then       'Receipt
			FindNow()
		ElseIf mType = 2 Then   'ReceiptCumInvoice
			FindNow1()
		ElseIf mType = 3 Then   'for Invoice
			mFromID = mInvoice.VendorID
			Session("mFromID") = mFromID
			FindNow2()
		End If
	End Sub
	Private Sub dgOrderList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgOrderList.RowCommand
		Select Case e.CommandName
			Case "SelectRec"
				Dim Index1 As Integer = CInt(e.CommandArgument) + dgOrderList.PageIndex * dgOrderList.PageSize
				mTransaction = Session("mTransaction")
				dgTransItemList.Visible = True
				If mType = 1 Then       'Receipt
					mPendingToReceiveTransItemList = PendingToReceiveTransItemList.GetPendingToReceiveTransItemList(mReceipt.TransTypeID, mReceipt.VendorID, 0, txtDate.Text.ToString, mOrderList.Item(Index1).ID, IsFromIssueBERParts:=False)
				ElseIf mType = 2 Then   'ReceiptCumInvoice()
					mPendingToReceiveTransItemList = PendingToReceiveTransItemList.GetPendingToReceiveTransItemList(mReceiptCumInvoice.TransTypeID, mReceiptCumInvoice.VendorID, 0, txtDate.Text.ToString, mOrderList.Item(Index1).ID, IsFromIssueBERParts:=False)
				End If
				Session("Index1") = Index1
				dgTransItemList.DataSource = mPendingToReceiveTransItemList
				lblTransItemListResult.Visible = True
				If mPendingToReceiveTransItemList.Count <> 0 Then
					lblTransItemListResult.Visible = True
					lblTransItemListResult.Text = "List of Parts as per criteria :" & mPendingToReceiveTransItemList.Count & " Record(s) found." '"List of Items : " + mPendingToReceiveTransItemList.Count + " Record (s) found"
				End If
				Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList
				dgTransItemList.PageIndex = 0
				dgTransItemList.DataBind()
				upnlTransItemList.Update()
		End Select
	End Sub
	Private Sub dgIssueList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgIssueList.RowCommand
		Select Case e.CommandName
			Case "SelectRec"
				Dim Index2 As Integer = CInt(e.CommandArgument) + dgIssueList.PageIndex * dgIssueList.PageSize
				'--------------------------------------------
				If mTransTypeID = 8 Or mTransTypeID = 12 Then
					mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mIssueList(Index2).ToStoreID.ToString) ''Added By Prashant 13-May-2020
					If mUserHasNoStoreRights.Count > 0 Then
						MSGBoxCtrl.Show("Alert!", $"We're sorry, but you do not have the necessary rights or permissions to access the store: {mIssueList(Index2).ToStoreName}.{Environment.NewLine} To gain access, please contact your Administrator.", "", MsgBoxStyle.OkOnly, "ResetStore")
						Exit Sub
					End If
				End If
				If mTransTypeID = 13 Or mTransTypeID = 27 Or mTransTypeID = 28 Or mTransTypeID = 47 Or mTransTypeID = 62 Then
					mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mIssueList(Index2).StoreID.ToString) ''Added By Prashant 13-May-2020
					If mUserHasNoStoreRights.Count > 0 Then
						MSGBoxCtrl.Show("Alert!", $"We're sorry, but you do not have the necessary rights or permissions to access the store: {mIssueList(Index2).StoreName}.{Environment.NewLine} To gain access, please contact your Administrator.", "", MsgBoxStyle.OkOnly, "ResetStore")
						Exit Sub
					End If
				End If
				'-------------------------------------------- ''End of Added By Prashant 13-May-2020
				dgTransItemList.Visible = True
				If Index2 >= 0 Then
					If mType = 1 Then    'Receipt
						mPendingToReceiveTransItemList = PendingToReceiveTransItemList.GetPendingToReceiveTransItemList(mReceipt.TransTypeID, mFromID, 0, mReceipt.RecdDate.ToString, mIssueList.Item(Index2).ID, IIf(chkReturnableBackFromCustomer.Checked, True, False), IsFromIssueBERParts:=False) 'ALL21052012-05
					ElseIf mType = 2 Then  'ReceiptCumInvoice
						mPendingToReceiveTransItemList = PendingToReceiveTransItemList.GetPendingToReceiveTransItemList(mReceiptCumInvoice.TransTypeID, mFromID, 0, mReceiptCumInvoice.RecCumInvDate.ToString, mIssueList.Item(Index2).ID, IIf(chkReturnableBackFromCustomer.Checked, True, False), ItemID.ToString, IsFromIssueBERParts:=False)   'ALL21052012-05

						If mReceiptCumInvoice.TransTypeID = 8 And mPendingToReceiveTransItemList.Count > 0 Then 'Added By Utkarsh ON 23-Feb-2012 For ALL22022012
							lnkSelectAll.Visible = True
						End If 'End

					End If
					Session("Index1") = Index2
					dgTransItemList.DataSource = mPendingToReceiveTransItemList

					If mPendingToReceiveTransItemList.Count <> 0 Then
						lblTransItemListResult.Visible = True
						lblTransItemListResult.Text = "List of Parts as per criteria :" & mPendingToReceiveTransItemList.Count & " Record(s) found."
					End If
					Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList
					dgTransItemList.PageIndex = 0
					dgTransItemList.DataBind()
					upnlTransItemList.Update()
				End If
		End Select
	End Sub
	Private Sub dgReceiptList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReceiptList.RowCommand
		Select Case e.CommandName
			Case "SelectRec"
				Dim Index3 As Integer = CInt(e.CommandArgument) + dgReceiptList.PageIndex * dgReceiptList.PageSize
				mTransaction = Session("mTransaction")
				mReceiptList = Session("mReceiptList")
				If mType = 3 Then    'Receipt
					mPendingReceiptItemList = PendingInvoiceList.GetPendingToInvoiceList(mFromID, "", mInvoice.InvoiceDate.ToString, mReceiptList.Item(Index3).ID.ToString, CurrencyID:=mInvoice.CurrencyID.ToString)
					Session("mPendingReceiptItemList") = mPendingReceiptItemList
					Session("Index") = Index3
					mVendorID = mReceiptList.Item(Index3).VendorID
					mDCNo = mReceiptList.Item(Index3).DCNO 'Added by Saylee on 20-june-2011
					If mReceiptList.Item(Index3).DCDate.ToString = "" Then
						mDCDate = ""
					Else
						mDCDate = CType(mReceiptList.Item(Index3).DCDate, String)
					End If
					mReceiptID = mReceiptList.Item(Index3).ID
					mAWBNo = mReceiptList.Item(Index3).AWBNo '**************************************
					Session("mVendorID") = mVendorID
					Session("mDCNo") = mDCNo 'Added by Saylee on 20-june-2011
					Session("mDCDate") = mDCDate
					Session("mReceiptID") = mReceiptID
					Session("mAWBNo") = mAWBNo '*************************************
					dgItemReceiptDetail.DataSource = mPendingReceiptItemList
					dgItemReceiptDetail.DataBind()
					dgItemReceiptDetail.Visible = True
					lblItemReceiptDetailResult.Visible = True
					btnDone.Enabled = (dgItemReceiptDetail IsNot Nothing) AndAlso (dgItemReceiptDetail.Rows.Count > 0)
					If mPendingReceiptItemList.Count <> 0 Then
						lblItemReceiptDetailResult.Text = "List of Parts as per criteria :" & mPendingReceiptItemList.Count & " Record(s) found."
					End If
					Session("mPendingReceiptItemList") = mPendingReceiptItemList
					dgItemReceiptDetail.PageIndex = 0
					dgItemReceiptDetail.DataBind()
					upnlItemReceiptDetail.Update()
					upnlButtons.Update()
				End If
		End Select
	End Sub
	Private Sub dgTransItemList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTransItemList.RowCommand
		mType = Session("mType")
		Select Case e.CommandName
			Case "SelectRec"
				Dim Index4 As Integer = CInt(e.CommandArgument) + dgTransItemList.PageIndex * dgTransItemList.PageSize
				If mType = 1 Then
					ItemSelectionForReceipt(Index4)
				ElseIf mType = 2 Then
					ItemSelectionForRCI(Index4)
				End If
		End Select
	End Sub
	Private Sub dgOrderList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgOrderList.PageIndexChanging
		dgOrderList.PageIndex = e.NewPageIndex
		dgTransItemList.Visible = True
		lblTransItemListResult.Visible = True
		dgOrderList.DataSource = mOrderList
		Session("mOrderList") = mOrderList
		dgOrderList.Visible = True
		dgOrderList.DataBind()
	End Sub
	Private Sub dgIssueList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgIssueList.PageIndexChanging
		dgIssueList.PageIndex = e.NewPageIndex
		dgTransItemList.Visible = True
		lblTransItemListResult.Visible = True
		dgIssueList.DataSource = mIssueList
		Session("mIssueList") = mIssueList
		dgIssueList.Visible = True
		dgIssueList.DataBind()
	End Sub
	Private Sub dgReceiptList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgReceiptList.PageIndexChanging
		dgReceiptList.PageIndex = e.NewPageIndex
		mReceiptList = Session("mReceiptList")
		dgReceiptList.DataSource = mReceiptList
		Session("mPendingReceiptItemList") = mPendingReceiptItemList
		Session("mReceiptList") = mReceiptList
		dgReceiptList.Visible = True
		dgReceiptList.DataBind()
	End Sub
	Private Sub dgTransItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTransItemList.PageIndexChanging
		dgTransItemList.PageIndex = e.NewPageIndex
		dgTransItemList.DataSource = mPendingToReceiveTransItemList
		Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList
		dgTransItemList.Visible = True
		dgTransItemList.DataBind()
	End Sub
	Private Sub dgItemReceiptDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgItemReceiptDetail.PageIndexChanging
		SetMultipleObject()
		dgItemReceiptDetail.PageIndex = e.NewPageIndex
		dgItemReceiptDetail.DataSource = mPendingReceiptItemList
		Session("mPendingReceiptItemList") = mPendingReceiptItemList
		dgItemReceiptDetail.Visible = True
		btnDone.Enabled = (dgItemReceiptDetail IsNot Nothing) AndAlso (dgItemReceiptDetail.Rows.Count > 0)
		dgItemReceiptDetail.DataBind()
		upnlButtons.Update()
	End Sub
	Private Sub rdbFromAllPendingOrder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbFromAllPendingOrder.CheckedChanged
		mIsAll = True
	End Sub
	Private Sub rdbFromLastOrder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbFromLastOrder.CheckedChanged
		mIsAll = False
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Session("Edit") = False
		If Session("OpenFrom") = "1" Then
			If mType = 1 And Request.QueryString("BackPage") = "wfReceipt_Ajax.aspx" Then   'Receipt
				mReceipt.ReceiptItems.Remove(mReceipt.ReceiptItems.CurrentItem)
				Session("mReceipt") = mReceipt
			ElseIf mType = 2 And Request.QueryString("BackPage") = "wfReceiptCumInvoice_Ajax.aspx" Then 'ReceiptcumInvoice
				'mReceiptCumInvoice.CancelEdit()
				mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
			ElseIf mType = 3 And Request.QueryString("BackPage") = "wfInvoice_Ajax.aspx" Then 'for Invoice
				mInvoice.InvoiceItems.Remove(mInvoice.InvoiceItems.CurrentItem)
				Session("mInvoice") = mInvoice
			End If
			Response.Redirect(Request.QueryString("BackPage"))
		Else
			Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub
	Private Sub SetMultipleObject()
		Dim chkSelect As CheckBox
		Dim Recordno, PageItems As Integer
		mPendingReceiptItemList = Session("mPendingReceiptItemList")
		mSelectList = Session("mSelectList")
		PageItems = dgItemReceiptDetail.Rows.Count - 1
		If mSelectList Is Nothing Then ReDim mSelectList(mPendingReceiptItemList.Count - 1)

		For I As Integer = 0 To PageItems
			Recordno = I + dgItemReceiptDetail.PageSize * dgItemReceiptDetail.PageIndex
			chkSelect = CType(dgItemReceiptDetail.Rows(I).FindControl("chkSelect"), CheckBox)
			mSelectList(Recordno) = chkSelect.Checked
		Next
		Session("mPendingReceiptItemList") = mPendingReceiptItemList
		Session("mSelectList") = mSelectList
	End Sub
	Private Sub SetObject()
		mPendingReceiptItemList = Session("mPendingReceiptItemList")
		mSelectList = Session("mSelectList")
		If mInvoice.IsNew Then
			If txtDate.Text.ToString = "" Then
				txtDate.Text = Today.Date
			Else
				mInvoice.InvoiceDate = txtDate.Text
			End If
		End If
		Dim ind As Integer
		Dim tmpIICounter As Integer = -1
		For ind = 0 To mPendingReceiptItemList.Count - 1
			If mSelectList(ind) = True Then
				tmpIICounter = ind
				Exit For
			End If
		Next
		If tmpIICounter >= 0 Then
			mVendorID = Session("mVendorID")
			mDCNo = Session("mDCNo") 'Added by Saylee on 20-june-2011
			mDCDate = Session("mDCDate")
			mReceiptID = Session("mReceiptID")
			mAWBNo = Session("mAWBNo") '***********************************************
			mInvoice.VendorID = mVendorID
			'Added By Vikrant On 12-Jun-2020 For ALL12062020
			If mInvoice.TransTypeID = 21 Or mInvoice.TransTypeID = 10 Then
				mInvoice.CurrencyID = mPendingReceiptItemList(tmpIICounter).CurrecnyID
				mInvoice.ConversionFactor = mPendingReceiptItemList(tmpIICounter).CurrencyConversionFactor
			End If
			'End
			mInvoice.InvoiceItems.Add(mInvoice.ID)
			mInvoice.InvoiceItems.CurrentIndex = mInvoice.InvoiceItems.Count - 1
			mInvoice.InvoiceItems.CurrentItem.ReceiptItemID = mPendingReceiptItemList(tmpIICounter).ReceiptItemID
			'Added By Prashant 5-Feb-2019 ALL04022019
			mInvoice.InvoiceItems.CurrentItem.DisplayUnitName = mPendingReceiptItemList(tmpIICounter).DisplayUnitName
			mInvoice.InvoiceItems.CurrentItem.BaseUnitID = mPendingReceiptItemList(tmpIICounter).BaseUnitID
			mInvoice.InvoiceItems.CurrentItem.DisplayUnitID = mPendingReceiptItemList(tmpIICounter).DisplayUnitID
			If mPendingReceiptItemList(tmpIICounter).IsSerialized = True Then
				mInvoice.InvoiceItems.CurrentItem.Qty = 1
				'mInvoice.InvoiceItems.CurrentItem.DisplayQty = 1
			Else
				mInvoice.InvoiceItems.CurrentItem.Qty = mPendingReceiptItemList(tmpIICounter).BalanceQty '/ mPendingReceiptItemList(tmpIICounter).Factor
				'mInvoice.InvoiceItems.CurrentItem.DisplayQty = mPendingReceiptItemList(tmpIICounter).BalanceQty
				'mInvoice.InvoiceItems.CurrentItem.DisplayQty = mPendingReceiptItemList(tmpIICounter).DisplayQty
			End If
			mInvoice.ReceiptID = mReceiptID
			mInvoice.DCNO = mDCNo 'Added by Saylee on 20-june-2011
			mInvoice.DCDate = mDCDate
			mInvoice.AWBNo = mAWBNo '*****************************
			If mInvoice.TransTypeID = 10 Then ''Added By Utkarsh ON 08-Oct-2013 FOR ALL07102013 
				mInvoice.InvoiceItems.CurrentItem.CCommercialRate = mPendingReceiptItemList(tmpIICounter).CCommercialRate
				mInvoice.InvoiceItems.CurrentItem.GROCRate = mPendingReceiptItemList(tmpIICounter).OrderRate
			Else
				'Added By Prashant 5-Feb-2019 ALL04022019
				'mInvoice.InvoiceItems.CurrentItem.CRate = mPendingReceiptItemList(tmpIICounter).OrderRate
				'Commneted and added On 30-Jan-2020
				'mInvoice.InvoiceItems.CurrentItem.CRate = (mPendingReceiptItemList(tmpIICounter).OrderRate * mPendingReceiptItemList(tmpIICounter).Factor)
				mInvoice.InvoiceItems.CurrentItem.CRate = (mPendingReceiptItemList(tmpIICounter).OrderRate)
				'End of Commneted and added On 30-Jan-2020
				'mInvoice.InvoiceItems.CurrentItem.DisplayCRate = mPendingReceiptItemList(tmpIICounter).OrderRate
			End If 'End
			'Added By Prashant 5-Feb-2019 ALL04022019
			mInvoice.InvoiceItems.CurrentItem.Factor = mPendingReceiptItemList(tmpIICounter).Factor

			mInvoice.InvoiceItems.CurrentItem.CGSTPercentage = mPendingReceiptItemList(tmpIICounter).CGSTPercentage
			mInvoice.InvoiceItems.CurrentItem.SGSTPercentage = mPendingReceiptItemList(tmpIICounter).SGSTPercentage
			mInvoice.InvoiceItems.CurrentItem.IGSTPercentage = mPendingReceiptItemList(tmpIICounter).IGSTPercentage
			Session("mInvoice") = mInvoice
			Session("mPendingReceiptItemList") = mPendingReceiptItemList
			Session("mSelectList") = mSelectList
			Session("OpenFrom") = "Pending"
			Session("PendingReceipt") = "True"
			Session("tmpIICounter") = tmpIICounter + 1
			Response.Redirect("wfInvoiceItem_Ajax.aspx?BackPage=" & "wfInvoice_Ajax.aspx" & "&ChildPage1=" & "wfReceiptPendingOrderList_Ajax.aspx" & "&ItemNo=" & HttpUtility.UrlEncode(mItemName) & "&mType=" & mType)
		Else
			SetSession()
			Session("Sender") = ""
			MSGBoxCtrl.show("Selection Alert!", "Selection Alert ! ", "Please Select at least one Item from List", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
	End Sub
	Private Sub btnDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDone.Click
		SetMultipleObject()
		SetObject()
	End Sub
	Private Sub btnCreateOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateOrder.Click
		Session("mReceipt") = mReceipt
		Session.Remove("OrderNo") 'Added by rajnish on 16-01-2008. OrderNo is use in Receiptlist and AutoOrdercreation Both.
		Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList
		'Coad Added 
		'DEVEN 19/03/2008
		If mReceiptCumInvoice IsNot Nothing Then
			If mReceiptCumInvoice.TransTypeID = 7 Then
				Response.Redirect("wfAutocOrderCreation_Ajax.aspx?BackPage=wfReceiptCumInvoice_Ajax.aspx&ChildPage1=wfReceiptPendingOrderList_Ajax.aspx&ChildPage=" & Request.QueryString("ChildPage") & "&ItemNo=" & HttpUtility.UrlEncode(mItemName) & "&mType=" & mType)
			Else
				Response.Redirect("wfAutocOrderCreation_Ajax.aspx?BackPage=wfReceipt_Ajax.aspx&ChildPage1=wfReceiptPendingOrderList_Ajax.aspx&ChildPage=" & Request.QueryString("ChildPage") & "&ItemNo=" & HttpUtility.UrlEncode(mItemName) & "&mType=" & mType)
			End If
		Else
			Response.Redirect("wfAutocOrderCreation_Ajax.aspx?BackPage=wfReceipt_Ajax.aspx&ChildPage1=wfReceiptPendingOrderList_Ajax.aspx&ChildPage=" & Request.QueryString("ChildPage") & "&ItemNo=" & HttpUtility.UrlEncode(mItemName) & "&mType=" & mType)
		End If
	End Sub
	'Private Sub txtDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
	'    dgOrderList.PageIndex = 0
	'    dgIssueList.PageIndex = 0
	'    dgReceiptList.PageIndex = 0
	'    dgTransItemList.PageIndex = 0
	'    dgItemReceiptDetail.PageIndex = 0
	'    If mType = 1 Then   'Receipt
	'        FindNow()
	'    ElseIf mType = 2 Then  'ReceiptCumInvoice
	'        FindNow1()
	'    ElseIf mType = 3 Then 'for Invoice
	'        mFromID = mInvoice.VendorID
	'        Session("mFromID") = mFromID
	'        FindNow2()
	'    End If
	'End Sub
	'New addition by Rupali on 22-Jun-09 for Sorting Order
	Private Sub dgOrderList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgOrderList.Sorting
		mOrderList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mOrderList") = mOrderList
		dgOrderList.DataSource = mOrderList
		dgOrderList.DataBind()
	End Sub
	Private Sub dgIssueList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgIssueList.Sorting
		mIssueList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mIssueList") = mIssueList
		dgIssueList.DataSource = mIssueList
		dgIssueList.DataBind()
	End Sub
	Private Sub dgReceiptList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgReceiptList.Sorting
		mReceiptList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mReceiptList") = mReceiptList
		dgReceiptList.DataSource = mReceiptList
		dgReceiptList.DataBind()
	End Sub
	Private Sub dgItemReceiptDetail_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgItemReceiptDetail.Sorting
		mPendingReceiptItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mPendingReceiptItemList") = mPendingReceiptItemList
		dgItemReceiptDetail.DataSource = mPendingReceiptItemList
		dgItemReceiptDetail.DataBind()
	End Sub
	Private Sub dgTransItemList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTransItemList.Sorting
		mPendingToReceiveTransItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList
		dgTransItemList.DataSource = mPendingToReceiveTransItemList
		dgTransItemList.DataBind()
	End Sub
	Private Sub lnkSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkSelectAll.Click 'Added By Utkarsh ON 22-Feb-2012 For ALL22022012
		If mPendingToReceiveTransItemList.Count > 0 Then
			For i As Integer = 0 To mPendingToReceiveTransItemList.Count - 1
				ItemSelectionForRCI(i, True)
				If Not i = mPendingToReceiveTransItemList.Count - 1 Then  'Avoid  Empty ReceiptCumInvoice Item to Add at last postion of collection
					mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
				End If
			Next
			mReceiptCumInvoice.Invoice.CalculateTotal()
			Response.Redirect("wfReceiptCumInvoice_Ajax.aspx?BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub 'End
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
	End Sub

	Private Sub cmbOrderText_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOrderText.SelectedIndexChanged
		txtNo.Text = ""
		txtAmend.Text = ""
		upnlDetails.Update()
	End Sub
#End Region
End Class