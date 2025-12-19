

'AJAX Conversion By : Saylee on 16-Sep-2014


Public Class wfTaskCardStep_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mTaskCard As TaskCard
#End Region

#Region " Helper Methods "
    Private Sub SaveFormtoObject()
        mTaskCard.TaskSteps.CurrentItem.MPDNo = Trim(txtMPDNo.Text)
        mTaskCard.TaskSteps.CurrentItem.AMMNo = Trim(txtAMMNo.Text)
        mTaskCard.TaskSteps.CurrentItem.Description = txtStepDesc.Text
        mTaskCard.TaskSteps.CurrentItem.Zone = txtZone.Text
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        Dim mTaskStep As TaskStep
        If mTaskCard.TaskSteps.IsSavable = False Then
            For Each mTaskStep In mTaskCard.TaskSteps
                For i As Integer = 0 To mTaskStep.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mTaskStep.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        If strMSG.Trim <> "" Then
            cvControlValidator.ErrorMessage = strMSG
            cvControlValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
    'Added By Vikrant On 02-Jan-2014 For All02012014
    Private Sub SaveTaskCard()
        Try
            mTaskCard.Save()
            MarkLog(Util.Action.Save, "TaskCard", "Additional Work : " + Chr(13) + "Description : " + txtStepDesc.Text, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
            MarkLog(Util.Action.Save, "TaskCard", "TaskCard No : " & mTaskCard.TaskCardNo, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End Try
    End Sub
    'End
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        mTaskCard = Session("wfTaskCard.TaskCard")

        If Not Page.IsPostBack Then
            setFocus(txtMPDNo)
            ''Get the Index 
            'Session("wfTaskCardStep.Index") = Request.QueryString("Index")
            'mTaskCard.BeginEdit()
            'If CType(Session("wfTaskCardStep.Index"), Integer) = -1 Then
            '    mTaskCard.TaskSteps.Add(mTaskCard.ID)
            '    Session("wfTaskCard.TaskCard") = mTaskCard
            '    lblTitle.Text = "Additional Work Detail [New]"
            'Else
            '    mTaskCard.TaskSteps.CurrentIndex = (CInt(Session("wfTaskCardStep.Index")))
            '    Session("wfTaskCard.TaskCard") = mTaskCard
            '    lblTitle.Text = "Additional Work Detail "
            'End If
            DataBind()
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        mTaskCard = Session("wfTaskCard.TaskCard")
        Dim clnTaskCard As TaskCard
        clnTaskCard = mTaskCard.Clone

        SaveFormtoObject()
        If Not CustomValidate1() Then
            mTaskCard = clnTaskCard
            Session("mTaskCard") = mTaskCard
            Session("wfTaskCard.TaskCard") = clnTaskCard
            upnlValidationsummary.Update()
            Exit Sub
        End If

        Session("wfTaskCard.TaskCard") = mTaskCard
        Try
            If mTaskCard.TaskSteps.CurrentItem.IsDirty Then

                If mTaskCard.TaskSteps.Contains(mTaskCard.TaskSteps.CurrentItem.ID, mTaskCard.TaskSteps.CurrentItem.Description) Then
                    'Added by ajay 11-09-2023
                    mTaskCard = clnTaskCard
                    Session("mTaskCard") = mTaskCard
                    Session("wfTaskCard.TaskCard") = clnTaskCard
                    'Back button dublicate record change issue sovle
                    '----------
                    MSGBoxCtrl.Show("Alert!", "<strong> You are trying to save the duplicate entry. </strong> <p> You can not add duplicate record. </p> ", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    If mTaskCard.TaskSteps.CurrentItem.IsSavable Then
                        mTaskCard.ApplyEdit()
                        Session("wfTaskCard.TaskCard") = mTaskCard
                        SaveTaskCard() 'Added By Vikrant On 03-Jan-2014 For All02012014
                        'Response.Redirect(BackPage.Pop(Session("BackPage1")) & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
                        upnlValidationsummary.Update()
                        upnlStepDetail.Update()

                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        'End
                    Else
                        cvControlValidator.ErrorMessage = mTaskCard.TaskSteps.CurrentItem.GetBrokenRulesString
                        cvControlValidator.IsValid = mTaskCard.TaskSteps.CurrentItem.IsValid
                        upnlValidationsummary.Update()

                        mTaskCard = clnTaskCard
                        Session("mTaskCard") = mTaskCard
                        Session("wfTaskCard.TaskCard") = clnTaskCard

                    End If
                End If
            Else
                mTaskCard.ApplyEdit()
                Session("wfTaskCard.TaskCard") = mTaskCard

                'Response.Redirect(BackPage.Pop(Session("BackPage1")) & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))

                upnlValidationsummary.Update()
                upnlStepDetail.Update()

                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
                'End
            End If

            Session("StepEdit") = False
        Catch ex As Exception
            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
            mTaskCard.CancelEdit()
        End Try
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        mTaskCard.CancelEdit()
        If mTaskCard.TaskSteps.CurrentItem.IsNew And Not Session("StepEdit") = True Then mTaskCard.TaskSteps.Remove(mTaskCard.TaskSteps.CurrentItem)
        Session("StepEdit") = False
        'Response.Redirect(BackPage.Pop(Session("BackPage1")) & "?BackPage5=" & Request.QueryString("BackPage5") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&GChildPage5=" & Request.QueryString("GChildPage5") & "&GChildPage6=" & Request.QueryString("GChildPage6") & "&BackPage4=" & Request.QueryString("BackPage4") & "&GChildPage7=" & Request.QueryString("GChildPage7") & "&GChildPage8=" & Request.QueryString("GChildPage8") & "&TaskBackPage=" & Request.QueryString("TaskBackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
        upnlValidationsummary.Update()

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End

    End Sub
#End Region
End Class
