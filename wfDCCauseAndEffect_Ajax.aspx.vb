Public Class wfDCCauseAndEffect_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mDCCauseAndEffect As DCCauseAndEffect
    Public mDCCauseAndEffectList As DCCauseAndEffectList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDCCauseAndEffect = CType(Session("mDCCauseAndEffect"), DCCauseAndEffect)
        mDCCauseAndEffectList = CType(Session("mDCCauseAndEffectList"), DCCauseAndEffectList)
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfDCCauseAndEffect_Ajax.aspx?" Then
            Session.Remove("mDCCauseAndEffect")
            Session.Remove("mDCCauseAndEffectList")
        End If
    End Sub
    Private Sub NewRecord()
        mDCCauseAndEffect = DCCauseAndEffect.NewCauseAndEffect(Guid.NewGuid)
        Session("mDCCauseAndEffect") = mDCCauseAndEffect
    End Sub
    Private Sub setObject()
        mDCCauseAndEffect.CauseAndEffect = Trim(txtDescription.Text)
        mDCCauseAndEffect.ShortCode = Trim(txtshortcode.Text)
    End Sub
    Private Sub SetTitle()
        If Not mDCCauseAndEffect.IsNew Then
            If Len(mDCCauseAndEffect.ShortCode) > 15 Then
                lbltitle.Text = "Delay/Cancellation Cause And Effect [" & mDCCauseAndEffect.ShortCode.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Delay/Cancellation Cause And Effect [" & mDCCauseAndEffect.ShortCode & "]"
            End If
        Else
            lbltitle.Text = "Delay/CancellationCause And Effect [New]"
        End If
        lblResult.Text = "List of Cause And Effect(s) : " & mDCCauseAndEffectList.Count & " Record(s) Found."
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mDCCauseAndEffect = DCCauseAndEffect.GetCauseAndEffect(mId)
        Session("mDCCauseAndEffect") = mDCCauseAndEffect
        txtshortcode.Focus()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mDCCauseAndEffect = DCCauseAndEffect.GetCauseAndEffect(mId)
        Session("mDCCauseAndEffect") = mDCCauseAndEffect
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mDCCauseAndEffectList = DCCauseAndEffectList.GetCauseAndEffectList("", "")
        dgCauseAndEffectList.DataSource = mDCCauseAndEffectList
        Session("mDCCauseAndEffectList") = mDCCauseAndEffectList
        DataBind()
        lblResult.Text = "List of Cause And Effect(s) : " & mDCCauseAndEffectList.Count & " Record(s) Found."
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
            Session("MiddleFrame") = "wfDCCauseAndEffect_Ajax.aspx?"
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
            mDCCauseAndEffect.Save()
            txtshortcode.Focus()
            MarkLog(Util.Action.Save, "FlightDelayCancellation", "Description : " + mDCCauseAndEffect.CauseAndEffect + " Short Code : " + mDCCauseAndEffect.ShortCode, Util.ErrorType.HandledError, mDCCauseAndEffect.ID, EventLogID)
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
    Private Sub dgCauseAndEffectList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCauseAndEffectList.RowCommand
        Dim index As Integer
        Dim mId As Guid

        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("FlightDelayCancellationView") And Not User.IsInRole("FlightDelayCancellationEdit")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                index = CInt(e.CommandArgument) + dgCauseAndEffectList.PageIndex * dgCauseAndEffectList.PageSize
                mId = mDCCauseAndEffectList(index).ID

                EditRecord(mId)
                txtDescription.DataBind()
                txtshortcode.DataBind()
                MarkLog(Util.Action.Edit, "FlightDelayCancellation", "Description : " + mDCCauseAndEffect.CauseAndEffect + " Short Code : " + mDCCauseAndEffect.ShortCode, Util.ErrorType.NoError, mDCCauseAndEffect.ID, EventLogID)
                SetTitle()
            Case "DeleteRec"
                If (Not User.IsInRole("FlightDelayCancellationDelete")) Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                    Exit Sub
                End If
                index = CInt(e.CommandArgument) + dgCauseAndEffectList.PageIndex * dgCauseAndEffectList.PageSize
                mId = mDCCauseAndEffectList(index).ID
                DeleteRecord(mId)
        End Select
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
                            mDCCauseAndEffect = CType(Session("mDCCauseAndEffect"), DCCauseAndEffect)
                            DCCauseAndEffect.DeleteCauseAndEffect(mDCCauseAndEffect.ID)
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
                                MarkLog(Util.Action.Delete, "FlightDelayCancellation", "Description : " + mDCCauseAndEffect.CauseAndEffect + " Short Code : " + mDCCauseAndEffect.ShortCode, Util.ErrorType.NoError, mDCCauseAndEffect.ID, EventLogID)
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
    Private Sub dgCauseAndEffectList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCauseAndEffectList.Sorting
        mDCCauseAndEffectList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        dgCauseAndEffectList.DataSource = mDCCauseAndEffectList
        Session("mDCCauseAndEffectList") = mDCCauseAndEffectList
        dgCauseAndEffectList.DataBind()
    End Sub
    Private Sub dgCauseAndEffectList_PageIndexChanging(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCauseAndEffectList.PageIndexChanging
        dgCauseAndEffectList.PageIndex = e.NewPageIndex
        dgCauseAndEffectList.DataSource = mDCCauseAndEffectList
        Session("mDCCauseAndEffectList") = mDCCauseAndEffectList
        dgCauseAndEffectList.DataBind()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        txtshortcode.Focus()
        NewRecord()
        txtDescription.Text = ""
        txtshortcode.Text = ""
        upnlDetails.Update()
        MarkLog(Util.Action.[New], "FlightDelayCancellation", "", Util.ErrorType.NoError, mDCCauseAndEffect.ID, EventLogID)
        SetTitle()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class