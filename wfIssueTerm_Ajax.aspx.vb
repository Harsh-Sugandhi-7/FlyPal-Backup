'Ajax Conversion by vikrant

Public Class wfIssueTerm_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mTerms As Terms
    Public mIssue As Issue
    'Dim Type As Int16 2 For Issue
#End Region

#Region " Business Properties "
    Private Sub GetSession()
        mTerms = Session("mTerms")
        mIssue = Session("mIssue")
    End Sub
    Private Sub SetSession()
        Session("mTerms") = mTerms
        Session("mIssue") = mIssue
    End Sub
    Private Sub setTerms()
        Dim i As Integer
        While i < mTerms.Count
            If mIssue.IssueTerms.Contains(mTerms.Item(i).ID) = True Then
                mTerms.Item(i).IsSelected = True
            Else
                mTerms.Item(i).IsSelected = False
            End If
            i = i + 1
        End While
    End Sub
    Private Sub DataFieldBind()
        'Type = Request.QueryString("Type")
        mTerms = Terms.GetTerms(mIssue.ID, 2)
        setTerms()
        dgTerm.DataSource = mTerms
        dgTerm.DataBind()
    End Sub
    Private Sub setSelectedTerms()
        Dim chkBox As CheckBox
        Dim Recordno As Integer
        Dim i As Integer
        ' Set Selected Notes value  
        For i = 0 To dgTerm.Rows.Count - 1
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
                    If mIssue.IssueTerms.Contains(mTerms.Item(i).ID) = False Then
                        mIssue.IssueTerms.Add(mTerms.Item(i).ID)
                        mIssue.IssueTerms.CurrentItem.Terms = mTerms.Item(i).Terms
                        mIssue.IssueTerms.CurrentItem.TermID = mTerms.Item(i).ID
                    End If
                Else
                    mIssue.IssueTerms.Remove(mTerms.Item(i).ID, "")
                End If
            End If
            i = i + 1
        End While
    End Sub
#End Region

#Region " Events "
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Session.Remove("mTerms")
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
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
        Session("mIssue") = mIssue
        Response.Redirect(Request.QueryString("BackPage"))
    End Sub
    'Private Sub imgbtnTerm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnTerm.Click
    '    Response.Redirect("wfTerm.aspx?ChildPage=wfIssueTerm.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&Type=" & Request.QueryString("Type"))
    'End Sub
    Private Sub hdnimgBtnTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnTerm.Click
        DataFieldBind()
        Session("mTerms") = mTerms
        upnlTermDetails.Update()
    End Sub
#End Region

End Class