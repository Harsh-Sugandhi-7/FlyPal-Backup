Imports Microsoft.VisualBasic.ApplicationServices

'16-Feb-2024 Concurrent User implementation by Kalpesh
'
Public Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim a As New UserLoginSession
        a.DeleteUserLoginSession(New Guid(Session("UserId").ToString), New Guid(Session("LoginSession").ToString))

        Web.Security.FormsAuthentication.SignOut()
        HttpContext.Current.Session.Abandon()

        Thread.CurrentPrincipal = Nothing
        Session.Remove("MenuID")
        Session.Remove("MiddleFrame")
        'Server.Transfer("Login.aspx")
        MarkLog(Util.Action.Logoff)
        'Drop all the references to the Principal.
        Thread.CurrentPrincipal = Nothing
        Dim str As String

        Dim str1 As String
        str1 = "delete_cookie();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str1, True)


        str = "<script language=javascript>  window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenPageScript", str)
        Session.Remove("ReminderFired")

    End Sub

End Class