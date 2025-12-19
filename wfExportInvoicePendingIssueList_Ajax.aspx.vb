Public Class wfExportInvoicePendingIssueList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mExportInvoice As ExportInvoice
    Public mPendingIssueListForExportInvoice As PendingIssueListForExportInvoice
    Public mDistinctTextListForIssue As DistinctTextListForIssue
    Public mDistinctTextListForOrder As DistinctTextListForOrder
    Public mUserHasNoStoreRights As UserHasNoStoreRights
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mExportInvoice = Session("mExportInvoice")
        mPendingIssueListForExportInvoice = Session("mPendingIssueListForExportInvoice")
    End Sub
    Private Sub BindGrid()
        Dim IssueText As String = IIf(cmbIssueText.SelectedIndex <= 0, "", cmbIssueText.SelectedItem.Text)
        dgPendingIssueList.PageIndex = 0
        mPendingIssueListForExportInvoice = PendingIssueListForExportInvoice.GetPendingIssueListForExportInvoice(IssueText, Val(txtNo.Text.Trim), txtDate.Text, txtSearch.Text.Trim, cmbToType.SelectedValue, IIf(cmbOrderText.SelectedIndex <= 0, "", cmbOrderText.SelectedItem.Text), Val(txtOrderNo.Text.Trim), txtAmend.Text.Trim)
        dgPendingIssueList.DataSource = mPendingIssueListForExportInvoice
        dgPendingIssueList.DataBind()
        lblResult.Text = "Pending Issue List For Export Invoice : " + CStr(mPendingIssueListForExportInvoice.Count) + " Record(s) Found"
        Session("mPendingIssueListForExportInvoice") = mPendingIssueListForExportInvoice
    End Sub
    Private Sub setIssueList(ByVal index As Integer)
        Dim mtmpPendingIssueItemListForExportInvoice As PendingIssueItemListForExportInvoice
        mtmpPendingIssueItemListForExportInvoice = PendingIssueItemListForExportInvoice.GetPendingIssueItemListForExportInvoice(mPendingIssueListForExportInvoice(index).IssueID.ToString)
        For i As Integer = 0 To mtmpPendingIssueItemListForExportInvoice.Count - 1
            If mExportInvoice.ExportInvoiceItems.Contains(mtmpPendingIssueItemListForExportInvoice(i).IssueItemID) Then
                'skip
            Else
                mExportInvoice.ExportInvoiceItems.Add(mExportInvoice.ID)
                With mExportInvoice.ExportInvoiceItems.CurrentItem
                    .IssueItemID = mtmpPendingIssueItemListForExportInvoice(i).IssueItemID
                    .ItemID = mtmpPendingIssueItemListForExportInvoice(i).ItemID
                    .PartNo = mtmpPendingIssueItemListForExportInvoice(i).ItemName
                    .Description = mtmpPendingIssueItemListForExportInvoice(i).ItemDescription
                    .SerialNo = mtmpPendingIssueItemListForExportInvoice(i).SerialNo
                    .Qty = mtmpPendingIssueItemListForExportInvoice(i).DisplayQty
                    .UnitName = mtmpPendingIssueItemListForExportInvoice(i).DisplayUnit
                    .HSNACSCode = mtmpPendingIssueItemListForExportInvoice(i).HSNACSCode 'Added By Prashant on 28-Sep-2021 For STR27092021
                End With
            End If
        Next
        mExportInvoice.ConsigneeID = mPendingIssueListForExportInvoice(index).VendorID
        If Not mExportInvoice.ConsigneeID.Equals(Guid.Empty) Then
            mExportInvoice.ConsigneeAddress = mPendingIssueListForExportInvoice(index).VendorAddress
        End If

        If AppSettings("ClientCode") = "UHPL" Then 'Added By Prashant 22-mar-2013  'ALL22032013
            mExportInvoice.IECCodeNo = "0303037865"
        End If
        Session("mExportInvoice") = mExportInvoice
        Response.Redirect("wfExportInvoice_Ajax.aspx?BackPage=Index.aspx")
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDistinctTextListForIssue = DistinctTextListForIssue.GetDistinctText("3", , True, "(All)")
        cmbIssueText.DataSource = mDistinctTextListForIssue
        cmbIssueText.DataBind()
        mDistinctTextListForOrder = DistinctTextListForOrder.GetDistinctTextList("1", , True, "(All)")
        cmbOrderText.DataSource = mDistinctTextListForOrder
        cmbOrderText.DataBind()
        BindGrid()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If txtDate.Text = "" Then
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
        End If
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
            If mExportInvoice.ExportInvoiceItems.Count = 0 Then
                txtDate.Enabled = True
            Else
                txtDate.Enabled = False
            End If
        End If
    End Sub
    Private Sub dgPendingIssueList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPendingIssueList.PageIndexChanging
        dgPendingIssueList.PageIndex = e.NewPageIndex
        dgPendingIssueList.DataSource = mPendingIssueListForExportInvoice
        dgPendingIssueList.DataBind()
        upnlPendingIssueList.Update()
        Session("mPendingIssueListForExportInvoice") = mPendingIssueListForExportInvoice
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        BindGrid()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Request.QueryString("BackPage") = "Index.aspx" Then
            Response.Redirect("Index.aspx")
        Else
            Session("Edit") = False
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub dgPendingIssueList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPendingIssueList.RowCommand
        Select Case e.CommandName
            Case "SelectRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPendingIssueList.PageIndex * dgPendingIssueList.PageSize
                '--------------------------------------------
                mUserHasNoStoreRights = UserHasNoStoreRights.GetUserHasNoStoreRights(User.Identity.Name, mPendingIssueListForExportInvoice(Index).FromStoreID.ToString) ''Added By Prashant 13-May-2020
                If mUserHasNoStoreRights.Count > 0 Then
                    MSGBoxCtrl.show("Alert!", "Sorry you do not have rights for this store " + mPendingIssueListForExportInvoice(Index).FromStoreName + " Please contact with admin.", "", MsgBoxStyle.OkOnly, "ResetStore")
                    Exit Sub
                End If
                '-------------------------------------------- ''End of Added By Prashant 13-May-2020
                setIssueList(Index)
        End Select
    End Sub
    Private Sub dgPendingIssueList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPendingIssueList.Sorting
        mPendingIssueListForExportInvoice.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgPendingIssueList.DataSource = mPendingIssueListForExportInvoice
        dgPendingIssueList.DataBind()
        Session("mPendingIssueListForExportInvoice") = mPendingIssueListForExportInvoice
        upnlPendingIssueList.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
    End Sub
#End Region

End Class