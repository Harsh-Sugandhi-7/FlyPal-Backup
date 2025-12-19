Public Class wfQuotationPartStockStatus_Ajax
    Inherits System.Web.UI.Page


#Region " Variable Declaration "
    Public mItemId As Guid = Guid.Empty
    Public mQuotation As Quotation
    Public mItemList As ItemStockStatusListForQuotation
    Public mPendingEnquiryList As PendingEnquiryList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItemId = Session("mItemId")
        mQuotation = Session("mQuotation")
        mItemList = Session("mItemList")
        mPendingEnquiryList = Session("mPendingEnquiryList")
    End Sub
    Private Sub SetSession()
        Session("mItemId") = mItemId
        Session("mQuotation") = mQuotation
        Session("mItemList") = mItemList
        Session("mPendingEnquiryList") = mPendingEnquiryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemId")
        Session.Remove("mItemList")
        Session.Remove("mPendingEnquiryList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetPage()
        lblResult.Text = "Part Stock Status List: " & mItemList.Count & " Record(s) found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgItemList.DataSource = mItemList
        DataBind()
        upnlDetails.Update()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If
            txtSearch.Text = Request.QueryString("Name")
            mItemList = ItemStockStatusListForQuotation.GetItemStockStatusListForQuotation(txtSearch.Text.Trim, mQuotation.Date.ToString)
            Session("mItemList") = mItemList

            If mQuotation.TransTypeID = Util.Trans.Quotation Then
                mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(Util.Trans.Enquiry, mQuotation.VendorID, mQuotation.QuotationItems.CurrentItem.ItemID, mQuotation.Date)        'Set DataSource of the Grid
            ElseIf mQuotation.TransTypeID = Util.Trans.PurchaseQuotation Then
                mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(Util.Trans.RequestingForQuotation, mQuotation.VendorID, mQuotation.QuotationItems.CurrentItem.ItemID, mQuotation.Date)        'Set DataSource of the Grid
            End If

            Session("mPendingEnquiryList") = mPendingEnquiryList
            DataFieldBind()
        End If
        SetPage()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgItemList.PageIndex = 0
        mItemList = ItemStockStatusListForQuotation.GetItemStockStatusListForQuotation(txtSearch.Text.Trim, mQuotation.Date.ToString)

        If mQuotation.TransTypeID = Util.Trans.Quotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(Util.Trans.Enquiry, mQuotation.VendorID, Guid.Empty, mQuotation.Date)    'Set DataSource of the Grid
        ElseIf mQuotation.TransTypeID = Util.Trans.PurchaseQuotation Then
            mPendingEnquiryList = PendingEnquiryList.GetPendingEnquiryList(Util.Trans.RequestingForQuotation, mQuotation.VendorID, Guid.Empty, mQuotation.Date)    'Set DataSource of the Grid
        End If

        Session("mItemList") = mItemList
        Session("mPendingEnquiryList") = mPendingEnquiryList
        DataFieldBind()
        SetPage()
    End Sub
      Private Sub dgItemList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgItemList.RowCommand
        Select Case e.CommandName
            Case "SelectPart"
                Dim Index As Integer = CInt(e.CommandArgument) + dgItemList.PageIndex * dgItemList.PageSize
                Dim ItemId As Guid = mItemList(Index).ItemID
                Session("mItemId") = mItemId
                mQuotation.QuotationItems.CurrentItem.ItemID = ItemId
                mQuotation.QuotationItems.CurrentItem.EnquiryItemID = Guid.Empty
                mQuotation.QuotationItems.CurrentItem.Qty = 0D
                mQuotation.QuotationItems.CurrentItem.IPCReference = IIf(mItemList(Index).IPCReference = "", "", mItemList(Index).IPCReference)
                Session("EnquiryQty") = 0D
                Session("mQuotation") = mQuotation
                Session("mQuotation") = mQuotation
                DataFieldBind()
                RemoveSession()
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        End Select
    End Sub
   Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub dgItemList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgItemList.PageIndexChanging
        dgItemList.PageIndex = e.NewPageIndex
        dgItemList.DataSource = mItemList
        Session("mItemList") = mItemList
        DataFieldBind()
    End Sub
       Private Sub dgItemList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgItemList.Sorting
        mItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItemList") = mItemList
       DataFieldBind
    End Sub
#End Region

End Class