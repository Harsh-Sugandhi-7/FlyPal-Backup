Partial Class wfStoreApprovalList
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
    Public mRequisitionItem As RequisitionItem
    Public mRequisitionItems As RequisitionItems
    Public mTransDate As String
    Public mEnquiryItemID As Guid
    Public mQuotationItemID As Guid
    Public mListFor As Integer
    Public mCustomerID As Guid
    Public mTransTypeID As Integer
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRequisitionItems = CType(Session("mRequisitionItems"), RequisitionItems)
        mRequisitionItem = CType(Session("mRequisitionItem"), RequisitionItem)
        mTransDate = Session("TransDate")
        mEnquiryItemID = Session("EnquiryItem")
        mListFor = Session("ListFor")
        mQuotationItemID = Session("QuotationItem")
        mCustomerID = Session("CustomerID")
        mTransTypeID = Session("TransTypeID")
    End Sub
    Private Sub SetSession()
        Session("mRequisitionItems") = mRequisitionItems
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'> document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "FocusScript", str)
    End Sub
    Private Sub FindNow()
        dgRequisitionItemList.CurrentPageIndex = 0
        If mListFor = 0 Then
            mRequisitionItems = RequisitionItems.GetRequisitionItems(Requisition.RequisitionLevel.ForStoreValidation, mTransDate, txtPartNumber.Text, mEnquiryItemID, mListFor, mTransTypeID, mCustomerID.ToString)
            dgRequisitionItemList.Columns(10).Visible = True
            dgRequisitionItemList.Columns(11).Visible = False
        ElseIf mListFor = 1 Then
            mRequisitionItems = RequisitionItems.GetRequisitionItems(Requisition.RequisitionLevel.ForStoreValidation, mTransDate, txtPartNumber.Text, mQuotationItemID, mListFor, mTransTypeID, mCustomerID.ToString)
            dgRequisitionItemList.Columns(10).Visible = False
            dgRequisitionItemList.Columns(11).Visible = True
        End If
        dgRequisitionItemList.DataSource = mRequisitionItems
        Session("mRequisitionItems") = mRequisitionItems
        DataBind()
        lblResult.Text = "List of Requisition Items as per criteria: " & mRequisitionItems.Count & " Record(s) found."
    End Sub
#End Region

#Region " DataBind "
    Private Sub SetObject()
        Dim chkSelect As CheckBox
        Dim item As DataGridItem
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgRequisitionItemList.Items.Count - 1
        ' Set Selected Notes value  
        For i = 0 To PageItems
            Recordno = i + dgRequisitionItemList.PageSize * dgRequisitionItemList.CurrentPageIndex
            item = dgRequisitionItemList.Items(i)
            chkSelect = CType(item.FindControl("chkSelect"), CheckBox)
            mRequisitionItems(Recordno).IsSelect = chkSelect.Checked
            mRequisitionItems(Recordno).MarkClean()
        Next
        Session("mRequisitionItems") = mRequisitionItems
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("Sender") = "" Then
            If txtPartNumber.Enabled = True Then
                SetFocus(txtPartNumber)
            End If
            FindNow()
        End If
        lblResult.Text = "List of Requisition Items as per criteria: " & mRequisitionItems.Count & " Record(s) found."
        SetSession()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SetObject()
        Session("AddRequisitionParts") = "True"
        Session("AddPart") = "True"
        If Session("StoreApprovalList") = "True" Then
            Session("StoreApprovalList") = "False"
            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        Else
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mRequisitionItems")
        If Session("StoreApprovalList") = "True" Then
            Session("StoreApprovalList") = "False"
            Session("AddPart") = "False"
            Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
        Else
            Session("AddRequisitionParts") = "False"
            Session("AddPart") = "False"
            Response.Redirect(Request.QueryString("BackPage"))
        End If
    End Sub
    Private Sub dgRequisitionItemList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRequisitionItemList.PageIndexChanged
        SetObject()
        dgRequisitionItemList.CurrentPageIndex = e.NewPageIndex
        dgRequisitionItemList.DataSource = mRequisitionItems
        dgRequisitionItemList.DataBind()
    End Sub
    'Added By Prashant 18-June-2009
    Private Sub dgRequisitionItemList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgRequisitionItemList.SortCommand
        mRequisitionItems.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRequisitionItems") = mRequisitionItems
        dgRequisitionItemList.DataSource = mRequisitionItems
        dgRequisitionItemList.DataBind()
    End Sub
    '------------------------------
#End Region

End Class
