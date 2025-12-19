Imports javax.print.attribute.standard
Imports javax.security.auth

Public Class wfDigitalSignatureRequest
    Inherits System.Web.UI.Page

#Region "Variable Declarations"

    Dim mDS_Queue As DS_Queue
    Dim mUserList As UserList

    'Dim mEventLogSession As EventLogSetSession
    'Dim mUser As System.Security.Principal.IPrincipal
    'Dim mGBUser As Global.User
    Dim mUser1 As User
#End Region

#Region "Data Binding"

    Private Sub Loadcombos()

        mUserList = UserList.GetListofUser("", AddTopItem:="(SELECT)")
        cmbAuthorizedUserList.DataSource = mUserList
        cmbAuthorizedUserList.DataBind()

        Session("wfDigitalSignatureRequest.UserList") = mUserList

        upnlDS.Update()
    End Sub

#End Region

#Region "Helper Methods"
    Private Sub GetSession()

        'mEventLogSession = Session("EventLogSession")
        mUser1 = Session("User")
        'mGBUser = Session("GlobalUser")

        mDS_Queue = Session("mDS_Queue")
        mUserList = Session("wfDigitalSignatureRequest.UserList")

    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            GetSession()

            If Not IsPostBack Then
                Loadcombos()
                txtModuleName.Text = mDS_Queue.ModuleName

                upnlDS.Update()
            End If

        Catch ex As Exception
            ex = ex.GetBaseException
        End Try
    End Sub
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Try
            If cmbAuthorizedUserList.SelectedIndex > 0 Then
                mUser1 = SI.UTILITY.User.GetUser(cmbAuthorizedUserList.SelectedItem.Text.Trim)
            End If
            If mUser1.UserEmail = "" Or mUser1.UserEmail = String.Empty Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Selected authorized person has not set mail id.", False), True)
                Exit Sub
            End If

            mDS_Queue.RequestedDateTime = Now
            mDS_Queue.AuthoriseduserID = New Guid(cmbAuthorizedUserList.SelectedValue)
            mDS_Queue.Description = txtDescription.Text


            If mDS_Queue.IsValid Then

                mDS_Queue = CType(mDS_Queue.Save(), DS_Queue)

                'ShowAlertBox("Submitted successfully", "success")
                MailForDigitalSignatureRequest.MailForDigitalSignatureRequest(rpt:=Nothing,
                                                                              UserName:=User.Identity.Name,
                                                                              AuthorizedUserName:=cmbAuthorizedUserList.SelectedItem.Text,
                                                                              Subject:="Signature Request on " + txtModuleName.Text.Trim + " Details",
                                                                              DocumentName:=txtModuleName.Text.Trim, Info:="",
                                                                              ToMailID:=mUser1.UserEmail,
                                                                              Remark:="", ReportGenratedBy:="")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Submitted successfully", False), True)
            Else

                'lblErrorList.Text = GetValidationList_DS()
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, "showErrorList();", True)
                'upnlErrorList.Update()
                upnlValidation.Update()

            End If


        Catch ex As Exception
            'ShowAlertBox(ex.Message.ToString, "error")
        End Try
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        'If mDS_Queue.ModuleID = 1 Then
        'Response.Redirect("WfCrew.aspx?ID=" + mDS_Queue.TransactionID.ToString + "&action=2")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
            End If
        'End If

    End Sub
#End Region


End Class