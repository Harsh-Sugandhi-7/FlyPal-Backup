'AJAX Conversion by vikrant on 21-Aug-2015

Public Class wfAuditDesignation_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mDesignation As Designation
    Public mDesignationList As DesignationList

    Dim EventLogID As Guid           'Added by Vikrant on 22-July-2011
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDesignation = CType(Session("mDesignation"), Designation)
        mDesignationList = CType(Session("mDesignationList"), DesignationList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mDesignation")
        Session.Remove("mDesignationList")
    End Sub
    Private Sub NewRecord()
        mDesignation = Designation.NewDesignation()
        Session("mDesignation") = mDesignation
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDesignation = Designation.GetDesignation(mId)
        Session("mDesignation") = mDesignation
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDesignation = Designation.GetDesignation(mId)
        Session("mDesignation") = mDesignation
    End Sub
    Private Sub setObject()
        mDesignation.Name = Trim(txtName.Text)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim mDesignationName As String
                        Try
                            Session("sender") = ""
                            mDesignation = CType(Session("mDesignation"), Designation)
                            mDesignationName = mDesignation.Name
                            Designation.DeleteDesignation(mDesignation.ID)
                            NewRecord()
                            DataFieldBind()
                            txtName.Text = ""
                            SetTitle()
                            upnlAuditDesignation.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Catch ex As Exception
                            If ex.Message.Contains("Record in use. Cannot delete record.") Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                DataFieldBind()
                                txtName.Text = ""
                                SetTitle()
                                upnlAuditDesignation.Update()
                            End If
                        Finally
                            If msgCount = 0 Then
                                'Changed by Vikrant on 25-July-2011
                                MarkLog(Flypal.Util.Action.Delete, "Designation", mDesignationName, Flypal.Util.ErrorType.NoError, mDesignation.ID, EventLogID)
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
        If mDesignation.IsNew Then
            lbltitle.Text = "Designation [New]"
        Else
            If Len(mDesignation.Name) > 15 Then
                lbltitle.Text = "Designation [" & mDesignation.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Designation [" & mDesignation.Name & "]"
            End If
        End If
        'Added by Amrita on 10-Dec-07 for displaying no of records in data grid.
        lblResult.Text = "Designation List: " & mDesignationList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDesignationList = DesignationList.GetDesignationList()
        Session("mDesignationList") = mDesignationList
        dgDesignationList.DataSource = mDesignationList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)             'Added by Vikrant on 25-July-2011
        If Not IsPostBack Then
            If txtName.Enabled = True Then
                txtName.Focus()
            End If

            NewRecord()
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Flypal.Util.Action.Close, "Designation", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        If Request.QueryString("BackPage1") <> "" Then
            Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1"))
        Else

            Session("sender") = ""
            Session("MiddleFrame") = ""
            Response.Redirect("Dashboard.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mDesignation.Save()
            'Changed by Vikrant on 25-July-2011
            MarkLog(Flypal.Util.Action.Save, "Designation", mDesignation.Name, Flypal.Util.ErrorType.HandledError, mDesignation.ID, EventLogID)
            mDesignation = Designation.NewDesignation()
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
    Private Sub dgDesignationList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDesignationList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mId As Guid = mDesignationList(CInt(e.CommandArgument)).ID
                EditRecord(mId)
                txtName.DataBind()
                SetTitle()
                If txtName.Enabled = True Then
                    txtName.Focus()
                End If
                'Changed by Vikrant on 25-July-2011
                MarkLog(Flypal.Util.Action.Edit, "Designation", mDesignation.Name, Flypal.Util.ErrorType.NoError, mDesignation.ID, EventLogID)
            Case "DeleteRec"
                Dim mId As Guid = mDesignationList(CInt(e.CommandArgument)).ID
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Changed by Vikrant on 25-July-2011
        MarkLog(Flypal.Util.Action.[New], "Designation", "", Flypal.Util.ErrorType.NoError, mDesignation.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        If txtName.Enabled Then
            txtName.Focus()
        End If
        SetTitle()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class