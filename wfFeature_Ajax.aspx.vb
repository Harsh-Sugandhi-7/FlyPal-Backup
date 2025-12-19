Public Class wfFeature_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mFeature As Feature
    Public mFeatureList As FeatureList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mFeature = CType(Session("mFeature"), Feature)
        mFeatureList = CType(Session("mFeatureList"), FeatureList)
    End Sub
    Private Sub SetSession()
        Session("mFeature") = mFeature
        Session("mFeatureList") = mFeatureList
    End Sub
    Private Sub NewRecord()
        mFeature = Feature.NewFeature()
        Session("mFeature") = mFeature
        lbltitle.Text = "Feature [New]"
        txtFeatureName.Text = ""
        pnlValidationSummary.Update()
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mFeature = Feature.GetFeature(mId)
        Session("mFeature") = mFeature
        If Len(mFeature.Name) > 15 Then
            lbltitle.Text = "Feature [" & mFeature.Name.Substring(0, 15) & "...]"
        Else
            lbltitle.Text = "Feature [" & mFeature.Name & "]"
        End If
        pnlValidationSummary.Update()
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        GridBind()
        mFeature = Feature.GetFeature(mId)
        Session("mFeature") = mFeature
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
    End Sub
    Private Sub setObject()
        mFeature.Name = Trim(txtFeatureName.Text)
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
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Feature.DeleteFeature(mFeature.ID)
                            NewRecord()
                            txtFeatureName.DataBind()
                            DataFieldBind()
                            upnlFeatureDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                Exit Sub
                            End If
                        Finally
                            MarkLog(Util.Action.Delete, "Feature", mFeature.Name, Util.ErrorType.NoError, mFeature.ID, EventLogID)
                            NewRecord()
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                    End If
                    If MSGBoxCtrl.Sender = "Delete" Then
                        txtFeatureName.Text = ""
                        NewRecord()
                        DataFieldBind()
                        upnlFeatureDetails.Update()
                    End If
                    GridBind()
                Case MsgBoxResult.Ok
                    GridBind()
            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mFeatureList = FeatureList.GetFeatureList(False)
        dgFeature.DataSource = mFeatureList
        Session("mFeatureList") = mFeatureList
        txtFeatureName.DataBind()
        upnlFeatureDetails.Update()
        GridBind()
    End Sub
    Private Sub GridBind()
        dgFeature.DataSource = mFeatureList
        dgFeature.DataBind()
        lblResult.Text = "Feature List: " & mFeatureList.Count & " Record(s) Found."
        upnlGridView.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If txtFeatureName.Enabled = True Then
            setFocus(txtFeatureName)
        End If
        If Not IsPostBack Then
            NewRecord()
            DataFieldBind()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If IsValid Then
                setObject()
                mFeature.Save()
                MarkLog(Util.Action.Save, "Feature", mFeature.Name, Util.ErrorType.NoError, mFeature.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
            Else
                GridBind()
                pnlValidationSummary.Update()
                Exit Sub
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            DataFieldBind()
        End Try
    End Sub
    Private Sub dgFeatureList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgFeature.RowCommand
        Select Case e.CommandName
            Case "EditView"
                Dim index As Integer = CInt(e.CommandArgument) + dgFeature.PageIndex * dgFeature.PageSize
                Dim mID As Guid = mFeatureList(index).ID
                EditRecord(mID)
                setFocus(txtFeatureName)
                txtFeatureName.DataBind()
                upnlFeatureDetails.Update()
                GridBind()
                MarkLog(Util.Action.Edit, "Feature", mFeature.Name, Util.ErrorType.NoError, mFeature.ID, EventLogID)
             Case "Remove"
                 Dim index As Integer = CInt(e.CommandArgument) + dgFeature.PageIndex * dgFeature.PageSize
                Dim mID As Guid = mFeatureList(index).ID
                'If mMachine.MachineFeatures.Contains(mID, "") = True Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "ReferenceFeatureDelete")
                '    NewFeature()
                '    GridBind()
                '    Exit Sub
                'Else
                DeleteRecord(mID)
                'End If
        End Select
    End Sub
    Private Sub dgFeatureList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgFeature.PageIndexChanging
        dgFeature.PageIndex = e.NewPageIndex
        dgFeature.DataSource = mFeatureList
        dgFeature.DataBind()
        Session("mFeatureList") = mFeatureList
        upnlGridView.Update()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        MarkLog(Util.Action.[New], "Feature", "", Util.ErrorType.NoError, mFeature.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        If txtFeatureName.Enabled = True Then
            setFocus(txtFeatureName)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Feature", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        ''If IsNothing(Request.QueryString("BackPage2")) Or Request.QueryString("BackPage2") = "" Then
        ''    Session("MiddleFrame") = ""
        ''    Response.Redirect("Dashboard.aspx")
        ''Else
        ''    Response.Redirect(Request.QueryString("BackPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ''End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class