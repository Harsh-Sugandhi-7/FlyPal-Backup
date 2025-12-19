
Public Class wfPrimaryModel_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mPrimaryModel As PrimaryModel
    Public mPrimaryModelList As PrimaryModelList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub GetSession()
        mPrimaryModel = Session("mPrimaryModel")
        mPrimaryModelList = Session("mPrimaryModelList")
    End Sub
    Private Sub SetSession()
        Session("mPrimaryModel") = mPrimaryModel
        Session("mPrimaryModelList") = mPrimaryModelList
    End Sub
    Private Sub NewRecord()
        mPrimaryModel = PrimaryModel.NewPrimaryModel(Guid.NewGuid)
        Session("mPrimaryModel") = mPrimaryModel
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mPrimaryModel = PrimaryModel.GetPrimaryModel(mId)
        Session("mPrimaryModel") = mPrimaryModel
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mPrimaryModel = PrimaryModel.GetPrimaryModel(mId)
        Session("mPrimaryModel") = mPrimaryModel
    End Sub
    Private Sub setObject()
        mPrimaryModel.Name = txtName.Text
        mPrimaryModel.FixedWing = rdbFixedWing.Checked
        mPrimaryModel.RotaryWing = rdbRotaryWing.Checked
        Session("mPrimaryModel") = mPrimaryModel
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'If CType(Session("sender"), String) = "Delete" Then
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            'Session("sender") = ""
                            mPrimaryModel = Session("mPrimaryModel")
                            PrimaryModel.PrimaryModelDelete(mPrimaryModel.ID)
                            Session.Remove("mPrimaryModel")
                            NewRecord()
                            DataFieldBind()
                            lblTitle.Text = "Primary Model Information [New]"
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
                            ElseIf ex.Number = 547 Then
                                If ex.Message.Contains("FK_tabModel_tabPrimaryModel") Then
                                    stringInfo = "Model"
                                End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "PrimaryModel", "Can't delete :" & mPrimaryModel.Name & " is Currently in use", Util.ErrorType.NoError, mPrimaryModel.ID, EventLogID)
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "PrimaryModel", mPrimaryModel.Name, Util.ErrorType.NoError, mPrimaryModel.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
        upnlPrimaryModel.Update()
    End Sub
    Private Sub ControlVisibility()

    End Sub
    Private Sub SetTitle()

    End Sub
    Private Sub DisableName(mID As Guid)

        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerModel(mID)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If

    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mPrimaryModelList = PrimaryModelList.GetPrimaryModelList()
        dgPrimaryModel.DataSource = mPrimaryModelList
        Session("mPrimaryModelList") = mPrimaryModelList
        DataBind()
        lblSearch.Text = "Primary Model List: " & mPrimaryModelList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("sender") = "" Then

            NewRecord()
            DataFieldBind()
        End If
        ControlVisibility()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "PrimaryModel", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("mPrimaryModel")
            Session.Remove("mPrimaryModelList")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mPrimaryModel.Save()

            MarkLog(Util.Action.Save, "PrimaryModel", mPrimaryModel.Name, Util.ErrorType.HandledError, mPrimaryModel.ID, EventLogID)
            NewRecord()
            Session("Save") = True
            DataFieldBind()
            SetSession()
            lblTitle.Text = "Primary Model Information [New]"
            upnlPrimaryModel.Update()
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
            End If
        End Try
    End Sub
    Private Sub dgPrimaryModel_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPrimaryModel.RowCommand

        Dim Idx As Int32
        Dim mId As Guid

        Select Case e.CommandName
            Case "EditView"
                Idx = CInt(e.CommandArgument)
                mId = mPrimaryModelList(Idx).ID
                EditRecord(mId)
                txtName.Text = mPrimaryModel.Name
                rdbFixedWing.Checked = mPrimaryModel.FixedWing
                rdbRotaryWing.Checked = mPrimaryModel.RotaryWing
                MarkLog(Util.Action.Edit, "PrimaryModel", mPrimaryModel.Name, Util.ErrorType.NoError, mPrimaryModel.ID, EventLogID)
                If Len(mPrimaryModel.Name) > 15 Then
                    lblTitle.Text = "Primary Model Information [" & mPrimaryModel.Name.Substring(0, 15) & "...]"
                Else
                    lblTitle.Text = "Primary Model Information [" & mPrimaryModel.Name & "]"
                End If
                DisableName(mId)
            Case "Remove"
                Idx = CInt(e.CommandArgument)
                mId = mPrimaryModelList(Idx).ID
                DeleteRecord(mId)
                MarkLog(Util.Action.Delete, "PrimaryModel", mPrimaryModel.Name, Util.ErrorType.HandledError, mPrimaryModel.ID, EventLogID)
        End Select
    End Sub
    Private Sub dgPrimaryModel_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgPrimaryModel.PageIndexChanging
        dgPrimaryModel.PageIndex = e.NewPageIndex
        dgPrimaryModel.DataSource = mPrimaryModelList
        Session("mPrimaryModelList") = mPrimaryModelList
        dgPrimaryModel.DataBind()

    End Sub

    Private Sub dgPrimaryModel_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgPrimaryModel.Sorting
        Session("mPrimaryModelList") = mPrimaryModelList
        dgPrimaryModel.DataSource = mPrimaryModelList
        dgPrimaryModel.DataBind()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        MarkLog(Util.Action.[New], "PrimaryModel", "", Util.ErrorType.NoError, mPrimaryModel.ID, EventLogID)
        NewRecord()
        txtName.Text = ""
        txtName.Enabled = True
        rdbFixedWing.Checked = True
        rdbRotaryWing.Checked = False
        lblTitle.Text = "Primary Model Information [New]"
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub htnBtnManufacturer_Click(sender As Object, e As System.EventArgs) Handles htnBtnManufacturer.Click
        DataFieldBind()
    End Sub
#End Region

End Class