Public Class wfSelectPeriodUnit_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declarations "
    Public mSelectPeriodUnits As SelectPeriodUnits
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
    End Sub
    Private Sub SetSession()
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub AddPeroidUnits()
        Dim chkBox As CheckBox
        Dim Recordno As Integer
        Dim i As Integer
        ' Set Selected Notes value  
        For i = 0 To dgSelectPeriodUnits.Rows.Count - 1
            Recordno = i + dgSelectPeriodUnits.PageSize * dgSelectPeriodUnits.PageIndex
            chkBox = CType(dgSelectPeriodUnits.Rows(i).FindControl("chkSelect"), CheckBox)
            mSelectPeriodUnits(Recordno).IsSelected = chkBox.Checked
        Next
        SetSession()
    End Sub
#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()
        dgSelectPeriodUnits.DataSource = mSelectPeriodUnits
        dgSelectPeriodUnits.DataBind()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        AddPeroidUnits()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            'Session.Remove("MiddleFrame")
            Session.Remove("Sender")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect(Request.QueryString("BackPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6"))
    End Sub
#End Region

End Class