Public Class wfPartStockStatusList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration"
    Public mOrder As Order
    Dim PartNo As String
    Dim mItemId As Guid = Guid.Empty
    Dim mItemStockStatusList As ItemStockStatusList
    Public mPendingFromListSalesOrder As PendingFromList
    'Public mPendingFromListRequisition As PendingFromList
    Public mReOrderLevelItemList As ReOrderLevelItemList
    Private mInvoiceItemListForFinanceApproval As InvoiceItemListForFinanceApproval
    Public mStockQtyDetailsForOrder As QtyDetailsForOrder
    Public mPendingQtyDetailsForOrder As QtyDetailsForOrder
    Public mReturnableQtyDetailsForOrder As QtyDetailsForOrder
    Public mPendingQtyDetailsForEROOrder As QtyDetailsForOrder
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mOrder = Session("mOrder")
        PartNo = Session("PartNo")
        mItemId = Session("mItemId")
        mItemStockStatusList = Session("mItemStockStatusList")
        mPendingFromListSalesOrder = Session("mPendingFromListSalesOrder")
        'mPendingFromListRequisition = Session("mPendingFromListRequisition")
        mReOrderLevelItemList = Session("mReOrderLevelItemList")
    End Sub
    Private Sub setSession()
        Session("mItemId") = mItemId
        Session("mOrder") = mOrder
        Session("mItemStockStatusList") = mItemStockStatusList
        Session("mPendingFromListSalesOrder") = mPendingFromListSalesOrder
        'Session("mPendingFromListRequisition") = mPendingFromListRequisition
        Session("mReOrderLevelItemList") = mReOrderLevelItemList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemId")
        Session.Remove("mItemStockStatusList")
        Session.Remove("mPendingFromListSalesOrder")
        'Session.Remove("mPendingFromListRequisition")
        Session.Remove("mReOrderLevelItemList")
        Session.Remove("PartNo")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub setObject(ByVal ItemId As Guid, ByVal UnitID As Guid, ByVal UnitName As String)
        mOrder.OrderItems.CurrentItem.ItemID = mItemId
        mOrder.OrderItems.CurrentItem.ItemFrom = FromOrder.PreviousTrans.Direct
        mOrder.OrderItems.CurrentItem.FromItemID = Guid.Empty
        mOrder.OrderItems.CurrentItem.FromNo = ""
        mOrder.OrderItems.CurrentItem.FromDate = ""
        mOrder.OrderItems.CurrentItem.IsSerializedPart = mItemStockStatusList(ItemId).IsSerialised 'Added By Prashant 5-Feb-2019 ALL04022019
        If (mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.AgainstTypeID = 5 Then
            If mItemStockStatusList(ItemId).IsSerialised Then
                mOrder.OrderItems.CurrentItem.Qty = 1
                mOrder.OrderItems.CurrentItem.IsSerializedPart = True
            End If
        Else
            mOrder.OrderItems.CurrentItem.Qty = 0D
        End If
        Dim mCRateOfLastOrderedItem As CRateOfLastOrderedItem
        mCRateOfLastOrderedItem = CRateOfLastOrderedItem.GetCRateOfLastOrderedItem(mOrder.TransTypeID, ItemId.ToString)

        If mCRateOfLastOrderedItem(0).ItemCRate <> 0 Then
            mOrder.OrderItems.CurrentItem.CRate = mCRateOfLastOrderedItem(0).ItemCRate
        Else
            mOrder.OrderItems.CurrentItem.CRate = mOrder.OrderItems.CurrentItem.ApproximateRate
        End If
        mOrder.OrderItems.CurrentItem.Remark = ""
        mOrder.OrderItems.CurrentItem.Note = ""
        mOrder.OrderItems.CurrentItem.UnitID = UnitID       'Added By Prashant 5-Feb-2019 ALL04022019
        mOrder.OrderItems.CurrentItem.UnitName = UnitName   'Added By Prashant 5-Feb-2019 ALL04022019
        Session("mOrder") = mOrder
    End Sub
    Private Sub setObjectFrom(Optional ByVal ItemId As String = "{00000000-0000-0000-0000-000000000000}", _
                              Optional ByVal FromItemID As String = "{00000000-0000-0000-0000-000000000000}", _
                              Optional ByVal Qty As Decimal = 0, Optional ByVal FromNo As String = "", Optional ByVal FromDate As String = "", _
                              Optional ByVal ItemFrom As FromOrder.PreviousTrans = FromOrder.PreviousTrans.Direct, _
                              Optional ByVal CustomerId As String = "{00000000-0000-0000-0000-000000000000}", _
                              Optional ByVal UnitID As String = "{00000000-0000-0000-0000-000000000000}", _
                              Optional ByVal UnitName As String = "", Optional ByVal IsSerialized As Boolean = False)
        mOrder.OrderItems.CurrentItem.ItemFrom = ItemFrom
        mOrder.OrderItems.CurrentItem.ItemID = New Guid(ItemId)
        mOrder.OrderItems.CurrentItem.FromItemID = New Guid(FromItemID)
        mOrder.OrderItems.CurrentItem.Qty = Qty
        mOrder.OrderItems.CurrentItem.FromNo = FromNo
        mOrder.OrderItems.CurrentItem.FromDate = FromDate
        mOrder.CustomerID = New Guid(CustomerId)
        mOrder.OrderItems.CurrentItem.UnitID = New Guid(UnitID)         'Added By Prashant 5-Feb-2019 ALL04022019
        mOrder.OrderItems.CurrentItem.UnitName = UnitName               'Added By Prashant 5-Feb-2019 ALL04022019
        Session("mOrder") = mOrder
    End Sub
    Private Sub GetList(Optional ByVal PartNo As String = "", Optional ByVal ItemId As String = "{00000000-0000-0000-0000-000000000000}")
        mItemStockStatusList = ItemStockStatusList.GetItemStockStatusList(PartNo, mOrder.OrderDate.ToString) 'mOrder.OrderDate.ToString Added by Prashant 19-Feb-2013 All19022013
        Session("mItemStockStatusList") = mItemStockStatusList
        lblResult.Text = "Part Stock Status List : " & mItemStockStatusList.Count & " No.of Record Found(s)."
    End Sub
    Private Sub GetDetail(Optional ByVal PartNo As String = "", Optional ByVal ItemId As String = "{00000000-0000-0000-0000-000000000000}")
        'mPendingFromListRequisition = PendingFromList.GetPendingFromList(PendingFromList.PendingListOf.Requisition, mOrder.CustomerID, New Guid(ItemId), mOrder.OrderDate.ToString)
        mPendingFromListSalesOrder = PendingFromList.GetPendingFromList(PendingFromList.PendingListOf.SalesOrder, mOrder.CustomerID, New Guid(ItemId), mOrder.OrderDate.ToString)
        mReOrderLevelItemList = ReOrderLevelItemList.GetReOrderLevelItemList(PartNo)

        Session("mPendingFromListSalesOrder") = mPendingFromListSalesOrder
        'Session("mPendingFromListRequisition") = mPendingFromListRequisition
        Session("mReOrderLevelItemList") = mReOrderLevelItemList
        upnlPendingSOItemList.Update()
        'upnlRequisitionDetails.Update()
        upnlReorderLevelList.Update()
    End Sub
    '----ADded by Shital on 04-Feb-2021
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Confirmation" Then
                        Try
                            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ItemId=" & Session("mItemId").ToString)
                            RemoveSession()
                            MarkLog(Util.Action.Save, "Part Stock Status list", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Confirmation" Then
                        Session.Remove("mItemId1")
                    End If

            End Select
        End If
    End Sub
    '--------
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgPartStockStatusList.DataSource = mItemStockStatusList
        dgPartStockStatusList.DataBind()
        dgPendingSOItemList.DataSource = mPendingFromListSalesOrder
        dgPendingSOItemList.DataBind()
        'dgPendingRequisitionItemList.DataSource = mPendingFromListRequisition
        'dgPendingRequisitionItemList.DataBind()
        dgReorderLevelList.DataSource = mReOrderLevelItemList                                       'MinReOrderLevel
        dgReorderLevelList.DataBind()

        If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 1 Then                                 'New Purchase and Part(Direct) 
            dgPartStockStatusList.Columns.Item(13).Visible = False
        End If
        If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 2 Then                                 ' New Purchase and Requisition
            dgPartStockStatusList.Columns.Item(13).Visible = True
            dgPartStockStatusList.Columns.Item(14).Visible = False
            'lblRequisitionDetails.Visible = True
            'dgPendingRequisitionItemList.Visible = True
        End If
        If mOrder.TransTypeID = 5 And mOrder.AgainstTypeID = 4 Then                                 ' New Purchase and Sales Order.
            dgPartStockStatusList.Columns.Item(13).Visible = True
            dgPartStockStatusList.Columns.Item(14).Visible = False
            dgPendingSOItemList.Visible = True
            lblDalesOrderDetail.Visible = True
        End If
        If (mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And mOrder.AgainstTypeID = 5 Then   ' (Exchange, Overhaul, Repair) and from Stock .
            dgPartStockStatusList.Columns.Item(13).Visible = False
        End If
        If mOrder.TransTypeID = 39 And mOrder.AgainstTypeID = 1 Then                                'Purchase for Rentail / Lease from Stock 
            dgPartStockStatusList.Columns.Item(13).Visible = False
        End If
    End Sub
    'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    Private Sub ControlVisibility()
        If AppSettings("ShowFirstPriorityParts") = "True" Then
            dgPartStockStatusList.Columns(5).Visible = True
        Else
            dgPartStockStatusList.Columns(5).Visible = False
        End If
    End Sub
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        If Not IsPostBack Then
            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If
            If Not PartNo = String.Empty Then
                txtSearch.Text = PartNo
                Session.Remove("PartNo")
                Session("PartNo") = Nothing
            Else
                txtSearch.Text = mOrder.OrderItems.CurrentItem.ItemName
            End If
            GetList(txtSearch.Text)
            DataFieldBind()
            ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        GetList(txtSearch.Text)
        DataFieldBind()
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
        upnlPartStockStatusList.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mOrder.OrderItems.CurrentItem.IsNew And Not Session("Edit") = True Then mOrder.OrderItems.Remove(mOrder.OrderItems.CurrentItem)
        RemoveSession()
        Session.Remove("Edit")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub dgPartStockStatusList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartStockStatusList.RowCommand
        Select Case e.CommandName
            Case "SelectRecord"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartStockStatusList.PageIndex * dgPartStockStatusList.PageSize
                GetList(txtSearch.Text, mItemStockStatusList(index).ItemID.ToString)
                GetDetail(txtSearch.Text, mItemStockStatusList(index).ItemID.ToString)
                Session("mItemId") = mItemStockStatusList(index).ItemID
                DataFieldBind()
                ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
            Case "SelectPart"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartStockStatusList.PageIndex * dgPartStockStatusList.PageSize
                Dim ItemId As Guid = mItemStockStatusList(index).ItemID
                mItemId = ItemId
                Session("mItemId") = mItemStockStatusList(index).ItemID
                setObject(ItemId, mItemStockStatusList(index).UnitID, mItemStockStatusList(index).UnitName)
                ' RemoveSession()
                'Commented by Shital on 04-Feb-2021
                ' Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ItemId=" & ItemId.ToString)
                'Added by Shital on 04-Feb-2021
                If mItemStockStatusList(index).orderItemReceiptBalanceQuantity > 0.0 Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.Alert, MSGBox.Message_Text.Alert, "An Order already exists for this Part or its Alternate Part. Do you still want to create another Order ?", MsgBoxStyle.YesNo, "Confirmation")
				Else
                    RemoveSession()
                    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ItemId=" & ItemId.ToString)
                End If

            Case "StockDetail"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartStockStatusList.PageIndex * dgPartStockStatusList.PageSize
                lblQuantityDetails.Text = "Quantity Details For Part No. - " & mItemStockStatusList(index).ItemName
                mStockQtyDetailsForOrder = QtyDetailsForOrder.GetQtyDetailsForOrderList(mItemStockStatusList(index).ItemID, QtyDetailsForOrder.SearchType.SerchType_Stock)
                mPendingQtyDetailsForOrder = QtyDetailsForOrder.GetQtyDetailsForOrderList(mItemStockStatusList(index).ItemID, QtyDetailsForOrder.SearchType.SerchType_PendinngForOutRight)
                mReturnableQtyDetailsForOrder = QtyDetailsForOrder.GetQtyDetailsForOrderList(mItemStockStatusList(index).ItemID, QtyDetailsForOrder.SearchType.SerchType_Returnable)
                mPendingQtyDetailsForEROOrder = QtyDetailsForOrder.GetQtyDetailsForOrderList(mItemStockStatusList(index).ItemID, QtyDetailsForOrder.SearchType.SerchType_PendinngForEROOrder)
                dgStock.DataSource = mStockQtyDetailsForOrder
                dgStock.DataBind()
                dgPending.DataSource = mPendingQtyDetailsForOrder
                dgPending.DataBind()
                dgReturnable.DataSource = mReturnableQtyDetailsForOrder
                dgReturnable.DataBind()
                dgPendingExchangeRepairOverhaulOrders.DataSource = mPendingQtyDetailsForEROOrder
                dgPendingExchangeRepairOverhaulOrders.DataBind()
                upnlQuantityDetails.Update()
                If (mStockQtyDetailsForOrder.Count = 0 And mPendingQtyDetailsForOrder.Count = 0 And mReturnableQtyDetailsForOrder.Count = 0 And mPendingQtyDetailsForEROOrder.Count = 0) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There are no stock details for this part", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    mdeQuantityDetails.Show()
                End If
            Case "LastTenPurchases"
                Dim index As Integer = CInt(e.CommandArgument) + dgPartStockStatusList.PageIndex * dgPartStockStatusList.PageSize
                lbltitle.Text = "Last 10 Purchases details for the Part No. - " & mItemStockStatusList(index).ItemName
                mInvoiceItemListForFinanceApproval = InvoiceItemListForFinanceApproval.GetInvoiceItemListForFinalApprovalList(mItemStockStatusList(index).ItemID)
                dgList.DataSource = mInvoiceItemListForFinanceApproval
                dgList.DataBind()
                upnlLast10Purchases.Update()
                If mInvoiceItemListForFinanceApproval.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no purchase information aginst this part", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    mdeLast10Purchases.Show()
                End If
        End Select
    End Sub
    Private Sub dgPendingSOItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingSOItemList.RowCommand
        Select Case e.CommandName
            Case "SelectRecord"
                Dim index As Integer = CInt(e.CommandArgument)
                setObjectFrom(mItemId.ToString, mPendingFromListSalesOrder(index).FromItemID.ToString, mPendingFromListSalesOrder(index).FromItemQty, _
                              mPendingFromListSalesOrder(index).FromTextNo, mPendingFromListSalesOrder(index).FromDate, FromOrder.PreviousTrans.SalesOrder, _
                              mPendingFromListSalesOrder(index).CustomerID.ToString, UnitID:=mPendingFromListSalesOrder(index).UnitID.ToString, _
                              UnitName:=mPendingFromListSalesOrder(index).UserName)
                RemoveSession()
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ItemId=" & mItemId.ToString)
        End Select
    End Sub
    'Private Sub dgPendingRequisitionItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingRequisitionItemList.RowCommand
    '    Select Case e.CommandName
    '        Case "Select"
    '            Dim index As Integer = CInt(e.CommandArgument)
    '            setObjectFrom(mItemId.ToString, mPendingFromListRequisition(index).FromItemID.ToString, mPendingFromListRequisition(index).FromItemQty, mPendingFromListRequisition(index).FromTextNo, mPendingFromListRequisition(index).FromDate, FromOrder.PreviousTrans.Requisition)
    '            RemoveSession()
    '            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ItemId=" & mItemId.ToString)
    '    End Select
    'End Sub
    Private Sub dgReorderLevelList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgReorderLevelList.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim index As Integer = CInt(e.CommandArgument)
                mOrder.OrderItems.CurrentItem.ItemID = mReOrderLevelItemList(index).ItemID
                mOrder.OrderItems.CurrentItem.Qty = mReOrderLevelItemList(index).OrderQTY
                mOrder.OrderItems.CurrentItem.CRate = mReOrderLevelItemList(index).Rate
                If mOrder.OrderItems.Contains(mOrder.OrderItems.CurrentItem) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Order Item", MsgBoxStyle.OkOnly, "")
                    mOrder.CancelEdit()
                    Exit Sub
                End If
                Session("mOrder") = mOrder
                RemoveSession()
                Response.Redirect(Request.QueryString("BackPage"))
        End Select
    End Sub
    Private Sub dgPartStockStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartStockStatusList.PageIndexChanging
        dgPartStockStatusList.PageIndex = e.NewPageIndex
        dgPartStockStatusList.DataSource = mItemStockStatusList
        Session("mItemStockStatusList") = mItemStockStatusList
        dgPartStockStatusList.DataBind()
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
        upnlPartStockStatusList.Update()
    End Sub
    Private Sub dgPartStockStatusList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartStockStatusList.Sorting
        mItemStockStatusList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItemStockStatusList") = mItemStockStatusList
        dgPartStockStatusList.DataSource = mItemStockStatusList
        dgPartStockStatusList.DataBind()
        ControlVisibility() 'Added by Vikrant On 11-Jul-2019 For ALL11072019	
        upnlPartStockStatusList.Update()
    End Sub
    Private Sub btnLast10PurchasesClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLast10PurchasesClose.Click
        mdeLast10Purchases.Hide()
    End Sub
    Private Sub btnlQuantityDetailsClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnlQuantityDetailsClose.Click
        mdeQuantityDetails.Hide()
    End Sub
    Private Sub btnAddNewPart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNewPart.Click
        If (Not User.IsInRole("PartNew")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            Dim mItem As Item
            mItem = Item.NewItem()
            Session("mItem") = mItem
            Session("Create") = "False"
            Session("PartInfo") = "True"

            Dim URL As Stack = New Stack    'STACK to store url of current page
            URL.Push(Request.Url)           'Inserting URL in STACK
            Session("URL") = URL
            Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfPartStockStatusList_Ajax.aspx")
        End If
    End Sub
    'Added by Vikrant On 11-Jul-2019 For ALL11072019	
    Private Sub dgPartStockStatusList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgPartStockStatusList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            If AppSettings("ShowFirstPriorityParts") = "True" AndAlso (e.Row.Cells(4).Text <> "" And e.Row.Cells(4).Text <> "&nbsp;") And (e.Row.Cells(2).Text <> e.Row.Cells(5).Text) Then
                e.Row.Cells(5).Font.Bold = True
            End If
        End If
    End Sub
    'End
#End Region

   
End Class