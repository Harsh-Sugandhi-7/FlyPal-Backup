'Added By Vikrant On 20-Aug-2015

Public Class wfAuditDepartment_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mDepartment As AuditDepartment
    Public mDepartmentList As AuditDepartmentList

    Dim EventLogID As Guid              'Added by Vikrant on 22-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDepartment = CType(Session("mDepartment"), AuditDepartment)
        mDepartmentList = CType(Session("mDepartmentList"), AuditDepartmentList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mDepartment")
        Session.Remove("mDepartmentList")
    End Sub
    Private Sub NewRecord()
        mDepartment = AuditDepartment.NewAuditDepartment
        Session("mDepartment") = mDepartment
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDepartment = AuditDepartment.GetChildAuditDepartment(mId)
        Session("mDepartment") = mDepartment
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDepartment = AuditDepartment.GetChildAuditDepartment(mId)
        Session("mDepartment") = mDepartment
    End Sub
    Private Sub setObject()
        mDepartment.Name = Trim(txtName.Text)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim mDeptName As String
                        Try
                            Session("sender") = ""
                            mDepartment = CType(Session("mDepartment"), AuditDepartment)
                            mDeptName = mDepartment.Name
                            Department.DeleteDepartment(mDepartment.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            txtName.Text = ""
                            upnlAuditDepartment.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed by Vikrant on 22-July-2011
                                MarkLog(Flypal.Util.Action.Delete, "Department", mDeptName, Flypal.Util.ErrorType.NoError, mDepartment.ID, EventLogID)
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
        If mDepartment.IsNew Then
            lbltitle.Text = "Department [New]"
        Else
            If Len(mDepartment.Name) > 15 Then
                lbltitle.Text = "Department [" & mDepartment.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Department [" & mDepartment.Name & "]"
            End If
        End If
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        lblResult.Text = "Department List: " & mDepartmentList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDepartmentList = AuditDepartmentList.GetAuditDepartmentList()
        Session("mDepartmentList") = mDepartmentList
        dgDepartmentList.DataSource = mDepartmentList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)       'Added by Vikrant on 22-July-2011
        If Not IsPostBack Then
            If txtName.Enabled = True Then
                txtName.Focus()
            End If

            NewRecord()
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mDepartment.Save()
            'Changed by Vikrant on 22-July-2011
            MarkLog(Flypal.Util.Action.Save, "Department", mDepartment.Name, Flypal.Util.ErrorType.HandledError, mDepartment.ID, EventLogID)
            mDepartment = AuditDepartment.NewAuditDepartment()
            NewRecord()
            DataFieldBind()
            SetTitle()
            If txtName.Enabled Then
                txtName.Focus()
            End If
            upnlAuditDepartment.Update()
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
    Private Sub dgDepartmentList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDepartmentList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mId As Guid = mDepartmentList(CInt(e.CommandArgument)).ID
                EditRecord(mId)
                txtName.DataBind()
                SetTitle()
                If txtName.Enabled = True Then
                    txtName.Focus()
                End If
                'Changed by Vikrant on 22-July-2011
                MarkLog(Flypal.Util.Action.Edit, "Department", mDepartment.Name, Flypal.Util.ErrorType.NoError, mDepartment.ID, EventLogID)
            Case "DeleteRec"
                Dim mId As Guid = mDepartmentList(CInt(e.CommandArgument)).ID
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Changed by Vikrant on 22-July-2011
        MarkLog(Flypal.Util.Action.[New], "Department", "", Flypal.Util.ErrorType.NoError, mDepartment.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        If txtName.Enabled = True Then
            txtName.Focus()
        End If
        SetTitle()
    End Sub
    Private Sub dgDepartmentList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgDepartmentList.Sorting
        mDepartmentList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mDepartmentList") = mDepartmentList
        dgDepartmentList.DataSource = mDepartmentList
        dgDepartmentList.DataBind()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Changed by Vikrant on 22-July-2011
        MarkLog(Flypal.Util.Action.Close, "Department", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        If Request.QueryString("ChildPage2") = "wfTask.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&AuditStandardID=" & Request.QueryString("AuditStandardID") & "&Type=" & Request.QueryString("Type"))

        ElseIf Request.QueryString("BackPage2") <> "" Then
            Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&AuditStandardID=" & Request.QueryString("AuditStandardID") & "&Type=" & Request.QueryString("Type"))
        Else
            Session("sender") = ""
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class