Public Class wfReceivedFromWorkShopAsServiceablReturned_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mReceiptCumInvoice As ReceiptCumInvoice
    Public mPartListForReceivedFromWorkShopAsServiceablReturned As PartListForReceivedFromWorkShopAsServiceablReturned
#End Region

#Region "Business Methods"
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mReceiptCumInvoice = CType(Session("mReceiptCumInvoice"), ReceiptCumInvoice)
        mPartListForReceivedFromWorkShopAsServiceablReturned = CType(Session("mPartListForReceivedFromWorkShopAsServiceablReturned"), PartListForReceivedFromWorkShopAsServiceablReturned)
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBinding(Optional ByVal PartNo As String = "", Optional ByVal ToDate As String = "")
        mPartListForReceivedFromWorkShopAsServiceablReturned = PartListForReceivedFromWorkShopAsServiceablReturned.GetPartListForRCIFromAircraftAsCoreUnitReturn(PartNo, mReceiptCumInvoice.WorkShopID.ToString, ToDate)
        lblResult.Text = "List of Part: " & mPartListForReceivedFromWorkShopAsServiceablReturned.Count & " Record(s) Found."
        dgPartList.DataSource = mPartListForReceivedFromWorkShopAsServiceablReturned
        Session("mPartListForReceivedFromWorkShopAsServiceablReturned") = mPartListForReceivedFromWorkShopAsServiceablReturned
        DataBind()
        upnlDetails.Update()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If txtDate.Text = "" Then
            txtDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
        End If
        If Not IsPostBack Then
            setFocus(txtName)

            If mReceiptCumInvoice.ReceiptCumInvoiceItems.Count - 1 = 0 Then
                txtDate.Enabled = True
            Else
                txtDate.Enabled = False
            End If
            txtDate.Text = mReceiptCumInvoice.RecCumInvDateFormatted
            DataFieldBinding(txtName.Text.Trim, txtDate.Text)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgPartList.PageIndex = 0
        DataFieldBinding(txtName.Text.Trim, txtDate.Text)
    End Sub
    Private Sub dgPartList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPartList.RowCommand
        Select Case e.CommandName
            Case "SelectPart"
                Dim Index As Integer = CInt(e.CommandArgument) + dgPartList.PageIndex * dgPartList.PageSize
                If txtDate.Text = "" Then
                    mReceiptCumInvoice.RecCumInvDate = Today.Date
                Else
                    mReceiptCumInvoice.RecCumInvDate = txtDate.Text
                End If
                mReceiptCumInvoice.FromTypeID = 16
                mReceiptCumInvoice.CurrencyID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).CurrencyID
                mReceiptCumInvoice.ConversionFactor = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ConversionFactor

                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromItemTypeID = 16 'From Work shop
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.FromPartList = True
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ItemID
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.Part = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ItemName
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PartDescription = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ItemDescription
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsPartFromListisSerialized = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).SerialisedStatus
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.BaseUnitID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).UnitID
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).UnitID

                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayQty = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).DisplayQty
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IssueItemID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).IssueItemID

                'If mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ExpiryMonths > 0 Then
                '    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate = mReceiptCumInvoice.Receipt.RecdDate
                '    If Not (mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate) Is System.DBNull.Value Then
                '        mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ExpiryDate = CDate(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.StartDate).AddMonths(mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ExpiryMonths)
                '    End If
                'End If
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).Rate / mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ConversionFactor
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCRate = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CRate       'Added By Prashant 5-Feb-2019 ALL04022019
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount = (mPartListForReceivedFromWorkShopAsServiceablReturned(Index).CRate * mPartListForReceivedFromWorkShopAsServiceablReturned(Index).DisplayQty)
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayCAmount = mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CAmount   'Added By Prashant 5-Feb-2019 ALL04022019
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CCommercialRate = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).CommercialRate / mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ConversionFactor
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.DisplayUnitID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).UnitID
                mReceiptCumInvoice.WorkShopID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).WorkShopID

                If (mPartListForReceivedFromWorkShopAsServiceablReturned(Index).SerialisedStatus = True And mPartListForReceivedFromWorkShopAsServiceablReturned(Index).PrimaryCategoryID = 2) Then
                    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.PrimaryCategoryID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).PrimaryCategoryID
                    mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.CodeNo = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).CodeNo
                End If
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagID = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ItemTagID
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemTagName = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).ItemTagName
                'Added on  07-Sep-2016 by Shital
                mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.IsAirworthinss = mPartListForReceivedFromWorkShopAsServiceablReturned(Index).IsAirworthiness

                Session("mReceiptCumInvoice") = mReceiptCumInvoice
                Session("TotalCount") = CDec(IIf(mPartListForReceivedFromWorkShopAsServiceablReturned(Index).SerialisedStatus, 1, 0)).ToString
                Session("mTotalPendingItemQty") = CDec(IIf(mPartListForReceivedFromWorkShopAsServiceablReturned(Index).SerialisedStatus, 1, 0)).ToString

                Session.Remove("mPartListForReceivedFromWorkShopAsServiceablReturned")
                mPartListForReceivedFromWorkShopAsServiceablReturned = Nothing
                DataFieldBinding()
                Response.Redirect("wfReceiptcumInvoiceItem_Ajax.aspx?BackPage=" & "wfReceiptCumInvoice_Ajax.aspx" & "&ChildPage1=" & "wfReceivedFromWorkShopAsServiceablReturned_Ajax.aspx")
        End Select
    End Sub
    Private Sub dgPartList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPartList.PageIndexChanging
        dgPartList.PageIndex = e.NewPageIndex
        dgPartList.DataSource = mPartListForReceivedFromWorkShopAsServiceablReturned
        Session("mPartListForReceivedFromWorkShopAsServiceablReturned") = mPartListForReceivedFromWorkShopAsServiceablReturned
        dgPartList.DataBind()
        upnlDetails.Update()
    End Sub
    Private Sub dgPartList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPartList.Sorting
        mPartListForReceivedFromWorkShopAsServiceablReturned.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPartListForReceivedFromWorkShopAsServiceablReturned") = mPartListForReceivedFromWorkShopAsServiceablReturned
        dgPartList.DataSource = mPartListForReceivedFromWorkShopAsServiceablReturned
        dgPartList.DataBind()
        upnlDetails.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem.ItemID.Equals(Guid.Empty) Then
            mReceiptCumInvoice.ReceiptCumInvoiceItems.Remove(mReceiptCumInvoice.ReceiptCumInvoiceItems.CurrentItem)
        End If
        Session("mReceiptCumInvoice") = mReceiptCumInvoice
        Session.Remove("mPartListForReceivedFromWorkShopAsServiceablReturned")
        mPartListForReceivedFromWorkShopAsServiceablReturned = Nothing
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
        dgPartList.PageIndex = 0
        DataFieldBinding(txtName.Text.Trim, txtDate.Text)
    End Sub
#End Region

End Class
