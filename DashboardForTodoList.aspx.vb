Partial Class DashboardForTodoList
	Inherits System.Web.UI.Page

#Region " Variable Declaration "
	'Added by Shital on 18-Mar-2021
	Public mPendingOrderListForAuthorization As OrderList
	Public mOrder As Order
	Dim mModuleName As String
	Public mPendingToReturnForExchangeRepairList As PendingToReturnForExchangeRepairList
	Public mPendingToReceiveTransItemList As PendingToReceiveTransItemList
	Public mOpenRequisitionList As RequisitionListNew 'Added by Prashant on 23-Mar-2021
	Public mRequisitionNew As RequisitionNew 'Added by Prashant on 23-Mar-2021
	Dim mRequisitionDetail As String 'Added by Prashant on 23-Mar-2021
	Public mPendingLoanToReturnList As PendingLoanToReturnList
	Public mPendingToReceiveTransItemListtoRecoverLoan As PendingToReceiveTransItemList
	Public mIssue As Issue
#End Region

#Region " Helper Methods "
	Private Sub GetSession()

	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		GetSession()
		If Not IsPostBack Then
			'Added  By Shital on 18-Mar-2021
			Session("MiddleFrame") = "DashboardForTodoList.aspx?"
			If User.IsInRole("OrderAuthorized") Then
				GetPendingOrderListforAuthorization()
				PhPendingOrderforAuthorization.Visible = True
			End If
			If User.IsInRole("IssueToVendorForExchangeNew") Then
				GetPendingToIssueEROList()
				PhPendingToIssueERO.Visible = True
			End If
			If User.IsInRole("ReceiptPONew") Then
				GetPendingToReceiptEROList()
				phPendingToReceiptERO.Visible = True
			End If
			If (User.IsInRole("EngineeringRequisitionAuthorized") And User.IsInRole("StoresRequisitionAuthorized") And User.IsInRole("WorkShopRequisitionAuthorized") And User.IsInRole("PlanningRequisitionAuthorized")) Then
				OpenRequisitionList()
				phOpenRequisitionList.Visible = True
			End If
			If User.IsInRole("RCIFromVendorForLoanReturnNew") Or User.IsInRole("RCIFromAircraftForLoanReturnNew") Or User.IsInRole("RCIFromStoreForLoanReturnNew") Or User.IsInRole("RCIFromCustomerForLoanReturnNew") Then
				GetPendingTotoRecoverLoan()
				phPendingtoRecover.Visible = True
			End If
			If User.IsInRole("IssueforLoanReturntoSupplierNew") Or User.IsInRole("IssueLoanReturnToStoreNew") Or User.IsInRole("IssueforLoanReturntoCustomerNew") Or User.IsInRole("IssueToCustomerAsRepairedReturnNew") Then
				GetPendingTotoReturnLoan()
				phPendingtoReturn.Visible = True
			End If
		End If
	End Sub
	Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
		Session("Message") = Context.Server.GetLastError.Message
		Session("Source") = Context.Server.GetLastError.Source
		Session("Trace") = Context.Server.GetLastError.StackTrace
	End Sub

#End Region

#Region "Methos"
	'Added by Shital on 18-mar-2021
	Private Sub GetPendingOrderListforAuthorization()
		mPendingOrderListForAuthorization = OrderList.GetOrderList("", "", 0, "", "", "01-Jan-1900", "01-Jan-2200", 1, "", "", CInt(Trans.PurchaseOrder))
		Session("mPendingOrderListForAuthorization") = mPendingOrderListForAuthorization

		grdPendingOrders.DataSource = mPendingOrderListForAuthorization
		grdPendingOrders.DataBind()
		' lblResultIndents.InnerText = " [ Total " & mPendingOrderListForAuthorization.Count & " Record(s) ]"
	End Sub
	Private Sub GetPendingToIssueEROList()
		mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(Guid.Empty, Guid.Empty, Today.Date.ToString, "", 16, PendingToReturnForExchangeRepairList.PendingAgainst.Order)

		Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList

		grdPendingToIssueforERO.DataSource = mPendingToReturnForExchangeRepairList
		grdPendingToIssueforERO.DataBind()
		' Span1.InnerText = " [ Total " & mPendingToReturnForExchangeRepairList.Count & " Record(s) ]"
	End Sub
	Private Sub GetPendingToReceiptEROList()
		mPendingToReceiveTransItemList = PendingToReceiveTransItemList.GetPendingToReceiveTransItemList(6, Guid.Empty, 0, Today.Date.ToString, Guid.Empty, IsFromIssueBERParts:=False)

		Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList

		grdPendingToReceiptforERO.DataSource = mPendingToReceiveTransItemList
		grdPendingToReceiptforERO.DataBind()
	End Sub
	'END
	Private Sub OpenRequisitionList() 'Added by Prashant on 23-Mar-2021
		mOpenRequisitionList = RequisitionListNew.GetRequisitionList("", "", 0, "01-Jan-1900", "01-Jan-2200", 1, "", "", "{00000000-0000-0000-0000-000000000000}", "", _
																	 0, 0)
		Session("mOpenRequisitionList") = mOpenRequisitionList

		dgRequisitionList.DataSource = mOpenRequisitionList
		dgRequisitionList.DataBind()
	End Sub
	Private Sub GetPendingTotoRecoverLoan()

		mPendingToReceiveTransItemListtoRecoverLoan = PendingToReceiveTransItemList.GetPendingToReceiveTransItemListforDashboard(0, Guid.Empty, 0, Today.Date.ToString, Guid.Empty, False, Guid.Empty.ToString, IsFromIssueBERParts:=False, IsRCIFromVendorForLoanReturn:=User.IsInRole("RCIFromVendorForLoanReturnNew"), IsRCIFromAircraftForLoanReturn:=User.IsInRole("RCIFromAircraftForLoanReturnNew"), IsRCIFromStoreForLoanReturn:=User.IsInRole("RCIFromStoreForLoanReturnNew"), IsRCIFromCustomerForLoanReturn:=User.IsInRole("RCIFromCustomerForLoanReturnNew"))

		Session("mPendingToReceiveTransItemListtoRecoverLoan") = mPendingToReceiveTransItemListtoRecoverLoan

		dgPendingTORecoverLoan.DataSource = mPendingToReceiveTransItemListtoRecoverLoan
		dgPendingTORecoverLoan.DataBind()
	End Sub
	Private Sub GetPendingTotoReturnLoan()

		mPendingLoanToReturnList = PendingLoanToReturnList.GetPendingLoanToReturnListForDashboard(Guid.Empty.ToString, Guid.Empty.ToString, Guid.Empty.ToString, Today.Date.ToString, "", Trans.None, IsIssueforLoanReturntoSupplierNew:=User.IsInRole("IssueforLoanReturntoSupplierNew"), IsIssueLoanReturnToStoreNew:=User.IsInRole("IssueLoanReturnToStoreNew"), IsIssueforLoanReturntoCustomerNew:=User.IsInRole("IssueforLoanReturntoCustomerNew"), IsIssueToCustomerAsRepairedReturnNew:=User.IsInRole("IssueToCustomerAsRepairedReturnNew"))
		Session("mPendingLoanToReturnList") = mPendingLoanToReturnList

		dgPendingListToReturnLoan.DataSource = mPendingLoanToReturnList
		dgPendingListToReturnLoan.DataBind()
	End Sub

#End Region

#Region "events"
	'Added by Shital on 18-Mar-2021
	Private Sub grdPendingOrders_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPendingOrders.PageIndexChanging
		grdPendingOrders.PageIndex = e.NewPageIndex
		grdPendingOrders.DataSource = mPendingOrderListForAuthorization
		Session("mPendingOrderListForAuthorization") = mPendingOrderListForAuthorization
		grdPendingOrders.DataBind()
		upnlPendingOrder.Update()
	End Sub
	Private Sub grdPendingOrders_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdPendingOrders.Sorting
		mPendingOrderListForAuthorization.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		grdPendingOrders.DataSource = mPendingOrderListForAuthorization
		Session("mPendingOrderListForAuthorization") = mPendingOrderListForAuthorization
		grdPendingOrders.DataBind()
	End Sub
	Private Sub grdPendingOrders_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPendingOrders.RowCommand

		mPendingOrderListForAuthorization = Session("mPendingOrderListForAuthorization")
		Select Case e.CommandName
			Case "EditView"
				'Dim Index As Integer = CInt(e.CommandArgument) + grdPendingOrders.PageIndex * grdPendingOrders.PageSize
				'Session("Index") = Index

				Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				EditRecord(mID)
				'If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
				'    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
				'    Exit Sub
				'End If
				mModuleName = "Purchase Order for New Purchase"
				Dim OrderDetail As String = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mPendingOrderListForAuthorization(mOrder.ID).VendorName & " Created By : " & mOrder.UserName
				MarkLog(Util.Action.Edit, mModuleName, OrderDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
				Dim str As String
				str = "openledgersame('wfPurchaseOrder_Ajax.aspx?BackPage=DashboardForTodoList.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)



		End Select
	End Sub
	Private Sub EditRecord(ByVal mId As Guid)
		mOrder = Order.GetOrder(mId)
		mOrder.MarkClean()
		Session("mOrder") = mOrder
		'================================================
	End Sub

	Private Sub grdPendingToIssueforERO_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPendingToIssueforERO.PageIndexChanging
		grdPendingToIssueforERO.PageIndex = e.NewPageIndex
		grdPendingToIssueforERO.DataSource = mPendingToReturnForExchangeRepairList
		Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
		grdPendingToIssueforERO.DataBind()
		UpnlPendingToIssueforERO.Update()
	End Sub
	Private Sub grdPendingToIssueforERO_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPendingToIssueforERO.RowCommand
		Dim mIssue As Issue
		mPendingToReturnForExchangeRepairList = Session("mPendingToReturnForExchangeRepairList")
		Select Case e.CommandName

			Case "SelectPart"

				Dim Index As Integer = CInt(e.CommandArgument) + grdPendingToIssueforERO.PageIndex * grdPendingToIssueforERO.PageSize
				'  setObject(Index)

				mIssue = Issue.NewIssue(16, False)
				mIssue.IDate = Today.Date
				mIssue.IssueItems.Add(mIssue.ID, 16)
				mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1

				Session("mIssue") = mIssue

				mIssue.VendorID = mPendingToReturnForExchangeRepairList.Item(Index).VendorID

				Dim mPendingToReturnForExchangeRepairInfo As PendingToReturnForExchangeRepairList.PendingToReturnForExchangeRepairInfo
				mPendingToReturnForExchangeRepairInfo = mPendingToReturnForExchangeRepairList.Item(Index)
				Session("mPendingToReturnForExchangeRepairInfo") = mPendingToReturnForExchangeRepairInfo

				Session("mIssue") = mIssue
				Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
				Response.Redirect("wfIssueStockItemList_Ajax.aspx?BackPage=Dashboard.aspx")

		End Select
	End Sub
	Private Sub grdPendingToIssueforERO_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdPendingToIssueforERO.Sorting
		mPendingToReturnForExchangeRepairList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		grdPendingToIssueforERO.DataSource = mPendingToReturnForExchangeRepairList
		Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
		grdPendingToIssueforERO.DataBind()
	End Sub
	Private Sub grdPendingToReceiptforERO_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPendingToReceiptforERO.PageIndexChanging
		grdPendingToReceiptforERO.PageIndex = e.NewPageIndex
		grdPendingToReceiptforERO.DataSource = mPendingToReturnForExchangeRepairList
		Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList
		grdPendingToReceiptforERO.DataBind()
		upnlPendingToReceiptERO.Update()
	End Sub
	Private Sub grdPendingToReceiptforERO_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPendingToReceiptforERO.RowCommand
		Dim mReceipt As Receipt
		Dim mOrderList As OrderList

		mPendingToReceiveTransItemList = Session("mPendingToReceiveTransItemList")
		Select Case e.CommandName

			Case "SelectRec"

				Dim Index As Integer = CInt(e.CommandArgument) + grdPendingToIssueforERO.PageIndex * grdPendingToIssueforERO.PageSize

				mReceipt = Receipt.NewReceipt(10)
				mReceipt.ReceiptItems.Add(mReceipt.ID, mReceipt.TransTypeID)
				mReceipt.ReceiptItems.CurrentIndex = mReceipt.ReceiptItems.Count - 1

				If mReceipt.ReceiptItems.Count > 0 Then
					mOrderList = OrderList.GetPendingOrderList("", "", 0, "", "", "1/1/1800", Today.Date.ToString, 2, "", Guid.Empty.ToString, , 4, Guid.Empty.ToString, CurrencyID:=mReceipt.ReceiptItems(0).OrderCurrencyID.ToString)
				Else
					mOrderList = OrderList.GetPendingOrderList("", "", 0, "", "", "1/1/1800", Today.Date.ToString, 2, "", Guid.Empty.ToString, , 4, Guid.Empty.ToString)
				End If

				mReceipt.VendorID = mOrderList.Item(0).VendorID
				mReceipt.OrderID = mOrderList.Item(0).ID

				mReceipt.RecdDate = Today.Date
				mReceipt.ReceiptItems.CurrentItem.PrimaryCategoryID = mPendingToReceiveTransItemList(Index).PrimaryCategoryID

				mReceipt.ReceiptItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemList(Index).Type
				mReceipt.ReceiptItems.CurrentItem.OrderItemID = mPendingToReceiveTransItemList(Index).OrderItemID
				mReceipt.ReceiptItems.CurrentItem.ItemTypeID = mPendingToReceiveTransItemList(Index).ItemTypeID
				mReceipt.ReceiptItems.CurrentItem.FromPartList = False
				mReceipt.ReceiptItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemList(Index).UnitID
				mReceipt.ReceiptItems.CurrentItem.DisplayQty = CDec(IIf(mPendingToReceiveTransItemList(Index).IsSerialized, 1, mPendingToReceiveTransItemList(Index).PendingItemQty))

				mReceipt.ReceiptItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemList(Index).OriginalReceiptDate
				mReceipt.ReceiptItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemList(Index).OriginalReceiptTextNo
				If mReceipt.ReceiptItems.CurrentItem.IsSerialized = True Then
					mReceipt.ReceiptItems.CurrentItem.SerialNo = mPendingToReceiveTransItemList(Index).SerialNo
				End If
				mReceipt.ReceiptItems.CurrentItem.IsWarranty = True
				mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate = mReceipt.RecdDate
				mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate = CDate(DateAdd(DateInterval.Month, Val(AppSettings("WarrantyForExchangeRepaired")), mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate))
				mReceipt.ReceiptItems.CurrentItem.WarrantyInDays = DateDiff(DateInterval.Day, mReceipt.ReceiptItems.CurrentItem.WarrantyStartDate, mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDate)
				mReceipt.ReceiptItems.CurrentItem.ItemTagID = mPendingToReceiveTransItemList(Index:=Index).ItemTagID
				mReceipt.ReceiptItems.CurrentItem.ItemTagName = mPendingToReceiveTransItemList(Index:=Index).ItemTagName

				mReceipt.ReceiptItems.CurrentItem.IsAirworthiness = mPendingToReceiveTransItemList(Index).IsAirworthiness
				mReceipt.ReceiptItems.CurrentItem.IsWarrantyApplicableCheckedInOrderItem = mPendingToReceiveTransItemList(Index:=Index).IsWarrantyApplicable
				mReceipt.ReceiptItems.CurrentItem.ReqEmployeeEmailIDs = mPendingToReceiveTransItemList(Index).ReqEmployeeEmailIDs
				mReceipt.ReceiptItems.CurrentItem.ReqNo = mPendingToReceiveTransItemList(Index).ReqNo
				mReceipt.ReceiptItems.CurrentItem.ReqEmployeeName = mPendingToReceiveTransItemList(Index).ReqEmployeeName
				mReceipt.ReceiptItems.CurrentItem.ReqQty = mPendingToReceiveTransItemList(Index).ReqQty
				mReceipt.ReceiptItems.CurrentItem.ReqDate = mPendingToReceiveTransItemList(Index).ReqDateFormatted.ToString
				mReceipt.ReceiptItems.CurrentItem.ReqEmployeeID = mPendingToReceiveTransItemList(Index).ReqEmployeeID
				mReceipt.ReceiptItems.CurrentItem.ReqItemID = mPendingToReceiveTransItemList(Index).ReqItemID

				mReceipt.ReceiptItems.CurrentItem.OrderCurrencyID = mPendingToReceiveTransItemList(Index).OrderCurrencyID
				Session("mReceipt") = mReceipt
				Session("mTotalPendingItemQty") = CDec(mPendingToReceiveTransItemList(Index).PendingItemQty())
				Session("TotalCount") = CDec(mPendingToReceiveTransItemList(Index).PendingItemQty())
				Response.Redirect("wfReceiptItem_Ajax.aspx?BackPage=Dashboard.aspx")

		End Select
	End Sub
	Private Sub grdPendingToReceiptforERO_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdPendingToReceiptforERO.Sorting
		mPendingToReceiveTransItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		grdPendingToReceiptforERO.DataSource = mPendingToReceiveTransItemList
		Session("mPendingToReceiveTransItemList") = mPendingToReceiveTransItemList
		grdPendingToReceiptforERO.DataBind()
	End Sub

	Private Sub dgPendingTORecoverLoan_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingTORecoverLoan.PageIndexChanging
		dgPendingTORecoverLoan.PageIndex = e.NewPageIndex
		dgPendingTORecoverLoan.DataSource = mPendingToReceiveTransItemListtoRecoverLoan
		Session("mPendingToReceiveTransItemListtoRecoverLoan") = mPendingToReceiveTransItemListtoRecoverLoan
		dgPendingTORecoverLoan.DataBind()
		UpnlPendingtoRecoverLoan.Update()
	End Sub
	Public Enum Transaction
		Order = 3
		Issue = 4
		Receipt = 5
	End Enum
	Private Sub dgPendingTORecoverLoan_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingTORecoverLoan.RowCommand
		Dim mReceiptCumInvoice As ReceiptCumInvoice
		Dim mTransTypeID As Trans
		Dim mFromPartList As Boolean
		Dim mFromID As Guid
		Dim mOrderList As OrderList
		Dim mFromToTypeID As Integer
		Dim mIssueList As IssueList
		Dim mLastWarrantyInformation As LastWarrantyInformation
		Dim mItemName As String
		Dim mTransaction As Transaction
		mTransaction = 4

		mPendingToReceiveTransItemListtoRecoverLoan = Session("mPendingToReceiveTransItemListtoRecoverLoan")
		Select Case e.CommandName

			Case "SelectRec"
				Dim Index As Integer = CInt(e.CommandArgument) + grdPendingToIssueforERO.PageIndex * grdPendingToIssueforERO.PageSize

				mReceiptCumInvoice = ReceiptCumInvoice.NewReceiptCumInvoice(mPendingToReceiveTransItemListtoRecoverLoan.Item(Index).ReceiptTransTypeID)
				mTransTypeID = mPendingToReceiveTransItemListtoRecoverLoan.Item(Index).ReceiptTransTypeID
				mFromToTypeID = CInt(IIf(mReceiptCumInvoice.FromTypeID = 14, 1, mReceiptCumInvoice.FromTypeID))
				If (mReceiptCumInvoice.FromTypeID = 14) Or (mReceiptCumInvoice.FromTypeID = 1) Then mFromID = mReceiptCumInvoice.VendorID 'From Vendor        '1->14 30-08-2006
				If mReceiptCumInvoice.FromTypeID = 2 Then mFromID = mReceiptCumInvoice.AircraftID 'For Aircraft
				If mReceiptCumInvoice.FromTypeID = 8 Then mFromID = mReceiptCumInvoice.StoreID 'For Store
				If mReceiptCumInvoice.FromTypeID = 16 Then mFromID = mReceiptCumInvoice.WorkShopID 'For WorkShop
				If mReceiptCumInvoice.FromTypeID = 17 Then mFromID = mReceiptCumInvoice.WOID 'For WorkOrder


				If mTransTypeID = 7 Or mTransTypeID = 8 Or mTransTypeID = 10 Or mTransTypeID = 11 Or mTransTypeID = 12 Or mTransTypeID = 13 Or mTransTypeID = 27 _
		   Or mTransTypeID = 28 Or mTransTypeID = 47 Or mTransTypeID = 54 Or mTransTypeID = 61 Or mTransTypeID = 62 Or mTransTypeID = 66 Or mTransTypeID = 73 Then  'ALL12102012-1   '73 Added By Prashant 10-Sep-2014 'ALL10092014
					mReceiptCumInvoice.ReceiptCumInvoiceItems.Add(mReceiptCumInvoice.ID)
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ConversionFactor = mReceiptCumInvoice.ConversionFactor
				End If
				If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count > 0 Then
					mOrderList = OrderList.GetPendingOrderList("", "", 0, "", "", "1/1/1800", Today.Date.ToString, 2, "", Guid.Empty.ToString, , 1, Guid.Empty.ToString, CurrencyID:=mReceiptCumInvoice.CurrencyID.ToString)
				Else
					mOrderList = OrderList.GetPendingOrderList("", "", 0, "", "", "1/1/1800", Today.Date.ToString, 2, "", Guid.Empty.ToString, , 4, Guid.Empty.ToString)
				End If
				mIssueList = IssueList.GetPendingIssueList("", 0, "1/1/1800", Today.Date.ToString, mFromToTypeID, "", 0, "", "", "", mTransTypeID, mFromID.ToString, Guid.Empty.ToString, False) 'ALL21052012-05

				mFromPartList = False
				mPendingToReceiveTransItemListtoRecoverLoan = Session("mPendingToReceiveTransItemListtoRecoverLoan")
				mTransTypeID = mReceiptCumInvoice.TransTypeID
				Dim Index1 As Integer
				Index1 = Session("Index1")
				If mReceiptCumInvoice.IsNew Then
					mReceiptCumInvoice.RecCumInvDate = Today.Date.ToString
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
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = mPendingToReceiveTransItemListtoRecoverLoan(Index).PrimaryCategoryID 'Added By Prashant On 07-Oct-2015 For ALL06102015
				If ((mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 3 Or mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 12 Or _
					 mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 13 Or mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 47) And _
				 mPendingToReceiveTransItemListtoRecoverLoan(Index).IsSerialized = False) Or mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 4 And _
				 mFromPartList = False Then    'If Order or Issue '19-06-2006

					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemListtoRecoverLoan(Index).Type ''--
					If mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 3 Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).OrderItemID
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemTypeID 'Added By Prashant On 26-Nov-2014 For ALL24112014
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
					If mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 4 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).IssueItemID ''--
					'This Checks whether Item Is Selected from PartList or Not
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = mFromPartList
					'Item of Aircraft/Store against Issue
					If (mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 12 Or mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 13) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = False) Then
						If mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 12 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).IssueItemID
						If mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 13 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).IssueItemID
						If mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 47 Then mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).IssueItemID
					End If
					'This will Returns PartName and PartDescription For Item Selected from PartList
					If (mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 12 Or mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 13 Or mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 47) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = True) Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemID
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemName
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemDescription
					End If


					'Kalpesh   - IF receipt is against Issue then SerialNo should come Automaticaly
					If mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 4 Then
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
						'  If IsSelectAll Then 'Added By Utkarsh On 22-Feb-2012 For ALL22022012
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CalibrationDoneOnDate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.CalibrationDoneOnDate
						' If 'End

					ElseIf mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 3 Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = ""
					End If
					If mReceiptCumInvoice.TransTypeID = 7 Then 'Added By Prashant 5-Feb-2019 ALL04022019
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemListtoRecoverLoan(Index).OrderItemUnitID 'Added By Prashant 5-Feb-2019 ALL04022019
					ElseIf mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 4 Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemListtoRecoverLoan(Index).IssueItemUnitID
					Else
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemListtoRecoverLoan(Index).UnitID
					End If
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(mPendingToReceiveTransItemListtoRecoverLoan(Index).PendingItemQty) 'Added By Prashant 11-May-2010

					If mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 4 Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.InvoiceItemRateForRCI 'Added by Prashant 30-Aug-2013 ALL30082013-1
						'Added By Prashant 5-Feb-2019 ALL04022019
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate
						'Added By Prashant On 27-Apr-2021 ALL26042021
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.COtherCharges = (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.RateEffRateDiffrenceForRCI * CDec(mPendingToReceiveTransItemListtoRecoverLoan(Index).PendingItemQty))
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
						' If IsSelectAll Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreName = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemDetailForReceipt.ToStoreName
						' End If
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

					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemListtoRecoverLoan(Index).OriginalReceiptDate
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemListtoRecoverLoan(Index).OriginalReceiptTextNo
					'if ReceiptItem is New and Receipt is against Order and Receiveing Part is Serialized
				ElseIf mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew And mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 3 And mPendingToReceiveTransItemListtoRecoverLoan(Index).IsSerialized = True And mFromPartList = False Then '19-06-2006  
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemListtoRecoverLoan(Index).Type
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OrderItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).OrderItemID
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTypeID = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemTypeID 'Added By Prashant On 26-Nov-2014 For ALL24112014
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
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemListtoRecoverLoan(Index).OrderItemUnitID 'Added By Prashant 5-Feb-2019 ALL04022019
					Else
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemListtoRecoverLoan(Index).UnitID
					End If
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(IIf(mPendingToReceiveTransItemListtoRecoverLoan(Index).IsSerialized, 1, mPendingToReceiveTransItemListtoRecoverLoan(Index).PendingItemQty)) 'CDec(mPendingToReceiveTransItemListtoRecoverLoan(Index).PendingItemQty)
					'05-09-2006
					'If mPendingToReceiveTransItemListtoRecoverLoan(Index).ExpiryMonth > 0 Then
					'    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mReceiptCumInvoice.Receipt.RecdDate
					'    If Not (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate) Is System.DBNull.Value Then
					'        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = CDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate).AddMonths(mPendingToReceiveTransItemListtoRecoverLoan(Index).ExpiryMonth)
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
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemListtoRecoverLoan(Index).OriginalReceiptDate
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemListtoRecoverLoan(Index).OriginalReceiptTextNo
				ElseIf mFromPartList = True Then '19-06-2006
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = mPendingToReceiveTransItemListtoRecoverLoan(Index).Type
					'This Checks whether Item Is Selected from PartList or Not
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = mFromPartList
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartFromListisSerialized = mPendingToReceiveTransItemListtoRecoverLoan(Index).IsSerialized
					'This will Returns PartName and PartDescription For Item Selected from PartList
					If (mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 12 Or mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 13 Or mPendingToReceiveTransItemListtoRecoverLoan(Index).Type = 47) And (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = True) Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemID
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemName
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemDescription
					End If
					If mReceiptCumInvoice.TransTypeID = 7 Then 'Added By Prashant 5-Feb-2019 ALL04022019
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemListtoRecoverLoan(Index).OrderItemUnitID 'Added By Prashant 5-Feb-2019 ALL04022019
					Else
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPendingToReceiveTransItemListtoRecoverLoan(Index).UnitID
					End If
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = CDec(IIf(mPendingToReceiveTransItemListtoRecoverLoan(Index).IsSerialized, 1, mPendingToReceiveTransItemListtoRecoverLoan(Index).PendingItemQty))
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptDate = mPendingToReceiveTransItemListtoRecoverLoan(Index).OriginalReceiptDate
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.OriginalReceiptTextNo = mPendingToReceiveTransItemListtoRecoverLoan(Index).OriginalReceiptTextNo
				End If
				'Added By Vikrant on 16-Aug-2012 All14082012-1
				If mReceiptCumInvoice.TransTypeID = 10 Then
					If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True Then
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo = mPendingToReceiveTransItemListtoRecoverLoan(Index).SerialNo
					End If
					'Added By Vikrant On 02-May-2013 For BA30042013-2
					If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
						mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BatchNo = mPendingToReceiveTransItemListtoRecoverLoan(Index).OrderTextNo
					End If
					'End
				End If
				'End

				'vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
				'Added By Prashant 17-Feb-2015-----------------------
				If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsSerialized = True Then 'Serialized
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
						mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)
						If mLastWarrantyInformation.Count > 0 Then
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = mLastWarrantyInformation(0).IsWarranty
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = mLastWarrantyInformation(0).WarrantyInDays
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = mLastWarrantyInformation(0).WarrantyStartDateFormatted.ToString
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = mLastWarrantyInformation(0).WarrantyExpiryDateFormatted.ToString
							mLastWarrantyInformation = Nothing
						End If
					End If
					If (mReceiptCumInvoice.TransTypeID <> 7 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = 2) Then 'Added By Prashant On 07-Oct-2015 For ALL06102015
						mLastWarrantyInformation = LastWarrantyInformation.GetLastWarrantyInformation(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.ToString, mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SerialNo)
						If mLastWarrantyInformation.Count > 0 Then
							mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = mLastWarrantyInformation(0).CodeNo
							mLastWarrantyInformation = Nothing
						End If
					End If
				Else 'Nonserialized
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarranty = False
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyStartDate = System.DBNull.Value
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyExpiryDate = System.DBNull.Value
					mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.WarrantyInDays = 0
				End If
				'----------------------------------------------------
				'Added by shital on 07-Sep-2016
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAirworthinss = mPendingToReceiveTransItemListtoRecoverLoan(Index).IsAirworthiness
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID = mPendingToReceiveTransItemListtoRecoverLoan(Index:=Index).ItemTagID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName = mPendingToReceiveTransItemListtoRecoverLoan(Index:=Index).ItemTagName
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsWarrantyApplicableCheckedInOrderItem = mPendingToReceiveTransItemListtoRecoverLoan(Index:=Index).IsWarrantyApplicable

				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CGSTPercentage = mPendingToReceiveTransItemListtoRecoverLoan(Index:=Index).CGSTPercentage
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.SGSTPercentage = mPendingToReceiveTransItemListtoRecoverLoan(Index:=Index).SGSTPercentage
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IGSTPercentage = mPendingToReceiveTransItemListtoRecoverLoan(Index:=Index).IGSTPercentage

				'Added By Vikrant On 19-Jun-2020 For ALL19062020-1
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeEmailIDs = mPendingToReceiveTransItemListtoRecoverLoan(Index).ReqEmployeeEmailIDs
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqNo = mPendingToReceiveTransItemListtoRecoverLoan(Index).ReqNo
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeName = mPendingToReceiveTransItemListtoRecoverLoan(Index).ReqEmployeeName
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqQty = mPendingToReceiveTransItemListtoRecoverLoan(Index).ReqQty
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqDate = mPendingToReceiveTransItemListtoRecoverLoan(Index).ReqDateFormatted.ToString
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqEmployeeID = mPendingToReceiveTransItemListtoRecoverLoan(Index).ReqEmployeeID
				mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ReqItemID = mPendingToReceiveTransItemListtoRecoverLoan(Index).ReqItemID
				'End
				mReceiptCumInvoice.ApplyEdit()
				Session("mFromPartList") = mFromPartList
				Session("mReceiptCumInvoice") = mReceiptCumInvoice
				Session("mTotalPendingItemQty") = CDec(mPendingToReceiveTransItemListtoRecoverLoan(Index).PendingItemQty())
				Session("TotalCount") = CDec(mPendingToReceiveTransItemListtoRecoverLoan(Index).PendingItemQty())
				mItemName = mPendingToReceiveTransItemListtoRecoverLoan(Index).ItemName
				Session("Edit") = False
				' If Not IsSelectAll Then 'Changed By Utkarsh On 22-Feb-2012 For ALL22022012
				Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?ChildPage1=" & "wfReceiptPendingOrderList_Ajax.aspx&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage"))
				' End If 'End

		End Select
	End Sub
	Private Sub dgPendingTORecoverLoan_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingTORecoverLoan.Sorting
		mPendingToReceiveTransItemListtoRecoverLoan.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgPendingTORecoverLoan.DataSource = mPendingToReceiveTransItemListtoRecoverLoan
		Session("mPendingToReceiveTransItemListtoRecoverLoan") = mPendingToReceiveTransItemListtoRecoverLoan
		dgPendingTORecoverLoan.DataBind()
	End Sub
	Private Sub dgPendingListToReturnLoan_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingListToReturnLoan.PageIndexChanging
		dgPendingListToReturnLoan.PageIndex = e.NewPageIndex
		dgPendingListToReturnLoan.DataSource = mPendingLoanToReturnList
		Session("mPendingLoanToReturnList") = mPendingLoanToReturnList
		dgPendingListToReturnLoan.DataBind()
		UpnlPendingtoReturnLoan.Update()
	End Sub

	Private Sub dgPendingListToReturnLoan_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingListToReturnLoan.RowCommand
		Dim mTransTypeID As Trans
		mPendingLoanToReturnList = Session("mPendingLoanToReturnList")
		Select Case e.CommandName

			Case "Select"


				Dim Index As Int32 = CInt(e.CommandArgument) + dgPendingListToReturnLoan.PageIndex * dgPendingListToReturnLoan.PageSize
				'SetObject(Index)
				mTransTypeID = mPendingLoanToReturnList.Item(Index).IssueTransTypeID
				mIssue = Issue.NewIssue(mTransTypeID, False)
				mIssue.IDate = Today.Date
				If mTransTypeID = 16 Or mTransTypeID = 18 Or mTransTypeID = 49 Or mTransTypeID = 51 Or mTransTypeID = 55 Or mTransTypeID = 58 Or mTransTypeID = 59 Or mTransTypeID = 60 Or ((mTransTypeID = 14 Or mTransTypeID = 44)) Then  '55, 58 Added By Prashant 6-Jan-2010  '72 Added by vikrant For New Requisition 
					mIssue.IssueItems.Add(mIssue.ID, mTransTypeID)
					mIssue.IssueItems.CurrentIndex = mIssue.IssueItems.Count - 1
				End If
				Session("mIssue") = mIssue
				mIssue.IssueItems.CurrentItem.LoanReceiptItemID = mPendingLoanToReturnList(Index).ReceiptItemID

				'Loan taken FROM Store ID will be now Issue to Store ID
				If mIssue.TransTypeID = 49 Then
					mIssue.VendorID = mPendingLoanToReturnList.Item(Index).FromStoreID
				ElseIf mIssue.TransTypeID = 51 Or mIssue.TransTypeID = 58 Then  '58 Added By Prashant 21-May-2010
					mIssue.VendorID = mPendingLoanToReturnList.Item(Index).FromStoreID
				ElseIf mIssue.TransTypeID = 55 Then             'Added By Prashant 6-Jan-2010
					mIssue.VendorID = mPendingLoanToReturnList.Item(Index).FromStoreID
				Else
					mIssue.ToStoreID = mPendingLoanToReturnList.Item(Index).FromStoreID
				End If
				If mIssue.IsNew Then
					mIssue.IDate = CDate(Today.Date.ToString)
				End If
				'Loan taken BY Store ID will be now Issue from Store ID  'Commented By Prashant 02-Sep-2011
				'If mIssue.TransTypeID <> Flypal.Util.Trans.IssuetoSupplierasRentalLease Then 'Added By Saylee 27-Jan-2010
				If mIssue.TransTypeID = Flypal.Util.Trans.LoanReturnToStore Then  'Added By Prashant 02-Sep-2011
					mIssue.StoreID = mPendingLoanToReturnList.Item(Index).ToStoreID
					Dim mLinkID As Guid
					mLinkID = mPendingLoanToReturnList.Item(Index).LinkID
					Session("mLinkID") = mLinkID
				End If
				Session("mIssue") = mIssue

				Session("mItemName") = mPendingLoanToReturnList(Index).ItemName
				Session("PartNo") = mPendingLoanToReturnList(Index).ItemName
				Session("CheckQty") = "False"
				Response.Redirect("wfPartStockStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfIssueItem_Ajax.aspx" & "&Name=" & HttpUtility.UrlEncode(mPendingLoanToReturnList(Index).ItemName))

		End Select
	End Sub

	Private Sub dgPendingListToReturnLoan_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingListToReturnLoan.Sorting
		mPendingLoanToReturnList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		dgPendingListToReturnLoan.DataSource = mPendingLoanToReturnList
		dgPendingListToReturnLoan.DataSource = mPendingLoanToReturnList
		Session("mPendingLoanToReturnList") = mPendingLoanToReturnList
		dgPendingListToReturnLoan.DataBind()
	End Sub


	'END
	'Added by Prashant on 23-Mar-2021
	Private Sub dgRequisitionList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionList.RowCommand
		Select Case e.CommandName
			Case "EditRec"
				Dim mID As Guid = New Guid(e.CommandArgument.ToString)
				EditRequisitionRecord(mID)
				mRequisitionDetail = mRequisitionNew.RequisitionNo + " Dated : " + mRequisitionNew.ReqDateFormatted + " Requested By : " + mRequisitionNew.EmployeeName + " Status : " + IIf(mRequisitionNew.StatusID = 1, "Open", "Authorized")
				If mRequisitionNew.TransTypeID = 65 Then
					mModuleName = "Engineering Requisition"
					If AppSettings("ClientCode") = "IND" Then
						mModuleName = "Spares Requisition"
					End If
				ElseIf mRequisitionNew.TransTypeID = 71 Then
					mModuleName = "Stores Requisition"
				ElseIf mRequisitionNew.TransTypeID = 72 Then
					mModuleName = "WorkShop Requisition"
				ElseIf mRequisitionNew.TransTypeID = 77 Then
					mModuleName = "Planning Requisition"
				End If
				MarkLog(Util.Action.Edit, mModuleName, mRequisitionDetail, Util.ErrorType.NoError, mID, EventLogID)
				Session("TransTypeID") = mRequisitionNew.TransTypeID
				Dim str As String
				str = "openledgersame('wfRequisition_Ajax.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
		End Select
	End Sub
	Private Sub dgRequisitionList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRequisitionList.PageIndexChanging
		mOpenRequisitionList = Session("mOpenRequisitionList")
		dgRequisitionList.PageIndex = e.NewPageIndex
		dgRequisitionList.DataSource = mOpenRequisitionList
		Session("mOpenRequisitionList") = mOpenRequisitionList
		dgRequisitionList.DataBind()
		upnlOpenRequisitionList.Update()
	End Sub
	Private Sub EditRequisitionRecord(ByVal mId As Guid)
		mRequisitionNew = RequisitionNew.GetRequisition(mId)
		Dim child As RequisitionItemNew
		For Each child In mRequisitionNew.RequisitionItemsNew
			If child.ItemID.Equals(Guid.Empty) Then
				' ''partno id .....
				' ''child.ItemID = Guid.NewGuid
				' ''child.Save()
			End If
		Next
		mRequisitionNew.MarkClean()
		Session("mRequisitionNew") = mRequisitionNew
	End Sub
	'End Of Added by Prashant on 23-Mar-2021
#End Region

End Class

