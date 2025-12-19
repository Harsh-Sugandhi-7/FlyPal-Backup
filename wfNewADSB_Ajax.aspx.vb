'Added By Vikrant For ADSBConfig

Public Class wfNewADSB_Ajax
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
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mModelMonitorMod As ModelMonitorMod
    Public Flag As Int16
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mModelMonitorModTypeList As ModelMonitorModTypeList
    Public mModelMonitorModPeriodUnitList As ModelMonitorModPeriodUnitList
    Dim str As String
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mIssueDate As String
    Public mAssemblyMonitorModStatusList As tmpAssemblyMonitorModStatusList
    Dim EventLogID As Guid 'Added by Vikrant on 28-July-2011
    Public mDirectiveDetail As String
    Public mModel As String
    Public mMonitorDirectiveType As String
    Public mMonitorDesc As String
    Public mLinkMaintenanceActionList As LinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance 'End
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim ModelID As Guid
    Dim ModelName As String
    Dim mAssemblyStatusModelWise As AssemblyStatusModelWise
    Private mADSBConfigurableList As ADSBConfigurableList
    Private mUpdateComplyHistoryAssemblyMonitorModStatusList As UpdateComplyHistoryAssemblyMonitorModStatusList
    Public mBoardInfo As AircraftInformationBoard.BoardInfo
    Public mMachineMaintenance As MachineMaintenance
    Public mAssemblyMonitorDetail As String
    Public mAircraft As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Dim IDForEventLog As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMachine = CType(Session("mMachine"), Machine)
        mAssemblyMonitorModStatus = CType(Session("mAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mModelMonitorMod = CType(Session("mModelMonitorMod"), ModelMonitorMod)
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorModTypeList = CType(Session("mModelMonitorModTypeList"), ModelMonitorModTypeList)
        mModelMonitorModPeriodUnitList = CType(Session("mModelMonitorModPeriodUnitList"), ModelMonitorModPeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mAssemblyMonitorModStatusList = CType(Session("mAssemblyMonitorModStatusList"), tmpAssemblyMonitorModStatusList)
        mLinkMaintenanceActionList = Session("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        mLinkMaintenanceList = Session("mLinkMaintenanceList") 'End
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mAssemblyStatusModelWise = Session("mAssemblyStatusModelWise")
        ModelID = Session("ModelIDForADSB")
        mADSBConfigurableList = Session("mADSBConfigurableList")
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mModelMonitorMod") = mModelMonitorMod
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mATAList") = mATAList
        Session("mModelMonitorModTypeList") = mModelMonitorModTypeList
        Session("mModelMonitorModPeriodUnitList") = mModelMonitorModPeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mAssemblyMonitorModStatusList") = mAssemblyMonitorModStatusList
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorModTypeList")
        Session.Remove("mModelMonitorModPeriodUnitList")
        Session.Remove("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session.Remove("mLinkMaintenanceList")
        Session.Remove("URL")
        Session.Remove("MaintenanceActivityID") 'End
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mAssemblyStatusModelWise")
        Session.Remove("mADSBConfigurableList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "NewADSB"


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
        'mModelMonitorMod.Code = Trim(txtCode.Text)
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If Trim(txtCode.Text).Length < 3 And Trim(txtCode.Text) <> "" Then
                mModelMonitorMod.Code = Trim(txtCode.Text).PadLeft(3, "0"c)
            Else
                mModelMonitorMod.Code = Trim(txtCode.Text)
            End If
        Else
            mModelMonitorMod.Code = Trim(txtCode.Text)
        End If
        mModelMonitorMod.Reference = Trim(txtReference.Text)
        mModelMonitorMod.Description = Trim(txtDescription.Text)
        mModelMonitorMod.Number = Trim(txtModificationNo.Text)
        If calIssueDate.Text = "" Then
            mModelMonitorMod.IssueDate = System.DBNull.Value
        Else
            mModelMonitorMod.IssueDate = calIssueDate.Text
        End If
        mModelMonitorMod.Note = Trim(txtNote.Text)
        mModelMonitorMod.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mModelMonitorMod.ModelMonitorModTypeID = CType(Val(cmbMonitorModType.SelectedValue), Int32)
        mModelMonitorMod.Applicability = Trim(txtApplicability.Text)
        mModelMonitorMod.ComplianceRequirement = Trim(txtComplianceRequirement.Text)
        mModelMonitorMod.RequiredManHours = txtRequiredManHours.Text.Trim
        mModelMonitorMod.SupersededByADNumber = Trim(txtSupersededByADNumber.Text)
        mModelMonitorMod.Zone = Trim(txtZone.Text) 'Added by Saylee on 23-July-2013 for BA22072013 
        mModelMonitorMod.Area = Trim(txtArea.Text)
        mModelMonitorMod.IsRII = chkIsRII.Checked 'End
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorMod.IsAttachmentAdded = True
            Else
                mModelMonitorMod.IsAttachmentAdded = False
            End If
        End If
        Session("mModelMonitorMod") = mModelMonitorMod
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mModelMonitorMod.ModelMonitorModPeriods
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
        mModelMonitorMod = Session("mModelMonitorMod")
    End Sub
    Private Sub SetGrid()
        Dim B, C, D, IsReadOnly As Boolean

        For j As Integer = 0 To dgMonitorModStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorModStatusList.Rows(j).Cells(17).Text, Boolean) 'IsConfigurable
            C = CType(Me.dgMonitorModStatusList.Rows(j).Cells(21).Text, Boolean) 'IsMaster
            D = CType(Me.dgMonitorModStatusList.Rows(j).Cells(23).Text, Boolean) 'IsAttachmentAdded
            IsReadOnly = CType(Me.dgMonitorModStatusList.Rows.Item(j).Cells(24).Text, Boolean)

            dgMonitorModStatusList.Rows(j).Cells(16).Enabled = IIf(IsReadOnly Or B = False, False, True) 'Configure
            dgMonitorModStatusList.Rows(j).Cells(19).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Delete
            dgMonitorModStatusList.Rows(j).Cells(18).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Edit

            If B = False Then
                'dgMonitorModStatusList.Rows(j).Cells(16).Enabled = False  'Configure
                btnAddPeriodUnit.Enabled = False
            Else
                'dgMonitorModStatusList.Rows(j).Cells(19).Enabled = False 'Delete
                'dgMonitorModStatusList.Rows(j).Cells(18).Enabled = False 'Edit
            End If

            If C = True Then
                dgMonitorModStatusList.Rows(j).Cells(20).Enabled = False 'History
            End If
            If D = False Then
                dgMonitorModStatusList.Rows(j).Cells(22).Enabled = False 'View
            End If
            If IsReadOnly Then
                Me.dgMonitorModStatusList.Rows.Item(j).BackColor = Color.OrangeRed
                Me.dgMonitorModStatusList.Rows.Item(j).ToolTip = "ReadOnly Aircraft"
                Me.dgMonitorModStatusList.Rows.Item(j).ForeColor = Color.White
            End If
        Next
    End Sub
    Private Sub SetPage()
        If mModelMonitorMod.IsNew Then
            lblTitle.Text = "Model Directive of [ Model: " & mModelMonitorMod.Model.Name & "] [New]"
        Else
            lblTitle.Text = "Model Directive of [ Model: " & mModelMonitorMod.Model.Name & "]"
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
        btnAddPeriodUnit.Enabled = mModelMonitorModPeriodUnitList.Count > 0
        btnPrint.Enabled = Not mModelMonitorMod.IsNew
        dgMonitorModStatusList.Visible = IIf(CType(Session("IsFromADSBConfig"), Boolean) = True, False, Not mModelMonitorMod.IsNew)
        lblResultModList.Visible = IIf(CType(Session("IsFromADSBConfig"), Boolean) = True, False, Not mModelMonitorMod.IsNew)
        lnkTools.Enabled = Not mModelMonitorMod.IsNew
        lnkSpares.Enabled = Not mModelMonitorMod.IsNew
        lnkTaskCards.Enabled = Not mModelMonitorMod.IsNew
        lnkLinkMaintenance.Enabled = Not mModelMonitorMod.IsNew

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorMod.IsNew Then
            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList = Session("mModelMonitorModConfiguredList")
            If Not mModelMonitorConfiguredList Is Nothing Then
                If mModelMonitorConfiguredList.Count > 0 Then
                    cmbMonitorModType.Enabled = False
                Else
                    cmbMonitorModType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mModelMonitorMod.ModelMonitorModPeriods
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
        If mModelMonitorMod.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Function Save() As Boolean
        Dim mModelMonitorModClone As ModelMonitorMod
        mModelMonitorModClone = CType(mModelMonitorMod, ModelMonitorMod)
        setObject()
        SetGridObject()
        If mModelMonitorMod.IsValid = True Then
            If mModelMonitorMod.ModelMonitorModPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "Model Directive cannot be saved without Period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mModelMonitorMod.ApplyEdit()
                mModelMonitorMod = CType(mModelMonitorMod.Save(), ModelMonitorMod)
                SaveAttachment()
                mDirectiveDetail = "Model : " & mModel & " Model Directive Type : " & mModelMonitorMod.ModelMonitorModTypeName & " Description : " & mModelMonitorMod.Description & " Directive Number : " & mModelMonitorMod.Number & " Effective Date : " & mModelMonitorMod.IssueDateFormatted.ToString
                MarkLog(Util.Action.Save, "Model Directive", mDirectiveDetail, Util.ErrorType.NoError, mModelMonitorMod.ID, EventLogID)
                'end
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorMod") = mModelMonitorMod
                Return True
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                End If
                mModelMonitorMod = mModelMonitorModClone
                Session("mModelMonitorMod") = mModelMonitorMod
                Return False
            Finally
                mModelMonitorModClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub AddSelectedPeroidUnits()
        Dim clnModelMonitorMod As ModelMonitorMod = mModelMonitorMod.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mModelMonitorMod.ModelMonitorModPeriods.Add(mModelMonitorMod.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, 1) 'HardFix mMachine.HourType = 1
                End If
            Next
            For i As Integer = 0 To mModelMonitorMod.ModelMonitorModPeriods.Count - 1
                mModelMonitorMod.ModelMonitorModPeriods(i).MonitorTypeID = mModelMonitorModTypeList(mModelMonitorMod.ModelMonitorModTypeID).MonitorTypeID
                If mModelMonitorModTypeList(mModelMonitorMod.ModelMonitorModTypeID).MonitorTypeID = 3 Then
                    mModelMonitorMod.ModelMonitorModPeriods(i).FrequencyValue = CStr(0)
                End If
            Next
            Session("mModelMonitorMod") = mModelMonitorMod
        Catch ex As Exception
            mModelMonitorMod = clnModelMonitorMod
            Session("mModelMonitorMod") = mModelMonitorMod
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnModelMonitorMod = Nothing
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mModelMonitorModPeriodUnitList.Count - 1
            If Not mModelMonitorMod.ModelMonitorModPeriods.Contains(mModelMonitorModPeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mModelMonitorModPeriodUnitList(i).ID, mModelMonitorModPeriodUnitList(i).PeriodID, mModelMonitorModPeriodUnitList(i).Name)
            End If
        Next
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub UpdatePanel()
        upnlMonitorDirectiveDetails.Update()
        upnlATAMaster.Update()
        upnlMonitorDirectiveType.Update()
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
                        IDForEventLog = mADSBConfigurableList(index).AssemblyMonitorModStatusID
                        mMonitorInfo = mModelMonitorMod.ModelMonitorModTypeName
                        mMonitorType = mModelMonitorMod.MonitorTypeName
                        mMonitorDesc = mModelMonitorMod.Description
                        mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc
                        'End
                        'Added by Saylee on 28-May-2009
                        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mADSBConfigurableList(index).AssemblyMonitorModStatusID)
                        '********************************
                        If mADSBConfigurableList(index).IsAttachmentAdded = True Then
                            mFileAttach = FileAttach.GetAttachment(mADSBConfigurableList(index).AssemblyMonitorModStatusID)
                        End If
                        'Added by Saylee on 9th-Oct-2009
                        mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mADSBConfigurableList(index).AssemblyMonitorModStatusID, 7)
                        '=============================

                        AssemblyMonitorModStatus.DeleteAssemblyMonitorModStatus(mADSBConfigurableList(index).AssemblyMonitorModStatusID)

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
                            If LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorMod.ID.ToString).Count > 0 Then
                                MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                Exit Sub
                            End If
                        End If
                        'End
                        mADSBConfigurableList = ADSBConfigurableList.GetADSBConfigurationList(ModelID, mModelMonitorMod.ID.ToString, False, mMonitorInfo, mMonitorType, mMonitorDesc) 'tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
                        dgMonitorModStatusList.DataSource = mADSBConfigurableList
                        dgMonitorModStatusList.DataBind()
                        Session("mADSBConfigurableList") = mADSBConfigurableList
                        SetGrid()
                        upnlAssemblyDetails.Update()
                    Catch ex As SqlException
                        If ex.Number = 8145 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 2627 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 547 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            MarkLog(Util.Action.Delete, "AssemblyModifications", "Can't delete :" & mAssemblyMonitorDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                        End If
                        msgCount = ex.Errors.Count
                    Finally
                        If msgCount = 0 Then
                            MarkLog(Util.Action.Delete, "AssemblyModifications", mAssemblyMonitorDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                        End If
                    End Try
                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "LinkMaintenance" Then
                        Session("sender") = ""
                        mADSBConfigurableList = ADSBConfigurableList.GetADSBConfigurationList(ModelID, mModelMonitorMod.ID.ToString, False, mMonitorInfo, mMonitorType, mMonitorDesc) 'tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
                        dgMonitorModStatusList.DataSource = mADSBConfigurableList
                        dgMonitorModStatusList.DataBind()
                        Session("mADSBConfigurableList") = mADSBConfigurableList
                        SetGrid()
                        upnlAssemblyDetails.Update()
                    End If
                    Session("sender") = ""
            End Select
        End If
    End Sub
    Private Sub SetRights() 'Added By Utkarsh On 15-Mar-2011
        If mAssemblyStatusModelWise.IsMaster Then
            If (User.IsInRole("MachineAssemblyModificationPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatusModelWise.IsMaster Then
            If (User.IsInRole("MachineAssemblyModificationPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If

    End Sub '*******************************
    Private Sub SetToolsSparesCount()
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorMod.ID)
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
                If (Not mModelMonitorMod.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mModelMonitorMod.ID)
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
        cmbMonitorModType.DataSource = mModelMonitorModTypeList
        ModelName = CType(Session("ModelName"), String)
        mAssemblyStatusModelWise = AssemblyStatusModelWise.GetAssemblyStatus(ModelID)
        Session("mAssemblyStatusModelWise") = mAssemblyStatusModelWise

        mModelMonitorModPeriodUnitList = ModelMonitorModPeriodUnitList.GetModelMonitorModPeriodUnitList(mAssemblyStatusModelWise.ID, True, ModelID.ToString)
        Session("mModelMonitorModPeriodUnitList") = mModelMonitorModPeriodUnitList
        dgPeriods.DataSource = mModelMonitorMod.ModelMonitorModPeriods

        mADSBConfigurableList = ADSBConfigurableList.GetADSBConfigurationList(ModelID:=ModelID, ModelMonitorModID:=mModelMonitorMod.ID.ToString, SkipNonConfiguredRecords:=False, MonitorInfo:=mModelMonitorMod.ModelMonitorModTypeName, MonitorType:=mModelMonitorMod.MonitorTypeName, MonitorDesc:=mModelMonitorMod.Description) 'tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
        dgMonitorModStatusList.DataSource = mADSBConfigurableList
        Session("mADSBConfigurableList") = mADSBConfigurableList

        lblResultModList.Text = "List of Configurations : " + mADSBConfigurableList.Count.ToString + " Record(s)"

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorMod.IsNew Then
            Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList
            mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(mModelMonitorMod.ModelID, mModelMonitorMod.ID.ToString)
            Session("mModelMonitorModConfiguredList") = mModelMonitorModConfiguredList
        End If
        calIssueDate.Text = mModelMonitorMod.IssueDateFormatted.ToString

        DataBind()
    End Sub
    Private Sub EditRecord(ByVal AssemblyMonitorModStatusID As Guid, ByVal AssemblyStausID As Guid, ByVal HourType As Integer)
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(AssemblyMonitorModStatusID, AssemblyStausID, HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("Edit") = True
        Response.Redirect("wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfNewADSB_Ajax.aspx")
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal AssemblyMonitorModStatusID As Guid, ByVal AssemblyStatusID As Guid)
        Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(AssemblyMonitorModStatusID, AssemblyStatusID, mMachine.HourType)

        'If mPrevAssemblyMonitorModStatus.IsMaster Then
        '    'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Directive Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfComplyAssemblyMonitorInspStatusList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)

        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        mUpdateComplyHistoryAssemblyMonitorModStatusList = UpdateComplyHistoryAssemblyMonitorModStatusList.GetComplyHistoryAssemblyMonitorModStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorModStatus.ModelMonitorModID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorModStatusList") = mUpdateComplyHistoryAssemblyMonitorModStatusList

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDirectiveHistoryWindow", "OpenDirectiveHistoryWindow();", True)
        'End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbATAChapter" Then
            If cmbATAChapter.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbMonitorModType" Then
            If cmbMonitorModType.SelectedIndex <= 0 Then
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
        If Not mModelMonitorMod.IsValid Then
            For i As Integer = 0 To mModelMonitorMod.GetBrokenRulesCollection.Count - 1
                str = str + mModelMonitorMod.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Rows(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mModelMonitorMod.ModelMonitorModPeriods(i).IsValid Then
                For j As Integer = 0 To mModelMonitorMod.ModelMonitorModPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorMod.ModelMonitorModPeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
            If Not mModelMonitorMod.ModelMonitorModPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mModelMonitorMod.ModelMonitorModPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorMod.ModelMonitorModPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            mModelMonitorModTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList("(SELECT)")
            Session("mModelMonitorModTypeList") = mModelMonitorModTypeList
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
                    If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                        Exit Sub
                    End If
                ElseIf Not mAssemblyStatusModelWise.IsMaster Then
                    If (User.IsInRole("MachineAssemblyModificationNew") Or User.IsInRole("MachineAssemblyModificationEdit")) = False Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                        Exit Sub
                    End If
                End If '*******************************

                'Added by saylee on 1-Jun-2016
                Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList
                mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(mModelMonitorMod.ModelID, mModelMonitorMod.ID.ToString)

                If mModelMonitorModConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mModelMonitorModConfiguredList.Count - 1
                        If i = mModelMonitorModConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mModelMonitorMod.ModelMonitorModPeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If

                mModelMonitorMod.ModelMonitorModPeriods.Remove(mModelMonitorMod.ModelMonitorModPeriods.Item(Index).ID)
                Session("mModelMonitorMod") = mModelMonitorMod
                dgPeriods.DataSource = mModelMonitorMod.ModelMonitorModPeriods
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
        If Not mModelMonitorMod.IsNew Then
            Dim mModelMonitorModConfiguredList As ModelMonitorConfiguredList
            mModelMonitorModConfiguredList = ModelMonitorConfiguredList.GetModelMonitorModConfiguredList(mModelMonitorMod.ModelID, mModelMonitorMod.ID.ToString)

            If mModelMonitorModConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mModelMonitorModConfiguredList.Count - 1
                    If i = mModelMonitorModConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mModelMonitorModConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.show("Alert!", "AD/SB is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow();", True)
    End Sub
    Private Sub cmbMonitorModType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorModType.SelectedIndexChanged
        mModelMonitorMod.ModelMonitorModTypeID = CType(Val(cmbMonitorModType.SelectedValue), Int32)
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If cmbMonitorModType.SelectedIndex > 0 Then
                Dim mMaxCodeMaintActTypewiseForModel As MaxCodeMaintActTypewiseForModel
                mMaxCodeMaintActTypewiseForModel = MaxCodeMaintActTypewiseForModel.GetCode(mModelMonitorMod.ModelID, 7, CInt(cmbMonitorModType.SelectedValue))
                If Int32.TryParse(mMaxCodeMaintActTypewiseForModel.Code, Nothing) Then
                    Dim TempCode As String = (CInt(mMaxCodeMaintActTypewiseForModel.Code) + 1).ToString
                    If TempCode.Length < 3 Then
                        mModelMonitorMod.Code = TempCode.PadLeft(3, "0"c)
                    Else
                        mModelMonitorMod.Code = TempCode
                    End If
                    txtCode.DataBind()
                Else
                    mModelMonitorMod.Code = ""
                    txtCode.DataBind()
                End If
            Else
                mModelMonitorMod.Code = ""
                txtCode.DataBind()
            End If
            upnlMonitorDirectiveDetails.Update()
        End If
        'End
        For i As Integer = 0 To mModelMonitorMod.ModelMonitorModPeriods.Count - 1
            mModelMonitorMod.ModelMonitorModPeriods(i).MonitorTypeID = mModelMonitorModTypeList(mModelMonitorMod.ModelMonitorModTypeID).MonitorTypeID
            If mModelMonitorModTypeList(mModelMonitorMod.ModelMonitorModTypeID).MonitorTypeID = 3 Then
                mModelMonitorMod.ModelMonitorModPeriods(i).FrequencyValue = CStr(0)
            End If
        Next
        dgPeriods.DataSource = mModelMonitorMod.ModelMonitorModPeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "Model Directive", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session.Remove("mMaintenanceTaskAndKit")
        Session.Remove("IsFromADSBConfig")
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
                mModelMonitorMod.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(mModelMonitorMod)
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
                mModelMonitorMod.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(mModelMonitorMod)
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
                mModelMonitorMod.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForMod(mModelMonitorMod)
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
            Session("MaintActivityID") = mModelMonitorMod.ID
            mLinkMaintenanceActionList = LinkMaintenanceActionList.GetLinkMaintActionList(True)
            Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList
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
        dgPeriods.DataSource = mModelMonitorMod.ModelMonitorModPeriods
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
        mModelMonitorMod.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mModelMonitorMod.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mModelMonitorMod.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mModelMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
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

        If mModelMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorMod.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mModelMonitorMod.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub dgMonitorModStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorModStatusList.RowCommand
        Dim AssemblyID As Guid
        Dim AssemblyStatusID As Guid
        Dim AsOnDate As String
        Dim HourType As Integer
        Select Case e.CommandName
            Case "Configure"
                AssemblyID = New Guid(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                AssemblyStatusID = New Guid(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                AsOnDate = dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(2).Text
                HourType = CInt(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewAssemblyMonitorModStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)
                mAssemblyMonitorModStatus.ModelMonitorModID(False) = mModelMonitorMod.ID
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                Session("IsOpenFromADSB") = "True"
                Session("RegNo") = dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                Response.Redirect("wfAssemblyMonitorModStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfNewADSB_Ajax.aspx")
            Case "EditRec"
                AssemblyStatusID = New Guid(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                HourType = CInt(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("IsOpenFromADSB") = "True"
                Session("RegNo") = dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                EditRecord(mADSBConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorModStatusID, AssemblyStatusID, HourType)
            Case "DeleteRec"
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                'mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex = index
                'Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
            Case "History"
                HistoryRecords(mADSBConfigurableList(CInt(e.CommandArgument)).MachineID, mADSBConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorModStatusID, mADSBConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mADSBConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorModStatusID)
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
    Private Sub dgMonitorModStatusList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorModStatusList.Sorting
        mADSBConfigurableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mADSBConfigurableList") = mADSBConfigurableList
        dgMonitorModStatusList.DataSource = mADSBConfigurableList
        dgMonitorModStatusList.DataBind()
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
        'Rpt = New crDetModelMonitorInsp
        'Dim ds As New dsCommon
        'Dim da As New CSLA.Data.ObjectAdapter
        'Dim ReportDetails As New rptStatusList

        ''For Current Value Grid
        'Dim TotalCount As Integer
        'Dim LHCount As Integer
        'Dim RHCount As Integer
        'LHCount = 9
        'RHCount = Me.mModelMonitorMod.ModelMonitorModPeriods.Count
        'If LHCount > RHCount Then
        '    TotalCount = LHCount
        'Else
        '    TotalCount = RHCount
        'End If

        'Dim temp As Integer
        'temp = 0
        'If temp < RHCount Then
        '    ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Code/Form No.", _
        '          txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '         dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        'Else
        '    ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Code/Form No.", _
        '                    txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                          "", ""))
        'End If
        'Dim I As Integer
        'For I = 0 To TotalCount - 1
        '    If I = 0 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "ATA Chapter", _
        '                    cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                  CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "ATA Chapter", _
        '                                           cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                                           "", ""))
        '        End If
        '    ElseIf I = 1 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", lblReference.Text, _
        '                     txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                   CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", lblReference.Text, _
        '                        txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                             "", ""))
        '        End If
        '    ElseIf I = 2 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Description", _
        '                            txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Description", _
        '                             txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                     "", ""))
        '        End If
        '    ElseIf I = 3 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Directive Type", _
        '                            cmbMonitorModType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '               CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Directive Type", _
        '                             cmbMonitorModType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                     "", ""))
        '        End If
        '    ElseIf I = 4 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Zone", _
        '                            txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                   CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Zone", _
        '                             txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                     "", ""))
        '        End If
        '    ElseIf I = 5 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Area", _
        '                            txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                   CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Area", _
        '                             txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                     "", ""))
        '        End If
        '    ElseIf I = 6 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Note", _
        '                            txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                   CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Note", _
        '                             txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                     "", ""))
        '        End If
        '    ElseIf I = 7 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Estd. Man Hours ", _
        '                            txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                   CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "Estd. Man Hours ", _
        '                             txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                     "", ""))
        '        End If
        '    ElseIf I = 8 Then
        '        If I < RHCount Then
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "", _
        '             "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '           CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '        Else
        '            ReportDetails.Add(New rptStatus(, 0, "Directive Details", "", _
        '                                 "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                     "", ""))
        '        End If
        '    Else
        '        ReportDetails.Add(New rptStatus(, 0, "Directive Details", "", _
        '                                 "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Directive", _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).PeriodUnitName, String), _
        '                    CType(Me.mModelMonitorMod.ModelMonitorModPeriods(I).FrequencyValue, String)))
        '    End If
        'Next

        'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, "Model Directive Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        'da.Fill(ds, ReportDetails)
        'da.Fill(ds, Report)
        'Dim mrptImage As rptImage = rptImage.GetImage(ds)
        'da.Fill(ds, mrptImage)
        'Rpt.SetDataSource(ds)
        'Session("CrystalReport") = Rpt
        'Dim Str1 As String
        'Str1 = "openTranDetail();"
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
#End Region

#End Region

End Class