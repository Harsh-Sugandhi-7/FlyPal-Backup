Public Class wfAMPDetail
    Inherits System.Web.UI.Page

#Region " ENUM "

    Private Enum LinkAction
        MakeApplicable = 1
        MakeApplicableAndStart = 2
        MakeNotApplicable = 3
        Comply = 4
        DoNothing = 5
    End Enum
#End Region


#Region " Variable Declaration "
    Public mAssemblyMonitorServiceStatusThreshold As AssemblyMonitorServiceStatus
    Public mModelMonitorServiceThreshold As ModelMonitorService
    ' Public mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
    Public mAssemblyMonitorServiceStatusInterval As AssemblyMonitorServiceStatus
    Public mModelMonitorServiceInterval As ModelMonitorService

    Public mMPDMaster As MPDMaster
    Public mServiceTypeList As ServiceTypeList
    Public mATAList As ATAList

    Public mSelectPeriodUnits As SelectPeriodUnits
    Public mModelMonitorServiceTypeList As ModelMonitorServiceTypeList
    Public mModelMonitorServicePeriodUnitList As ModelMonitorServicePeriodUnitList

    Dim Flag As Int16
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Dim EventLogID As Guid

    Public mUnit As String
    Public mModel As String
    Public mMonitorType As String
    Public mDescription As String
    Public mDetail As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mModuleList As ModuleList

    Dim mMPDTypeList As MPDTypeList
    Dim mMPDSkillList As MPDSkillList

    Dim mLastMPDRef As LastMPDAMPRef
    Dim RegNo As String
    Dim LicenseNoThreshold As String = String.Empty
    Dim LicenseNoInterval As String = String.Empty

    Dim EmpNameThreshold As String = String.Empty
    Dim EmpNameInterval As String = String.Empty

    Dim DoneByIDThreshold As Guid = Guid.Empty
    Dim DoneByIDInterval As Guid = Guid.Empty
    Dim mFromEditThresholdInterval As String = ""
    Dim AirframeCurrentValues As String = ""

    Public mAssemblyMonitorServiceStatusNA As AssemblyMonitorServiceStatus
    Public mModelMonitorServiceNA As ModelMonitorService
    Dim mLinkMaintenanceList As LinkMaintenanceList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyMonitorServiceStatusThreshold = CType(Session("mAssemblyMonitorServiceStatusThreshold"), AssemblyMonitorServiceStatus)
        mModelMonitorServiceThreshold = CType(Session("mModelMonitorServiceThreshold"), ModelMonitorService)
        mMachine = CType(Session("mMachine"), Machine)

        mAssemblyMonitorServiceStatusInterval = CType(Session("mAssemblyMonitorServiceStatusInterval"), AssemblyMonitorServiceStatus)
        mModelMonitorServiceInterval = CType(Session("mModelMonitorServiceInterval"), ModelMonitorService)

        mATAList = CType(Session("mATAList"), ATAList)
        mModelMonitorServiceTypeList = CType(Session("mModelMonitorServiceTypeList"), ModelMonitorServiceTypeList)
        mModelMonitorServicePeriodUnitList = CType(Session("mModelMonitorServicePeriodUnitList"), ModelMonitorServicePeriodUnitList)
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)

        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mModuleList = Session("mModuleList")
        mLastMPDRef = Session("mLastMPDRef")
        mMPDMaster = Session("mMPDMaster")

        RegNo = Session("RegNo")
        mAssemblyStatus = Session("mAssemblyStatus")
        mFromEditThresholdInterval = Session("FromEditThresholdInterval")

        mModelMonitorServiceNA = Session("mModelMonitorServiceNA")
        mAssemblyMonitorServiceStatusNA = Session("mAssemblyMonitorServiceStatusNA")
        AirframeCurrentValues = Session("AirframeCurrentValues")
        mLinkMaintenanceList = Session("mLinkMaintenanceList")
    End Sub
    Private Sub SetSession()
        Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
        Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval

        Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
        Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold

        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine

        Session("mATAList") = mATAList
        Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList
        Session("mModelMonitorServicePeriodUnitList") = mModelMonitorServicePeriodUnitList

        Session("mSelectPeriodUnits") = mSelectPeriodUnits

        Session("mLastMPDRef") = mLastMPDRef
        Session("RegNo") = RegNo
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("FromEditThresholdInterval") = mFromEditThresholdInterval
        Session("AirframeCurrentValues") = AirframeCurrentValues
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mATAList")
        Session.Remove("mModelMonitorServiceTypeList")
        Session.Remove("mSelectPeriodUnits")
        Session.Remove("URL")
        Session.Remove("MaintenanceActivityID")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")

        Session.Remove("mLastMPDRef")
        Session.Remove("RegNo")
        Session.Remove("mAssemblyStatus")
        Session.Remove("mAssemblyMonitorServiceStatusInterval")
        Session.Remove("mModelMonitorServiceInterval")

        Session.Remove("mAssemblyMonitorServiceStatusThreshold")
        Session.Remove("mModelMonitorServiceThreshold")
        Session.Remove("FromEditThresholdInterval")
        Session.Remove("AirframeCurrentValues")
        Session.Remove("mLinkMaintenanceList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtReference" Then
            If Len(txtReference.Text) > 500 Then
                custValidator.ErrorMessage = "Reference Too Long"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 1000 Then
                custValidator.ErrorMessage = "Note can't be more than 1000 chars."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        End If
    End Sub
    Private Sub SetObjectThreshold()
        'If AppSettings("SetModelCodeTypeWise") = "True" Then
        '    If Trim(txtCode.Text).Length < 3 And Trim(txtCode.Text) <> "" Then
        '        mModelMonitorServiceThreshold.Code = Trim(txtCode.Text).PadLeft(3, "0"c)
        '    Else
        '        mModelMonitorServiceThreshold.Code = Trim(txtCode.Text)
        '    End If


        'Else
        '    mModelMonitorServiceThreshold.Code = Trim(txtCode.Text)

        'End If

        mModelMonitorServiceThreshold.ATAID = mMPDMaster.ATAID
        mModelMonitorServiceThreshold.Reference = Trim(txtReference.Text)
        mModelMonitorServiceThreshold.Description = Trim(txtDescription.Text)
        '  mModelMonitorServiceThreshold.ModelMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue.ToString), Int32)
        mModelMonitorServiceThreshold.Note = Trim(txtNote.Text)
        '  mModelMonitorServiceThreshold.ShowInCofA = chkShowInCofA.Checked
        ' mModelMonitorServiceThreshold.RequiredManHours = txtRequiredManHours.Text.Trim
        mModelMonitorServiceThreshold.Zone = mMPDMaster.Zone
        '  mModelMonitorServiceThreshold.Area = Trim(txtArea.Text)
        ' mModelMonitorServiceThreshold.IsRII = chkIsRII.Checked 'End
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorServiceThreshold.IsAttachmentAdded = True
            Else
                mModelMonitorServiceThreshold.IsAttachmentAdded = False
            End If
        End If


        mModelMonitorServiceThreshold.TaskCardNo = txtAMPNo.Text.Trim

        mModelMonitorServiceThreshold.Applicability = txtApplicability.Text.Trim
        ' mModelMonitorServiceThreshold.Source = txtSource.Text.Trim
        mModelMonitorServiceThreshold.Access = mMPDMaster.Access.Trim
        mModelMonitorServiceThreshold.MPDSkillID = mMPDMaster.MPDSkillID
        mModelMonitorServiceThreshold.MPDTypeID = mMPDMaster.MPDTypeID

        '  mModelMonitorServiceThreshold.AccessOpenCloseManHours = txtAccessManHours.Text.Trim
        ''********************

        Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold


    End Sub
    Private Sub SetPeriodUnitsThreshold()
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits()
        Dim i As Int32
        Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
        'While i <= mPeriodUnitList.Count - 1
        '    If mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Contains(mPeriodUnitList(i).ID) = False Then
        '        mSelectPeriodUnits.Add(mPeriodUnitList(i).ID, mPeriodUnitList(i).PeriodID, mPeriodUnitList(i).PeriodUnitName)
        '    End If
        '    i = i + 1
        'End While

        While i <= mModelMonitorServicePeriodUnitList.Count - 1
            If mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Contains(mModelMonitorServicePeriodUnitList(i).ID) = False Then
                mSelectPeriodUnits.Add(mModelMonitorServicePeriodUnitList(i).ID, mModelMonitorServicePeriodUnitList(i).PeriodID, mModelMonitorServicePeriodUnitList(i).Name)
            End If
            i = i + 1
        End While

        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Public Sub SetGridObjectThreshold()
        Dim txtFrequencyValue As TextBox
        With mModelMonitorServiceThreshold.ModelMonitorServicePeriods
            For i As Integer = 0 To .Count - 1
                REM: Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriodsThreshold.Rows(i).FindControl("txtFrequencyValueThreshold"), TextBox)
                REM:Setting the Object with the Values of the Controls
                If .Item(i).PeriodID = 2 And Decimal.MaxValue <= Val(txtFrequencyValue.Text.Trim) Then    'Hours 
                    .Item(i).FrequencyValue = ""
                Else
                    .Item(i).FrequencyValue = Trim(txtFrequencyValue.Text)
                End If
            Next i
        End With
        Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold


    End Sub
    Private Sub SetObjectInterval()
        'If AppSettings("SetModelCodeTypeWise") = "True" Then
        '    If Trim(txtCode.Text).Length < 3 And Trim(txtCode.Text) <> "" Then
        '        mModelMonitorServiceInterval.Code = Trim(txtCode.Text).PadLeft(3, "0"c)
        '    Else
        '        mModelMonitorServiceInterval.Code = Trim(txtCode.Text)
        '    End If


        'Else
        '    mModelMonitorServiceInterval.Code = Trim(txtCode.Text)

        'End If

        mModelMonitorServiceInterval.ATAID = mMPDMaster.ATAID
        mModelMonitorServiceInterval.Reference = Trim(txtReference.Text)
        mModelMonitorServiceInterval.Description = Trim(txtDescription.Text)
        '  mModelMonitorServiceInterval.ModelMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue.ToString), Int32)
        mModelMonitorServiceInterval.Note = Trim(txtNote.Text)
        '  mModelMonitorServiceInterval.ShowInCofA = chkShowInCofA.Checked
        ' mModelMonitorServiceInterval.RequiredManHours = txtRequiredManHours.Text.Trim
        mModelMonitorServiceInterval.Zone = mMPDMaster.Zone
        '  mModelMonitorServiceInterval.Area = Trim(txtArea.Text)
        ' mModelMonitorServiceInterval.IsRII = chkIsRII.Checked 'End
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorServiceInterval.IsAttachmentAdded = True
            Else
                mModelMonitorServiceInterval.IsAttachmentAdded = False
            End If
        End If


        mModelMonitorServiceInterval.TaskCardNo = txtAMPNo.Text.Trim

        mModelMonitorServiceInterval.Applicability = txtApplicability.Text.Trim
        ' mModelMonitorServiceInterval.Source = txtSource.Text.Trim
        mModelMonitorServiceInterval.Access = mMPDMaster.Access.Trim
        mModelMonitorServiceInterval.MPDSkillID = mMPDMaster.MPDSkillID
        mModelMonitorServiceInterval.MPDTypeID = mMPDMaster.MPDTypeID

        '  mModelMonitorServiceInterval.AccessOpenCloseManHours = txtAccessManHours.Text.Trim
        ''********************

        Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval


    End Sub
    Private Sub SetPeriodUnitsInterval()
        mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits()
        Dim i As Int32
        Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
        'While i <= mPeriodUnitList.Count - 1
        '    If mModelMonitorServiceInterval.ModelMonitorServicePeriods.Contains(mPeriodUnitList(i).ID) = False Then
        '        mSelectPeriodUnits.Add(mPeriodUnitList(i).ID, mPeriodUnitList(i).PeriodID, mPeriodUnitList(i).PeriodUnitName)
        '    End If
        '    i = i + 1
        'End While
        While i <= mModelMonitorServicePeriodUnitList.Count - 1
            If mModelMonitorServiceInterval.ModelMonitorServicePeriods.Contains(mModelMonitorServicePeriodUnitList(i).ID) = False Then
                mSelectPeriodUnits.Add(mModelMonitorServicePeriodUnitList(i).ID, mModelMonitorServicePeriodUnitList(i).PeriodID, mModelMonitorServicePeriodUnitList(i).Name)
            End If
            i = i + 1
        End While


        Session("mSelectPeriodUnits") = mSelectPeriodUnits
    End Sub
    Public Sub SetGridObjectInterval()
        Dim txtFrequencyValue As TextBox
        With mModelMonitorServiceInterval.ModelMonitorServicePeriods
            For i As Integer = 0 To .Count - 1
                REM: Geting the Controls from the DataGrid
                txtFrequencyValue = CType(Me.dgPeriodsInterval.Rows(i).FindControl("txtFrequencyValueInterval"), TextBox)
                REM:Setting the Object with the Values of the Controls
                If .Item(i).PeriodID = 2 And Decimal.MaxValue <= Val(txtFrequencyValue.Text.Trim) Then    'Hours 
                    .Item(i).FrequencyValue = ""
                Else
                    .Item(i).FrequencyValue = Trim(txtFrequencyValue.Text)
                End If
            Next i
        End With
        Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval


    End Sub
    Public Sub DataFieldBind()
        mServiceTypeList = ServiceTypeList.GetServiceTypeList(True)
        cmbType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        mModelMonitorServicePeriodUnitList = ModelMonitorServicePeriodUnitList.GetModelMonitorServicePeriodUnitList(mAssemblyStatus.ID)         'mModel.ID)
        Session("mModelMonitorServicePeriodUnitList") = mModelMonitorServicePeriodUnitList

        dgPeriodsThreshold.DataSource = mModelMonitorServiceThreshold.ModelMonitorServicePeriods
        dgPeriodsInterval.DataSource = mModelMonitorServiceInterval.ModelMonitorServicePeriods

        dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
        dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods

        BindLicenceNoThreshold()
        BindLicenceNoInterval()
        DataBind()


        If mAssemblyMonitorServiceStatusThreshold.DoneOn.ToString <> "" Then
            txtDoneOnDateThreshold.Text = CDate(mAssemblyMonitorServiceStatusThreshold.DoneOn).ToString(AppSettings("DateFormat"))
        End If
        If mAssemblyMonitorServiceStatusInterval.DoneOn.ToString <> "" Then
            txtDoneOnDateInterval.Text = CDate(mAssemblyMonitorServiceStatusInterval.DoneOn).ToString(AppSettings("DateFormat"))
        End If

        If txtDoneOnDateThreshold.Text <> "" Then
            phThresholdDoneDetails.Visible = True
        Else
            phThresholdDoneDetails.Visible = False
        End If

        If txtDoneOnDateInterval.Text <> "" Then
            phIntervalDoneDetails.Visible = True
        Else
            phIntervalDoneDetails.Visible = False
        End If

        If Session("FromEditThresholdInterval") = "True" Then
            txtAMPNo.Enabled = False
            btnAddPeriodUnitInterval.Enabled = False
            btnAddPeriodUnitThreshold.Enabled = False
            dgPeriodsThreshold.Columns(2).Visible = False
            dgPeriodsInterval.Columns(2).Visible = False
            If Not mAssemblyMonitorServiceStatusThreshold.IsNew Then
                pnlThreshold.Enabled = True
                chkIsThreshold.Checked = True

                If mAssemblyMonitorServiceStatusThreshold.DoneOnFormatted.ToString <> "" Then
                    phThresholdDoneDetails.Visible = True
                    rdbIsComplianceThresholdYes.Checked = True
                    upnlIsComplianceThreshold.Update()
                Else
                    rdbIsComplianceThresholdNo.Checked = True
                End If

                txtAMPNo.Text = mModelMonitorServiceThreshold.TaskCardNo
                txtReference.Text = mModelMonitorServiceThreshold.Reference
                txtNote.Text = mModelMonitorServiceThreshold.Note

                If mLinkMaintenanceList Is Nothing Then
                    mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorServiceThreshold.ID.ToString)
                    Session("mLinkMaintenanceList") = mLinkMaintenanceList

                End If

                If mLinkMaintenanceList.Count = 1 Then

                    Try
                        If mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.MakeApplicable Then
                            rdbMakeApplicable.Checked = True
                        ElseIf mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.MakeApplicableAndStart Then
                            rdbMakeApplicableAndStart.Checked = True
                        ElseIf mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.MakeNotApplicable Then
                            rdbMakeNotApplicable.Checked = True
                        ElseIf mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.Comply Then
                            rdbComply.Checked = True
                        ElseIf mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.DoNothing Then
                            rdbDoNothing.Checked = True
                        End If
                    Catch ex As SqlException
                        If ex.Number = 8145 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 2627 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                        ElseIf ex.Number = 547 Then
                            MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                        End If

                    End Try
                End If


            Else
                pnlThreshold.Enabled = False
                chkIsThreshold.Checked = False
                rdbIsComplianceThresholdNo.Checked = True
            End If

            If Not mAssemblyMonitorServiceStatusInterval.IsNew Then
                pnlInterval.Enabled = True
                chkIsInterval.Checked = True
                If mAssemblyMonitorServiceStatusInterval.DoneOnFormatted.ToString <> "" Then
                    phIntervalDoneDetails.Visible = True
                    rdbIsComplianceIntervalYes.Checked = True
                    upnlIsComplianceInterval.Update()
                Else
                    rdbIsComplianceIntervalNo.Checked = True
                End If

                If mAssemblyMonitorServiceStatusInterval.DoneOnFormatted.ToString = "" Then
                    phNAStart.Visible = True
                End If

                txtAMPNo.Text = mModelMonitorServiceInterval.TaskCardNo
                txtReference.Text = mModelMonitorServiceInterval.Reference
                txtNote.Text = mModelMonitorServiceInterval.Note
            Else
                pnlInterval.Enabled = False
                chkIsInterval.Checked = False
                rdbIsComplianceIntervalNo.Checked = True
            End If

            If Session("MonitorTypeID") = "3" Then
                chkIsApplicable.Checked = False
                txtAMPNo.Text = mModelMonitorServiceNA.TaskCardNo
                txtReference.Text = mModelMonitorServiceNA.Reference
                txtNote.Text = mModelMonitorServiceNA.Note
            Else
                chkIsApplicable.Checked = True
            End If


            chkIsApplicable.Enabled = False

        Else
            chkIsApplicable.Checked = False
            chkIsApplicable.Enabled = True
            phThresholdDoneDetails.Visible = False
            txtAMPNo.Enabled = True
            rdbIsComplianceThresholdNo.Checked = True
            rdbIsComplianceThresholdYes.Checked = False
            rdbIsComplianceIntervalNo.Checked = True
            rdbIsComplianceIntervalYes.Checked = False

        End If
        If chkIsApplicable.Checked Then
            phCompliance.Visible = True
            phLine.Visible = True
        Else
            phCompliance.Visible = False
            phLine.Visible = False
        End If
        If rdbIsComplianceIntervalYes.Checked Then
            phNAStart.Visible = False
        Else
            phNAStart.Visible = True
        End If

    End Sub
    Private Sub AddSelectedPeriodUnitsThreshold(DoneOnDate As String)
        Dim clnModelMonitorServiceThreshold = mModelMonitorServiceThreshold.Clone
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        Try
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit As SelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected = True Then
                    If Not mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Contains(mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID) Then
                        mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Add(mModelMonitorServiceThreshold.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
                        mModelMonitorServiceThreshold.ModelMonitorServicePeriods.CurrentItem.MonitorTypeID = mModelMonitorServiceThreshold.MonitorTypeID
                        Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod = AssemblyMonitorServiceStatusPeriod.NewAssemblyMonitorServiceStatusPeriod(mAssemblyMonitorServiceStatusThreshold.ID,
                                                                                                                                                                                 mModelMonitorServiceThreshold.ModelMonitorServicePeriods.CurrentItem.ID,
                                                                                                                                                                                 mAssemblyStatus.ID, mSelectPeriodUnit.PeriodID, mSelectPeriodUnit.PeriodUnitID, 0, DoneOnDate.ToString)
                        mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Add(mAssemblyMonitorServiceStatusPeriod)

                    End If

                End If
            Next
            For i As Integer = 0 To mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Count - 1
                mModelMonitorServiceThreshold.ModelMonitorServicePeriods(i).MonitorTypeID = mModelMonitorServiceTypeList(mModelMonitorServiceThreshold.ModelMonitorServiceTypeID).MonitorTypeID
                If mModelMonitorServiceTypeList(mModelMonitorServiceThreshold.ModelMonitorServiceTypeID).MonitorTypeID = 3 Then        'this is for No Frequency
                    mModelMonitorServiceThreshold.ModelMonitorServicePeriods(i).FrequencyValue = CStr(0)
                End If
            Next
            Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold
            Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
        Catch ex As Exception
            mModelMonitorServiceThreshold = clnModelMonitorServiceThreshold
            Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.Show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnModelMonitorServiceThreshold = Nothing

        End Try
    End Sub


    Private Sub AddSelectedPeriodUnitsInterval(DoneOnDate As String)
        Dim clnModelMonitorServiceInterval = mModelMonitorServiceInterval.Clone
        'Added by Saylee on 10-Feb-2020,  All27072020
        Dim mHourType As Integer = 0
        If mAssemblyStatus.IsSpareAssembly = True Then
            mHourType = mAssemblyStatus.HourType
        Else
            mHourType = mMachine.HourType
        End If
        '*********************
        Try
            If IsNothing(mSelectPeriodUnits) Then
                mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
            End If
            For Each mSelectPeriodUnit As SelectPeriodUnit In mSelectPeriodUnits
                If mSelectPeriodUnit.IsSelected = True Then
                    If Not mModelMonitorServiceInterval.ModelMonitorServicePeriods.Contains(mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID) Then
                        mModelMonitorServiceInterval.ModelMonitorServicePeriods.Add(mModelMonitorServiceInterval.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
                        mModelMonitorServiceInterval.ModelMonitorServicePeriods.CurrentItem.MonitorTypeID = mModelMonitorServiceInterval.MonitorTypeID
                        Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod = AssemblyMonitorServiceStatusPeriod.NewAssemblyMonitorServiceStatusPeriod(mAssemblyMonitorServiceStatusInterval.ID,
                                                                                                                                                                            mModelMonitorServiceInterval.ModelMonitorServicePeriods.CurrentItem.ID,
                                                                                                                                                                            mAssemblyStatus.ID, mSelectPeriodUnit.PeriodID, mSelectPeriodUnit.PeriodUnitID, 0, DoneOnDate.ToString)
                        mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Add(mAssemblyMonitorServiceStatusPeriod)
                    End If

                End If
            Next
            For i As Integer = 0 To mModelMonitorServiceInterval.ModelMonitorServicePeriods.Count - 1
                mModelMonitorServiceInterval.ModelMonitorServicePeriods(i).MonitorTypeID = mModelMonitorServiceTypeList(mModelMonitorServiceInterval.ModelMonitorServiceTypeID).MonitorTypeID
                If mModelMonitorServiceTypeList(mModelMonitorServiceInterval.ModelMonitorServiceTypeID).MonitorTypeID = 3 Then        'this is for No Frequency
                    mModelMonitorServiceInterval.ModelMonitorServicePeriods(i).FrequencyValue = CStr(0)
                End If
            Next
            Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval
            Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        Catch ex As Exception
            mModelMonitorServiceInterval = clnModelMonitorServiceInterval
            Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval
            If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
                MSGBoxCtrl.Show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
            End If
        Finally
            clnModelMonitorServiceInterval = Nothing
            Session.Remove("mSelectPeriodUnits")
            mSelectPeriodUnits = Nothing
        End Try
    End Sub
    Public Sub AddPeriodUnitsInterval()
        If mModelMonitorServiceInterval.ModelMonitorServicePeriods.Count > 0 And chkIsInterval.Checked Then
            For i As Integer = 0 To mModelMonitorServiceInterval.ModelMonitorServicePeriods.Count - 1
                Dim PeriodID As Integer = mModelMonitorServiceInterval.ModelMonitorServicePeriods(i).PeriodID
                Dim PeriodUnitID As Integer = mModelMonitorServiceInterval.ModelMonitorServicePeriods(i).PeriodUnitID

                If Not mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Contains(PeriodUnitID, PeriodID) Then
                    mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Add(mModelMonitorServiceThreshold.ID, PeriodUnitID, PeriodID, 1)
                    mModelMonitorServiceThreshold.ModelMonitorServicePeriods.CurrentItem.MonitorTypeID = mModelMonitorServiceThreshold.MonitorTypeID
                    Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod = AssemblyMonitorServiceStatusPeriod.NewAssemblyMonitorServiceStatusPeriod(mAssemblyMonitorServiceStatusThreshold.ID,
                                                                                                                                                                              mModelMonitorServiceThreshold.ModelMonitorServicePeriods.CurrentItem.ID,
                                                                                                                                                                              mAssemblyStatus.ID, PeriodID, PeriodUnitID, 0, Today.Date.ToString)
                    mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Add(mAssemblyMonitorServiceStatusPeriod)
                End If

            Next

            Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold
            Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
            dgPeriodsThreshold.DataSource = mModelMonitorServiceThreshold.ModelMonitorServicePeriods
            dgPeriodsThreshold.DataBind()
            upnlPeriodsThreshold.Update()
        End If



        If mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Count > 0 And chkIsThreshold.Checked Then
            For i As Integer = 0 To mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Count - 1
                Dim PeriodID As Integer = mModelMonitorServiceThreshold.ModelMonitorServicePeriods(i).PeriodID
                Dim PeriodUnitID As Integer = mModelMonitorServiceThreshold.ModelMonitorServicePeriods(i).PeriodUnitID

                If Not mModelMonitorServiceInterval.ModelMonitorServicePeriods.Contains(PeriodUnitID, PeriodID) Then
                    mModelMonitorServiceInterval.ModelMonitorServicePeriods.Add(mModelMonitorServiceInterval.ID, PeriodUnitID, PeriodID, 1)
                    mModelMonitorServiceInterval.ModelMonitorServicePeriods.CurrentItem.MonitorTypeID = mModelMonitorServiceInterval.MonitorTypeID
                    Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod = AssemblyMonitorServiceStatusPeriod.NewAssemblyMonitorServiceStatusPeriod(mAssemblyMonitorServiceStatusInterval.ID,
                                                                                                                                                                                    mModelMonitorServiceInterval.ModelMonitorServicePeriods.CurrentItem.ID,
                                                                                                                                                                                    mAssemblyStatus.ID, PeriodID, PeriodUnitID, 0, Today.Date.ToString)
                    mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Add(mAssemblyMonitorServiceStatusPeriod)
                End If
            Next

            Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval
            Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
            dgPeriodsInterval.DataSource = mModelMonitorServiceInterval.ModelMonitorServicePeriods
            dgPeriodsInterval.DataBind()
            upnlPeriodsInterval.Update()
        End If

    End Sub

    Public Sub SetLicenceCountThreshold()
        If mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNoThreshold()
        If mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNoThreshold.Text = mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNoThreshold.Text = String.Empty
        End If
    End Sub
    Public Sub SetLicenceCountInterval()
        If mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNoInterval()
        If mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNoInterval.Text = mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNoInterval.Text = String.Empty
        End If
    End Sub

    Public Function CustomValidate3() As Boolean

        If chkIsApplicable.Checked = True Then Exit Function

        Dim str As String = ""
        SetObjectNA()

        If Not mModelMonitorServiceNA.IsValid Then
            For i As Integer = 0 To mModelMonitorServiceNA.GetBrokenRulesCollection.Count - 1
                str = str + "NA Activity : " + mModelMonitorServiceNA.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If

        For counter As Integer = 0 To mModelMonitorServiceNA.ModelMonitorServicePeriods.Count - 1
            If Not mModelMonitorServiceNA.ModelMonitorServicePeriods(counter).IsValid Then
                For i As Integer = 0 To mModelMonitorServiceNA.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection.Count - 1
                    str = str + "NA Activity : " + mModelMonitorServiceNA.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If
        Next

        If Not mAssemblyMonitorServiceStatusNA.IsValid Then
            For i As Integer = 0 To mAssemblyMonitorServiceStatusNA.GetBrokenRulesCollection.Count - 1
                str = str + "NA Activity : " + mAssemblyMonitorServiceStatusNA.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods.Count - 1)
            If Not mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods(i).IsValid Then
                For x As Int16 = 0 To CShort(mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
                    str = str + "NA Activity : " + mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvATAChapter.ErrorMessage = str
            cvATAChapter.IsValid = False
            Return False
        Else
            cvATAChapter.IsValid = True
            Return True
        End If
    End Function
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        SetObjectThreshold()
        SetGridObjectThreshold()

        SetThresholdStatusObject()
        SetGridThresholdStatusObject()

        SetObjectInterval()
        SetGridObjectInterval()

        SetIntervalStatusObject()
        SetGridIntervalStatusObject()


        If chkIsThreshold.Checked Then
            If Not mModelMonitorServiceThreshold.IsValid Then
                For i As Integer = 0 To mModelMonitorServiceThreshold.GetBrokenRulesCollection.Count - 1
                    str = str + "Threshold Activity : " + mModelMonitorServiceThreshold.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If

            For counter As Integer = 0 To dgPeriodsThreshold.Rows.Count - 1
                If Not mModelMonitorServiceThreshold.ModelMonitorServicePeriods(counter).IsValid Then
                    For i As Integer = 0 To mModelMonitorServiceThreshold.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection.Count - 1
                        str = str + "Threshold Activity : " + mModelMonitorServiceThreshold.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection(i).Description + "<BR>"
                    Next
                End If
            Next



            If Not mAssemblyMonitorServiceStatusThreshold.IsValid Then
                For i As Integer = 0 To mAssemblyMonitorServiceStatusThreshold.GetBrokenRulesCollection.Count - 1
                    str = str + "Threshold Activity : " + mAssemblyMonitorServiceStatusThreshold.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If
            For i As Integer = 0 To CShort(mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Count - 1)
                If Not mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods(i).IsValid Then
                    For x As Int16 = 0 To CShort(mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
                        str = str + "Threshold Activity : " + mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                    Next
                End If
            Next


        End If


        If chkIsInterval.Checked Then
            If Not mModelMonitorServiceInterval.IsValid Then
                For i As Integer = 0 To mModelMonitorServiceInterval.GetBrokenRulesCollection.Count - 1
                    str = str + "Interval Activity : " + mModelMonitorServiceInterval.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If

            For counter As Integer = 0 To dgPeriodsInterval.Rows.Count - 1
                If Not mModelMonitorServiceInterval.ModelMonitorServicePeriods(counter).IsValid Then
                    For i As Integer = 0 To mModelMonitorServiceInterval.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection.Count - 1
                        str = str + "Interval Activity : " + mModelMonitorServiceInterval.ModelMonitorServicePeriods(counter).GetBrokenRulesCollection(i).Description + "<BR>"
                    Next
                End If
            Next


            If Not mAssemblyMonitorServiceStatusInterval.IsValid Then
                For i As Integer = 0 To mAssemblyMonitorServiceStatusInterval.GetBrokenRulesCollection.Count - 1
                    str = str + "Interval Activity : " + mAssemblyMonitorServiceStatusInterval.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            End If
            For i As Integer = 0 To CShort(mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Count - 1)
                If Not mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods(i).IsValid Then
                    For x As Int16 = 0 To CShort(mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
                        str = str + "Interval Activity : " + mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                    Next
                End If
            Next



        End If


        If str <> "" Then
            cvATAChapter.ErrorMessage = str
            cvATAChapter.IsValid = False
            Return False
        Else
            cvATAChapter.IsValid = True
            Return True
        End If
    End Function
    Public Function SaveThreshold() As Boolean

        If Not chkIsThreshold.Checked Then Return False

        SetObjectThreshold()
        SetGridObjectThreshold()

        Dim mModelMonitorServiceThresholdClone As ModelMonitorService
        mModelMonitorServiceThresholdClone = CType(mModelMonitorServiceThreshold, ModelMonitorService)

        If mModelMonitorServiceThreshold.IsValid = True Then

            Try

                Dim ServiceMPDTitle As String = ""

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "AMP"
                Else
                    ServiceMPDTitle = "Model Service"
                End If

                If mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Count = 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired,
                                    MSGBox.Message_text.PeriodRequired,
                                    ServiceMPDTitle + " cannot be saved without Period units",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Return False

                End If

                mModelMonitorServiceThreshold.ApplyEdit()
                mModelMonitorServiceThreshold = CType(mModelMonitorServiceThreshold.Save, ModelMonitorService)

                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold
                mModel = mModelMonitorServiceThreshold.Model.Name
                mMonitorType = mModelMonitorServiceThreshold.ModelMonitorServiceTypeName
                mDescription = txtDescription.Text
                mDetail = "Model : " + mModel + " Monitor Type : " + mMonitorType + " Description : " + mDescription

                MarkLog(Action:=Action.Save,
                        ModuleName:="Model Service",
                        Detail:=mDetail,
                        ErrorType:=ErrorType.NoError,
                        TransID:=mModelMonitorServiceThreshold.ID,
                        EventLogID)

                'End

                If SaveThresholdStatus() Then
                    Return True
                End If

                Return False
            Catch ex As SqlException

                If ex.Number = 8145 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.ProcedureError,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 2627 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.Duplicate,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 547 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                    MSGBox.Message_text.saveAlert,
                                    "This Entry is used by Some One.",
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

                mModelMonitorServiceThreshold = mModelMonitorServiceThresholdClone
                Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold

                Return False

            End Try

        Else
            Return False
        End If
    End Function
    Private Sub SetGridThresholdStatusObject()
        mAssemblyMonitorServiceStatusThreshold = Session("mAssemblyMonitorServiceStatusThreshold")
        Dim calDoneOn, txtDueOnValue, txtExtensionValue As TextBox
        'If mAssemblyMonitorServiceStatusThreshold.ModelMonitorService.MonitorTypeID <> 3 Then
        '    For i As Integer = 0 To CShort(dgThresholdValues.Rows.Count - 1)
        '        txtElapsedValue = CType(Me.dgThresholdValues.Rows(i).FindControl("txtElapsedValue"), TextBox)
        '        txtRemainingValue = CType(Me.dgThresholdValues.Rows(i).FindControl("txtRemainingValue"), TextBox)
        '        With mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
        '            .Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
        '            .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
        '        End With
        '    Next i
        'End If
        For j As Integer = 0 To Me.dgThresholdValues.Rows.Count - 1
            calDoneOn = CType(Me.dgThresholdValues.Rows(j).FindControl("txtDoneOnValueThreshold"), TextBox)
            txtDueOnValue = CType(Me.dgThresholdValues.Rows(j).FindControl("txtDueOnValueThreshold"), TextBox)
            txtExtensionValue = CType(Me.dgThresholdValues.Rows(j).FindControl("txtExtensionValueThreshold"), TextBox) 'Added By Saylee on 22-07-2008
            With mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(j).DoneOnValue = ""
                    Else
                        .Item(j).DoneOnValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(j).DoneOnValue = Trim(calDoneOn.Text)
                End If
                .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)

                '.Item(j).ElapsedValue = Me.dgThresholdValues.Rows(j).Cells(4).ToString
                '.Item(j).RemainingValue = Me.dgThresholdValues.Rows(j).Cells(8).ToString
            End With
        Next j
        Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
    End Sub
    Public Sub SetThresholdStatusObject()
        ' If Not rdbIsComplianceThresholdYes.Checked Then Exit Sub

        mAssemblyMonitorServiceStatusThreshold = Session("mAssemblyMonitorServiceStatusThreshold")
        With mAssemblyMonitorServiceStatusThreshold
            If Not mModelMonitorServiceThreshold.IsNew And mAssemblyMonitorServiceStatusThreshold.IsNew Then
                .ModelMonitorServiceID(False) = mModelMonitorServiceThreshold.ID
                dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
                dgThresholdValues.DataBind()
                upnlThresholdValues.Update()
            End If

            '.ModelMonitorInsp.Code = mModelMonitorInsp.Code
            .ModelMonitorService.Reference = mModelMonitorServiceThreshold.Reference
            .ModelMonitorService.Description = mModelMonitorServiceThreshold.Description
            .ModelMonitorService.RequiredManHours = mModelMonitorServiceThreshold.RequiredManHours


            If txtDoneOnDateThreshold.Text = "" Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = txtDoneOnDateThreshold.Text
            End If

            If chkIsThreshold.Checked Then 'If rdbIsComplianceThresholdYes.Checked Then
                .IsApplicable = True
            Else
                .IsApplicable = False
            End If



            If txtDoneOnDateInterval.Text <> "" And rdbIsComplianceIntervalYes.Checked Then
                .IsApplicable = False
            Else
                .IsApplicable = True
            End If



            .DoneWONo = Trim(txtWorkOrNoThreshold.Text)
            .DoneRemark = Trim(txtRemarkThreshold.Text)
            .RequiredManHours = Trim(txtRequiredManHoursThreshold.Text)
            .Place = Trim(txtPlaceThreshold.Text)


            Dim LicenseNo As String = String.Empty 'Added By Prashant On 12-Jun-2012 FOR ALL08062012
            Dim EmpName As String = String.Empty
            If (txtLicenceNoThreshold.Text.Trim.IndexOf("[") > 0 And txtLicenceNoThreshold.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNoThreshold.Text.Substring(0, txtLicenceNoThreshold.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNoThreshold.Text.Trim, txtLicenceNoThreshold.Text.Trim.IndexOf("[") + 2, txtLicenceNoThreshold.Text.Trim.IndexOf("]") - txtLicenceNoThreshold.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNoThreshold.Text)
            End If
            .LicenseNo = LicenseNo
            .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID


        End With
    End Sub
    Public Function SaveThresholdStatus() As Boolean
        ' If Not rdbIsComplianceThresholdYes.Checked Then Return False

        SetThresholdStatusObject()
        SetGridThresholdStatusObject()

        If mAssemblyMonitorServiceStatusThreshold.IsValid Then
            mAssemblyMonitorServiceStatusThreshold = Session("mAssemblyMonitorServiceStatusThreshold")
            mAssemblyMonitorServiceStatusThreshold.ApplyEdit()
            mAssemblyMonitorServiceStatusThreshold = CType(mAssemblyMonitorServiceStatusThreshold.Save(), AssemblyMonitorServiceStatus)

            Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold

            Return True
        Else
            Dim str As String = ""
            For i As Integer = 0 To mAssemblyMonitorServiceStatusThreshold.GetBrokenRulesCollection.Count - 1
                str = str + "Threshold Activity : " + mAssemblyMonitorServiceStatusThreshold.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
            For i As Integer = 0 To CShort(mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Count - 1)
                If Not mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Item(i).IsValid Then
                    For x As Int16 = 0 To CShort(mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
                        str = str + "Threshold Activity : " + mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                    Next
                End If
            Next
            If str <> "" Then
                cvATAChapter.ErrorMessage = str
                cvATAChapter.IsValid = False
                Return False
            Else
                cvATAChapter.IsValid = True
                '  Return True
            End If
        End If

    End Function

    Public Function SaveInterval() As Boolean

        If Not chkIsInterval.Checked Then Return False

        SetObjectInterval()
        SetGridObjectInterval()

        Dim mModelMonitorServiceIntervalClone As ModelMonitorService
        mModelMonitorServiceIntervalClone = CType(mModelMonitorServiceInterval, ModelMonitorService)

        If mModelMonitorServiceInterval.IsValid = True Then

            Try

                Dim ServiceMPDTitle As String = ""

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "AMP"
                Else
                    ServiceMPDTitle = "Model Service"
                End If

                If mModelMonitorServiceInterval.ModelMonitorServicePeriods.Count = 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired,
                                    MSGBox.Message_text.PeriodRequired,
                                    ServiceMPDTitle + " cannot be saved without Period units",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Return False

                End If

                mModelMonitorServiceInterval.ApplyEdit()
                mModelMonitorServiceInterval = CType(mModelMonitorServiceInterval.Save, ModelMonitorService)

                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval
                mModel = mModelMonitorServiceInterval.Model.Name
                mMonitorType = mModelMonitorServiceInterval.ModelMonitorServiceTypeName
                mDescription = txtDescription.Text
                mDetail = "Model : " + mModel + " Monitor Type : " + mMonitorType + " Description : " + mDescription

                MarkLog(Action:=Action.Save,
                        ModuleName:="Model Service",
                        Detail:=mDetail,
                        ErrorType:=ErrorType.NoError,
                        TransID:=mModelMonitorServiceInterval.ID,
                        EventLogID)

                'End


                If SaveIntervalStatus() Then
                    Return True
                End If



                Return False

            Catch ex As SqlException

                If ex.Number = 8145 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.ProcedureError,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 2627 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.Duplicate,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 547 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                    MSGBox.Message_text.saveAlert,
                                    "This Entry is used by Some One.",
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

                mModelMonitorServiceInterval = mModelMonitorServiceIntervalClone
                Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval

                Return False

            End Try

        Else
            Return False
        End If
    End Function
    Private Sub SetGridIntervalStatusObject()
        mAssemblyMonitorServiceStatusInterval = Session("mAssemblyMonitorServiceStatusInterval")
        Dim calDoneOn, txtDueOnValue, txtExtensionValue As TextBox
        'If mAssemblyMonitorServiceStatusInterval.ModelMonitorService.MonitorTypeID <> 3 Then
        '    For i As Integer = 0 To CShort(dgIntervalValues.Rows.Count - 1)
        '        txtElapsedValue = CType(Me.dgIntervalValues.Rows(i).FindControl("txtElapsedValue"), TextBox)
        '        txtRemainingValue = CType(Me.dgIntervalValues.Rows(i).FindControl("txtRemainingValue"), TextBox)
        '        With mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
        '            .Item(i).ElapsedValue = Trim(txtElapsedValue.Text)
        '            .Item(i).RemainingValue = Trim(txtRemainingValue.Text)
        '        End With
        '    Next i
        'End If
        For j As Integer = 0 To Me.dgIntervalValues.Rows.Count - 1
            calDoneOn = CType(Me.dgIntervalValues.Rows(j).FindControl("txtDoneOnValueInterval"), TextBox)
            txtDueOnValue = CType(Me.dgIntervalValues.Rows(j).FindControl("txtDueOnValueInterval"), TextBox)
            txtExtensionValue = CType(Me.dgIntervalValues.Rows(j).FindControl("txtExtensionValueInterval"), TextBox) 'Added By Saylee on 22-07-2008
            With mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text.Trim) Then
                        .Item(j).DoneOnValue = ""
                    Else
                        .Item(j).DoneOnValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(j).DoneOnValue = Trim(calDoneOn.Text)
                End If
                .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)

                '.Item(j).ElapsedValue = Me.dgIntervalValues.Rows(j).Cells(4).ToString
                '.Item(j).RemainingValue = Me.dgIntervalValues.Rows(j).Cells(8).ToString
            End With
        Next j
        Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
    End Sub
    Private Sub SetIntervalStatusObject()
        mAssemblyMonitorServiceStatusInterval = Session("mAssemblyMonitorServiceStatusInterval")
        With mAssemblyMonitorServiceStatusInterval
            If Not mModelMonitorServiceInterval.IsNew And mAssemblyMonitorServiceStatusInterval.IsNew Then
                .ModelMonitorServiceID(False) = mModelMonitorServiceInterval.ID
                dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                dgIntervalValues.DataBind()
                upnlIntervalValues.Update()
            End If

            '.ModelMonitorInsp.Code = mModelMonitorInsp.Code
            .ModelMonitorService.Reference = mModelMonitorServiceInterval.Reference
            .ModelMonitorService.Description = mModelMonitorServiceInterval.Description
            .ModelMonitorService.RequiredManHours = mModelMonitorServiceInterval.RequiredManHours


            If txtDoneOnDateInterval.Text = "" Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = txtDoneOnDateInterval.Text
            End If

            If chkIsInterval.Checked Then ' If rdbIsComplianceIntervalYes.Checked Then
                .IsApplicable = True
            Else
                .IsApplicable = False
            End If


            .DoneWONo = Trim(txtWorkOrNoInterval.Text)
            .DoneRemark = Trim(txtRemarkInterval.Text)
            .RequiredManHours = Trim(txtRequiredManHoursInterval.Text)
            .Place = Trim(txtPlaceInterval.Text)


            Dim LicenseNo As String = String.Empty 'Added By Prashant On 12-Jun-2012 FOR ALL08062012
            Dim EmpName As String = String.Empty
            If (txtLicenceNoInterval.Text.Trim.IndexOf("[") > 0 And txtLicenceNoInterval.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNoInterval.Text.Substring(0, txtLicenceNoInterval.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNoInterval.Text.Trim, txtLicenceNoInterval.Text.Trim.IndexOf("[") + 2, txtLicenceNoInterval.Text.Trim.IndexOf("]") - txtLicenceNoInterval.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNoInterval.Text)
            End If
            .LicenseNo = LicenseNo
            .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID


        End With
    End Sub
    Public Function SaveIntervalStatus() As Boolean
        'If Not rdbIsComplianceIntervalYes.Checked Then Return False

        SetIntervalStatusObject()
        SetGridIntervalStatusObject()



        'Linking Activity
        If rdbIsComplianceIntervalNo.Checked And rdbIsComplianceThresholdYes.Checked And txtDoneOnDateThreshold.Text <> "" Then
            If rdbMakeApplicable.Checked Then
                '' ActionID = LinkAction.MakeApplicable
                mAssemblyMonitorServiceStatusInterval.IsApplicable = True
            ElseIf rdbMakeApplicableAndStart.Checked Then
                'MakeApplicableAndStart
                mAssemblyMonitorServiceStatusInterval.IsApplicable = True

                'Setting Currrent Values to Done On Values...as default
                For i As Integer = 0 To mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Count - 1
                    With mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                        If .Item(i).PeriodID = 2 Then
                            If Not Period.IsDate(mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Item(i).CurrentValueFormatted) Then
                                .Item(i).DoneOnValue = ""
                            Else
                                .Item(i).DoneOnValueFormatted = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Item(i).CurrentValueFormatted
                            End If
                        Else
                            .Item(i).DoneOnValue = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Item(i).CurrentValue
                        End If

                        ''ExtensionValue
                        '.Item(i).ExtensionValue = PeriodValues(i)
                    End With
                Next
            ElseIf rdbMakeNotApplicable.Checked Then
                'ActionID = LinkAction.MakeNotApplicable
                mAssemblyMonitorServiceStatusInterval.IsApplicable = False
            ElseIf rdbComply.Checked Then
                '' ActionID = LinkAction.Comply
                mAssemblyMonitorServiceStatusInterval.IsApplicable = True
                mAssemblyMonitorServiceStatusInterval.DoneOn = mAssemblyMonitorServiceStatusThreshold.DoneOnFormatted

                mAssemblyMonitorServiceStatusInterval.DoneRemark = mAssemblyMonitorServiceStatusThreshold.DoneRemark 'mMultiCompliance.DoneRemark
                mAssemblyMonitorServiceStatusInterval.DoneWONo = mAssemblyMonitorServiceStatusThreshold.DoneWONo

                mAssemblyMonitorServiceStatusInterval.Place = mAssemblyMonitorServiceStatusThreshold.Place
                mAssemblyMonitorServiceStatusInterval.LicenseNo = mAssemblyMonitorServiceStatusThreshold.AllLicenceNos
                mAssemblyMonitorServiceStatusInterval.DoneByID = mAssemblyMonitorServiceStatusThreshold.DoneByID


                For i As Integer = 0 To mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Count - 1  'Number of rows in 2 -dim array.Zero Based
                    For j As Integer = 0 To mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Count - 1
                        With mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                            If (mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods(i).PeriodUnitID = (.Item(j).PeriodUnitID)) And (mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods(i).PeriodID = (.Item(j).PeriodID)) Then

                                If .Item(j).PeriodID = 2 Then
                                    If Not Period.IsDate(mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods(i).CurrentValue) Then
                                        .Item(j).CurrentValue = ""
                                    Else
                                        .Item(j).CurrentValueFormatted = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods(i).CurrentValueFormatted
                                    End If
                                Else
                                    .Item(j).CurrentValue = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods(i).CurrentValue
                                End If
                            End If
                        End With
                    Next
                Next
            ElseIf rdbDoNothing.Checked Then
                '' ActionID = LinkAction.DoNothing

            End If

        End If
        If mAssemblyMonitorServiceStatusInterval.IsValid Then

            mAssemblyMonitorServiceStatusInterval.ApplyEdit()
            mAssemblyMonitorServiceStatusInterval = CType(mAssemblyMonitorServiceStatusInterval.Save(), AssemblyMonitorServiceStatus)

            Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval

            Return True
        Else
            Dim str As String = ""
            For i As Integer = 0 To mAssemblyMonitorServiceStatusInterval.GetBrokenRulesCollection.Count - 1
                str = str + "Interval Activity : " + mAssemblyMonitorServiceStatusInterval.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
            For i As Integer = 0 To CShort(mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Count - 1)
                If Not mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Item(i).IsValid Then
                    For x As Int16 = 0 To CShort(mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
                        str = str + "Interval Activity : " + mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                    Next
                End If
            Next
            If str <> "" Then
                cvATAChapter.ErrorMessage = str
                cvATAChapter.IsValid = False
                Return False
            Else
                cvATAChapter.IsValid = True
                '  Return True
            End If
        End If
    End Function
    Private Sub SetColorThreshold()
        If Not mAssemblyMonitorServiceStatusThreshold Is Nothing Then
            If mAssemblyMonitorServiceStatusThreshold.ModelMonitorService.MonitorTypeID = 1 And Not mAssemblyMonitorServiceStatusThreshold.DoneOn Is System.DBNull.Value Then
                Dim txtdueOnValue As TextBox
                For i As Integer = 0 To dgThresholdValues.Rows.Count - 1
                    txtdueOnValue = CType(dgThresholdValues.Rows(i).FindControl("txtDueOnValueThreshold"), TextBox)
                    txtdueOnValue.BackColor = System.Drawing.Color.Red
                    txtdueOnValue.ForeColor = System.Drawing.Color.White
                Next
                lblRed.Visible = True
                lblInfo.Visible = True
            Else
                lblRed.Visible = False
                lblInfo.Visible = False
            End If
        End If
    End Sub 'End
    Private Sub SetObjectNA()

        If Session("FromEditThresholdInterval") = "True" Then
            'do nothing
        Else
            Dim mID As Guid = Guid.NewGuid
            mModelMonitorServiceNA = ModelMonitorService.NewModelMonitorService(mID, mAssemblyStatus.Assembly.ModelID, mMachine.HourType, mID)
            mAssemblyMonitorServiceStatusNA = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Today.Date.ToString, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
        End If


        If mModelMonitorServiceTypeList.Contains(mMPDMaster.ServiceTypeID, 3) And mModelMonitorServiceNA.IsNew Then 'N/A type
            mModelMonitorServiceNA.ModelMonitorServiceTypeID = CType(Val(mModelMonitorServiceTypeList(mMPDMaster.ServiceTypeID, 3).ID), Int32)
        End If

        mModelMonitorServiceNA.MPDMasterID = mMPDMaster.ID
        mModelMonitorServiceNA.ATAID = mMPDMaster.ATAID
        mModelMonitorServiceNA.Reference = Trim(txtReference.Text)
        mModelMonitorServiceNA.Description = Trim(txtDescription.Text)
        '  mModelMonitorServiceNA.ModelMonitorServiceTypeID = CType(Val(cmbMonitorServiceType.SelectedValue.ToString), Int32)
        mModelMonitorServiceNA.Note = Trim(txtNote.Text)
        '  mModelMonitorServiceNA.ShowInCofA = chkShowInCofA.Checked
        ' mModelMonitorServiceNA.RequiredManHours = txtRequiredManHours.Text.Trim
        mModelMonitorServiceNA.Zone = mMPDMaster.Zone
        '  mModelMonitorServiceNA.Area = Trim(txtArea.Text)
        ' mModelMonitorServiceNA.IsRII = chkIsRII.Checked 'End
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mModelMonitorServiceNA.IsAttachmentAdded = True
            Else
                mModelMonitorServiceNA.IsAttachmentAdded = False
            End If
        End If


        mModelMonitorServiceNA.TaskCardNo = txtAMPNo.Text.Trim

        mModelMonitorServiceNA.Applicability = txtApplicability.Text.Trim
        ' mModelMonitorServiceNA.Source = txtSource.Text.Trim
        mModelMonitorServiceNA.Access = mMPDMaster.Access.Trim
        mModelMonitorServiceNA.MPDSkillID = mMPDMaster.MPDSkillID
        mModelMonitorServiceNA.MPDTypeID = mMPDMaster.MPDTypeID

        '  mModelMonitorServiceNA.AccessOpenCloseManHours = txtAccessManHours.Text.Trim
        ''********************
        If mModelMonitorServiceNA.IsNew Then mModelMonitorServiceNA.ModelMonitorServicePeriods.Add(mModelMonitorServiceNA.ID, 1, 1, mMachine.HourType)

        mModelMonitorServiceNA.ModelMonitorServicePeriods.CurrentItem.MonitorTypeID = mModelMonitorServiceNA.MonitorTypeID
        mModelMonitorServiceNA.ModelMonitorServicePeriods.CurrentItem.FrequencyValue = "0"
        Session("mModelMonitorServiceNA") = mModelMonitorServiceNA



        'Status
        Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod
        If mAssemblyMonitorServiceStatusNA.IsNew Then
            mAssemblyMonitorServiceStatusPeriod =
                AssemblyMonitorServiceStatusPeriod.NewAssemblyMonitorServiceStatusPeriod(mAssemblyMonitorServiceStatusNA.ID,
                                                                                         mModelMonitorServiceNA.ModelMonitorServicePeriods.CurrentItem.ID,
                                                                                         mAssemblyStatus.ID, 1, 1, 0, Today.Date.ToString)
            mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods.Add(mAssemblyMonitorServiceStatusPeriod)
        End If
        With mAssemblyMonitorServiceStatusNA
            If Not mModelMonitorServiceNA.IsNew And mAssemblyMonitorServiceStatusNA.IsNew Then
                .ModelMonitorServiceID(True) = mModelMonitorServiceNA.ID
            End If

            '.ModelMonitorInsp.Code = mModelMonitorInsp.Code
            .ModelMonitorService.Reference = mModelMonitorServiceNA.Reference
            .ModelMonitorService.Description = mModelMonitorServiceNA.Description
            .ModelMonitorService.RequiredManHours = mModelMonitorServiceNA.RequiredManHours


            'If txtDoneOnDateNA.Text = "" Then
            '    .DoneOn = System.DBNull.Value
            'Else
            '    .DoneOn = txtDoneOnDateNA.Text
            'End If
            '.DoneWONo = Trim(txtWorkOrNoNA.Text)
            '.DoneRemark = Trim(txtRemarkNA.Text)
            '.RequiredManHours = Trim(txtRequiredManHoursNA.Text)
            '.Place = Trim(txtPlaceNA.Text)




        End With
        Session("mAssemblyMonitorServiceStatusNA") = mAssemblyMonitorServiceStatusNA
    End Sub
    Public Function SaveNARecord() As Boolean

        SetObjectNA()
        Dim mModelMonitorServiceNAClone As ModelMonitorService
        mModelMonitorServiceNAClone = CType(mModelMonitorServiceNA, ModelMonitorService)

        If mModelMonitorServiceNA.IsValid = True Then

            Try

                Dim ServiceMPDTitle As String = ""

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "AMP"
                Else
                    ServiceMPDTitle = "Model Service"
                End If

                If mModelMonitorServiceNA.ModelMonitorServicePeriods.Count = 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired,
                                    MSGBox.Message_text.PeriodRequired,
                                    ServiceMPDTitle + " cannot be saved without Period units",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Return False

                End If

                mModelMonitorServiceNA.ApplyEdit()
                mModelMonitorServiceNA = CType(mModelMonitorServiceNA.Save, ModelMonitorService)

                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                Session("mModelMonitorServiceNA") = mModelMonitorServiceNA
                mModel = mModelMonitorServiceNA.Model.Name
                mMonitorType = mModelMonitorServiceNA.ModelMonitorServiceTypeName
                mDescription = txtDescription.Text
                mDetail = "Model : " + mModel + " Monitor Type : " + mMonitorType + " Description : " + mDescription

                MarkLog(Action:=Action.Save,
                        ModuleName:="Model Service",
                        Detail:=mDetail,
                        ErrorType:=ErrorType.NoError,
                        TransID:=mModelMonitorServiceNA.ID,
                        EventLogID)

                'End


                'Status
                mAssemblyMonitorServiceStatusNA.IsApplicable = False
                mAssemblyMonitorServiceStatusNA.ModelMonitorServiceID(True) = mModelMonitorServiceNA.ID
                If mAssemblyMonitorServiceStatusNA.IsValid Then
                    mAssemblyMonitorServiceStatusNA = Session("mAssemblyMonitorServiceStatusNA")
                    mAssemblyMonitorServiceStatusNA.ApplyEdit()
                    mAssemblyMonitorServiceStatusNA = CType(mAssemblyMonitorServiceStatusNA.Save(), AssemblyMonitorServiceStatus)

                    Session("mAssemblyMonitorServiceStatusNA") = mAssemblyMonitorServiceStatusNA

                    Return True
                Else
                    Dim str As String = ""
                    For i As Integer = 0 To mAssemblyMonitorServiceStatusNA.GetBrokenRulesCollection.Count - 1
                        str = str + "NA Activity : " + mAssemblyMonitorServiceStatusNA.GetBrokenRulesCollection(i).Description + "<BR>"
                    Next
                    For i As Integer = 0 To CShort(mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods.Count - 1)
                        If Not mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods.Item(i).IsValid Then
                            For x As Int16 = 0 To CShort(mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
                                str = str + "NA Activity : " + mAssemblyMonitorServiceStatusNA.AssemblyMonitorServiceStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                            Next
                        End If
                    Next
                    If str <> "" Then
                        cvATAChapter.ErrorMessage = str
                        cvATAChapter.IsValid = False
                        Return False
                    Else
                        cvATAChapter.IsValid = True
                        '  Return True
                    End If
                End If

                Return False



            Catch ex As SqlException

                If ex.Number = 8145 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.ProcedureError,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 2627 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                    MSGBox.Message_text.Duplicate,
                                    ex.Procedure,
                                    MsgBoxStyle.OkOnly,
                                    "")

                ElseIf ex.Number = 547 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                    MSGBox.Message_text.saveAlert,
                                    "This Entry is used by Some One.",
                                    MsgBoxStyle.OkOnly,
                                    "")

                End If

                mModelMonitorServiceNA = mModelMonitorServiceNAClone
                Session("mModelMonitorServiceNA") = mModelMonitorServiceNA

                Return False

            End Try

        Else
            Return False
        End If
    End Function
    Private Function Save() As Boolean
        Dim mIsThresholdSaved As Boolean = False
        Dim mIsIntervalSaved As Boolean = False
        Dim mIsNARecordSaved As Boolean = False

        If Not IsValid Then Exit Function

        If chkIsApplicable.Checked Then
            If SaveThreshold() Then
                mIsThresholdSaved = True
            Else
                mIsThresholdSaved = False
            End If


            If SaveInterval() Then
                mIsIntervalSaved = True
            Else
                mIsIntervalSaved = False
            End If

            If mIsThresholdSaved = True Or mIsIntervalSaved = True Then

                'Linking
                ' If rdbIsComplianceThresholdYes.Checked = False And rdbIsComplianceIntervalYes.Checked = False Then
                If rdbIsComplianceIntervalYes.Checked = False And chkIsInterval.Checked = True Then SaveLinkActivity()
                '  End If

                Return True
            Else
                Return False
            End If
        Else
            If SaveNARecord() Then
                Return True
            Else
                Return False
            End If

        End If


    End Function
    Public Sub SaveLinkActivity()
        Dim ActionID As Integer = 0
        If rdbMakeApplicable.Checked Then
            ActionID = LinkAction.MakeApplicable
        ElseIf rdbMakeApplicableAndStart.Checked Then
            ActionID = LinkAction.MakeApplicableAndStart
        ElseIf rdbMakeNotApplicable.Checked Then
            ActionID = LinkAction.MakeNotApplicable
        ElseIf rdbComply.Checked Then
            ActionID = LinkAction.Comply
        ElseIf rdbDoNothing.Checked Then
            ActionID = LinkAction.DoNothing
        End If


        Dim mLinkMaintenance As LinkMaintenance

        If mLinkMaintenanceList Is Nothing Then
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorServiceThreshold.ID.ToString)
        End If

        If mLinkMaintenanceList.Count = 0 Then
            mLinkMaintenanceList.add(LinkMaintenance.NewChildLinkedMaintenance(Guid.NewGuid, mModelMonitorServiceThreshold.ID, mModelMonitorServiceInterval.ID, 1))
            mLinkMaintenanceList(0).MaintenanceActionID = ActionID
            Try
                mLinkMaintenanceList = CType(mLinkMaintenanceList.Save, LinkMaintenanceList)
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
                End If

            End Try
        End If
    End Sub



#End Region


#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            setFocus(txtAMPNo)
            mModelMonitorServiceTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList("<SELECT>")
            Session("mModelMonitorServiceTypeList") = mModelMonitorServiceTypeList
            AddSelectedPeriodUnitsThreshold(Today.Date.ToString)
            AddSelectedPeriodUnitsInterval(Today.Date.ToString)
            DataFieldBind()
            SetLicenceCountThreshold()
            SetLicenceCountInterval()
            SetColorThreshold()
        End If
        lblTitle.Text = "MPD Configuration for " + RegNo + " [ " + mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo + " ]"
        txtApplicableTo.Text = RegNo.Trim
        txtCurrentValues.Text = AirframeCurrentValues.Trim.Replace("<br>", vbCrLf)
    End Sub
    Private Sub btnAddPeriodUnit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPeriodUnitThreshold.Click, btnAddPeriodUnitInterval.Click

        'THRESHOLD
        SetObjectThreshold()
        SetPeriodUnitsThreshold()
        SetGridObjectThreshold()

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorServiceThreshold.IsNew Then
            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
            mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorServiceThreshold.ModelID, mModelMonitorServiceThreshold.ID.ToString)

            If mModelMonitorConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                    If i = mModelMonitorConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                    End If
                Next

                Dim ServiceMPDTitle As String = ""
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD"
                Else
                    ServiceMPDTitle = "Service"
                End If

                MSGBoxCtrl.Show("Alert!", ServiceMPDTitle + " is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")

                Exit Sub

            End If
        End If


        'INTERVAL
        SetObjectInterval()
        SetPeriodUnitsInterval()
        SetGridObjectInterval()

        'Added by saylee on 1-Jun-2016
        If Not mModelMonitorServiceInterval.IsNew Then
            Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
            mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorServiceInterval.ModelID, mModelMonitorServiceInterval.ID.ToString)

            If mModelMonitorConfiguredList.Count > 0 Then
                Dim SerialNos As String = String.Empty

                For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                    If i = mModelMonitorConfiguredList.Count - 1 Then
                        SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                    Else
                        SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                    End If
                Next

                Dim ServiceMPDTitle As String = ""
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ServiceMPDTitle = "MPD"
                Else
                    ServiceMPDTitle = "Service"
                End If

                MSGBoxCtrl.Show("Alert!", ServiceMPDTitle + " is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")

                Exit Sub

            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow()", True)
    End Sub
    Private Sub hdnBtnPeriodUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnPeriodUnit.Click
        mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
        If chkIsThreshold.Checked Then AddSelectedPeriodUnitsThreshold(Today.Date.ToString)

        If chkIsInterval.Checked Then AddSelectedPeriodUnitsInterval(Today.Date.ToString)

        dgPeriodsThreshold.DataSource = mModelMonitorServiceThreshold.ModelMonitorServicePeriods
        dgPeriodsThreshold.DataBind()
        upnlPeriodsThreshold.Update()

        dgPeriodsInterval.DataSource = mModelMonitorServiceInterval.ModelMonitorServicePeriods
        dgPeriodsInterval.DataBind()
        upnlPeriodsInterval.Update()

        dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
        dgThresholdValues.DataBind()
        upnlThresholdValues.Update()
        SetColorThreshold()
    End Sub
    Protected Sub txtLicenceNoThreshold_TextChanged(sender As Object, e As System.EventArgs)
        If (txtLicenceNoThreshold.Text.Trim.IndexOf("[") > 0 And txtLicenceNoThreshold.Text.Trim.IndexOf("]") > 0) Then
            LicenseNoThreshold = txtLicenceNoThreshold.Text.Substring(0, txtLicenceNoThreshold.Text.Trim.IndexOf("[")).Trim
            EmpNameThreshold = Mid(txtLicenceNoThreshold.Text.Trim, txtLicenceNoThreshold.Text.Trim.IndexOf("[") + 2, txtLicenceNoThreshold.Text.Trim.IndexOf("]") - txtLicenceNoThreshold.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNoThreshold = Trim(txtLicenceNoThreshold.Text)
        End If
        DoneByIDThreshold = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNoThreshold, EmpNameThreshold).EmpID
        Session("LicenseNoThreshold") = LicenseNoThreshold
        Session("EmployeeIDThreshold") = DoneByIDThreshold
        If Not DoneByIDThreshold.Equals(Guid.Empty) Then
            If mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees(0).EmployeeID = DoneByIDThreshold
                mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNoThreshold
                mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHoursThreshold.Text
                mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees(0).EmployeeName = EmpNameThreshold
            Else
                mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatusThreshold.ID, 5, DoneByIDThreshold, LicenseNoThreshold, txtRequiredManHoursThreshold.Text, EmpNameThreshold)
            End If
        Else
            If mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
        BindLicenceNoThreshold()
        SetLicenceCountThreshold()
        'txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetailsThreshold.Update()
    End Sub
    Protected Sub txtLicenceNoInterval_TextChanged(sender As Object, e As System.EventArgs)
        If (txtLicenceNoInterval.Text.Trim.IndexOf("[") > 0 And txtLicenceNoInterval.Text.Trim.IndexOf("]") > 0) Then
            LicenseNoInterval = txtLicenceNoInterval.Text.Substring(0, txtLicenceNoInterval.Text.Trim.IndexOf("[")).Trim
            EmpNameInterval = Mid(txtLicenceNoInterval.Text.Trim, txtLicenceNoInterval.Text.Trim.IndexOf("[") + 2, txtLicenceNoInterval.Text.Trim.IndexOf("]") - txtLicenceNoInterval.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNoInterval = Trim(txtLicenceNoInterval.Text)
        End If
        DoneByIDInterval = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNoInterval, EmpNameInterval).EmpID
        Session("LicenseNoInterval") = LicenseNoInterval
        Session("EmployeeIDInterval") = DoneByIDInterval
        If Not DoneByIDInterval.Equals(Guid.Empty) Then
            If mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees(0).EmployeeID = DoneByIDInterval
                mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNoInterval
                mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHoursInterval.Text
                mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees(0).EmployeeName = EmpNameInterval
            Else
                mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatusInterval.ID, 5, DoneByIDInterval, LicenseNoInterval, txtRequiredManHoursInterval.Text, EmpNameInterval)
            End If
        Else
            If mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
        BindLicenceNoInterval()
        SetLicenceCountInterval()
        'txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetailsInterval.Update()
    End Sub
    Protected Sub txtRequiredManHoursThreshold_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorServiceStatusThreshold.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHoursThreshold.Text
            Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
            upnlMonitoringStatusDetailsThreshold.Update()
        End If
    End Sub
    Protected Sub txtRequiredManHoursInterval_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorServiceStatusInterval.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHoursInterval.Text
            Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
            upnlMonitoringStatusDetailsInterval.Update()
        End If
    End Sub
    Protected Sub txtFrequencyValueThreshold_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Count - 1
            Dim txtFrequencyValueThreshold As TextBox = CType(Me.dgPeriodsThreshold.Rows(i).FindControl("txtFrequencyValueThreshold"), TextBox)
            With mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
                .Item(i).FrequencyValue = Trim(txtFrequencyValueThreshold.Text)
            End With
        Next i
        dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
        dgThresholdValues.DataBind()
        upnlThresholdValues.Update()
        SetColorThreshold()
        Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
    End Sub
    Protected Sub txtDoneOnValueThreshold_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Count - 1
            Dim calDoneOn As TextBox = CType(Me.dgThresholdValues.Rows(i).FindControl("txtDoneOnValueThreshold"), TextBox)
            With mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text) Then
                        .Item(i).DoneOnValueFormatted = ""
                    Else
                        .Item(i).DoneOnValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(i).DoneOnValue = Trim(calDoneOn.Text)
                End If
            End With
        Next i
        dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
        dgThresholdValues.DataBind()
        upnlThresholdValues.Update()
        SetColorThreshold()
        Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
    End Sub
    Protected Sub txtDueOnValueThreshold_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Count - 1
            Dim txtDueOnValue As TextBox = CType(Me.dgThresholdValues.Rows(i).FindControl("txtDueOnValueThreshold"), TextBox)
            With mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(txtDueOnValue.Text) Then
                        .Item(i).DueOnValueFormatted = ""
                    Else
                        .Item(i).DueOnValueFormatted = Trim(txtDueOnValue.Text)
                    End If
                Else
                    .Item(i).DueOnValue = Trim(txtDueOnValue.Text)
                End If
            End With
        Next i

        dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
        dgThresholdValues.DataBind()
        upnlThresholdValues.Update()
        SetColorThreshold()
        Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
    End Sub
    Protected Sub txtExtensionValueThreshold_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgThresholdValues.Rows(i).FindControl("txtExtensionValueThreshold"), TextBox)

            With mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
        dgThresholdValues.DataBind()
        upnlThresholdValues.Update()
        SetColorThreshold()
        Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
    End Sub


    ''' INTERVAL
    Protected Sub txtFrequencyValueInterval_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Count - 1
            Dim txtFrequencyValueInterval As TextBox = CType(Me.dgPeriodsInterval.Rows(i).FindControl("txtFrequencyValueInterval"), TextBox)
            With mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                .Item(i).FrequencyValue = Trim(txtFrequencyValueInterval.Text)
            End With
        Next i
        dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
        dgIntervalValues.DataBind()
        upnlIntervalValues.Update()
        Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
    End Sub
    Protected Sub txtDoneOnValueInterval_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Count - 1
            Dim calDoneOn As TextBox = CType(Me.dgIntervalValues.Rows(i).FindControl("txtDoneOnValueInterval"), TextBox)
            With mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(calDoneOn.Text) Then
                        .Item(i).DoneOnValueFormatted = ""
                    Else
                        .Item(i).DoneOnValueFormatted = Trim(calDoneOn.Text)
                    End If
                Else
                    .Item(i).DoneOnValue = Trim(calDoneOn.Text)
                End If
            End With
        Next i
        dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
        dgIntervalValues.DataBind()
        upnlIntervalValues.Update()
        Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
    End Sub
    Protected Sub txtDueOnValueInterval_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For i As Integer = 0 To mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Count - 1
            Dim txtDueOnValue As TextBox = CType(Me.dgIntervalValues.Rows(i).FindControl("txtDueOnValueInterval"), TextBox)
            With mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(txtDueOnValue.Text) Then
                        .Item(i).DueOnValueFormatted = ""
                    Else
                        .Item(i).DueOnValueFormatted = Trim(txtDueOnValue.Text)
                    End If
                Else
                    .Item(i).DueOnValue = Trim(txtDueOnValue.Text)
                End If
            End With
        Next i

        dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
        dgIntervalValues.DataBind()
        upnlIntervalValues.Update()
        Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
    End Sub
    Protected Sub txtExtensionValueInterval_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgIntervalValues.Rows(i).FindControl("txtExtensionValueInterval"), TextBox)

            With mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
        dgIntervalValues.DataBind()
        upnlIntervalValues.Update()
        Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If IsValid Then
            If chkIsApplicable.Checked = False And txtNote.Text = "" Then
                MSGBoxCtrl.Show("Alert..!!", "Please enter Note as this MPD is not Applicable to this Model/Aircraft.", "", MsgBoxStyle.OkOnly, "App")
                Exit Sub
            End If
            If chkIsApplicable.Checked = True And Not CustomValidate2() = True Then upnlValidationSummary.Update() : Exit Sub

            If chkIsApplicable.Checked = False And Not CustomValidate3() = True Then upnlValidationSummary.Update() : Exit Sub

            If Save() = True Then
                ' ControlVisibility()
                'SetCaption()
                ' UpdatePanel()
                pnlThreshold.Enabled = False
                pnlInterval.Enabled = False

                upnlThreshold.Update()
                upnlInterval.Update()
                mMPDMaster = MPDMaster.GetMPDMaster(mMPDMaster.ID)
                Session("mMPDMaster") = mMPDMaster
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub

    Private Sub chkIsComplianceThreshold_CheckedChanged(sender As Object, e As EventArgs) Handles rdbIsComplianceThresholdYes.CheckedChanged, rdbIsComplianceThresholdNo.CheckedChanged
        If rdbIsComplianceThresholdYes.Checked Then
            phThresholdDoneDetails.Visible = True
        Else
            phThresholdDoneDetails.Visible = False
        End If
        upnlMonitoringStatusDetailsThreshold.Update()
    End Sub

    Private Sub chkIsComplianceInterval_CheckedChanged(sender As Object, e As EventArgs) Handles rdbIsComplianceIntervalYes.CheckedChanged, rdbIsComplianceIntervalNo.CheckedChanged
        If rdbIsComplianceIntervalYes.Checked Then
            phIntervalDoneDetails.Visible = True
            phNAStart.Visible = False
        Else
            phIntervalDoneDetails.Visible = False
            phNAStart.Visible = True
        End If
        upnlLinkActivity.Update()
        upnlMonitoringStatusDetailsInterval.Update()
    End Sub

    Private Sub dgPeriodsThreshold_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgPeriodsThreshold.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgPeriodsThreshold.PageIndex * dgPeriodsThreshold.PageSize
                If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
                    If mAssemblyStatus.IsMaster Then 'Added By Utkarsh On 15-Mar-2011
                        If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                            mUnit = mModelMonitorServiceThreshold.ModelMonitorServicePeriods(Index).PeriodUnitName
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If
                    ElseIf Not mAssemblyStatus.IsMaster Then
                        If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If
                    End If '*******************************
                End If

                Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList

                If chkIsThreshold.Checked Then
                    mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorServiceThreshold.ModelID, mModelMonitorServiceThreshold.ID.ToString)

                    If mModelMonitorConfiguredList.Count > 0 Then
                        Dim SerialNos As String = String.Empty

                        For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                            If i = mModelMonitorConfiguredList.Count - 1 Then
                                SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                            Else
                                SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                            End If
                        Next

                        MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                        Exit Select
                    End If

                    mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Remove(mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Item(Index).ID, "")
                    Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
                    dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
                    dgThresholdValues.DataBind()
                    upnlThresholdValues.Update()

                    mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Remove(mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Item(Index).ID)
                    Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold
                    dgPeriodsThreshold.DataSource = mModelMonitorServiceThreshold.ModelMonitorServicePeriods
                    dgPeriodsThreshold.DataBind()
                    upnlPeriodsThreshold.Update()



                    SetColorThreshold()
                End If

                'Interval

                If chkIsInterval.Checked Then


                    mModelMonitorConfiguredList = Nothing
                    mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorServiceInterval.ModelID, mModelMonitorServiceInterval.ID.ToString)

                    If mModelMonitorConfiguredList.Count > 0 Then
                        Dim SerialNos As String = String.Empty

                        For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                            If i = mModelMonitorConfiguredList.Count - 1 Then
                                SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                            Else
                                SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                            End If
                        Next

                        MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorServiceInterval.ModelMonitorServicePeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                        Exit Select
                    End If

                    mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Remove(mModelMonitorServiceInterval.ModelMonitorServicePeriods.Item(Index).ID, "")
                    Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
                    dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                    dgIntervalValues.DataBind()
                    upnlIntervalValues.Update()

                    mModelMonitorServiceInterval.ModelMonitorServicePeriods.Remove(mModelMonitorServiceInterval.ModelMonitorServicePeriods.Item(Index).ID)
                    Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval
                    dgPeriodsInterval.DataSource = mModelMonitorServiceInterval.ModelMonitorServicePeriods
                    dgPeriodsInterval.DataBind()
                    upnlPeriodsInterval.Update()
                End If

        End Select
    End Sub

    Private Sub dgPeriodsInterval_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgPeriodsInterval.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim Index As Int32 = CInt(e.CommandArgument) + dgPeriodsInterval.PageIndex * dgPeriodsInterval.PageSize
                If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
                    If mAssemblyStatus.IsMaster Then 'Added By Utkarsh On 15-Mar-2011
                        If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                            mUnit = mModelMonitorServiceInterval.ModelMonitorServicePeriods(Index).PeriodUnitName
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If
                    ElseIf Not mAssemblyStatus.IsMaster Then
                        If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
                            Exit Sub
                        End If
                    End If '*******************************
                End If
                Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList

                If chkIsThreshold.Checked Then



                    mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorServiceThreshold.ModelID, mModelMonitorServiceThreshold.ID.ToString)

                    If mModelMonitorConfiguredList.Count > 0 Then
                        Dim SerialNos As String = String.Empty

                        For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                            If i = mModelMonitorConfiguredList.Count - 1 Then
                                SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                            Else
                                SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                            End If
                        Next

                        MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                        Exit Select
                    End If
                    mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Remove(mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Item(Index).ID, "")
                    Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
                    dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
                    dgThresholdValues.DataBind()
                    upnlThresholdValues.Update()
                    SetColorThreshold()

                    mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Remove(mModelMonitorServiceThreshold.ModelMonitorServicePeriods.Item(Index).ID)
                    Session("mModelMonitorServiceThreshold") = mModelMonitorServiceThreshold
                    dgPeriodsThreshold.DataSource = mModelMonitorServiceThreshold.ModelMonitorServicePeriods
                    dgPeriodsThreshold.DataBind()
                    upnlPeriodsThreshold.Update()


                End If

                'Interval


                If chkIsInterval.Checked Then


                    mModelMonitorConfiguredList = Nothing
                    mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorServiceConfiguredList(mModelMonitorServiceInterval.ModelID, mModelMonitorServiceInterval.ID.ToString)

                    If mModelMonitorConfiguredList.Count > 0 Then
                        Dim SerialNos As String = String.Empty

                        For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
                            If i = mModelMonitorConfiguredList.Count - 1 Then
                                SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
                            Else
                                SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
                            End If
                        Next

                        MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorServiceInterval.ModelMonitorServicePeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
                        Exit Select
                    End If

                    mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Remove(mModelMonitorServiceInterval.ModelMonitorServicePeriods.Item(Index).ID, "")
                    Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
                    dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
                    dgIntervalValues.DataBind()
                    upnlIntervalValues.Update()

                    mModelMonitorServiceInterval.ModelMonitorServicePeriods.Remove(mModelMonitorServiceInterval.ModelMonitorServicePeriods.Item(Index).ID)
                    Session("mModelMonitorServiceInterval") = mModelMonitorServiceInterval
                    dgPeriodsInterval.DataSource = mModelMonitorServiceInterval.ModelMonitorServicePeriods
                    dgPeriodsInterval.DataBind()
                    upnlPeriodsInterval.Update()


                End If
        End Select
    End Sub

    Private Sub txtDoneOnDateThreshold_TextChanged(sender As Object, e As EventArgs) Handles txtDoneOnDateThreshold.TextChanged
        If IsPostBack Then
            SetObjectThreshold()
            Dim mAssemblyMonitorServiceStatusThresholdClone As AssemblyMonitorServiceStatus = mAssemblyMonitorServiceStatusThreshold.Clone

            If Session("FromEditThresholdInterval") = "False" Then
                mAssemblyMonitorServiceStatusThreshold = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyMonitorServiceStatusThreshold.AssemblyID, mAssemblyMonitorServiceStatusThreshold.AssemblyStatusID, txtDoneOnDateThreshold.Text.ToString, mModelMonitorServiceThreshold.ModelID, mMachine.HourType)
                For Each tmpAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod In mAssemblyMonitorServiceStatusThresholdClone.AssemblyMonitorServiceStatusPeriods
                    tmpAssemblyMonitorServiceStatusPeriod.DoneOnValue = tmpAssemblyMonitorServiceStatusPeriod.CurrentValue
                    mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Add(tmpAssemblyMonitorServiceStatusPeriod)
                Next

            Else
                Dim mtmpAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mAssemblyMonitorServiceStatusThreshold.ID, mAssemblyMonitorServiceStatusThreshold.AssemblyStatusID, txtDoneOnDateThreshold.Text.ToString, mMachine.HourType)
                For Each tmpAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod In mtmpAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                    mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods.Item(tmpAssemblyMonitorServiceStatusPeriod.PeriodID, tmpAssemblyMonitorServiceStatusPeriod.PeriodUnitID).DoneOnValue = tmpAssemblyMonitorServiceStatusPeriod.CurrentValue
                Next
            End If

            Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold


            SetThresholdStatusObject()

            dgThresholdValues.DataSource = mAssemblyMonitorServiceStatusThreshold.AssemblyMonitorServiceStatusPeriods
            dgThresholdValues.DataBind()
            SetColorThreshold()
            upnlRedLabel.Update()
            'upnlElapsedRemainingValues.Update()
            upnlThresholdValues.Update()
            Session("mAssemblyMonitorServiceStatusThreshold") = mAssemblyMonitorServiceStatusThreshold
        End If
    End Sub

    Private Sub chkIsApplicable_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsApplicable.CheckedChanged
        If chkIsApplicable.Checked Then
            phCompliance.Visible = True
            phLine.Visible = True
        Else
            phCompliance.Visible = False
            phLine.Visible = False
        End If
    End Sub

    Private Sub txtDoneOnDateInterval_TextChanged(sender As Object, e As EventArgs) Handles txtDoneOnDateInterval.TextChanged
        If IsPostBack Then
            SetObjectInterval()
            Dim mAssemblyMonitorServiceStatusIntervalClone As AssemblyMonitorServiceStatus = mAssemblyMonitorServiceStatusInterval.Clone
            If Session("FromEditThresholdInterval") = "False" Then
                mAssemblyMonitorServiceStatusInterval = AssemblyMonitorServiceStatus.NewAssemblyMonitorServiceStatus(Guid.NewGuid, mAssemblyMonitorServiceStatusInterval.AssemblyID, mAssemblyMonitorServiceStatusInterval.AssemblyStatusID, txtDoneOnDateInterval.Text.ToString, mModelMonitorServiceInterval.ModelID, mMachine.HourType)

                For Each tmpAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod In mAssemblyMonitorServiceStatusIntervalClone.AssemblyMonitorServiceStatusPeriods
                    tmpAssemblyMonitorServiceStatusPeriod.DoneOnValue = tmpAssemblyMonitorServiceStatusPeriod.CurrentValue
                    mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Add(tmpAssemblyMonitorServiceStatusPeriod)
                Next
            Else
                Dim mtmpAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusFromEntry(mAssemblyMonitorServiceStatusInterval.ID, mAssemblyMonitorServiceStatusInterval.AssemblyStatusID, txtDoneOnDateInterval.Text.ToString, mMachine.HourType)
                For Each tmpAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod In mtmpAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
                    mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods.Item(tmpAssemblyMonitorServiceStatusPeriod.PeriodID, tmpAssemblyMonitorServiceStatusPeriod.PeriodUnitID).DoneOnValue = tmpAssemblyMonitorServiceStatusPeriod.CurrentValue
                Next
            End If



            Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval


            SetIntervalStatusObject()
            dgIntervalValues.DataSource = mAssemblyMonitorServiceStatusInterval.AssemblyMonitorServiceStatusPeriods
            dgIntervalValues.DataBind()
            upnlIntervalValues.Update()
            Session("mAssemblyMonitorServiceStatusInterval") = mAssemblyMonitorServiceStatusInterval
        End If
    End Sub

    Private Sub chkIsInterval_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsInterval.CheckedChanged
        If chkIsInterval.Checked Then
            pnlLinkActivity.Enabled = True
            AddPeriodUnitsInterval()
        Else
            pnlLinkActivity.Enabled = False
        End If
        upnlLinkActivity.Update()
    End Sub
#End Region

End Class