Public Class wfDDUpComingEvents_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mUpcomingEvent As UpcomingEvent
    Public mUpcomingEventList As UpcomingEventList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mUpcomingEvent = CType(Session("mUpcomingEvent"), UpcomingEvent)
        mUpcomingEventList = CType(Session("mUpcomingEventList"), UpcomingEventList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mUpcomingEvent")
        Session.Remove("mUpcomingEventList")
    End Sub
    Private Sub MakeControlsBlank()
        txtEventDate.Text = ""
        txtEventDetails.Text = ""
    End Sub
    Private Sub NewRecord()
        mUpcomingEvent = UpcomingEvent.NewUpcomingEvent(Guid.NewGuid)
        Session("mUpcomingEvent") = mUpcomingEvent
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mUpcomingEvent = UpcomingEvent.GetUpcomingEvent(mId)
        Session("mUpcomingEvent") = mUpcomingEvent
        SetTitle()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        EditRecord(mId)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub setObject()
        mUpcomingEvent.EventDate = txtEventDate.Text
        mUpcomingEvent.EventDesc = Trim(txtEventDetails.Text)
        mUpcomingEvent.InfoToShow = chkToShow.Checked
        Session("mUpcomingEvent") = mUpcomingEvent
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mUpcomingEvent = Session("mUpcomingEvent")
                            UpcomingEvent.DeleteUpcomingEvent(mUpcomingEvent.ID)
                            MarkLog(Util.Action.Delete, "UpcomingEvent", mUpcomingEvent.EventDateFormatted.ToString, Util.ErrorType.NoError, mUpcomingEvent.ID, EventLogID)
                            NewRecord()
                            MakeControlsBlank()
                            DataFieldBind()
                            ControlVisibility()
                            upnlDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                DataFieldBind()
                                MakeControlsBlank()
                                upnlDetails.Update()
                                Exit Sub
                            End If
                        Finally

                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then

                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then

                    End If
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
    
    Private Sub SetTitle()
        If mUpcomingEvent.IsNew = True Then
            lblTitle.Text = "Event [New]"
        Else
            If Len(mUpcomingEvent.EventDesc) > 15 Then
                lblTitle.Text = "Event [" & mUpcomingEvent.EventDesc.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Event [" & mUpcomingEvent.EventDesc & "]"
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mUpcomingEventList = UpcomingEventList.GetEventList()
        Session("mUpcomingEventList") = mUpcomingEventList
        dgEventDetails.DataSource = mUpcomingEventList
        DataBind()
        lblResult.Text = "Event List: " & mUpcomingEventList.Count & " Record(s) Found."
    End Sub
    Private Sub ControlVisibility()
        If mUpcomingEventList.Count > 5 Then
            btnBackTop.Visible = True
            btnSaveTop.Visible = True
        Else
            btnBackTop.Visible = False
            btnSaveTop.Visible = False
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfDDUpComingEvents_Ajax.aspx"
            NewRecord()
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnSaveTop.Click
        If (Not User.IsInRole("UpcomingEventNew") And mUpcomingEvent.IsNew) Or (Not User.IsInRole("UpcomingEventEdit") And Not mUpcomingEvent.IsNew) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Try
            If IsValid Then
                setObject()
                mUpcomingEvent.Save()
                MarkLog(Util.Action.Save, "UpcomingEvent", mUpcomingEvent.EventDateFormatted.ToString, Util.ErrorType.HandledError, mUpcomingEvent.ID, EventLogID)
                NewRecord()
                MakeControlsBlank()
                DataFieldBind()
                ControlVisibility()
            End If

        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End Try
    End Sub
    Private Sub dgEventDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgEventDetails.PageIndexChanging
        dgEventDetails.PageIndex = e.NewPageIndex
        dgEventDetails.DataSource = mUpcomingEventList
        Session("mUpcomingEventList") = mUpcomingEventList
        dgEventDetails.DataBind()
    End Sub
    Private Sub dgEventDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEventDetails.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("UpcomingEventView") And Not User.IsInRole("UpcomingEventEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Dim index As Integer = CInt(e.CommandArgument) + dgEventDetails.PageIndex * dgEventDetails.PageSize
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)

                EditRecord(mID)
                txtEventDate.Text = mUpcomingEvent.EventDateFormatted.ToString
                txtEventDate.DataBind()
                txtEventDetails.DataBind()
                chkToShow.DataBind()


                MarkLog(Util.Action.Edit, "UpcomingEvent", mUpcomingEvent.EventDateFormatted.ToString, Util.ErrorType.NoError, mUpcomingEvent.ID, EventLogID)
            Case "DeleteRec"
                If (Not User.IsInRole("UpcomingEventDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                'Dim index As Integer = CInt(e.CommandArgument) + dgEventDetails.PageIndex * dgEventDetails.PageSize
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MakeControlsBlank()
        DataFieldBind()
        MarkLog(Util.Action.[New], "UpcomingEvent", "", Util.ErrorType.NoError, mUpcomingEvent.ID, EventLogID)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackTop.Click, btnBack.Click
        Session("sender") = ""
        MarkLog(Util.Action.Close, "UpcomingEvent", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("MiddleFrame") = ""
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    
#End Region



End Class