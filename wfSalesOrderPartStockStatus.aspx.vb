Partial Class wfSalesOrderPartStockStatus
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mItemId As Guid = Guid.Empty
    Public mSalesOrder As SalesOrder
    Public mItemList As ItemStockStatusListForSalesOrder
    Public mPendingQuotationItemList As PendingQuotationList
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mItemId = Session("mItemId")
        mSalesOrder = Session("mSalesOrder")
        mItemList = Session("mItemList")
        mPendingQuotationItemList = Session("mPendingQuotationItemList")
    End Sub
    Private Sub SetSession()
        Session("mItemId") = mItemId
        Session("mSalesOrder") = mSalesOrder
        Session("mItemList") = mItemList
        Session("mPendingQuotationItemList") = mPendingQuotationItemList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mItemId")
        Session.Remove("mItemList")
        Session.Remove("mPendingQuotationItemList")
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetObject(ByVal Index As Int32)
        ''mSalesOrder.SalesOrderItems.CurrentItem.QuotationItemID = mPendingQuotationItemList(Index).QuotationItemID
        ''mSalesOrder.SalesOrderItems.CurrentItem.ItemID = mItemId
        ''mSalesOrder.SalesOrderItems.CurrentItem.QuotationNo = mPendingQuotationItemList(Index).QuotationTextNo
        ''mSalesOrder.SalesOrderItems.CurrentItem.QuotationDate = mPendingQuotationItemList(Index).QuotationDate
        ''mSalesOrder.SalesOrderItems.CurrentItem.Qty = mPendingQuotationItemList(Index).QuotationQty

        ''Session("QuotationQty") = mPendingQuotationItemList(Index).QuotationQty
        ''Session("mSalesOrder") = mSalesOrder
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgItemList.DataSource = mItemList
        dgPendingItemList.DataSource = mPendingQuotationItemList
        DataBind()
    End Sub
    Public Sub NewPage(ByVal s As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dgItemList.CurrentPageIndex = e.NewPageIndex
        dgItemList.DataSource = mItemList
        Session("mItemList") = mItemList
        dgItemList.DataBind()
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
            mItemList = ItemStockStatusListForSalesOrder.GetItemStockStatusListForSalesOrder(txtSearch.Text.Trim)
            Session("mItemList") = mItemList
            ''  mPendingQuotationItemList = PendingQuotationList.GetPendingQuotationList(mSalesOrder.VendorID, mSalesOrder.SalesOrderItems.CurrentItem.ItemID, mSalesOrder.Date)       'Set DataSource of the Grid
            Session("mPendingQuotationItemList") = mPendingQuotationItemList
            DataFieldBind()
        End If
        lblResult.Text = "List of pending items : " & mItemList.Count & " Record(s) found."
        ''lblResult1.Text = "Pending Item Details List : " & mPendingQuotationItemList.Count & " Record(s) found"
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgItemList.CurrentPageIndex = 0
        mItemList = ItemStockStatusListForSalesOrder.GetItemStockStatusListForSalesOrder(txtSearch.Text.Trim)
        ''   mPendingQuotationItemList = PendingQuotationList.GetPendingQuotationList(mSalesOrder.VendorID, Guid.Empty, mSalesOrder.Date)     'Set DataSource of the Grid
        Session("mItemList") = mItemList
        Session("mPendingQuotationItemList") = mPendingQuotationItemList
        DataFieldBind()
        lblResult.Text = "List of pending items : " & mItemList.Count & " Record(s) found."
        lblResult1.Text = "Pending Item Details List : " & mPendingQuotationItemList.Count & " Record(s) found"
    End Sub
    Private Sub dgItemList_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgItemList.ItemCommand
        Dim Index As Int32 = e.Item.ItemIndex + dgItemList.CurrentPageIndex * dgItemList.PageSize
        Select Case e.CommandName
            Case "Select"
                Dim ItemId As Guid = New Guid(e.Item.Cells(0).Text)
                mItemId = ItemId
                Session("mItemId") = mItemId
                ''  mPendingQuotationItemList = PendingQuotationList.GetPendingQuotationList(mSalesOrder.VendorID, ItemId, mSalesOrder.Date)  'Set DataSource of the Grid
                Session("mPendingQuotationItemList") = mPendingQuotationItemList
                DataFieldBind()
                lblResult.Text = "List of pending items : " & mItemList.Count & " Record(s) found."
                lblResult1.Text = "Pending Item Details List : " & mPendingQuotationItemList.Count & " Record(s) found"
            Case "SelectPart"
                Dim ItemId As Guid = New Guid(e.Item.Cells(0).Text)
                'Dim mCurrency As Currency = Currency.GetCurrency(mSalesOrder.CurrencyID)
                mItemId = ItemId
                Session("mItemId") = mItemId
                mSalesOrder.SalesOrderItems.CurrentItem.ItemID = ItemId
                mSalesOrder.SalesOrderItems.CurrentItem.QuotationItemID = Guid.Empty
                'mSalesOrder.SalesOrderItems.CurrentItem.Currency = mCurrency.Name
                Session("mSalesOrder") = mSalesOrder
                RemoveSession()
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        End Select
    End Sub
    Private Sub dgPendingItemList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPendingItemList.ItemCommand
        Dim Index As Int32 = e.Item.ItemIndex + dgPendingItemList.CurrentPageIndex * dgPendingItemList.PageSize
        Select Case e.CommandName
            Case "Select"
                SetObject(Index)
                RemoveSession()
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
#End Region

End Class
