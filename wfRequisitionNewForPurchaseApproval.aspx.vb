Partial Class wfRequisitionNewForPurchaseApproval
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

#Region " Variable Declaration "
    Public mRequisitionItemListNewForPurchaseApproval As RequisitionItemListNewForPurchaseApproval
    Public mRequisitionNewForPurchaseApprovals As RequisitionNewForPurchaseApprovals
    Public Index As Integer
    Public mInvoiceItemListForFinalApproval As InvoiceItemListForFinanceApproval

    Public mQuotationItems As QuotationItems
    Public mQuotationItemsAsPerItem As QuotationItems

    Dim EventLogID As Guid
    Dim mRequisitionForStoreValidation As String
#End Region

#Region " Enum "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mRequisitionItemListNewForPurchaseApproval = Session("mRequisitionItemListNewForPurchaseApproval")
        mRequisitionNewForPurchaseApprovals = Session("mRequisitionNewForPurchaseApprovals")
        mQuotationItems = Session("mQuotationItems")
        Index = CInt(Session("Index"))
        mQuotationItemsAsPerItem = Session("mQuotationItemsAsPerItem")
    End Sub
    Private Sub setSession()
        Session("mRequisitionItemListNewForPurchaseApproval") = mRequisitionItemListNewForPurchaseApproval
        Session("mRequisitionNewForPurchaseApprovals") = mRequisitionNewForPurchaseApprovals
        Session("mQuotationItemsAsPerItem") = mQuotationItemsAsPerItem
        Session("mQuotationItems") = mQuotationItems
    End Sub
    Private Sub setObject()
        Dim cmbValue As DropDownList
        Dim i As Integer = 0

        Dim mRequisitionNewForPurchaseApproval As RequisitionNewForPurchaseApproval
        For Each mRequisitionNewForPurchaseApproval In mRequisitionNewForPurchaseApprovals
            cmbValue = CType(Me.dgRequisitionList.Items(i).FindControl("cmbAppQuotation"), DropDownList)
            mRequisitionNewForPurchaseApproval.QuotationItemID = New Guid(cmbValue.SelectedValue)
            i = i + 1
        Next
    End Sub
    Private Function IsSelectedIndex() As Boolean
        Dim i As Integer = 0
        Dim cmbValue As DropDownList
        For i = 0 To dgRequisitionList.Items.Count - 1
            cmbValue = CType(dgRequisitionList.Items(i).FindControl("cmbAppQuotation"), DropDownList)
            If cmbValue.SelectedIndex > 0 Then
                Return True
                Exit Function
            Else
                Return False
            End If
        Next
    End Function
    Private Sub SetPage()
        lblTitle.Text = "Requisition For Purchase Approval"
    End Sub
    Private Sub Save()
        Dim ItemwiseRequisitionDetailsClone As RequisitionNewForPurchaseApprovals
        ItemwiseRequisitionDetailsClone = mRequisitionNewForPurchaseApprovals.Clone
        Try
            If Not mRequisitionNewForPurchaseApprovals.Count = 0 Then
                setObject()
                mRequisitionNewForPurchaseApprovals.Save()
                mRequisitionForStoreValidation = "Requisition No. : " + dgRequisitionList.Items(0).Cells(3).Text + " Part No. : " + txtPartNo.Text + " Requested Qty. : " + txtTotReqQty.Text + mRequisitionNewForPurchaseApprovals(0).QuotationDetail
                MarkLog(Util.Action.Save, "Requisition Approval", mRequisitionForStoreValidation, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                Session("mRequisitionNewForPurchaseApprovals") = mRequisitionNewForPurchaseApprovals
                Response.Redirect("Index.aspx?BackPage=" & Request.QueryString("BackPage"))
            End If
        Catch ex As SqlClient.SqlException
            Session("ItemwiseRequisitionDetailsClone") = ItemwiseRequisitionDetailsClone
        Finally
            ItemwiseRequisitionDetailsClone = Nothing
        End Try
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "RequisitionNewForPurchaseApproval"

        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
        End Select
    End Function
    Private Sub SetGrid()
        Dim item As DataGridItem
        Dim msize As Integer
        Dim linkbutton As LinkButton
        For i As Integer = 0 To dgReqQuotes.Items.Count - 1
            item = dgReqQuotes.Items(i)
            msize = CType(item.Cells(13).Text, Int32)
            linkbutton = item.Cells(12).FindControl("LinkButton1")
            If msize <= 0 Then
                linkbutton.Enabled = False
            End If
        Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        txtPartNo.Text = mRequisitionItemListNewForPurchaseApproval(Index).PartNo
        txtDescription.Text = mRequisitionItemListNewForPurchaseApproval(Index).Description
        txtTotReqQty.Text = Format(mRequisitionItemListNewForPurchaseApproval(Index).RequestedQty, "##0.##")

        mRequisitionNewForPurchaseApprovals = RequisitionNewForPurchaseApprovals.GetRequisitionNewForPurchaseApprovals(mRequisitionItemListNewForPurchaseApproval(Index).ReqItemID)
        dgRequisitionList.DataSource = mRequisitionNewForPurchaseApprovals

        Session("mRequisitionNewForPurchaseApprovals") = mRequisitionNewForPurchaseApprovals

        Dim mItemID As Guid
        mItemID = mRequisitionItemListNewForPurchaseApproval(Index).ItemID
        'Last 10 Purchases
        mInvoiceItemListForFinalApproval = InvoiceItemListForFinanceApproval.GetInvoiceItemListForFinalApprovalList(mItemID)
        dgInvoiceItemList.DataSource = mInvoiceItemListForFinalApproval
        Session("mInvoiceItemListForFinalApproval") = mInvoiceItemListForFinalApproval
        '==========
        If Not mRequisitionNewForPurchaseApprovals.Count = 0 Then
            mQuotationItemsAsPerItem = QuotationItems.GetQuotationItems(Today.Date.ToString, Guid.Empty, mRequisitionItemListNewForPurchaseApproval(Index).ItemID, "", 0, "")
            dgReqQuotes.DataSource = mQuotationItemsAsPerItem
            Session("mQuotationItemsAsPerItem") = mQuotationItemsAsPerItem
        End If

        Dim i As Integer
        For i = 0 To mRequisitionNewForPurchaseApprovals.Count - 1
            mQuotationItems = QuotationItems.GetQuotationItems(Today.Date.ToString, mRequisitionItemListNewForPurchaseApproval(Index).ReqItemID, mRequisitionItemListNewForPurchaseApproval(Index).ItemID, "", 0, "<SELECT>")
            dgRequisitionList.DataBind()
            Dim cmbValue As DropDownList
            cmbValue = CType(Me.dgRequisitionList.Items(i).FindControl("cmbAppQuotation"), DropDownList)
            cmbValue.DataSource = mQuotationItems
        Next
        Session("mQuotationItems") = mQuotationItems

        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim customValidator As CustomValidator
        customValidator = CType(s, CustomValidator)
        If customValidator.ControlToValidate = "txtPartNo" Then
            If IsSelectedIndex() = True Then
                e.IsValid = True
            Else
                customValidator.ErrorMessage = "Select quotation for approval"
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        End If
        SetPage()
        SetGrid()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New])) Or (Not IsInRole(Rights.Edit)) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            Exit Sub
        End If
        If IsValid Then
            Save()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'mRequisitionForStoreValidation = "Part No. : " + txtPartNo.Text + " Requested Qty. : " + txtTotReqQty.Text
        MarkLog(Util.Action.Close, "Requisition Approval", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("IsValid") = IsValid
        Session.Remove("Index")
        mQuotationItems = Nothing
        Response.Redirect("Index.aspx")
    End Sub
    Private Sub dgReqQuotes_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReqQuotes.ItemCommand
        If e.CommandName = "Select" Then
            Dim mQuotationAttachmentList As QuotationAttachmentList
            Dim Index1 As Integer = e.Item.ItemIndex + dgReqQuotes.PageSize * dgReqQuotes.CurrentPageIndex

            mQuotationAttachmentList = QuotationAttachmentList.GetQuotationAttachmentList(mRequisitionItemListNewForPurchaseApproval(Index).ItemID)
            If mQuotationAttachmentList.Count > 0 Then
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                Dim path As String = AppSettings("DOCPath") & StrName & mQuotationAttachmentList.Item(Index1).Extension

                Dim fs As FileStream
                If File.Exists(AppSettings("DOCPath")) = False Then
                    'Delete File if exist
                    System.IO.File.Delete(AppSettings("DOCPath") & StrName & mQuotationAttachmentList.Item(Index1).Extension)
                    ' Create the file.
                    fs = File.Create(path)
                    '' Add some information to the file.
                    fs.Write(mQuotationAttachmentList.Item(Index1).ImageFile, 0, mQuotationAttachmentList.Item(Index1).ImageFile.Length)
                    fs.Close()
                    Session("DOCPath") = path
                    Dim Str As String
                    Str = "<script language=Javascript>openFile();</script>"
                    ClientScript.RegisterStartupScript(Me.GetType(), "openFilel", Str)
                End If
            End If
        End If
    End Sub
#End Region

End Class
