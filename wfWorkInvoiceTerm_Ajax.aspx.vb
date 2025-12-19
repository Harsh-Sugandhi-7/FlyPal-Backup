Public Class wfWorkInvoiceTerm_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mTerms As Terms
    Public mWorkInvoice As WorkInvoice
#End Region

#Region " Business Properties "
    Private Sub GetSession()
        mTerms = Session("mTerms")
        mWorkInvoice = Session("mWorkInvoice")
    End Sub
    Private Sub SetSession()
        Session("mTerms") = mTerms
        Session("mWorkInvoice") = mWorkInvoice
    End Sub
    Private Sub setTerms()
        Dim i As Integer
        While i < mTerms.Count
            If mWorkInvoice.WorkInvoiceTerms.Contains(mTerms.Item(i).ID, "") = True Then
                mTerms.Item(i).IsSelected = True
            Else
                mTerms.Item(i).IsSelected = False
            End If
            i = i + 1
        End While
    End Sub
    Private Sub DataFieldBind()
        mTerms = Terms.GetTerms(mWorkInvoice.ID, 6)
        setTerms()
        dgTerm.DataSource = mTerms
        dgTerm.DataBind()
    End Sub
    Private Sub SetSelectedTerms()
        Dim chkBox As CheckBox
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgTerm.Rows.Count - 1
        For i = 0 To PageItems
            Recordno = i + dgTerm.PageSize * dgTerm.PageIndex
            chkBox = CType(dgTerm.Rows(i).FindControl("chkSelect"), CheckBox)
            mTerms(Recordno).IsSelected = chkBox.Checked
        Next
        Session("mTerms") = mTerms
    End Sub
    Private Sub setObject()
        Dim i As Integer = 0
        While i < mTerms.Count
            If mTerms.Item(i).IsDirty = True Then
                If mTerms.Item(i).IsSelected = True Then
                    If mWorkInvoice.WorkInvoiceTerms.Contains(mTerms.Item(i).ID, "") = False Then
                        mWorkInvoice.WorkInvoiceTerms.Add(mWorkInvoice.ID)
                        mWorkInvoice.WorkInvoiceTerms.CurrentItem.Terms = mTerms.Item(i).Terms
                        mWorkInvoice.WorkInvoiceTerms.CurrentItem.TermID = mTerms.Item(i).ID
                    End If
                Else
                    mWorkInvoice.WorkInvoiceTerms.Remove(mTerms.Item(i).ID, "")
                End If
            End If
            i = i + 1
        End While
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            SetSession()
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        setSelectedTerms()
        setObject()
        Session("mWorkInvoice") = mWorkInvoice
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    Private Sub hdnimgBtnTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnTerm.Click
        DataFieldBind()
        Session("mTerms") = mTerms
    End Sub
#End Region

End Class