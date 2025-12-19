'AJAX Conversion By Vikrant

Partial Class wfEmployeeSkill_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeSkill As EmployeeSkill

    Public mSkillList As SkillList

    Public BackPage As String

    Dim EventLogID As Guid 'Added by Saylee on 20-July-2011
    Public mSkill As Skill
    '18-Aug-2016
    Public mEmployeeSkillList As EmployeeSkillList
    Public mMPDSkillList As MPDSkillList
#End Region

#Region " Helper Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeSkill = Session("mEmployeeSkill")
        mEmployeeSkillList = Session("mEmployeeSkillList")
        mSkillList = Session("mSkillList")
        mSkill = Session("mSkill")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeSkill") = mEmployeeSkill
        Session("mEmployeeSkillList") = mEmployeeSkillList
        Session("mSkillList") = mSkillList
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub DataFieldBind()

        If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then
            mMPDSkillList = MPDSkillList.GetSkillList(False)
            chkSkillList.DataSource = mMPDSkillList
            chkSkillList.DataBind()
        Else
            mSkillList = SkillList.GetSkillList()
            chkSkillList.DataSource = mSkillList
            chkSkillList.DataBind()
        End If

        ' mSkillList = SkillList.GetSkillList()
        'Added by Shital on 18-Aug-2016
        'cmbSkillList.DataSource = mSkillList
        'cmbSkillList.DataBind()

        mEmployeeSkillList = EmployeeSkillList.GetEmployeeSkillList(mEmployee.ID)
        Session("mEmployeeSkillList") = mEmployeeSkillList


        If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then
            For i As Integer = 0 To chkSkillList.Items.Count - 1
                If mEmployeeSkillList.Contains(CType(chkSkillList.Items(i).Value, Integer)) Then
                    chkSkillList.Items(i).Selected = True
                End If
            Next
        Else
            For i As Integer = 0 To mEmployeeSkillList.Count - 1
                For j As Integer = 0 To chkSkillList.Items.Count - 1
                    If mEmployeeSkillList.Item(i).SkillID.Equals(New Guid(chkSkillList.Items(j).Value)) Then
                        chkSkillList.Items(j).Selected = True
                    End If
                Next
                ' chkSkillList.DataBind()
            Next
        End If

        Session("mSkillList") = mSkillList
        '18-Aug-2016
        txtEmployeeName.Text = mEmployee.Name
        ' upnlSkillDetails.DataBind()

        upnlSkillDetails.Update()
    End Sub
    Private Sub NewRecordSkillMaster()
        mSkill = Skill.NewSkill
        Session("mSkill") = mSkill
    End Sub

    Private Sub ControlVisibilityForAttachment()

        'Commented by Shital on 18-Aug-2016
        'If mEmployeeSkill.ImageSize > 0 Then
        '    ImageButton1.Visible = True
        '    btnDelAttach.Enabled = True
        'Else
        '    ImageButton1.Visible = False
        'End If
    End Sub

    Private Sub SetTitle()
        If mEmployeeSkill.IsNew Then
            lblTitle.Text = "Employee Skill Information"
        Else
            If Len(mEmployeeSkill.SkillName) > 15 Then
                lblTitle.Text = "Employee Skill Information [" & mEmployeeSkill.SkillName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Skill Information [" & mEmployeeSkill.SkillName & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub Save()
        SetObject()
        mEmployee.Save()
        'If txtName.Enabled = True Then
        '    setFocus(txtEmpNo)
        'End If
        MarkLog(Flypal.Util.Action.Save, "Employee Skill", "Emp : " + mEmployee.EmpNoName + " Skill : " + mEmployeeSkill.SkillName, Flypal.Util.ErrorType.HandledError, mEmployeeSkill.ID, EventLogID)
        SetSession()
        SetTitle()
    End Sub
    Private Sub SetObject()

        mEmployeeSkill.EmployeeID = mEmployee.ID
        'Commented by Shital on 18-Aug-2016
        'mEmployeeSkill.SkillID = New Guid(cmbSkillList.SelectedValue)
        'mEmployeeSkill.Value = Trim(txtValue.Text)
        'mEmployeeSkill.Remark = Trim(txtRemark.Text)
        'mEmployeeSkill.IsSkill = chkIsSkill.Checked

    End Sub

    Private Sub AttachMyFile()
        Try
            mEmployeeSkill.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mEmployeeSkill.ImageSize = Session("FileUpload.FileSize")
            mEmployeeSkill.FileExtension = Session("FileUpload.FileExtension")
            Session("mEmployeeSkill") = mEmployeeSkill
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    Private Sub RemoveSessionForSkillMaster()
        Session.Remove("mSkill")
        Session.Remove("mSkillList")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
            ControlVisibilityForAttachment()
            ' SetTitle()
            imgSkill.Visible = IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", False, True)

        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeSkillNew") And mEmployeeSkill.IsNew) Or (Not User.IsInRole("EmployeeSkillEdit") And Not mEmployeeSkill.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Flypal.Util.Action.Save, "Employee Skill", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Skill : " + mEmployeeSkill.SkillName, Flypal.Util.ErrorType.HandledError, mEmployeeSkill.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                'commented by Shital on 18-Aug-2016
                ' SetObject()

                If AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True" Then
                    For i As Integer = 0 To chkSkillList.Items.Count - 1
                        If Not mEmployeeSkillList.Contains(CType(chkSkillList.Items(i).Value, Integer)) Then
                            If chkSkillList.Items(i).Selected Then
                                mEmployeeSkill = EmployeeSkill.NewEmployeeSkill
                                mEmployeeSkill.EmployeeID = mEmployee.ID
                                mEmployeeSkill.MPDSkillID = Val(chkSkillList.Items(i).Value)
                                Session("mEmployeeSkill") = mEmployeeSkill
                                mEmployeeSkill.Save()
                            End If
                        Else
                            mEmployeeSkill = EmployeeSkill.GetEmployeeSkill(mEmployee.ID, Val(chkSkillList.Items(i).Value))
                            If chkSkillList.Items(i).Selected = False Then
                                EmployeeSkill.DeleteEmployeeSkill(mEmployeeSkill.ID) 'New Guid(chkSkillList.Items(i).Value))
                            End If
                        End If

                    Next
                Else
                    'Added by Shital on 18-Aug-2016
                    For i As Integer = 0 To chkSkillList.Items.Count - 1
                        If Not mEmployeeSkillList.Contains(New Guid(chkSkillList.Items(i).Value), "") Then
                            If chkSkillList.Items(i).Selected Then
                                mEmployeeSkill = EmployeeSkill.NewEmployeeSkill
                                mEmployeeSkill.EmployeeID = mEmployee.ID
                                mEmployeeSkill.SkillID = New Guid(chkSkillList.Items(i).Value)
                                Session("mEmployeeSkill") = mEmployeeSkill
                                mEmployeeSkill.Save()
                            End If
                        Else
                            mEmployeeSkill = EmployeeSkill.GetEmployeeSkill(mEmployee.ID, New Guid(chkSkillList.Items(i).Value))
                            If chkSkillList.Items(i).Selected = False Then
                                EmployeeSkill.DeleteEmployeeSkill(mEmployeeSkill.ID) 'New Guid(chkSkillList.Items(i).Value))
                            End If
                        End If

                    Next
                End If


                ' mEmployeeSkill.Save()
                SetSession()
                lblTitle.Text = "Employee Skill Information [New]"
                'MarkLog(FlyPal.Util.Action.Save, "Employee Skill", "Emp : " + mEmployee.EmpNoName + " Skill : " + mSkillList(mEmployeeSkill.SkillID).Name, FlyPal.Util.ErrorType.NoError, mEmployeeSkill.ID, EventLogID)
                MarkLog(FlyPal.Util.Action.Save, "Employee Skill", "Emp : " + mEmployee.EmpNoName, FlyPal.Util.ErrorType.NoError, mEmployeeSkill.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2601 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    'Added on 18-Aug-2016
    Private Sub chkSkillList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles chkSkillList.SelectedIndexChanged
        For i As Integer = 0 To chkSkillList.Items.Count - 1
            If chkSkillList.Items(i).Selected Then

            End If
        Next
    End Sub
    Private Sub imgSkill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgSkill.Click
        SetObject() 'Added Code
        NewRecordSkillMaster()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSkillWindow", "OpenSkillWindow();", True)
        'Response.Redirect("wfSkill.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeSkill_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        'If Not mEmployeeSkill.IsNew Then
        '    MarkLog(Flypal.Util.Action.Close, "Employee Skill", "Emp : " + mEmployee.EmpNoName + " Skill : " + mEmployeeSkill.SkillName, Flypal.Util.ErrorType.NoError, mEmployeeSkill.ID, EventLogID)
        'End If
        'Added by Vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    'Commented by Shital on 18-Aug-2016
    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    '----------------------------------------------------------------------
    '    Dim No As New Random
    '    Dim StrName As String = "abc" & No.Next.ToString
    '    '----------------------------------------------------------------------
    '    If mEmployeeSkill.ImageSize > 0 Then
    '        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mEmployeeSkill.FileExtension
    '        Dim fs As FileStream
    '        If File.Exists(AppSettings("DOCPath")) = False Then
    '            'Delete File if exist
    '            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeSkill.FileExtension)
    '            ' Create the file.
    '            fs = File.Create(path)
    '            '' Add some information to the file.
    '            fs.Write(mEmployeeSkill.ImageFile, 0, mEmployeeSkill.ImageFile.Length)
    '            fs.Close()
    '            Session("DOCPath") = path
    '            Dim Str As String
    '            Str = "openFile();"
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
    '        End If
    '    End If
    'End Sub   

    'Commented by Shital on 18-Aug-2016
    'Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
    '    Dim fileSize1 As Integer = 0
    '    Dim file1(fileSize1) As Byte
    '    mEmployeeSkill.ImageFile = file1
    '    mEmployeeSkill.ImageSize = 0

    '    ImageButton1.Visible = False
    '    btnDelAttach.Enabled = False
    'End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        '   MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlSkillDetails.Update()
    End Sub

    Private Sub hdnBtnSkill_Click(sender As Object, e As System.EventArgs) Handles hdnBtnSkill.Click
        mSkillList = Session("mSkillList")
        chkSkillList.DataSource = mSkillList
        chkSkillList.DataBind()

        For i As Integer = 0 To mEmployeeSkillList.Count - 1
            For j As Integer = 0 To chkSkillList.Items.Count - 1
                If mEmployeeSkillList.Item(i).SkillID.Equals(New Guid(chkSkillList.Items(j).Value)) Then
                    chkSkillList.Items(j).Selected = True
                End If
            Next
        Next
        upnlSkillDetails.Update()
    End Sub
#End Region

End Class
