Public Class wfNewMPDService_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
    End Enum
#End Region

#Region " Variable Declaration "
    'for Object
    Public mMachine As Machine
    Public mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mModelMonitorService As ModelMonitorService
    Public Flag As Int16
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mModelMonitorServiceTypeList As ModelMonitorServiceTypeList
    Public mModelMonitorServicePeriodUnitList As ModelMonitorServicePeriodUnitList
    Dim str As String
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mIssueDate As String
    Public mAssemblyMonitorServiceStatusList As tmpAssemblyMonitorServiceStatusList
    Dim EventLogID As Guid 'Added by Vikrant on 28-July-2011
    Public mServiceDetail As String
    Public mModel As String
    Public mMonitorServiceType As String
    Public mMonitorDesc As String
    Public mLinkMaintenanceActionList As LinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance 'End
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim ModelID As Guid
    Dim ModelName As String
    Dim mAssemblyStatusModelWise As AssemblyStatusModelWise
    Private mMPDConfigurableList As MPDConfigurableList
    Private mUpdateComplyHistoryAssemblyMonitorServiceStatusList As UpdateComplyHistoryAssemblyMonitorServiceStatusList
    Public mBoardInfo As AircraftInformationBoard.BoardInfo
    Public mMachineMaintenance As MachineMaintenance
    Public mAssemblyMonitorDetail As String
    Public mAircraft As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Dim IDForEventLog As Guid
    Dim mMPDTypeList As MPDTypeList 'Added by Saylee on 19-Apr-2023
    Dim mMPDSkillList As MPDSkillList 'Added by Saylee on 19-Apr-2023

    Dim mLastMPDRef As LastMPDAMPRef 'Added by Ajay on 20-07-2023
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyMonitorServiceStatus = CType(Session("mAssemblyMonitorServiceStatus"), AssemblyMonitorServiceStatus)
        mModelMonitorService = CType(Session("mModelMonitorService"), ModelMonitorService)
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorServiceTypeList = CType(Session("mModelMonitorServiceTypeList"), ModelMonitorServiceTypeList)
        mModelMonitorServicePeriodUnitList = CType(Session("mModelMonitorServicePeriodUnitList"), ModelMonitorServicePeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mAssemblyMonitorServiceStatusList = CType(Session("mAssemblyMonitorServiceStatusList"), tmpAssemblyMonitorServiceStatusList)
        mLinkMaintenanceActionList = Session("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        mLinkMaintenanceList = Session("mLinkMaintenanceList") 'End
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mAssemblyStatusModelWise = Session("mAssemblyStatusModelWise")
        ModelID = Session("ModelIDForNewMPD")
        mMPDConfigurableList = Session("mMPDConfigurableList")
        mLastMPDRef = Session("mLastMPDRef")
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mModelMonitorService") = mModelMonitorService
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mATAList") = mATAList
        Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList
        Session("mModelMonitorServicePeriodUnitList") = mModelMonitorServicePeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mAssemblyMonitorServiceStatusList") = mAssemblyMonitorServiceStatusList
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End
        Session("mLastMPDRef") = mLastMPDRef
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorServiceTypeList")
        Session.Remove("mModelMonitorServicePeriodUnitList")
        Session.Remove("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session.Remove("mLinkMaintenanceList")
        Session.Remove("URL")
        Session.Remove("MaintenanceActivityID") 'End
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mAssemblyStatusModelWise")
        Session.Remove("mMPDConfigurableList")
        Session.Remove("mLastMPDRef")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "NewMPD"


        'Depending upon decided IsInRole String; checkign Rights of the User
        Select Case CheckFor
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
        End Select
    End Function
    Private Sub setObject()
        'mModelMonitorService.Code = Trim(txtCode.Text)
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If Trim(txtCode.Text).Length < 3 And Trim(txtCode.Text) <> "" Then
                mModelMonitorService.Code = Trim(txtCode.Text).PadLeft(3, "0"c)
            Else
                mModelMonitorService.Code = Trim(txtCode.Text)
            End If
        Else
            mModelMonitorService.Code = Trim(txtCode.Text)
        End If
        mModelMonitorService.Reference = Trim(txtReference.Text)
        mModelMonitorService.Description = Trim(txtDescription.Text)
        mModelMonitorService.Note = Trim(txtNote.Text)
        mModelMonitorService.ATAID = New Guid(cmbATAChapter.SelectedValue)
        mModelMonitorService.ModelMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue), Int32)
        mModelMonitorService.ShowInCofA = chkShowInCofA.Checked
        mModelMonitorService.RequiredManHours = txtRequiredManHours.Text
        mModelMonitorService.Zone = Trim(txtZone.Text) 'Added by Saylee on 23-July-2013 for BA22072013 
        mModelMonitorService.Area = Trim(txtArea.Text)
        mModelMonitorService.IsRII = chkIsRII.Checked 'End
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorService.IsAttachmentAdded = True
            Else
                mModelMonitorService.IsAttachmentAdded = False
            End If
        End If

        'Added by Saylee on 19-Apr-2023
        mModelMonitorService.TaskCardNo = txtTaskCardNo.Text.Trim
        mModelMonitorService.TaskHeading = txtTaskCardHeader.Text.Trim
        mModelMonitorService.Applicability = txtApplicability.Text.Trim
        mModelMonitorService.Source = txtSource.Text.Trim
        mModelMonitorService.Access = txtAccess.Text.Trim
        mModelMonitorService.MPDSkillID = Val(cmbSkillcode.SelectedValue.ToString)
        mModelMonitorService.MPDTypeID = Val(cmbMPDType.SelectedValue.ToString)
        mModelMonitorService.AccessOpenCloseManHours = txtAccessManHours.Text.Trim
        ''********************
        Session("mModelMonitorService") = mModelMonitorService
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mModelMonitorService.ModelMonitorServicePeriods
            For i As Integer = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                'Setting the Object with the Values of the Controls
                If .Item(i).PeriodID = 2 And Decimal.MaxValue < Val(txtFrequencyValue.Text) Then
                    .Item(i).FrequencyValue = ""
                Else
                    .Item(i).FrequencyValue = Trim(txtFrequencyValue.Text)
                End If
            Next i
        End With
        mModelMonitorService = Session("mModelMonitorService")
    End Sub
    Private Sub SetGrid()
        Dim B, C, D, IsReadOnly As Boolean

        For j As Integer = 0 To dgMonitorServiceStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(17).Text, Boolean) 'IsConfigurable
            C = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(18).Text, Boolean) 'IsMaster
            D = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(19).Text, Boolean) 'IsAttachmentAdded
            IsReadOnly = CType(Me.dgMonitorServiceStatusList.Rows.Item(j).Cells(20).Text, Boolean)

            dgMonitorServiceStatusList.Rows(j).Cells(16).Enabled = IIf(IsReadOnly Or B = False, False, True) 'Configure
            '''''dgMonitorServiceStatusList.Rows(j).Cells(19).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Delete
            '''''dgMonitorServiceStatusList.Rows(j).Cells(18).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Edit

            If B = False Then
                'dgMonitorServiceStatusList.Rows(j).Cells(16).Enabled = False 'Configure
                btnAddPeriodUnit.Enabled = False
            Else
                'dgMonitorServiceStatusList.Rows(j).Cells(19).Enabled = False 'Delete
                'dgMonitorServiceStatusList.Rows(j).Cells(18).Enabled = False 'Edit
            End If
            ''''If C = True Then
            ''''    dgMonitorServiceStatusList.Rows(j).Cells(20).Enabled = False 'History
            ''''End If
            ''''If D = False Then
            ''''    dgMonitorServiceStatusList.Rows(j).Cells(22).Enabled = False 'View
            ''''End If

            If IsReadOnly Then
                Me.dgMonitorServiceStatusList.Rows.Item(j).BackColor = Color.OrangeRed
                Me.dgMonitorServiceStatusList.Rows.Item(j).ToolTip = "ReadOnly Aircraft"
                Me.dgMonitorServiceStatusList.Rows.Item(j).ForeColor = Color.White
            End If
        Next
    End Sub
    Private Sub SetPage()

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD"
        Else
            ServiceMPDTitle = "Service"
        End If

        If mModelMonitorService.IsNew Then
            lblTitle.Text = "Model " + ServiceMPDTitle + " of [ Model: " & mModelMonitorService.Model.Name & "] [New]"
        Else
            lblTitle.Text = "Model " + ServiceMPDTitle + " of [ Model: " & mModelMonitorService.Model.Name & "]"
        End If

        'Added By Saylee ON 4-Feb-2013 for BA04022013
        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
            lblReference.Text = "Task Source Reference"
        ElseIf AppSettings("ClientCode") = "Indamer" Then 'Added By Prashant 3-Apr-2013  'Indamer03042013
            lblReference.Text = "Task Code/Reference"
            txtReference.ToolTip = "Enter Task Code/Reference"
        Else
            lblReference.Text = "Reference"
        End If
        upnlTitle.Update()
    End Sub
    Private Sub ControlVisibility()
        btnAddPeriodUnit.Enabled = mModelMonitorServicePeriodUnitList.Count > 0
        btnPrint.Enabled = Not mModelMonitorService.IsNew
        dgMonitorServiceStatusList.Visible = IIf(CType(Session("IsFromMPDConfig"), Boolean) = True, False, Not mModelMonitorService.IsNew)
        lblResultServiceList.Visible = IIf(CType(Session("IsFromMPDConfig"), Boolean) = True, False, Not mModelMonitorService.IsNew)
        lnkTools.Enabled = Not mModelMonitorService.IsNew
        lnkSpares.Enabled = Not mModelMonitorService.IsNew
        lnkTaskCards.Enabled = Not mModelMonitorService.IsNew
        lnkLinkMaintenance.Enabled = Not mModelMonitorService.IsNew

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorService.IsNew Then
            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList = Session("mModelMonitorServiceConfiguredList")
            If Not mModelMonitorConfiguredList Is Nothing Then
                If mModelMonitorConfiguredList.Count > 0 Then
                    cmbMonitorServiceType.Enabled = False
                Else
                    cmbMonitorServiceType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mModelMonitorService.ModelMonitorServicePeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mModelMonitorConfiguredList.Count > 0 Then
                            txtFrequencyValue.Enabled = False
                        Else
                            txtFrequencyValue.Enabled = True
                        End If

                    Next i
                End With
            End If

        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mModelMonitorService.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Function Save() As Boolean
        Dim mModelMonitorServiceClone As ModelMonitorService
        mModelMonitorServiceClone = CType(mModelMonitorService, ModelMonitorService)
        setObject()
        SetGridObject()
        Dim ServiceMPDTitle As String = ""

        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "MPD"
        Else
            ServiceMPDTitle = "Model Service"
        End If
        If mModelMonitorService.IsValid = True Then
            If mModelMonitorService.ModelMonitorServicePeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired,
                                MSGBox.Message_text.PeriodRequired,
                                ServiceMPDTitle + " cannot be saved without Period units",
                                MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try


                mModelMonitorService.ApplyEdit()
                mModelMonitorService = CType(mModelMonitorService.Save(), ModelMonitorService)
                SaveAttachment()
                mServiceDetail = "Model : " & mModel & " Model Service Type : " & mModelMonitorService.ModelMonitorServiceTypeName & " Description : " & mModelMonitorService.Description
                MarkLog(Util.Action.Save,
                        "Model Service",
                        mServiceDetail,
                        Util.ErrorType.NoError,
                        mModelMonitorService.ID, EventLogID)
                'end
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorService") = mModelMonitorService
                Return True
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                End If
                mModelMonitorService = mModelMonitorServiceClone
                Session("mModelMonitorService") = mModelMonitorService
                Return False
            Finally
                mModelMonitorServiceClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub AddSelectedPeroidUnits()
        Dim clnModelMonitorService As ModelMonitorService = mModelMonitorService.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mModelMonitorService.ModelMonitorServicePeriods.Add(mModelMonitorService.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, 1) 'HardFix mMachine.HourType = 1
                End If
            Next
            For i As Integer = 0 To mModelMonitorService.ModelMonitorServicePeriods.Count - 1
                mModelMonitorService.ModelMonitorServicePeriods(i).MonitorTypeID = mModelMonitorServiceTypeList(mModelMonitorService.ModelMonitorServiceTypeID).MonitorTypeID
                If mModelMonitorServiceTypeList(mModelMonitorService.ModelMonitorServiceTypeID).MonitorTypeID = 3 Then
                    mModelMonitorService.ModelMonitorServicePeriods(i).FrequencyValue = CStr(0)
                End If
            Next
            Session("mModelMonitorService") = mModelMonitorService
        Catch ex As Exception
            mModelMonitorService = clnModelMonitorService
            Session("mModelMonitorService") = mModelMonitorService
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.Show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnModelMonitorService = Nothing
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mModelMonitorServicePeriodUnitList.Count - 1
            If Not mModelMonitorService.ModelMonitorServicePeriods.Contains(mModelMonitorServicePeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mModelMonitorServicePeriodUnitList(i).ID, mModelMonitorServicePeriodUnitList(i).PeriodID, mModelMonitorServicePeriodUnitList(i).Name)
            End If
        Next
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub UpdatePanel()
        upnlMonitorServiceDetails.Update()
        upnlATAMaster.Update()
        upnlMonitorServiceType.Update()
        upnlPeriods.Update()
        upnlOtherDetails.Update()
        upnlActionBtn.Update()
        upnlTitle.Update()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    Try
                        Session("sender") = ""
                        Dim index As Integer = Session("Index")
                        IDForEventLog = mMPDConfigurableList(index).AssemblyMonitorServiceStatusID
                        mMonitorInfo = mModelMonitorService.ModelMonitorServiceTypeName
                        mMonitorType = mModelMonitorService.MonitorTypeName
                        mMonitorDesc = mModelMonitorService.Description
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
                        'Added By Utkarsh On 01-jun-2012 FOR Link Maintenance
                        If AppSettings("LinkMaintenance") = "True" Then
                            If LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorService.ID.ToString).Count > 0 Then
                                MSGBoxCtrl.Show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                Exit Sub
                            End If
                        End If
                        'End
                        mMPDConfigurableList = MPDConfigurableList.GetMPDConfigurationList(ModelID, mModelMonitorService.ID.ToString, False, mMonitorInfo, mMonitorType, mMonitorDesc) 'tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
                        dgMonitorServiceStatusList.DataSource = mMPDConfigurableList
                        dgMonitorServiceStatusList.DataBind()
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
                            MarkLog(Util.Action.Delete, "AssemblyServices", "Can't delete :" & mAssemblyMonitorDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                        End If
                        msgCount = ex.Errors.Count
                    Finally
                        If msgCount = 0 Then
                            MarkLog(Util.Action.Delete, "AssemblyServices", mAssemblyMonitorDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                        End If
                    End Try
                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "LinkMaintenance" Then
                        Session("sender") = ""
                        mMPDConfigurableList = MPDConfigurableList.GetMPDConfigurationList(ModelID, mModelMonitorService.ID.ToString, False, mMonitorInfo, mMonitorType, mMonitorDesc) 'tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
                        dgMonitorServiceStatusList.DataSource = mMPDConfigurableList
                        dgMonitorServiceStatusList.DataBind()
                        Session("mMPDConfigurableList") = mMPDConfigurableList
                        SetGrid()
                        upnlAssemblyDetails.Update()
                    End If
                    Session("sender") = ""
            End Select
        End If
    End Sub
    Private Sub SetRights() 'Added By Utkarsh On 15-Mar-2011
        If mAssemblyStatusModelWise.IsMaster Then
            If (User.IsInRole("MachineAssemblyServicePrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatusModelWise.IsMaster Then
            If (User.IsInRole("MachineAssemblyServicePrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If

    End Sub '*******************************
    Private Sub SetToolsSparesCount()
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorService.ID)
        lnkTools.Text = "Tools (" + mMaintenanceKitDetailsCount.MaintenanceToolsCount.ToString + " record(s))"
        lnkSpares.Text = "Spares (" + mMaintenanceKitDetailsCount.MaintenanceSparesCount.ToString + " record(s))"
        lnkTaskCards.Text = "Task Cards (" + mMaintenanceKitDetailsCount.MaintenanceTasksCount.ToString + " record(s))"
        lnkLinkMaintenance.Text = "Link Maint. Activity (" + mMaintenanceKitDetailsCount.LinkMaintActivityCount.ToString + " record(s))"
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
                If (Not mModelMonitorService.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mModelMonitorService.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList(, "(SELECT)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbMonitorServiceType.DataSource = mModelMonitorServiceTypeList
        ModelName = CType(Session("ModelName"), String)
        mAssemblyStatusModelWise = AssemblyStatusModelWise.GetAssemblyStatus(ModelID)
        Session("mAssemblyStatusModelWise") = mAssemblyStatusModelWise

        mModelMonitorServicePeriodUnitList = ModelMonitorServicePeriodUnitList.GetModelMonitorServicePeriodUnitList(mAssemblyStatusModelWise.ID, IsAllPeriodsofAllAssemblyRequired:=True, ModelID:=ModelID.ToString)
        Session("mModelMonitorServicePeriodUnitList") = mModelMonitorServicePeriodUnitList
        dgPeriods.DataSource = mModelMonitorService.ModelMonitorServicePeriods

        mMPDConfigurableList = MPDConfigurableList.GetMPD_AMPConfigurationList(ModelID:=ModelID, ModelMonitorServiceID:=mModelMonitorService.ID.ToString, SkipNonConfiguredRecords:=False, MonitorInfo:=mModelMonitorService.ModelMonitorServiceTypeName, MonitorType:=mModelMonitorService.MonitorTypeName, MonitorDesc:=mModelMonitorService.Description) 'tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
        dgMonitorServiceStatusList.DataSource = mMPDConfigurableList
        Session("mMPDConfigurableList") = mMPDConfigurableList

        lblResultServiceList.Text = "List of Configurations : " + mMPDConfigurableList.Count.ToString + " Record(s)"

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorService.IsNew Then
            Dim mModelMonitorServiceConfiguredList As ModelMonitorConfiguredList
            mModelMonitorServiceConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorService.ModelID, mModelMonitorService.ID.ToString)
            Session("mModelMonitorServiceConfiguredList") = mModelMonitorServiceConfiguredList
        End If

        mMPDTypeList = MPDTypeList.GetTypeList(True)
        cmbMPDType.DataSource = mMPDTypeList

        mMPDSkillList = MPDSkillList.GetSkillList(True)
        cmbSkillcode.DataSource = mMPDSkillList

        'Added by Ajay 21-01-2023
        mLastMPDRef = LastMPDAMPRef.GetLastMPDAMPRefForModel(ModelID:=ModelID)
        Session("mLastMPDRef") = mLastMPDRef
        If (mLastMPDRef.MPDNo <> "") Then lblMPDNo.Text = "MPD No.: " + mLastMPDRef.MPDNo + ",Rev No.: " + mLastMPDRef.RevNo + ",Dated: " + mLastMPDRef.FromDateFormatted


        DataBind()
    End Sub
    Private Sub EditRecord(ByVal AssemblyMonitorServiceStatusID As Guid, ByVal AssemblyStausID As Guid, ByVal HourType As Integer)
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(AssemblyMonitorServiceStatusID, AssemblyStausID, HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("Edit") = True

        Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfNewMPDService_Ajax.aspx")
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal AssemblyMonitorServiceStatusID As Guid, ByVal AssemblyStatusID As Guid)
        Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(AssemblyMonitorServiceStatusID, AssemblyStatusID, mMachine.HourType)

        'If mPrevAssemblyMonitorServiceStatus.IsMaster Then
        '    'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Service Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfComplyAssemblyMonitorServiceStatusList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
        Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorServiceStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)

        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        mUpdateComplyHistoryAssemblyMonitorServiceStatusList = UpdateComplyHistoryAssemblyMonitorServiceStatusList.GetComplyHistoryAssemblyMonitorServiceStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorServiceStatus.ModelMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorServiceStatusList") = mUpdateComplyHistoryAssemblyMonitorServiceStatusList

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow();", True)
        'End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbATAChapter" Then
            If cmbATAChapter.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbMonitorServiceType" Then
            If cmbMonitorServiceType.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text) > 1000 Then
                custValidator.ErrorMessage = "Description can't be more than 1000 chars."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                custValidator.ErrorMessage = "Note can't be more than 1000 chars."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtReference" Then
            If Len(txtReference.Text) > 500 Then
                custValidator.ErrorMessage = "Reference Too Long"
                e.IsValid = False
            End If
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        'If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        setObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtFrequencyValue As TextBox
        If Not mModelMonitorService.IsValid Then
            For i As Integer = 0 To mModelMonitorService.GetBrokenRulesCollection.Count - 1
                str = str + mModelMonitorService.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Rows(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mModelMonitorService.ModelMonitorServicePeriods(i).IsValid Then
                For j As Integer = 0 To mModelMonitorService.ModelMonitorServicePeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorService.ModelMonitorServicePeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            If Not mModelMonitorService.ModelMonitorServicePeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mModelMonitorService.ModelMonitorServicePeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorService.ModelMonitorServicePeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvDescription.ErrorMessage = str
            cvDescription.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'Added by Vikrant on 28-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtCode.Enabled = True Then
                setFocus(txtCode)
            End If
            mModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("(SELECT)")
            Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList
            AddSelectedPeroidUnits()
            DataFieldBind()
            ControlVisibility()
            SetGrid()
            SetPage()
            SetRights()  'Added By Utkarsh On 15-Mar-2011
            SetToolsSparesCount()
            ControlVisibilityForAttachment()
        End If
    End Sub
    Private Sub dgPeriods_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgPeriods.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgPeriods.PageIndex * dgPeriods.PageSize
                If mAssemblyStatusModelWise.IsMaster Then 'Added By Utkarsh On 15-Mar-2011
                    If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                        Exit Sub
                    End If
                ElseIf Not mAssemblyStatusModelWise.IsMaster Then
                    If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                        Exit Sub
                    End If
                End If '*******************************

                'Added by saylee on 1-Jun-2016
                Dim mModelMonitorServiceConfiguredList As ModelMonitorConfiguredList
                mModelMonitorServiceConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorService.ModelID, mModelMonitorService.ID.ToString)

                If mModelMonitorServiceConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mModelMonitorServiceConfiguredList.Count - 1
                        If i = mModelMonitorServiceConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mModelMonitorServiceConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mModelMonitorServiceConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorService.ModelMonitorServicePeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If

                mModelMonitorService.ModelMonitorServicePeriods.Remove(mModelMonitorService.ModelMonitorServicePeriods.Item(Index).ID)
                Session("mModelMonitorService") = mModelMonitorService
                dgPeriods.DataSource = mModelMonitorService.ModelMonitorServicePeriods
                dgPeriods.DataBind()
                upnlPeriods.Update()
        End Select
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If Not IsValid Then upnlValidationSummary.Update() : Exit Sub
        If Not CustomValidate2() = True Then upnlValidationSummary.Update() : Exit Sub
        If Save() Then
            ControlVisibility()
            SetPage()
            UpdatePanel()
            upnlAssemblyDetails.Update()
        End If
    End Sub
    Private Sub imgbtnATAChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnATAChapter.Click
        setObject()
        RemoveSession()
    End Sub
    Private Sub btnAddPeriodUnit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeriodUnit.Click
        SetPeroidUnits()
        SetGridObject()
        setObject()


        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorService.IsNew Then
            Dim mModelMonitorServiceConfiguredList As ModelMonitorConfiguredList
            mModelMonitorServiceConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorService.ModelID, mModelMonitorService.ID.ToString)

            If mModelMonitorServiceConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mModelMonitorServiceConfiguredList.Count - 1
                    If i = mModelMonitorServiceConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mModelMonitorServiceConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mModelMonitorServiceConfiguredList(i).SerialNo + ","
                    End If
                Next
                Dim ServiceMPDTitle As String = ""

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD"
                Else
                    ServiceMPDTitle = "Service"
                End If

                ' MSGBoxCtrl.Show("Alert!", "MPD is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")

                MSGBoxCtrl.Show("Alert!",
                                ServiceMPDTitle + " is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. 
                                          So new Frequency cannot be added",
                                "In Order to add frequency please delete all configured status first.",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow();", True)
    End Sub
    Private Sub cmbMonitorServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorServiceType.SelectedIndexChanged
        mModelMonitorService.ModelMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue), Int32)
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If cmbMonitorServiceType.SelectedIndex > 0 Then
                Dim mMaxCodeMaintActTypewiseForModel As MaxCodeMaintActTypewiseForModel
                mMaxCodeMaintActTypewiseForModel = MaxCodeMaintActTypewiseForModel.GetCode(mModelMonitorService.ModelID, 6, CInt(cmbMonitorServiceType.SelectedValue))
                If Int32.TryParse(mMaxCodeMaintActTypewiseForModel.Code, Nothing) Then
                    Dim TempCode As String = (CInt(mMaxCodeMaintActTypewiseForModel.Code) + 1).ToString
                    If TempCode.Length < 3 Then
                        mModelMonitorService.Code = TempCode.PadLeft(3, "0"c)
                    Else
                        mModelMonitorService.Code = TempCode
                    End If
                    txtCode.DataBind()
                Else
                    mModelMonitorService.Code = ""
                    txtCode.DataBind()
                End If
            Else
                mModelMonitorService.Code = ""
                txtCode.DataBind()
            End If
            upnlMonitorServiceDetails.Update()
        End If
        'End
        For i As Integer = 0 To mModelMonitorService.ModelMonitorServicePeriods.Count - 1
            mModelMonitorService.ModelMonitorServicePeriods(i).MonitorTypeID = mModelMonitorServiceTypeList(mModelMonitorService.ModelMonitorServiceTypeID).MonitorTypeID
            If mModelMonitorServiceTypeList(mModelMonitorService.ModelMonitorServiceTypeID).MonitorTypeID = 3 Then
                mModelMonitorService.ModelMonitorServicePeriods(i).FrequencyValue = CStr(0)
            End If
        Next
        dgPeriods.DataSource = mModelMonitorService.ModelMonitorServicePeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "Model Service", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session.Remove("mMaintenanceTaskAndKit")
        Session.Remove("IsFromMPDConfig")
        Session("EditMasterRecord") = "False"
        Response.Redirect("index.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub lnkTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTools.Click
        If IsValid Then
            setObject()

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorService.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(mModelMonitorService)
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 3
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkSpares_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSpares.Click
        If IsValid Then
            setObject()

            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorService.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(mModelMonitorService)
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 2
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkTaskCards_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTaskCards.Click
        If IsValid Then
            setObject()
            If Not mMaintenanceTaskAndKit Is Nothing Then
                mModelMonitorService.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForService(mModelMonitorService)
            End If

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 1 'Added by Saylee on 23-July-2013 for BA22072013 	
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub lnkLinkMaintenance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLinkMaintenance.Click
        If IsValid Then
            setObject()
            Session("MaintActivityID") = mModelMonitorService.ID
            mLinkMaintenanceActionList = LinkMaintenanceActionList.GetLinkMaintActionList(True)
            Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList
            Session("ModelIDForMPD") = ModelID
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLinkMaintActivityWindow", "OpenLinkMaintActivityWindow();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnAddTools_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnTools.Click
        SetToolsSparesCount()
        Session.Remove("mChild")
        upnlOtherDetails.Update()
    End Sub
    Private Sub hdnBtnPeriodUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnPeriodUnit.Click
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        AddSelectedPeroidUnits()
        dgPeriods.DataSource = mModelMonitorService.ModelMonitorServicePeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub hdnimgBtnATAChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnATAChapter.Click
        mATAList = ATAList.GetATAList(, "(SELECT)")
        cmbATAChapter.DataSource = mATAList
        Session("mATAList") = mATAList
        cmbATAChapter.DataBind()
        upnlATAMaster.Update()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mModelMonitorService.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mModelMonitorService.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorService.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mModelMonitorService.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mModelMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorService.ID)
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

        If mModelMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorService.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mModelMonitorService.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub dgMonitorServiceStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorServiceStatusList.RowCommand
        Dim AssemblyID As Guid
        Dim AssemblyStatusID As Guid
        Dim AsOnDate As String
        Dim HourType As Integer
        Select Case e.CommandName
            Case "Configure"
                AssemblyID = New Guid(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                AssemblyStatusID = New Guid(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                AsOnDate = dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(2).Text
                HourType = CInt(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)
                mAssemblyMonitorServiceStatus.ModelMonitorServiceID(False) = mModelMonitorService.ID
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                mMachine = Machine.GetMachine(mMPDConfigurableList(CInt(e.CommandArgument)).MachineID)
                Session("mMachine") = mMachine
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
                Session("IsOpenFromMPD") = "True"
                Session("RegNo") = dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                Response.Redirect("wfAssemblyMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfNewMPDService_Ajax.aspx")
            Case "EditRec"
                AssemblyStatusID = New Guid(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                HourType = CInt(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("IsOpenFromMPD") = "True"

                mMachine = Machine.GetMachine(mMPDConfigurableList(CInt(e.CommandArgument)).MachineID)
                Session("mMachine") = mMachine
                Session("RegNo") = dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                EditRecord(mMPDConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorServiceStatusID, AssemblyStatusID, HourType)
            Case "DeleteRec"
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                'mTmpComplyAssemblyMonitorServiceStatusList.CurrentIndex = index
                'Session("mTmpComplyAssemblyMonitorServiceStatusList") = mTmpComplyAssemblyMonitorServiceStatusList
            Case "History"
                HistoryRecords(mMPDConfigurableList(CInt(e.CommandArgument)).MachineID, mMPDConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorServiceStatusID, mMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mMPDConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorServiceStatusID)
                Session("mFileAttach") = mFileAttach
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
        End Select
    End Sub
    Private Sub dgMonitorServiceStatusList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorServiceStatusList.Sorting
        mMPDConfigurableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMPDConfigurableList") = mMPDConfigurableList
        dgMonitorServiceStatusList.DataSource = mMPDConfigurableList
        dgMonitorServiceStatusList.DataBind()
        SetGrid()
    End Sub
    Private Sub hdnBtnLinkMaintActivity_Click(sender As Object, e As System.EventArgs) Handles hdnBtnLinkMaintActivity.Click
        SetToolsSparesCount()
        upnlOtherDetails.Update()
    End Sub
#End Region

#Region " Report "
    'Created By :- Pallavi , Date -10/08/2006
#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Rpt = New crDetModelMonitorService
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 9
        RHCount = Me.mModelMonitorService.ModelMonitorServicePeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Service Details", "Code/Form No.",
                  txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                 dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Service Details", "Code/Form No.",
                            txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                                  "", ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "ATA Chapter",
                            cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                          CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "ATA Chapter",
                                                   cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                                                   "", ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", lblReference.Text,
                             txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                           CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", lblReference.Text,
                                txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                                     "", ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Description",
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Description",
                                     txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                             "", ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Service Type",
                                    cmbMonitorServiceType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                       CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Service Type",
                                     cmbMonitorServiceType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                             "", ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Zone",
                                    txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                           CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Zone",
                                     txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                             "", ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Area",
                                    txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                           CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Area",
                                     txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                             "", ""))
                End If
            ElseIf I = 6 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Note",
                                    txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                           CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Note",
                                     txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                             "", ""))
                End If
            ElseIf I = 7 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Estd. Man Hours ",
                                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                           CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "Estd. Man Hours ",
                                     txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                             "", ""))
                End If
            ElseIf I = 8 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "",
                     "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                   CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Service Details", "",
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                             "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Service Details", "",
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Service",
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).PeriodUnitName, String),
                            CType(Me.mModelMonitorService.ModelMonitorServicePeriods(I).FrequencyValue, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Model Service Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
#End Region

#End Region

End Class