Partial Class wfMgtApprovedRequisitionItems
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

#Region "Variables"
    Public mQuotationItem As QuotationItem
    Public mQuotationItems As QuotationItems
    Dim mTransDate As String
    Dim mOrderItemID As Guid = Guid.Empty
    Dim mVendorName As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mQuotationItems = CType(Session("mQuotationItems"), QuotationItems)
        mQuotationItem = CType(Session("mQuotationItem"), QuotationItem)
        mTransDate = Session("TransDate")
        mOrderItemID = Session("OrderItem")
        mVendorName = Session("VendorName")
    End Sub
    Private Sub SetSession()
        Session("mQuotationItems") = mQuotationItems
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "FocusScript", str)
    End Sub
    Private Sub FindNow()
        mQuotationItems = QuotationItems.GetQuotationItems(mTransDate, mVendorName, txtPartNumber.Text, mOrderItemID)
        dgOrderItemList.DataSource = mQuotationItems
        Session("mQuotationItems") = mQuotationItems
        DataBind()
    End Sub
#End Region

#Region " DataBind "
    Private Sub SetObject()
        Dim chkSelect As CheckBox
        For I As Integer = 0 To dgOrderItemList.Items.Count - 1
            chkSelect = CType(dgOrderItemList.Items(I).FindControl("chkSelect"), CheckBox)
            mQuotationItems.Item(I).IsSelect = chkSelect.Checked
            mQuotationItems.Item(I).MarkClean()
        Next
        Session("mQuotationItems") = mQuotationItems
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("Sender") = "" Then
            FindNow()
        End If
        lblResult.Text = "List of Quotation Items as per criteria: " & mQuotationItems.Count & " Record(s) found."
        SetSession()
        If txtPartNumber.Enabled = True Then
            SetFocus(txtPartNumber)
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SetObject()
        Session("AddPart") = "True"
        'If Session("QuotationApprovalList") = "True" Then
        '    Session("QuotationApprovalList") = "False"
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        'Else
        '  Response.Redirect(Request.QueryString("BackPage"))
        '  End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ''Session.Remove("mQuotationItems")
        'If Session("StoreApprovalList") = "True" Then
        '    Session("StoreApprovalList") = "False"
        Session("AddPart") = "False"
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        ' Else
        '  Session("AddRequisitionParts") = "False"
        '  Session("AddPart") = "False"
        '  Response.Redirect(Request.QueryString("BackPage"))
        '  End If
    End Sub
    Private Sub dgOrderItemList_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgOrderItemList.PageIndexChanged
        dgOrderItemList.CurrentPageIndex = e.NewPageIndex
        dgOrderItemList.DataSource = mQuotationItems
        dgOrderItemList.DataBind()
    End Sub
#End Region

End Class
