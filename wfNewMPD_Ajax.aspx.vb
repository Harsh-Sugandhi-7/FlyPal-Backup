'Added by vikrant For MPD
Public Class wfNewMPD_Ajax
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
    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mModelMonitorInsp As ModelMonitorInsp
    Public Flag As Int16
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mModelMonitorInspTypeList As ModelMonitorInspTypeList
    Public mModelMonitorInspPeriodUnitList As ModelMonitorInspPeriodUnitList
    Dim str As String
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mIssueDate As String
    Public mAssemblyMonitorInspStatusList As tmpAssemblyMonitorInspStatusList
    Dim EventLogID As Guid 'Added by Vikrant on 28-July-2011
    Public mInspectionDetail As String
    Public mModel As String
    Public mMonitorInspectionType As String
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
    Private mUpdateComplyHistoryAssemblyMonitorInspStatusList As UpdateComplyHistoryAssemblyMonitorInspStatusList
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
        mAssemblyMonitorInspStatus = CType(Session("mAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)
        mModelMonitorInsp = CType(Session("mModelMonitorInsp"), ModelMonitorInsp)
        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorInspTypeList = CType(Session("mModelMonitorInspTypeList"), ModelMonitorInspTypeList)
        mModelMonitorInspPeriodUnitList = CType(Session("mModelMonitorInspPeriodUnitList"), ModelMonitorInspPeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mAssemblyMonitorInspStatusList = CType(Session("mAssemblyMonitorInspStatusList"), tmpAssemblyMonitorInspStatusList)
        mLinkMaintenanceActionList = Session("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        mLinkMaintenanceList = Session("mLinkMaintenanceList") 'End
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mAssemblyStatusModelWise = Session("mAssemblyStatusModelWise")
        ModelID = Session("ModelIDForMPD")
        mMPDConfigurableList = Session("mMPDConfigurableList")
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mModelMonitorInsp") = mModelMonitorInsp
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mATAList") = mATAList
        Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
        Session("mModelMonitorInspPeriodUnitList") = mModelMonitorInspPeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
        Session("mAssemblyMonitorInspStatusList") = mAssemblyMonitorInspStatusList
        Session("mLinkMaintenanceActionList") = mLinkMaintenanceActionList 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session("mLinkMaintenanceList") = mLinkMaintenanceList 'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorInspTypeList")
        Session.Remove("mModelMonitorInspPeriodUnitList")
        Session.Remove("mLinkMaintenanceActionList") 'Added By Utkarsh ON 09-Jan-2012 FOR Link Maintenance
        Session.Remove("mLinkMaintenanceList")
        Session.Remove("URL")
        Session.Remove("MaintenanceActivityID") 'End
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mAssemblyStatusModelWise")
        Session.Remove("mMPDConfigurableList")
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
        'mModelMonitorInsp.Code = Trim(txtCode.Text)
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If Trim(txtCode.Text).Length < 3 And Trim(txtCode.Text) <> "" Then
                mModelMonitorInsp.Code = Trim(txtCode.Text).PadLeft(3, "0"c)
            Else
                mModelMonitorInsp.Code = Trim(txtCode.Text)
            End If
        Else
            mModelMonitorInsp.Code = Trim(txtCode.Text)
        End If
        mModelMonitorInsp.Reference = Trim(txtReference.Text)
        mModelMonitorInsp.Description = Trim(txtDescription.Text)
        mModelMonitorInsp.Note = Trim(txtNote.Text)
        mModelMonitorInsp.ATAID = New Guid(cmbATAChapter.SelectedValue)
        mModelMonitorInsp.ModelMonitorInspTypeID = CType(Val(cmbMonitorInspType.SelectedValue), Int32)
        mModelMonitorInsp.ShowInCofA = chkShowInCofA.Checked
        mModelMonitorInsp.RequiredManHours = txtRequiredManHours.Text
        mModelMonitorInsp.Zone = Trim(txtZone.Text) 'Added by Saylee on 23-July-2013 for BA22072013 
        mModelMonitorInsp.Area = Trim(txtArea.Text)
        mModelMonitorInsp.IsRII = chkIsRII.Checked 'End
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorInsp.IsAttachmentAdded = True
            Else
                mModelMonitorInsp.IsAttachmentAdded = False
            End If
        End If
        Session("mModelMonitorInsp") = mModelMonitorInsp
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mModelMonitorInsp.ModelMonitorInspPeriods
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
        mModelMonitorInsp = Session("mModelMonitorInsp")
    End Sub
    Private Sub SetGrid()
        Dim B, C, D, IsReadOnly As Boolean

        For j As Integer = 0 To dgMonitorInspStatusList.Rows.Count - 1
            B = CType(Me.dgMonitorInspStatusList.Rows(j).Cells(17).Text, Boolean) 'IsConfigurable
            C = CType(Me.dgMonitorInspStatusList.Rows(j).Cells(21).Text, Boolean) 'IsMaster
            D = CType(Me.dgMonitorInspStatusList.Rows(j).Cells(23).Text, Boolean) 'IsAttachmentAdded
            IsReadOnly = CType(Me.dgMonitorInspStatusList.Rows.Item(j).Cells(24).Text, Boolean)

            dgMonitorInspStatusList.Rows(j).Cells(16).Enabled = IIf(IsReadOnly Or B = False, False, True) 'Configure
            dgMonitorInspStatusList.Rows(j).Cells(19).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Delete
            dgMonitorInspStatusList.Rows(j).Cells(18).Enabled = IIf(IsReadOnly Or B = True, False, True) 'Edit

            If B = False Then
                'dgMonitorInspStatusList.Rows(j).Cells(16).Enabled = False 'Configure
                btnAddPeriodUnit.Enabled = False
            Else
                'dgMonitorInspStatusList.Rows(j).Cells(19).Enabled = False 'Delete
                'dgMonitorInspStatusList.Rows(j).Cells(18).Enabled = False 'Edit
            End If
            If C = True Then
                dgMonitorInspStatusList.Rows(j).Cells(20).Enabled = False 'History
            End If
            If D = False Then
                dgMonitorInspStatusList.Rows(j).Cells(22).Enabled = False 'View
            End If

            If IsReadOnly Then
                Me.dgMonitorInspStatusList.Rows.Item(j).BackColor = Color.OrangeRed
                Me.dgMonitorInspStatusList.Rows.Item(j).ToolTip = "ReadOnly Aircraft"
                Me.dgMonitorInspStatusList.Rows.Item(j).ForeColor = Color.White
            End If
        Next
    End Sub
    Private Sub SetPage()
        If mModelMonitorInsp.IsNew Then
            lblTitle.Text = "Model Inspection of [ Model: " & mModelMonitorInsp.Model.Name & "] [New]"
        Else
            lblTitle.Text = "Model Inspection of [ Model: " & mModelMonitorInsp.Model.Name & "]"
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
        btnAddPeriodUnit.Enabled = mModelMonitorInspPeriodUnitList.Count > 0
        btnPrint.Enabled = Not mModelMonitorInsp.IsNew
        dgMonitorInspStatusList.Visible = IIf(CType(Session("IsFromMPDConfig"), Boolean) = True, False, Not mModelMonitorInsp.IsNew)
        lblResultInspList.Visible = IIf(CType(Session("IsFromMPDConfig"), Boolean) = True, False, Not mModelMonitorInsp.IsNew)
        lnkTools.Enabled = Not mModelMonitorInsp.IsNew
        lnkSpares.Enabled = Not mModelMonitorInsp.IsNew
        lnkTaskCards.Enabled = Not mModelMonitorInsp.IsNew
        lnkLinkMaintenance.Enabled = Not mModelMonitorInsp.IsNew

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorInsp.IsNew Then
            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList = Session("mModelMonitorInspConfiguredList")
            If Not mModelMonitorConfiguredList Is Nothing Then
                If mModelMonitorConfiguredList.Count > 0 Then
                    cmbMonitorInspType.Enabled = False
                Else
                    cmbMonitorInspType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mModelMonitorInsp.ModelMonitorInspPeriods
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
        If mModelMonitorInsp.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Function Save() As Boolean
        Dim mModelMonitorInspClone As ModelMonitorInsp
        mModelMonitorInspClone = CType(mModelMonitorInsp, ModelMonitorInsp)
        setObject()
        SetGridObject()
        If mModelMonitorInsp.IsValid = True Then
            If mModelMonitorInsp.ModelMonitorInspPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "Model Inspection cannot be saved without Period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mModelMonitorInsp.ApplyEdit()
                mModelMonitorInsp = CType(mModelMonitorInsp.Save(), ModelMonitorInsp)
                SaveAttachment()
                mInspectionDetail = "Model : " & mModel & " Model Inspection Type : " & mModelMonitorInsp.ModelMonitorInspTypeName & " Description : " & mModelMonitorInsp.Description
                MarkLog(Util.Action.Save, "Model Inspection", mInspectionDetail, Util.ErrorType.NoError, mModelMonitorInsp.ID, EventLogID)
                'end
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorInsp") = mModelMonitorInsp
                Return True
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                End If
                mModelMonitorInsp = mModelMonitorInspClone
                Session("mModelMonitorInsp") = mModelMonitorInsp
                Return False
            Finally
                mModelMonitorInspClone = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub AddSelectedPeroidUnits()
        Dim clnModelMonitorInsp As ModelMonitorInsp = mModelMonitorInsp.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mModelMonitorInsp.ModelMonitorInspPeriods.Add(mModelMonitorInsp.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, 1) 'HardFix mMachine.HourType = 1
                End If
            Next
            For i As Integer = 0 To mModelMonitorInsp.ModelMonitorInspPeriods.Count - 1
                mModelMonitorInsp.ModelMonitorInspPeriods(i).MonitorTypeID = mModelMonitorInspTypeList(mModelMonitorInsp.ModelMonitorInspTypeID).MonitorTypeID
                If mModelMonitorInspTypeList(mModelMonitorInsp.ModelMonitorInspTypeID).MonitorTypeID = 3 Then
                    mModelMonitorInsp.ModelMonitorInspPeriods(i).FrequencyValue = CStr(0)
                End If
            Next
            Session("mModelMonitorInsp") = mModelMonitorInsp
        Catch ex As Exception
            mModelMonitorInsp = clnModelMonitorInsp
            Session("mModelMonitorInsp") = mModelMonitorInsp
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnModelMonitorInsp = Nothing
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mModelMonitorInspPeriodUnitList.Count - 1
            If Not mModelMonitorInsp.ModelMonitorInspPeriods.Contains(mModelMonitorInspPeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mModelMonitorInspPeriodUnitList(i).ID, mModelMonitorInspPeriodUnitList(i).PeriodID, mModelMonitorInspPeriodUnitList(i).Name)
            End If
        Next
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub UpdatePanel()
        upnlMonitorInspectionDetails.Update()
        upnlATAMaster.Update()
        upnlMonitorInspectionType.Update()
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
                        IDForEventLog = mMPDConfigurableList(index).AssemblyMonitorInspStatusID
                        mMonitorInfo = mModelMonitorInsp.ModelMonitorInspTypeName
                        mMonitorType = mModelMonitorInsp.MonitorTypeName
                        mMonitorDesc = mModelMonitorInsp.Description
                        mAssemblyMonitorDetail = "Aircraft : " + mAircraft + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType + " Description : " + mMonitorDesc
                        'End
                        'Added by Saylee on 28-May-2009
                        mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfoForComplyDelete(mMPDConfigurableList(index).AssemblyMonitorInspStatusID)
                        '********************************
                        If mMPDConfigurableList(index).IsAttachmentAdded = True Then
                            mFileAttach = FileAttach.GetAttachment(mMPDConfigurableList(index).AssemblyMonitorInspStatusID)
                        End If
                        'Added by Saylee on 9th-Oct-2009
                        mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mMPDConfigurableList(index).AssemblyMonitorInspStatusID, 6)
                        '=============================

                        AssemblyMonitorInspStatus.DeleteAssemblyMonitorInspStatus(mMPDConfigurableList(index).AssemblyMonitorInspStatusID)
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
                            If LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorInsp.ID.ToString).Count > 0 Then
                                MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LinkMaintenance")
                                Exit Sub
                            End If
                        End If
                        'End
                        mMPDConfigurableList = MPDConfigurableList.GetMPDConfigurationList(ModelID, mModelMonitorInsp.ID.ToString, False, mMonitorInfo, mMonitorType, mMonitorDesc) 'tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
                        dgMonitorInspStatusList.DataSource = mMPDConfigurableList
                        dgMonitorInspStatusList.DataBind()
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
                            MarkLog(Util.Action.Delete, "AssemblyInspections", "Can't delete :" & mAssemblyMonitorDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) ' mEnquiry.ID)
                        End If
                        msgCount = ex.Errors.Count
                    Finally
                        If msgCount = 0 Then
                            MarkLog(Util.Action.Delete, "AssemblyInspections", mAssemblyMonitorDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID)
                        End If
                    End Try
                Case MsgBoxResult.No

                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "LinkMaintenance" Then
                        Session("sender") = ""
                        mMPDConfigurableList = MPDConfigurableList.GetMPDConfigurationList(ModelID, mModelMonitorInsp.ID.ToString, False, mMonitorInfo, mMonitorType, mMonitorDesc) 'tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
                        dgMonitorInspStatusList.DataSource = mMPDConfigurableList
                        dgMonitorInspStatusList.DataBind()
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
            If (User.IsInRole("MachineAssemblyInspectionPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mAssemblyStatusModelWise.IsMaster Then
            If (User.IsInRole("MachineAssemblyInspectionPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If

    End Sub '*******************************
    Private Sub SetToolsSparesCount()
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mModelMonitorInsp.ID)
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
                If (Not mModelMonitorInsp.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mModelMonitorInsp.ID)
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
        cmbMonitorInspType.DataSource = mModelMonitorInspTypeList
        ModelName = CType(Session("ModelName"), String)
        mAssemblyStatusModelWise = AssemblyStatusModelWise.GetAssemblyStatus(ModelID)
        Session("mAssemblyStatusModelWise") = mAssemblyStatusModelWise

        mModelMonitorInspPeriodUnitList = ModelMonitorInspPeriodUnitList.GetModelMonitorInspPeriodUnitList(mAssemblyStatusModelWise.ID, IsAllPeriodsofAllAssemblyRequired:=True, ModelID:=ModelID.ToString)
        Session("mModelMonitorInspPeriodUnitList") = mModelMonitorInspPeriodUnitList
        dgPeriods.DataSource = mModelMonitorInsp.ModelMonitorInspPeriods

        mMPDConfigurableList = MPDConfigurableList.GetMPDConfigurationList(ModelID:=ModelID, ModelMonitorInspID:=mModelMonitorInsp.ID.ToString, SkipNonConfiguredRecords:=False, MonitorInfo:=mModelMonitorInsp.ModelMonitorInspTypeName, MonitorType:=mModelMonitorInsp.MonitorTypeName, MonitorDesc:=mModelMonitorInsp.Description) 'tmpComplyAssemblyMonitorInspStatusList.GetDueMonitorInspList(Today.Date.ToString, Guid.Empty.ToString, ModelName, "", , , , , , , , False)
        dgMonitorInspStatusList.DataSource = mMPDConfigurableList
        Session("mMPDConfigurableList") = mMPDConfigurableList

        lblResultInspList.Text = "List of Configurations : " + mMPDConfigurableList.Count.ToString + " Record(s)"

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorInsp.IsNew Then
            Dim mModelMonitorInspConfiguredList As ModelMonitorConfiguredList
            mModelMonitorInspConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInsp.ModelID, mModelMonitorInsp.ID.ToString)
            Session("mModelMonitorInspConfiguredList") = mModelMonitorInspConfiguredList
        End If

        DataBind()
    End Sub
    Private Sub EditRecord(ByVal AssemblyMonitorInspStatusID As Guid, ByVal AssemblyStausID As Guid, ByVal HourType As Integer)
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(AssemblyMonitorInspStatusID, AssemblyStausID, HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("Edit") = True
        Response.Redirect("wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfNewMPD_Ajax.aspx")
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal AssemblyMonitorInspStatusID As Guid, ByVal AssemblyStatusID As Guid)
        Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        mMachine = Machine.GetMachine(MachineID)
        Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(AssemblyMonitorInspStatusID, AssemblyStatusID, mMachine.HourType)

        'If mPrevAssemblyMonitorInspStatus.IsMaster Then
        '    'MessageBox.Show("This is a master record and can not be edited from here", "Comply Component Monitor Inspection Status", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Dim msg As New SIMsgBox(Page, "Master Record!", "There is no history for this record", "", MsgBoxStyle.OKOnly)
        '    msg.ReplacePage = "wfComplyAssemblyMonitorInspStatusList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage")
        '    msg.Show()
        '    Exit Sub
        'Else
        mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
        Session("From") = 1 'Edit record
        ''
        'Dim mMachine As Machine = Machine.GetMachine(mTmpComplyAssemblyMonitorInspStatusList(Index).MachineID)
        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)

        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus

        mUpdateComplyHistoryAssemblyMonitorInspStatusList = UpdateComplyHistoryAssemblyMonitorInspStatusList.GetComplyHistoryAssemblyMonitorInspStatusList(mAssemblyStatus.AssemblyID, mAssemblyMonitorInspStatus.ModelMonitorInspID, mMachine.HourType)
        Session("mUpdateComplyHistoryAssemblyMonitorInspStatusList") = mUpdateComplyHistoryAssemblyMonitorInspStatusList

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspectionHistoryWindow", "OpenInspectionHistoryWindow();", True)
        'End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbATAChapter" Then
            If cmbATAChapter.SelectedIndex <= 0 Then
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "cmbMonitorInspType" Then
            If cmbMonitorInspType.SelectedIndex <= 0 Then
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
        If Not mModelMonitorInsp.IsValid Then
            For i As Integer = 0 To mModelMonitorInsp.GetBrokenRulesCollection.Count - 1
                str = str + mModelMonitorInsp.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Rows(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mModelMonitorInsp.ModelMonitorInspPeriods(i).IsValid Then
                For j As Integer = 0 To mModelMonitorInsp.ModelMonitorInspPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorInsp.ModelMonitorInspPeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
            If Not mModelMonitorInsp.ModelMonitorInspPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mModelMonitorInsp.ModelMonitorInspPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mModelMonitorInsp.ModelMonitorInspPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            mModelMonitorInspTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("(SELECT)")
            Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
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
                    If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                        Exit Sub
                    End If
                ElseIf Not mAssemblyStatusModelWise.IsMaster Then
                    If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                        Exit Sub
                    End If
                End If '*******************************

                'Added by saylee on 1-Jun-2016
                Dim mModelMonitorInspConfiguredList As ModelMonitorConfiguredList
                mModelMonitorInspConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInsp.ModelID, mModelMonitorInsp.ID.ToString)

                If mModelMonitorInspConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mModelMonitorInspConfiguredList.Count - 1
                        If i = mModelMonitorInspConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mModelMonitorInspConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mModelMonitorInspConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mModelMonitorInsp.ModelMonitorInspPeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If

                mModelMonitorInsp.ModelMonitorInspPeriods.Remove(mModelMonitorInsp.ModelMonitorInspPeriods.Item(Index).ID)
                Session("mModelMonitorInsp") = mModelMonitorInsp
                dgPeriods.DataSource = mModelMonitorInsp.ModelMonitorInspPeriods
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
        If Not mModelMonitorInsp.IsNew Then
            Dim mModelMonitorInspConfiguredList As ModelMonitorConfiguredList
            mModelMonitorInspConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInsp.ModelID, mModelMonitorInsp.ID.ToString)

            If mModelMonitorInspConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mModelMonitorInspConfiguredList.Count - 1
                    If i = mModelMonitorInspConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mModelMonitorInspConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mModelMonitorInspConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.show("Alert!", "MPD is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow();", True)
    End Sub
    Private Sub cmbMonitorInspType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorInspType.SelectedIndexChanged
        mModelMonitorInsp.ModelMonitorInspTypeID = CType(Val(cmbMonitorInspType.SelectedValue), Int32)
        'Added By Vikrant On 26-Nov-2018 For APFT26112018
        If AppSettings("SetModelCodeTypeWise") = "True" Then
            If cmbMonitorInspType.SelectedIndex > 0 Then
                Dim mMaxCodeMaintActTypewiseForModel As MaxCodeMaintActTypewiseForModel
                mMaxCodeMaintActTypewiseForModel = MaxCodeMaintActTypewiseForModel.GetCode(mModelMonitorInsp.ModelID, 6, CInt(cmbMonitorInspType.SelectedValue))
                If Int32.TryParse(mMaxCodeMaintActTypewiseForModel.Code, Nothing) Then
                    Dim TempCode As String = (CInt(mMaxCodeMaintActTypewiseForModel.Code) + 1).ToString
                    If TempCode.Length < 3 Then
                        mModelMonitorInsp.Code = TempCode.PadLeft(3, "0"c)
                    Else
                        mModelMonitorInsp.Code = TempCode
                    End If
                    txtCode.DataBind()
                Else
                    mModelMonitorInsp.Code = ""
                    txtCode.DataBind()
                End If
            Else
                mModelMonitorInsp.Code = ""
                txtCode.DataBind()
            End If
            upnlMonitorInspectionDetails.Update()
        End If
        'End
        For i As Integer = 0 To mModelMonitorInsp.ModelMonitorInspPeriods.Count - 1
            mModelMonitorInsp.ModelMonitorInspPeriods(i).MonitorTypeID = mModelMonitorInspTypeList(mModelMonitorInsp.ModelMonitorInspTypeID).MonitorTypeID
            If mModelMonitorInspTypeList(mModelMonitorInsp.ModelMonitorInspTypeID).MonitorTypeID = 3 Then
                mModelMonitorInsp.ModelMonitorInspPeriods(i).FrequencyValue = CStr(0)
            End If
        Next
        dgPeriods.DataSource = mModelMonitorInsp.ModelMonitorInspPeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "Model Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                mModelMonitorInsp.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForInsp(mModelMonitorInsp)
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
                mModelMonitorInsp.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForInsp(mModelMonitorInsp)
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
                mModelMonitorInsp.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            Else
                mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForInsp(mModelMonitorInsp)
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
            Session("MaintActivityID") = mModelMonitorInsp.ID
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
        dgPeriods.DataSource = mModelMonitorInsp.ModelMonitorInspPeriods
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
        mModelMonitorInsp.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mModelMonitorInsp.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorInsp.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mModelMonitorInsp.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mModelMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorInsp.ID)
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

        If mModelMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mModelMonitorInsp.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mModelMonitorInsp.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub dgMonitorInspStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorInspStatusList.RowCommand
        Dim AssemblyID As Guid
        Dim AssemblyStatusID As Guid
        Dim AsOnDate As String
        Dim HourType As Integer
        Select Case e.CommandName
            Case "Configure"
                AssemblyID = New Guid(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                AssemblyStatusID = New Guid(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                AsOnDate = dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(2).Text
                HourType = CInt(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, AssemblyID, AssemblyStatusID, Today.Date.ToString, ModelID, HourType)
                mAssemblyMonitorInspStatus.ModelMonitorInspID(False) = mModelMonitorInsp.ID
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                Session("IsOpenFromMPD") = "True"
                Session("RegNo") = dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                Response.Redirect("wfAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=wfNewMPD_Ajax.aspx")
            Case "EditRec"
                AssemblyStatusID = New Guid(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                HourType = CInt(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("IsOpenFromMPD") = "True"
                Session("RegNo") = dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                EditRecord(mMPDConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorInspStatusID, AssemblyStatusID, HourType)
            Case "DeleteRec"
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
                'mTmpComplyAssemblyMonitorInspStatusList.CurrentIndex = index
                'Session("mTmpComplyAssemblyMonitorInspStatusList") = mTmpComplyAssemblyMonitorInspStatusList
            Case "History"
                HistoryRecords(mMPDConfigurableList(CInt(e.CommandArgument)).MachineID, mMPDConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorInspStatusID, mMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mMPDConfigurableList(CInt(e.CommandArgument)).AssemblyMonitorInspStatusID)
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
    Private Sub dgMonitorInspStatusList_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMonitorInspStatusList.Sorting
        mMPDConfigurableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMPDConfigurableList") = mMPDConfigurableList
        dgMonitorInspStatusList.DataSource = mMPDConfigurableList
        dgMonitorInspStatusList.DataBind()
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
        Rpt = New crDetModelMonitorInsp
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 9
        RHCount = Me.mModelMonitorInsp.ModelMonitorInspPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Code/Form No.", _
                  txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                 dgPeriods.Columns.Item(0).HeaderText, dgPeriods.Columns.Item(1).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Code/Form No.", _
                            txtCode.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                                  "", ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "ATA Chapter", _
                            cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                          CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "ATA Chapter", _
                                                   cmbATAChapter.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                                                   "", ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", lblReference.Text, _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", lblReference.Text, _
                                txtReference.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                                     "", ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Description", _
                                     txtDescription.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Inspection Type", _
                                    cmbMonitorInspType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                       CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Inspection Type", _
                                     cmbMonitorInspType.SelectedItem.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Zone", _
                                    txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Zone", _
                                     txtZone.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Area", _
                                    txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Area", _
                                     txtArea.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 6 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Note", _
                                    txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Note", _
                                     txtNote.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 7 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Estd. Man Hours ", _
                                    txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                           CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "Estd. Man Hours ", _
                                     txtRequiredManHours.Text, , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            ElseIf I = 8 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "", _
                     "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                   CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                             "", ""))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Inspection Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Frequency of Monitoring Inspection", _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).PeriodUnitName, String), _
                            CType(Me.mModelMonitorInsp.ModelMonitorInspPeriods(I).FrequencyValue, String)))
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Model Inspection Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

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