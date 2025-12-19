Public Class wfByMail_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mUserEmailID As String = ""
    Dim mUserCcEmailID As String = ""
    Dim ReportGenratedByVisibility As String = ""
    Dim RemarkVisibility As String = ""
#End Region

#Region " Business Methods "
    Private Sub getSession()
        mUserEmailID = Session("UserEmailID")
        mUserCcEmailID = Session("UserCcEmailID")
        ReportGenratedByVisibility = Session("ReportGenratedByVisibility")
        RemarkVisibility = Session("RemarkVisibility")
    End Sub
#End Region

#Region "Events"
    Private Sub wfByMail_Ajax_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
        If Not IsPostBack Then
            txtMailIDs.Focus()
            txtMailIDs.Text = mUserEmailID
            txtMailIDs.DataBind()
            txtCCIDs.Text = mUserCcEmailID
            txtCCIDs.DataBind()
            txtReportGenratedBy.DataBind()
            lblReportGenratedBy.DataBind()
            txtRemark.DataBind()
            lblRemark.DataBind()
        End If
    End Sub
    Private Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        Dim mopenas As String = Request.QueryString("Type")
        Session("ToSendMailIDs") = txtMailIDs.Text
        Session("SendMailRemark") = txtRemark.Text.Trim
        Session("ReportGenratedBy") = txtReportGenratedBy.Text.Trim
        Session("CcSendMailIDs") = txtCCIDs.Text
        Session.Remove("UserEmailID")
        Session.Remove("UserCcEmailID")
        Session.Remove("ReportGenratedByVisibility")
        Session.Remove("RemarkVisibility")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentToSendMail();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
#End Region

End Class