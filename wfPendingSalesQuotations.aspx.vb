Partial Class wfPendingSalesQuotations
    Inherits System.Web.UI.Page

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

#Region "Variable Declaration"
    Public mPendingQuotationList As PendingQuotationList
    Public mPendingSalesQuotationItems As PendingSalesQuotationItems
    Public mSalesOrder As SalesOrder
    Public mSelectList() As Boolean
    Public mPrevTransID As Guid
    Private mIsAll As Boolean = False
    Private mSalesOrderDate As String
    Private mVendorID As Guid
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mSalesOrder = Session("mSalesOrder")
        mPendingQuotationList = Session("mPendingQuotationList")
        mPendingSalesQuotationItems = Session("mPendingSalesQuotationItems")
    End Sub
    Private Sub SetMultipleObject()
        Dim chkSelect As CheckBox
        Dim item As DataGridItem
        Dim Recordno, PageItems As Integer
        PageItems = dgTransItemList.Items.Count - 1
        For I As Integer = 0 To PageItems
            Recordno = I + dgTransItemList.PageSize * dgTransItemList.CurrentPageIndex
            item = dgTransItemList.Items(I)
            chkSelect = CType(item.FindControl("chkSelect"), CheckBox)
            mPendingSalesQuotationItems(Recordno).IsSelected = chkSelect.Checked
        Next
        Session("mPendingSalesQuotationItems") = mPendingSalesQuotationItems
    End Sub
#End Region

#Region "Data Binding"
    Public Sub DataFieldBind()
        If txtSalesOrderDate.Text.ToString = "" Then
            txtSalesOrderDate.Text = Today.Date
        End If
        If mIsAll Then
            mPendingQuotationList = PendingQuotationList.GetPendingQuotationList(txtSalesOrderDate.Text.ToString, mSalesOrder.VendorID, Guid.Empty)
        Else
            mPendingQuotationList = PendingQuotationList.GetPendingQuotationList(txtSalesOrderDate.Text.ToString, mSalesOrder.VendorID, mPrevTransID)
        End If
        dgTransList.DataSource = mPendingQuotationList
        Session("mPendingQuotationList") = mPendingQuotationList
        dgTransList.DataBind()
        lblResult.Text = "List of Quotations : " + mPendingQuotationList.Count.ToString + " Record (s) found"
        If mPendingQuotationList.Count = 0 Then
            btnDone.Enabled = False
        Else
            btnDone.Enabled = True
        End If
    End Sub
    Private Sub dgTransList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgTransList.PageIndexChanged
        dgTransList.CurrentPageIndex = e.NewPageIndex
        dgTransList.DataSource = mPendingQuotationList
        Session("mPendingQuotationList") = mPendingQuotationList
        dgTransList.DataBind()
    End Sub
    Private Sub dgTransItemList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgTransItemList.PageIndexChanged
        dgTransItemList.CurrentPageIndex = e.NewPageIndex
        dgTransItemList.DataSource = mPendingSalesQuotationItems
        Session("mPendingSalesQuotationItems") = mPendingSalesQuotationItems
        dgTransItemList.DataBind()
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            If mPrevTransID.Equals(Guid.Empty) Then
                rdbFromAllPendingQuotation.Checked = True
            Else
                rdbFromLastQuotation.Checked = True
            End If
            If txtSalesOrderDate.Text.ToString = "" Then
                txtSalesOrderDate.Text = Today.Date
            End If
            DataFieldBind()
            If mSalesOrder.IsNew Then
                txtSalesOrderDate.Enabled = True
                rdbFromLastQuotation.Checked = False
                rdbFromAllPendingQuotation.Checked = True
            Else
                txtSalesOrderDate.Enabled = False
                rdbFromLastQuotation.Checked = True
                rdbFromAllPendingQuotation.Checked = False
            End If
        End If
    End Sub
    Private Sub rdbFromLastQuotation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbFromLastQuotation.CheckedChanged
        mIsAll = False
    End Sub
    Private Sub rdbFromAllPendingQuotation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbFromAllPendingQuotation.CheckedChanged
        mIsAll = True
    End Sub
    Private Sub dgTransList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgTransList.ItemCommand
        If e.Item.ItemIndex = -1 Then Exit Sub
        Dim Index As Integer = e.Item.ItemIndex + dgTransList.CurrentPageIndex * dgTransList.PageSize
        Select Case e.CommandName
            Case "Select"
                mPendingSalesQuotationItems = PendingSalesQuotationItems.GetPendingQuotationList(mPendingQuotationList.Item(Index).ID)
                dgTransItemList.CurrentPageIndex = 0
                dgTransItemList.DataSource = mPendingSalesQuotationItems
                Session("mPendingSalesQuotationItems") = mPendingSalesQuotationItems
                dgTransItemList.DataBind()
                lblResult1.Text = "List of Quotation Item (s): " + mPendingSalesQuotationItems.Count.ToString + " Record (s) found"
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        dgTransList.CurrentPageIndex = 0
        dgTransItemList.CurrentPageIndex = 0
        DataFieldBind()
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        SetMultipleObject()
        Session("PendingQuotationItems") = "True"

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub dgTransList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgTransList.SortCommand
        mPendingQuotationList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mPendingQuotationList") = mPendingQuotationList
        dgTransList.DataSource = mPendingQuotationList
        dgTransList.DataBind()
    End Sub
#End Region

End Class
