Public Class wfOrderTerm_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mTerms As Terms
    Public mOrder As Order
    Dim Type As Int16
#End Region

#Region " Business Properties "
    Private Sub GetSession()
        mTerms = Session("mTerms")
        mOrder = Session("mOrder")
    End Sub
    Private Sub SetSession()
        Session("mTerms") = mTerms
        Session("mOrder") = mOrder
    End Sub
    Private Sub setTerms()
        Dim i As Integer
        While i < mTerms.Count
            If mOrder.OrderTerms.Contains(mTerms.Item(i).ID) = True Then
                mTerms.Item(i).IsSelected = True
            Else
                mTerms.Item(i).IsSelected = False
            End If
            i = i + 1
        End While
    End Sub
    Private Sub DataFieldBind()
        'Type = Request.QueryString("Type")
        mTerms = Terms.GetTerms(mOrder.ID, 1)
        setTerms()
        dgTerm.DataSource = mTerms
        If AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ" Then ' SPZ Code added by Saylee on 13-Jun-2022 
            dgTerm.AllowPaging = False
        End If
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
                    If mOrder.OrderTerms.Contains(mTerms.Item(i).ID) = False Then
                        mOrder.OrderTerms.Add(mTerms.Item(i).ID)
                        mOrder.OrderTerms.CurrentItem.Terms = mTerms.Item(i).Terms
                        mOrder.OrderTerms.CurrentItem.TermID = mTerms.Item(i).ID
                    End If
                Else
                    mOrder.OrderTerms.Remove(mTerms.Item(i).ID, "")
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
    Private Sub dgTerm_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTerm.PageIndexChanging
        dgTerm.PageIndex = e.NewPageIndex
        dgTerm.DataSource = mTerms
        Session("mTerms") = mTerms
        dgTerm.DataBind()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        setSelectedTerms()
        setObject()
        Session("mOrder") = mOrder
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    'Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
    '    Session.Remove("mTerms")
    '    Response.Redirect(Request.QueryString("BackPage"))
    'End Sub
    Private Sub hdnimgBtnTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnTerm.Click
        DataFieldBind()
        Session("mTerms") = mTerms
    End Sub
#End Region

End Class