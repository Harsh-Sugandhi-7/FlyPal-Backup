'Added By Vikrant For ADSBConfig
Public Class wfNewCompADSB_Ajax
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
    Public mCompMonitorModStatus As CompMonitorModStatus
    Public mPartMonitorMod As PartMonitorMod
    Public Flag As Int16
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mPartMonitorModTypeList As PartMonitorModTypeList
    Public mPartMonitorModPeriodUnitList As PartMonitorModPeriodUnitList  'PartMonitorModPeriodUnitList
    Dim str As String
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mIssueDate As String
    Dim EventLogID As Guid 'Added by Vikrant on 28-July-2011
    Public mDirectiveDetail As String
    Public mModel As String
    Public mMonitorDesc As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim PartID As Guid
    Dim mCompStatusPartWise As CompStatusPartWise
    Private mCompADSBConfigurableList As CompADSBConfigurableList
    Private mUpdateComplyHistoryCompMonitorModStatusList As UpdateComplyHistoryCompMonitorModStatusList
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
        mCompMonitorModStatus = CType(Session("mCompMonitorModStatus"), CompMonitorModStatus)
        mPartMonitorMod = CType(Session("mPartMonitorMod"), PartMonitorMod)
        mATAList = CType(Session("mATAListForNewCompADSB"), ATAList)
        mPartMonitorModTypeList = CType(Session("mPartMonitorModTypeListForNewCompADSB"), PartMonitorModTypeList)
        mPartMonitorModPeriodUnitList = CType(Session("mPartMonitorModPeriodUnitListForNewCompADSB"), PartMonitorModPeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        mMaintenanceTaskAndKit = CType(Session("mMaintenanceTaskAndKit"), MaintenanceTaskAndKit)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeletedForNewCompADSB")
        mCompStatusPartWise = Session("mCompStatusPartWise")
        PartID = Session("PartIDForNewCompADSB")
        mCompADSBConfigurableList = Session("mCompADSBConfigurableList")
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mPartMonitorMod") = mPartMonitorMod
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mATAListForNewCompADSB") = mATAList
        Session("mPartMonitorModTypeListForNewCompADSB") = mPartMonitorModTypeList
        Session("mPartMonitorModPeriodUnitListForNewCompADSB") = mPartMonitorModPeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAListForNewCompADSB")
        Session.Remove("mPartMonitorModTypeListForNewCompADSB")
        Session.Remove("mPartMonitorModPeriodUnitListForNewCompADSB")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeletedForNewCompADSB")
        Session.Remove("mCompStatusPartWise")
        Session.Remove("mCompADSBConfigurableList")
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
        mPartMonitorMod.Code = Trim(txtCode.Text)
        mPartMonitorMod.Reference = Trim(txtReference.Text)
        mPartMonitorMod.Description = Trim(txtDescription.Text)
        mPartMonitorMod.Note = Trim(txtNote.Text)
        mPartMonitorMod.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mPartMonitorMod.PartMonitorModTypeID = CType(Val(cmbMonitorModType.SelectedValue), Int32)
        mPartMonitorMod.ShowInCofA = chkShowInCofA.Checked
        mPartMonitorMod.RequiredManHours = txtRequiredManHours.Text.Trim
        'Added by Saylee on 23-July-2013 for BA22072013 
        mPartMonitorMod.Zone = Trim(txtZone.Text)
        mPartMonitorMod.Area = Trim(txtArea.Text)
        mPartMonitorMod.IsRII = chkIsRII.Checked
        mPartMonitorMod.Number = txtModificationNo.Text
        'End
        If (calIssueDate.Text <> "") Then
            mPartMonitorMod.IssueDate = calIssueDate.Text.ToString
        Else
            mPartMonitorMod.IssueDate = System.DBNull.Value
        End If
        mPartMonitorMod.IsApplicable = chkApplicable.Checked
        mPartMonitorMod.Applicability = Trim(txtApplicability.Text)
        mPartMonitorMod.ComplianceRequirement = Trim(txtComplianceRequirement.Text)

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mPartMonitorMod.IsAttachmentAdded = True
            Else
                mPartMonitorMod.IsAttachmentAdded = False
            End If
            'Else
            '    .IsAttachmentAdded = False
        End If

        Session("mPartMonitorMod") = mPartMonitorMod
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mPartMonitorMod.PartMonitorModPeriods
            Dim I As Integer
            For I = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(I).FindControl("txtFrequencyValue"), TextBox)
                'Setting the Object with the Values of the Controls
                .Item(I).FrequencyValue = Trim(txtFrequencyValue.Text)
            Next I
        End With
        Session("mPartMonitorMod") = mPartMonitorMod
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
                'dgMonitorModStatusList.Rows(j).Cells(16).Enabled = False 'Configure
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
        If mPartMonitorMod.IsNew Then
            lblTitle.Text = "Part Modification of [ Part: " & mPartMonitorMod.Part.Name & "] [New]"
        Else
            lblTitle.Text = "Part Modification of [ Part: " & mPartMonitorMod.Part.Name & "]"
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
        btnAddPeriodUnit.Enabled = mPartMonitorModPeriodUnitList.Count > 0
        btnPrint.Enabled = Not mPartMonitorMod.IsNew
        dgMonitorModStatusList.Visible = IIf(CType(Session("IsFromADSBConfig"), Boolean) = True, False, Not mPartMonitorMod.IsNew)
        lblResultModList.Visible = IIf(CType(Session("IsFromADSBConfig"), Boolean) = True, False, Not mPartMonitorMod.IsNew)
        lnkTools.Enabled = Not mPartMonitorMod.IsNew
        lnkSpares.Enabled = Not mPartMonitorMod.IsNew
        lnkTaskCards.Enabled = Not mPartMonitorMod.IsNew

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorMod.IsNew Then
            Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList = Session("mPartMonitorModConfiguredList")
            If Not mPartMonitorModConfiguredList Is Nothing Then
                If mPartMonitorModConfiguredList.Count > 0 Then
                    cmbMonitorModType.Enabled = False
                Else
                    cmbMonitorModType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mPartMonitorMod.PartMonitorModPeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mPartMonitorModConfiguredList.Count > 0 Then
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
        If mPartMonitorMod.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Function Save() As Boolean
        Dim mPartMonitorModClone As PartMonitorMod
        mPartMonitorModClone = CType(mPartMonitorMod, PartMonitorMod)
        setObject()
        SetGridObject()
        If mPartMonitorMod.IsValid = True Then
            If mPartMonitorMod.PartMonitorModPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Part Modification.Part Modification can not be saved without period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mPartMonitorMod.ApplyEdit()
                mPartMonitorMod = CType(mPartMonitorMod.Save(), PartMonitorMod)
                SaveAttachment()
                'Commented By Utkarsh On 27-Jul-2011 For All19072011
                '     MarkLog(Util.Action.Save, "PartMonitorSer", "ATAChapter->" + mPartMonitorMod.ATAChapter + " -> " + " Part Name -> " + mPartMonitorMod.Part.Name + " Part Monitor Insp Type Name -> " + mPartMonitorMod.PartMonitorInspTypeName, Util.ErrorType.NoError, mPartMonitorMod.ID)
                'End
                Session("mPartMonitorMod") = mPartMonitorMod
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
                mPartMonitorMod = mPartMonitorModClone
                Session("mPartMonitorMod") = mPartMonitorMod
                Return False
            Finally
                'Added By Utkarsh On 26-Jul-2011 For All19072011
                'MaintDetail = "Monitor Insp Type : " + mPartMonitorMod.PartMonitorInspTypeName + " Description : " + mPartMonitorMod.Description
                mDirectiveDetail = "Part : " & mPartMonitorMod.Part.Name & " Part Modification Type : " & mPartMonitorMod.PartMonitorModTypeName & " Description : " & mPartMonitorMod.Description & " Mod No. : " & mPartMonitorMod.Number
                MarkLog(Util.Action.Save, "Part Modification", mDirectiveDetail, Util.ErrorType.NoError, mPartMonitorMod.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub AddSelectedPeroidUnits()
        Dim clnPartMonitorMod As PartMonitorMod = mPartMonitorMod.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mPartMonitorMod.PartMonitorModPeriods.Add(mPartMonitorMod.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, 1) 'HardFix mMachine.HourType = 1
                    mPartMonitorMod.SetZeroFrequencyValue()
                End If
            Next
            Session("mPartMonitorMod") = mPartMonitorMod
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mPartMonitorMod = clnPartMonitorMod
            Session("mPartMonitorMod") = mPartMonitorMod
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnPartMonitorMod = Nothing
            mSelectPeriodUnits = Nothing
            Session.Remove("mSelectPeriodUnits")
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mPartMonitorModPeriodUnitList.Count - 1
            If Not mPartMonitorMod.PartMonitorModPeriods.Contains(mPartMonitorModPeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mPartMonitorModPeriodUnitList(i).ID, mPartMonitorModPeriodUnitList(i).PeriodID, mPartMonitorModPeriodUnitList(i).Name)
            End If
        Next
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub UpdatePanel()
        upnlMonitorModificationDetails.Update()
        upnlATAMaster.Update()
        upnlMonitorModificationType.Update()
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
                            IDForEventLog = mCompADSBConfigurableList(index).ID
                            mDirectiveDetail = "Reg No. : " + mCompADSBConfigurableList(index).RegNo & " Assembly Info : " & mCompADSBConfigurableList(index).AssemblyInfo & " Part Info : " & mCompADSBConfigurableList(index).PartSerialNo & " Monitor Info : " & mCompADSBConfigurableList(index).MonitorInfo

                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompADSBConfigurableList(index).ID, 10)

                            '=============================
                            If mCompADSBConfigurableList(index).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mCompADSBConfigurableList(index).ID)
                            End If
                            CompMonitorModStatus.DeleteCompMonitorModStatus(mCompADSBConfigurableList(index).ID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            Session("mMachineMaintenance") = mMachineMaintenance
                            mCompADSBConfigurableList = CompADSBConfigurableList.GetADSBConfigurationList(PartID:=PartID, PartMonitorModID:=mPartMonitorMod.ID.ToString, SkipNonConfiguredRecords:=False, ModelID:=mPartMonitorMod.ModelID.ToString)
                            dgMonitorModStatusList.DataSource = mCompADSBConfigurableList
                            dgMonitorModStatusList.DataBind()
                            Session("mCompADSBConfigurableList") = mCompADSBConfigurableList
                            SetGrid()
                            upnlAssemblyDetails.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "ComponentModifications", "Can't delete : " & mDirectiveDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'mEnquiry.ID)'Added By Utkarsh On 28-Jul-2011 For All19072011
                                'End
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "ComponentModifications", mDirectiveDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID) 'Added By Utkarsh On 28-Jul-2011 For All19072011
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
            If (User.IsInRole("MachineComponentModificationPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineComponentModificationNew") Or User.IsInRole("MachineComponentModificationEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mCompStatusPartWise.IsMaster Then
            If (User.IsInRole("MachineComponentModificationPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineComponentModificationNew") Or User.IsInRole("MachineComponentModificationEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        End If

    End Sub '*******************************
    Private Sub SetToolsSparesCount()
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorMod.ID)
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
                If (Not mPartMonitorMod.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mPartMonitorMod.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeletedForNewCompADSB") = IsAttachmentDeleted
            End If
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mATAList = ATAList.GetATAList(, "(SELECT)")
        Session("mATAListForNewCompADSB") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbMonitorModType.DataSource = mPartMonitorModTypeList
        mCompStatusPartWise = CompStatusPartWise.GetCompStatus(PartID) 'fetch comp status of any one comp for getting periods
        Session("mCompStatusPartWise") = mCompStatusPartWise

        mPartMonitorModPeriodUnitList = PartMonitorModPeriodUnitList.GetPartMonitorModPeriodUnitList(mCompStatusPartWise.ID, IsAllPeriodsofAllCompsRequired:=True, PartID:=PartID.ToString)
        Session("mPartMonitorModPeriodUnitListForNewCompADSB") = mPartMonitorModPeriodUnitList
        dgPeriods.DataSource = mPartMonitorMod.PartMonitorModPeriods

        mCompADSBConfigurableList = CompADSBConfigurableList.GetADSBConfigurationList(PartID:=PartID, PartMonitorModID:=mPartMonitorMod.ID.ToString, SkipNonConfiguredRecords:=False, ModelID:=mPartMonitorMod.ModelID.ToString)
        dgMonitorModStatusList.DataSource = mCompADSBConfigurableList
        Session("mCompADSBConfigurableList") = mCompADSBConfigurableList

        lblResultModList.Text = "List of Configurations : " + mCompADSBConfigurableList.Count.ToString + " Record(s)"


        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorMod.IsNew Then
            Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList
            mPartMonitorModConfiguredList = PartMonitorConfiguredList.GetPartMonitorModConfiguredList(mPartMonitorMod.PartID, mPartMonitorMod.ID.ToString)
            Session("mPartMonitorModConfiguredList") = mPartMonitorModConfiguredList
        End If
        DataBind()
        calIssueDate.Text = mPartMonitorMod.IssueDateFormatted.ToString
    End Sub
    Private Sub EditRecord(ByVal CompMonitorModStatusID As Guid, ByVal CompStausID As Guid, ByVal AssemblyStausID As Guid, ByVal HourType As Integer)
        Dim mCompMonitorModStatus As CompMonitorModStatus
        mCompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(CompMonitorModStatusID, AssemblyStausID, CompStausID, HourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfNewCompADSB_Ajax.aspx")
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal CompMonitorModStatusID As Guid, ByVal AssemblyStatusID As Guid, ByVal CompStatusID As Guid, ByVal DoneOn As String)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorModStatus As CompMonitorModStatus
        Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(CompMonitorModStatusID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mMachine.HourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
        Session("EnFrom") = 1 'EditRecord

        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn) 'CHK For DoneOn Date
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus

        mUpdateComplyHistoryCompMonitorModStatusList = UpdateComplyHistoryCompMonitorModStatusList.GetComplyHistoryCompMonitorModStatusList(mCompStatus.CompID, mCompMonitorModStatus.PartMonitorModID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorModStatusList") = mUpdateComplyHistoryCompMonitorModStatusList
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModificationHistoryWindow", "OpenModificationHistoryWindow();", True)
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
        If Not mPartMonitorMod.IsValid Then
            For i As Integer = 0 To mPartMonitorMod.GetBrokenRulesCollection.Count - 1
                str = str + mPartMonitorMod.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Items(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mPartMonitorMod.PartMonitorModPeriods(i).IsValid Then
                For j As Integer = 0 To mPartMonitorMod.PartMonitorModPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorMod.PartMonitorModPeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
            If Not mPartMonitorMod.PartMonitorModPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mPartMonitorMod.PartMonitorModPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorMod.PartMonitorModPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            mPartMonitorModTypeList = PartMonitorModTypeList.GetPartMonitorModTypeList("(SELECT)")
            Session("mPartMonitorModTypeListForNewCompADSB") = mPartMonitorModTypeList
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
                Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList
                mPartMonitorModConfiguredList = PartMonitorConfiguredList.GetPartMonitorModConfiguredList(mPartMonitorMod.PartID, mPartMonitorMod.ID.ToString)

                If mPartMonitorModConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mPartMonitorModConfiguredList.Count - 1
                        If i = mPartMonitorModConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mPartMonitorMod.PartMonitorModPeriods.Item(Index).PeriodUnitName + " frequency is configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If
                mPartMonitorMod.PartMonitorModPeriods.Remove(mPartMonitorMod.PartMonitorModPeriods.Item(Index).ID)
                Session("mPartMonitorMod") = mPartMonitorMod
                dgPeriods.DataSource = mPartMonitorMod.PartMonitorModPeriods
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
        If Not mPartMonitorMod.IsNew Then
            Dim mPartMonitorModConfiguredList As PartMonitorConfiguredList
            mPartMonitorModConfiguredList = PartMonitorConfiguredList.GetPartMonitorModConfiguredList(mPartMonitorMod.PartID, mPartMonitorMod.ID.ToString)

            If mPartMonitorModConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mPartMonitorModConfiguredList.Count - 1
                    If i = mPartMonitorModConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mPartMonitorModConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.show("Alert!", "Modification is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow();", True)
    End Sub
    Private Sub cmbMonitorModType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorModType.SelectedIndexChanged
        mPartMonitorMod.PartMonitorModTypeID = CType(Val(cmbMonitorModType.SelectedValue), Int32)
        dgPeriods.DataSource = mPartMonitorMod.PartMonitorModPeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "Part Modification", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                mPartMonitorMod.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompMod(mPartMonitorMod)

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
                mPartMonitorMod.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompMod(mPartMonitorMod)

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
                mPartMonitorMod.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompMod(mPartMonitorMod)

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
        dgPeriods.DataSource = mPartMonitorMod.PartMonitorModPeriods
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
        mPartMonitorMod.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mPartMonitorMod.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorMod.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPartMonitorMod.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mPartMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorMod.ID)
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

        If mPartMonitorMod.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorMod.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mPartMonitorMod.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub dgMonitorModStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorModStatusList.RowCommand
        Dim CompID As Guid
        Dim CompStatusID As Guid
        Dim HourType As Integer
        Select Case e.CommandName
            Case "Configure"
                CompID = New Guid(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                CompStatusID = New Guid(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                HourType = CInt(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mMachine As Machine = Machine.GetMachine(mCompADSBConfigurableList(CInt(e.CommandArgument)).MachineID)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompADSBConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, mCompADSBConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString)
                mCompMonitorModStatus = CompMonitorModStatus.NewCompMonitorModStatus(Guid.NewGuid, mCompStatus.CompID, mCompADSBConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString, mCompStatus.Comp.PartID, mCompADSBConfigurableList(CInt(e.CommandArgument)).ModelID, CompStatusID, HourType)
                mCompMonitorModStatus.PartMonitorModID(True) = mPartMonitorMod.ID
                Session("mCompStatus") = mCompStatus
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                Session("mMachine") = mMachine
                Session("IsOpenFromADSB") = "True"
                Session("RegNo") = dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                Response.Redirect("wfCompMonitorModStatus_AJAX.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage4=wfNewCompADSB_Ajax.aspx")
            Case "EditRec"
                CompStatusID = New Guid(dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompADSBConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, mCompADSBConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString)
                Dim mMachine As Machine = Machine.GetMachine(mCompADSBConfigurableList(CInt(e.CommandArgument)).MachineID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompStatus") = mCompStatus
                Session("IsOpenFromADSB") = "True"
                Session("mMachine") = mMachine
                Session("RegNo") = dgMonitorModStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                EditRecord(mCompADSBConfigurableList(CInt(e.CommandArgument)).ID, mCompADSBConfigurableList(CInt(e.CommandArgument)).CompStatusID, mCompADSBConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, mCompADSBConfigurableList(CInt(e.CommandArgument)).HourType)
            Case "DeleteRec"
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "Delete Component Modification Status.", MsgBoxStyle.YesNo, "Delete")
            Case "History"
                HistoryRecords(mCompADSBConfigurableList(CInt(e.CommandArgument)).MachineID, mCompADSBConfigurableList(CInt(e.CommandArgument)).ID, mCompADSBConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, mCompADSBConfigurableList(CInt(e.CommandArgument)).CompStatusID, mCompADSBConfigurableList(CInt(e.CommandArgument)).DoneOnFormatted.ToString)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mCompADSBConfigurableList(CInt(e.CommandArgument)).ID)
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
        mCompADSBConfigurableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompADSBConfigurableList") = mCompADSBConfigurableList
        dgMonitorModStatusList.DataSource = mCompADSBConfigurableList
        dgMonitorModStatusList.DataBind()
        SetGrid()
    End Sub
#End Region
End Class