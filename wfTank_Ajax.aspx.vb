Public Class wfTank_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mTank As Tank
    Public mTankList As TankList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTank = CType(Session("mTank"), Tank)
        mTankList = CType(Session("mTankList"), TankList)
    End Sub
    Private Sub SetSession()
        Session("mTank") = mTank
        Session("mTankList") = mTankList
    End Sub
    Private Sub NewRecord()
        mTank = Tank.NewTank(Guid.NewGuid)
        Session("mTank") = mTank
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mTank = Tank.GetTank(mId)
        Session("mTank") = mTank
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfTank.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mTank = Tank.GetTank(mId)
        Session("mTank") = mTank
    End Sub
    Private Sub setObject()
        mTank.Name = Trim(txtTankName.Text)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
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
                            Session("sender") = ""
                            ' mTank = CType(Session("mTank"), Tank)
                            Tank.DeleteTank(mTank.ID)
                            NewRecord()
                            txtTankName.Text = ""
                            DataFieldBind()
                            SetTitle()
                            upnlGrid.Update()
                            upnlTank.Update()
                            upnlTitle.Update()
                            'Response.Redirect("wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8114 Or ex.Number = 8115 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            txtTankName.Text = ""
                            DataFieldBind()
                            upnlTank.Update()
                            upnlTitle.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Tank", mTank.Name, Util.ErrorType.NoError, mTank.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    'Response.Redirect("wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    DataFieldBind()
                    'Response.Redirect("wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
                    DataFieldBind()
                    'Response.Redirect("wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            DataFieldBind()
            ' Response.Redirect("wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
        ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then   'Code Added
            DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mTank.IsNew Then
            lbltitle.Text = "Tank [New]"
        Else
            If Len(mTank.Name) > 15 Then
                lbltitle.Text = "Tank [" & mTank.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Tank [" & mTank.Name & "]"
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTankList = TankList.GetTankList()
        Session("mTankList") = mTankList
        dgTank.DataSource = mTankList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
      
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            NewRecord()
            txtTankName.Focus()
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnCloseTop.Click
        MarkLog(Util.Action.Close, "Tank", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""

        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        ' Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("MachineNew") And mTank.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mTank.IsNew) Then
            setObject()
            SetSession()
            MarkLog(Util.Action.Save, "Tank", User.Identity.Name & " is not Authorized User to save " & mTank.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

            Exit Sub
        End If
        Try
            setObject()
            mTank.Save()
            MarkLog(Util.Action.Save, "Tank", mTank.Name, Util.ErrorType.NoError, mTank.ID, EventLogID)
            mTank = Tank.NewTank(Guid.NewGuid)
            DataFieldBind()
            SetSession()
            SetTitle()


            upnlGrid.Update()
            upnlTank.Update()
            upnlTitle.Update()

        Catch ex As SqlException
           If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, "", MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub
    Private Sub dgTank_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTank.RowCommand
        Select Case e.CommandName
            Case "ViewRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTank.PageSize * dgTank.PageIndex
                Dim mID As Guid = mTankList(Index).ID
                Dim mName As String = mTankList(Index).Name
                If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Edit, "Tank", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecord(mID)
                txtTankName.DataBind()
                setFocus(txtTankName)
                SetTitle()
                upnlTitle.Update()
                upnlTank.Update()
                MarkLog(Util.Action.Edit, "Tank", mTank.Name, Util.ErrorType.NoError, mTank.ID, EventLogID)
                SetTitle()
            Case "DeleteRec"
                Dim Index As Integer = CInt(e.CommandArgument) + dgTank.PageSize * dgTank.PageIndex
                Dim mID As Guid = mTankList(Index).ID
                Dim mName As String = mTankList(Index).Name
                If (Not User.IsInRole("MachineDelete")) Then
                    setObject()
                    SetSession()
                    MarkLog(Util.Action.Delete, "Tank", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecord(Mid)
        End Select
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'If (Not User.IsInRole("MachineNew") And mTank.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mTank.IsNew) Then
        '    setObject()
        '    SetSession()
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfTank.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage")
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        MarkLog(Util.Action.[New], "Tank", "", Util.ErrorType.NoError, mTank.ID, EventLogID)
        NewRecord()
        txtTankName.Text = ""
        DataFieldBind()
        If txtTankName.Enabled = True Then
            setFocus(txtTankName)
        End If
        SetTitle()
        upnlGrid.Update()
    End Sub
#End Region

  
End Class