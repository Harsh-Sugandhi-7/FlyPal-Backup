Public Class wfSendMailForPaymentAdvice_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
        Authorized = 8
    End Enum
#End Region

#Region "Variable Declaration"
    Dim rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
    Public mPaymentAdvice As PaymentAdvice
    Dim PAToAC As Boolean = False
    Dim ACToPA As Boolean = False
#End Region

#Region "Methods"
    Private Sub getSession()
        rpt = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)
        mPaymentAdvice = Session("mPaymentAdvice")
        PAToAC = Session("PAToAC")
        ACToPA = Session("ACToPA")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        'Deciding IsInRole String to check Rights

        If PAToAC = True Then
            IsInRoleString = "PaymentAdvice"
        ElseIf ACToPA = True Then
            IsInRoleString = "PendingPA"
        End If


        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.FindNow
                Return User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "View") Or User.IsInRole(IsInRoleString + "Edit") Or User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
#End Region

#Region "Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getSession()
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Session("CloseWithoutSendMail") = True
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub

    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
        If Not IsInRole(Rights.Authorized) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If PAToAC = True Then
            Dim str As String
            str = "This Payment Advice is Created By : " & User.Identity.Name
            Dim StrMailBody As String = ""

            StrMailBody = "<html>"
            StrMailBody = StrMailBody + "<head>"
            StrMailBody = StrMailBody + "</head>"
            StrMailBody = StrMailBody + "<body style=""font-family: Tahoma; font-size: smaller;"">"
            ' StrMailBody = StrMailBody + "<b>Dear" + mPaymentAdvice.PaymentTo.ToString + "</b>"
            StrMailBody = StrMailBody + "<br /><br />"
            StrMailBody = StrMailBody + "Please find the attached approved payment advice for Supplier " + mPaymentAdvice.VendorName.ToString
            StrMailBody = StrMailBody + "<br /> <br />"

            Try
                SendMailFile.SendMailFile(Session("CrystalReport"), User.Identity.Name, "Payment Advice For " + mPaymentAdvice.VendorName.ToString, mPaymentAdvice.PaymentNo, _
                                          Info:=StrMailBody.ToString, VendorEmailID:="", ToMailID:=Trim(txtMailIDs.Text), CCMailID:=Trim(txtCCIDs.Text), BCCMailID:="", Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), ReportPath:=Session("ReportPath"), ReportByMail:=True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)


                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If

            Catch ex As Exception
                MSGBoxCtrl.show("Error", "Error Sending Mail", ex.InnerException.ToString + ex.Message.ToString, MsgBoxStyle.OkOnly, "")
            End Try
        ElseIf ACToPA = True Then
            Session("ToMailIDs") = txtMailIDs.Text
            Session("CCMailIDs") = txtCCIDs.Text
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If

    End Sub

#End Region

End Class