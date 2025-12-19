
'Created: Saylee   4-Jan-2022

Public Class wfNewCompMPDService_Ajax
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
    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Public mPartMonitorService As PartMonitorService
    Public Flag As Int16
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mPartMonitorServiceTypeList As PartMonitorServiceTypeList
    Public mPartMonitorServicePeriodUnitList As PartMonitorServicePeriodUnitList
    Dim str As String
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mIssueDate As String
    Dim EventLogID As Guid 'Added by Vikrant on 28-July-2011
    Public mServiceDetail As String
    Public mModel As String
    Public mMonitorServiceType As String
    Public mMonitorDesc As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim PartID As Guid
    Dim mCompStatusPartWise As CompStatusPartWise
    Private mCompMPDConfigurableList As CompMPDConfigurableList
    Private mUpdateComplyHistoryCompMonitorServiceStatusList As UpdateComplyHistoryCompMonitorServiceStatusList
    Public mBoardInfo As AircraftInformationBoard.BoardInfo
    Public mMachineMaintenance As MachineMaintenance
    Public mAssemblyMonitorDetail As String
    Public mAircraft As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Dim IDForEventLog As Guid
    Dim mMPDTypeList As MPDTypeList
    Dim mMPDSkillList As MPDSkillList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mCompMonitorServiceStatus = CType(Session("mCompMonitorServiceStatus"), CompMonitorServiceStatus)
        mPartMonitorService = CType(Session("mPartMonitorService"), PartMonitorService)
        mATAList = CType(Session("mATAListForNewCompMPD"), ATAList)
        mPartMonitorServiceTypeList = CType(Session("mPartMonitorServiceTypeListForNewCompMPD"), PartMonitorServiceTypeList)
        mPartMonitorServicePeriodUnitList = CType(Session("mPartMonitorServicePeriodUnitListForNewCompMPD"), PartMonitorServicePeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeletedForNewCompMPD")
        mCompStatusPartWise = Session("mCompStatusPartWise")
        PartID = Session("PartIDForNewCompMPD")
        mCompMPDConfigurableList = Session("mCompMPDConfigurableList")
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mPartMonitorService") = mPartMonitorService
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mATAListForNewCompMPD") = mATAList
        Session("mPartMonitorServiceTypeListForNewCompMPD") = mPartMonitorServiceTypeList
        Session("mPartMonitorServicePeriodUnitListForNewCompMPD") = mPartMonitorServicePeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAListForNewCompMPD")
        Session.Remove("mPartMonitorServiceTypeListForNewCompMPD")
        Session.Remove("mPartMonitorServicePeriodUnitListForNewCompMPD")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeletedForNewCompMPD")
        Session.Remove("mCompStatusPartWise")
        Session.Remove("mCompMPDConfigurableList")
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
        mPartMonitorService.Code = Trim(txtCode.Text)
        mPartMonitorService.Reference = Trim(txtReference.Text)
        mPartMonitorService.Description = Trim(txtDescription.Text)
        mPartMonitorService.Note = Trim(txtNote.Text)
        mPartMonitorService.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mPartMonitorService.PartMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue), Int32)
        mPartMonitorService.ShowInCofA = chkShowInCofA.Checked
        mPartMonitorService.RequiredManHours = txtRequiredManHours.Text.Trim
        'Added by Saylee on 23-July-2013 for BA22072013 
        mPartMonitorService.Zone = Trim(txtZone.Text)
        mPartMonitorService.Area = Trim(txtArea.Text)
        mPartMonitorService.IsRII = chkIsRII.Checked
        'End

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mPartMonitorService.IsAttachmentAdded = True
            Else
                mPartMonitorService.IsAttachmentAdded = False
            End If
            'Else
            '    .IsAttachmentAdded = False
        End If

        'Added by Saylee on 19-Apr-2023
        mPartMonitorService.TaskCardNo = txtTaskCardNo.Text.Trim
        mPartMonitorService.TaskHeading = txtTaskCardHeader.Text.Trim
        mPartMonitorService.Applicability = txtApplicability.Text.Trim
        mPartMonitorService.Source = txtSource.Text.Trim
        mPartMonitorService.Access = txtAccess.Text.Trim
        mPartMonitorService.MPDSkillID = Val(cmbSkillcode.SelectedValue.ToString)
        mPartMonitorService.MPDTypeID = Val(cmbMPDType.SelectedValue.ToString)
        mPartMonitorService.AccessOpenCloseManHours = txtAccessManHours.Text.Trim
        ''********************

        Session("mPartMonitorService") = mPartMonitorService
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mPartMonitorService.PartMonitorServicePeriods
            Dim I As Integer
            For I = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(I).FindControl("txtFrequencyValue"), TextBox)
                'Setting the Object with the Values of the Controls
                .Item(I).FrequencyValue = Trim(txtFrequencyValue.Text)
            Next I
        End With
        Session("mPartMonitorService") = mPartMonitorService
    End Sub
    Private Sub SetGrid()
        Dim B, C, D, IsReadOnly As Boolean

        For j As Integer = 0 To dgMonitorServiceStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(17).Text, Boolean) 'IsConfigurable
            C = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(21).Text, Boolean) 'IsMaster
            D = CType(Me.dgMonitorServiceStatusList.Rows(j).Cells(23).Text, Boolean) 'IsAttachmentAdded
            IsReadOnly = CType(Me.dgMonitorServiceStatusList.Rows.Item(j).Cells(24).Text, Boolean)

            dgMonitorServiceStatusList.Rows(j).Cells(16).Enabled = IIf(IsReadOnly Or B = False, False, True) 'Configure
            dgMonitorServiceStatusList.Rows(j).Cells(19).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Delete
            dgMonitorServiceStatusList.Rows(j).Cells(18).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Edit

            If B = False Then
                'dgMonitorServiceStatusList.Rows(j).Cells(16).Enabled = False 'Configure
                btnAddPeriodUnit.Enabled = False
            Else
                'dgMonitorServiceStatusList.Rows(j).Cells(19).Enabled = False 'Delete
                'dgMonitorServiceStatusList.Rows(j).Cells(18).Enabled = False 'Edit
            End If
            If C = True Then
                dgMonitorServiceStatusList.Rows(j).Cells(20).Enabled = False 'History
            End If
            If D = False Then
                dgMonitorServiceStatusList.Rows(j).Cells(22).Enabled = False 'View
            End If
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


        If mPartMonitorService.IsNew Then
            lblTitle.Text = "Part " + ServiceMPDTitle + " of [ Part: " & mPartMonitorService.Part.Name & "] [New]"
        Else
            lblTitle.Text = "Part " + ServiceMPDTitle + " of [ Part: " & mPartMonitorService.Part.Name & "]"
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
        btnAddPeriodUnit.Enabled = mPartMonitorServicePeriodUnitList.Count > 0
        btnPrint.Enabled = Not mPartMonitorService.IsNew
        dgMonitorServiceStatusList.Visible = IIf(CType(Session("IsFromMPDConfig"), Boolean) = True, False, Not mPartMonitorService.IsNew)
        lblResultServiceList.Visible = IIf(CType(Session("IsFromMPDConfig"), Boolean) = True, False, Not mPartMonitorService.IsNew)
        lnkTools.Enabled = Not mPartMonitorService.IsNew
        lnkSpares.Enabled = Not mPartMonitorService.IsNew
        lnkTaskCards.Enabled = Not mPartMonitorService.IsNew

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorService.IsNew Then
            Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList = Session("mPartMonitorServiceConfiguredList")
            If Not mPartMonitorServiceConfiguredList Is Nothing Then
                If mPartMonitorServiceConfiguredList.Count > 0 Then
                    cmbMonitorServiceType.Enabled = False
                Else
                    cmbMonitorServiceType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mPartMonitorService.PartMonitorServicePeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mPartMonitorServiceConfiguredList.Count > 0 Then
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
        If mPartMonitorService.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Function Save() As Boolean
        Dim mPartMonitorServiceClone As PartMonitorService
        mPartMonitorServiceClone = CType(mPartMonitorService, PartMonitorService)
        setObject()
        SetGridObject()
        If mPartMonitorService.IsValid = True Then
            If mPartMonitorService.PartMonitorServicePeriods.Count = 0 Then
                Dim ServiceMPDTitle As String = ""
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD"
                Else
                    ServiceMPDTitle = "Part Service"
                End If

                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save " + ServiceMPDTitle + "." + ServiceMPDTitle + " can not be saved without period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mPartMonitorService.ApplyEdit()
                mPartMonitorService = CType(mPartMonitorService.Save(), PartMonitorService)
                SaveAttachment()
                'Commented By Utkarsh On 27-Jul-2011 For All19072011
                '     MarkLog(Util.Action.Save, "PartMonitorSer", "ATAChapter->" + mPartMonitorService.ATAChapter + " -> " + " Part Name -> " + mPartMonitorService.Part.Name + " Part Monitor Service Type Name -> " + mPartMonitorService.PartMonitorServiceTypeName, Util.ErrorType.NoError, mPartMonitorService.ID)
                'End
                Session("mPartMonitorService") = mPartMonitorService
                Return True
            Catch ex As SqlException
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                End If
                mPartMonitorService = mPartMonitorServiceClone
                Session("mPartMonitorService") = mPartMonitorService
                Return False
            Finally
                'Added By Utkarsh On 26-Jul-2011 For All19072011
                'MaintDetail = "Monitor Service Type : " + mPartMonitorService.PartMonitorServiceTypeName + " Description : " + mPartMonitorService.Description
                mServiceDetail = "Part : " & mPartMonitorService.Part.Name & " Part Service Type : " & mPartMonitorService.PartMonitorServiceTypeName & " Description : " & mPartMonitorService.Description
                MarkLog(Util.Action.Save, "Part Service", mServiceDetail, Util.ErrorType.NoError, mPartMonitorService.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub AddSelectedPeroidUnits()
        Dim clnPartMonitorService As PartMonitorService = mPartMonitorService.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mPartMonitorService.PartMonitorServicePeriods.Add(mPartMonitorService.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, 1) 'HardFix mMachine.HourType = 1
                    mPartMonitorService.SetZeroFrequencyValue()
                End If
            Next
            Session("mPartMonitorService") = mPartMonitorService
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mPartMonitorService = clnPartMonitorService
            Session("mPartMonitorService") = mPartMonitorService
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnPartMonitorService = Nothing
            mSelectPeriodUnits = Nothing
            Session.Remove("mSelectPeriodUnits")
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mPartMonitorServicePeriodUnitList.Count - 1
            If Not mPartMonitorService.PartMonitorServicePeriods.Contains(mPartMonitorServicePeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mPartMonitorServicePeriodUnitList(i).ID, mPartMonitorServicePeriodUnitList(i).PeriodID, mPartMonitorServicePeriodUnitList(i).Name)
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
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            Dim index As Integer = Session("Index")
                            IDForEventLog = mCompMPDConfigurableList(index).CompMonitorServiceStatusID
                            mServiceDetail = "Reg No. : " + mCompMPDConfigurableList(index).RegNo & " Assembly Info : " & mCompMPDConfigurableList(index).AssemblyInfo & " Part Info : " & mCompMPDConfigurableList(index).PartSerialNo & " Monitor Info : " & mCompMPDConfigurableList(index).MonitorInfo

                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMPDConfigurableList(index).CompMonitorServiceStatusID, 9)

                            '=============================
                            If mCompMPDConfigurableList(index).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mCompMPDConfigurableList(index).CompMonitorServiceStatusID)
                            End If
                            CompMonitorServiceStatus.DeleteCompMonitorServiceStatus(mCompMPDConfigurableList(index).CompMonitorServiceStatusID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            Session("mMachineMaintenance") = mMachineMaintenance
                            mCompMPDConfigurableList = CompMPDConfigurableList.GetMPDConfigurationServiceList(PartID:=PartID, PartMonitorServiceID:=mPartMonitorService.ID.ToString, SkipNonConfiguredRecords:=False, ModelID:=mPartMonitorService.ModelID.ToString)
                            dgMonitorServiceStatusList.DataSource = mCompMPDConfigurableList
                            dgMonitorServiceStatusList.DataBind()
                            Session("mCompMPDConfigurableList") = mCompMPDConfigurableList
                            SetGrid()
                            upnlAssemblyDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "ComponentServices", "Can't delete : " & mServiceDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'mEnquiry.ID)'Added By Utkarsh On 28-Jul-2011 For All19072011
                                'End
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "ComponentServices", mServiceDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID) 'Added By Utkarsh On 28-Jul-2011 For All19072011
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    Session("sender") = ""
            End Select
        End If
    End Sub
    Private Sub SetRights() 'Added By Utkarsh On 15-Mar-2011
        If mCompStatusPartWise.IsMaster Then
            If (User.IsInRole("MachineAssemblyServicePrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mCompStatusPartWise.IsMaster Then
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
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorService.ID)
        lnkTools.Text = "Tools (" + mMaintenanceKitDetailsCount.MaintenanceToolsCount.ToString + " record(s))"
        lnkSpares.Text = "Spares (" + mMaintenanceKitDetailsCount.MaintenanceSparesCount.ToString + " record(s))"
        lnkTaskCards.Text = "Task Cards (" + mMaintenanceKitDetailsCount.MaintenanceTasksCount.ToString + " record(s))"
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
                If (Not mPartMonitorService.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mPartMonitorService.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeletedForNewCompMPD") = IsAttachmentDeleted
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList(, "(SELECT)")
        Session("mATAListForNewCompMPD") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbMonitorServiceType.DataSource = mPartMonitorServiceTypeList
        mCompStatusPartWise = CompStatusPartWise.GetCompStatus(PartID) 'fetch comp status of any one comp for getting periods
        Session("mCompStatusPartWise") = mCompStatusPartWise

        mPartMonitorServicePeriodUnitList = PartMonitorServicePeriodUnitList.GetPartMonitorServicePeriodUnitList(mCompStatusPartWise.ID, IsAllPeriodsofAllCompsRequired:=True, PartID:=PartID.ToString)
        Session("mPartMonitorServicePeriodUnitListForNewCompMPD") = mPartMonitorServicePeriodUnitList
        dgPeriods.DataSource = mPartMonitorService.PartMonitorServicePeriods

        mCompMPDConfigurableList = CompMPDConfigurableList.GetMPDConfigurationServiceList(PartID:=PartID, PartMonitorServiceID:=mPartMonitorService.ID.ToString, SkipNonConfiguredRecords:=False, ModelID:=mPartMonitorService.ModelID.ToString)
        dgMonitorServiceStatusList.DataSource = mCompMPDConfigurableList
        Session("mCompMPDConfigurableList") = mCompMPDConfigurableList

        lblResultServiceList.Text = "List of Configurations : " + mCompMPDConfigurableList.Count.ToString + " Record(s)"
        mMPDTypeList = MPDTypeList.GetTypeList(True)
        cmbMPDType.DataSource = mMPDTypeList

        mMPDSkillList = MPDSkillList.GetSkillList(True)
        cmbSkillcode.DataSource = mMPDSkillList


        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorService.IsNew Then
            Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList
            mPartMonitorServiceConfiguredList = PartMonitorConfiguredList.GetPartMonitorServiceConfiguredList(mPartMonitorService.PartID, mPartMonitorService.ID.ToString)
            Session("mPartMonitorServiceConfiguredList") = mPartMonitorServiceConfiguredList
        End If
        DataBind()
    End Sub
    Private Sub EditRecord(ByVal CompMonitorServiceStatusID As Guid, ByVal CompStausID As Guid, ByVal AssemblyStausID As Guid, ByVal HourType As Integer)
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        mCompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(CompMonitorServiceStatusID, AssemblyStausID, CompStausID, HourType)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Response.Redirect("wfCompMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfNewCompMPDService_Ajax.aspx")
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal CompMonitorServiceStatusID As Guid, ByVal AssemblyStatusID As Guid, ByVal CompStatusID As Guid, ByVal DoneOn As String)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
        Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(CompMonitorServiceStatusID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mMachine.HourType)
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
        Session("EnFrom") = 1 'EditRecord

        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn) 'CHK For DoneOn Date
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus

        mUpdateComplyHistoryCompMonitorServiceStatusList = UpdateComplyHistoryCompMonitorServiceStatusList.GetComplyHistoryCompMonitorServiceStatusList(mCompStatus.CompID, mCompMonitorServiceStatus.PartMonitorServiceID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorServiceStatusList") = mUpdateComplyHistoryCompMonitorServiceStatusList
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenServiceHistoryWindow", "OpenServiceHistoryWindow();", True)
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
        If Not mPartMonitorService.IsValid Then
            For i As Integer = 0 To mPartMonitorService.GetBrokenRulesCollection.Count - 1
                str = str + mPartMonitorService.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Items(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mPartMonitorService.PartMonitorServicePeriods(i).IsValid Then
                For j As Integer = 0 To mPartMonitorService.PartMonitorServicePeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorService.PartMonitorServicePeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
            If Not mPartMonitorService.PartMonitorServicePeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mPartMonitorService.PartMonitorServicePeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorService.PartMonitorServicePeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            'If txtCode.Enabled = True Then
            '    setFocus(txtCode)
            'End If
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                txtTaskCardNo.Focus()
            ElseIf txtCode.Enabled = True Then
                txtCode.Focus()
            End If
            mPartMonitorServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList("(SELECT)")
            Session("mPartMonitorServiceTypeListForNewCompMPD") = mPartMonitorServiceTypeList
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
                If (Not User.IsInRole("MachineDelete") Or Not User.IsInRole("ComponentInstallationDelete")) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                'Added by saylee on 1-Jun-2016
                Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList
                mPartMonitorServiceConfiguredList = PartMonitorConfiguredList.GetPartMonitorServiceConfiguredList(mPartMonitorService.PartID, mPartMonitorService.ID.ToString)

                If mPartMonitorServiceConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mPartMonitorServiceConfiguredList.Count - 1
                        If i = mPartMonitorServiceConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mPartMonitorService.PartMonitorServicePeriods.Item(Index).PeriodUnitName + " frequency is configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If
                mPartMonitorService.PartMonitorServicePeriods.Remove(mPartMonitorService.PartMonitorServicePeriods.Item(Index).ID)
                Session("mPartMonitorService") = mPartMonitorService
                dgPeriods.DataSource = mPartMonitorService.PartMonitorServicePeriods
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
        If Not mPartMonitorService.IsNew Then
            Dim mPartMonitorServiceConfiguredList As PartMonitorConfiguredList
            mPartMonitorServiceConfiguredList = PartMonitorConfiguredList.GetPartMonitorServiceConfiguredList(mPartMonitorService.PartID, mPartMonitorService.ID.ToString)

            If mPartMonitorServiceConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mPartMonitorServiceConfiguredList.Count - 1
                    If i = mPartMonitorServiceConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mPartMonitorServiceConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.show("Alert!", "Service is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow();", True)
    End Sub
    Private Sub cmbMonitorServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorServiceType.SelectedIndexChanged
        mPartMonitorService.PartMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue), Int32)
        dgPeriods.DataSource = mPartMonitorService.PartMonitorServicePeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "Part Service", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                mPartMonitorService.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompService(mPartMonitorService)

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
                mPartMonitorService.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompService(mPartMonitorService)

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
                mPartMonitorService.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompService(mPartMonitorService)

            Session("mMaintenanceTaskAndKit") = mMaintenanceTaskAndKit
            Session("mChild") = 1   'Added by Saylee on 23-July-2013 for BA22072013 
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenToolsWindow", "OpenToolsWindow();", True)
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
        dgPeriods.DataSource = mPartMonitorService.PartMonitorServicePeriods
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
        mPartMonitorService.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mPartMonitorService.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorService.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPartMonitorService.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mPartMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorService.ID)
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

        If mPartMonitorService.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorService.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mPartMonitorService.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub dgMonitorServiceStatusList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorServiceStatusList.RowCommand
        Dim CompID As Guid
        Dim CompStatusID As Guid
        Dim HourType As Integer
        Select Case e.CommandName
            Case "Configure"
                CompID = New Guid(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                CompStatusID = New Guid(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                HourType = CInt(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mMachine As Machine = Machine.GetMachine(mCompMPDConfigurableList(CInt(e.CommandArgument)).MachineID)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString)
                mCompMonitorServiceStatus = CompMonitorServiceStatus.NewCompMonitorServiceStatus(Guid.NewGuid, mCompStatus.CompID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString, mCompStatus.Comp.PartID, mCompMPDConfigurableList(CInt(e.CommandArgument)).ModelID, CompStatusID, HourType)
                mCompMonitorServiceStatus.PartMonitorServiceID(True) = mPartMonitorService.ID
                Session("mCompStatus") = mCompStatus
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                Session("mMachine") = mMachine
                Session("IsOpenFromMPD") = "True"
                Session("RegNo") = dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                Response.Redirect("wfCompMonitorServiceStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage4=wfNewCompMPDService_Ajax.aspx")
            Case "EditRec"
                CompStatusID = New Guid(dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString)
                Dim mMachine As Machine = Machine.GetMachine(mCompMPDConfigurableList(CInt(e.CommandArgument)).MachineID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompStatus") = mCompStatus
                Session("IsOpenFromMPD") = "True"
                Session("mMachine") = mMachine
                Session("RegNo") = dgMonitorServiceStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                EditRecord(mCompMPDConfigurableList(CInt(e.CommandArgument)).CompMonitorServiceStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).CompStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).HourType)
            Case "DeleteRec"
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "Delete Component Service Status.", MsgBoxStyle.YesNo, "Delete")
            Case "History"
                HistoryRecords(mCompMPDConfigurableList(CInt(e.CommandArgument)).MachineID, mCompMPDConfigurableList(CInt(e.CommandArgument)).CompMonitorServiceStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).CompStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).DoneOnFormatted.ToString)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mCompMPDConfigurableList(CInt(e.CommandArgument)).CompMonitorServiceStatusID)
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
    Private Sub dgMonitorServiceStatusList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorServiceStatusList.Sorting
        mCompMPDConfigurableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompMPDConfigurableList") = mCompMPDConfigurableList
        dgMonitorServiceStatusList.DataSource = mCompMPDConfigurableList
        dgMonitorServiceStatusList.DataBind()
        SetGrid()
    End Sub
#End Region

End Class