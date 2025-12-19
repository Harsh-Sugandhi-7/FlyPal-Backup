Public Class wfSelectPeriod_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mSelectPeriods As SelectPeriods = SelectPeriods.NewSelectPeriods
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mSelectPeriods = Session("mSelectPeriods")
    End Sub
    Private Sub SetSession()
        Session("mSelectPeriods") = mSelectPeriods
    End Sub
    Private Sub AddPeroids()
        Dim chkBox As CheckBox
        Dim Recordno, PageItems As Integer
        Dim i As Integer
        PageItems = dgSelectPeriod.Rows.Count - 1
        For i = 0 To PageItems
            Recordno = i + dgSelectPeriod.PageSize * dgSelectPeriod.PageIndex
            chkBox = CType(dgSelectPeriod.Rows(i).FindControl("chkSelect"), CheckBox)
            mSelectPeriods(Recordno).IsSelected = chkBox.Checked
            mSelectPeriods(Recordno).MarkClean()
        Next
         SetSession()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgSelectPeriod.DataSource = mSelectPeriods
        dgSelectPeriod.DataBind()
        Session("mSelectPeriods") = mSelectPeriods
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDone.Click
        AddPeroids()

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            'Session.Remove("MiddleFrame")
            Session.Remove("Sender")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        If (Not Session("wfWODetail.WO") Is Nothing) And Session("Sender") = "wfWODetail.aspx" Then
            Response.Redirect(BackPage.Pop(Session("BackPage")))
            Session.Remove("Sender")
        ElseIf Request.QueryString("BackPage1") = "wfnWODetail_AJAX.aspx" Then
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
        Else
            Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
        End If
    End Sub
#End Region

End Class