Partial Class wfTerm
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents valError As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents txt As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents reset As System.Web.UI.WebControls.Button

    Protected WithEvents CustomValidator1 As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents print As System.Web.UI.WebControls.ImageButton
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
    Public mTerm As Term
    Public mTermList As TermList
    Public BackPage As String
    Public Type As Integer
    Public mTransTypeID As Trans
    Public mTransactionList As TransactionList
#End Region

#Region " Enumeration "
    Private Enum Rights
        [New] = 0
        Edit = 1
        Delete = 2
        View = 3
        Print = 4
        Authorized = 5
    End Enum
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTerm = Session("mTerm")
        mTermList = Session("mTermList")
        mTransTypeID = Session("mTransTypeId")
    End Sub
    Private Sub SetSession()
        Session("mTerm") = mTerm
        Session("mTermList") = mTermList
    End Sub
    Private Sub NewRecord()
        ' mTerm = Term.NewTerm
        mTerm = Term.NewTerm(Guid.Empty, Type)
        Session("mTerm") = mTerm
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mTerm = Term.GetTerm(mId, Type)
        Session("mTerm") = mTerm
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type")
        Session("sender") = "Delete"
        msg1.Show()
        mTerm = Term.GetTerm(mId, Type)
        Session("mTerm") = mTerm
    End Sub
    Private Sub setObject()
        mTerm.Terms = txtName.Text
        mTerm.Type = Type
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Delete" Then
                        Try
                            Session("sender") = ""
                            mTerm = Session("mTerm")
                            Term.DeleteTerm(mTerm.ID, Type)
                            Response.Redirect("wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type")
                                msg1.Show()
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                    Response.Redirect("wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0")
                Case MsgBoxResult.OK '' And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0")
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0")
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
            Response.Redirect("wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0")
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetPage()
        If Type = 1 Then
            If mTerm.Terms = "" Then
                lblTitle.Text = "Term [New]"
            Else
                If mTerm.Terms.Length > 30 Then
                    lblTitle.Text = "Term [" + mTerm.Terms.Substring(0, 26) + "..." + "]"
                Else
                    lblTitle.Text = "Term [" + mTerm.Terms + "]"
                End If
            End If
        ElseIf Type = 2 Then
            If mTerm.Terms = "" Then
                lblTitle.Text = "Term [New]"
            Else
                If mTerm.Terms.Length > 30 Then
                    lblTitle.Text = "Term [" + mTerm.Terms.Substring(0, 26) + "..." + "]"
                Else
                    lblTitle.Text = "Term [" + mTerm.Terms + "]"
                End If
            End If
        End If
    End Sub
    Private Function IsInRole(ByVal mRights As Rights) As Boolean
        mTransactionList = TransactionList.GetTransactionList
        ' Session("mTransactionList") = mTransactionList
        Dim strIssue As String = mTransactionList.GetModuleName(mTransTypeID)
        With User
            Select Case mRights
                Case Rights.[New]
                    Return ((.IsInRole("EnquiryNew") And Type = 3) Or (.IsInRole("QuotationNew") And Type = 4) Or _
                            (.IsInRole("PurchaseQuotationNew") And Type = 4) Or (.IsInRole("SalesOrderNew") And Type = 5) Or _
                            (.IsInRole("OrderNew") And Type = 1) Or (.IsInRole(strIssue + "New") And Type = 2) Or _
                            (.IsInRole("SalesInvoiceNew") And Type = 9) Or (.IsInRole("OrderForExchangeNew") And Type = 1) Or _
                            (.IsInRole("PurchaseOrderRepairOverHaulNew") And Type = 1) Or (.IsInRole("PurchaseOrderRentalLeaseNew") And Type = 1))
                Case Rights.Edit
                    Return ((.IsInRole("EnquiryEdit") And Type = 3) Or (.IsInRole("QuotationEdit") And Type = 4) Or _
                            (.IsInRole("PurchaseQuotationEdit") And Type = 4) Or (.IsInRole("SalesOrderEdit") And Type = 5) Or _
                            (.IsInRole("OrderEdit") And Type = 1) Or (.IsInRole(strIssue + "Edit") And Type = 2) Or _
                            (.IsInRole("SalesInvoiceEdit") And Type = 9) Or (.IsInRole("OrderForExchangeEdit") And Type = 1) Or _
                            (.IsInRole("PurchaseOrderRepairOverHaulEdit") And Type = 1) Or (.IsInRole("PurchaseOrderRentalLeaseEdit") And Type = 1))
                Case Rights.Delete
                    Return ((.IsInRole("EnquiryDelete") And Type = 3) Or (.IsInRole("QuotationDelete") And Type = 4) Or _
                            (.IsInRole("PurchaseQuotationDelete") And Type = 4) Or (.IsInRole("SalesOrderDelete") And Type = 5) Or _
                            (.IsInRole("OrderDelete") And Type = 1) Or (.IsInRole(strIssue + "Delete") And Type = 2) Or _
                            (.IsInRole("SalesInvoiceDelete") And Type = 9) Or (.IsInRole("OrderForExchangeDelete") And Type = 1) Or _
                            (.IsInRole("PurchaseOrderRepairOverHaulDelete") And Type = 1) Or (.IsInRole("PurchaseOrderRentalLeaseDelete") And Type = 1))
                Case Rights.View
                    Return ((.IsInRole("EnquiryView") And Type = 3) Or (.IsInRole("QuotationView") And Type = 4) Or _
                            (.IsInRole("PurchaseQuotationView") And Type = 4) Or (.IsInRole("SalesOrderView") And Type = 5) Or _
                            (.IsInRole("OrderView") And Type = 1) Or (.IsInRole(strIssue + "View") And Type = 2) Or _
                            (.IsInRole("SalesInvoiceView") And Type = 9) Or (.IsInRole("OrderForExchangeView") And Type = 1) Or _
                            (.IsInRole("PurchaseOrderRepairOverHaulView") And Type = 1) Or (.IsInRole("PurchaseOrderRentalLeaseView") And Type = 1))
                Case Rights.Print
                    Return ((.IsInRole("EnquiryPrint") And Type = 3) Or (.IsInRole("QuotationPrint") And Type = 4) Or _
                            (.IsInRole("PurchaseQuotationPrint") And Type = 4) Or (.IsInRole("SalesOrderPrint") And Type = 5) Or _
                            (.IsInRole("OrderPrint") And Type = 1) Or (.IsInRole(strIssue + "Print") And Type = 2) Or _
                            (.IsInRole("SalesInvoicePrint") And Type = 9) Or (.IsInRole("OrderForExchangePrint") And Type = 1) Or _
                            (.IsInRole("PurchaseOrderRepairOverHaulPrint") And Type = 1) Or (.IsInRole("PurchaseOrderRentalLeasePrint") And Type = 1))
                Case Rights.Authorized
                    Return ((.IsInRole("EnquiryAuthorized") And Type = 3) Or (.IsInRole("QuotationAuthorized") And Type = 4) Or _
                            (.IsInRole("PurchaseQuotationAuthorized") And Type = 4) Or (.IsInRole("SalesOrderAuthorized") And Type = 5) Or _
                            (.IsInRole("OrderAuthorized") And Type = 1) Or (.IsInRole(strIssue + "Authorized") And Type = 2) Or _
                            (.IsInRole("SalesInvoiceAuthorized") And Type = 9) Or (.IsInRole("OrderForExchangeAuthorized") And Type = 1) Or _
                            (.IsInRole("PurchaseOrderRepairOverHaulAuthorized") And Type = 1) Or (.IsInRole("PurchaseOrderRentalLeaseAuthorized") And Type = 1))
            End Select
        End With
    End Function
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTermList = TermList.GetTermList("", Type)
        dgTerm.DataSource = mTermList
        Session("mTermList") = mTermList
        ''mTransactionList = TransactionList.GetTransactionList
        ''Session("mTransactionList") = mTransactionList
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtName" Then
            If Len(txtName.Text) > 500 Then
                custValidator.ErrorMessage = "Term must not  be greater than 500 characters."
                txtName.Text = txtName.Text.Trim.Substring(0, 500) + "..."
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        Type = Request.QueryString("Type")
        BackPage = Request.QueryString("BackPage")
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        If Not IsPostBack And Session("sender") = "" Then
            NewRecord()
            DataFieldBind()
        Else
            dgTerm.DataSource = mTermList
            dgTerm.DataBind()
        End If
        SetPage()
        MessageBoxResult()
        If mTermList.Count > 25 Then
            btnBackTop.Visible = True
        Else
            btnBackTop.Visible = False
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click, btnBackTop.Click
        Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Commented by Prashant 01-July-2011
        'If (Not IsInRole(Rights.[New]) And mTerm.IsNew) Or (Not IsInRole(Rights.Edit) And Not mTerm.IsNew) Then
        '    setObject()
        '    SetSession()
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0"
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        Try
            If IsValid() Then
                setObject()
                mTerm.Save()
                mTerm = Term.NewTerm
                DataFieldBind()
                SetSession()
                If Type = 1 Then
                    lblTitle.Text = "Term [New]"
                ElseIf Type = 2 Then
                    lblTitle.Text = "Term [New]"
                End If
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 2627 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type")
                Session("sender") = "Delete"
                msg1.Show()
            ElseIf ex.Number = 547 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type")
                Session("sender") = "Delete"
                msg1.Show()
            End If
        End Try
    End Sub
    Private Sub dgTerm_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgTerm.ItemCommand
        If e.Item.Cells(0).Text = "TermID" Or e.Item.Cells(0).Text = "" Then Exit Sub
        Dim mId As Guid = New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "Edit"
                'Commented by Prashant 01-July-2011
                'If (Not IsInRole(Rights.View) And Not IsInRole(Rights.Edit)) Then
                '    setObject()
                '    SetSession()
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0"
                '    Session("sender") = "Authorization"
                '    msg.Show()
                '    Exit Sub
                'End If
                EditRecord(mId)
                txtName.DataBind()
                setFocus(txtName)
                If Len(mTerm.Terms.Trim) > 30 Then
                    lblTitle.Text = "Term [" & mTerm.Terms.Trim.Substring(0, 26) + "..." & "]"
                Else
                    lblTitle.Text = "Term [" & mTerm.Terms & "]"
                End If
                'If Type = 1 Then
                '    lblTitle.Text = "Term [" & IIf(Len(mTerm.Terms.Trim) > 30, mTerm.Terms.Trim.Substring(0, 26) + "...", mTerm.Terms) & "]"
                'ElseIf Type = 2 Then
                '    lblTitle.Text = "Term [" & IIf(Len(mTerm.Terms.Trim) > 30, mTerm.Terms.Trim.Substring(0, 26) + "...", mTerm.Terms) & "]"
                'End If
            Case "Delete"
                'Commented by Prashant 01-July-2011
                'If (Not IsInRole(Rights.Delete)) Then
                '    setObject()
                '    SetSession()
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfTerm.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type") & "&MsgResult=0"
                '    Session("sender") = "Authorization"
                '    msg.Show()
                '    Exit Sub
                'End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        setFocus(txtName)
        NewRecord()
        txtName.Text = ""
        DataFieldBind()
        lblTitle.Text = "Term [New]"
    End Sub
    Private Sub dgTerm_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgTerm.SortCommand
        mTermList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mTermList") = mTermList
        dgTerm.DataSource = mTermList
        dgTerm.DataBind()
    End Sub
#End Region
End Class
