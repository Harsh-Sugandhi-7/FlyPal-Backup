'AJAX Conversion by vikrant on 21-Aug-2015

Public Class wfAuditor_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAuditor As Auditor
    Public mAuditorList As AuditorList
    Protected mDesignationList As DesignationList
    Dim EventLogID As Guid          'Added by Vikrant on 25-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAuditor = CType(Session("mAuditor"), Auditor)
        mAuditorList = CType(Session("mAuditorList"), AuditorList)
        mDesignationList = Session("mDesignationList_Auditor")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAuditor")
        Session.Remove("mAuditorList")
    End Sub
    Private Sub NewRecord()
        mAuditor = Auditor.NewAuditor()
        Session("mAuditor") = mAuditor
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mAuditor = Auditor.GetChildAuditor(mId)
        Session("mAuditor") = mAuditor
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mAuditor = Auditor.GetChildAuditor(mId)
        Session("mAuditor") = mAuditor
    End Sub
    Private Sub setObject()
        mAuditor.Name = Trim(txtName.Text)
        mAuditor.DesignationID = New Guid(cmbDesignationList.SelectedValue)
        mAuditor.IsNotWorking = chkWorkingStatus.Checked
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim mAuditorName As String
                        Try
                            Session("sender") = ""
                            mAuditor = CType(Session("mAuditor"), Auditor)
                            mAuditorName = mAuditor.Name
                            Auditor.DeleteAuditor(mAuditor.ID)
                            NewRecord()
                            DataFieldBind()
                            txtName.Text = ""
                            SetTitle()
                            upnlAuditor.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                'Ajay 21-11-2023
                                NewRecord()
                                DataFieldBind()
                                SetTitle()
                                upnlAuditor.Update()

                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed by Vikrant on 25-July-2011
                                MarkLog(Flypal.Util.Action.Delete, "Auditor", mAuditorName, Flypal.Util.ErrorType.NoError, mAuditor.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
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
    Private Sub SetTitle()
        If mAuditor.IsNew Then
            lbltitle.Text = "Lead Auditor [New]"
        Else
            If Len(mAuditor.Name) > 15 Then
                lbltitle.Text = "Lead Auditor [" & mAuditor.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Lead Auditor [" & mAuditor.Name & "]"
            End If
        End If

        lblResult.Text = "Lead Auditor List: " & mAuditorList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAuditorList = AuditorList.GetAuditorList()
        Session("mAuditorList") = mAuditorList
        dgAuditorList.DataSource = mAuditorList

        mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
        cmbDesignationList.DataSource = mDesignationList
        Session("mDesignationList_Auditor") = mDesignationList

        If Not mDesignationList.Contains(mAuditor.DesignationID) Then
            mAuditor.DesignationID = Guid.Empty
        End If


        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)          'Added by Vikrant on 25-July-2011
        If Not IsPostBack Then
            If txtName.Enabled = True Then
                txtName.Focus()
            End If


            NewRecord()
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub imgbtnDesignation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnDesignation.Click
        SetObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenAuditDesignationWindow", "OpenAuditDesignationWindow()", True)
    End Sub
    Private Sub hdnimgBtnDesignation_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnDesignation.Click
        mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
        cmbDesignationList.DataSource = mDesignationList
        Session("mDesignationList_Auditor") = mDesignationList
        cmbDesignationList.DataBind()
        upnlAuditor.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Changed by Vikrant on 25-July-2011
        MarkLog(Flypal.Util.Action.Close, "Auditor", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        If Request.QueryString("BackPage1") <> "" Then
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
        Else
            'Changed by Vikrant on 25-July-2011
            MarkLog(Flypal.Util.Action.Close, "Auditor", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
            Session("sender") = ""

            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mAuditor.Save()
            'Changed by Vikrant on 25-July-2011
            MarkLog(Flypal.Util.Action.Save, "Auditor", mAuditor.Name, Flypal.Util.ErrorType.HandledError, mAuditor.ID, EventLogID)
            NewRecord()
            DataFieldBind()
            SetTitle()
            If txtName.Enabled Then
                txtName.Focus()
            End If
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub
    Private Sub dgAuditorList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditorList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mId As Guid = mAuditorList(CInt(e.CommandArgument)).ID
                EditRecord(mId)
                txtName.DataBind()
                cmbDesignationList.DataBind()
                chkWorkingStatus.DataBind()
                SetTitle()
                If txtName.Enabled Then
                    txtName.Focus()
                End If
                'Changed by Vikrant on 25-July-2011
                MarkLog(Flypal.Util.Action.Edit, "Auditor", mAuditor.Name, Flypal.Util.ErrorType.NoError, mAuditor.ID, EventLogID)
            Case "DeleteRec"
                Dim mId As Guid = mAuditorList(CInt(e.CommandArgument)).ID
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Changed by Vikrant on 25-July-2011
        MarkLog(Flypal.Util.Action.[New], "Auditor", "", Flypal.Util.ErrorType.NoError, mAuditor.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        If txtName.Enabled Then
            txtName.Focus()
        End If
        SetTitle()
    End Sub
    Private Sub dgAuditorList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgAuditorList.Sorting
        mAuditorList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAuditorList") = mAuditorList
        dgAuditorList.DataSource = mAuditorList
        dgAuditorList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class