'Added By Vikrant For MPD
Public Class wfNewCompMPD_Ajax
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
    Public mCompMonitorInspStatus As CompMonitorInspStatus
    Public mPartMonitorInsp As PartMonitorInsp
    Public Flag As Int16
    'For Combo
    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mATAList As ATAList
    Public mPartMonitorInspTypeList As PartMonitorInspTypeList
    Public mPartMonitorInspPeriodUnitList As PartMonitorInspPeriodUnitList
    Dim str As String
    Dim mMaintenanceTaskAndKit As MaintenanceTaskAndKit
    Public mIssueDate As String
    Dim EventLogID As Guid 'Added by Vikrant on 28-July-2011
    Public mInspectionDetail As String
    Public mModel As String
    Public mMonitorInspectionType As String
    Public mMonitorDesc As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim PartID As Guid
    Dim mCompStatusPartWise As CompStatusPartWise
    Private mCompMPDConfigurableList As CompMPDConfigurableList
    Private mUpdateComplyHistoryCompMonitorInspStatusList As UpdateComplyHistoryCompMonitorInspStatusList
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
        mCompMonitorInspStatus = CType(Session("mCompMonitorInspStatus"), CompMonitorInspStatus)
        mPartMonitorInsp = CType(Session("mPartMonitorInsp"), PartMonitorInsp)
        mATAList = CType(Session("mATAListForNewCompMPD"), ATAList)
        mPartMonitorInspTypeList = CType(Session("mPartMonitorInspTypeListForNewCompMPD"), PartMonitorInspTypeList)
        mPartMonitorInspPeriodUnitList = CType(Session("mPartMonitorInspPeriodUnitListForNewCompMPD"), PartMonitorInspPeriodUnitList)
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
        Session("mPartMonitorInsp") = mPartMonitorInsp
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mATAListForNewCompMPD") = mATAList
        Session("mPartMonitorInspTypeListForNewCompMPD") = mPartMonitorInspTypeList
        Session("mPartMonitorInspPeriodUnitListForNewCompMPD") = mPartMonitorInspPeriodUnitList
        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAListForNewCompMPD")
        Session.Remove("mPartMonitorInspTypeListForNewCompMPD")
        Session.Remove("mPartMonitorInspPeriodUnitListForNewCompMPD")
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
        mPartMonitorInsp.Code = Trim(txtCode.Text)
        mPartMonitorInsp.Reference = Trim(txtReference.Text)
        mPartMonitorInsp.Description = Trim(txtDescription.Text)
        mPartMonitorInsp.Note = Trim(txtNote.Text)
        mPartMonitorInsp.ATAID = New Guid(cmbATAChapter.SelectedValue.ToString)
        mPartMonitorInsp.PartMonitorInspTypeID = CType(Val(cmbMonitorInspType.SelectedValue), Int32)
        mPartMonitorInsp.ShowInCofA = chkShowInCofA.Checked
        mPartMonitorInsp.RequiredManHours = txtRequiredManHours.Text.Trim
        'Added by Saylee on 23-July-2013 for BA22072013 
        mPartMonitorInsp.Zone = Trim(txtZone.Text)
        mPartMonitorInsp.Area = Trim(txtArea.Text)
        mPartMonitorInsp.IsRII = chkIsRII.Checked
        'End

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mPartMonitorInsp.IsAttachmentAdded = True
            Else
                mPartMonitorInsp.IsAttachmentAdded = False
            End If
            'Else
            '    .IsAttachmentAdded = False
        End If

        Session("mPartMonitorInsp") = mPartMonitorInsp
    End Sub
    Public Sub SetGridObject()
        Dim txtFrequencyValue As TextBox
        With mPartMonitorInsp.PartMonitorInspPeriods
            Dim I As Integer
            For I = 0 To .Count - 1
                'Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriods.Rows(I).FindControl("txtFrequencyValue"), TextBox)
                'Setting the Object with the Values of the Controls
                .Item(I).FrequencyValue = Trim(txtFrequencyValue.Text)
            Next I
        End With
        Session("mPartMonitorInsp") = mPartMonitorInsp
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
        If mPartMonitorInsp.IsNew Then
            lblTitle.Text = "Part Inspection of [ Part: " & mPartMonitorInsp.Part.Name & "] [New]"
        Else
            lblTitle.Text = "Part Inspection of [ Part: " & mPartMonitorInsp.Part.Name & "]"
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
        btnAddPeriodUnit.Enabled = mPartMonitorInspPeriodUnitList.Count > 0
        btnPrint.Enabled = Not mPartMonitorInsp.IsNew
        dgMonitorInspStatusList.Visible = IIf(CType(Session("IsFromMPDConfig"), Boolean) = True, False, Not mPartMonitorInsp.IsNew)
        lblResultInspList.Visible = IIf(CType(Session("IsFromMPDConfig"), Boolean) = True, False, Not mPartMonitorInsp.IsNew)
        lnkTools.Enabled = Not mPartMonitorInsp.IsNew
        lnkSpares.Enabled = Not mPartMonitorInsp.IsNew
        lnkTaskCards.Enabled = Not mPartMonitorInsp.IsNew

        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorInsp.IsNew Then
            Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList = Session("mPartMonitorInspConfiguredList")
            If Not mPartMonitorInspConfiguredList Is Nothing Then
                If mPartMonitorInspConfiguredList.Count > 0 Then
                    cmbMonitorInspType.Enabled = False
                Else
                    cmbMonitorInspType.Enabled = True
                End If

                Dim txtFrequencyValue As TextBox
                With mPartMonitorInsp.PartMonitorInspPeriods
                    For i As Integer = 0 To .Count - 1
                        'Geting the Controls from the DataGrid
                        txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
                        'Setting the Object with the Values of the Controls
                        If mPartMonitorInspConfiguredList.Count > 0 Then
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
        If mPartMonitorInsp.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Function Save() As Boolean
        Dim mPartMonitorInspClone As PartMonitorInsp
        mPartMonitorInspClone = CType(mPartMonitorInsp, PartMonitorInsp)
        setObject()
        SetGridObject()
        If mPartMonitorInsp.IsValid = True Then
            If mPartMonitorInsp.PartMonitorInspPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodUnitRequired, MSGBox.Message_text.PeriodUnitRequired, "You are trying to save Part Inspection.Part Inspection can not be saved without period units", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                mPartMonitorInsp.ApplyEdit()
                mPartMonitorInsp = CType(mPartMonitorInsp.Save(), PartMonitorInsp)
                SaveAttachment()
                'Commented By Utkarsh On 27-Jul-2011 For All19072011
                '     MarkLog(Util.Action.Save, "PartMonitorSer", "ATAChapter->" + mPartMonitorInsp.ATAChapter + " -> " + " Part Name -> " + mPartMonitorInsp.Part.Name + " Part Monitor Insp Type Name -> " + mPartMonitorInsp.PartMonitorInspTypeName, Util.ErrorType.NoError, mPartMonitorInsp.ID)
                'End
                Session("mPartMonitorInsp") = mPartMonitorInsp
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
                mPartMonitorInsp = mPartMonitorInspClone
                Session("mPartMonitorInsp") = mPartMonitorInsp
                Return False
            Finally
                'Added By Utkarsh On 26-Jul-2011 For All19072011
                'MaintDetail = "Monitor Insp Type : " + mPartMonitorInsp.PartMonitorInspTypeName + " Description : " + mPartMonitorInsp.Description
                mInspectionDetail = "Part : " & mPartMonitorInsp.Part.Name & " Part Inspection Type : " & mPartMonitorInsp.PartMonitorInspTypeName & " Description : " & mPartMonitorInsp.Description
                MarkLog(Util.Action.Save, "Part Inspection", mInspectionDetail, Util.ErrorType.NoError, mPartMonitorInsp.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub AddSelectedPeroidUnits()
        Dim clnPartMonitorInsp As PartMonitorInsp = mPartMonitorInsp.Clone
        Try
            Dim mSelectPeriodUnit As SelectPeriodUnit
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected Then
                    mPartMonitorInsp.PartMonitorInspPeriods.Add(mPartMonitorInsp.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, 1) 'HardFix mMachine.HourType = 1
                    mPartMonitorInsp.SetZeroFrequencyValue()
                End If
            Next
            Session("mPartMonitorInsp") = mPartMonitorInsp
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mPartMonitorInsp = clnPartMonitorInsp
            Session("mPartMonitorInsp") = mPartMonitorInsp
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnPartMonitorInsp = Nothing
            mSelectPeriodUnits = Nothing
            Session.Remove("mSelectPeriodUnits")
        End Try
    End Sub
    Private Sub SetPeroidUnits()
        Dim mSelectPeriodUnits As SelectPeriodUnits
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
        For i As Integer = 0 To mPartMonitorInspPeriodUnitList.Count - 1
            If Not mPartMonitorInsp.PartMonitorInspPeriods.Contains(mPartMonitorInspPeriodUnitList(i).ID) Then
                mSelectPeriodUnits.Add(mPartMonitorInspPeriodUnitList(i).ID, mPartMonitorInspPeriodUnitList(i).PeriodID, mPartMonitorInspPeriodUnitList(i).Name)
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
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Session("sender") = ""
                            Dim index As Integer = Session("Index")
                            IDForEventLog = mCompMPDConfigurableList(index).CompMonitorInspStatusID
                            mInspectionDetail = "Reg No. : " + mCompMPDConfigurableList(index).RegNo & " Assembly Info : " & mCompMPDConfigurableList(index).AssemblyInfo & " Part Info : " & mCompMPDConfigurableList(index).PartSerialNo & " Monitor Info : " & mCompMPDConfigurableList(index).MonitorInfo

                            'Added by Saylee on 9th-Oct-2009
                            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMPDConfigurableList(index).CompMonitorInspStatusID, 9)

                            '=============================
                            If mCompMPDConfigurableList(index).IsAttachmentAdded = True Then
                                mFileAttach = FileAttach.GetAttachment(mCompMPDConfigurableList(index).CompMonitorInspStatusID)
                            End If
                            CompMonitorInspStatus.DeleteCompMonitorInspStatus(mCompMPDConfigurableList(index).CompMonitorInspStatusID)
                            MachineMaintenance.DeleteMachineMaintenance(mMachineMaintenance.ID)
                            If Not mFileAttach Is Nothing Then
                                If mFileAttach.Size > 0 Then
                                    FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
                                End If
                            End If
                            Session("mMachineMaintenance") = mMachineMaintenance
                            mCompMPDConfigurableList = CompMPDConfigurableList.GetMPDConfigurationList(PartID:=PartID, PartMonitorInspID:=mPartMonitorInsp.ID.ToString, SkipNonConfiguredRecords:=False, ModelID:=mPartMonitorInsp.ModelID.ToString)
                            dgMonitorInspStatusList.DataSource = mCompMPDConfigurableList
                            dgMonitorInspStatusList.DataBind()
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
                                MarkLog(Util.Action.Delete, "ComponentInspections", "Can't delete : " & mInspectionDetail & " is Currently in use", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'mEnquiry.ID)'Added By Utkarsh On 28-Jul-2011 For All19072011
                                'End
                            End If
                            'DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "ComponentInspections", mInspectionDetail, Util.ErrorType.NoError, IDForEventLog, EventLogID) 'Added By Utkarsh On 28-Jul-2011 For All19072011
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
            If (User.IsInRole("MachineAssemblyInspectionPrint")) = False Then
                btnPrint.Enabled = False
                btnPrint.ToolTip = "You are not authorized user"
            End If

            If (User.IsInRole("MachineAssemblyInspectionNew") Or User.IsInRole("MachineAssemblyInspectionEdit")) = False Then
                btnSave.Enabled = False
                btnSave.ToolTip = "You are not authorized user"
            End If
        ElseIf Not mCompStatusPartWise.IsMaster Then
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
        Dim mMaintenanceKitDetailsCount As MaintenanceKitDetailsCount = MaintenanceKitDetailsCount.GetMaintenanceKitDetailsCount(mPartMonitorInsp.ID)
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
                If (Not mPartMonitorInsp.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mPartMonitorInsp.ID)
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
        cmbMonitorInspType.DataSource = mPartMonitorInspTypeList
        mCompStatusPartWise = CompStatusPartWise.GetCompStatus(PartID) 'fetch comp status of any one comp for getting periods
        Session("mCompStatusPartWise") = mCompStatusPartWise

        mPartMonitorInspPeriodUnitList = PartMonitorInspPeriodUnitList.GetPartMonitorInspPeriodUnitList(mCompStatusPartWise.ID, IsAllPeriodsofAllCompsRequired:=True, PartID:=PartID.ToString)
        Session("mPartMonitorInspPeriodUnitListForNewCompMPD") = mPartMonitorInspPeriodUnitList
        dgPeriods.DataSource = mPartMonitorInsp.PartMonitorInspPeriods

        mCompMPDConfigurableList = CompMPDConfigurableList.GetMPDConfigurationList(PartID:=PartID, PartMonitorInspID:=mPartMonitorInsp.ID.ToString, SkipNonConfiguredRecords:=False, ModelID:=mPartMonitorInsp.ModelID.ToString)
        dgMonitorInspStatusList.DataSource = mCompMPDConfigurableList
        Session("mCompMPDConfigurableList") = mCompMPDConfigurableList

        lblResultInspList.Text = "List of Configurations : " + mCompMPDConfigurableList.Count.ToString + " Record(s)"


        'Added by saylee on 1-Jun-2016
        If Not mPartMonitorInsp.IsNew Then
            Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList
            mPartMonitorInspConfiguredList = PartMonitorConfiguredList.GetPartMonitorInspConfiguredList(mPartMonitorInsp.PartID, mPartMonitorInsp.ID.ToString)
            Session("mPartMonitorInspConfiguredList") = mPartMonitorInspConfiguredList
        End If
        DataBind()
    End Sub
    Private Sub EditRecord(ByVal CompMonitorInspStatusID As Guid, ByVal CompStausID As Guid, ByVal AssemblyStausID As Guid, ByVal HourType As Integer)
        Dim mCompMonitorInspStatus As CompMonitorInspStatus
        mCompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(CompMonitorInspStatusID, AssemblyStausID, CompStausID, HourType)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Response.Redirect("wfCompMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=wfNewCompMPD_Ajax.aspx")
    End Sub
    Private Sub HistoryRecords(ByVal MachineID As Guid, ByVal CompMonitorInspStatusID As Guid, ByVal AssemblyStatusID As Guid, ByVal CompStatusID As Guid, ByVal DoneOn As String)
        mMachine = Machine.GetMachine(MachineID)
        Dim mCompMonitorInspStatus As CompMonitorInspStatus
        Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(CompMonitorInspStatusID, AssemblyStatusID, CompStatusID, mMachine.HourType)

        mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatusFromEntry(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
        Session("EnFrom") = 1 'EditRecord

        Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(AssemblyStatusID)
        Dim mCompStatus As CompStatus
        mCompStatus = CompStatus.GetCompStatus(CompStatusID, AssemblyStatusID, DoneOn) 'CHK For DoneOn Date
        Session("mMachine") = mMachine
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mCompStatus") = mCompStatus

        mUpdateComplyHistoryCompMonitorInspStatusList = UpdateComplyHistoryCompMonitorInspStatusList.GetComplyHistoryCompMonitorInspStatusList(mCompStatus.CompID, mCompMonitorInspStatus.PartMonitorInspID, mMachine.HourType)
        Session("mUpdateComplyHistoryCompMonitorInspStatusList") = mUpdateComplyHistoryCompMonitorInspStatusList
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspectionHistoryWindow", "OpenInspectionHistoryWindow();", True)
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
        If Not mPartMonitorInsp.IsValid Then
            For i As Integer = 0 To mPartMonitorInsp.GetBrokenRulesCollection.Count - 1
                str = str + mPartMonitorInsp.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgPeriods.Rows.Count - 1)
            'tem = dgPeriods.Items(i)
            txtFrequencyValue = CType(Me.dgPeriods.Rows(i).FindControl("txtFrequencyValue"), TextBox)
            If Not mPartMonitorInsp.PartMonitorInspPeriods(i).IsValid Then
                For j As Integer = 0 To mPartMonitorInsp.PartMonitorInspPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorInsp.PartMonitorInspPeriods.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
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
            If Not mPartMonitorInsp.PartMonitorInspPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mPartMonitorInsp.PartMonitorInspPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mPartMonitorInsp.PartMonitorInspPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            mPartMonitorInspTypeList = PartMonitorInspTypeList.GetPartMonitorInspTypeList("(SELECT)")
            Session("mPartMonitorInspTypeListForNewCompMPD") = mPartMonitorInspTypeList
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
                Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList
                mPartMonitorInspConfiguredList = PartMonitorConfiguredList.GetPartMonitorInspConfiguredList(mPartMonitorInsp.PartID, mPartMonitorInsp.ID.ToString)

                If mPartMonitorInspConfiguredList.Count > 0 Then
                    Dim SerialNos As String = String.Empty

                    For i As Integer = 0 To mPartMonitorInspConfiguredList.Count - 1
                        If i = mPartMonitorInspConfiguredList.Count - 1 Then
                            SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo
                        Else
                            SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo + ","
                        End If
                    Next

                    MSGBoxCtrl.show("Remove Alert!", "Selected " + mPartMonitorInsp.PartMonitorInspPeriods.Item(Index).PeriodUnitName + " frequency is configured on Component(s) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                    Exit Select
                End If
                mPartMonitorInsp.PartMonitorInspPeriods.Remove(mPartMonitorInsp.PartMonitorInspPeriods.Item(Index).ID)
                Session("mPartMonitorInsp") = mPartMonitorInsp
                dgPeriods.DataSource = mPartMonitorInsp.PartMonitorInspPeriods
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
        If Not mPartMonitorInsp.IsNew Then
            Dim mPartMonitorInspConfiguredList As PartMonitorConfiguredList
            mPartMonitorInspConfiguredList = PartMonitorConfiguredList.GetPartMonitorInspConfiguredList(mPartMonitorInsp.PartID, mPartMonitorInsp.ID.ToString)

            If mPartMonitorInspConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mPartMonitorInspConfiguredList.Count - 1
                    If i = mPartMonitorInspConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mPartMonitorInspConfiguredList(i).SerialNo + ","
                    End If
                Next

                MSGBoxCtrl.show("Alert!", "Inspection is already configured on Component(s) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow();", True)
    End Sub
    Private Sub cmbMonitorInspType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonitorInspType.SelectedIndexChanged
        mPartMonitorInsp.PartMonitorInspTypeID = CType(Val(cmbMonitorInspType.SelectedValue), Int32)
        dgPeriods.DataSource = mPartMonitorInsp.PartMonitorInspPeriods
        dgPeriods.DataBind()
        upnlPeriods.Update()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "Part Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                mPartMonitorInsp.MaintenanceToolID = mMaintenanceTaskAndKit.MaintenanceToolID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompInsp(mPartMonitorInsp)

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
                mPartMonitorInsp.MaintenanceKitID = mMaintenanceTaskAndKit.MaintenanceKitID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompInsp(mPartMonitorInsp)

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
                mPartMonitorInsp.MaintenanceTaskID = mMaintenanceTaskAndKit.MaintenanceTaskID
            End If

            mMaintenanceTaskAndKit = MaintenanceTaskAndKit.GetMaintenanceTaskAndKitDetailForCompInsp(mPartMonitorInsp)
            
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
        dgPeriods.DataSource = mPartMonitorInsp.PartMonitorInspPeriods
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
        mPartMonitorInsp.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mPartMonitorInsp.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorInsp.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mPartMonitorInsp.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mPartMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorInsp.ID)
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

        If mPartMonitorInsp.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mPartMonitorInsp.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mPartMonitorInsp.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub dgMonitorInspStatusList_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMonitorInspStatusList.RowCommand
        Dim CompID As Guid
        Dim CompStatusID As Guid
        Dim HourType As Integer
        Select Case e.CommandName
            Case "Configure"
                CompID = New Guid(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                CompStatusID = New Guid(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                HourType = CInt(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(3).Text)
                Dim mMachine As Machine = Machine.GetMachine(mCompMPDConfigurableList(CInt(e.CommandArgument)).MachineID)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString)
                mCompMonitorInspStatus = CompMonitorInspStatus.NewCompMonitorInspStatus(Guid.NewGuid, mCompStatus.CompID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString, mCompStatus.Comp.PartID, mCompMPDConfigurableList(CInt(e.CommandArgument)).ModelID, CompStatusID, HourType)
                mCompMonitorInspStatus.PartMonitorInspID(True) = mPartMonitorInsp.ID
                Session("mCompStatus") = mCompStatus
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                Session("mMachine") = mMachine
                Session("IsOpenFromMPD") = "True"
                Session("RegNo") = dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                Response.Redirect("wfCompMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage4=wfNewCompMPD_Ajax.aspx")
            Case "EditRec"
                CompStatusID = New Guid(dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                Dim mAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID)
                Dim mCompStatus As CompStatus = CompStatus.GetCompStatus(CompStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, Today.Date.ToString)
                Dim mMachine As Machine = Machine.GetMachine(mCompMPDConfigurableList(CInt(e.CommandArgument)).MachineID)
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mCompStatus") = mCompStatus
                Session("IsOpenFromMPD") = "True"
                Session("mMachine") = mMachine
                Session("RegNo") = dgMonitorInspStatusList.Rows(CInt(e.CommandArgument)).Cells(5).Text.ToString
                EditRecord(mCompMPDConfigurableList(CInt(e.CommandArgument)).CompMonitorInspStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).CompStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).HourType)
            Case "DeleteRec"
                Session("Index") = CInt(e.CommandArgument)
                MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "Delete Component Inspection Status.", MsgBoxStyle.YesNo, "Delete")
            Case "History"
                HistoryRecords(mCompMPDConfigurableList(CInt(e.CommandArgument)).MachineID, mCompMPDConfigurableList(CInt(e.CommandArgument)).CompMonitorInspStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).AssemblyStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).CompStatusID, mCompMPDConfigurableList(CInt(e.CommandArgument)).DoneOnFormatted.ToString)
            Case "ViewRec"
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttach = FileAttach.GetAttachment(mCompMPDConfigurableList(CInt(e.CommandArgument)).CompMonitorInspStatusID)
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
        mCompMPDConfigurableList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCompMPDConfigurableList") = mCompMPDConfigurableList
        dgMonitorInspStatusList.DataSource = mCompMPDConfigurableList
        dgMonitorInspStatusList.DataBind()
        SetGrid()
    End Sub
#End Region
End Class