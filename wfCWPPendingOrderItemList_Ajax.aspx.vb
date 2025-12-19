Public Class wfCWPPendingOrderItemList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPendingOrderItemListForCwp As PendingOrderItemListForCwp
    Private mCWP As CWP
    Dim mRequisitionItemsNew As RequisitionItemsNew 'Added By Vikrant On 20-Dec-2016 For ALL20122016
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCWP = Session("mCWP")
        mPendingOrderItemListForCwp = Session("mPendingOrderItemListForCwp")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPendingOrderItemListForCwp")
    End Sub
    'Private Sub ClearAll()
    '    If Session("MiddleFrame") <> "wfCWPPendingOrderItemList_Ajax.aspx?" Then
    '        RemoveSession()
    '    End If
    'End Sub
    Private Sub setObjectForComp(ByVal RequisitionItemNew As RequisitionItemNew)
        mCWP.CWPComps.CurrentItem.SrNo = mCWP.CWPComps.CurrentIndex + 1
        mCWP.CWPComps.CurrentItem.PartID = RequisitionItemNew.ItemID
        mCWP.CWPComps.CurrentItem.PartNo = RequisitionItemNew.PartNo
        mCWP.CWPComps.CurrentItem.Description = RequisitionItemNew.Description
        mCWP.CWPComps.CurrentItem.Qty = RequisitionItemNew.RequestedQty
        'mCWP.ApplyEdit()
    End Sub
    Public Sub setObject(ByVal mPendingOrderItemListForCwp As PendingOrderItemListForCwp, ByVal Index As Integer)
        mCWP.CWPDate = txtDate.Text
        mCWP.OrderItemID = mPendingOrderItemListForCwp(Index:=Index).OrderItemID
        mCWP.MachineID = mPendingOrderItemListForCwp(Index:=Index).MachineID
        mCWP.RegNo = mPendingOrderItemListForCwp(Index:=Index).RegNo
        mCWP.TechDirectionID = mPendingOrderItemListForCwp(Index:=Index).TechDirectionID
        mCWP.Position = mPendingOrderItemListForCwp(Index:=Index).Position
        mCWP.RemovalReason = mPendingOrderItemListForCwp(Index:=Index).RemovalReason
        mCWP.CompRemDate = mPendingOrderItemListForCwp(Index:=Index).RemovedOn
        mCWP.ShopWONo = mPendingOrderItemListForCwp(Index:=Index).OrderNo
        mCWP.ShopWODate = mPendingOrderItemListForCwp(Index:=Index).OrderDateFormatted.ToString
        mCWP.NHASerialNo = mPendingOrderItemListForCwp(Index:=Index).AirFrameSerialNo
        mCWP.PartNo = mPendingOrderItemListForCwp(Index:=Index).ItemName
        mCWP.PartDescription = mPendingOrderItemListForCwp(Index:=Index).ItemDescription
        mCWP.SerialNo = mPendingOrderItemListForCwp(Index:=Index).ReceiptItemSerialNo
        mCWP.TSOCSOLSO = mPendingOrderItemListForCwp(Index:=Index).TSOCSOLSO
        Dim LastVisitNo As Integer = mPendingOrderItemListForCwp.LastVisitNo(txtDate.Text, mPendingOrderItemListForCwp(Index:=Index).ItemName, mPendingOrderItemListForCwp(Index:=Index).ReceiptItemSerialNo)
        If LastVisitNo > 0 Then
            mCWP.VisitNo = LastVisitNo + 1
        End If
        'Added By Vikrant On 20-Dec-2016 For ALL20122016
        mRequisitionItemsNew = RequisitionItemsNew.GetRequisitionItemsForCWPComponents(mPendingOrderItemListForCwp(Index:=Index).OrderID, True)
        For Each mRequisitionItemNew As RequisitionItemNew In mRequisitionItemsNew
            If Not mCWP.CWPComps.Contains(mRequisitionItemNew.PartNo) Then
                mCWP.CWPComps.Add(mCWP.ID)
                setObjectForComp(mRequisitionItemNew)
                mCWP.WorkShopID = mRequisitionItemNew.WorkShopID
            End If
        Next
        'End
        Session("mCWP") = mCWP
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPendingOrderItemListForCwp = PendingOrderItemListForCwp.GetPendingOrderItemListForCwp(ItemName:=txtName.Text, Text:="", No:=0, Amend:="", FromDate:="1/1/1900", ToDate:="1/1/3300")
        dgPendingCWPOrderItemList.DataSource = mPendingOrderItemListForCwp
        Session("mPendingOrderItemListForCwp") = mPendingOrderItemListForCwp
        lblResult.Text = "Pending Order Item List : " + CStr(mPendingOrderItemListForCwp.Count) + " Record(s) Found"
        DataBind()
    End Sub
    Private Sub dgPendingCWPOrderItemList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingCWPOrderItemList.PageIndexChanging
        dgPendingCWPOrderItemList.PageIndex = e.NewPageIndex
        mPendingOrderItemListForCwp = Session("mPendingOrderItemListForCwp")
        dgPendingCWPOrderItemList.DataSource = mPendingOrderItemListForCwp
        Session("mPendingOrderItemListForCwp") = mPendingOrderItemListForCwp
        dgPendingCWPOrderItemList.DataBind()
    End Sub
    Private Sub GridBind()
        dgPendingCWPOrderItemList.DataSource = mPendingOrderItemListForCwp
        dgPendingCWPOrderItemList.DataBind()
        Session("mPendingOrderItemListForCwp") = mPendingOrderItemListForCwp
        upnlPendingCWPOrderItemList.Update()
    End Sub
#End Region

#Region " Event "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ClearAll()
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            If txtDate.Text = "" Then
                txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            End If
            If txtName.Enabled = True Then
                txtName.Focus()
            End If
            DataFieldBind()
        End If
    End Sub
    Private Sub txtDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtDate.TextChanged
        dgPendingCWPOrderItemList.PageIndex = 0
        mPendingOrderItemListForCwp = PendingOrderItemListForCwp.GetPendingOrderItemListForCwp(ItemName:=txtName.Text, Text:="", No:=0, Amend:="", FromDate:="1/1/1900", ToDate:=txtDate.Text)
        lblResult.Text = "Pending Order Item List : " + CStr(mPendingOrderItemListForCwp.Count) + " Record(s) Found"
        GridBind()
    End Sub
    Private Sub txtName_TextChanged(sender As Object, e As System.EventArgs) Handles txtName.TextChanged
        txtName.Focus()
        dgPendingCWPOrderItemList.PageIndex = 0
        mPendingOrderItemListForCwp = PendingOrderItemListForCwp.GetPendingOrderItemListForCwp(ItemName:=txtName.Text, Text:="", No:=0, Amend:="", FromDate:="1/1/1900", ToDate:=txtDate.Text)
        lblResult.Text = "Pending Order Item List : " + CStr(mPendingOrderItemListForCwp.Count) + " Record(s) Found"
        GridBind()
    End Sub
    Private Sub dgPendingCWPOrderItemList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingCWPOrderItemList.RowCommand
        Select Case e.CommandName
            Case "SelectPart"
                GridBind()
                Dim Index As Integer = CInt(e.CommandArgument) + dgPendingCWPOrderItemList.PageIndex * dgPendingCWPOrderItemList.PageSize
                mPendingOrderItemListForCwp = Session("mPendingOrderItemListForCwp")
                setObject(mPendingOrderItemListForCwp, Index)
                Response.Redirect("wfCWP_Ajax.aspx")
        End Select
    End Sub
    Private Sub btnTopClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTopClose.Click, btnBottomClose.Click
        Response.Redirect("index.aspx")
    End Sub
    Private Sub dgPendingList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingCWPOrderItemList.Sorting
        mPendingOrderItemListForCwp = Session("mPendingOrderItemListForCwp")
        mPendingOrderItemListForCwp.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
       GridBind()
    End Sub
#End Region

End Class