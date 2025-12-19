Public Class wfSkill_Ajax
    Inherits System.Web.UI.Page
#Region " Variable Declaration "
    Public mSkillList As SkillList
    Dim EventLogID As Guid
    Public mSkill As Skill

#End Region
#Region " Helper Methods "
    Public Sub GetSession()
        mSkillList = Session("mSkillList")
        mSkill = Session("mSkill")
    End Sub
    Private Sub SetSession()
        Session("mSkillList") = mSkillList
        Session("mSkill") = mSkill
    End Sub
    Private Sub setObjectSkillMaster()
        mSkill.Name = Trim(txtSkill.Text)
        mSkill.Code = Trim(txtCode.Text)
    End Sub
    Private Sub RemoveSessionForSkillMaster()
        Session.Remove("mSkill")
        Session.Remove("mSkillList")
    End Sub
    Private Sub NewRecordSkillMaster()
        mSkill = Skill.NewSkill
        Session("mSkill") = mSkill
        txtSkill.Text = ""
        txtCode.Text = ""
    End Sub
    Private Sub DataFieldBindSkillMaster()
        mSkillList = SkillList.GetSkillList()
        dgSkill.DataSource = mSkillList
        Session("mSkillList") = mSkillList
        upnlSkillMaster.DataBind()
    End Sub
    Private Sub EditRecordSkillMaster(ByVal mId As Guid)
        mSkill = Skill.GetSkill(mId)
        Session("mSkill") = mSkill
    End Sub
    Private Sub DeleteRecordSkillMaster(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteSkillMaster")
        mSkill = Skill.GetSkill(mId)
        Session("mSkill") = mSkill
    End Sub
  
#End Region
#Region "Skill Master"
    Private Sub btnAddSkillMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddSkillMaster.Click

        SetSession()
        NewRecordSkillMaster()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSkillWindow", "OpenSkillWindow()", True)
        If (Flypal.Util.Action.[New]) Then

            lblTitleSkillMaster.Text = "Skill [New]"
            upnlskillmast.Update()

        End If
    End Sub

    Private Sub btnCloseSkillMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseSkillMaster.Click
        SetSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnSaveSkillMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSkillMaster.Click
        If (Not User.IsInRole("EmployeeNew") And mSkill.IsNew) Or (Not User.IsInRole("EmployeeEdit") And Not mSkill.IsNew) Then
            setObjectSkillMaster()
            MarkLog(Flypal.Util.Action.Save, "Skill", User.Identity.Name & " is not Authorized User to save " + mSkill.Name, Flypal.Util.ErrorType.HandledError, mSkill.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Try
            setObjectSkillMaster()
            mSkill.Save()
            MarkLog(Flypal.Util.Action.Save, "Skill", mSkill.Name, Flypal.Util.ErrorType.HandledError, mSkill.ID, EventLogID)
            NewRecordSkillMaster()
            DataFieldBindSkillMaster()
            lblTitleSkillMaster.Text = "Skill [New]"
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub


#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBindSkillMaster()
        End If
    End Sub
    Private Sub dgSkill_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSkill.RowCommand
        Dim Idx As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                Idx = CInt(e.CommandArgument) + dgSkill.PageIndex * dgSkill.PageSize
                mID = CType(dgSkill.DataKeys(CInt(e.CommandArgument)).Value, Guid)
                'Idx = CInt(e.CommandArgument) + dgSkill.PageIndex * dgSkill.PageSize  'CInt(e.CommandArgument)

                'mID = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("EmployeeView") And Not User.IsInRole("EmployeeEdit")) Then
                    setObjectSkillMaster()
                    MarkLog(Flypal.Util.Action.Edit, "Skill", User.Identity.Name & " is not Authorized User to edit " + mSkill.Name, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecordSkillMaster(mID)
                txtSkill.DataBind()

                'Added by Shital on 18-Aug-2016
                txtCode.DataBind()

                MarkLog(Flypal.Util.Action.Edit, "Skill", mSkill.Name, Flypal.Util.ErrorType.NoError, mSkill.ID, EventLogID)
                If Len(mSkill.Name) > 15 Then
                    lblTitleSkillMaster.Text = "Skill [" & mSkill.Name.Substring(0, 15) & "...]"
                Else
                    lblTitleSkillMaster.Text = "Skill [" & mSkill.Name & "]"
                End If
                If txtSkill.Enabled = True Then
                    setFocus(txtSkill)
                End If

                upnlskillmast.Update()

            Case "DeleteRec"
                Idx = CInt(e.CommandArgument) + dgSkill.PageIndex * dgSkill.PageSize
                mID = CType(dgSkill.DataKeys(CInt(e.CommandArgument)).Value, Guid)

                'mID = New Guid(e.CommandArgument.ToString)
                If (Not User.IsInRole("EmployeeDelete")) Then
                    setObjectSkillMaster()
                    MarkLog(Flypal.Util.Action.Delete, "Skill", User.Identity.Name & " is not Authorized User to delete " + mSkill.Name, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecordSkillMaster(mID)
        End Select

    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1

                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteSkillMaster" Then
                        Try
                            Session("sender") = ""
                            mSkill = Session("mSkill")
                            Skill.DeleteSkill(mSkill.ID)
                            NewRecordSkillMaster()
                            DataFieldBindSkillMaster()
                            lblTitleSkillMaster.Text = "Skill Information"
                            upnlSkillMaster.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Skill", "Can't delete : " + mSkill.Name + "  is Currently in use", Flypal.Util.ErrorType.NoError, mSkill.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecordSkillMaster()
                            txtSkill.DataBind()
                            lblTitleSkillMaster.Text = "Skill Information"
                            upnlSkillMaster.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Skill", mSkill.Name, Flypal.Util.ErrorType.NoError, mSkill.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteSkillMaster" Then
                        NewRecordSkillMaster()
                        txtSkill.DataBind()
                        lblTitleSkillMaster.Text = "Skill Information [New]"
                        upnlSkillMaster.Update()
                    End If
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub dgSkill_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgSkill.PageIndexChanging
        dgSkill.PageIndex = e.NewPageIndex
        dgSkill.DataSource = mSkillList
        Session("mSkillList") = mSkillList
        dgSkill.DataBind()


    End Sub
End Class