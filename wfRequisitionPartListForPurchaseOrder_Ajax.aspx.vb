Public Class wfRequisitionPartListForPurchaseOrder_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration"
    Public mOrder As Order
    Dim PartNo As String
    Dim mItemId As Guid = Guid.Empty
    Dim mItemStockStatusList As ItemStockStatusList
    Public mPendingFromListSalesOrder As PendingFromList
    Public mPendingFromListRequisition As PendingFromList
    Public mReOrderLevelItemList As ReOrderLevelItemList
    Public mRequisitionItemsNew As RequisitionItemsNew
    Public mDistinctTextListForRequisition As DistinctTextListForRequisition
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mOrder = Session("mOrder")
        PartNo = Session("PartNo")
        mItemId = Session("mItemId")
        mRequisitionItemsNew = Session("mRequisitionItemsNew")
        mPendingFromListSalesOrder = Session("mPendingFromListSalesOrder")
        mPendingFromListRequisition = Session("mPendingFromListRequisition")
        mReOrderLevelItemList = Session("mReOrderLevelItemList")
    End Sub
    Private Sub setSession()
        Session("mItemId") = mItemId
        Session("mOrder") = mOrder
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        Session("mPendingFromListSalesOrder") = mPendingFromListSalesOrder
        Session("mPendingFromListRequisition") = mPendingFromListRequisition
        Session("mReOrderLevelItemList") = mReOrderLevelItemList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemId")
        'Session.Remove("mRequisitionItemsNew")
        Session.Remove("mPendingFromListSalesOrder")
        Session.Remove("mPendingFromListRequisition")
        Session.Remove("mReOrderLevelItemList")
        Session.Remove("PartNo")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub setObject(ByVal ItemId As Guid, ByVal RequisitionItemNewID As Guid, Optional ByVal RequisitionItemNewOrderBalQty As Decimal = 0,
                          Optional ByVal RequisitionItemNewRequisitionNo As String = "", Optional UnitID As String = "{00000000-0000-0000-0000-000000000000}",
                          Optional UnitName As String = "", Optional ByVal HSNACSCode As String = "", Optional ByVal ReqDateFormatted As String = "")
        mOrder.OrderDate = txtDate.Text
        mOrder.OrderItems.CurrentItem.ItemID = mItemId
        mOrder.OrderItems.CurrentItem.ItemFrom = FromOrder.PreviousTrans.Requisition
        mOrder.OrderItems.CurrentItem.FromItemID = Guid.Empty
        mOrder.OrderItems.CurrentItem.Qty = 0D
        mOrder.OrderItems.CurrentItem.FromNo = ""
        mOrder.OrderItems.CurrentItem.FromDate = ""
        mOrder.OrderItems.CurrentItem.Qty = 0
        'Added By Prashant 3-Jun-2011
        Dim mCRateOfLastOrderedItem As CRateOfLastOrderedItem
        mCRateOfLastOrderedItem = CRateOfLastOrderedItem.GetCRateOfLastOrderedItem(mOrder.TransTypeID, ItemId.ToString)

        If mCRateOfLastOrderedItem(0).ItemCRate <> 0 Then
            mOrder.OrderItems.CurrentItem.CRate = mCRateOfLastOrderedItem(0).ItemCRate
        Else
            mOrder.OrderItems.CurrentItem.CRate = mOrder.OrderItems.CurrentItem.ApproximateRate
        End If
        mOrder.OrderItems.CurrentItem.Remark = ""
        mOrder.OrderItems.CurrentItem.Note = ""

        'Added by vikrant For New Requisition
        With mOrder.OrderItems.CurrentItem
            If Not .RequisitionItemOrderItems.Contains(RequisitionItemNewID) Then
                'if NOT then add
                .RequisitionItemOrderItems.Add(.ID, RequisitionItemNewID, RequisitionItemNewOrderBalQty, RequisitionItemNewRequisitionNo)
            Else
                'if YES fire Message
                MSGBoxCtrl.show(MSGBox.Message_title.ValidationAlert, MSGBox.Message_text.ValidationAlert, "Requisition item already taken for Order", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End With
        'End
        mOrder.OrderItems.CurrentItem.UnitID = New Guid(UnitID)     'Added By Prashant 5-Feb-2019 ALL04022019
        mOrder.OrderItems.CurrentItem.UnitName = UnitName           'Added By Prashant 5-Feb-2019 ALL04022019
        mOrder.OrderItems.CurrentItem.RequisitionTextNo = RequisitionItemNewRequisitionNo
        mOrder.OrderItems.CurrentItem.HSNACSCode = HSNACSCode 'Added By Prashant on 28-Sep-2021 For STR27092021
        mOrder.OrderItems.CurrentItem.FromNo = RequisitionItemNewRequisitionNo
        mOrder.OrderItems.CurrentItem.FromDate = ReqDateFormatted
        Session("mOrder") = mOrder
    End Sub
    Private Sub GetList(Optional ByVal PartNo As String = "", Optional ByVal ItemId As String = "{00000000-0000-0000-0000-000000000000}")
        'mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(mOrder.OrderDate, txtSearch.Text.Trim, Guid.Empty, 2, , , , , , IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue), CInt(Val(txtNo.Text)))

        'Added by Shital on 18-Oct-2019
        If mOrder.ExchangeOrderTypeID = 2 Then  'ExchangeOrderTypeID = 2 i.e ExchangeDestinationType Order against Requisition Items
            mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(txtDate.Text, txtSearch.Text.Trim, Guid.Empty, 2,
                                                                                  CInt(cmbRequisition.SelectedValue), , , , ,
                                                                                  IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue),
                                                                                  CInt(Val(txtNo.Text)), , , True, ReqTypeID:=CInt(cmbType.SelectedValue),
                                                                                  ClientCode:=AppSettings("ClientCode"))
        Else
            mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForList(txtDate.Text, txtSearch.Text.Trim, Guid.Empty, 2, CInt(cmbRequisition.SelectedValue), , , , , IIf(cmbRequisitionText.SelectedIndex <= 0, "", cmbRequisitionText.SelectedValue), CInt(Val(txtNo.Text)), ReqTypeID:=CInt(cmbType.SelectedValue))
        End If
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
    End Sub
    Private Sub GetDetail(Optional ByVal PartNo As String = "", Optional ByVal ItemId As String = "{00000000-0000-0000-0000-000000000000}")
        mPendingFromListRequisition = PendingFromList.GetPendingFromList(PendingFromList.PendingListOf.Requisition, mOrder.CustomerID, New Guid(ItemId), mOrder.OrderDate.ToString)
        mPendingFromListSalesOrder = PendingFromList.GetPendingFromList(PendingFromList.PendingListOf.SalesOrder, mOrder.CustomerID, New Guid(ItemId), mOrder.OrderDate.ToString)
        mReOrderLevelItemList = ReOrderLevelItemList.GetReOrderLevelItemList(PartNo)

        Session("mPendingFromListSalesOrder") = mPendingFromListSalesOrder
        Session("mPendingFromListRequisition") = mPendingFromListRequisition
        Session("mReOrderLevelItemList") = mReOrderLevelItemList
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'Added By Vikrant On 07-Oct-2014 For ALL07102014
                    If MSGBoxCtrl.Sender = "SaveItemMaster" Then
                        Try
                            Session("Sender") = ""
                            Dim mItem As Item
                            Dim index As Integer = Session("Index")

                            mItem = Item.NewItem(mRequisitionItemsNew(index).PartNo, mRequisitionItemsNew(index).Description, mRequisitionItemsNew(index).IPCReference)
                            Session("mItem") = mItem

                            mItemId = mItem.ID
                            Session("mItemId") = mItemId
                            Session("mOrder") = mOrder
                            Session("PartInfo") = "True"
                            Dim URL As Stack = New Stack
                            URL.Push(Request.Url.ToString.Substring(Request.Url.ToString.LastIndexOf("/") + 1))
                            Session("URL") = URL
                            Session("RequisitionItemID") = mRequisitionItemsNew(index).ID
                            Response.Redirect("wfPartInformation_Ajax.aspx?BackPage=" & "wfRequisitionPartListForPurchaseOrder_Ajax.aspx")
                        Catch ex As Exception
                            Throw ex.GetBaseException
                        End Try
                    End If
                    'End
                    If MSGBoxCtrl.Sender = "Confirmation" Then
                        Try
                            Dim index As Integer = Session("Index")
                            setObject(mItemId, mRequisitionItemsNew(index).ID, mRequisitionItemsNew(index).OrderBalQty, mRequisitionItemsNew(index).RequisitionNo,
                              mRequisitionItemsNew(index).UnitID.ToString, mRequisitionItemsNew(index).Unit,
                              HSNACSCode:=Session("mHSNACSCode"), ReqDateFormatted:=mRequisitionItemsNew(index).ReqDateFormatted)
                            RemoveSession()
                            MarkLog(Util.Action.Save, "Requisition Part added", " Part No. " + mRequisitionItemsNew(index).PartNo + " Item ID " + mRequisitionItemsNew(index).ItemID.ToString, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            Session.Remove("mHSNACSCode")
                            Session.Remove("Index")
                            Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx")
                            MarkLog(Util.Action.Save, "Requisition Part list for Purchase order", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    'Added By Vikrant On 07-Oct-2014 For ALL07102014
                    If MSGBoxCtrl.Sender = "SaveItemMaster" Then
                        Session("sender") = ""
                    End If
                    If MSGBoxCtrl.Sender = "Confirmation" Then
                        Session("sender") = ""
                        Session.Remove("mHSNACSCode")
                        Session.Remove("Index")
                    End If
                    'End
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgPartStockStatusList.DataSource = mRequisitionItemsNew
        mDistinctTextListForRequisition = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)", CInt(cmbRequisition.SelectedValue))
        cmbRequisitionText.DataSource = mDistinctTextListForRequisition
        dgPartStockStatusList.DataBind()
    End Sub
    Private Sub DropDownListBind()
        mDistinctTextListForRequisition = DistinctTextListForRequisition.GetDistinctTextList("16", , True, "(All)", CInt(cmbRequisition.SelectedValue))
        cmbRequisitionText.DataSource = mDistinctTextListForRequisition
        cmbRequisitionText.DataBind()
    End Sub
    Public Sub PartPage(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        'dgPartStockStatusList.CurrentPageIndex = e.NewPageIndex
        '===============================
        mRequisitionItemsNew = CType(Session("mRequisitionItemsNew"), RequisitionItemsNew)
        '===============================
        dgPartStockStatusList.DataSource = mRequisitionItemsNew
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        dgPartStockStatusList.DataBind()
        lblResult.Text = "Requisition Part List :" & mRequisitionItemsNew.Count & " Record(s) Found."
    End Sub
    Public Sub Controlvisibility()
        If cmbRequisition.SelectedValue = "71" Then 'Stores Req
            cmbType.Enabled = False
            cmbType.ClearSelection()
        Else
            cmbType.Enabled = True
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        If Not IsPostBack Then
            If txtDate.Text = "" Then
                txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            End If
            If mOrder.IsNew Then
                txtDate.Enabled = True
                txtDate.Text = mOrder.OrderDateFormatted
                If mOrder.OrderItems.Count >= 2 Then
                    txtDate.Enabled = False
                End If
            Else
                txtDate.Enabled = False
                txtDate.Text = mOrder.OrderDateFormatted
            End If
            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If
            'txtSearch.Text = mOrder.OrderItems.CurrentItem.ItemName
            GetList(txtSearch.Text)
            DataFieldBind()
            DropDownListBind()
            Controlvisibility()
        Else
            dgPartStockStatusList.DataSource = mRequisitionItemsNew
            dgPartStockStatusList.DataBind()
        End If
        lblResult.Text = "Requisition Part List :" & mRequisitionItemsNew.Count & " Record(s) Found."
       
       
    End Sub
    Private Sub cmbRequisitionText_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRequisitionText.SelectedIndexChanged
        txtNo.Text = ""
        If cmbRequisitionText.SelectedIndex = 0 Then
            txtNo.Visible = False
        Else
            txtNo.Visible = True
        End If
        If cmbRequisitionText.Enabled = True Then
            cmbRequisitionText.Focus()
        End If
        btnFindNow_Click(sender:=sender, e:=e)
    End Sub
    Private Sub txtDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtDate.TextChanged, txtSearch.TextChanged, txtNo.TextChanged, cmbType.SelectedIndexChanged
        btnFindNow_Click(sender:=sender, e:=e)
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartStockStatusList.PageIndex = 0
        GetList(txtSearch.Text)
        DataFieldBind()
        lblResult.Text = "Requisition Part List :" & mRequisitionItemsNew.Count & " Record(s) Found."
    End Sub
    Private Sub dgPartStockStatusList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartStockStatusList.RowCommand
        Select Case e.CommandName
            Case "SelectPart"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartStockStatusList.PageIndex * dgPartStockStatusList.PageSize
                'Added By Vikrant On 07-Oct-2014 For ALL07102014
                Session("Index") = Index
                Dim ItemID As Guid = Guid.Empty
                Dim mHSNACSCode As String  'Added By Prashant on 28-Sep-2021 For STR27092021
                Dim mFetchItemByName As FetchItemByName = FetchItemByName.GetItemByName(mRequisitionItemsNew(Index).PartNo)
                If mFetchItemByName.Count > 0 Then
                    ItemID = mFetchItemByName(0).ID
                    mHSNACSCode = mFetchItemByName(0).HSNACSCode
                End If
                If ItemID.Equals(Guid.Empty) Then
                    MSGBoxCtrl.show("Alert", "Part not added in Part Master", "Do you want to add it in Part Master", MsgBoxStyle.YesNo, "SaveItemMaster")
                    Exit Sub
                Else 'End
                    mItemId = ItemID
                    Session("mItemId") = mItemId
                    'Session("AddRequisitionPart") = "True"
                    If mOrder.ExchangeOrderTypeID <> 2 Then  'Added by Shital on 23-Oct-2019
                        If mOrder.OrderItems.Contains(ItemID:=mItemId) Then
                            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "Order Item", MsgBoxStyle.OkOnly, "")
                            mOrder.CancelEdit()
                            Exit Sub
                        End If
                    End If

                    If mOrder.ExchangeOrderTypeID = 2 Then
                        'Do nothing
                    Else
                        If mRequisitionItemsNew(Index).orderItemReceiptBalanceQuantity > 0.0 Then
							MSGBoxCtrl.Show(MSGBox.Message_Title.Alert, MSGBox.Message_Text.Alert, "An Order already exists for this Part or its Alternate Part. Do you still want to create another Order ?", MsgBoxStyle.YesNo, "Confirmation")
							Exit Sub
                        End If
                    End If
                    setObject(mItemId, mRequisitionItemsNew(Index).ID, mRequisitionItemsNew(Index).OrderBalQty, mRequisitionItemsNew(Index).RequisitionNo,
                              mRequisitionItemsNew(Index).UnitID.ToString, mRequisitionItemsNew(Index).Unit,
                              HSNACSCode:=mHSNACSCode, ReqDateFormatted:=mRequisitionItemsNew(Index).ReqDateFormatted)
                    RemoveSession()
                    'Added by Shital on 18-Oct-2019
                    If mOrder.ExchangeOrderTypeID = 2 Then
                        Response.Redirect("wfOrderItem_Ajax.aspx?BackPage=wfPurchaseOrder_Ajax.aspx")
                    Else
                        MarkLog(Util.Action.Save, "Requisition Part added", " Part No. " + mRequisitionItemsNew(Index).PartNo + " Item ID " + mRequisitionItemsNew(Index).ItemID.ToString, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Response.Redirect("wfPurchaseOrder_Ajax.aspx?BackPage=index.aspx")
                    End If
                End If
        End Select
    End Sub
    Private Sub dgPartStockStatusList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartStockStatusList.Sorting
        mRequisitionItemsNew.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        dgPartStockStatusList.DataSource = mRequisitionItemsNew
        dgPartStockStatusList.DataBind()
    End Sub
    Private Sub dgPartStockStatusList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartStockStatusList.PageIndexChanging
        dgPartStockStatusList.PageIndex = e.NewPageIndex
        dgPartStockStatusList.DataSource = mRequisitionItemsNew
        Session("mRequisitionItemsNew") = mRequisitionItemsNew
        dgPartStockStatusList.DataBind()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mOrder.OrderItems.CurrentItem.IsNew And Not Session("Edit") = True Then mOrder.OrderItems.Remove(mOrder.OrderItems.CurrentItem)
        RemoveSession()
        Session.Remove("Edit")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub cmbRequisition_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbRequisition.SelectedIndexChanged
        DropDownListBind()
        Controlvisibility()
        btnFindNow_Click(sender:=sender, e:=e)
    End Sub
#End Region

    
End Class