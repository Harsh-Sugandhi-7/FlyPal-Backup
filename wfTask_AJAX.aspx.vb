'Created By     :   Saylee
'Dated          :   20-Aug-2015



Public Class wfTask_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditTask As AuditTask
    Public mAuditTaskList As AuditTaskList
    Public mAuditCategoryList As AuditCategoryList
    Public mDepartmentList As AuditDepartmentList

    Dim AuditStandardID As Guid
    Dim AuditStandardName As String
    Dim TaskCategoryID As String

    Dim EventLogID As Guid

    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAuditTask = CType(Session("mAuditTask"), AuditTask)
        mAuditTaskList = CType(Session("mAuditTaskList"), AuditTaskList)
        mAuditCategoryList = CType(Session("mAuditCategoryList"), AuditCategoryList)
        mDepartmentList = CType(Session("mDepartmentList"), AuditDepartmentList)

        TaskCategoryID = Session("TaskCategoryID")
        AuditStandardID = New Guid(Session("AuditStandardID").ToString)
        AuditStandardName = Session("AuditStandardName")


        mFileAttach = Session("mFileAttach")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditTask")
        Session.Remove("mAuditTaskList")
        Session.Remove("mAuditCategoryList")
        Session.Remove("mDepartmentList")
    End Sub
    Private Sub SetSession()
        Session("mAuditTask") = mAuditTask
        Session("mAuditTaskList") = mAuditTaskList
        Session("mAuditCategoryList") = mAuditCategoryList
        Session("mDepartmentList") = mDepartmentList
        Session("AuditStandardID") = AuditStandardID
        Session("AuditStandardName") = AuditStandardName
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub NewRecord()
        mAuditTask = AuditTask.NewAuditTask()
        Session("mAuditTask") = mAuditTask
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mAuditTask = AuditTask.GetChildAuditTask(mId)
        Session("mAuditTask") = mAuditTask
    End Sub
   
    Private Sub DeleteRecord(ByVal mId As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfTask.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type")
        'Session("sender") = "Delete"
        'msg1.Show()
        SetGrid()
        upnlGrid.Update()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAuditTask = AuditTask.GetChildAuditTask(mId)
        Session("mAuditTask") = mAuditTask
    End Sub
    Private Sub setObject()
        mAuditTask.AuditCategoryID = New Guid(cmbAuditCategory.SelectedValue.ToString)
        mAuditTask.Code = Trim(txtCode.Text)
        mAuditTask.Description = Trim(txtDescription.Text)
        mAuditTask.Note = Trim(txtNote.Text)
        mAuditTask.DepartmentID = New Guid(cmbDepartmentList.SelectedValue.ToString)


        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mAuditTask.IsAttachmentAdded = True
            Else
                mAuditTask.IsAttachmentAdded = False
            End If
        End If

        Session("mAuditTask") = mAuditTask
    End Sub
    Private Sub GetAttachment()

        If mAuditTask.IsAttachmentAdded = True And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAuditTask.ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub GetAttachment(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)

        If mIsAttachemntAdded = True Then
            mFileAttach = FileAttach.GetAttachment(ID)
            Session("mFileAttach") = mFileAttach
        End If
    End Sub
    Private Sub ViewImage(ByVal ID As Guid, ByVal mIsAttachemntAdded As Boolean)
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment(ID, mIsAttachemntAdded)
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub SaveAttachment()

        If mFileAttach Is Nothing And mAuditTask.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachment(mAuditTask.ID)
            Session("mFileAttach") = mFileAttach
        End If

        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAuditTask.ID
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                    mFileAttach = Nothing
                    Session("mFileAttach") = mFileAttach
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), False)
                End Try
            Else
                If (Not mAuditTask.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAuditTask.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mAuditTask.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub ControlVisibility()

        If mAuditTask.AuditTaskExecutionCount > 0 Or mAuditTask.AuditTaskScheduleCount > 0 Or mAuditTask.AuditTask_AuditCount > 0 Then
            cmbAuditCategory.Enabled = False
        Else
            cmbAuditCategory.Enabled = True
        End If

        ControlVisibilityForAttachment()
    End Sub
    Private Sub ControlVisibility(ByVal SearchIndex As Int32)
        cmbTaskCategorySearch.Visible = IIf(SearchIndex = 1, True, False)
        cmbDepartmentListSearch.Visible = IIf(SearchIndex = 2, True, False)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            '
                            Session("NewAuditTask") = "False"
                            mAuditTask = CType(Session("mAuditTask"), AuditTask)
                            AuditTask.DeleteAuditTask(mAuditTask.ID)
                            ' Response.Redirect("wfTask.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type"))
                            ControlVisibility()
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            SetGrid()
                            upnlTaskDet.Update()
                            upnlGrid.Update()
                            upnlResult.Update()
                            upnlTitle.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
                                'Ajay 20-11-2023
                                NewRecord()
                                DataFieldBind()
                                SetTitle()
                                SetGrid()
                                upnlTaskDet.Update()
                                upnlGrid.Update()
                                upnlResult.Update()
                                upnlTitle.Update()
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            SetGrid()
                            upnlGrid.Update()
                            If msgCount = 0 Then
                                MarkLog(FlyPal.Util.Action.Delete, "Audit Task", mAuditTask.Code, FlyPal.Util.ErrorType.NoError, mAuditTask.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    '
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    '
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            '
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            '
        End If
    End Sub
    Private Sub SetTitle()
        If mAuditTask.IsNew Then
            lbltitle.Text = "Task [New]"
        Else
            If Len(mAuditTask.AuditCategoryName) > 15 Then
                lbltitle.Text = "Task [" & mAuditTask.AuditCategoryName.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Task [" & mAuditTask.AuditCategoryName & "]"
            End If
        End If

        lblResult.Text = "Task List: " & mAuditTaskList.Count & " Record(s) Found."
    End Sub
    Private Sub ClearAll()
        'If Session("MiddleFrame") <> "wfTask_AJAX.aspx?" And Session("MiddleFrame") <> "wfAuditExecutionList_Ajax.aspx?" And Session("MiddleFrame") <> "wfAuditScheduleList_Ajax.aspx?" And Session("MiddleFrame") <> "wfAuditList_Ajax.aspx?" Then
        '    Session.Remove("mAuditTask")
        '    Session.Remove("mAuditTaskList")
        '    Session.Remove("mAuditCategoryList")
        '    Session.Remove("mDepartmentList")
        '    Session.Remove("NewAuditTask")
        'End If
    End Sub
    Private Sub SetGrid()
        'Ajay H 27-09-2023
        'Dim B As Boolean
        'For j As Integer = 0 To dgTaskList.Rows.Count - 1
        '    B = CType(Me.dgTaskList.Rows.Item(j).Cells(9).Text, Boolean)
        '    If B = False Then
        '        dgTaskList.Rows.Item(j).Cells(8).Enabled = False
        '    End If
        'Next
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()

        mAuditTaskList = AuditTaskList.GetAuditTaskList(AuditStandardID)
        Session("mAuditTaskList") = mAuditTaskList
        dgTaskList.DataSource = mAuditTaskList

        mAuditCategoryList = AuditCategoryList.GetAuditCategoryList(AuditStandardID, "(SELECT)")
        cmbAuditCategory.DataSource = mAuditCategoryList

        cmbTaskCategorySearch.DataSource = mAuditCategoryList
        cmbTaskCategorySearch.DataBind()

        mDepartmentList = AuditDepartmentList.GetAuditDepartmentList("(SELECT)")
        cmbDepartmentList.DataSource = mDepartmentList

        cmbDepartmentListSearch.DataSource = mDepartmentList
        cmbDepartmentListSearch.DataBind()

        Session("mDepartmentList") = mDepartmentList

        If Not mAuditTask Is Nothing Then
            If Not mAuditCategoryList.Contains(mAuditTask.AuditCategoryID) Then
                mAuditTask.AuditCategoryID = Guid.Empty
            End If

            If Not mDepartmentList.Contains(mAuditTask.DepartmentID) Then
                mAuditTask.DepartmentID = Guid.Empty
            End If

            mAuditTask.AuditStandardName = AuditStandardName
        End If

        txtAuditStandard.Text = AuditStandardName
        DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAuditCategory" Then
            If cmbAuditCategory.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please select Task Category."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'ElseIf custValidator.ControlToValidate = "cmbDepartmentList" Then
            '    If cmbDepartmentList.SelectedIndex <= 0 Then
            '        custValidator.ErrorMessage = "Please select Department."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        ElseIf custValidator.ControlToValidate = "txtCode" Then
            If Len(txtCode.Text) > 500 Then
                custValidator.ErrorMessage = "Code should not be greater than 100 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text) > 5000 Then
                custValidator.ErrorMessage = "Description should not be greater than 5000 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text) > 0 Then
                custValidator.ErrorMessage = "Description required."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                custValidator.ErrorMessage = "Note should not be greater than 1000 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If cmbAuditCategory.Enabled = True And Session("Search") <> "True" Then
            setFocus(cmbAuditCategory)
        Else
            setFocus(cmbSearch)
            Session("Search") = "False"
        End If
        If Not IsPostBack Then
            AuditStandardID = New Guid(Session("AuditStandardID").ToString)
            Session("AuditStandardID") = AuditStandardID

            'If Request.QueryString("BackPage2") <> "wfTaskListForAuditSchedule_AJAX.aspx" Then Session("MiddleFrame") = "wfTask_AJAX.aspx?"
            If Session("NewAuditTask") <> "True" Then
                NewRecord()
            Else
                Session("NewAuditTask") = "True"
            End If
            DataFieldBind()
            ControlVisibility()
            SetTitle()
            SetGrid()
        End If

    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveTop.Click
        ''If (Not User.IsInRole("TaskNew") And mAuditTask.IsNew) Or (Not User.IsInRole("TaskEdit") And Not mAuditTask.IsNew) Then
        ''    setObject()
        ''    SetSession()
        ''    MarkLog(FlyPal.Util.Action.Save, "Task", "Not Authorized User", FlyPal.Util.ErrorType.HandledError, Guid.Empty)
        ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        ''    msg.ReplacePage = "wfTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
        ''    Session("sender") = "Authorization"
        ''    msg.Show()
        ''    Exit Sub
        ''End If
        If Not IsValid Then upnlValidation.Update() : Exit Sub
        Try
            setObject()
            If Not mAuditTask.IsValid Then upnlValidation.Update() : Exit Sub
            mAuditTask.Save()
             SaveAttachment()
            MarkLog(Flypal.Util.Action.Save, "Task", mAuditTask.Code, Flypal.Util.ErrorType.HandledError, mAuditTask.ID, EventLogID)
            mAuditTask = AuditTask.NewAuditTask()
            NewRecord()
            DataFieldBind()
            ControlVisibility()
            SetSession()
            SetTitle()
            SetGrid()
            mFileAttach = Nothing
            Session.Remove("mFileAttach")
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
            If cmbAuditCategory.Enabled = True Then
                setFocus(cmbAuditCategory)
            End If
            upnlTaskDet.Update()
            upnlTitle.Update()
            upnlGrid.Update()
            upnlResult.Update()
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
            End If
            DataFieldBind()
        End Try
    End Sub
    Private Sub dgTaskList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTaskList.RowCommand

        Select Case e.CommandName
            Case "EditRec"
                ''If (Not User.IsInRole("TaskView") And Not User.IsInRole("TaskEdit")) Then
                ''    setObject()
                ''    SetSession()
                ''    MarkLog(Flypal.Util.Action.Edit, "Task", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                Dim Idx As Int32 = e.CommandArgument.ToString + dgTaskList.PageIndex * dgTaskList.PageSize
                Dim mID As Guid = mAuditTaskList(Idx).ID
                EditRecord(mID)
                dgTaskList.DataSource = mAuditTaskList
                DataBind()
                SetGrid()
                ControlVisibility()
                SetTitle()
                If cmbAuditCategory.Enabled = True Then
                    setFocus(cmbAuditCategory)
                ElseIf txtCode.Enabled = True Then
                    setFocus(txtCode)
                End If
                upnlTaskDet.Update()
                upnlTitle.Update()
                upnlGrid.Update()
                upnlAttachment.Update()
                MarkLog(Flypal.Util.Action.Edit, "Task", mAuditTask.Code, Flypal.Util.ErrorType.NoError, mAuditTask.ID, EventLogID)
            Case "DeleteRec"
                ''If (Not User.IsInRole("TaskDelete")) Then
                ''    setObject()
                ''    SetSession()
                ''    MarkLog(Flypal.Util.Action.Delete, "Task", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
                ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                ''    msg.ReplacePage = "wfTask.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Type=" & Request.QueryString("Type") & "&AssemblyTypeId=" & Request.QueryString("AssemblyTypeId")
                ''    Session("sender") = "Authorization"
                ''    msg.Show()
                ''    Exit Sub
                ''End If
                Dim Idx As Int32 = e.CommandArgument.ToString + dgTaskList.PageIndex * dgTaskList.PageSize
                Dim mID As Guid = mAuditTaskList(Idx).ID
                DeleteRecord(mID)
            Case "ViewRec"
                Dim Idx As Int32 = e.CommandArgument.ToString + dgTaskList.PageIndex * dgTaskList.PageSize
                Dim mID As Guid = mAuditTaskList(Idx).ID
                Dim mIsAttachemntAdded As Boolean = mAuditTaskList(Idx).IsAttachmentAdded
                SetGrid()
                ViewImage(mID, mIsAttachemntAdded)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        MarkLog(Flypal.Util.Action.[New], "Task", "", Flypal.Util.ErrorType.NoError, mAuditTask.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        ControlVisibility()
        cmbTaskCategorySearch.SelectedValue = TaskCategoryID
        'mAuditTaskList = AuditTaskList.GetAuditTaskList("", cmbTaskCategorySearch.SelectedValue.ToString, cmbDepartmentListSearch.SelectedValue.ToString, AuditStandardID.ToString)
        'Session("TaskCategoryID") = cmbTaskCategorySearch.SelectedValue.ToString
        'dgTaskList.DataSource = mAuditTaskList
        'dgTaskList.DataBind()
        'SetGrid()
        'If cmbAuditCategory.Enabled = True Then
        '    setFocus(cmbAuditCategory)
        'End If
        mFileAttach = Nothing
        Session.Remove("mFileAttach")
        SetTitle()
        upnlTitle.Update()
        upnlTaskDet.Update()
        upnlResult.Update()

    End Sub

    Private Sub dgTaskList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTaskList.Sorting
        mAuditTaskList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAuditTaskList") = mAuditTaskList
        dgTaskList.DataSource = mAuditTaskList
        dgTaskList.DataBind()
        SetGrid()  'Added By Utkarsh On 4-May-2011

    End Sub
    Private Sub imgbtnAuditCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnAuditCategory.Click
        setObject()
        'Response.Redirect("wfAuditCategory.aspx?ChildPage2=wfTask.aspx" & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2"))
        Session("NewAuditTask") = "True"
        Session("AuditStandardID") = AuditStandardID
        'If Request.QueryString("BackPage2") <> "wfTaskListForAuditSchedule_AJAX.aspx" Then
        '    Dim str As String
        '    str = "<script language='javascript'>openledgersame('wfAuditCategory.aspx?ChildPage2=Index.aspx&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type") & "');</script>"
        '    ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        'Else
        '    Response.Redirect("wfAuditCategory.aspx?ChildPage2=wfTask.aspx&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type"))
        'End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAuditCategoryWindow", "OpenAuditCategoryWindow()", True)
    End Sub
    Private Sub imgbtnDepartment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnDepartment.Click
        setObject()
        'Response.Redirect("wfAuditCategory.aspx?ChildPage2=wfTask.aspx" & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2"))
        Session("NewAuditTask") = "True"
        'Response.Redirect("wfAuditDepartment.aspx?ChildPage2=wfTask.aspx&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAuditDepartmentWindow", "OpenAuditDepartmentWindow()", True)
    End Sub
    Private Sub btnCloseTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        MarkLog(Flypal.Util.Action.Close, "Task", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Session.Remove("Search")
        RemoveSession()
        'Session.Remove("mTask")
        'If Request.QueryString("BackPage2") <> "" Then
        '    Response.Redirect(Request.QueryString("BackPage2") & "?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&AuditStandardID=" & AuditStandardID.ToString & "&AuditStandardID=" & AuditStandardID.ToString & "&Type=" & Request.QueryString("Type"))
        'Else
        '    Session("MiddleFrame") = ""
        '    Response.Redirect("Dashboard.aspx")
        'End If
        Session.Remove("NewAuditTask")
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFindNow.Click
        dgTaskList.PageIndex = 0
        mAuditTaskList = AuditTaskList.GetAuditTaskList("", cmbTaskCategorySearch.SelectedValue.ToString, cmbDepartmentListSearch.SelectedValue.ToString, AuditStandardID.ToString)
        Session("TaskCategoryID") = cmbTaskCategorySearch.SelectedValue.ToString
        dgTaskList.DataSource = mAuditTaskList
        Session("mAuditTaskList") = mAuditTaskList
        dgTaskList.DataBind()
        SetGrid()  'Added By Utkarsh On 4-May-2011
        lblResult.Text = "Task List: " & mAuditTaskList.Count & " Record(s) Found."

        upnlResult.Update()
        upnlGrid.Update()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        GetAttachment()
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                'Dim Str As String
                'Str = "<script language=Javascript>openFile();</script>"
                'ClientScript.RegisterStartupScript(Me.GetType(), "openFilel", Str)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub cmbSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearch.SelectedIndexChanged
        cmbTaskCategorySearch.SelectedIndex = 0
        cmbDepartmentListSearch.SelectedIndex = 0
        ControlVisibility(cmbSearch.SelectedIndex)
        If cmbSearch.Enabled = True Then
            setFocus(cmbSearch)
            Session("Search") = "True"
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        GetAttachment()

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnAuditCategory_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnAuditCategory.Click
        mAuditCategoryList = AuditCategoryList.GetAuditCategoryList(AuditStandardID, "(SELECT)")
        cmbAuditCategory.DataSource = mAuditCategoryList
        cmbAuditCategory.DataBind()

        cmbTaskCategorySearch.DataSource = mAuditCategoryList
        cmbTaskCategorySearch.DataBind()

        upnlAuditCategory.Update()
        upnlSearch.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAuditTask.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAuditTask.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAuditTask.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mFileAttach = Session("mFileAttach")
        mAuditTask.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlAttachment.Update()
    End Sub
    Private Sub hdnimgBtnAuditDepartment_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnAuditDepartment.Click
        mDepartmentList = AuditDepartmentList.GetAuditDepartmentList("(SELECT)")
        cmbDepartmentList.DataSource = mDepartmentList
        cmbDepartmentList.DataBind()

        cmbDepartmentListSearch.DataSource = mDepartmentList
        cmbDepartmentListSearch.DataBind()

        upnlDepartment.Update()
        upnlSearch.Update()
    End Sub
#End Region


End Class