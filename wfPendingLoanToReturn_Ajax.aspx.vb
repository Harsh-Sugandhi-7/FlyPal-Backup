'AJAX Conversion By Vikrant On 04-Nov-2014

Public Class wfPendingLoanToReturn_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPendingLoanToReturnList As PendingLoanToReturnList
    Public mIssue As Issue
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mIssue = Session("mIssue")
        mPendingLoanToReturnList = Session("mPendingLoanToReturnList")
    End Sub
    Private Sub SetSession()
        Session("mIssue") = mIssue
        Session("mPendingLoanToReturnList") = mPendingLoanToReturnList
    End Sub
    Private Sub SetObject(ByVal Index As Int32)
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
        mIssue.IssueTo = mPendingLoanToReturnList.Item(Index).FromStoreName
        If mIssue.IsNew Then
            mIssue.IDate = CDate(txtDate.Text)
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
    End Sub
    Private Sub ControlVisibility()
        If mIssue.TransTypeID = 49 Then
            lblTitle.Text = "Loan pending To Return To Supplier"
            dgPendingList.Columns(17).Visible = False
            dgPendingList.Columns(18).Visible = False
        ElseIf mIssue.TransTypeID = 51 Then
            lblTitle.Text = "Loan pending To Return To Customer"
            dgPendingList.Columns(17).Visible = False
            dgPendingList.Columns(18).Visible = False
        ElseIf mIssue.TransTypeID = 58 Then
            lblTitle.Text = "Repaired pending To Return To Customer"
            dgPendingList.Columns(17).Visible = False
            dgPendingList.Columns(18).Visible = False
        ElseIf mIssue.TransTypeID = 55 Then
            lblTitle.Text = "Pending Rental/Lease of Supplier"
            dgPendingList.Columns(8).Visible = False
            dgPendingList.Columns(9).Visible = False
        Else
            lblTitle.Text = "Loan pending To Return To Store"
            dgPendingList.Columns(17).Visible = False
            dgPendingList.Columns(18).Visible = False
        End If
        If mIssue.IssueItems.Count - 1 = 0 Then
            txtDate.Enabled = True
        Else
            txtDate.Enabled = False
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        txtDate.Text = CDate(mIssue.IDate).ToString(AppSettings("DateFormat"))
        mPendingLoanToReturnList = PendingLoanToReturnList.GetPendingLoanToReturnList(mIssue.StoreID, mIssue.ToStoreID, mIssue.VendorID, mIssue.IDate.ToString, "", mIssue.TransTypeID)
        Session("mPendingLoanToReturnList") = mPendingLoanToReturnList
        dgPendingList.DataSource = mPendingLoanToReturnList
        DataBind()
    End Sub
    Private Sub GridBind()
        dgPendingList.DataSource = mPendingLoanToReturnList
        dgPendingList.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If txtDate.Text = "" Then
                txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
            End If
            If txtName.Enabled = True Then
                txtName.Focus()
            End If
            txtName.Text = Request.QueryString("Name")
            DataFieldBind()
            lblResult.Text = "Pending To Loan Return Details : " & mPendingLoanToReturnList.Count & " Record(s) found."
            ControlVisibility()
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If mIssue.IsNew Then
            mIssue.IDate = CDate(txtDate.Text)
        End If
        mPendingLoanToReturnList = PendingLoanToReturnList.GetPendingLoanToReturnList(mIssue.StoreID, mIssue.ToStoreID, mIssue.VendorID, mIssue.IDate.ToString, txtName.Text.Trim, mIssue.TransTypeID)
        Session("mPendingLoanToReturnList") = mPendingLoanToReturnList
        dgPendingList.DataSource = mPendingLoanToReturnList
        dgPendingList.DataBind()
        lblResult.Text = "Pending To Loan Return Details : " & mPendingLoanToReturnList.Count & " Record(s) found."
        ControlVisibility()
    End Sub
    Private Sub dgPendingList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingList.PageIndexChanging
        dgPendingList.PageIndex = e.NewPageIndex
        dgPendingList.DataSource = mPendingLoanToReturnList
        Session("mPendingLoanToReturnList") = mPendingLoanToReturnList
        dgPendingList.DataBind()
        ControlVisibility()
    End Sub
    Private Sub dgPendingList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingList.RowCommand
        Select Case e.CommandName
            Case "Select"
                GridBind()
                Dim Index As Int32 = CInt(e.CommandArgument) + dgPendingList.PageIndex * dgPendingList.PageSize
                SetObject(Index)
                Session("mItemName") = mPendingLoanToReturnList(Index).ItemName
                Session("PartNo") = mPendingLoanToReturnList(Index).ItemName
                Session("CheckQty") = "False"
                ''Response.Redirect("wfPartStockStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                Response.Redirect("wfPartStockStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfIssueItem_Ajax.aspx" & "&Name=" & HttpUtility.UrlEncode(mPendingLoanToReturnList(Index).ItemName))
                ''  Response.Redirect("wfPartStockStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfPendingLoanToReturn.aspx")
                '& "&ItemId=" & ItemId.ToString
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'If Session("Edit") = True And Request.QueryString("BackPage") = "wfIssue.aspx" Then
        '    mIssue.IssueItems.RemoveAt(mIssue.IssueItems.CurrentIndex)
        '    Session("Edit") = False
        '    Session("Back") = False
        '    Response.Redirect(Request.QueryString("BackPage"))
        'Else
        '    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        'End If
        If Session("Back") = True Then
            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        Else
            mIssue.IssueItems.RemoveAt(mIssue.IssueItems.CurrentIndex)
            'Session("Edit") = False
            Session("Back") = False
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub dgPendingList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingList.Sorting
        'GridBind()
        mPendingLoanToReturnList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingLoanToReturnList") = mPendingLoanToReturnList
        dgPendingList.DataSource = mPendingLoanToReturnList
        dgPendingList.DataBind()
        ControlVisibility()
    End Sub
#End Region

End Class