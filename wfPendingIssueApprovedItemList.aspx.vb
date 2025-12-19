Partial Class wfPendingIssueApprovedItemList
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
    Public mItemID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRequisitionItems = CType(Session("mRequisitionItems"), RequisitionItems)
        mRequisitionItem = CType(Session("mRequisitionItem"), RequisitionItem)
        mTransDate = Session("TransDate")
        mItemID = Session("ItemID")
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
        mRequisitionItems = RequisitionItems.GetRequisitionItems(Requisition.RequisitionLevel.ForEngIssueApproval, mTransDate, txtPartNumber.Text, mItemID, 0)
        dgPendingIssueApprovedItemList.DataSource = mRequisitionItems
        Session("mRequisitionItems") = mRequisitionItems
        DataBind()
        lblResult.Text = "Issue Approved Requisition part List: " & mRequisitionItems.Count & " Record(s) found."
    End Sub
#End Region

#Region " DataBind "
    Private Sub SetObject()
        Dim chkSelect As CheckBox
        For I As Integer = 0 To dgPendingIssueApprovedItemList.Items.Count - 1
            chkSelect = CType(dgPendingIssueApprovedItemList.Items(I).FindControl("chkSelect"), CheckBox)
            mRequisitionItems.Item(I).IsSelect = chkSelect.Checked
            mRequisitionItems.Item(I).MarkClean()
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
        lblResult.Text = "Issue Approved Requisition part List: " & mRequisitionItems.Count & " Record(s) found."
        SetSession()
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        FindNow()
    End Sub
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SetObject()
        Session("CheckQty") = "True"
        Session("AddRequisitionPart") = "True"
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mRequisitionItems")
        Session("StoreApprovalList") = "False"
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
    End Sub
    Private Sub dgPendingIssueApprovedItemList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPendingIssueApprovedItemList.PageIndexChanged
        dgPendingIssueApprovedItemList.CurrentPageIndex = e.NewPageIndex
        dgPendingIssueApprovedItemList.DataSource = mRequisitionItems
        dgPendingIssueApprovedItemList.DataBind()
    End Sub
#End Region

End Class
