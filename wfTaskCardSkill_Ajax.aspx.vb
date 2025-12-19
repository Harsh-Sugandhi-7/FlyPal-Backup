'Created By Shital On 18-Aug-2016
Public Class wfTaskCardSkill_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mTaskCard As TaskCard
    Public mTaskCardSkill As TaskCardSkill
    Public mSkillList As SkillList
    Public BackPage As String
    Dim EventLogID As Guid
    Public mSkill As Skill
    Public mTaskCardSkillList As TaskCardSkills
#End Region

#Region " Helper Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub GetSession()
        mTaskCard = Session("mTaskCard")
        mTaskCardSkillList = Session("mTaskCardSkillList")
        mSkillList = Session("mSkillList")
        mSkill = Session("mSkill")
    End Sub
    Private Sub SetSession()
        Session("mTaskCard") = mTaskCard
        Session("mTaskCardSkillList") = mTaskCardSkillList
        Session("mSkillList") = mSkillList
        Session("mSkill") = mSkill
    End Sub
    Private Sub NewRecordSkillMaster()
        mSkill = Skill.NewSkill
        Session("mSkill") = mSkill
    End Sub
    Private Sub SetObject()
        'mTaskCardSkill.TaskCardID = mTaskCard.ID
    End Sub
    Private Sub DataFieldBind()
        mSkillList = SkillList.GetSkillList()

        mTaskCardSkillList = TaskCardSkills.GetTaskCardSkills(mTaskCard.ID)
        Session("mTaskCardSkillList") = mTaskCardSkillList

        chkSkillList.DataSource = mSkillList
        chkSkillList.DataBind()
        Session("mSkillList") = mSkillList
        txtTaskCardNo.Text = mTaskCard.TaskCardNo

        For i As Integer = 0 To mTaskCardSkillList.Count - 1
            For j As Integer = 0 To chkSkillList.Items.Count - 1
                If mTaskCardSkillList.Item(i).SkillID.Equals(New Guid(chkSkillList.Items(j).Value)) Then
                    chkSkillList.Items(j).Selected = True
                End If
            Next
        Next
        'upnlSkillDetails.DataBind()
        upnlSkillDetails.Update()
    End Sub
    Private Sub SetTitle()
        If mTaskCardSkill.IsNew Then
            lblTitle.Text = "Task Card Skill Information"
        Else
            If Len(mTaskCardSkill.SkillName) > 15 Then
                lblTitle.Text = "Task Card Skill Information [" & mTaskCardSkill.SkillName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Task Card Skill Information [" & mTaskCardSkill.SkillName & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Function CustomValidate1() As Boolean
        Dim strMSG As String = ""
        Dim mTaskCardTool As TaskCardTool
        If Not mTaskCard.TaskCardTools.IsValid Then
            For Each mTaskCardTool In mTaskCard.TaskCardTools
                For i As Integer = 0 To mTaskCardTool.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mTaskCardTool.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If
        If strMSG.Trim <> "" Then
            cvSkill.ErrorMessage = strMSG
            cvSkill.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub SaveTaskCard()
        Try
            mTaskCard.Save()
            MarkLog(Util.Action.Save, "TaskCard", "TaskCard No : " & mTaskCard.TaskCardNo, Util.ErrorType.NoError, mTaskCard.ID, EventLogID)
        Catch ex As Exception
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Exception, SIMsgBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly)
            MSGBoxCtrl.show(MSGBox.Message_title.Exception, MSGBox.Message_text.ErrorMessage, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End Try
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        '  If mTaskCard.TaskCardSkills.CurrentItem.IsNew And Not Session("SkillEdit") = True Then mTaskCard.TaskCardSkills.Remove(mTaskCard.TaskCardSkills.CurrentItem)
        Session("SkillEdit") = False
        upnlValidationSummary.Update()

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("TaskCardNew") And mTaskCard.IsNew) Or (Not User.IsInRole("TaskCardEdit") And Not mTaskCard.IsNew) Then
            SetSession()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If Not Page.IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate1() Then
            upnlValidationSummary.Update()
            Exit Sub
        End If

        Session("mTaskCard") = mTaskCard
        For i As Integer = 0 To chkSkillList.Items.Count - 1
            If Not mTaskCard.TaskCardSkills.Contains(New Guid(chkSkillList.Items(i).Value), "") Then
                If chkSkillList.Items(i).Selected Then
                    mTaskCard.TaskCardSkills.Add(mTaskCard.ID)
                    mTaskCard.TaskCardSkills.CurrentItem.SkillID = New Guid(chkSkillList.Items(i).Value)
                    mTaskCard.TaskCardSkills.CurrentItem.SkillName = mSkillList.Item(New Guid(chkSkillList.Items(i).Value)).Name
                    mTaskCard.TaskCardSkills.CurrentItem.SkillCode = mSkillList.Item(New Guid(chkSkillList.Items(i).Value)).Code
                    SaveTaskCard()

                End If
            Else
                mTaskCardSkill = TaskCardSkill.GetTaskCardSkill(mTaskCard.ID, New Guid(chkSkillList.Items(i).Value))
                If chkSkillList.Items(i).Selected = False Then
                    'TaskCardSkill.DeleteTaskCardSkill(mTaskCardSkill.ID) 
                    mTaskCard.TaskCardSkills.Remove(mTaskCardSkill.ID)
                    mTaskCard.Save()
                End If
            End If
        Next

        upnlValidationSummary.Update()
        upnlSkillDetails.Update()

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
#End Region
    Private Sub imgSkill_Click(sender As Object, e As System.EventArgs) Handles imgSkill.Click
        SetObject() 'Added Code
        NewRecordSkillMaster()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSkillWindow", "OpenSkillWindow();", True)
    End Sub

    Private Sub hdnBtnSkill_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSkill.Click
        mTaskCardSkillList = Session("mTaskCardSkillList")
        chkSkillList.DataSource = mSkillList
        chkSkillList.DataBind()
        For i As Integer = 0 To mTaskCardSkillList.Count - 1
            For j As Integer = 0 To chkSkillList.Items.Count - 1
                If mTaskCardSkillList.Item(i).SkillID.Equals(New Guid(chkSkillList.Items(j).Value)) Then
                    chkSkillList.Items(j).Selected = True
                End If
            Next
        Next
        upnlSkillDetails.Update()
    End Sub
End Class