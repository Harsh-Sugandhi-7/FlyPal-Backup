Public Class wfCustomerTermList_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mCustomerTerms As CustomerTerms
    Public mCustomerContract As CustomerContract
    Dim Type As Int16
#End Region

#Region " Business Properties "
    Private Sub GetSession()
        mCustomerTerms = Session("mCustomerTerms")
        mCustomerContract = Session("mCustomerContract")
    End Sub
    Private Sub SetSession()
        Session("mCustomerTerms") = mCustomerTerms
        Session("mCustomerContract") = mCustomerContract
    End Sub
    Private Sub setCustomerTerms()
        Dim i As Integer
        While i < mCustomerTerms.Count
            If mCustomerContract.CustomerContractTerms.Contains(mCustomerTerms.Item(i).ID) = True Then
                mCustomerTerms.Item(i).IsSelected = True
            Else
                mCustomerTerms.Item(i).IsSelected = False
            End If
            i = i + 1
        End While
    End Sub
    Private Sub DataFieldBind()
       mCustomerTerms = CustomerTerms.GetCustomerTerms(mCustomerContract.ID, 0)
        ''mCustomerTerms = CustomerTerms.GetCustomerTerms(Guid.Empty, 0)
        setCustomerTerms()
        dgTerm.DataSource = mCustomerTerms
        dgTerm.DataBind()
    End Sub
    Private Sub SetSelectedCustomerTerms()
        Dim chkBox As CheckBox
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgTerm.Rows.Count - 1
        For i = 0 To PageItems
            Recordno = i + dgTerm.PageSize * dgTerm.PageIndex
            chkBox = CType(dgTerm.Rows(i).FindControl("chkSelect"), CheckBox)
            mCustomerTerms(Recordno).IsSelected = chkBox.Checked
        Next
        Session("mCustomerTerms") = mCustomerTerms
    End Sub
    Private Sub setObject()
        Dim i As Integer = 0
        While i < mCustomerTerms.Count
            If mCustomerTerms.Item(i).IsDirty = True Then
                If mCustomerTerms.Item(i).IsSelected = True Then
                    If mCustomerContract.CustomerContractTerms.Contains(mCustomerTerms.Item(i).ID) = False Then
                        mCustomerContract.CustomerContractTerms.Add(mCustomerTerms.Item(i).ID)
                        mCustomerContract.CustomerContractTerms.CurrentItem.Terms = mCustomerTerms.Item(i).Terms
                        mCustomerContract.CustomerContractTerms.CurrentItem.CustomerTermID = mCustomerTerms.Item(i).ID
                    End If
                Else
                    mCustomerContract.CustomerContractTerms.Remove(mCustomerTerms.Item(i).ID, "")
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
        dgTerm.DataSource = mCustomerTerms
        Session("mCustomerTerms") = mCustomerTerms
        dgTerm.DataBind()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        SetSelectedCustomerTerms()
        setObject()
        Session("mCustomerContract") = mCustomerContract
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
     End Sub
    Private Sub hdnimgBtnTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnTerm.Click
        DataFieldBind()
        Session("mCustomerTerms") = mCustomerTerms
        upnlTerm.Update()
    End Sub
#End Region

End Class