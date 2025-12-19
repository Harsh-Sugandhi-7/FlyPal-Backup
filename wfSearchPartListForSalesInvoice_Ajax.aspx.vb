Public Class wfSearchPartListForSalesInvoice_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mItems As Items
    Public mSalesInvoice As SalesInvoice
    Public Text, Index, IsSerialized, DescriptionText As String

    Public mCurrentpage As Integer = 1
    Public mpageSize As Integer = 10
    Dim mpageindex As Integer = 0
    Dim pagecount As Integer = 0
    Dim totalCountForCustomePaging As Integer = 0

    Dim EventLogID As Guid
    Dim mCRateOfLastSalesInvoiceItem As CRateOfLastSalesInvoiceItem

    Dim mGSTPercentage As GSTPercentage
    Dim mVendor As Vendor

#End Region

#Region " Business Methods "

    Private Sub GetSession()

        mSalesInvoice = CType(Session("mSalesInvoice"), SalesInvoice)
        mItems = CType(Session("mItems"), Items)

    End Sub

    Private Sub RemoveSession()

        Session.Remove("mItemList")

    End Sub

    Private Sub FindNow(Optional PartNo As String = "",
                        Optional Description As String = "")

        mItems = Flypal.Items.GetItems(1,
                                       PartNo,
                                       Description, "", "", "", "",
                                       Guid.Empty.ToString,
                                       IsCustomPaging:=False,
                                       CurrentPage:=mpageindex,
                                       PageSize:=mpageSize)

        Session("mItems") = mItems
        Session("IsSerialized") = IsSerialized
        gdvItem.DataSource = mItems
        gdvItem.DataBind()
        UpdateItemGridView()

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        If control.Enabled = False Or control.Visible = False Then Exit Sub
        control.Focus()

    End Sub

    Private Sub DataFieldBind(Optional PartNo As String = "",
                              Optional Description As String = "")

        Index = Session("Index")
        Text = Session("Text")
        DescriptionText = Session("DescriptionText")
        IsSerialized = Session("IsSerialized")

        FindNow(PartNo, Description)
        txtName.Text = Text
        txtDescription.Text = DescriptionText

    End Sub

    Private Sub UpdateItemGridView()

        lblResult.Text = "List of Part as per criteria : " & mItems.Count &
                         " Record(s) found."

        gdvItem.DataBind()
        upnlgrid.Update()

    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        GetSession()

        If Not IsPostBack Then

            setFocus(txtName)
            txtName.Text = Session("PartNo").ToString
            txtName.DataBind()
            DataFieldBind(txtName.Text.Trim, txtDescription.Text.Trim)
            Session.Remove("PartNo")

        End If

    End Sub

    Private Sub GV_Item_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gdvItem.PageIndexChanging

        gdvItem.PageIndex = e.NewPageIndex
        FindNow(txtName.Text)

    End Sub

    Private Sub GV_Item_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvItem.RowCommand

        Select Case e.CommandName
            Case "SelectRec"

                Dim Index As Integer = CInt(e.CommandArgument) + gdvItem.PageIndex * gdvItem.PageSize
                Dim mId As Guid = mItems(Index).ID
                mSalesInvoice.SalesInvoiceItems.CurrentItem.TransTypeID = 74
                mSalesInvoice.SalesInvoiceItems.CurrentItem.ItemID = mId

                If mItems(Index).SerialisedStatus = True Then
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.Qty = 1
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.IsSerialized = True
                Else
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.IsSerialized = False
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.Qty = 0.0
                End If

                mCRateOfLastSalesInvoiceItem = CRateOfLastSalesInvoiceItem.GetCRateOfLastSalesInvoiceItem(ItemID:=mId.ToString)

                If mCRateOfLastSalesInvoiceItem(0).ItemCRate <> 0 Then
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.CRate = mCRateOfLastSalesInvoiceItem(0).ItemCRate
                Else
                    mSalesInvoice.SalesInvoiceItems.CurrentItem.CRate = 0
                End If

                mSalesInvoice.SalesInvoiceItems.CurrentItem.Remark = ""
                mSalesInvoice.SalesInvoiceItems.CurrentItem.Note = ""
                Session("mSalesInvoice") = mSalesInvoice
                DataFieldBind(txtName.Text.Trim, txtDescription.Text.Trim)
                Session.Remove("mItems")
                mItems = Nothing
                Response.Redirect(Request.QueryString("ChildPage") &
                                  "?BackPage=" &
                                  Request.QueryString("BackPage"))

        End Select

    End Sub

    Private Sub GV_Item_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gdvItem.Sorting

        mItems.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mItems") = mItems
        gdvItem.DataSource = mItems
        UpdateItemGridView()

    End Sub

    Private Sub SearchRecords(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click

        gdvItem.PageIndex = 0
        mpageindex = 0

        FindNow(txtName.Text.Trim,
                txtDescription.Text.Trim)

    End Sub

    Private Sub Close_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        RemoveSession()
        mItems = Nothing
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))

    End Sub

#End Region

End Class