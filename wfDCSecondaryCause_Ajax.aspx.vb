Public Class wfDCSecondaryCause_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mDCSecondaryCause As DCSecondaryCause
    Public mDCSecondaryCauseList As DCSecondaryCauseList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDCSecondaryCause = CType(Session("mDCSecondaryCause"), DCSecondaryCause)
        mDCSecondaryCauseList = CType(Session("mDCSecondaryCauseList"), DCSecondaryCauseList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDCSecondaryCause_Ajax.aspx?" Then
            Session.Remove("mDCSecondaryCause")
            Session.Remove("mDCSecondaryCauseList")
        End If
    End Sub
    Private Sub NewRecord()
        mDCSecondaryCause = DCSecondaryCause.NewSecondaryCause(Guid.NewGuid)
        Session("mDCSecondaryCause") = mDCSecondaryCause
    End Sub
    Private Sub setObject()
        mDCSecondaryCause.SecondaryCause = Trim(txtDescription.Text)
        mDCSecondaryCause.ShortCode = Trim(txtshortcode.Text)
    End Sub
    Private Sub SetTitle()
        If Not mDCSecondaryCause.IsNew Then
            If Len(mDCSecondaryCause.ShortCode) > 15 Then
                lbltitle.Text = "Delay/Cancellation Secondary Cause [" & mDCSecondaryCause.ShortCode.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Delay/Cancellation Secondary Cause [" & mDCSecondaryCause.ShortCode & "]"
            End If
        Else
            lbltitle.Text = "Delay/Cancellation Secondary Cause [New]"
        End If
        lblResult.Text = "List of Secondary Cause(s) : " & mDCSecondaryCauseList.Count & " Record(s) Found."
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDCSecondaryCause = DCSecondaryCause.GetSecondaryCause(mId)
        Session("mDCSecondaryCause") = mDCSecondaryCause
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDCSecondaryCause = DCSecondaryCause.GetSecondaryCause(mId)
        Session("mDCSecondaryCause") = mDCSecondaryCause
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            mDCSecondaryCause = CType(Session("mDCSecondaryCause"), DCSecondaryCause)
                            DCSecondaryCause.DeleteSecondaryCause(mDCSecondaryCause.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "FlightDelayCancellation", "Description : " + mDCSecondaryCause.SecondaryCause + " Short Code : " + mDCSecondaryCause.ShortCode, Util.ErrorType.NoError, mDCSecondaryCause.ID, EventLogID)
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
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDCSecondaryCauseList = DCSecondaryCauseList.GetSecondaryCauseList("", "")
        dgSecondaryCauseList.DataSource = mDCSecondaryCauseList
        Session("mDCSecondaryCauseList") = mDCSecondaryCauseList
        DataBind()
        lblResult.Text = "List of Secondary Cause(s) : " & mDCSecondaryCauseList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If txtDescription.Enabled = True Then
                txtshortcode.Focus()
            End If
            Session("MiddleFrame") = "wfDCSecondaryCause_Ajax.aspx?"
            NewRecord()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("FlightDelayCancellationNew") And Not User.IsInRole("FlightDelayCancellationEdit")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Try
            setObject()
            mDCSecondaryCause.Save()
            If txtDescription.Enabled = True Then
                txtshortcode.Focus()
            End If
            MarkLog(Util.Action.Save, "FlightDelayCancellation", "Description : " + mDCSecondaryCause.SecondaryCause + " Short Code : " + mDCSecondaryCause.ShortCode, Util.ErrorType.HandledError, mDCSecondaryCause.ID, EventLogID)
            NewRecord()
            DataFieldBind()
            SetTitle()
            upnlDetails.Update()
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
    Private Sub dgSecondaryCauseList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSecondaryCauseList.RowCommand
        Dim index As Integer
        Dim mId As Guid

        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("FlightDelayCancellationView") And Not User.IsInRole("FlightDelayCancellationEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                index = CInt(e.CommandArgument) + dgSecondaryCauseList.PageIndex * dgSecondaryCauseList.PageSize
                mId = mDCSecondaryCauseList(index).ID

                EditRecord(mId)
                txtshortcode.Focus()
                txtDescription.DataBind()
                txtshortcode.DataBind()
                MarkLog(Util.Action.Edit, "FlightDelayCancellation", "Description : " + mDCSecondaryCause.SecondaryCause + " Short Code : " + mDCSecondaryCause.ShortCode, Util.ErrorType.NoError, mDCSecondaryCause.ID, EventLogID)
                SetTitle()
            Case "DeleteRec"
                If (Not User.IsInRole("FlightDelayCancellationDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                index = CInt(e.CommandArgument) + dgSecondaryCauseList.PageIndex * dgSecondaryCauseList.PageSize
                mId = mDCSecondaryCauseList(index).ID
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "FlightDelayCancellation", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("MiddleFrame") = "wfFlightDelayCancellationList_Ajax.aspx"
        ClearAll()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect("wfFlightDelayCancellation_Ajax.aspx")
    End Sub
    Private Sub dgSecondaryCauseList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSecondaryCauseList.Sorting
        mDCSecondaryCauseList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mDCSecondaryCauseList") = mDCSecondaryCauseList
        dgSecondaryCauseList.DataSource = mDCSecondaryCauseList
        dgSecondaryCauseList.DataBind()
    End Sub
    Private Sub dgSecondaryCauseList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSecondaryCauseList.PageIndexChanging
        dgSecondaryCauseList.PageIndex = e.NewPageIndex
        dgSecondaryCauseList.DataSource = mDCSecondaryCauseList
        Session("mDCSecondaryCauseList") = mDCSecondaryCauseList
        dgSecondaryCauseList.DataBind()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        txtshortcode.Focus()
        NewRecord()
        txtDescription.Text = ""
        txtshortcode.Text = ""
        upnlDetails.Update()
        MarkLog(Util.Action.[New], "FlightDelayCancellation", "", Util.ErrorType.NoError, mDCSecondaryCause.ID, EventLogID)
        SetTitle()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    
End Class