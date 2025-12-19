Partial Class wfTaskList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variable and Declarations"
    'Object variables required for binding data grid
    Public mWOJobsTasksReqPartList As WOJobsTasksReqPartList
    Public mWOJobsTasksUsedPartList As WOJobsTasksUsedPartList
    'Parent object and its Child collection(s)
    Public mWO As WO
    Public mWOJobs As WOJobs
    Public mWOJob As WOJob
    Public mWOJobCharges As WOJobCharges
    Public mWOJobCharge As WOJobCharge

    Public mWOReqPartCharges As WOReqPartCharges

    Public mWOID As Guid
    Dim mWOJobTasks As WOJobTasks
    Dim mWOJobNo As String
    'For Report
    ' Private objrptReportSearchingCriteria As New rptReportSearchingCriteria
#End Region

#Region " Methods"
    Private Sub GetSession()
        mWO = CType(Session("mWO"), WO)
        mWOJobTasks = Session("mWOJobTasks")
        mWOJobNo = Session("mWOJobNo")
    End Sub
    Private Sub SetSession()
        Session("mWO") = mWO
        Session("mWOJobTasks") = mWOJobTasks
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mWO")
        Session.Remove("mWOJobTasks")
    End Sub
    Private Sub DataFieldBind()
        dgJobList.DataSource = mWO.WOJobs
        dgTaskList.DataSource = mWOJobTasks
        DataBind()
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
            Result1 = -1
        Else
            Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        End If
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes

                    If CType(Session("sender"), String) = "Close" Then
                        Try
                            Session("Sender") = ""
                            mWO.ApplyEdit()
                            mWO = CType(mWO.Save, WO)
                            Session("mWO") = mWO
                            Response.Redirect("Index.aspx")
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            End If
                            DataFieldBind()
                        End Try
                        ''MsgBox for Charge Deletion
                        '--------------------------------------------------------------------------------
                    End If

                    If CType(Session("sender"), String) = "Remove" Then
                        Try
                            Session("Sender") = ""
                            'WOJobTask.DeleteWOJobTask(mWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                            mWO.WOJobs.CurrentItem.WOJobTasks.Remove(mWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                            Response.Redirect("wfTaskList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 2627 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            ElseIf ex.Number = 547 Then
                                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                                msg1.Show()
                            End If
                            DataFieldBind()
                        End Try
                        ''MsgBox for Charge Deletion
                        '--------------------------------------------------------------------------------
                    End If
                Case MsgBoxResult.No

                    If Session("Sender") = "Close" Then
                        Session("Sender") = ""
                        Session("mWOJobNo") = Nothing
                        Response.Redirect("index.aspx")
                    Else
                        Session("Sender") = ""
                        Response.Redirect("wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage"))
                    End If

                Case MsgBoxResult.OK And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()
                    Response.Redirect("wfTaskList.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
                    Response.Redirect("wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            Response.Redirect("wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)

    End Sub
    Private Sub EditRecord(ByVal index As Integer)
        mWO.BeginEdit()
        mWO.WOJobs.CurrentItem.WOJobTasks.CurrentIndex = index
        Session("mCurrentRow") = index
        Session("mWO") = mWO
        Session("mCurrentRow") = index
    End Sub
    Private Sub DeleteRecord(ByVal index As Integer)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Remove, SIMsgBox.Message_text.Remove, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
        Session("sender") = "Remove"
        msg1.Show()
        mWO.WOJobs.CurrentItem.WOJobTasks.CurrentIndex = index
        Session("mCurrentRow") = index
        Session("mWO") = mWO
    End Sub
    Private Sub DeleteChargeRecord(ByVal index As Integer)
        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
        Session("sender") = "DeleteCharge"
        msg1.Show()
        mWO.WOJobCharges.CurrentIndex = index
        Session("mWO") = mWO
    End Sub
    Private Sub SetTitle()
        lblTitle.Text = "Work Order [" & mWO.WONumber & "]"
    End Sub
#End Region

#Region " Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
            SetSession()
            If Not IsNothing(Session("mWOJobNo")) Then
                lblWOJob.Text = "Task List For [" & mWOJobNo & "] Job."
            End If
        End If
        SetTitle()
        MessageBoxResult()
    End Sub
    Private Sub dgJobList_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgJobList.ItemCommand
        Dim mId As New Guid(e.Item.Cells(0).Text)
        Dim JobIndex As Int32 = e.Item.ItemIndex + dgJobList.CurrentPageIndex * dgJobList.PageSize
        Select Case e.CommandName
            Case "Edit"
                mWOJobNo = mWO.WOJobs.Item(mId).JobDescription
                Session("mWOJobNo") = mWOJobNo
                lblWOJob.Text = "Task List For " & mWOJobNo & " Job."
                lblWOJob.DataBind()
                mWOJobTasks = mWO.WOJobs.Item(mId).WOJobTasks
                Session("mWOJobTasks") = mWOJobTasks
                dgTaskList.DataSource = mWOJobTasks
                dgTaskList.DataBind()
                mWO.WOJobs.CurrentIndex = JobIndex
        End Select
    End Sub
    Private Sub dgTaskList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgTaskList.ItemCommand
        Dim mId As New Guid(e.Item.Cells(0).Text)
        Select Case e.CommandName
            Case "Edit"
                EditRecord(e.Item.ItemIndex)
                Session("TaskList") = "True"
                Dim str As String
                str = "<script language='javascript'>  openledgersame('wfWOJobTaskDetail.aspx?BackPage=wfTaskList.aspx'); </script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
            Case "Remove"
                DeleteRecord(e.Item.ItemIndex)
        End Select
    End Sub
    Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click, btnAddNewTop.Click
        SetSession()
        mWO.BeginEdit()
        mWO.WOJobs.CurrentItem.WOJobTasks.Add(mWO.WOJobs.CurrentItem.ID, mWO.LocationName)
        Session("Edit") = False
        Session("mCurrentRow") = -1
        Session("TaskList") = "True"
        Dim str As String
        str = "<script language='javascript'>  openledgersame('wfWOJobTaskDetail.aspx?BackPage=wfTaskList.aspx'); </script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        Session("IsValid") = IsValid
        If mWO.IsDirty Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
            msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
            Session("sender") = "Close"
            msg1.Show()
        Else
            RemoveSession()
            Session("mWOJobNo") = Nothing
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            mWO.ApplyEdit()
            mWO.Save()
            'MarkLog(Util.Action.Save, "WO", mWO.WONo, Util.ErrorType.NoError, mWO.ID)
            mWO.MarkClean()
            Session("mWO") = mWO

        Catch ex As SqlException
            If ex.Number = 8145 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            ElseIf ex.Number = 2627 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            ElseIf ex.Number = 547 Then
                Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                msg1.ReplacePage = "wfTaskList.aspx?BackPage=" & Request.QueryString("BackPage")
                msg1.Show()
            End If
            DataFieldBind()
        End Try
    End Sub
#End Region

End Class
