'Added by Vikrant

Public Class wfTraining_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mTraining As Training

    Public mTrainingList As TrainingList
    Public mTrainingTypeList As TrainingTypeList
    'Public mModelList As ModelList
    'Public BackPage As String
    Public IsFromRenewal As String = ""
    Public mIsRenew As Boolean = False
    Public mnIsRenew As Boolean = False
    'Added by Vikrant on 20-July-2011
    Dim EventLogID As Guid

    'AJAX
    Public mTrainingDesignation As TrainingDesignation
    Public mTrainingDesignationList As TrainingDesignationList
    Public mTrainingDesignationID As Guid
    Public mDesignationList As DesignationList
    Public mTrainingOrgDetailID As Guid
    Public mTrainingOrgList As TrainingOrgList
    Public mTrainingOrg As TrainingOrg
    Public mTrainingOrgDetail As TrainingOrgDetail
    Public mTrainingOrgDetailList As TrainingOrgDetailList
    Public mTrainingModelList As TrainingModelList
    Public mTrainingModelID As Guid
    Public mTrainingModel As TrainingModel
    Public mModels As Models
    Public mTrainingType As TrainingType
    'End
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTraining = CType(Session("mTraining"), Training)
        mTrainingList = CType(Session("mTrainingList"), TrainingList)

        mTrainingTypeList = CType(Session("mTrainingTypeList"), TrainingTypeList)
        'mModelList = CType(Session("mModelList"), ModelList)
        IsFromRenewal = Request.QueryString("IsFromRenewal")
        'AJAX
        mTrainingDesignation = Session("mTrainingDesignation")
        mTrainingDesignationList = Session("mTrainingDesignationList")
        mDesignationList = Session("mDesignationList")
        mTrainingOrgDetail = Session("mTrainingOrgDetail")
        mTrainingOrgDetailList = Session("mTrainingOrgDetailList")
        mTrainingOrgList = Session("mTrainingOrgList")
        mTrainingModelList = CType(Session("mTrainingModelList"), TrainingModelList)
        mModels = CType(Session("mModels"), Models)
        mTrainingType = Session("mTrainingType")
        'END
    End Sub
    Private Sub SetSession()
        Session("mTraining") = mTraining
        Session("mTrainingList") = mTrainingList

        Session("mTrainingTypeList") = mTrainingTypeList
        'Session("mModelList") = mModelList
    End Sub
    'AJAX
    Private Sub SetSessionForTrainingDesg()
        Session("mTrainingDesignation") = mTrainingDesignation
        Session("mTrainingDesignationList") = mTrainingDesignationList
        Session("mDesignationList") = mDesignationList
    End Sub
    'AJAX
    Private Sub SetSessionForTrainingOrg()
        Session("mTrainingOrgDetail") = mTrainingOrgDetail
        Session("mTrainingOrgDetailList") = mTrainingOrgDetailList
        Session("mTrainingOrgList") = mTrainingOrgList
    End Sub
    'AJAX
    Private Sub SetSessionForTrainingModel()
        Session("mTrainingModel") = mTrainingModel
        Session("mTrainingModelList") = mTrainingModelList
        Session("mModels") = mModels
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfTraining_Ajax.aspx" Then
            Session.Remove("mTraining")
            Session.Remove("mTrainingList")

            Session.Remove("mTrainingTypeList")
            'Session.Remove("mModelList")

            Session.Remove("NewTraining")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub NewRecord()
        mTraining = Training.NewTraining(Guid.NewGuid)
        mTrainingTypeList = TrainingTypeList.GetTrainingTypeList(, "<SELECT>")
        'mModelList = ModelList.GetModelList(1, "", "{00000000-0000-0000-0000-000000000000}", "{00000000-0000-0000-0000-000000000000}", "<SELECT>")
        SetSession()
        txtName.Enabled = True
    End Sub
    Private Sub EditRecord(ByVal ID As Guid)
        mTraining = Training.GetTraining(ID)
        Session("mTraining") = mTraining
    End Sub
    Private Sub DeleteRecord(ByVal ID As Guid)
        'AJAX Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfTraining.aspx?MsgResult=0&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCntrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mTraining = Training.GetTraining(ID)
        Session("mTraining") = mTraining
    End Sub
    'AJAX
    Private Sub ClearControls()
        txtName.Text = ""
        cmbTrainingType.SelectedIndex = 0
        chkRecurringStatus.Checked = False
        txtFreqInMonths.Text = ""
        txtWarningDays.Text = ""
        lnkTrainingDesignation.Visible = False
        lnkTrainingModel.Visible = False
        lnkTrainingOrgDetail.Visible = False
    End Sub
    'AJAX
    Private Sub setObjectForTrainingOrg()
        Dim chkValue As New CheckBox
        Dim mIsNotSelect As Boolean = True

        For i As Integer = 0 To mTrainingOrgList.Count - 1
            chkValue = CType(Me.dgTrainingOrgDetailList.Rows(i).FindControl("chkTrainingOrg"), CheckBox)
            mTrainingOrgList(i).IsSelect = chkValue.Checked

            If chkValue.Checked = True Then
                If Not mTrainingOrgDetailList.Contains(mTrainingOrgList(i).ID, TrainingOrgDetailList.SearchWith.TrainingOrgID) Then
                    mTrainingOrgDetail = TrainingOrgDetail.NewTrainingOrgDetail(mTrainingOrgDetailID)
                    mTrainingOrgDetail.TrainingID = mTraining.ID
                    mTrainingOrgDetail.TrainingOrgID = mTrainingOrgList(i).ID
                    If mTrainingOrgDetail.IsValid Then
                        mTrainingOrgDetail = CType(mTrainingOrgDetail.Save, TrainingOrgDetail)
                        MarkLog(Flypal.Util.Action.Save, "Training Organization Detail", "Training : " & mTraining.Name & " Training Org: " & mTrainingOrgList(i).Name, Flypal.Util.ErrorType.HandledError, mTrainingOrgDetail.ID, EventLogID)
                    End If
                End If
            Else
                If mTrainingOrgDetailList.Contains(mTrainingOrgList(i).ID, TrainingOrgDetailList.SearchWith.TrainingOrgID) Then
                    TrainingOrgDetail.DeleteTrainingOrgDetail(mTrainingOrgDetailList.Item(mTrainingOrgList(i).ID, TrainingOrgDetailList.SearchWith.TrainingOrgID).ID)
                    'MarkLog(Flypal.Util.Action.Delete, "Training Organization Detail", "Training : " & mTraining.Name & " Training Org: " & mTrainingOrgList(i).Name, Flypal.Util.ErrorType.HandledError, mTrainingOrgDetail.ID, EventLogID)
                End If
            End If
        Next
    End Sub
    'AJAX
    Private Sub setObjectForTrainingModel()
        Dim chkValue As New CheckBox

        For i As Integer = 0 To mModels.Count - 1
            chkValue = CType(Me.dgTrainingModelList.Rows(i).FindControl("chkModel"), CheckBox)
            mModels(i).IsSelected = chkValue.Checked

            If chkValue.Checked = True Then
                If Not mTrainingModelList.Contains(mModels(i).ID, TrainingModelList.SearchWith.ModelID) Then
                    mTrainingModel = TrainingModel.NewTrainingModel(mTrainingModelID)
                    mTrainingModel.TrainingID = mTraining.ID
                    mTrainingModel.ModelID = mModels(i).ID
                    If mTrainingModel.IsValid Then
                        mTrainingModel = CType(mTrainingModel.Save, TrainingModel)
                        MarkLog(Flypal.Util.Action.Save, "Training Model", "Training : " & mTraining.Name & " Training Model: " & mModels(i).Name, Flypal.Util.ErrorType.HandledError, mTrainingModel.ID, EventLogID)
                    End If
                End If
            Else
                If mTrainingModelList.Contains(mModels(i).ID, TrainingModelList.SearchWith.ModelID) Then
                    TrainingModel.DeleteTrainingModel(mTrainingModelList.Item(mModels(i).ID, TrainingModelList.SearchWith.ModelID).ID)
                End If
            End If
        Next
    End Sub
    'AJAX
    Private Sub setObjectForTrainingDesg()
        Dim chkValue As New CheckBox

        For i As Integer = 0 To mDesignationList.Count - 1
            chkValue = CType(Me.dgTrainingDesignationList.Rows(i).FindControl("chkDesignation"), CheckBox)
            mDesignationList(i).IsSelect = chkValue.Checked

            If chkValue.Checked = True Then
                If Not mTrainingDesignationList.Contains(mDesignationList(i).ID, TrainingDesignationList.SearchWith.DesignationID) Then
                    mTrainingDesignation = TrainingDesignation.NewTrainingDesignation(mTrainingDesignationID)
                    mTrainingDesignation.TrainingID = mTraining.ID
                    mTrainingDesignation.DesignationID = mDesignationList(i).ID
                    If mTrainingDesignation.IsValid Then
                        mTrainingDesignation = CType(mTrainingDesignation.Save, TrainingDesignation)
                        MarkLog(Flypal.Util.Action.Save, "Training Designation ", "Training : " & mTraining.Name & " Training Org: " & mDesignationList(i).Name, Flypal.Util.ErrorType.HandledError, mTrainingDesignation.ID, EventLogID)
                    End If
                End If
            Else
                If mTrainingDesignationList.Contains(mDesignationList(i).ID, TrainingDesignationList.SearchWith.DesignationID) Then
                    TrainingDesignation.DeleteTrainingDesignation(mTrainingDesignationList.Item(mDesignationList(i).ID, TrainingDesignationList.SearchWith.DesignationID).ID)

                End If
            End If
        Next

    End Sub
    Private Sub setObject()
        With mTraining
            .Name = Trim(txtName.Text)
            .TrainingTypeID = New Guid(cmbTrainingType.SelectedValue)
            .RecurringStatus = chkRecurringStatus.Checked
            If txtFreqInMonths.Text = "" Then
                .FreqInMonths = 0
            Else
                .FreqInMonths = Trim(txtFreqInMonths.Text)
            End If
            If txtWarningDays.Text = "" Then
                .WarningDays = 0
            Else
                .WarningDays = Trim(txtWarningDays.Text)
            End If

            '.ModelID = New Guid(cmbModelList.SelectedValue)
        End With
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCntrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCntrl.Sender = "Delete" Then
                        Try
                            Dim mTraining As Training
                            Session("sender") = ""
                            Session("NewTraining") = "False"
                            mTraining = CType(Session("mTraining"), Training)
                            Training.DeleteTraining(mTraining.ID)
                            NewRecord()
                            DataFieldBind()
                            ControlVisibility1()
                            ClearControls()
                            SetTitle()
                            upnlTrainingDetails.Update()
                            upnlGridView.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind()
                            ControlVisibility1()
                            ClearControls()
                            SetTitle()
                            upnlTrainingDetails.Update()
                            upnlGridView.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Training", mTraining.Name, Flypal.Util.ErrorType.NoError, mTraining.ID, EventLogID)
                            End If
                        End Try
                    ElseIf MSGBoxCntrl.Sender = "DeleteTrainingType" Then
                        Try
                            Session("Sender") = ""
                            mTrainingType = Session("mTrainingType")
                            TrainingType.DeleteTrainingType(mTrainingType.ID)
                            NewRecordTrainingType()
                            DataFieldBindForTrainingType()
                            txtTrainingTypeName.Text = ""
                            txtTrainingTypeName.DataBind()
                            lblTitleTrainingType.Text = "Training Type Information [New]"
                            upnlTrainingType.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Util.Action.Delete, "TrainingType", "Can't delete :" & mTrainingType.Name & " is Currently in use", Util.ErrorType.NoError, mTrainingType.ID, EventLogID) 'changes by Vikrant on 20-July-2011
                                MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecordTrainingType()
                            txtTrainingTypeName.Text = ""
                            txtTrainingTypeName.DataBind()
                            lblTitleTrainingType.Text = "Training Type Information [New]"
                            upnlTrainingType.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Uncommented Line & Made changes by Vikrant on 20-July-2011
                                MarkLog(Util.Action.Delete, "TrainingType", mTrainingType.Name, Util.ErrorType.NoError, mTrainingType.ID, EventLogID)
                            End If
                        End Try

                    ElseIf MSGBoxCntrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        If mTraining.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            Save()
                            Dim mopenas As String = Request.QueryString("Type")
                            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                Exit Sub
                            End If
                            Response.Redirect("Index.aspx")
                        Else
                            upnlValidationSummary.Update() 'AJAX
                            Session.Remove("IsValid")
                            'AJAX Response.Redirect("wfTraining.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCntrl.Sender = "Delete" Then
                        Session("sender") = ""
                        NewRecord()
                        SetTitle()
                        ClearControls()
                        ControlVisibility1()
                        upnlTrainingDetails.Update()
                    End If
                    If MSGBoxCntrl.Sender = "DeleteTrainingType" Then
                        Session("sender") = ""
                        NewRecordTrainingType()
                        txtTrainingTypeName.Text = ""
                        txtTrainingTypeName.DataBind()
                        lblTitleTrainingType.Text = "Training Type Information [New]"
                        upnlTrainingType.Update()
                    End If
                    If MSGBoxCntrl.Sender = "Close" Then
                        Session("sender") = ""
                        Session("MiddleFrame") = ""
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        Response.Redirect("Index.aspx")
                    End If
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
        If mTraining.IsNew Then
            lbltitle.Text = "Training Information [New]"
        Else
            If Len(mTraining.Name) > 15 Then
                lbltitle.Text = "Training Information [" & mTraining.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Training Information [" & mTraining.Name & "]"
            End If
        End If
        upnlTitle.Update() 'AJAX
    End Sub
    Private Sub addAttributes()
        'Freq In Months
        txtFreqInMonths.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFreqInMonths').value,event)")
        'Warning Days
        txtWarningDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtWarningDays').value,event)")
    End Sub
    'AJAX
    Private Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim str As String = ""
        If Trim(txtName.Text) = "" Then str = "Training Name Required."

        If str <> "" Then
            cvTrainingType.ErrorMessage = str
            cvTrainingType.IsValid = False
            'Return False
        End If
        'Return True
    End Sub
    Public Sub ControlVisibility(ByVal index As Integer)
        txtFor.Visible = IIf(index > 0, True, False)
        lblFor.Visible = IIf(index > 0, True, False)
    End Sub
    Public Sub ControlVisibility1()
        If mTraining.IsNew Then
            lnkTrainingDesignation.Visible = False
            lnkTrainingOrgDetail.Visible = False
            lnkTrainingModel.Visible = False
        Else
            lnkTrainingDesignation.Visible = True
            lnkTrainingOrgDetail.Visible = True
            lnkTrainingModel.Visible = True
        End If
        upnlModalPopUpLinks.Update()
    End Sub
    Private Sub Save()
        If (Not User.IsInRole("TrainingNew") And mTraining.IsNew) Or (Not User.IsInRole("TrainingEdit") And Not mTraining.IsNew) Then
            setObject()
            SetSession()
            MarkLog(Flypal.Util.Action.Save, "Training", User.Identity.Name & " is not Authorized User to save " & mTraining.Name, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'AJAX Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        'Added by Vikrant on 22-July-2011
        Page.Validate("1")
        'End
        If IsValid Then
            Try
                setObject()
                mTraining.Save()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                MarkLog(Flypal.Util.Action.Save, "Training", mTraining.Name, Flypal.Util.ErrorType.NoError, mTraining.ID, EventLogID)
                NewRecord()
                ''txtName.DataBind()
                ''cmbTrainingType.DataBind()
                ''cmbModelList.DataBind()
                ''chkRecurringStatus.DataBind()
                ''txtFreqInMonths.DataBind()
                ''txtWarningDays.DataBind()
                DataFieldBind()
                ControlVisibility1()
                SetSession()
                SetTitle()
                'AJAX
                upnlTrainingDetails.Update()
                upnlGridView.Update()
                'END
                'lbltitle.Text = "Training Information [New]"
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    'AJAX Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2627 Then
                    'AJAX Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 2601 Then
                    'AJAX Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                ElseIf ex.Number = 547 Then
                    'AJAX Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "Delete")
                End If
            End Try
        Else 'AJAX
            upnlValidationSummary.Update()
        End If
    End Sub
#Region "Training Type"
    Private Sub NewRecordTrainingType()
        mTrainingType = TrainingType.NewTrainingType
        Session("mTrainingType") = mTrainingType
    End Sub
    Private Sub setObjectForTrainingType()
        mTrainingType.Name = txtTrainingTypeName.Text
    End Sub
    Private Sub EditRecordForTrainingType(ByVal mId As Guid)
        mTrainingType = TrainingType.GetTrainingType(mId)
        txtTrainingTypeName.Text = mTrainingType.Name
        Session("mTrainingType") = mTrainingType
    End Sub
    Private Sub DataFieldBindForTrainingType()
        mTrainingTypeList = TrainingTypeList.GetTrainingTypeList()
        dgTrainingType.DataSource = mTrainingTypeList
        Session("mTrainingTypeList") = mTrainingTypeList
        dgTrainingType.DataBind()
    End Sub
    Private Sub SetSessionForTrainingType()
        Session("mTrainingType") = mTrainingType
        Session("mTrainingTypeList") = mTrainingTypeList
    End Sub
    Private Sub DeleteRecordForTrainingType(ByVal mId As Guid)
        MSGBoxCntrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteTrainingType")
        mTrainingType = TrainingType.GetTrainingType(mId)
        Session("mTrainingType") = mTrainingType
    End Sub
#End Region
    Private Sub DisableName(ByVal mId As Guid) 'Added by : Shital 19-Jun-2020, ALL16062020
        Dim mTransCountAsPerMasters As TransCountAsPerMasters = TransCountAsPerMasters.GetTransCountAsPerTraining(mId)
        If Not mTransCountAsPerMasters Is Nothing Then
            txtName.Enabled = mTransCountAsPerMasters.Count = 0
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTrainingTypeList = TrainingTypeList.GetTrainingTypeList(, "<SELECT>")
        cmbTrainingType.DataSource = mTrainingTypeList
        Session("mTrainingTypeList") = mTrainingTypeList
        'cmbTrainingType.DataBind()

        'mModelList = ModelList.GetModelList(1, "", "{00000000-0000-0000-0000-000000000000}", "{00000000-0000-0000-0000-000000000000}", "<SELECT>")
        'cmbModelList.DataSource = mModelList
        'Session("mModelList") = mModelList
        'cmbModelList.DataBind()

        mTrainingList = TrainingList.GetTrainingList()
        dgTrainingList.DataSource = mTrainingList
        Session("mTrainingList") = mTrainingList

        'dgTrainingList.DataBind()
        DataBind()

        lblResult.Text = "Training List: " & mTrainingList.Count & " Record(s) Found."
    End Sub
    'AJAX
    Private Sub DataFieldBindForTrainingDesg()
        mDesignationList = DesignationList.GetDesignationList()
        dgTrainingDesignationList.DataSource = mDesignationList
        Session("mDesignationList") = mDesignationList

        mTrainingDesignationList = TrainingDesignationList.GetTrainingDesignationList(mTraining.ID)
        Dim Child As TrainingDesignation
        For Each Child In mTrainingDesignationList
            If mDesignationList.Contains(Child.DesignationID) Then
                mDesignationList.Item(Child.DesignationID).IsSelect = True
            End If
        Next

        DataBind()
    End Sub
    'AJAX
    Private Sub DataFieldBindForTrainingModel()
        mModels = Models.GetModelsForTraining(mTraining.ID)
        'dgTrainingModelList.DataSource = mModels
        Session("mModels") = mModels

        mTrainingModelList = TrainingModelList.GetTrainingModelList(mTraining.ID)
        Session("mTrainingModelList") = mTrainingModelList
        Dim Child As TrainingModel
        For Each Child In mTrainingModelList
            If mModels.Contains(Child.ModelID) Then
                mModels.Item(Child.ModelID).IsSelected = True
            End If
        Next
        dgTrainingModelList.DataSource = mModels
        dgTrainingModelList.DataBind()
        txtModelName.Text = mTraining.Name
    End Sub
    'AJAX
    Private Sub DataFieldBindForTrainingOrg()
        mTrainingOrgList = TrainingOrgList.GetTrainingOrgList()

        mTrainingOrgDetailList = TrainingOrgDetailList.GetTrainingOrgDetailList(mTraining.ID)
        Dim Child As TrainingOrgDetail
        For Each Child In mTrainingOrgDetailList
            If mTrainingOrgList.Contains(Child.TrainingOrgID) Then
                mTrainingOrgList.Item(Child.TrainingOrgID).IsSelect = True
            End If
        Next
        dgTrainingOrgDetailList.DataSource = mTrainingOrgList
        Session("mTrainingOrgList") = mTrainingOrgList
        DataBind()
    End Sub
    'AJAX
    Private Sub ShowTrainingDesignations()
        lnkTrainingDesignation_ModalPopupExtender.Show()

        mDesignationList = DesignationList.GetDesignationList()
        dgTrainingDesignationList.DataSource = mDesignationList
        Session("mDesignationList") = mDesignationList

        mTrainingDesignationList = TrainingDesignationList.GetTrainingDesignationList(mTraining.ID)
        Session("mTrainingDesignationList") = mTrainingDesignationList
        Dim Child As TrainingDesignation
        For Each Child In mTrainingDesignationList
            If mDesignationList.Contains(Child.DesignationID) Then
                mDesignationList.Item(Child.DesignationID).IsSelect = True
            End If
        Next
        txtTrainingName.Text = mTraining.Name
        DataBind()
    End Sub
    'AJAX
    Private Sub ShowTrainingOrganisations()
        lnkTrainingOrgDetail_ModalPopupExtender.Show()
        mTrainingOrgList = TrainingOrgList.GetTrainingOrgList()

        mTrainingOrgDetailList = TrainingOrgDetailList.GetTrainingOrgDetailList(mTraining.ID)
        Dim Child As TrainingOrgDetail
        For Each Child In mTrainingOrgDetailList
            If mTrainingOrgList.Contains(Child.TrainingOrgID) Then
                mTrainingOrgList.Item(Child.TrainingOrgID).IsSelect = True
            End If
        Next
        dgTrainingOrgDetailList.DataSource = mTrainingOrgList
        Session("mTrainingOrgList") = mTrainingOrgList
        Session("mTrainingOrgDetailList") = mTrainingOrgDetailList
        txtOrganisationName.Text = mTraining.Name
        DataBind()
    End Sub
    'AJAX
    Private Sub ShowTrainingModels()
        lnkTrainingModel_ModalPopupExtender.Show()
        DataFieldBindForTrainingModel()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        addAttributes()
        'Added by Vikrant on 20-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtName.Enabled = True Then
                setFocus(txtName)
            End If
            If Session("MiddleFrame") <> "wfTraining_Ajax.aspx" Then
                Session("MiddleFrame") = "wfTraining_Ajax.aspx"
            End If

            If Session("NewTraining") <> "True" Then
                NewRecord()
            Else
                Session("NewTraining") = "True"
            End If
            Session("mTraining") = mTraining
            DataFieldBind()
        Else
            dgTrainingList.DataSource = mTrainingList
            dgTrainingList.DataBind()
            lblResult.Text = "Training List: " & mTrainingList.Count & " Record(s) Found."
        End If

        'If mTrainingList.Count > 25 Then
        '    btnBackTop.Visible = True
        'Else
        '    btnBackTop.Visible = False
        'End If
        SetTitle()
        ControlVisibility1()
        'AJAX MessageBoxResult()
        SetSession()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Save()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If

        NewRecord()
        MarkLog(Flypal.Util.Action.[New], "Training", "", Flypal.Util.ErrorType.NoError, mTraining.ID, EventLogID)
        ControlVisibility1()

        ' ''txtName.Text = ""
        ' ''cmbTrainingType.SelectedIndex = 0
        ' ''cmbModelList.SelectedIndex = 0
        ' ''chkRecurringStatus.Checked = False
        ' ''txtFreqInMonths.Text = ""
        ' ''txtWarningDays.Text = ""
        DataFieldBind()
        lbltitle.Text = "Training Information [New]"
        'AJAX
        upnlTitle.Update()
        upnlValidationSummary.Update()
        'upnlTrainingDetails.Update()
        upnlSearchCriteria.Update()
        'END
    End Sub
    Private Sub btnTrainingTypeList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrainingTypeList.Click
        setObject() 'Added Code
        Session("NewTraining") = "True"
        NewRecordTrainingType()
        TrainingType_ModalPopupExtender.Show()
        DataFieldBindForTrainingType()
        txtTrainingTypeName.Text = ""
        dgTrainingType.PageIndex = 0
        upnlTrainingType.Update()
        'Response.Redirect("wfTrainingType.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=wfTraining_Ajax.aspx" & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    End Sub
    Private Sub btnBackTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Flypal.Util.Action.Close, "Training", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        setObject()
        If mTraining.IsDirty Then
            MSGBoxCntrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            Session("MiddleFrame") = ""
            Session.Remove("NewTraining")
            'Response.Redirect("Dashboard.aspx")

            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If

            If Request.QueryString("ChildPage2") = "wfEmployeeTraining_Ajax.aspx" And IsFromRenewal = "True" Then
                Session("MiddleFrame") = "wfEmployeeDueForRenewal_Ajax.aspx"
                IsFromRenewal = "False"
                Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))

            ElseIf Request.QueryString("ChildPage2") = "wfEmployeeTraining_Ajax.aspx" Then
                Response.Redirect(Request.QueryString("ChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
            Else
                Response.Redirect("Index.aspx")
            End If
        End If
    End Sub

    Private Sub dgTrainingList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTrainingList.PageIndexChanging
        dgTrainingList.PageIndex = e.NewPageIndex
        dgTrainingList.DataSource = mTrainingList
        Session("mTrainingList") = mTrainingList
        dgTrainingList.DataBind()
    End Sub
    Private Sub dgTrainingList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingList.RowCommand
        Dim Idx As Int32
        Dim mID As New Guid
        Dim mName As String
        Select Case e.CommandName
            Case "EditRec"
                If (Not User.IsInRole("TrainingView") And Not User.IsInRole("TrainingEdit")) Then
                    setObject()
                    SetSession()
                    MarkLog(Flypal.Util.Action.Edit, "Training", User.Identity.Name & " is not Authorized User to edit " & mName, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'AJAX Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    'msg.ReplacePage = "wfTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = mTrainingList(Idx).ID
                mName = mTrainingList(Idx).Name
                EditRecord(mID)
                ''txtName.DataBind()
                DataFieldBind()
                ControlVisibility1()
                cmbTrainingType.SelectedValue = mTraining.TrainingTypeID.ToString
                'cmbModelList.SelectedValue = mTraining.ModelID.ToString
                'cmbTrainingType.DataBind()
                'cmbModelList.DataBind()
                DisableName(mID) 'Added by : Shital 19-Jun-2020, ALL16062020
                MarkLog(Flypal.Util.Action.Edit, "Training", mTraining.Name, Flypal.Util.ErrorType.NoError, mTraining.ID, EventLogID)
                If Len(mTraining.Name) > 15 Then
                    lbltitle.Text = "Training Information [" & mTraining.Name.Substring(0, 15) & "... ]"
                Else
                    lbltitle.Text = "Training Information [" & mTraining.Name & " ]"
                End If
                'AJAX
                upnlTitle.Update()
                upnlTrainingDetails.Update()
                'End
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
            Case "DeleteRec"
                Idx = CInt(e.CommandArgument) + dgTrainingList.PageIndex * dgTrainingList.PageSize
                mID = mTrainingList(Idx).ID
                mName = mTrainingList(Idx).Name
                If (Not User.IsInRole("TrainingDelete")) Then
                    setObject()
                    SetSession()
                    MarkLog(Flypal.Util.Action.Delete, "Training", User.Identity.Name & " is not Authorized User to delete " & mName, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'AJAX Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                    'msg.ReplacePage = "wfTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If

                DeleteRecord(mID)
        End Select
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If cmbSearchType.SelectedValue = 0 Then
            mTrainingList = TrainingList.GetTrainingList(, , , )
        ElseIf cmbSearchType.SelectedValue = 1 Then
            mTrainingList = TrainingList.GetTrainingList(, Trim(txtFor.Text), , )
        ElseIf cmbSearchType.SelectedValue = 2 Then
            mTrainingList = TrainingList.GetTrainingList(, , txtFor.Text, )
        End If
        dgTrainingList.DataSource = mTrainingList
        dgTrainingList.DataBind()
        Session("mTrainingList") = mTrainingList

        lblResult.Text = "Training List: " & mTrainingList.Count & " Record(s) Found."
        'AJAX
        upnlSearchCriteria.Update()
        upnlValidationSummary.Update()
        upnlGridView.Update()
        'End
    End Sub
    Private Sub lnkTrainingDesignation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkTrainingDesignation.Click
        Session("mTraining") = mTraining
        Session("Training") = "wfTraining_Ajax.aspx"
        Session("NewTraining") = "True"
        'Dim str As String

        'AJAX If Request.QueryString("ChildPage2") = "wfEmployeeTraining.aspx" Then
        '    str = "<script language='javascript'>openledgersame('wfTrainingDesignation.aspx?Childpage3=wfTraining.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal") & "');</script>"
        'Else
        '    str = "<script language='javascript'>openledgersame('wfTrainingDesignation.aspx?Childpage3=index.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal") & "');</script>"
        'End If
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        ShowTrainingDesignations() 'AJAX
    End Sub
    Private Sub lnkTrainingOrgDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkTrainingOrgDetail.Click
        Session("mTraining") = mTraining
        Session("Training") = "wfTraining_Ajax.aspx"
        Session("NewTraining") = "True"
        'Dim str As String
        'AJAX If Request.QueryString("ChildPage2") = "wfEmployeeTraining.aspx" Then
        '    str = "<script language='javascript'>openledgersame('wfTrainingOrgDetail.aspx?Childpage3=wfTraining.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal") & "');</script>"
        'Else
        '    str = "<script language='javascript'>openledgersame('wfTrainingOrgDetail.aspx?Childpage3=index.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal") & "');</script>"
        'End If
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        ShowTrainingOrganisations() 'AJAX
    End Sub
    Private Sub lnkTrainingModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkTrainingModel.Click
        Session("mTraining") = mTraining
        Session("Training") = "wfTraining_Ajax.aspx"
        Session("NewTraining") = "True"
        'Dim str As String
        'AJAX If Request.QueryString("ChildPage2") = "wfEmployeeTraining.aspx" Then
        '    str = "<script language='javascript'>openledgersame('wfTrainingModel.aspx?Childpage3=wfTraining.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal") & "');</script>"
        'Else
        '    str = "<script language='javascript'>openledgersame('wfTrainingModel.aspx?Childpage3=index.aspx&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal") & "');</script>"
        'End If
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        ShowTrainingModels() 'AJAX
    End Sub
    Private Sub cmbSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchType.SelectedIndexChanged
        Dim index As Integer
        txtFor.Text = ""
        index = cmbSearchType.SelectedIndex
        ControlVisibility(index)
    End Sub

    'Added By Prashant 23-June-2009 for grid sorting 
    Private Sub dgTrainingList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgTrainingList.Sorting
        mTrainingList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mTrainingList") = mTrainingList
        dgTrainingList.DataSource = mTrainingList
        dgTrainingList.DataBind()
    End Sub
    '----------------------------------------------
    'AJAX- New Event for MessageBox Control 
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCntrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub chkRecurringStatus_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRecurringStatus.CheckedChanged
        If chkRecurringStatus.Checked = True Then
            txtFreqInMonths.Enabled = True
        Else
            txtFreqInMonths.Enabled = False
        End If
    End Sub
#End Region

#Region "Training Designation"
    'AJAX
    Private Sub btnSaveTrainingDesg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrainingDesg.Click
        If (Not User.IsInRole("TrainingNew") And mTraining.IsNew) Or (Not User.IsInRole("TrainingEdit") And Not mTraining.IsNew) Then
            setObjectForTrainingDesg()
            SetSessionForTrainingDesg()
            'MarkLog(Flypal.Util.Action.Save, "Training", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
            'AJAX Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfTraning.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Dim chkValue As New CheckBox
        Dim mIsNotSelect As Boolean = True


        If IsValid Then
            Try
                setObjectForTrainingDesg()
                DataFieldBindForTrainingDesg()
                SetSessionForTrainingDesg()
                lblTitleTrainingDesg.Text = "Training Designation Information"
                'lnkTrainingDesignation_ModalPopupExtender.Show()
                lnkTrainingDesignation_ModalPopupExtender.Hide()
                'Response.Redirect("wfTraining.aspx")
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    'AJAX Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraning.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'AJAX Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraning.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'AJAX Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraning.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    'AJAX
    Private Sub btnCloseTrainingDesg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseTrainingDesg.Click
        Session.Remove("mTrainingDesignation")
        Session.Remove("mTrainingDesignationList")
        Session.Remove("mDesignationList")
    End Sub
#End Region

#Region "Training Organization"
    'AJAX
    Private Sub btnSaveTrainingOrg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrainingOrg.Click
        If (Not User.IsInRole("TrainingNew") And mTraining.IsNew) Or (Not User.IsInRole("TrainingEdit") And Not mTraining.IsNew) Then
            setObjectForTrainingOrg()
            SetSessionForTrainingOrg()
            MarkLog(Flypal.Util.Action.Save, "Training", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfTraningOrgDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Dim chkValue As New CheckBox
        Dim mIsNotSelect As Boolean = True

        If IsValid Then
            Try
                setObjectForTrainingOrg()
                DataFieldBindForTrainingOrg()
                SetSessionForTrainingOrg()
                lbltitle.Text = "Training Organization Detail"
                'lnkTrainingOrgDetail_ModalPopupExtender.Show()
                lnkTrainingOrgDetail_ModalPopupExtender.Hide()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraningOrgDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraningOrgDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraningOrgDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    'AJAX
    Private Sub btnCloseTraningOrg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseTraningOrg.Click
        Session.Remove("mTrainingOrgDetail")
        Session.Remove("mTrainingOrgDetailList")
        Session.Remove("mTrainingOrgList")
    End Sub
#End Region

#Region "Training Model"
    'AJAX
    Private Sub btnSaveTrainingModel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTrainingModel.Click
        If (Not User.IsInRole("TrainingNew") And mTraining.IsNew) Or (Not User.IsInRole("TrainingEdit") And Not mTraining.IsNew) Then
            setObjectForTrainingModel()
            SetSessionForTrainingModel()
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
            'msg.ReplacePage = "wfTraning.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Dim chkValue As New CheckBox
        Dim mIsNotSelect As Boolean = True


        If IsValid Then
            Try
                setObjectForTrainingModel()
                DataFieldBindForTrainingModel()
                SetSessionForTrainingModel()
                lbltitle.Text = "Training Model Information"
                'lnkTrainingModel_ModalPopupExtender.Show()
                lnkTrainingModel_ModalPopupExtender.Hide()
                upnlModalPopUpLinks.Update()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraning.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraning.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
                    'msg1.ReplacePage = "wfTraning.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&Childpage3=" & Request.QueryString("Childpage3") & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    'AJAX
    Private Sub btnCloseTrainingModel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseTrainingModel.Click
        Session.Remove("mTrainingModel")
        Session.Remove("mTrainingModelList")
        Session.Remove("mModels")
    End Sub
#End Region

#Region "Training Type"
    Private Sub btnCloseTrainingType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTrainingType.Click
        Session.Remove("mTrainingType")
        'Session.Remove("mTrainingTypeList")
        DataFieldBind()
        TrainingType_ModalPopupExtender.Hide()
        upnlTrainingDetails.Update()
    End Sub
    Private Sub btnSaveTrainingType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveTrainingType.Click
        If (Not User.IsInRole("TrainingNew") And mTrainingType.IsNew) Or (Not User.IsInRole("TrainingEdit") And Not mTrainingType.IsNew) Then
            setObjectForTrainingType()
            SetSessionForTrainingType()
            MarkLog(Util.Action.Save, "TrainingType", User.Identity.Name & " is not Authorized User to save " & mTrainingType.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID) 'changes by Vikrant on 20-July-2011
            MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        Try
            setObjectForTrainingType()
            mTrainingType.Save()
            If txtTrainingTypeName.Enabled = True Then
                setFocus(txtTrainingTypeName)
            End If
            MarkLog(Util.Action.Save, "TrainingType", mTrainingType.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID) 'changes by Vikrant on 20-July-2011
            mTrainingType = TrainingType.NewTrainingType
            txtTrainingTypeName.Text = ""
            DataFieldBindForTrainingType()
            SetSessionForTrainingType()
            upnlTrainingType.Update()
            lblTitleTrainingType.Text = "Training Type Information [New]"
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Then
                MSGBoxCntrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCntrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub
    Private Sub dgTrainingType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTrainingType.PageIndexChanging
        dgTrainingType.PageIndex = e.NewPageIndex
        dgTrainingType.DataSource = mTrainingTypeList
        Session("mTrainingTypeList") = mTrainingTypeList
        dgTrainingType.DataBind()
    End Sub
    Private Sub dgTrainingType_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTrainingType.RowCommand
        Dim Index As Integer
        Dim mID As Guid
        Dim mName As String
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgTrainingType.PageSize * dgTrainingType.PageIndex
                mID = mTrainingTypeList(Index).ID
                mName = mTrainingTypeList(Index).Name
                If (Not User.IsInRole("TrainingView") And Not User.IsInRole("TrainingEdit")) Then
                    setObjectForTrainingType()
                    SetSessionForTrainingType()
                    'changes by Vikrant on 20-July-2011
                    MarkLog(Util.Action.Edit, "TrainingType", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecordForTrainingType(mID)
                txtTrainingTypeName.DataBind()
                'changes by Vikrant on 20-July-2011
                MarkLog(Util.Action.Edit, "TrainingType", mTrainingType.Name, Util.ErrorType.NoError, mTrainingType.ID, EventLogID)
                If Len(mTrainingType.Name) > 15 Then
                    lblTitleTrainingType.Text = "Training Type Information [" & mTrainingType.Name.Substring(0, 15) & "...]"
                Else
                    lblTitleTrainingType.Text = "Training Type Information [" & mTrainingType.Name & "]"
                End If
                If txtTrainingTypeName.Enabled = True Then
                    setFocus(txtTrainingTypeName)
                End If
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgTrainingType.PageSize * dgTrainingType.PageIndex
                mID = mTrainingTypeList(Index).ID
                mName = mTrainingTypeList(Index).Name

                If (Not User.IsInRole("TrainingDelete")) Then
                    setObjectForTrainingType()
                    SetSessionForTrainingType()
                    MarkLog(Flypal.Util.Action.Delete, "TrainingType", User.Identity.Name & " is not Authorized User to delete " & mName, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCntrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecordForTrainingType(mID)
        End Select
    End Sub
    Private Sub btnNewTrainingType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewTrainingType.Click
        If txtTrainingTypeName.Enabled = True Then
            setFocus(txtTrainingTypeName)
        End If
        NewRecordTrainingType()
        MarkLog(Util.Action.[New], "TrainingType", "", Util.ErrorType.NoError, mTrainingType.ID, EventLogID)
        txtTrainingTypeName.Text = ""
        lblTitleTrainingType.Text = "Training Type Information [New]"
    End Sub
#End Region





End Class