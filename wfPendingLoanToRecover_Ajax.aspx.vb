Public Class wfPendingLoanToRecover_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPendingLoanToRecover As PendingLoanToRecover
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public mUserHasNoStoreRights As UserHasNoStoreRights
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mReceiptCumInvoice = Session("mReceiptCumInvoice")
        mPendingLoanToRecover = Session("mPendingLoanToRecover")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject(ByVal Index As Int32)
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.LoanIssueItemID = mPendingLoanToRecover(Index).IssueItemID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StoreID = mPendingLoanToRecover(Index).FromStoreID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID = mPendingLoanToRecover(Index).ItemTagID
        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName = mPendingLoanToRecover(Index).ItemTagName
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPendingLoanToRecover = PendingLoanToRecover.GetPendingLoanToRecover(mReceiptCumInvoice.StoreID, "", mReceiptCumInvoice.RecCumInvDate.ToString)
        Session("mPendingLoanToRecover") = mPendingLoanToRecover
        dgPendingList.DataSource = mPendingLoanToRecover
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            DataFieldBind()
        End If
        lblResult.Text = "Pending loan to receive from Store  : " & mPendingLoanToRecover.Count & " Record(s) found."
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPendingList.PageIndex = 0
        mPendingLoanToRecover = PendingLoanToRecover.GetPendingLoanToRecover(mReceiptCumInvoice.StoreID, txtName.Text.Trim, mReceiptCumInvoice.RecCumInvDate.ToString)
        Session("mPendingLoanToRecover") = mPendingLoanToRecover
        dgPendingList.DataSource = mPendingLoanToRecover
        dgPendingList.DataBind()
        lblResult.Text = "Pending loan to receive from Store : " & mPendingLoanToRecover.Count & " Record(s) found."
        upnlPendingList.Update()
    End Sub
    Private Sub dgPendingList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingList.RowCommand
        Dim mPrevTransID As Guid = Guid.Empty
         Select Case e.CommandName
            Case "SelectRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPendingList.PageIndex * dgPendingList.PageSize
                '--------------------------------------------
                mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mPendingLoanToRecover(Index).FromStoreID.ToString) ''Added By Prashant 13-May-2020
                If mUserHasNoStoreRights.Count > 0 Then
                    MSGBoxCtrl.show("Alert!", "Sorry you do not have rights for this store " + mPendingLoanToRecover(Index).FromStoreName + " Please contact with admin.", "", MsgBoxStyle.OkOnly, "ResetStore")
                    dgPendingList.DataSource = mPendingLoanToRecover
                    dgPendingList.DataBind()
                    upnlPendingList.Update()
                    Exit Sub
                End If
                '-------------------------------------------- ''End of Added By Prashant 13-May-2020
                SetObject(Index)
                If (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 0) Or (mReceiptCumInvoice.ReceiptCumInvoiceItems.Count = 1 And mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsNew) Then
                    Session("mPrevTransID") = Guid.Empty
                Else
                    Session("mPrevTransID") = mReceiptCumInvoice.ReceiptCumInvoiceItems.Item(mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 2).IssueItemDetailForReceipt.IssueID
                End If
                mReceiptCumInvoice.StoreID = mPendingLoanToRecover(Index).ToStoreID
                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                Session("mFromToTypeID") = CInt(IIf(mReceiptCumInvoice.FromTypeID = 14, 1, mReceiptCumInvoice.FromTypeID)) '8  'Store
                Session("mPrimaryOrderType") = 4 'TransListOf.Order_LoanRecovery
                Session("mTransaction") = 4 'Transaction.Issue
                Session("mFromPartList") = False
                Session("OpenFrom") = 2
                'Session("ItemID") = mPendingLoanToRecover(Index).ItemID 'Added By Vikrant On 20-Feb-2013 For  All20022013-1
                Session("ItemID") = mPendingLoanToRecover(Index).LinkID '--Above line commented and this line Added By Prashant On 24-Feb-2014 
                dgPendingList.DataSource = mPendingLoanToRecover
                dgPendingList.DataBind()
                upnlPendingList.Update()
                Response.Redirect("wfReceiptPendingOrderList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfPendingLoanToRecover_Ajax.aspx&mType=2")
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Request.QueryString("BackPage") = "wfReceiptCumInvoice_Ajax.aspx" Then 'ReceiptcumInvoice
            mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
            Session("mReceiptCumInvoice") = mReceiptCumInvoice
        End If
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub dgPendingList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingList.PageIndexChanging
        dgPendingList.PageIndex = e.NewPageIndex
        dgPendingList.DataSource = mPendingLoanToRecover
        dgPendingList.DataBind()
        upnlPendingList.Update()
    End Sub
    Private Sub dgPendingList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingList.Sorting
        mPendingLoanToRecover.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingLoanToRecover") = mPendingLoanToRecover
        dgPendingList.DataSource = mPendingLoanToRecover
        dgPendingList.DataBind()
        upnlPendingList.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
    End Sub
#End Region

End Class