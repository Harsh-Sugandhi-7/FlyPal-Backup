Public Class wfPendingToReturnExchangeCoreUnitList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    'Public mIssue As Issue
    Public mPendingToReturnForExchangeRepairList As PendingToReturnForExchangeRepairList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        'mIssue = Session("mIssue")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPendingToReturnForExchangeRepairList")
        Session.Remove("mPendingToReturnForExchangeRepairInfo")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfPendingToReturnExchangeCoreUnitList_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Public Sub setObject(ByVal Index As Integer)
        '23-03-2007*** ---------------------------------------------------------------------------------
        'If Issue is doing for 'ISSUE TO Vendor FOR EXCHANGE REPAIR'then...
        'Then Select First against what RECEIPT/ORDER FOR EXCHANGE REPAIR?, we issing Part
        'mIssue = Session("mIssue")
        mPendingToReturnForExchangeRepairList = Session("mPendingToReturnForExchangeRepairList")
        'mIssue.VendorID = mPendingToReturnForExchangeRepairList.Item(Index).VendorID
        Dim mPendingToReturnForExchangeRepairInfo As PendingToReturnForExchangeRepairList.PendingToReturnForExchangeRepairInfo
        mPendingToReturnForExchangeRepairInfo = mPendingToReturnForExchangeRepairList.Item(Index)
        Session("mPendingToReturnForExchangeRepairInfo") = mPendingToReturnForExchangeRepairInfo
        'Session("mIssue") = mIssue
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(Guid.Empty, Guid.Empty, CDate(txtDate.Text).ToString, "", 16, 1, ExchangeOrdersOnly:=1) 'Pending Against Order
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        lblResult.Text = "Pending Exchange Order(s) details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
        DataBind()
    End Sub
    Private Sub dgPendingList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingList.PageIndexChanging
        dgPendingList.PageIndex = e.NewPageIndex
        mPendingToReturnForExchangeRepairList = Session("mPendingToReturnForExchangeRepairList")
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        dgPendingList.DataBind()
    End Sub
    Private Sub GridBind()
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        dgPendingList.DataBind()
    End Sub
#End Region

#Region " Event "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            Session("MiddleFrame") = "wfPendingToReturnExchangeCoreUnitList_Ajax.aspx?"
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
        dgPendingList.PageIndex = 0
        mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(Guid.Empty, Guid.Empty, CDate(txtDate.Text).ToString, txtName.Text.Trim, 16, 1, ExchangeOrdersOnly:=1) '1 For Aginst Order
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        dgPendingList.DataBind()
        lblResult.Text = "Pending Exchange Order(s) details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
    End Sub
    Private Sub txtName_TextChanged(sender As Object, e As System.EventArgs) Handles txtName.TextChanged
        txtName.Focus()
        dgPendingList.PageIndex = 0
        mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(Guid.Empty, Guid.Empty, CDate(txtDate.Text).ToString, txtName.Text.Trim, 16, 1, ExchangeOrdersOnly:=1) '1 For Aginst Order
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        dgPendingList.DataBind()
        lblResult.Text = "Pending Exchange Order(s) details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
    End Sub
    Private Sub dgPendingList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingList.RowCommand
        Select Case e.CommandName
            Case "SelectForDiscard"
                GridBind()
                Dim Index As Integer = CInt(e.CommandArgument) + dgPendingList.PageIndex * dgPendingList.PageSize
                setObject(Index)
                'Response.Redirect("wfIssueStockItemListForDiscardExchangeCoreUnit_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=wfPendingToReturnExchangeCoreUnitList_Ajax.aspx")
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfIssueStockItemListForDiscardExchangeCoreUnit_Ajax.aspx?BackPage=index.aspx');", True)
                Response.Redirect("wfIssueStockItemListForDiscardExchangeCoreUnit_Ajax.aspx?BackPage=index.aspx&IssueDate=" & txtDate.Text)
        End Select
    End Sub
    'Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
    '    If mIssue.IsNew Then
    '        mIssue.IDate = CDate(txtDate.Text)
    '    End If
    '    dgPendingList.PageIndex = 0
    '    mPendingToReturnForExchangeRepairList = PendingToReturnForExchangeRepairList.GetPendingToReturnForExchangeRepairList(mIssue.StoreID, mIssue.VendorID, mIssue.IDate.ToString, txtName.Text.Trim, mIssue.TransTypeID, 1)
    '    dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
    '    Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
    '    dgPendingList.DataBind()
    '    lblResult.Text = "Pending Order details : " + CStr(mPendingToReturnForExchangeRepairList.Count) + " Record(s) Found"
    'End Sub
    Private Sub btnTopClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session("MiddleFrame") = ""
        Response.Redirect("DashBoard.aspx")
    End Sub
    Private Sub dgPendingList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingList.Sorting
        mPendingToReturnForExchangeRepairList = Session("mPendingToReturnForExchangeRepairList")
        mPendingToReturnForExchangeRepairList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingToReturnForExchangeRepairList") = mPendingToReturnForExchangeRepairList
        dgPendingList.DataSource = mPendingToReturnForExchangeRepairList
        dgPendingList.DataBind()
    End Sub
#End Region

End Class