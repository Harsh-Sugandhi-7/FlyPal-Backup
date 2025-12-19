'AJAX Conversion By Vikrant On 04-Nov-2014

Public Class wfPendingToReturnForExchangeRepair_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mFromStoreID As Guid
    Public mVendorID As Guid
    Public mTransTypeID As Trans
    Public mIssueDate As String = ""
    Public mIssue As Issue
    Dim mItemId As Guid = Guid.Empty
    Public mPendingToReturnForExchangeRepairList As PendingToReturnForExchangeRepairList
    Private mPendingAgainst As PendingToReturnForExchangeRepairList.PendingAgainst
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssue = Session("mIssue")
        mTransTypeID = Session("mTransTypeID")
        mPendingAgainst = CType(Session("mPendingAgainst"), PendingToReturnForExchangeRepairList.PendingAgainst)
    End Sub
    Public Sub setObject(ByVal Index As Integer)
        '23-03-2007*** ---------------------------------------------------------------------------------
        'If Issue is doing for 'ISSUE TO Vendor FOR EXCHANGE REPAIR'then...
        'Then Select First against what RECEIPT/ORDER FOR EXCHANGE REPAIR?, we issing Part
        mIssue = Session("mIssue")
        mPendingToReturnForExchangeRepairList = Session("mPendingToReturnForExchangeRepairList")
        mPendingAgainst = Session("mPendingAgainst")

        mIssue.VendorID = mPendingToReturnForExchangeRepairList.Item(Index).VendorID
        'mIssue.StoreID = mPendingToReturnForExchangeRepairList.Item(Index).FromStoreID

        If (mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Receipt) And (Not mPendingToReturnForExchangeRepairList.Item(Index).ReceiptItemID.Equals(Guid.Empty)) Then
            'Assinging LoanReceiptItemID against of we are returning Item
            mIssue.StoreID = mPendingToReturnForExchangeRepairList.Item(Index).FromStoreID
            mIssue.IssueItems.CurrentItem.LoanReceiptItemID = mPendingToReturnForExchangeRepairList.Item(Index).ReceiptItemID
        ElseIf (mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Receipt) And (mPendingToReturnForExchangeRepairList.Item(Index).ReceiptItemID.Equals(Guid.Empty)) Then
            mIssue.StoreID = mPendingToReturnForExchangeRepairList.Item(Index).FromStoreID
            Exit Sub
        ElseIf (mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Order) Then
            'If Not mPendingToReturnForExchangeRepairList.Item(Index).ReceiptItemID.Equals(Guid.Empty) Then
            '    mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingToReturnForExchangeRepairList.Item(Index).ReceiptItemID
            '    mIssue.IssueItems.CurrentItem.Qty = mPendingToReturnForExchangeRepairList.Item(Index).LoanQty
            'Else
            '    mIssue.IssueItems.CurrentItem.ItemID = mPendingToReturnForExchangeRepairList(Index).ItemID
            '    SessionPartNo = mPendingToReturnForExchangeRepairList(Index).ItemID
            '    Description = mPendingToReturnForExchangeRepairList(Index).ItemID
            'End If
            'mIssue.IssueItems.CurrentItem.ReceiptItemID = mPendingToReturnForExchangeRepairList.Item(Index).ReceiptItemID
            'mIssue.IssueItems.CurrentItem.Qty = mPendingToReturnForExchangeRepairList.Item(Index).LoanQty
            Dim mPendingToReturnForExchangeRepairInfo As PendingToReturnForExchangeRepairList.PendingToReturnForExchangeRepairInfo
            mPendingToReturnForExchangeRepairInfo = mPendingToReturnForExchangeRepairList.Item(Index)
            Session("mPendingToReturnForExchangeRepairInfo") = mPendingToReturnForExchangeRepairInfo
        End If
        Session("mIssue") = mIssue
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
    End Sub
    ''Added by Vikrant on 12-July-2012 For ALL12072012
    'Private Sub ControlVisibility()
    '    If mTransTypeID = 16 Then
    '        dgPendingList.Columns(7).Visible = True
    '    End If
    'End Sub
    ''End
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        txtDate.Text = CDate(mIssue.IDate).ToString(AppSettings("Dateformat"))
        mTransTypeID = Session("mTransTypeID")
        mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Order
        Session("mPendingAgainst") = mPendingAgainst
        mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(mIssue.StoreID, mIssue.VendorID, _
                                                                                                                             mIssue.IDate.ToString, "", _
                                                                                                                             mIssue.TransTypeID, _
                                                                                                                             mPendingAgainst, _
                                                                                                                             SerialNo:=txtSerialNo.Text.Trim)
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        If mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Order Then
            lblResult.Text = "Pending Order details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
        Else
            lblResult.Text = "Pending to loan return details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
        End If
        DataBind()
    End Sub
    Private Sub dgPendingList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingList.PageIndexChanging
        dgPendingList.PageIndex = e.NewPageIndex
        mPendingToReturnForExchangeRepairList = Session("mPendingToReturnForExchangeRepairList")
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        'ControlVisibility()
        dgPendingList.DataBind()
    End Sub
    Private Sub GridBind()
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        dgPendingList.DataBind()
    End Sub
#End Region

#Region " Event "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()

        If Not IsPostBack And Session("sender") = "" Then
            If txtDate.Text = "" Then
                txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            End If
            If txtName.Enabled = True Then
                txtName.Focus()
            End If
            DataFieldBind()
            If mIssue.IssueItems.Count - 1 = 0 Then
                txtDate.Enabled = True
            Else
                txtDate.Enabled = False
            End If
            'ControlVisibility() 'Added by Vikrant on 12-July-2012 For ALL12072012
        End If
    End Sub
    Private Sub rdbOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbOrders.CheckedChanged
        If mIssue.IsNew Then
            mIssue.IDate = CDate(txtDate.Text)
        End If
        txtName.Text = ""
        If rdbOrders.Checked = True Then
            dgPendingList.Columns(5).HeaderText = "ERO Qty."
            'dgPendingList.Columns(5).Visible = True
            dgPendingList.Columns(4).Visible = False
        Else
            dgPendingList.Columns(5).HeaderText = "Loan to Return Qty."
            dgPendingList.Columns(4).Visible = True
            'dgPendingList.Columns(5).Visible = False
        End If
        mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Order
        Session("mPendingAgainst") = mPendingAgainst
        'DataFieldBind()
        mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(mIssue.StoreID, mIssue.VendorID, _
                                                                                                                             mIssue.IDate.ToString, "", _
                                                                                                                             mIssue.TransTypeID, _
                                                                                                                             mPendingAgainst, _
                                                                                                                             SerialNo:=txtSerialNo.Text.Trim)
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        lblResult.Text = "Pending Order details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
        dgPendingList.DataBind()
    End Sub
    Private Sub rdbRaceipts_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbRaceipts.CheckedChanged
        If mIssue.IsNew Then
            mIssue.IDate = CDate(txtDate.Text)
        End If
        txtName.Text = ""
        If rdbRaceipts.Checked = True Then
            dgPendingList.Columns(6).HeaderText = "Loan to Return Qty."
            dgPendingList.Columns(4).Visible = True
            dgPendingList.Columns(5).Visible = False
        Else
            dgPendingList.Columns(6).HeaderText = "ERO Qty."
            dgPendingList.Columns(5).Visible = True
            dgPendingList.Columns(4).Visible = False
        End If
        mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Receipt
        Session("mPendingAgainst") = mPendingAgainst

        mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(mIssue.StoreID, mIssue.VendorID, _
                                                                                                                             mIssue.IDate.ToString, "", _
                                                                                                                             mIssue.TransTypeID, _
                                                                                                                             mPendingAgainst, _
                                                                                                                             SerialNo:=txtSerialNo.Text.Trim)
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        lblResult.Text = "Pending to loan return details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
        dgPendingList.DataBind()
    End Sub
    Private Sub dgPendingList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingList.RowCommand

        Select Case e.CommandName
            Case "SelectPart"
                GridBind()
                Dim Index As Integer = CInt(e.CommandArgument) + dgPendingList.PageIndex * dgPendingList.PageSize
                setObject(Index)
                If (mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Order) Then
                    'Response.Redirect("wfIssueItem.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfPendingToReturnForExchangeRepair.aspx")
                    Response.Redirect("wfIssueStockItemList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfPendingToReturnForExchangeRepair_Ajax.aspx")
                Else
                    Response.Redirect("wfPendingToIssueItemList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfIssueItem_Ajax.aspx")
                End If
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click, txtDate.TextChanged
        If mIssue.IsNew Then
            mIssue.IDate = CDate(txtDate.Text)
        End If
        dgPendingList.PageIndex = 0
        mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(mIssue.StoreID, mIssue.VendorID, _
                                                                                                                             mIssue.IDate.ToString, _
                                                                                                                             txtName.Text.Trim, _
                                                                                                                             mIssue.TransTypeID, _
                                                                                                                             mPendingAgainst, _
                                                                                                                             SerialNo:=txtSerialNo.Text.Trim)
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        'ControlVisibility()
        dgPendingList.DataBind()
        If mPendingAgainst = PendingToReturnForExchangeRepairList.PendingAgainst.Order Then
            lblResult.Text = "Pending Order details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
        Else
            lblResult.Text = "Pending to loan return details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'If Session("Edit") = True And Request.QueryString("BackPage") = "wfIssue.aspx" Then
        If Request.QueryString("BackPage") = "wfIssue_Ajax.aspx" Then
            mIssue.IssueItems.RemoveAt(mIssue.IssueItems.CurrentIndex)
            Session("Edit") = False
            Response.Redirect(Request.QueryString("BackPage"))
        Else
            Response.Redirect("Index.aspx")
        End If
        ' Response.Redirect("Index.aspx")
    End Sub
    Private Sub dgPendingList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingList.Sorting
        mPendingToReturnForExchangeRepairList = Session("mPendingToReturnForExchangeRepairList")
        mPendingToReturnForExchangeRepairList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        'ControlVisibility()
        dgPendingList.DataBind()
    End Sub
#End Region

End Class