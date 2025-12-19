Imports System.Net
Public Class wfFeedBackForm_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim EventLogID As Guid
    Public mUser As User
    Dim mRating As Integer
    Dim mCompanyDetail As CompanyDetail
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mUser = Session("mUser")
        mCompanyDetail = Session("mCompanyDetail")
        mRating = Session("mRating")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUser")
        Session.Remove("mCompanyDetail")
        Session.Remove("mRating")
    End Sub
    Private Function IPAddress() As String
        Dim server As String = Nothing
        server = Me.Context.Request.UserHostAddress()
        If server = "127.0.0.1" Then
            server = Dns.GetHostName()
        End If

        Dim heserver As IPHostEntry = Dns.Resolve(server)
        Dim curAdd As IPAddress
        For Each curAdd In heserver.AddressList
            curAdd.ToString()
        Next curAdd
        Return curAdd.ToString()

        ''Return Me.Context.Request.UserHostAddress()
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    
                Case MsgBoxResult.No
                    
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "Success" Then
                        Session("MiddleFrame") = ""
                        RemoveSession()
                        Response.Redirect("index.aspx")
                    End If
            End Select
        End If
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfFeedBackForm_Ajax.aspx?"
            DataBind()
        End If
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        Web.Security.FormsAuthentication.SignOut()
        Session.Remove("MenuID")
        Session.Remove("MiddleFrame")
        RemoveSession()
        MarkLog(Util.Action.Logoff)
        'Drop all the references to the Principal.
        Thread.CurrentPrincipal = Nothing
        'Dim str As String
        'str = "window.open('Index.aspx', '_top', 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); </script>"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenPageScript", str, True)
        Response.Redirect("Login.aspx")
    End Sub
    Private Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        If Not (RadioButton1.Checked Or RadioButton2.Checked Or RadioButton3.Checked Or RadioButton4.Checked Or RadioButton5.Checked Or RadioButton6.Checked Or _
            RadioButton7.Checked Or RadioButton8.Checked Or RadioButton9.Checked Or RadioButton10.Checked) Then
            MSGBoxCtrl.show("Alert!", "Please give the rating", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If Not txtSuggestionAnswer.Text.Length > 0 Then
            MSGBoxCtrl.show("Alert!", "Please give suggestion", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim NPS ''As NPSService.FlyPalNPSService = New NPSService.FlyPalNPSService

        If AppSettings("ServiceReferenceLocation") = "Local" Then
            NPS = New NPSService.FlyPalNPSService
        Else
            NPS = New NPSServiceBytzsoft.FlyPalNPSService
        End If

        Dim result As String = NPS.SubmitFeedBack(AppSettings("CorporateID"), mCompanyDetail.CompanyName, 2, New SmartDate(Date.Today.ToString).Date.ToString("dd-MMM-yyyy"), mUser.Name, txtSuggestionAnswer.Text, mRating, IPAddress())

        Dim temp As String() = result.Split(",")

        If temp(0) = "1" Then

            'Update user 
            ' 
            mUser.FeedBackSubmittedDate = DateTime.Now
            mUser.Save()
            Session("mUser") = mUser
            '---

            'ShowAlertBox("Feedback Submitted successfully.", "success")

            Dim msg As String = ""

            If mRating <= 6 Then
                msg = "Thanks for your feedback. We highly value all ideas and suggestions. In the future, our team might reach out to you to learn more about it so that it exceeds your expectations."
            ElseIf mRating >= 7 And mRating <= 8 Then
                msg = "Thanks for your feedback. Your ideas, and suggestions play a major role to improve us."
            ElseIf mRating > 8 Then
                msg = "Thanks for your feedback. We will make sure you have the best possible experience."
            End If
            MSGBoxCtrl.show("Success!", msg, "", MsgBoxStyle.OkOnly, "Success")
        Else
            MSGBoxCtrl.show("Error!", temp(1), "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Protected Sub RadioButton1_CheckedChanged(sender As Object, e As System.EventArgs) Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged, RadioButton3.CheckedChanged, RadioButton4.CheckedChanged, RadioButton5.CheckedChanged, RadioButton6.CheckedChanged, RadioButton7.CheckedChanged, RadioButton8.CheckedChanged, RadioButton9.CheckedChanged, RadioButton10.CheckedChanged
        Dim RdBtn As String = DirectCast(sender, System.Web.UI.WebControls.RadioButton).Text
        If RdBtn = "7" Or RdBtn = "8" Or RdBtn = "9" Or RdBtn = "10" Then
            lblSuggestionQuestion.Text = "Any suggestion/ change that would like to add in FlyPal?"
        Else
            lblSuggestionQuestion.Text = "How can we improve your experience?"
        End If
        mRating = CInt(DirectCast(sender, System.Web.UI.WebControls.RadioButton).Text)
        Session("mRating") = mRating
        upnlFeedBack.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region


    
End Class