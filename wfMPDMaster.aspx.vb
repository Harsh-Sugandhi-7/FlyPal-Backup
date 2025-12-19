'=============================================
' Created By : Sachin
' Create date: 3-Jun-2024
'=============================================  

Public Class wfMPDMaster
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mMPDMasterList As MPDMasterList
    Public mMPDMaster As MPDMaster
    Public mATAList As ATAList
    Public mServiceTypeList As ServiceTypeList
    Public mPrimaryModelList As PrimaryModelList
    Dim mMPDTypeList As MPDTypeList
    Dim mMPDSkillList As MPDSkillList
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Public mMPDConfigurableList As MPDConfigurableList

    Public mAssemblyMonitorServiceStatusThreshold As AssemblyMonitorServiceStatus
    Public mModelMonitorServiceThreshold As ModelMonitorService

    Public mAssemblyMonitorServiceStatusInterval As AssemblyMonitorServiceStatus
    Public mModelMonitorServiceInterval As ModelMonitorService
    Dim IDForEventLog As Guid
    Public mAircraft As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mMonitorDesc As String
    Public mModelMonitorService As ModelMonitorService
    Public mAssemblyMonitorDetail As String
    Public mBoardInfo As AircraftInformationBoard.BoardInfo
    Public mMachineMaintenance As MachineMaintenance
    Dim mLinkMaintenanceList As LinkMaintenanceList
#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mMPDMaster = CType(Session("mMPDMaster"), MPDMaster)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mMPDConfigurableList = Session("mMPDConfigurableList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMPDConfigurableList")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mMPDMaster")
    End Sub
    Private Sub SetObject()
        mMPDMaster.MPDTaskNumber = Trim(txtMPDTaskNo.Text)
        mMPDMaster.Description = Trim(txtDescription.Text)
        mMPDMaster.ServiceTypeID = Trim(cmbType.SelectedValue)
        mMPDMaster.TaskIntervalDescription = Trim(txtTaskTimings.Text)
        mMPDMaster.ATAID = New Guid(cmbATA.SelectedValue.ToString)
        mMPDMaster.PrimaryModelID = New Guid(cmbPrimaryModel.SelectedValue.ToString)
        mMPDMaster.Applicability = Trim(txtApplicability.Text)
        mMPDMaster.MPDTypeID = Val(cmbMRBCategories.SelectedValue)
        mMPDMaster.MPDSkillID = Val(cmbSkill.SelectedValue)
        mMPDMaster.Zone = Trim(txtZone.Text)
        mMPDMaster.Access = Trim(txtAccess.Text)
        mMPDMaster.Note = Trim(txtNote.Text)


        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mMPDMaster.IsAttachmentAdded = True
            Else
                mMPDMaster.IsAttachmentAdded = False
            End If
        End If
        Session("mMPDMaster") = mMPDMaster


    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mMPDMaster.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mMPDMaster.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mMPDMaster.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Function Save() As Boolean
        If Not IsValid Then Exit Function

        'Authountation
        ''Dim mCheck As New Authenticate.CheckAuthentication(True)
        ''Dim mMachineList As MachineList = MachineList.GetMachineList()
        ''If mMachine.IsNew = True And mMachineList.Count >= mCheck.Number("Aircraft") And mCheck.Number("Aircraft") <> -1 Then
        ''    Dim msg1 As New SIMsgBox(Page, "Authentication", "This version does not supports more than " & mCheck.Number("Aircraft").ToString & " Aircrafts", "", MsgBoxStyle.OKOnly)
        ''    msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
        ''    msg1.Show()
        ''    Return False
        ''End If
        Dim clnMPDMaster As MPDMaster
        clnMPDMaster = CType(mMPDMaster, MPDMaster)
        SetObject()
        'SetGridObject()


        If mMPDMaster.IsValid = True Then
            Try
                'mMPDMaster.ApplyEdit()
                'mMPDMaster = CType(mMPDMaster.Save(), MPDMaster)
                mMPDMaster.Save()
                SaveAttachment() 'D&BChart
                Session("mMPDMaster") = mMPDMaster
                'Added By Utkarsh ON 21-Aug-2013 FOR ALL20082013-1
                'If Not Session("ShowUseMachineList") Is Nothing AndAlso CBool(Session("ShowUseMachineList")) Then
                '    Dim mUserMachineList As UserMachineList = New UserMachineList
                '    If mUserMachineList.ShowUsermachineList() Then
                '        Session("MachineID") = mMachine.ID
                '        Session("MachineName") = mMachine.RegNo
                '        Session("MachineURL") = Request.Url
                '        Session.Remove("ShowUseMachineList")
                '        Response.Redirect("wfUserMachineList.aspx")
                '    End If
                'End If
                'End
                DataFieldBind()
                'Commented By Utkarsh On 2-Aug-2011 For All19072011
                'MarkLog(Util.Action.Save, "Aircraft", mMachine.RegNo, Util.ErrorType.NoError, mMachine.ID, EventLogID)
                'End
                Return True
            Catch ex As SqlException
                Session("clnMPDMaster") = clnMPDMaster
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " ", MsgBoxStyle.OkOnly, "")
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            Catch ex1 As Exception
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Save, SIMsgBox.Message_text.Save, "Invalid , cannot save", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfMachine.aspx?BackPage=" & Request.QueryString("BackPage")
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "Invalid , cannot save", MsgBoxStyle.OkOnly, "")
                Return False
            Finally
                'clnMachine = Nothing
                ''Added By Utkarsh On 2-Aug-2011 For All19072011
                'MarkLog(Util.Action.Save, "Aircraft", mMachine.RegNo, Util.ErrorType.NoError, mMachine.ID, EventLogID)
                ''End
            End Try
        Else
            'If Not CustomValidate2() Then upnlValidationSummary.Update()
            'Return False
        End If
    End Function
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbATA" Then
            If cmbATA.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select ATA from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbType" Then
            If cmbType.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select Task Type from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbPrimaryModel" Then
            If cmbPrimaryModel.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select Primary Model from the list."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtTaskTimings" Then
            If Len(txtTaskTimings.Text) > 1000 Then
                custValidator.ErrorMessage = "Task Description can't be more than 1000 chars."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'ElseIf custValidator.ControlToValidate = "txtDescription" Then
            '    If Len(txtDescription.Text) > 1000 Then
            '        custValidator.ErrorMessage = "Description can't be more than 1000 chars."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
            'ElseIf custValidator.ControlToValidate = "txtReference" Then
            '    If Len(txtReference.Text) > 500 Then
            '        custValidator.ErrorMessage = "Reference Too Long"
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                custValidator.ErrorMessage = "Note can't be more than 1000 chars."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        End If

    End Sub
    Private Sub SetGrid()
        Dim B, C, D, IsReadOnly As Boolean
        For j As Integer = 0 To dgMonitorList.Rows.Count - 1
            B = CType(Me.dgMonitorList.Rows(j).Cells(21).Text, Boolean) 'IsConfigurable
            C = CType(Me.dgMonitorList.Rows(j).Cells(23).Text, Boolean) 'IsMaster
            D = CType(Me.dgMonitorList.Rows(j).Cells(24).Text, Boolean) 'IsAttachmentAdded

            IsReadOnly = CType(Me.dgMonitorList.Rows.Item(j).Cells(25).Text, Boolean) 'IsMachineReadOnly
            dgMonitorList.Rows(j).Cells(20).Enabled = IIf(IsReadOnly Or B = False, False, True) 'Configure
            ''''dgMonitorList.Rows(j).Cells(21).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Delete
            ''''dgMonitorList.Rows(j).Cells(20).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Edit

            '''''''If C = True Then
            '''''''    dgMonitorList.Rows(j).Cells(22).Enabled = False 'History
            '''''''End If
            '''''''If D = False Then
            '''''''    dgMonitorList.Rows(j).Cells(24).Enabled = False 'View
            '''''''End If

            If IsReadOnly Then
                Me.dgMonitorList.Rows.Item(j).BackColor = Color.OrangeRed
                Me.dgMonitorList.Rows.Item(j).ToolTip = "ReadOnly Aircraft"
                Me.dgMonitorList.Rows.Item(j).ForeColor = Color.White
            End If
        Next
    End Sub
    Private Sub EditRecord(ByVal Index As Int32)

        Dim mAssemblyStatus As AssemblyStatus
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus

        Dim mdummyAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        Dim mdummyModelMonitorService As ModelMonitorService
        Dim mMachine As Machine
        Dim AirframeCurrentValues As String = ""
        mMachine = Machine.GetMachine(mMPDConfigurableList(Index).MachineID)
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mMPDConfigurableList.Item(Index).AssemblyMonitorServiceStatusID, mMPDConfigurableList.Item(Index).AssemblyStatusID, mMachine.HourType)
        mModelMonitorService = ModelMonitorService.GetModelMonitorService(mAssemblyMonitorServiceStatus.ModelMonitorServiceID)

        mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMPDConfigurableList(Index).AssemblyStatusID)

        Dim mAircrafyCurrValue As AircraftCurrentStatusList = AircraftCurrentStatusList.GetAircraftDailyStatusMachineList(, mMachine.RegNo.ToString, , , , mAssemblyMonitorServiceStatus.AsOnDateFormatted.ToString)
        AirframeCurrentValues = mAircrafyCurrValue(0).ShowPeriods
        Session("AirframeCurrentValues") = AirframeCurrentValues

        Dim mID As Guid = Guid.NewGuid
        ' mdummyModelMonitorService = ModelMonitorService.NewModelMonitorService(mID, mMPDConfigurableList.Item(Index).ModelID, mMachine.HourType, mID)
        ' mdummyAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mMPDConfigurableList.Item(Index).AssemblyID, mMPDConfigurableList.Item(Index).AssemblyStatusID, Today.Date.ToString, mMPDConfigurableList.Item(Index).ModelID, mMachine.HourType)

        Dim dummyAssemblyMonitorServiceStatusID As Guid = Guid.Empty
        dummyAssemblyMonitorServiceStatusID = mMPDConfigurableList.Item(mMPDConfigurableList.Item(Index).AMPTaskNo, IIf(mMPDConfigurableList.Item(Index).MonitorTypeID = 1, 2, 1)).AssemblyMonitorServiceStatusID

        If Not dummyAssemblyMonitorServiceStatusID.Equals(Guid.Empty) Then
            mdummyAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(dummyAssemblyMonitorServiceStatusID, mMPDConfigurableList.Item(Index).AssemblyStatusID, mMachine.HourType)
            mdummyModelMonitorService = ModelMonitorService.GetModelMonitorService(mdummyAssemblyMonitorServiceStatus.ModelMonitorServiceID)
        Else
            mdummyModelMonitorService = ModelMonitorService.NewModelMonitorService(mID, mMPDConfigurableList.Item(Index).ModelID, mMachine.HourType, mID)
            mdummyAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mMPDConfigurableList.Item(Index).AssemblyID, mMPDConfigurableList.Item(Index).AssemblyStatusID, Today.Date.ToString, mMPDConfigurableList.Item(Index).ModelID, mMachine.HourType)
        End If



        If mMPDConfigurableList.Item(Index).MonitorTypeID = 1 Then
            Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatus
            Session("mModelMonitorServiceThreshold") = mModelMonitorService
            Session("MonitorTypeID") = mModelMonitorService.MonitorTypeID.ToString
            'dummy interval detail, just for binding
            Session("mAssemblyMonitorServiceStatusInterval") = mdummyAssemblyMonitorServiceStatus
            Session("mModelMonitorServiceInterval") = mdummyModelMonitorService

            If mLinkMaintenanceList Is Nothing Then
                mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorService.ID.ToString)
                Session("mLinkMaintenanceList") = mLinkMaintenanceList
            End If

        ElseIf mMPDConfigurableList.Item(Index).MonitorTypeID = 2 Then
            Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatus
            Session("mModelMonitorServiceInterval") = mModelMonitorService
            Session("MonitorTypeID") = mModelMonitorService.MonitorTypeID.ToString
            'dummy threshold detail, just for binding
            Session("mAssemblyMonitorServiceStatusThreshold") = mdummyAssemblyMonitorServiceStatus
            Session("mModelMonitorServiceThreshold") = mdummyModelMonitorService
        Else
            Session("mAssemblyMonitorServiceStatusNA") = mAssemblyMonitorServiceStatus
            Session("mModelMonitorServiceNA") = mModelMonitorService
            Session("MonitorTypeID") = mModelMonitorService.MonitorTypeID.ToString
            mID = Guid.NewGuid
            Session("mModelMonitorServiceInterval") = ModelMonitorService.NewModelMonitorService(mID, mMPDConfigurableList.Item(Index).ModelID, mMachine.HourType, mID)
            Session("mAssemblyMonitorServiceStatusInterval") = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mMPDConfigurableList.Item(Index).AssemblyID, mMPDConfigurableList.Item(Index).AssemblyStatusID, Today.Date.ToString, mMPDConfigurableList.Item(Index).ModelID, mMachine.HourType)


            mID = Guid.NewGuid
            Session("mModelMonitorServiceThreshold") = ModelMonitorService.NewModelMonitorService(mID, mMPDConfigurableList.Item(Index).ModelID, mMachine.HourType, mID)
            Session("mAssemblyMonitorServiceStatusThreshold") = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mMPDConfigurableList.Item(Index).AssemblyID, mMPDConfigurableList.Item(Index).AssemblyStatusID, Today.Date.ToString, mMPDConfigurableList.Item(Index).ModelID, mMachine.HourType)


        End If

        ' Session("mPrevAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus

        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus


        If mAssemblyMonitorServiceStatus.IsAttachmentAdded Then
            Dim mFileAttach As FileAttach = FileAttach.GetAttachment(mAssemblyMonitorServiceStatus.ID) 'Sort = 1 - Installation
            Session("mFileAttach") = mFileAttach
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorServiceStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If

        Session("mAssemblyInfo") = mMPDConfigurableList.Item(Index).RegNo + "->" + mMPDConfigurableList.Item(Index).ModelSerialNo + "->" + mMPDConfigurableList.Item(Index).Reference + "->" + mMPDConfigurableList.Item(Index).TypeDet + "->" + mMPDConfigurableList.Item(Index).ATA.ToString + "->" + mMPDConfigurableList.Item(Index).Description
        Session("RegNo") = mMPDConfigurableList.Item(Index).RegNo.ToString
        ''' RemoveSession()
        Session("FromEditThresholdInterval") = "True" 'Edit record
        Response.Redirect("wfAMPDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfMPDMaster.aspx")
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        mMPDConfigurableList.CurrentIndex = Index
        Session("Index") = Index
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
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
                            Dim index As Integer = CType(Session("Index"), Integer)
                            IDForEventLog = mMPDConfigurableList(index).AssemblyMonitorServiceStatusID
                            mMonitorInfo = mMPDConfigurableList(index).TypeDet
                            mMonitorType = mMPDConfigurableList(index).MonitorType
                            mMonitorDesc = mMPDConfigurableList(index).Description
                            mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc
                            'End
                            'Added by Saylee on 28-May-2009
                            mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mMPDConfigurableList(index).AssemblyMonitorServiceStatusID)
                            '********************************
                            If mMPDConfigurableList(index).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mMPDConfigurableList(index).AssemblyMonitorServiceStatusID)
                            End If
                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mMPDConfigurableList(index).AssemblyMonitorServiceStatusID, 6)
                            '=============================

                            AssemblyMonitorServiceStatus.DeleteAssemblyMonitorServiceStatus(mMPDConfigurableList(index).AssemblyMonitorServiceStatusID)
                            ModelMonitorService.DeleteModelMonitorService(mMPDConfigurableList(index).ModelMonitorServiceID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If

                            Session("mMachineMaintenance") = mMachineMaintenance
                            'Added by Saylee on 28-May-2009
                            mBoardInfo.IsComplyDelete = True
                            mBoardInfo.ApplyEdit()
                            mBoardInfo.Save()
                            Session("mAircraftInformationBoardList") = Nothing
                            '********************************

                            mMPDConfigurableList = MPDConfigurableList.GetAMPConfigurationList(PrimaryModelID:=mMPDMaster.PrimaryModelID, ServiceType:=mMPDMaster.ServiceTypeID, MPDMasterID:=mMPDMaster.ID.ToString)
                            dgMonitorList.DataSource = mMPDConfigurableList
                            dgMonitorList.DataBind()
                            Session("mMPDConfigurableList") = mMPDConfigurableList
                            SetGrid()
                            upnlAssemblyDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "AssemblyService", "Can't delete :" & mAssemblyMonitorDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "AssemblyService", mAssemblyMonitorDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                            End If
                            mMPDMaster = MPDMaster.GetMPDMaster(mMPDMaster.ID)
                            Session("mMPDMaster") = mMPDMaster
                            cmbType.DataBind()
                            upnlDetails.Update()
                        End Try
                    End If
                Case MsgBoxResult.No

                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        'mEmployeeList = EmployeeList.GetEmployeeList("", "", "(SELECT)")
        'cmbType.DataSource = mEmployeeList
        'Session("mEmployeeList") = mEmployeeList
        'cmbType
        'cmbATA
        'cmbPrimaryModel
        'cmbMRBCategories
        'cmbSkill

        mATAList = ATAList.GetATAList("", "(SELECT)") 'Added By Saylee on 12-Aug-2010
        Session("mATAList") = mATAList
        cmbATA.DataSource = mATAList
        'cmbATA.DataBind()

        mServiceTypeList = ServiceTypeList.GetServiceTypeList(True)
        cmbType.DataSource = mServiceTypeList
        'cmbType.DataBind()
        Session("mServiceTypeList") = mServiceTypeList

        mPrimaryModelList = PrimaryModelList.GetPrimaryModelList(AddTopItem:="(SELECT)")
        cmbPrimaryModel.DataSource = mPrimaryModelList
        'cmbPrimaryModel.DataBind()

        mMPDTypeList = MPDTypeList.GetTypeList(True)
        cmbMRBCategories.DataSource = mMPDTypeList

        mMPDSkillList = MPDSkillList.GetSkillList(True)
        cmbSkill.DataSource = mMPDSkillList
        If Not mMPDMaster.IsNew Then
            mMPDConfigurableList = MPDConfigurableList.GetAMPConfigurationList(PrimaryModelID:=mMPDMaster.PrimaryModelID, ServiceType:=mMPDMaster.ServiceTypeID, MPDMasterID:=mMPDMaster.ID.ToString) 'tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
            dgMonitorList.DataSource = mMPDConfigurableList
            Session("mMPDConfigurableList") = mMPDConfigurableList
            lblResultInspList.Text = "List of Configurations : " + mMPDConfigurableList.Count.ToString + " Record(s)"
            lblTitle.Text = "MPD Master Details for " & "MPD Task No. " + mMPDMaster.MPDTaskNumber
        Else

            lblTitle.Text = "MPD Master Details" & " [New]"
        End If




        DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not IsPostBack Then
            DataFieldBind()
            SetGrid()
            ControlVisibilityForAttachment()
        End If
    End Sub

    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mMPDMaster.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlAttachFile.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mMPDMaster.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mMPDMaster.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mMPDMaster.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mMPDMaster.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mMPDMaster.ID)
            Session("mFileAttach") = mFileAttach
        End If
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mMPDMaster.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mMPDMaster.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mMPDMaster.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Session.Remove("FromEditThresholdInterval")
        RemoveSession()
        Response.Redirect("Index.aspx")
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsValid Then
            'If Not CustomValidate2() Then upnlValidationSummary.Update() : Exit Sub

            SetObject()
            'DataFieldBind()
            If Save() = True Then
                If Not mMPDMaster.IsNew Then
                    mMPDConfigurableList = MPDConfigurableList.GetAMPConfigurationList(PrimaryModelID:=mMPDMaster.PrimaryModelID, ServiceType:=mMPDMaster.ServiceTypeID, MPDMasterID:=mMPDMaster.ID.ToString) 'tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
                    dgMonitorList.DataSource = mMPDConfigurableList
                    Session("mMPDConfigurableList") = mMPDConfigurableList
                    lblResultInspList.Text = "List of Configurations : " + mMPDConfigurableList.Count.ToString + " Record(s)"
                    lblTitle.Text = "MPD Master Details for " & "MPD Task No. " + mMPDMaster.MPDTaskNumber
                Else

                    lblTitle.Text = "MPD Master Details" & " [New]"
                End If
                upnlAssemblyDetails.Update()
                upnlTitle.Update()
                upnlDetails.Update()
                SetGrid()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Else
                upnlValidationSummary.Update()
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub

    Private Sub dgMonitorList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgMonitorList.RowCommand
        Dim AssemblyID As Guid
        Dim AssemblyStatusID As Guid
        Dim AsOnDate As String
        Dim HourType As Integer
        Dim ModelID As Guid
        Dim AirframeCurrentValues As String
        Select Case e.CommandName
            Case "Configure"
                AssemblyID = New Guid(dgMonitorList.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                AssemblyStatusID = New Guid(dgMonitorList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                AsOnDate = dgMonitorList.Rows(CInt(e.CommandArgument)).Cells(2).Text
                HourType = CInt(dgMonitorList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                ModelID = New Guid(dgMonitorList.Rows(CInt(e.CommandArgument)).Cells(26).Text)



                Dim mModelMonitorServiceTypeList As ModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList(ServiceTypeID:=mMPDMaster.ServiceTypeID)

                'Threshold
                Dim mID As Guid = Guid.NewGuid
                mModelMonitorServiceThreshold = ModelMonitorService.NewModelMonitorService(mID, ModelID, HourType, mID)
                mAssemblyMonitorServiceStatusThreshold = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)

                mModelMonitorServiceThreshold.ATAID = mMPDMaster.ATAID
                'mModelMonitorServiceThreshold.Reference = Trim(txtReference.Text)
                mModelMonitorServiceThreshold.Description = mMPDMaster.Description

                If mModelMonitorServiceTypeList.Contains(mMPDMaster.ServiceTypeID, 1) Then  'ONE TIME
                    mModelMonitorServiceThreshold.ModelMonitorServiceTypeID = CType(Val(mModelMonitorServiceTypeList(mMPDMaster.ServiceTypeID, 1).ID), Int32)
                End If

                mModelMonitorServiceThreshold.Note = mMPDMaster.Note
                mModelMonitorServiceThreshold.Zone = Trim(mMPDMaster.Zone)
                ' mModelMonitorServiceThreshold.Area = Trim(mMPDMaster.area)
                mModelMonitorServiceThreshold.Applicability = mMPDMaster.Applicability.Trim
                ' mModelMonitorServiceThreshold.Source = txtSource.Text.Trim
                mModelMonitorServiceThreshold.Access = mMPDMaster.Access.Trim
                mModelMonitorServiceThreshold.MPDSkillID = mMPDMaster.MPDSkillID
                mModelMonitorServiceThreshold.MPDTypeID = mMPDMaster.MPDTypeID
                mModelMonitorServiceThreshold.MPDMasterID = mMPDMaster.ID



                'INTERVAL
                mID = Guid.NewGuid
                mModelMonitorServiceInterval = ModelMonitorService.NewModelMonitorService(mID, ModelID, HourType, mID)
                mAssemblyMonitorServiceStatusInterval = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)


                mModelMonitorServiceInterval.ATAID = mMPDMaster.ATAID
                'mModelMonitorServiceInterval.Reference = Trim(txtReference.Text)
                mModelMonitorServiceInterval.Description = mMPDMaster.Description
                If mModelMonitorServiceTypeList.Contains(mMPDMaster.ServiceTypeID, 2) Then 'Recurring
                    mModelMonitorServiceInterval.ModelMonitorServiceTypeID = CType(Val(mModelMonitorServiceTypeList(mMPDMaster.ServiceTypeID, 2).ID), Int32)
                End If
                mModelMonitorServiceInterval.Note = mMPDMaster.Note

                mModelMonitorServiceInterval.Zone = Trim(mMPDMaster.Zone)
                ' mModelMonitorServiceInterval.Area = Trim(mMPDMaster.area)
                mModelMonitorServiceInterval.Applicability = mMPDMaster.Applicability.Trim
                ' mModelMonitorServiceInterval.Source = txtSource.Text.Trim
                mModelMonitorServiceInterval.Access = mMPDMaster.Access.Trim
                mModelMonitorServiceInterval.MPDSkillID = mMPDMaster.MPDSkillID
                mModelMonitorServiceInterval.MPDTypeID = mMPDMaster.MPDTypeID
                mModelMonitorServiceInterval.MPDMasterID = mMPDMaster.ID


                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Dim mMachine As Machine = Machine.GetMachine(mAssemblyStatus.MachineID)


                Dim mAircrafyCurrValue As AircraftCurrentStatusList = AircraftCurrentStatusList.GetAircraftDailyStatusMachineList(, mMachine.RegNo, , , , Today.Date.ToString)
                AirframeCurrentValues = mAircrafyCurrValue(0).ShowPeriods


                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mMachine") = mMachine
                Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold
                Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold

                Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval
                Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
                Session("mMPDMaster") = mMPDMaster
                Session("AirframeCurrentValues") = AirframeCurrentValues
                Session("FromEditThresholdInterval") = "False"
                Session("MonitorTypeID") = "0"
                Session("RegNo") = dgMonitorList.Rows(CInt(e.CommandArgument)).Cells(6).Text.ToString
                Response.Redirect("wfAMPDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfMPDMaster.aspx")

            Case "EditRec"
                'Dim Index As Integer = CInt(e.CommandArgument) + dgMonitorList.PageSize * dgMonitorList.PageIndex
                'Dim mId As Guid = mMPDConfigurableList(Index).AssemblyMonitorServiceStatusID
                If (Not User.IsInRole("AssemblyServiceMonitorView") And Not User.IsInRole("AssemblyServiceMonitorEdit")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If


                EditRecord(CInt(e.CommandArgument))
            Case "DeleteRec"

                If (Not User.IsInRole("AssemblyServiceMonitorDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecord(CInt(e.CommandArgument))
        End Select
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub dgMonitorList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgMonitorList.PageIndexChanging
        dgMonitorList.PageIndex = e.NewPageIndex
        dgMonitorList.DataSource = mMPDConfigurableList
        dgMonitorList.DataBind()
        SetGrid()
    End Sub
#End Region
End Class