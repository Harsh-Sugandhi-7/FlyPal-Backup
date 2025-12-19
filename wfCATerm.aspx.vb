Public Class wfCATerm
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mTerms As Terms
    Public mEmpCAAuthorization As EmpCAAuthorization
    Dim Type As Int16
#End Region

#Region " Business Properties "
    Private Sub GetSession()
        mTerms = Session("mTerms")
        mEmpCAAuthorization = Session("mEmpCAAuthorization")
    End Sub
    Private Sub SetSession()
        Session("mTerms") = mTerms
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
    Private Sub setTerms()
        Dim i As Integer
        While i < mTerms.Count
            If mEmpCAAuthorization.EmpCAAuthorizationTerms.Contains(mTerms.Item(i).ID) = True Then
                mTerms.Item(i).IsSelected = True
            Else
                mTerms.Item(i).IsSelected = False
            End If
            i = i + 1
        End While
    End Sub
    Private Sub DataFieldBind()
        'Type = Request.QueryString("Type")
        mTerms = Terms.GetTerms(mEmpCAAuthorization.ID, 11)
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
                    If mEmpCAAuthorization.EmpCAAuthorizationTerms.Contains(mTerms.Item(i).ID) = False Then
                        mEmpCAAuthorization.EmpCAAuthorizationTerms.Add(mTerms.Item(i).ID)
                        mEmpCAAuthorization.EmpCAAuthorizationTerms.CurrentItem.Terms = mTerms.Item(i).Terms
                        mEmpCAAuthorization.EmpCAAuthorizationTerms.CurrentItem.TermID = mTerms.Item(i).ID
                    End If
                Else
                    mEmpCAAuthorization.EmpCAAuthorizationTerms.Remove(mTerms.Item(i).ID, "")
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
        SetSelectedTerms()
        setObject()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        ''Response.Redirect(Request.QueryString("BackPage"))



        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

    End Sub
    Private Sub hdnimgBtnTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnTerm.Click
        'DataFieldBind()
        mTerms = Terms.GetTerms(mEmpCAAuthorization.ID, 11)
        dgTerm.DataSource = mTerms
        setTerms()
        dgTerm.DataBind()

        upnlTerm.Update()
        Session("mTerms") = mTerms
    End Sub
#End Region


End Class