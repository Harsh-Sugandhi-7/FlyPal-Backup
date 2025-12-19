Public Class wfPurchaseOrderList_Ajax
	Inherits System.Web.UI.Page

#Region " Enumaration "
	Private Enum Rights
		[New] = 1
		Edit = 2
		Delete = 3
		Save = 4
		View = 5
		Print = 6
		FindNow = 7
	End Enum
#End Region

#Region " Variable Declaration "
	Public mOrderList As OrderList
	Public mOrder As Order
	Public mDistinctTextListForOrder As DistinctTextListForOrder
	Public OrderType As Integer
	Public SearchOrderType As Integer
	Dim IsOverhaul As Boolean
	Dim SearchValue, DateIndex, FromDate, ToDate, StatusId, Priority, OrderText, PartNoSearchForOrder, OrderNoSearch, Amend,
		POAgainstType, POFor, POrderType, PoToW, InternalOrderNoSearch, SupplierForOrdeSearch, ReqNoOnPurchaseOrderList, AircraftSearchForPO, QuotationNoForPO, SearchText As String
	Dim mModuleName As String
	Dim EventLogID As Guid                                      'Added by Saylee on 19-July-2011
	Public Flag As Integer = 0
	Public mShowTopAmendedOrderNo As ShowTopAmendedOrderNo      'Added by Saylee on 22-Nov-2012 for ALL22112012
	'Public mTransactionListCount As TransactionListCount        'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
	Dim mTransTypeID As Trans
	Dim mFileAttach As FileAttach 'Added By Vikrant On 23-Dec-2014 For All23122014-2
	Dim mPendingTransactionCount As PendingTransactionCount
	Dim mModuleList As ModuleList 'Added by Shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
	Public mPOTowards As POTowards
	Public mDistinctTextListForRequisitionOnPurchaseOrderList As DistinctTextListForRequisition
	Dim ReqTextOnPurchaseOrderList As String = ""
	Dim IsPBHPurchaseOnOrderList As Boolean = False

	Private RedirectToNewUIHelper As New RedirectToNewUIHelper
	Public AttachmentHelper As New AttachmentHelper

#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mOrder = Session("mOrder")
		mOrderList = Session("mOrderList")
		mDistinctTextListForOrder = Session("mDistinctTextListForOrder")
		SearchValue = Session("SearchValue")
		DateIndex = Session("DateIndex")
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		StatusId = Session("StatusId")
		Priority = Session("Priority")
		OrderText = Session("OrderText")
		PartNoSearchForOrder = Session("PartNoSearchForOrder")
		OrderNoSearch = IIf(IsNothing(Session("OrderNoSearch")), 0, Session("OrderNoSearch"))
		Amend = IIf(IsNothing(Session("AmendSearch")), "", Session("AmendSearch"))
		POrderType = Session("POrderType")
		POAgainstType = Session("POAgainstType")
		POFor = Session("POFor")
		'mTransactionListCount = Session("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
		mPendingTransactionCount = Session("mPendingTransactionCount")
		SearchOrderType = Session("SearchOrderType")
		mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
		PoToW = Session("PoToW")
		mDistinctTextListForRequisitionOnPurchaseOrderList = Session("mDistinctTextListForRequisitionOnPurchaseOrderList")
		ReqTextOnPurchaseOrderList = Session("ReqTextOnPurchaseOrderList")
		InternalOrderNoSearch = Session("InternalOrderNoSearch")
		SupplierForOrdeSearch = Session("SupplierForOrdeSearch")
		ReqNoOnPurchaseOrderList = IIf(IsNothing(Session("ReqNoOnPurchaseOrderList")), 0, Session("ReqNoOnPurchaseOrderList"))
		AircraftSearchForPO = Session("AircraftSearchForPO")
		QuotationNoForPO = Session("QuotationNoForPO")
		IsPBHPurchaseOnOrderList = Session("IsPBHPurchaseOnOrderList")
		SearchText = Session("SearchTextForPOList")
	End Sub
	Private Sub SetSession()
		Session("mOrder") = mOrder
		Session("mOrderList") = mOrderList
		Session("mDistinctTextListForOrder") = mDistinctTextListForOrder
		Session("OrderType") = OrderType
		Session("POrderType") = POrderType
		Session("POAgainstType") = POAgainstType
		Session("POFor") = POFor
		Session("mDistinctTextListForRequisitionOnPurchaseOrderList") = mDistinctTextListForRequisitionOnPurchaseOrderList
		Session("SearchTextForPOList") = SearchText
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mOrder")
		Session.Remove("mOrderList")
		Session.Remove("SearchValue")
		Session.Remove("DateIndex")
		Session.Remove("FromDate")
		Session.Remove("ToDate")
		Session.Remove("StatusId")
		Session.Remove("Priority")
		Session.Remove("OrderText")
		Session.Remove("PartNoSearchForOrder")
		Session.Remove("OrderNoSearch")
		Session.Remove("Amend")
		Session.Remove("OrderType")
		Session.Remove("POrderType")
		Session.Remove("POAgainstType")
		Session.Remove("POFor")
		'Session.Remove("mTransactionListCount") 'Added By Vikrant On 19-AUg-2013 For ALL16082013-1
		Session.Remove("mDistinctTextListForOrder")
		Session.Remove("mPendingTransactionCount")
		Session.Remove("SearchOrderType")
		Session.Remove("PoToW")
		Session.Remove("mDistinctTextListForRequisitionOnPurchaseOrderList")
		Session.Remove("ReqTextOnPurchaseOrderList")
		Session.Remove("InternalOrderNoSearch")
		Session.Remove("SupplierForOrdeSearch")
		Session.Remove("ReqNoOnPurchaseOrderList")
		Session.Remove("AircraftSearchForPO")
		Session.Remove("QuotationNoForPO")
		Session.Remove("IsPBHPurchaseOnOrderList")
		Session.Remove("SearchTextForPOList")
	End Sub
	Private Sub ClearAll()
		OrderType = Session("OrderType")
		If InStr(Session("MiddleFrame"), "wfPurchaseOrderList_Ajax.aspx?OrderType=" & OrderType) <= 0 Then
			RemoveSession()
			Session.Remove("POCreated")
		End If
	End Sub
	Private Sub NewRecord()
		If OrderType <> 2 Then
			mOrder = Order.NewOrder(IIf(cmbOrderType.SelectedValue = 100, 38, cmbOrderType.SelectedValue))
			mOrder.OrderDate = Today.Date
			mOrder.AgainstTypeID = CInt(IIf(cmbPOAgainstType.SelectedValue = 8, 5, cmbPOAgainstType.SelectedValue)) ' CInt(cmbPOAgainstType.SelectedValue)
			mOrder.IsCustomer = IIf(cmbFor.SelectedValue = 1, False, True)
			mOrder.IsOverhaul = IIf(cmbOrderType.SelectedIndex = 2, True, False)
		Else
			mOrder = Order.NewOrder(39)
			mOrder.OrderDate = Today.Date
			mOrder.AgainstTypeID = CInt(cmbPOAgainstType.SelectedValue)
			mOrder.IsCustomer = IIf(cmbFor.SelectedValue = 1, False, True)
			mOrder.IsOverhaul = False
		End If
		'=======Added By Saylee on 2nd Nov 2007============ In Order to keep selected criteria as it is
		POrderType = cmbOrderType.SelectedIndex
		POAgainstType = cmbPOAgainstType.SelectedIndex
		POFor = cmbFor.SelectedIndex
		Session("mOrder") = mOrder
		Session("POrderType") = POrderType
		Session("POFor") = POFor
		Session("POAgainstType") = POAgainstType
		mTransTypeID = mOrder.TransTypeID
		'================================================
	End Sub
	Private Sub EditRecord(ByVal mId As Guid)
		mOrder = Order.GetOrder(mId)
		mTransTypeID = mOrder.TransTypeID
		mOrder.MarkClean()
		'=======Added By Saylee on 2nd Nov 2007============ In Order to keep selected criteria as it is
		POrderType = cmbOrderType.SelectedIndex
		POAgainstType = cmbPOAgainstType.SelectedIndex
		POFor = cmbFor.SelectedIndex
		Session("mOrder") = mOrder
		Session("POrderType") = POrderType
		Session("POFor") = POFor
		Session("POAgainstType") = POAgainstType
		'================================================
	End Sub
	Private Sub DeleteRecord(ByVal mId As Guid)
		MSGBoxCtrl.Show(MSGBox.Message_Title.Delete, MSGBox.Message_Text.Delete, "", MsgBoxStyle.YesNo, "Delete")
		mOrder = Order.GetOrder(mId)
		Session("mOrder") = mOrder
	End Sub
	Private Sub SetControl()
		setPeriod(DateIndex)
		CallFindNow(SearchValue)
		GridBind()
		'cmbSearch.SelectedValue = SearchValue
		cmbDate.SelectedIndex = DateIndex
		cmbStatus.SelectedValue = StatusId
		cmbPriority.SelectedValue = Priority
		cmbPOTowards.SelectedValue = PoToW
		If SearchOrderType = "39" Then
			cmbSearchOrderType.SelectedValue = "0"
		Else
			cmbSearchOrderType.SelectedValue = SearchOrderType.ToString
		End If
		If mDistinctTextListForOrder.Contains(OrderText) Then
			cmbOrderText.SelectedValue = IIf(OrderText = "", "(All)", OrderText)
		Else
			cmbOrderText.SelectedValue = "(All)"
		End If
		If mDistinctTextListForRequisitionOnPurchaseOrderList.Contains(ReqTextOnPurchaseOrderList) Then
			cmbRequisitionText.SelectedValue = IIf(ReqTextOnPurchaseOrderList = "", "(All)", ReqTextOnPurchaseOrderList)
		Else
			cmbRequisitionText.SelectedValue = "(All)"
		End If
		txtPartNoSearch.Text = PartNoSearchForOrder
		txtNo.Text = OrderNoSearch
		txtAmend.Text = Amend
		txtInternalOrderNo.Text = InternalOrderNoSearch
		txtSupplier.Text = SupplierForOrdeSearch
		txtRequisitionNo.Text = ReqNoOnPurchaseOrderList
		txtForAircraftSearch.Text = AircraftSearchForPO
		txtQuotationNo.Text = QuotationNoForPO
		chkIsPBHPurchase.Checked = IsPBHPurchaseOnOrderList
		'===============Added By Saylee on 2nd Nov 2007===============  In Order to keep selected criteria as it is
		cmbOrderType.SelectedIndex = POrderType
		Select Case POrderType
			Case Is = 0
				cmbPOAgainstType.Items.Clear()
				If OrderType = 1 Then
					cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("None (Part)", 1))
					cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Quotations" + " (" + mPendingTransactionCount.QuotationCountForOrder.ToString + ")", 2))
					cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Sales Order" + " (" + mPendingTransactionCount.SalesOrderCountForOrder.ToString + ")", 4))
					cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Requisition Items" + " (" + mPendingTransactionCount.ReqItemCountForOrder.ToString + ")", 6))
					'cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Approved Quotations", 3))
					cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Enquiry Items" + " (" + mPendingTransactionCount.EnqItemCountForOrder.ToString + ")", 7)) 'Added By Vikrant On 04-Jan-2017 For ALL04012017
				ElseIf OrderType = 2 Then
					cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("None (Part)", 1))
					cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Quotations" + " (" + mPendingTransactionCount.RentalLeaseQuotationCountForOrder.ToString + ")", 2))
				End If
			Case Is = 1
				cmbPOAgainstType.Items.Clear()
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("From Stock", 5))
				'Added by Shital on 18-Oct-2019
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Requisition Items" + " (" + mPendingTransactionCount.ReqExchangeItemCountForOrder.ToString + ")", 8))
			Case Is = 2, 3
				cmbPOAgainstType.Items.Clear()
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("From Stock", 5))
				'Added by Shital on 18-Oct-2019
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Requisition Items" + " (" + mPendingTransactionCount.ReqExchangeItemCountForOrder.ToString + ")", 8))
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Quotations" + " (" + mPendingTransactionCount.RepairOverhulQuotationCountForOrder.ToString + ")", 2))
		End Select
		'==============================================================
		cmbPOAgainstType.SelectedIndex = POAgainstType
		cmbFor.SelectedIndex = POFor
		'==================================================================
		ControlVisibility(SearchValue, DateIndex)
		lblResult.Text = "As per criteria :" & mOrderList.Count & " Record(s) found."
		If SearchText IsNot Nothing Then
			SearchText = IIf(SearchText = "", "", SearchText)
		Else
			SearchText = ""
		End If
	End Sub
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "Delete" Then
						Try
							Session("sender") = ""
							mOrder = CType(Session("mOrder"), Order)
							'Added By Vikrant On 23-Dec-2014 For All23122014-2
							If mOrder.IsAttachmentAdded = True Then
								mFileAttach = FileAttach.GetAttachment(mOrder.ID)
							End If
							'End
							mShowTopAmendedOrderNo = ShowTopAmendedOrderNo.GetTopAmendedOrderNo(mOrder.Text, mOrder.No)
							If (mOrder.StatusID = 3) And (Not (mOrder.ID.Equals(mShowTopAmendedOrderNo.ID))) Then
								MSGBoxCtrl.Show(MSGBox.Message_Title.Alert, MSGBox.Message_Text.Alert, "You cannot delete this record as it is already amended.", MsgBoxStyle.OkOnly, "")
								Exit Sub
							End If
							mOrder.Delete()
							mOrder.Save()
							'Added By Vikrant On 23-Dec-2014 For All23122014-2
							If mFileAttach IsNot Nothing Then
								If mFileAttach.Size > 0 Then
									FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
								End If
							End If
							'End
							SendMail()
							DataFieldBind()
							PendingTransCount()
							SetControl()
							upnTopButtons.Update()
						Catch ex As SqlException
							If ex.Number = 547 Then
								Dim stringInfo As String = ""
								If ex.Message.Contains("tabCWP") Then
									stringInfo = "CWP."
								ElseIf ex.Message.Contains("tabReceiptItem") Then
									stringInfo = "Receipt."
								ElseIf ex.Message.Contains("tabIssueItem") Then
									stringInfo = "Issue."
								ElseIf ex.Message.Contains("tabOrderItemFollowUp") Then
									stringInfo = "Order Follow Up."
								ElseIf ex.Message.Contains("tabPaymentAdviceItem") Then
									stringInfo = "Payment Advice."
								ElseIf ex.Message.Contains("tabReqItem") Then
									stringInfo = "Requisition Item."
								End If
								MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDeleting, MSGBox.Message_Text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
								Exit Sub
							End If
						Finally
							TotalCount()
							Dim OrderDetail As String = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mOrderList(mOrder.ID).VendorName & " Created By : " & mOrder.UserName
							MarkLog(Util.Action.Delete, mModuleName, OrderDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
						End Try
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						Session("sender") = ""
						DataFieldBind()
						SetControl()
					End If
					If MSGBoxCtrl.Sender = "Delete" Then
						Session("sender") = ""
						DataFieldBind()
						SetControl()
					End If
				Case MsgBoxResult.Ok
					DataFieldBind()
					SetControl()
			End Select
		End If
	End Sub
	Private Sub FindNow(Optional ByVal ItemName As String = "", Optional ByVal Text As String = "", Optional ByVal No As Integer = 0,
						Optional ByVal Amend As String = "", Optional ByVal IntOrderNo As String = "", Optional ByVal FromDate As String = "1/1/1900",
						Optional ByVal ToDate As String = "1/1/2200", Optional ByVal StatusID As Integer = 0, Optional ByVal QuotationNo As String = "",
						Optional ByVal VendorName As String = "", Optional ByVal TransTypeID As Integer = 0, Optional ByVal PrimaryOrderType As Integer = 0,
						Optional ByVal IsOverhaul As Boolean = False, Optional ByVal Priority As Integer = 0, Optional ByVal AircraftReg As String = "",
						Optional ByVal POTowardsID As Integer = 0, Optional ByVal ReqText As String = "", Optional ByVal ReqNo As Integer = 0,
						Optional ByVal IsPBHPurchase As Boolean = 0, Optional ByVal SearchText As String = "")
		mOrderList = Nothing
		dgGridView.DataSource = Nothing
		'Get List From the Database as per Criteria             
		mOrderList = OrderList.GetOrderList(ItemName, Text, No, Amend, IntOrderNo, FromDate, ToDate, StatusID, QuotationNo, VendorName, TransTypeID,
											PrimaryOrderType, IsOverhaul, Priority, AircraftReg:=AircraftReg, POTowardsID:=POTowardsID, ReqText:=ReqText,
											ReqNo:=ReqNo, IsPBHPurchase:=IsPBHPurchase, SearchText:=SearchText)
		'Set DataSource of the Grid
		Session("mOrderList") = mOrderList
		dgGridView.DataSource = mOrderList
	End Sub
	Private Sub CallFindNow(ByVal Index As Integer)
		Dim tmpmTransTypeID As Trans = 0
		If OrderType = 1 Then
			'If Index = 10 Then
			tmpmTransTypeID = SearchOrderType
			'Else
			'tmpmTransTypeID = 0
			'End If
		Else
			tmpmTransTypeID = Util.Trans.RentialLeaseOtder
		End If
		FindNow(ItemName:=Trim(PartNoSearchForOrder), Text:=Trim(OrderText), No:=CInt(Val(OrderNoSearch)), Amend:=Trim(Amend),
				IntOrderNo:=Trim(InternalOrderNoSearch), FromDate:=txtFromDate.Text.Trim, ToDate:=txtToDate.Text.Trim, StatusID:=CInt(StatusId),
				QuotationNo:=Trim(QuotationNoForPO), VendorName:=Trim(SupplierForOrdeSearch), TransTypeID:=tmpmTransTypeID,
				PrimaryOrderType:=IIf(SearchOrderType > 0, 0, OrderType), IsOverhaul:=IsOverhaul, Priority:=CInt(Priority), AircraftReg:=Trim(AircraftSearchForPO),
					POTowardsID:=CInt(PoToW), ReqText:=Trim(ReqTextOnPurchaseOrderList), ReqNo:=CInt(Val(ReqNoOnPurchaseOrderList)), IsPBHPurchase:=IsPBHPurchaseOnOrderList,
					SearchText:=txtSearchBox.Text.Trim)
		'Select Case Index
		'    Case -1
		'Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", "",  tmpmTransTypeID, OrderType) 'for all records
		'    Case 0  'all
		'        Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", "", tmpmTransTypeID, OrderType)   'for all records
		'    Case 1 'Order date
		'        Call FindNow("", "", 0, "", "", txtFromDate.Text.ToString, txtToDate.Text.ToString, 0, "", "", tmpmTransTypeID, OrderType)
		'    Case 2  'Order Text , No And Amend
		'        Call FindNow("", OrderText, CInt(Val(No)), Amend, "", FromDate, ToDate, 0, "", "", tmpmTransTypeID, OrderType)
		'    Case 3 ' Internal Order No 
		'        Call FindNow("", "", 0, "", Name, FromDate, ToDate, 0, "", "", tmpmTransTypeID, OrderType)
		'    Case 4  'ItemName
		'        Call FindNow(Name, "", 0, "", "", FromDate, ToDate, 0, "", "", tmpmTransTypeID, OrderType)
		'    Case 5 ' Vendor Name
		'        Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", Name, tmpmTransTypeID, OrderType)
		'    Case 6 ' QuotationNo
		'        Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, Name, "", tmpmTransTypeID, OrderType)
		'    Case 7 ' Status
		'        Call FindNow("", "", 0, "", "", FromDate, ToDate, CInt(StatusId), "", "", tmpmTransTypeID, OrderType)
		'        'New Addition By Yogita on 17-Dec-2007 to solve Bug No:-PO_30_A2
		'    Case 8 'Priority
		'        Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", "", tmpmTransTypeID, OrderType, , CInt(Priority))
		'    Case 9 ' For Aircraft 
		'        Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", "", tmpmTransTypeID, OrderType, , , AircraftReg:=Name)
		'    Case 10 ' OrderType
		'        Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", "", tmpmTransTypeID, 0, IsOverhaul)
		'    Case 11 ' PO Towards 
		'Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", "", tmpmTransTypeID, OrderType, , , "", POTowardsID:=CInt(PoToW))
		'    Case 12 ' Requisition No 
		'        Call FindNow("", "", 0, "", "", FromDate, ToDate, 0, "", "", tmpmTransTypeID, OrderType, , , "", POTowardsID:=0, ReqText:=ReqTextOnPurchaseOrderList, _
		'                     ReqNo:=CInt(Val(No)))
		'End Select
		dgGridView.PageIndex = 0
		dgGridView.PageSize = CInt(cmbShowE.SelectedItem.ToString)
	End Sub
	Private Sub ControlVisibility(ByVal SearchValue As Int32, Optional ByVal DateIndex As Int32 = 0)
		'cmbDate.Visible = IIf(SearchValue = 1, True, False)
		lblFromDate.Visible = IIf(DateIndex <> 0, True, False)
		lblToDate.Visible = IIf(DateIndex <> 0, True, False)
		'Added by Saylee on 16-June 2007**************
		If DateIndex = 6 Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = True
			txtToDate.Enabled = True
		ElseIf (DateIndex = 1 Or DateIndex = 2 Or DateIndex = 3 Or DateIndex = 4 Or DateIndex = 5) Then
			txtFromDate.Visible = True
			txtToDate.Visible = True
			txtFromDate.Enabled = False
			txtToDate.Enabled = False
		Else
			txtFromDate.Visible = False
			txtToDate.Visible = False
		End If
		'**********************************************
		'cmbOrderText.Visible = IIf(SearchValue = 2, True, False)
		'lblNo.Visible = IIf((SearchValue = 2 And cmbOrderText.SelectedIndex <> 0) Or (SearchValue = 12 And cmbRequisitionText.SelectedIndex <> 0), True, False)
		'txtNo.Visible = IIf((SearchValue = 2 And cmbOrderText.SelectedIndex <> 0) Or (SearchValue = 12 And cmbRequisitionText.SelectedIndex <> 0), True, False)
		'txtAmend.Visible = IIf(SearchValue = 2 And cmbOrderText.SelectedIndex <> 0, True, False)
		'txtName.Visible = IIf((SearchValue >= 3 And SearchValue <= 6) Or SearchValue = 9, True, False)
		'cmbStatus.Visible = IIf(SearchValue = 7, True, False)
		'cmbSearchOrderType.Visible = IIf(SearchValue = 10, True, False)
		'cmbPriority.Visible = IIf(SearchValue = 8, True, False)
		'cmbPOTowards.Visible = IIf(SearchValue = 11, True, False)
		'cmbRequisitionText.Visible = IIf(SearchValue = 12, True, False)
		'-------------------Added By Prashant--------------
		'Select Case OrderType
		'    Case Util.Trans.PurchaseOrder
		'        btnPrintTop.ToolTip = "Click to Print Purchase Order Outright"
		'        btnBottomPrint.ToolTip = "Click to Print Purchase Order Outright"
		'    Case Util.Trans.PurchaseOrderForExchangeRepair
		'        btnPrintTop.ToolTip = "Click to Print Purchase Order Exchange/Warranty"
		'        btnBottomPrint.ToolTip = "Click to Print Purchase Order Exchange/Warranty"
		'    Case Util.Trans.OverHaulRepairOrder
		'        btnPrintTop.ToolTip = "Click to Print Purchase Order Repair/Overhaul"
		'        btnBottomPrint.ToolTip = "Click to Print Purchase Order Repair/Overhaul"
		'    Case Util.Trans.RentialLeaseOtder
		'        btnPrintTop.ToolTip = "Click to Print Purchase Order Rential/Lease"
		'        btnBottomPrint.ToolTip = "Click to Print Purchase Order Rential/Lease"
		'End Select
		'-------------------------------------------------
		If AppSettings("ClientCode") = "CE" Then
			dgGridView.Columns(7).Visible = False
			'dgGridView.Columns(8).Visible = False
			dgGridView.Columns(12).Visible = True   'AircraftReg
			If OrderType = 2 Then
				dgGridView.Columns(13).Visible = False   'POTowards
			Else
				dgGridView.Columns(13).Visible = True   'POTowards
			End If
		Else
			'dgGridView.Columns(7).Visible = True
			'dgGridView.Columns(8).Visible = True
			dgGridView.Columns(12).Visible = False  'AircraftReg
			dgGridView.Columns(13).Visible = False  'POTowards
		End If
		'Sankalp  30-10-25
		If AppSettings("ClientCode") = "7AR" Then
			dgGridView.Columns(11).Visible = True    'Due Date
		Else
			dgGridView.Columns(11).Visible = False   'Due Date
		End If
	End Sub
	Private Sub setPeriod(ByVal Index As Int32)
		Select Case Index
			Case 0 'All'
				txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
				txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
			Case 1 'Last 1 Week
				txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat").ToString)
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 2 'Last 1 Month
				txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat").ToString)
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 3 'Last 1 Quater
				Select Case Today.Month
					Case 1, 2, 3
						txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
						txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat").ToString)
					Case 4, 5, 6
						txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
						txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
					Case 7, 8, 9
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
						txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
					Case 10, 11, 12
						txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
						txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)
				End Select
			Case 4 'Last 1 Year
				txtFromDate.Text = Today.AddDays(1).AddYears(-1).ToString(AppSettings("DateFormat").ToString)
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 5 'Current Financial Year
				If Today.Month <= 3 Then  'Jan|Feb|Mar
					txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat").ToString)
				Else
					txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat").ToString)    '31-Mar-2006
				End If
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
			Case 6 'Between Dates
				FromDate = IIf(DateIndex = 6 And FromDate <> "", FromDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
				ToDate = IIf(DateIndex = 6 And ToDate <> "", ToDate, Today.Date.ToString(AppSettings("DateFormat").ToString)) 'Changes by Prashant on 09-01-2008
				txtFromDate.Text = FromDate
				txtToDate.Text = ToDate
		End Select
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub
	Private Sub ClearControls()
		txtNo.Text = ""
		txtAmend.Text = ""
		'txtName.Text = ""
	End Sub
	Private Sub addAttributes()
		txtNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtNo').value,event)")
	End Sub

	Private Sub SetModuleName()

		If mOrder IsNot Nothing Then

			If mOrder.TransTypeID = 5 Then
				mModuleName = "Order"
			End If
			If mOrder.TransTypeID = 31 Then
				mModuleName = "OrderForExchange"
			End If
			If mOrder.TransTypeID = 38 And mOrder.IsOverhaul = True Then
				mModuleName = "PurchaseOrderRepairOverHaul"
			End If
			If mOrder.TransTypeID = 38 And mOrder.IsOverhaul = False Then
				mModuleName = "PurchaseOrderRepairOverHaul"
			End If
			If mOrder.TransTypeID = 39 Then
				mModuleName = "PurchaseOrderRentalLease"
			End If
			Session("mModuleName") = mModuleName

		End If

	End Sub

	Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
		Dim IsInRoleString As String = ""
		'Deciding IsInRole String to check Rights
		Select Case mTransTypeID
			Case Util.Trans.PurchaseOrder
				IsInRoleString = "Order"
			Case Util.Trans.PurchaseOrderForExchangeRepair
				IsInRoleString = "OrderForExchange"
			Case Util.Trans.OverHaulRepairOrder
				IsInRoleString = "PurchaseOrderRepairOverHaul"
			Case Util.Trans.RentialLeaseOtder
				IsInRoleString = "PurchaseOrderRentalLease"
		End Select
		'Depending upon decided IsInRole String; checkign Rights of the User
		Select Case CheckFor
			Case Rights.[New]
				Return User.IsInRole(IsInRoleString + "New")
			Case Rights.Edit
				Return User.IsInRole(IsInRoleString + "Edit")
			Case Rights.Save
				Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
			Case Rights.Delete
				Return User.IsInRole(IsInRoleString + "Delete")
			Case Rights.View
				Return User.IsInRole(IsInRoleString + "View")
			Case Rights.Print
				Return User.IsInRole(IsInRoleString + "Print")
			Case Rights.FindNow
				Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
		End Select
	End Function
	Public Sub SendMail()
		'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
		'If AppSettings("MailsRequire") = "True" Then
		If mModuleList.Item("Order").MailsRequire = True Then
			If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019 
				'Do nothing
				Exit Sub
			End If
			Dim str As String
			str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Order No.: <b> " & mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend) & "</b> Created on: <b> " + mOrder.OrderDateFormatted + "</b> Deleted by User: <b> " + User.Identity.Name + " </b> on: <b> " + New SmartDate(Today.Date).FormattedText + "</b>,</font></P> ")
			str = str + ("</body></html>")
			SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Order Deleted", mOrder.Text + "-" + mOrder.No.ToString + IIf(mOrder.Amend = "", "", "-" + mOrder.Amend), Info:=str, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
		End If
	End Sub
#End Region

#Region " DataFieldBind "
	Private Sub DataFieldBind()
		FromDate = IIf(IsNothing(FromDate), "1/1/1900", FromDate)
		ToDate = IIf(IsNothing(ToDate), "1/1/2200", ToDate)

		SearchValue = IIf(IsNothing(SearchValue), 1, SearchValue)
		DateIndex = IIf(IsNothing(DateIndex), 1, DateIndex)

		mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
		cmbOrderText.DataSource = mDistinctTextListForOrder

		POAgainstType = IIf(IsNothing(POAgainstType), 0, POAgainstType)
		POFor = IIf(IsNothing(POFor), 0, POFor)
		POrderType = IIf(IsNothing(POrderType), 0, POrderType)

		Session("mDistinctTextListForOrder") = mDistinctTextListForOrder

		mPOTowards = POTowards.GetPOTowards("(All)")
		cmbPOTowards.DataSource = mPOTowards

		mDistinctTextListForRequisitionOnPurchaseOrderList = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)")
		cmbRequisitionText.DataSource = mDistinctTextListForRequisitionOnPurchaseOrderList
		Session("mDistinctTextListForRequisitionOnPurchaseOrderList") = mDistinctTextListForRequisitionOnPurchaseOrderList

		DataBind()
	End Sub
	Public Sub TotalCount()
		'mTransactionListCount = TransactionListCount.GetTransactionListCountt(, OrderType)
		'Session("mTransactionListCount") = mTransactionListCount
		'lblPurchaseOrderList.Text = "List of Purchase Orders" & " [Total No of Record(s):-" & mTransactionListCount(0).Count.ToString & "]"
		lblPurchaseOrderList.Text = "List of Purchase Orders"
		upnlTitle.Update()
	End Sub
	Public Sub GridBind()
		dgGridView.DataBind()
		upnlGridView.Update()
	End Sub
	Private Sub PendingTransCount()
		mPendingTransactionCount = PendingTransactionCount.GetCount(Today.Date.ToString, ClientCode:=AppSettings("ClientCode"))
		Session("mPendingTransactionCount") = mPendingTransactionCount
	End Sub
	Private Sub EnabledDisabled()
		'btnBottomPrint.Enabled = IIf(mOrderList.Count = 0, False, True)
		btnPrintTop.Enabled = IIf(mOrderList.Count = 0, False, True)
		'btnBottomExport.Enabled = IIf(mOrderList.Count = 0, False, True)
		btnExportTop.Enabled = IIf(mOrderList.Count = 0, False, True)
		upnlTitle.Update()
		'upnTopButtons.Update()
		'upnBottomButtons.Update()
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
		ClearAll()
		addAttributes()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Saylee on 19-July-2011
		If Not IsPostBack And Session("sender") = "" Then
			If cmbDate.Enabled = True Then
				setFocus(cmbDate)
			End If

			OrderType = Request.QueryString("OrderType")
			Session("OrderType") = OrderType
			Session("MiddleFrame") = "wfPurchaseOrderList_Ajax.aspx?OrderType=" & OrderType
			cmbShowE.SelectedValue = "4"
			DataFieldBind()
			TotalCount()
			'New Addition By Yogita on 17-Dec-2007 to solve Bug No:- PO_30_A2
			If OrderType <> 2 Then
				lblOrderTypeSearch.Visible = True
				cmbSearchOrderType.Visible = True
				lblRequisitionNo.Visible = True
				cmbRequisitionText.Visible = True
				txtRequisitionNo.Visible = True
				'cmbSearch.Items.Add(New System.Web.UI.WebControls.ListItem("Order Type", 10))
				'cmbSearch.Items.Add(New System.Web.UI.WebControls.ListItem("Requisition No.", 12))
			End If
			If AppSettings("ClientCode") = "CE" Then
				'cmbSearch.Items(9).Text = "AC Tail"
				lblForAircraftSearch.Text = "AC Tail"
				If OrderType <> 2 Then
					'cmbSearch.Items.Add(New System.Web.UI.WebControls.ListItem("PO. Towards", 11))
					lblPOTowardsSearch.Visible = True
					cmbPOTowards.Visible = True
				End If
			Else
				'lblForAircraftSearch.Text = "For Aircraft"
				lblForAircraftSearch.Text = "Aircraft"
			End If
			PendingTransCount()
			SetControl()
			EnabledDisabled()
			If Session("POCreated") Is Nothing Then
				If AppSettings("NewUi") = "True" And OrderType <> 2 Then
					CreatePOFromNewUI(sender:=sender, e:=e)
					Session("POCreated") = True
				End If
			End If

		End If
		SetModuleName()
	End Sub
	Private Sub dgGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgGridView.RowCommand

		Dim mID As New Guid(e.CommandArgument.ToString)
		Try

			Select Case e.CommandName
				Case "EditView"

					EditRecord(mID)
					If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
						MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If
					SetModuleName()
					Dim OrderDetail As String = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mOrderList(mOrder.ID).VendorName & " Created By : " & mOrder.UserName
					MarkLog(Util.Action.Edit, mModuleName, OrderDetail, Util.ErrorType.NoError, mOrder.ID, EventLogID)
					Dim str As String
					str = "openledgersame('wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

				Case "DeleteRecord"

					mTransTypeID = mOrderList(mID).TransID
					If (Not IsInRole(Rights.Delete)) Then
						MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If
					DeleteRecord(mID)
				'Added By Vikrant On 23-Dec-2014 For All23122014-2
				Case "ViewRec"

					'Sankalp 04-09-25
					Dim mFileAttachments As New FileAttachments
					mFileAttachments = FileAttachments.GetChildFileAttachments(ReferenceID:=mID)
					Dim AttachmentCount As Integer = mFileAttachments.Count

					If AttachmentCount > 1 Then

						Session("mFileAttachments") = mFileAttachments
						ScriptManager.RegisterStartupScript(Me, [GetType], "OpenAttachWindow", "OpenAttachWindow();", True)

					Else

						Dim FileAttach As FileAttach
						FileAttach = FileAttach.GetAttachment(ReferenceID:=mID)

						AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

						ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)


					End If

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	'Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
	'    cmbDate.SelectedIndex = 0
	'    cmbOrderText.SelectedIndex = 0
	'    cmbRequisitionText.SelectedIndex = 0
	'    ClearControls()
	'    Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0 And cmbDate.Visible, cmbDate.SelectedIndex, 0)
	'      ControlVisibility(cmbSearch.SelectedValue, DateIndex)
	'    setPeriod(DateIndex)
	'    If cmbSearch.Enabled = True Then
	'        setFocus(cmbSearch)
	'    End If
	'End Sub
	Private Sub cmbDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged, cmbOrderText.SelectedIndexChanged, cmbRequisitionText.SelectedIndexChanged
		If sender.ID = "cmbDate" Then
			'ClearControls()
			Dim DateIndex As Int32 = IIf(cmbDate.SelectedIndex >= 0, cmbDate.SelectedIndex, 0)
			ControlVisibility(1, DateIndex)
			setPeriod(DateIndex)
			If cmbDate.Enabled = True Then
				setFocus(cmbDate)
			End If
		ElseIf sender.id = "cmbOrderText" Then
			'ClearControls()
			txtNo.Text = "0"
			txtAmend.Text = ""
			If cmbOrderText.Enabled = True Then
				setFocus(cmbOrderText)
			End If
		ElseIf sender.id = "cmbRequisitionText" Then
			'ClearControls()
			txtRequisitionNo.Text = "0"
			If cmbRequisitionText.Enabled = True Then
				setFocus(cmbRequisitionText)
			End If
		End If
	End Sub
	Private Sub setVariables()
		'SearchValue = IIf(cmbSearch.SelectedValue < 0, 0, cmbSearch.SelectedValue)
		DateIndex = IIf(cmbDate.SelectedIndex < 0, 0, cmbDate.SelectedIndex)
		FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "1/1/1900")
		ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "1/1/2200")
		StatusId = IIf(cmbStatus.SelectedIndex <= 0, 0, cmbStatus.SelectedValue)
		Priority = IIf(cmbPriority.SelectedIndex <= 0, 0, cmbPriority.SelectedValue)
		PoToW = IIf(cmbPOTowards.SelectedIndex <= 0, 0, cmbPOTowards.SelectedValue)

		OrderText = IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedValue)
		ReqTextOnPurchaseOrderList = IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue)
		PartNoSearchForOrder = txtPartNoSearch.Text.Trim
		OrderNoSearch = txtNo.Text.Trim
		Amend = txtAmend.Text.Trim
		InternalOrderNoSearch = txtInternalOrderNo.Text.Trim
		SupplierForOrdeSearch = txtSupplier.Text.Trim
		ReqNoOnPurchaseOrderList = txtRequisitionNo.Text.Trim
		AircraftSearchForPO = txtForAircraftSearch.Text.Trim
		QuotationNoForPO = txtQuotationNo.Text.Trim
		IsPBHPurchaseOnOrderList = chkIsPBHPurchase.Checked
		SearchOrderType = IIf(cmbSearchOrderType.SelectedIndex <= 0, 0, cmbSearchOrderType.SelectedValue)
		SearchText = IIf(txtSearchBox.Text.Trim = "", "", txtSearchBox.Text.Trim)
		If OrderType = 1 Then
			Select Case cmbSearchOrderType.SelectedIndex
				Case 0
					SearchOrderType = 0
					IsOverhaul = False
				Case 1 'New Purchase i.e 5 TranstypeID
					SearchOrderType = Util.Trans.PurchaseOrder
					IsOverhaul = False
				Case 2 'Exchange i.e 31 TranstypeID
					SearchOrderType = Util.Trans.PurchaseOrderForExchangeRepair
					IsOverhaul = False
				Case 3 'Exchange i.e 38 TranstypeID OverHaul
					SearchOrderType = Util.Trans.OverHaulRepairOrder
					IsOverhaul = True
				Case 4 'Exchange i.e 38 TranstypeID Repair
					SearchOrderType = Util.Trans.OverHaulRepairOrder
					IsOverhaul = False
			End Select
		Else
			SearchOrderType = Util.Trans.RentialLeaseOtder
			IsOverhaul = False
		End If
		Session("FromDate") = FromDate
		Session("ToDate") = ToDate
		Session("SearchValue") = SearchValue
		Session("DateIndex") = DateIndex
		Session("StatusId") = StatusId
		Session("Priority") = Priority
		Session("OrderText") = OrderText
		Session("PartNoSearchForOrder") = PartNoSearchForOrder
		Session("OrderNoSearch") = OrderNoSearch
		Session("AmendSearch") = Amend
		Session("SearchOrderType") = SearchOrderType
		Session("IsOverhaul") = IsOverhaul
		Session("PoToW") = PoToW
		Session("ReqTextOnPurchaseOrderList") = ReqTextOnPurchaseOrderList
		Session("InternalOrderNoSearch") = InternalOrderNoSearch
		Session("SupplierForOrdeSearch") = SupplierForOrdeSearch
		Session("ReqNoOnPurchaseOrderList") = ReqNoOnPurchaseOrderList
		Session("AircraftSearchForPO") = AircraftSearchForPO
		Session("QuotationNoForPO") = QuotationNoForPO
		Session("IsPBHPurchaseOnOrderList") = IsPBHPurchaseOnOrderList
		Session("SearchTextForPOList") = SearchText
	End Sub
	Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click, txtSearchBox.TextChanged
		Flag = 1 'VVVVVVVVVV
		setVariables()
		CallFindNow(SearchValue)
		EnabledDisabled()

		lblResult.Text = "As per criteria :" & mOrderList.Count & " Record(s) found."
		GridBind()
		upnTopButtons.Update()
	End Sub
	Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewTop.Click 'btnBottomAddNew.Click,
		NewRecord()
		If (Not IsInRole(Rights.[New])) Then
			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization, MSGBox.Message_Text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If
		SetModuleName()
		MarkLog(Util.Action.[New], mModuleName, "", Util.ErrorType.NoError, mOrder.ID, EventLogID)
		If mOrder.AgainstTypeID = 1 Then
			Dim str As String
			str = "openledgersame('wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
		End If
		If mOrder.AgainstTypeID = 2 Then
			If cmbPOAgainstType.SelectedValue = 2 Then
				Dim str As String
				str = "openledgersame('wfPendingPurchaseQuotations_Ajax.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
			End If
		End If
		If mOrder.AgainstTypeID = 3 Then
			If cmbPOAgainstType.SelectedValue = 3 Then
				Dim str As String
				If AppSettings("NewRequisition") = "True" Then 'Added By Prashant 23-Jul-2012
					str = "openledgersame('wfApprovedQuotationItems_Ajax.aspx?BackPage=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				Else
					str = "openledgersame('wfMgtApprovedQuotationItems.aspx?BackPage=index.aspx');"
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
				End If

			End If
		End If
		If mOrder.AgainstTypeID = 5 Then
			Dim str As String

			'str = "openledgersame('wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
			'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)

			'Added by Shital on 18-Oct-2019
			'Added here for Exchage as Requistion Items
			If cmbPOAgainstType.SelectedIndex = 1 Then
				mOrder.OrderItems.Add(mOrder.ID)
				Session("mOrder") = mOrder
				mOrder.ExchangeOrderTypeID = 2
				str = "openledgersame('wfRequisitionPartListForPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
			Else
				mOrder.ExchangeOrderTypeID = 1
				str = "openledgersame('wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
			End If

			'-------------
		End If
		'Added By Prashant 3-Feb-2010
		If mOrder.AgainstTypeID = 4 Then  'Sales Order
			Dim str As String
			str = "openledgersame('wfSalesOrderForPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
		End If
		'----------------------------
		'Added by vikrant For New Requisition
		If mOrder.AgainstTypeID = 6 Then
			Dim str As String
			'str = "openledgersame('wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
			mOrder.OrderItems.Add(mOrder.ID)
			Session("mOrder") = mOrder
			str = "openledgersame('wfRequisitionPartListForPurchaseOrder_Ajax.aspx?BackPage=index.aspx');"
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
		End If
		'Added By Vikrant On 04-Jan-2017 For ALL04012017
		If mOrder.AgainstTypeID = 7 Then
			mOrder.OrderItems.Add(mOrder.ID)
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfPendingEnquiryItemsForOrder_Ajax.aspx?BackPage=index.aspx');", True)
		End If
		'End
	End Sub
	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click 'btnBottomClose.Click,
		RemoveSession()
		Session("MiddleFrame") = ""
		Response.Redirect("Dashboard.aspx")
	End Sub
	Private Sub cmbOrderType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOrderType.SelectedIndexChanged
		Select Case cmbOrderType.SelectedIndex
			Case Is = 0
				cmbPOAgainstType.Items.Clear()
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("None (Part)", 1))
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Quotations" + " (" + mPendingTransactionCount.QuotationCountForOrder.ToString + ")", 2))
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Sales Order" + " (" + mPendingTransactionCount.SalesOrderCountForOrder.ToString + ")", 4))
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Requisition Items" + " (" + mPendingTransactionCount.ReqItemCountForOrder.ToString + ")", 6))
				'cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Approved Quotations", 3))
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Enquiry Items" + " (" + mPendingTransactionCount.EnqItemCountForOrder.ToString + ")", 7)) 'Added By Vikrant On 04-Jan-2017 For ALL04012017
			Case Is = 1
				cmbPOAgainstType.Items.Clear()
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("From Stock", 5))
				'Added by Shital on 18-Oct-2019
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Requisition Items" + " (" + mPendingTransactionCount.ReqExchangeItemCountForOrder.ToString + ")", 8))
				'Commented by Shital on 08-jul-2021 for client requirement n added it (2,3) in case 1
				'Case Is = 2, 3
				'    cmbPOAgainstType.Items.Clear()
				'    cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("From Stock", 5))
			Case Is = 2, 3
				cmbPOAgainstType.Items.Clear()
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("From Stock", 5))
				'Added by Shital on 18-Oct-2019
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Requisition Items" + " (" + mPendingTransactionCount.ReqExchangeItemCountForOrder.ToString + ")", 8))
				cmbPOAgainstType.Items.Add(New System.Web.UI.WebControls.ListItem("Quotations" + " (" + mPendingTransactionCount.RepairOverhulQuotationCountForOrder.ToString + ")", 2))
		End Select
	End Sub
	Private Sub dgGridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgGridView.PageIndexChanging
		dgGridView.PageIndex = e.NewPageIndex
		dgGridView.DataSource = mOrderList
		Session("mOrderList") = mOrderList
		GridBind()
	End Sub
	Private Sub dgGridView_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgGridView.Sorting
		mOrderList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mOrderList") = mOrderList
		dgGridView.DataSource = mOrderList
		GridBind()
	End Sub
	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub
	Private Sub btnExportTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportTop.Click ', btnBottomExport.Click
		Dim da As New CSLA.Data.ObjectAdapter
		Dim mdsOrderList As New dsOrderList
		mdsOrderList.Clear()
		mOrderList = Session("mOrderList")
		SearchStr1 = ""
		SearchStr2 = ""

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
		mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
		mCompanyDetail.WebSite, "Order List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"),
		"", "", "", "", AppSettings("Logo"))

		da.Fill(mdsOrderList, "ReportData", Report)
		da.Fill(mdsOrderList, "OrderList", mOrderList)


		Dim columnToRemove2 As String() = {"ID", "SearchStr5", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion",
										   "SINote", "CurrencyName", "CurrencySymbol", "SearchStr3", "SearchStr4", "SearchStr6", "SearchStr7",
										   "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14",
										   "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20",
										   "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

		For i As Integer = 0 To columnToRemove2.Length - 1
			If mdsOrderList.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
				mdsOrderList.Tables("ReportData").Columns.Remove(columnToRemove2(i))
			End If
		Next

		Dim columnToRemove As String() = {"ID", "OrderDate", "OrderDateSorting", "Text", "No", "Amend", "QuotationDate", "TransTypeID",
										  "BillName", "ShipName", "LocationName", "IsFOC", "IsOverhaul", "IsCustomer", "AgainstTypeID", "OpeningLine",
										  "Priority", "SrNo", "CurrencyID", "ConversionFactor", "TransID", "IsAttachmentAdded", "VendorID",
										  "IsCalibrationOrder", "QuotationNo", "QuotationDateFormatted"}

		For i As Integer = 0 To columnToRemove.Length - 1
			If mdsOrderList.Tables("OrderList").Columns.Contains(columnToRemove(i)) Then
				mdsOrderList.Tables("OrderList").Columns.Remove(columnToRemove(i))
			End If
		Next

		Dim dsNew As New DataSet
		dsNew.Clear()

		dsNew.Merge(mdsOrderList.Tables("ReportData"))
		dsNew.Merge(mdsOrderList.Tables("OrderList"))

		dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "Criteria"
		dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "By"

		dsNew.Tables("OrderList").Columns("OrderDateFormatted").ColumnName = "Date"
		dsNew.Tables("OrderList").Columns("OrderNo").ColumnName = "Number"
		dsNew.Tables("OrderList").Columns("IntOrderNo").ColumnName = "Int. Order No."
		dsNew.Tables("OrderList").Columns("OrderType").ColumnName = "Type"
		dsNew.Tables("OrderList").Columns("VendorName").ColumnName = "Supplier"
		dsNew.Tables("OrderList").Columns("KindAttn").ColumnName = "Kind Attn."
		dsNew.Tables("OrderList").Columns("CGrandTotal").ColumnName = "Grand Total"
		dsNew.Tables("OrderList").Columns("CurrencyName").ColumnName = "Currency"
		dsNew.Tables("OrderList").Columns("DeliveryWithinDays").ColumnName = "Delivery in Days"
		dsNew.Tables("OrderList").Columns("UserName").ColumnName = "User Name"
		dsNew.Tables("OrderList").Columns("AuthorizedBy").ColumnName = "Authorized By"
		If AppSettings("ClientCode") = "CE" Then
			dsNew.Tables("OrderList").Columns("AircraftReg").ColumnName = "AC Tail"
		Else
			dsNew.Tables("OrderList").Columns("AircraftReg").ColumnName = "Aircraft"
		End If
		dsNew.Tables("OrderList").Columns("POTowards").ColumnName = "PO. Towards"

		dsNew.Tables("ReportData").TableName = "Searching Criteria"
		dsNew.Tables("OrderList").TableName = "Order List Report"
		Session("ExcelFileName") = "Order List Report"
		Session("dsNew") = dsNew
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
		'Added by Shital on 18-Jan-2021
		Dim OrderDetail As String = mOrder.OrderNo + " Dated : " + mOrder.OrderDateFormatted + " to " + mOrderList(mOrder.ID).VendorName & " Created By : " & mOrder.UserName
		MarkLog(Util.Action.Print, mModuleName, "Export To excel " + OrderDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
		'--------
	End Sub
	Protected Sub OnRowDataBound(sender As Object, e As GridViewRowEventArgs) 'Added By Prashant 5-Aug-2020 All05082020
		If e.Row.RowType = DataControlRowType.DataRow Then
			If (CDbl(e.Row.Cells(21).Text) <= 0.0) Then 'Sum Receipt Balance Qty Column
				e.Row.Cells(19).BackColor = Color.Green
				'e.Row.Cells(18).Width = 40
			Else
				e.Row.Cells(19).BackColor = Color.YellowGreen
				'e.Row.Cells(18).Width = 40
			End If
		End If
	End Sub
	Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
		dgGridView.PageSize = CInt(cmbShowE.SelectedItem.ToString)
		dgGridView.DataSource = mOrderList
		dgGridView.DataBind()
		ControlVisibility(0)
		setVariables()
		SetControl()
		upnlGridView.Update()
	End Sub

	Private Sub CreatePOFromNewUI(sender As Object, e As EventArgs) Handles btnNewUi.Click

		Dim NewUrl As String = RedirectToNewUIHelper.NavigationLinkForNewUI(Request:=Request,
																			 NavigationLink:="Procurement?tab=orders")

		ScriptManager.RegisterStartupScript(Me,
											[GetType],
											"Open in New Tab",
											$"window.open('{NewUrl}', '_blank');",
											True)

	End Sub

#End Region

#Region " Report "
	'Created By :- Jyoti
	'Dated On 9/5/2007
#Region "Report Variable Declaration"
	Dim mCompanyDetail As New CompanyDetail
	Dim objStatus As rptStatus
	Private SearchStr1 As String
	Private SearchStr2 As String
#End Region

#Region "Event"
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintTop.Click ' btnBottomPrint.Click,
		Dim Rpt As New crOrderList
		Dim da As New CSLA.Data.ObjectAdapter
		Dim ds As New dsCommon
		Dim ReportDetails As New rptStatusList
		SearchStr1 = ""
		SearchStr2 = ""
		'If cmbSearch.SelectedIndex = 0 Then
		'    'All
		'    SearchStr1 = "The report shows all records till date."
		'    SearchStr2 = ""
		'ElseIf cmbSearch.SelectedIndex = 1 Then
		'    'Date
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    If cmbDate.SelectedIndex = 0 Then
		'        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text
		'    ElseIf cmbDate.SelectedIndex = 6 Then
		'        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text.ToString).FormattedText
		'    Else
		'        SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbDate.SelectedItem.Text + " " + lblFromDate.Text + " " + New SmartDate(txtFromDate.Text.ToString).FormattedText + " " + lblToDate.Text + " " + New SmartDate(txtToDate.Text.ToString).FormattedText
		'    End If
		'ElseIf cmbSearch.SelectedIndex = 2 Then
		'    'Order
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbOrderText.SelectedItem.Text + " " + lblNo.Text + " " + txtNo.Text + " " + "_" + txtAmend.Text
		'ElseIf cmbSearch.SelectedIndex = 3 Then
		'    'Internal Order No.
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		'ElseIf cmbSearch.SelectedIndex = 4 Then
		'    'Part Number
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		'ElseIf cmbSearch.SelectedIndex = 5 Then
		'    'Vendor
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		'ElseIf cmbSearch.SelectedIndex = 6 Then
		'    'Quotation No.
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		'ElseIf cmbSearch.SelectedIndex = 7 Then
		'    'Status
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbStatus.SelectedItem.Text
		'ElseIf cmbSearch.SelectedIndex = 8 Then
		'    'Priority
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbPriority.SelectedItem.Text
		'ElseIf cmbSearch.SelectedIndex = 9 Then
		'    'For Aircraft
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + txtName.Text
		'ElseIf cmbSearch.SelectedIndex = 10 Then
		'    'Order Type
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbSearchOrderType.SelectedItem.Text
		'ElseIf cmbSearch.SelectedIndex = 11 Then
		'    'PO Towards
		'    SearchStr1 = "The report shows records filtered by the following criteria"
		'    SearchStr2 = "By" + " " + cmbSearch.SelectedItem.Text + " " + ":" + " " + cmbPOTowards.SelectedItem.Text
		'End If
		ReportDetails.Add(New rptStatus(, 0, ,
			  dgGridView.Columns.Item(1).HeaderText, dgGridView.Columns.Item(2).HeaderText, dgGridView.Columns.Item(3).HeaderText,
			  dgGridView.Columns.Item(4).HeaderText, dgGridView.Columns.Item(5).HeaderText, dgGridView.Columns.Item(6).HeaderText,
			  IIf(AppSettings("ClientCode") = "CE", dgGridView.Columns.Item(12).HeaderText, dgGridView.Columns.Item(7).HeaderText),
			  IIf(AppSettings("ClientCode") = "CE", dgGridView.Columns.Item(13).HeaderText, dgGridView.Columns.Item(8).HeaderText),
			  dgGridView.Columns.Item(9).HeaderText, dgGridView.Columns.Item(10).HeaderText, dgGridView.Columns.Item(12).HeaderText,
			  dgGridView.Columns.Item(15).HeaderText, dgGridView.Columns.Item(16).HeaderText, dgGridView.Columns.Item(17).HeaderText))
		Dim TotalCount As Integer
		TotalCount = Me.dgGridView.PageCount
		Dim j As Integer
		Dim I As Integer
		Dim str(13) As String
		For j = 0 To TotalCount - 1
			Me.dgGridView.PageIndex = j
			Me.dgGridView.DataSource = mOrderList
			Session("mOrderList") = mOrderList
			dgGridView.DataBind()

			For I = 0 To Me.dgGridView.PageSize - 1
				If I <= Me.dgGridView.Rows.Count - 1 Then

					str(0) = ""
					str(1) = ""
					str(2) = ""
					str(3) = ""
					str(4) = ""
					str(5) = ""
					str(6) = ""
					str(7) = ""
					str(8) = ""
					str(9) = ""
					str(10) = ""
					str(11) = ""
					str(12) = ""
					str(13) = ""
					If Me.dgGridView.Rows(I).Cells.Item(1).Text <> "&nbsp;" Then str(0) = Me.dgGridView.Rows(I).Cells.Item(1).Text
					If Me.dgGridView.Rows(I).Cells.Item(2).Text <> "&nbsp;" Then str(1) = Me.dgGridView.Rows(I).Cells.Item(2).Text
					If Me.dgGridView.Rows(I).Cells.Item(3).Text <> "&nbsp;" Then str(2) = Me.dgGridView.Rows(I).Cells.Item(3).Text
					If Me.dgGridView.Rows(I).Cells.Item(4).Text <> "&nbsp;" Then str(3) = Me.dgGridView.Rows(I).Cells.Item(4).Text
					If Me.dgGridView.Rows(I).Cells.Item(5).Text <> "&nbsp;" Then str(4) = Me.dgGridView.Rows(I).Cells.Item(5).Text
					If Me.dgGridView.Rows(I).Cells.Item(6).Text <> "&nbsp;" Then str(5) = Me.dgGridView.Rows(I).Cells.Item(6).Text

					If AppSettings("ClientCode") = "CE" Then
						If Me.dgGridView.Rows(I).Cells.Item(13).Text <> "&nbsp;" Then str(6) = Me.dgGridView.Rows(I).Cells.Item(13).Text
						If Me.dgGridView.Rows(I).Cells.Item(14).Text <> "&nbsp;" Then str(7) = Me.dgGridView.Rows(I).Cells.Item(14).Text
					Else
						If Me.dgGridView.Rows(I).Cells.Item(7).Text <> "&nbsp;" Then str(6) = Me.dgGridView.Rows(I).Cells.Item(7).Text
						If Me.dgGridView.Rows(I).Cells.Item(8).Text <> "&nbsp;" Then str(7) = Me.dgGridView.Rows(I).Cells.Item(8).Text
					End If

					If Me.dgGridView.Rows(I).Cells.Item(9).Text <> "&nbsp;" Then str(8) = Me.dgGridView.Rows(I).Cells.Item(9).Text
					If Me.dgGridView.Rows(I).Cells.Item(10).Text <> "&nbsp;" Then str(9) = Me.dgGridView.Rows(I).Cells.Item(10).Text
					If Me.dgGridView.Rows(I).Cells.Item(12).Text <> "&nbsp;" Then str(10) = Me.dgGridView.Rows(I).Cells.Item(12).Text
					If Me.dgGridView.Rows(I).Cells.Item(15).Text <> "&nbsp;" Then str(11) = Me.dgGridView.Rows(I).Cells.Item(15).Text
					If Me.dgGridView.Rows(I).Cells.Item(16).Text <> "&nbsp;" Then str(12) = Me.dgGridView.Rows(I).Cells.Item(16).Text
					If Me.dgGridView.Rows(I).Cells.Item(17).Text <> "&nbsp;" Then str(13) = Me.dgGridView.Rows(I).Cells.Item(17).Text
					ReportDetails.Add(New rptStatus(, 1, , str(0),
						str(1), str(2), str(3), str(4), str(5), str(6), str(7), str(8), str(9), str(10), str(11), str(12), str(13)))

				End If
			Next
		Next
		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
		mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
		mCompanyDetail.WebSite, "Order List Report", SearchStr1, SearchStr2, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
		Dim mrptImage As rptImage = rptImage.GetImage(ds)
		da.Fill(ds, mrptImage)
		da.Fill(ds, ReportDetails)
		da.Fill(ds, Report)
		Rpt.SetDataSource(ds)
		Session("CrystalReport") = Rpt
		SetModuleName()
		Dim Str1 As String
		Str1 = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
		Me.dgGridView.DataSource = mOrderList
		Session("mOrderList") = mOrderList
		dgGridView.DataBind()
	End Sub
#End Region

#End Region

End Class