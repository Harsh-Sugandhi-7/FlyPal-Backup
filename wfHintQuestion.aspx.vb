Public Class wfHintQuestion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            For i As Integer = 0 To BulletedList1.Items.Count - 1
                If i / 2 = 0 Then
                    BulletedList1.Items(i).Attributes("syle") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-color:rgb(128,0,64);background-color:white;"
                Else
                    BulletedList1.Items(i).Attributes("syle") = "border-top-style:none; border-bottom-style:solid; border-left-style:none; border-right-style:none; border-color:rgb(128,0,64);background-color: #b6dad8;"
                End If

            Next
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
End Class