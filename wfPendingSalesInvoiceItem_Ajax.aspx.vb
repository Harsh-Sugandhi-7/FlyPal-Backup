Public Class wfPendingSalesInvoiceItem_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mPendingToSalesInvoiceItemList As PendingToSalesInvoiceItemList
    Public mPendingList As PendingSalesInvoiceList
    Public mSalesInvoice As SalesInvoice
    Public mItemId As Guid = Guid.Empty
    Public BalanceQty As Decimal
    Dim PartNo As String
    Dim mCRateOfLastSalesInvoiceItem As CRateOfLastSalesInvoiceItem

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mItemId = Session("mItemId")
        mSalesInvoice = Session("mSalesInvoice")
        mPendingToSalesInvoiceItemList = Session("mPendingToSalesInvoiceItemList")
        mPendingList = Session("mPendingList")
        PartNo = Session("PartNo")

    End Sub

    Private Sub SetSession()

        Session("mSalesInvoice") = mSalesInvoice
        Session("mPendingToSalesInvoiceItemList") = mPendingToSalesInvoiceItemList
        Session("mPendingList") = mPendingList

    End Sub

    Private Sub SetObject(Index As Int32)

        mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemID = mItemId
        mSalesInvoice.SalesInvoiceItems.CurrentItem.TransTypeID = 23
        mSalesInvoice.SalesInvoiceItems.CurrentItem.IssueItemID = mPendingList(Index).IssueItemID
        mSalesInvoice.SalesInvoiceItems.CurrentItem.Qty = mPendingList(Index).BalanceQty
        Session("BalanceQty") = mSalesInvoice.SalesInvoiceItems.CurrentItem.Qty
        mCRateOfLastSalesInvoiceItem = CRateOfLastSalesInvoiceItem.GetCRateOfLastSalesInvoiceItem(ItemID:=mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemID.ToString)

        If mCRateOfLastSalesInvoiceItem(0).ItemCRate <> 0 Then
            mSalesInvoice.SalesInvoiceItems.CurrentItem.CRate = mCRateOfLastSalesInvoiceItem(0).ItemCRate
        Else
            mSalesInvoice.SalesInvoiceItems.CurrentItem.CRate = 0
        End If

        mSalesInvoice.SalesInvoiceItems.CurrentItem.Remark = ""
        mSalesInvoice.SalesInvoiceItems.CurrentItem.Note = ""
        Session("mSalesInvoice") = mSalesInvoice

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        If control.Enabled = False Or control.Visible = False Then Exit Sub
        control.Focus()

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        dgPartStockStatusList.DataSource = mPendingToSalesInvoiceItemList
        dgItemOrderIssueDetail.DataSource = mPendingList
        DataBind()

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()

        If Not IsPostBack Then

            If txtSearch.Enabled = True Then
                setFocus(txtSearch)
            End If

            txtSearch.Text = PartNo
            mPendingToSalesInvoiceItemList = PendingToSalesInvoiceItemList.GetPendingToSalesInvoiceItemList(mSalesInvoice.VendorID,
                                                                                                            txtSearch.Text.Trim,
                                                                                                            mSalesInvoice.SalesInvoiceDate.ToString)

            mPendingList = PendingSalesInvoiceList.GetPendingToSalesInvoiceList(mSalesInvoice.VendorID,
                                                                                "",
                                                                                mSalesInvoice.SalesInvoiceDate.ToString)

            Session("mPendingToSalesInvoiceItemList") = mPendingToSalesInvoiceItemList
            Session("mPendingList") = mPendingList
            DataFieldBind()

        End If

        lblResult.Text = "Part Stock Status List : " & mPendingToSalesInvoiceItemList.Count & " Record(s) found."
        lblResult1.Text = "Pending Item Stock List : " & mPendingList.Count & " Record(s) found"

    End Sub

    Private Sub SearchRecords(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click

        mPendingToSalesInvoiceItemList = PendingToSalesInvoiceItemList.GetPendingToSalesInvoiceItemList(mSalesInvoice.VendorID,
                                                                                                        txtSearch.Text.Trim,
                                                                                                        mSalesInvoice.SalesInvoiceDate.ToString)

        mPendingList = PendingSalesInvoiceList.GetPendingToSalesInvoiceList(mSalesInvoice.VendorID,
                                                                            "",
                                                                            mSalesInvoice.SalesInvoiceDate.ToString)
        Session("mPendingList") = mPendingList
        Session("mPendingToSalesInvoiceItemList") = mPendingToSalesInvoiceItemList
        DataFieldBind()
        lblResult.Text = "Part Stock Status List : " & mPendingToSalesInvoiceItemList.Count & " Record(s) found."
        lblResult1.Text = "Pending Item Stock List : " & mPendingList.Count & " Record(s) found"

    End Sub

    Private Sub GV_PartStockStatusList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgPartStockStatusList.RowCommand

        Select Case e.CommandName
            Case "Select"

                Dim Index As Integer = CInt(e.CommandArgument) + dgPartStockStatusList.PageIndex * dgPartStockStatusList.PageSize

                mPendingList = PendingSalesInvoiceList.GetPendingToSalesInvoiceList(mSalesInvoice.VendorID,
                                                                                    mPendingToSalesInvoiceItemList(Index).ItemName,
                                                                                    mSalesInvoice.SalesInvoiceDate.ToString)
                Session("mPendingList") = mPendingList

                DataFieldBind()

                lblResult.Text = "Part Stock Status List : " & mPendingToSalesInvoiceItemList.Count & " Record(s) found."
                lblResult1.Text = "Pending Item Stock List : " & mPendingList.Count & " Record(s) found"

        End Select

    End Sub

    Private Sub GV_ItemOrderIssueDetail_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgItemOrderIssueDetail.RowCommand

        Select Case e.CommandName
            Case "Select"

                Dim Index As Integer = CInt(e.CommandArgument) + dgItemOrderIssueDetail.PageIndex * dgItemOrderIssueDetail.PageSize
                SetObject(Index)

                Session.Remove("mPendingToSalesInvoiceItemList")
                Session.Remove("mPendingList")

                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))

        End Select

    End Sub

    Private Sub GoBack(sender As Object, e As EventArgs) Handles btnBack.Click

        Session("mSalesInvoice") = mSalesInvoice
        Response.Redirect("wfSalesInvoiceItem_Ajax.aspx?BackPage=wfPendingSalesInvoiceItem_Ajax.aspx")

    End Sub

    'New addition by Rupali on 22-Jun-09 for Sorting Order
    Private Sub GV_ItemOrderIssueDetail_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgItemOrderIssueDetail.Sorting

        mPendingList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingList") = mPendingList
        dgItemOrderIssueDetail.DataSource = mPendingList
        dgItemOrderIssueDetail.DataBind()

    End Sub

    'New addition by Rupali on 22-Jun-09 for Sorting Order
    Private Sub GV_PartStockStatusList_Sorting(source As Object, e As GridViewSortEventArgs) Handles dgPartStockStatusList.Sorting

        mPendingToSalesInvoiceItemList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingToSalesInvoiceItemList") = mPendingToSalesInvoiceItemList
        dgPartStockStatusList.DataSource = mPendingToSalesInvoiceItemList
        dgPartStockStatusList.DataBind()

    End Sub

#End Region

End Class