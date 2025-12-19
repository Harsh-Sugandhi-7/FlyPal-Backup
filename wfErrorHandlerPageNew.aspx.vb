Public Class wfErrorHandlerPageNew
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Try

                Dim objErr As Exception = Session("Error")
                Dim ErrorMessage As String = ""

                ErrorMessage = "Error Date: " + Date.Now.ToString + vbCrLf
                ErrorMessage += "Error Source: " + Request("aspxerrorpath").ToString + vbCrLf
                ErrorMessage += "Error Message: " + objErr.Message.ToString + vbCrLf
                ErrorMessage += "Error Stack: " + objErr.StackTrace.ToString + vbCrLf
                ErrorMessage += "Error Stack: " + "______________________________________________________________________________" + vbCrLf

                WriteError(ErrorMessage)

            Catch ex As Exception

            End Try

        End If

    End Sub

    Protected Sub Login(sender As Object, e As EventArgs) Handles bntLogin.Click

        Web.Security.FormsAuthentication.SignOut()
        Web.Security.FormsAuthentication.RedirectToLoginPage()
        Session.Abandon()
        Session.Remove("MenuID")
        Session.Remove("MiddleFrame")

        MarkLog(Action.Logoff)

        Thread.CurrentPrincipal = Nothing

    End Sub

    Private Sub WriteError(ErrorMessage As String)

        Try

            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim TodayDate As String = Day & Month & Year
            Dim Path As String = Request.PhysicalApplicationPath & "errlog\" & Thread.CurrentPrincipal.Identity.Name & "-" & TodayDate

            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            WriteLine(1, ErrorMessage + vbLf)
            FileClose(1)

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Protected Sub CloseScreen(sender As Object, e As EventArgs) Handles close.Click

        Try

            Web.Security.FormsAuthentication.SignOut()
            Web.Security.FormsAuthentication.RedirectToLoginPage()
            Session.Abandon()
            Session.Remove("MenuID")
            Session.Remove("MiddleFrame")

            MarkLog(Action.Logoff)

            Thread.CurrentPrincipal = Nothing

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

End Class