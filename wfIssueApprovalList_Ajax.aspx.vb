'AJAX Conversion By Vikrant On 06-Nov-2014

Public Class wfIssueApprovalList_Ajax
    Inherits System.Web.UI.Page

#Region " Variables "
    Public mPendingIssueApprovalItemList As PendingIssueApprovalItemList
    Public mPendingToIssueList As PendingToIssueList
    Dim mTransTypeID As Trans
    Dim mStoreID As Guid
    Dim mItemId As Guid = Guid.Empty
    Public mIssue As Issue
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mPendingIssueApprovalItemList = CType(Session("mPendingIssueApprovalItemList"), PendingIssueApprovalItemList)
        mPendingToIssueList = CType(Session("mPendingToIssueList"), PendingToIssueList)
        'mItemId = Session("mItemId")
        mIssue = Session("mIssue")
        ' mTransDate = Session("TransDate")
        ' mIssueItemID = Session("IssueItem")
    End Sub
    Private Sub SetSession()
        Session("mPendingIssueApprovalItemList") = mPendingIssueApprovalItemList
        Session("mPendingToIssueList") = mPendingToIssueList
        Session("mIssue") = mIssue
    End Sub
    Private Sub FindNow()
        mPendingIssueApprovalItemList = PendingIssueApprovalItemList.GetPendingIssueApprovalItemList(mIssue.IDate.ToString, txtSearch.Text, mIssue.MachineID, mIssue.StoreID)
        dgRequisitionItemList.DataSource = mPendingIssueApprovalItemList
        Session("mPendingIssueApprovalItemList") = mPendingIssueApprovalItemList
        DataBind()
    End Sub
    Private Sub setObject(ByVal Index As Int32)
        'mInvoice.InvoiceItems.CurrentItem.ItemID = mItemId'AvailableItemQty
        mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingToIssueList(Index).ReceiptItemID
        'mIssue.IssueItems.CurrentItem.Qty = mPendingToIssueList(Index).AvailableQuantity 'Commented By Prashant 3-Jun-2010
        mIssue.IssueItems.CurrentItem.DisplayQty = mPendingToIssueList(Index).AvailableQuantity  'Added By Prashant 3-Jun-2010
        mIssue.IssueItems.CurrentItem.DisplayUnitID = mPendingToIssueList(Index).UnitID     'Added By Saylee 21-July-2010
        Session("AvailableQuantity") = mPendingToIssueList(Index).AvailableQuantity
        Session("mIssue") = mIssue
    End Sub
#End Region

#Region " DatafieldBind "
    Private Sub DataFieldBind()
        GetSession()
        dgRequisitionItemList.DataSource = mPendingIssueApprovalItemList
        dgStockList.DataSource = mPendingToIssueList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("Sender") = "" Then
            If txtSearch.Enabled = True Then
                txtSearch.Focus()
            End If
            ''txtSearch.Text = PartNo
            mPendingIssueApprovalItemList = PendingIssueApprovalItemList.GetPendingIssueApprovalItemList(mIssue.IDate.ToString, txtSearch.Text, mIssue.MachineID, mIssue.StoreID, mIssue.WOID.ToString)

            'mPendingToIssueList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mIssue.IssueItems.CurrentItem.ItemName, , , , , mIssue.IDate, mIssue.TransTypeID, Guid.Empty.ToString) 'mIssue.IssueItems.CurrentItem.ItemID.ToString)
            mPendingToIssueList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, Guid.Empty.ToString, mIssue.IDate.ToString, _
                                                                           CType(mIssue.TransTypeID, Flypal.Util.Trans), ToTypeIDOfIssue:=mIssue.ToTypeID)
            Session("mPendingIssueApprovalItemList") = mPendingIssueApprovalItemList
            Session("mPendingToIssueList") = mPendingToIssueList
            DataFieldBind()
            lblResult.Text = "Issue Approved Requisition Part List : " & mPendingIssueApprovalItemList.Count & " Record(s) found."
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        mPendingIssueApprovalItemList = PendingIssueApprovalItemList.GetPendingIssueApprovalItemList(mIssue.IDate.ToString, txtSearch.Text, mIssue.MachineID, mIssue.StoreID, mIssue.WOID.ToString)
        ' mPendingToIssueList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mPendingIssueApprovalItemList.Item(dgRequisitionItemList.SelectedIndex).ItemName, , , , , mIssue.IDate, mIssue.TransTypeID, mPendingIssueApprovalItemList.Item(dgRequisitionItemList.SelectedIndex).ItemID.ToString)
        Session("mPendingIssueApprovalItemList") = mPendingIssueApprovalItemList
        ''Session("mPendingToIssueList") = mPendingToIssueList
        DataFieldBind()
        lblResult.Text = "Issue Approved Requisition Part List : " & mPendingIssueApprovalItemList.Count & " Record(s) found."
        upnlReqGrid.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("mIssue") = mIssue
        Response.Redirect("wfIssueItem_Ajax.aspx?BackPage=wfIssue_Ajax.aspx")
    End Sub
    Private Sub dgRequisitionItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgRequisitionItemList.PageIndexChanging
        dgRequisitionItemList.PageIndex = e.NewPageIndex
        dgRequisitionItemList.DataSource = mPendingIssueApprovalItemList
        Session("mPendingIssueApprovalItemList") = mPendingIssueApprovalItemList
        dgRequisitionItemList.DataBind()
    End Sub
    Private Sub dgRequisitionItemList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRequisitionItemList.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim Index As Integer = CInt(e.CommandArgument) + dgRequisitionItemList.PageIndex * dgRequisitionItemList.PageSize
                'lblResult1.Visible = True
                'dgStockList.Visible = True
                'mPendingIssueApprovalItemList = PendingIssueApprovalItemList.GetPendingIssueApprovalItemList(mIssue.IDate, txtSearch.Text)
                'Session("mPendingIssueApprovalItemList") = mPendingIssueApprovalItemList
                'mPendingToIssueList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mPendingIssueApprovalItemList.Item(dgRequisitionItemList.SelectedIndex).ItemName, , , , , mIssue.IDate, mIssue.TransTypeID, mPendingIssueApprovalItemList.Item(dgRequisitionItemList.SelectedIndex).ItemID.ToString)
                mPendingToIssueList = PendingToIssueList.GetPendingToIssueList(mIssue.StoreID, mPendingIssueApprovalItemList.Item(Index).LinkID.ToString, _
                                                                               mIssue.IDate.ToString, CType(mIssue.TransTypeID, Flypal.Util.Trans), _
                                                                               ToTypeIDOfIssue:=mIssue.ToTypeID)
                Session("mPendingToIssueList") = mPendingToIssueList
                DataFieldBind()
                'dgStockList.DataSource = mPendingToIssueList
                'dgStockList.DataBind()
                'lblResult.Text = "Issue Approved Requisition Part List : " & mPendingIssueApprovalItemList.Count & " Record(s) found."
                lblResult1.Text = "Part Stock List : " & mPendingToIssueList.Count & " Record(s) found"
                upnlPartStockGrid.Update()
        End Select
    End Sub
    Private Sub dgStockList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgStockList.PageIndexChanging
        dgStockList.PageIndex = e.NewPageIndex
        dgStockList.DataSource = mPendingToIssueList
        Session("mPendingToIssueList") = mPendingToIssueList
        dgStockList.DataBind()
    End Sub
    Private Sub dgStockList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgStockList.RowCommand
        Select Case e.CommandName
            Case "Select"
                Dim Index As Integer = CInt(e.CommandArgument) + dgStockList.PageIndex * dgStockList.PageSize
                setObject(Index)
                Session("CheckQty") = "True"
                Session("AddRequisitionParts") = "True"
                Session.Remove("mPendingIssueApprovalItemList")
                Session.Remove("mPendingToIssueList")
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        End Select
    End Sub
    'Added By Prashant 18-June-2009 for sorting
    Private Sub dgRequisitionItemList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRequisitionItemList.Sorting
        mPendingIssueApprovalItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingIssueApprovalItemList") = mPendingIssueApprovalItemList
        dgRequisitionItemList.DataSource = mPendingIssueApprovalItemList
        dgRequisitionItemList.DataBind()
    End Sub
    Private Sub dgStockList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgStockList.Sorting
        mPendingToIssueList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingToIssueList") = mPendingToIssueList
        dgStockList.DataSource = mPendingToIssueList
        dgStockList.DataBind()
    End Sub
    '------------------------------------------

#End Region

End Class